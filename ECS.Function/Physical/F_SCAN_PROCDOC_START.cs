using ECS.Common.Helper;
using INNO6.Core.Manager;
using INNO6.IO;
using Scanlab.Sirius;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECS.Function.Physical
{
    public class F_SCAN_PROCDOC_START : AbstractFunction
    {
        private const int NOTBUSY = 0;
        private const int BUSY = 1;
        private const int ERROR = 1;
        private const int NOERROR = 0;
        private const int OK = 1;
        private const int NOTOK = 0;

        private const string ALARM_SCAN_PROCDOC_START_TIMEOUT = "E3015";
        private const string ALARM_SCAN_PROCDOC_START_FAIL = "E3016";


        private const string DO_NAME_SCAN_PROCDOC_START = "oRTC.oScan.Document";
        private const string DO_NAME_SCAN_LAYER_START = "oRTC.oScan.LayerObject";
        private const string VIO_NAME_SCAN_PROCESS_DOCUMENT = "vSet.oScan.Document";
        private const string VIO_NAME_SCAN_PROCESS_DOCFILEPATH = "vSet.sScan.DocFilePath";
        private const string VIO_NAME_SCAN_TEMP_OBJECT = "vSet.oScan.TempObject";

        private const string DI_NAME_SCAN_BUSY = "iRTC.iScan.BusyStatus";
        private const string DI_NAME_SCAN_ERROR = "iRTC.iScan.ErrorStatus";
        private const string DI_NAME_SCAN_POSITIONACK = "iRTC.iScan.PosAckStatus";
        private const string DI_NAME_SCAN_POWER = "iRTC.iScan.PowerStatus";
        private const string DI_NAME_SCAN_TEMP = "iRTC.iScan.TempStatus";

        private const string IO_AXIS_Y_ABS_POSITION = "vSet.dAxisY.AbsPosition";
        private const string IO_AXIS_X_ABS_POSITION = "vSet.dAxisX.AbsPosition";

        private const string F_PROCESS_SCANNERONLY = "F_PROCESS_SCANNERONLY";


        private IDocument _ScanProcessDocument;

        public override bool CanExecute()
        {
            bool check = true;
            IsAbort = false;
            IsProcessing = false;
            string filePath = DataManager.Instance.GET_STRING_DATA(IoNameHelper.V_STR_SET_SCAN_DOCUMENT_FILEPATH, out bool _);

            check &= this.EquipmentStatusCheck();

            if(filePath.EndsWith(".dxf"))
            {
                check &= ProcessDxfLoad(filePath);
            }
            else
            {
                check &= ProcessFileLoad(filePath);
            }
            

            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                return check;
            }
            else
            {
                
                check &= DataManager.Instance.GET_INT_DATA(DI_NAME_SCAN_BUSY, out bool _) == NOTBUSY;
                check &= DataManager.Instance.GET_INT_DATA(DI_NAME_SCAN_ERROR, out bool _) == NOERROR;

                

                //check &= DataManager.Instance.GET_INT_DATA(DI_NAME_SCAN_POSITIONACK, out bool _) == OK;
                //check &= DataManager.Instance.GET_INT_DATA(DI_NAME_SCAN_POWER, out bool _) == OK;
                //check &= DataManager.Instance.GET_INT_DATA(DI_NAME_SCAN_TEMP, out bool _) == OK;

                return check;
            }
        }

        private bool ProcessFileLoad(string filePath)
        {
            string fullPath = Path.GetFullPath(filePath);

            _ScanProcessDocument = DocumentSerializer.OpenSirius(fullPath);

            if (_ScanProcessDocument != null) return true;
            else return false;
        }

        private bool ProcessDxfLoad(string filePath)
        {
            string fullPath = Path.GetFullPath(filePath);

            _ScanProcessDocument = DocumentSerializer.OpenDxf(fullPath);

            if (_ScanProcessDocument != null) return true;
            else return false;
        }

        public override string Execute()
        {
            String result = F_RESULT_SUCCESS;

            foreach (Layer layer in _ScanProcessDocument.Layers)
            {
                if (result != F_RESULT_SUCCESS) break;
                
                switch (layer.MotionType)
                {
                    case MotionType.ScannerOnly:
                        {
                            // 스캔너 가공영역의 분할이 없는 경우
                            var doc = new DocumentDefault("ScannerOnlyDoc");
                            doc.Layers.Add(layer);
                            DataManager.Instance.SET_OBJECT_DATA(VIO_NAME_SCAN_TEMP_OBJECT, doc);
                            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_PROCESS_SCANNERONLY);
                        }
                        break;
                    case MotionType.StageAndScanner:
                        {
                            // 전체 프로세스 영역이 스캔 가공 영역 보다 커서 Stage와 Scanner가 동시에 구동해야 하는 경우 
                            var doc = new DocumentDefault("StagedAndScannerDoc");
                            var newLayer = new Layer("newLayer");

                            foreach(IEntity e in layer)
                            {
                                if(e.Name.StartsWith("Divide"))
                                {
                                    var divide = e as Group;
                                    Vector2 vector2 = divide.Location;

                                    result = StageABSMove(vector2);

                                    if (result != F_RESULT_SUCCESS)
                                    {
                                        return result;
                                    }
                                    else
                                    {
                                        newLayer.Add(e);
                                        continue;
                                    }
                                }  
                            }

                            doc.Layers.Add(newLayer);
                            DataManager.Instance.SET_OBJECT_DATA(VIO_NAME_SCAN_TEMP_OBJECT, doc);
                            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_PROCESS_SCANNERONLY);
                        }
                        break;
                    case MotionType.StageOnly:
                        {
                            // Stage 스텝 이동 후에 스캐너 가공
                            var doc = new DocumentDefault("StagedAndScannerDoc");
                            doc.Layers.Add(layer);
                            DataManager.Instance.SET_OBJECT_DATA(VIO_NAME_SCAN_TEMP_OBJECT, doc);
                            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_PROCESS_SCANNERONLY);
                        }
                        break;
                }
            }

            return result;
        }

        public override void ExecuteWhenSimulate()
        {
            //DataManager.Instance.SET_INT_DATA(DI_NAME_SCAN_BUSY, 1);
        }

        public override void PostExecute()
        {

        }

        private string StageABSMove(Vector2 absPosition)
        {
            DataManager.Instance.SET_DOUBLE_DATA(IO_AXIS_X_ABS_POSITION, absPosition.X);
            DataManager.Instance.SET_DOUBLE_DATA(IO_AXIS_Y_ABS_POSITION, absPosition.Y);

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_MOVE_TO_SETPOS");
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_MOVE_TO_SETPOS");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                Thread.Sleep(10);

                if (IsAbort)
                {
                    return F_RESULT_ABORT;
                }
                else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                {

                    AlarmManager.Instance.SetAlarm(ALARM_SCAN_PROCDOC_START_TIMEOUT);
                    return this.F_RESULT_TIMEOUT;
                }
                else if (!FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST("F_X_AXIS_MOVE_TO_SETPOS") &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST("F_Y_AXIS_MOVE_TO_SETPOS"))
                {
                    return this.F_RESULT_SUCCESS;
                }
                else if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
                {
                    ExecuteWhenSimulate();
                }
                else
                {
                    IsProcessing = true;
                    continue;
                }
            }
        }
    }
}
