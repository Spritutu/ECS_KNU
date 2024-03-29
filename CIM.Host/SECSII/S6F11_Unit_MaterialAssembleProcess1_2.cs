﻿using CIM.Common;
using CIM.Manager;
using SDC.Core;
using SDC.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYSWIN.Secl;

namespace CIM.Host.Swin.SECSII
{
    class S6F11_Unit_MaterialAssembleProcess1_2 : SFMessage
    {
        /*S6F11(Material Process Change), CEID: 911, 912, 913, 914, 915, 916, 917, 918, 919, 920, 921, 922, 923, 924, 925 */
        // CEID : 915
        public S6F11_Unit_MaterialAssembleProcess1_2(SECSDriver driver)
            : base(driver)
        {

        }

        public override void DoWork(string driverName, object obj)
        {
            int stream = 6, function = 11;
            bool bResult;

            #region Varialbe
            string sDataID = "0";
            string sCRST = ((int)DataManager.Instance.GET_INT_DATA("vSys_HostData_Mode", out bResult)).ToString();
            string sEQPID = CommonData.Instance.EQP_SETTINGS.EQPID;
            string sCEID = "915";
            string sUNITID = string.Empty; // 어디서 가져올지 정해지면 코딩

            string sCELLID = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_CellID", out bResult);
            string sPPID = RMSManager.Instance.GetCurrentPPID();
            string sPRODUCTID = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_ProductID", out bResult);
            string sSTEPID = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_StepID", out bResult);

            int nUnit_List = DataManager.Instance.GET_INT_DATA("vSys_UnitInfos_Count", out bResult); // m_lnkHostData.UnitNo;
            string[] aUNITID = new string[nUnit_List];

            int nUnit1MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC1_MaterialCount", out bResult);
            int nUnit2MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC2_MaterialCount", out bResult);
            int nUnit3MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC3_MaterialCount", out bResult);
            int nUnit4MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC4_MaterialCount", out bResult);
            int nUnit5MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC5_MaterialCount", out bResult);
            int nUnit6MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC6_MaterialCount", out bResult);
            int nUnit7MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC7_MaterialCount", out bResult);
            int nUnit8MaterialList = DataManager.Instance.GET_INT_DATA("vSys_PLC8_MaterialCount", out bResult);

            string[] Unit1_MAT_BAT_ID = new string[nUnit1MaterialList];
            string[] Unit1_MAT_BAT_NAME = new string[nUnit1MaterialList];
            string[] Unit1_MAT_ID = new string[nUnit1MaterialList];
            string[] Unit1_MAT_TYPE = new string[nUnit1MaterialList];
            string[] Unit1_MAT_ST = new string[nUnit1MaterialList];
            string[] Unit1_MAT_PORT_ID = new string[nUnit1MaterialList];
            string[] Unit1_MAT_STATE = new string[nUnit1MaterialList];
            string[] Unit1_MAT_TOTAL_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_USE_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_ASSEM_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_NG_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_MAINQTY_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_PRODUCT_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_PROCE_USE_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_PROCE_ASSEM_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_PROC_NG_QTY = new string[nUnit1MaterialList];
            string[] Unit1_MAT_SUPPLY_QUEST_QTY = new string[nUnit1MaterialList];
            string[] Unit1_aTemp = new string[nUnit1MaterialList];
            for (int i = 0; i < nUnit1MaterialList; i++)
            {
                Unit1_MAT_BAT_ID[i] = "";
                Unit1_MAT_BAT_NAME[i] = "";
                Unit1_MAT_ID[i] = "";
                Unit1_MAT_TYPE[i] = "";
                Unit1_MAT_ST[i] = "";
                Unit1_MAT_PORT_ID[i] = "";
                Unit1_MAT_STATE[i] = "";
                Unit1_MAT_TOTAL_QTY[i] = "";
                Unit1_MAT_USE_QTY[i] = "";
                Unit1_MAT_ASSEM_QTY[i] = "";
                Unit1_MAT_NG_QTY[i] = "";
                Unit1_MAT_MAINQTY_QTY[i] = "";
                Unit1_MAT_PRODUCT_QTY[i] = "";
                Unit1_MAT_PROCE_USE_QTY[i] = "";
                Unit1_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit1_MAT_PROC_NG_QTY[i] = "";
                Unit1_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit2_MAT_BAT_ID = new string[nUnit2MaterialList];
            string[] Unit2_MAT_BAT_NAME = new string[nUnit2MaterialList];
            string[] Unit2_MAT_ID = new string[nUnit2MaterialList];
            string[] Unit2_MAT_TYPE = new string[nUnit2MaterialList];
            string[] Unit2_MAT_ST = new string[nUnit2MaterialList];
            string[] Unit2_MAT_PORT_ID = new string[nUnit2MaterialList];
            string[] Unit2_MAT_STATE = new string[nUnit2MaterialList];
            string[] Unit2_MAT_TOTAL_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_USE_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_ASSEM_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_NG_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_MAINQTY_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_PRODUCT_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_PROCE_USE_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_PROCE_ASSEM_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_PROC_NG_QTY = new string[nUnit2MaterialList];
            string[] Unit2_MAT_SUPPLY_QUEST_QTY = new string[nUnit2MaterialList];
            string[] Unit2_aTemp = new string[nUnit2MaterialList]; 
            for (int i = 0; i < nUnit2MaterialList; i++)
            {
                Unit2_MAT_BAT_ID[i] = "";
                Unit2_MAT_BAT_NAME[i] = "";
                Unit2_MAT_ID[i] = "";
                Unit2_MAT_TYPE[i] = "";
                Unit2_MAT_ST[i] = "";
                Unit2_MAT_PORT_ID[i] = "";
                Unit2_MAT_STATE[i] = "";
                Unit2_MAT_TOTAL_QTY[i] = "";
                Unit2_MAT_USE_QTY[i] = "";
                Unit2_MAT_ASSEM_QTY[i] = "";
                Unit2_MAT_NG_QTY[i] = "";
                Unit2_MAT_MAINQTY_QTY[i] = "";
                Unit2_MAT_PRODUCT_QTY[i] = "";
                Unit2_MAT_PROCE_USE_QTY[i] = "";
                Unit2_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit2_MAT_PROC_NG_QTY[i] = "";
                Unit2_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit3_MAT_BAT_ID = new string[nUnit3MaterialList];
            string[] Unit3_MAT_BAT_NAME = new string[nUnit3MaterialList];
            string[] Unit3_MAT_ID = new string[nUnit3MaterialList];
            string[] Unit3_MAT_TYPE = new string[nUnit3MaterialList];
            string[] Unit3_MAT_ST = new string[nUnit3MaterialList];
            string[] Unit3_MAT_PORT_ID = new string[nUnit3MaterialList];
            string[] Unit3_MAT_STATE = new string[nUnit3MaterialList];
            string[] Unit3_MAT_TOTAL_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_USE_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_ASSEM_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_NG_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_MAINQTY_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_PRODUCT_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_PROCE_USE_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_PROCE_ASSEM_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_PROC_NG_QTY = new string[nUnit3MaterialList];
            string[] Unit3_MAT_SUPPLY_QUEST_QTY = new string[nUnit3MaterialList];
            string[] Unit3_aTemp = new string[nUnit3MaterialList]; 
            for (int i = 0; i < nUnit3MaterialList; i++)
            {
                Unit3_MAT_BAT_ID[i] = "";
                Unit3_MAT_BAT_NAME[i] = "";
                Unit3_MAT_ID[i] = "";
                Unit3_MAT_TYPE[i] = "";
                Unit3_MAT_ST[i] = "";
                Unit3_MAT_PORT_ID[i] = "";
                Unit3_MAT_STATE[i] = "";
                Unit3_MAT_TOTAL_QTY[i] = "";
                Unit3_MAT_USE_QTY[i] = "";
                Unit3_MAT_ASSEM_QTY[i] = "";
                Unit3_MAT_NG_QTY[i] = "";
                Unit3_MAT_MAINQTY_QTY[i] = "";
                Unit3_MAT_PRODUCT_QTY[i] = "";
                Unit3_MAT_PROCE_USE_QTY[i] = "";
                Unit3_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit3_MAT_PROC_NG_QTY[i] = "";
                Unit3_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit4_MAT_BAT_ID = new string[nUnit4MaterialList];
            string[] Unit4_MAT_BAT_NAME = new string[nUnit4MaterialList];
            string[] Unit4_MAT_ID = new string[nUnit4MaterialList];
            string[] Unit4_MAT_TYPE = new string[nUnit4MaterialList];
            string[] Unit4_MAT_ST = new string[nUnit4MaterialList];
            string[] Unit4_MAT_PORT_ID = new string[nUnit4MaterialList];
            string[] Unit4_MAT_STATE = new string[nUnit4MaterialList];
            string[] Unit4_MAT_TOTAL_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_USE_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_ASSEM_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_NG_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_MAINQTY_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_PRODUCT_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_PROCE_USE_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_PROCE_ASSEM_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_PROC_NG_QTY = new string[nUnit4MaterialList];
            string[] Unit4_MAT_SUPPLY_QUEST_QTY = new string[nUnit4MaterialList];
            string[] Unit4_aTemp = new string[nUnit4MaterialList]; 
            for (int i = 0; i < nUnit4MaterialList; i++)
            {
                Unit4_MAT_BAT_ID[i] = "";
                Unit4_MAT_BAT_NAME[i] = "";
                Unit4_MAT_ID[i] = "";
                Unit4_MAT_TYPE[i] = "";
                Unit4_MAT_ST[i] = "";
                Unit4_MAT_PORT_ID[i] = "";
                Unit4_MAT_STATE[i] = "";
                Unit4_MAT_TOTAL_QTY[i] = "";
                Unit4_MAT_USE_QTY[i] = "";
                Unit4_MAT_ASSEM_QTY[i] = "";
                Unit4_MAT_NG_QTY[i] = "";
                Unit4_MAT_MAINQTY_QTY[i] = "";
                Unit4_MAT_PRODUCT_QTY[i] = "";
                Unit4_MAT_PROCE_USE_QTY[i] = "";
                Unit4_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit4_MAT_PROC_NG_QTY[i] = "";
                Unit4_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit5_MAT_BAT_ID = new string[nUnit5MaterialList];
            string[] Unit5_MAT_BAT_NAME = new string[nUnit5MaterialList];
            string[] Unit5_MAT_ID = new string[nUnit5MaterialList];
            string[] Unit5_MAT_TYPE = new string[nUnit5MaterialList];
            string[] Unit5_MAT_ST = new string[nUnit5MaterialList];
            string[] Unit5_MAT_PORT_ID = new string[nUnit5MaterialList];
            string[] Unit5_MAT_STATE = new string[nUnit5MaterialList];
            string[] Unit5_MAT_TOTAL_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_USE_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_ASSEM_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_NG_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_MAINQTY_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_PRODUCT_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_PROCE_USE_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_PROCE_ASSEM_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_PROC_NG_QTY = new string[nUnit5MaterialList];
            string[] Unit5_MAT_SUPPLY_QUEST_QTY = new string[nUnit5MaterialList];
            string[] Unit5_aTemp = new string[nUnit5MaterialList]; 
            for (int i = 0; i < nUnit5MaterialList; i++)
            {
                Unit5_MAT_BAT_ID[i] = "";
                Unit5_MAT_BAT_NAME[i] = "";
                Unit5_MAT_ID[i] = "";
                Unit5_MAT_TYPE[i] = "";
                Unit5_MAT_ST[i] = "";
                Unit5_MAT_PORT_ID[i] = "";
                Unit5_MAT_STATE[i] = "";
                Unit5_MAT_TOTAL_QTY[i] = "";
                Unit5_MAT_USE_QTY[i] = "";
                Unit5_MAT_ASSEM_QTY[i] = "";
                Unit5_MAT_NG_QTY[i] = "";
                Unit5_MAT_MAINQTY_QTY[i] = "";
                Unit5_MAT_PRODUCT_QTY[i] = "";
                Unit5_MAT_PROCE_USE_QTY[i] = "";
                Unit5_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit5_MAT_PROC_NG_QTY[i] = "";
                Unit5_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit6_MAT_BAT_ID = new string[nUnit6MaterialList];
            string[] Unit6_MAT_BAT_NAME = new string[nUnit6MaterialList];
            string[] Unit6_MAT_ID = new string[nUnit6MaterialList];
            string[] Unit6_MAT_TYPE = new string[nUnit6MaterialList];
            string[] Unit6_MAT_ST = new string[nUnit6MaterialList];
            string[] Unit6_MAT_PORT_ID = new string[nUnit6MaterialList];
            string[] Unit6_MAT_STATE = new string[nUnit6MaterialList];
            string[] Unit6_MAT_TOTAL_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_USE_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_ASSEM_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_NG_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_MAINQTY_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_PRODUCT_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_PROCE_USE_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_PROCE_ASSEM_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_PROC_NG_QTY = new string[nUnit6MaterialList];
            string[] Unit6_MAT_SUPPLY_QUEST_QTY = new string[nUnit6MaterialList];
            string[] Unit6_aTemp = new string[nUnit6MaterialList]; 
            for (int i = 0; i < nUnit6MaterialList; i++)
            {
                Unit6_MAT_BAT_ID[i] = "";
                Unit6_MAT_BAT_NAME[i] = "";
                Unit6_MAT_ID[i] = "";
                Unit6_MAT_TYPE[i] = "";
                Unit6_MAT_ST[i] = "";
                Unit6_MAT_PORT_ID[i] = "";
                Unit6_MAT_STATE[i] = "";
                Unit6_MAT_TOTAL_QTY[i] = "";
                Unit6_MAT_USE_QTY[i] = "";
                Unit6_MAT_ASSEM_QTY[i] = "";
                Unit6_MAT_NG_QTY[i] = "";
                Unit6_MAT_MAINQTY_QTY[i] = "";
                Unit6_MAT_PRODUCT_QTY[i] = "";
                Unit6_MAT_PROCE_USE_QTY[i] = "";
                Unit6_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit6_MAT_PROC_NG_QTY[i] = "";
                Unit6_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit7_MAT_BAT_ID = new string[nUnit7MaterialList];
            string[] Unit7_MAT_BAT_NAME = new string[nUnit7MaterialList];
            string[] Unit7_MAT_ID = new string[nUnit7MaterialList];
            string[] Unit7_MAT_TYPE = new string[nUnit7MaterialList];
            string[] Unit7_MAT_ST = new string[nUnit7MaterialList];
            string[] Unit7_MAT_PORT_ID = new string[nUnit7MaterialList];
            string[] Unit7_MAT_STATE = new string[nUnit7MaterialList];
            string[] Unit7_MAT_TOTAL_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_USE_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_ASSEM_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_NG_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_MAINQTY_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_PRODUCT_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_PROCE_USE_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_PROCE_ASSEM_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_PROC_NG_QTY = new string[nUnit7MaterialList];
            string[] Unit7_MAT_SUPPLY_QUEST_QTY = new string[nUnit7MaterialList];
            string[] Unit7_aTemp = new string[nUnit7MaterialList]; 
            for (int i = 0; i < nUnit7MaterialList; i++)
            {
                Unit7_MAT_BAT_ID[i] = "";
                Unit7_MAT_BAT_NAME[i] = "";
                Unit7_MAT_ID[i] = "";
                Unit7_MAT_TYPE[i] = "";
                Unit7_MAT_ST[i] = "";
                Unit7_MAT_PORT_ID[i] = "";
                Unit7_MAT_STATE[i] = "";
                Unit7_MAT_TOTAL_QTY[i] = "";
                Unit7_MAT_USE_QTY[i] = "";
                Unit7_MAT_ASSEM_QTY[i] = "";
                Unit7_MAT_NG_QTY[i] = "";
                Unit7_MAT_MAINQTY_QTY[i] = "";
                Unit7_MAT_PRODUCT_QTY[i] = "";
                Unit7_MAT_PROCE_USE_QTY[i] = "";
                Unit7_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit7_MAT_PROC_NG_QTY[i] = "";
                Unit7_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            string[] Unit8_MAT_BAT_ID = new string[nUnit8MaterialList];
            string[] Unit8_MAT_BAT_NAME = new string[nUnit8MaterialList];
            string[] Unit8_MAT_ID = new string[nUnit8MaterialList];
            string[] Unit8_MAT_TYPE = new string[nUnit8MaterialList];
            string[] Unit8_MAT_ST = new string[nUnit8MaterialList];
            string[] Unit8_MAT_PORT_ID = new string[nUnit8MaterialList];
            string[] Unit8_MAT_STATE = new string[nUnit8MaterialList];
            string[] Unit8_MAT_TOTAL_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_USE_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_ASSEM_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_NG_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_MAINQTY_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_PRODUCT_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_PROCE_USE_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_PROCE_ASSEM_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_PROC_NG_QTY = new string[nUnit8MaterialList];
            string[] Unit8_MAT_SUPPLY_QUEST_QTY = new string[nUnit8MaterialList];
            string[] Unit8_aTemp = new string[nUnit8MaterialList]; 
            for (int i = 0; i < nUnit8MaterialList; i++)
            {
                Unit8_MAT_BAT_ID[i] = "";
                Unit8_MAT_BAT_NAME[i] = "";
                Unit8_MAT_ID[i] = "";
                Unit8_MAT_TYPE[i] = "";
                Unit8_MAT_ST[i] = "";
                Unit8_MAT_PORT_ID[i] = "";
                Unit8_MAT_STATE[i] = "";
                Unit8_MAT_TOTAL_QTY[i] = "";
                Unit8_MAT_USE_QTY[i] = "";
                Unit8_MAT_ASSEM_QTY[i] = "";
                Unit8_MAT_NG_QTY[i] = "";
                Unit8_MAT_MAINQTY_QTY[i] = "";
                Unit8_MAT_PRODUCT_QTY[i] = "";
                Unit8_MAT_PROCE_USE_QTY[i] = "";
                Unit8_MAT_PROCE_ASSEM_QTY[i] = "";
                Unit8_MAT_PROC_NG_QTY[i] = "";
                Unit8_MAT_SUPPLY_QUEST_QTY[i] = "";
            }

            #endregion

            #region Define
            Unit1_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_BatchID", out bResult);
            Unit1_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_BatchName", out bResult);
            Unit1_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_ID", out bResult);
            Unit1_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_Type", out bResult);
            Unit1_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_ST", out bResult);
            Unit1_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_PortID", out bResult);
            Unit1_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC1_EtoC_Assem1_2_State", out bResult);
            Unit1_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit1_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit1_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit1_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit1_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit1_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit1_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit1_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit1_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit1_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC1_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit2_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_BatchID", out bResult);
            Unit2_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_BatchName", out bResult);
            Unit2_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_ID", out bResult);
            Unit2_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_Type", out bResult);
            Unit2_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_ST", out bResult);
            Unit2_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_PortID", out bResult);
            Unit2_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC2_EtoC_Assem1_2_State", out bResult);
            Unit2_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit2_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit2_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit2_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit2_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit2_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit2_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit2_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit2_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit2_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC2_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit3_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_BatchID", out bResult);
            Unit3_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_BatchName", out bResult);
            Unit3_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_ID", out bResult);
            Unit3_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_Type", out bResult);
            Unit3_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_ST", out bResult);
            Unit3_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_PortID", out bResult);
            Unit3_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC3_EtoC_Assem1_2_State", out bResult);
            Unit3_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit3_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit3_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit3_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit3_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit3_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit3_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit3_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit3_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit3_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC3_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit4_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_BatchID", out bResult);
            Unit4_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_BatchName", out bResult);
            Unit4_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_ID", out bResult);
            Unit4_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_Type", out bResult);
            Unit4_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_ST", out bResult);
            Unit4_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_PortID", out bResult);
            Unit4_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC4_EtoC_Assem1_2_State", out bResult);
            Unit4_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit4_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit4_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit4_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit4_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit4_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit4_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit4_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit4_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit4_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC4_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit5_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_BatchID", out bResult);
            Unit5_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_BatchName", out bResult);
            Unit5_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_ID", out bResult);
            Unit5_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_Type", out bResult);
            Unit5_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_ST", out bResult);
            Unit5_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_PortID", out bResult);
            Unit5_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC5_EtoC_Assem1_2_State", out bResult);
            Unit5_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit5_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit5_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit5_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit5_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit5_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit5_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit5_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit5_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit5_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC5_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit6_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_BatchID", out bResult);
            Unit6_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_BatchName", out bResult);
            Unit6_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_ID", out bResult);
            Unit6_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_Type", out bResult);
            Unit6_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_ST", out bResult);
            Unit6_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_PortID", out bResult);
            Unit6_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC6_EtoC_Assem1_2_State", out bResult);
            Unit6_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit6_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit6_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit6_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit6_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit6_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit6_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit6_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit6_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit6_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC6_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit7_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_BatchID", out bResult);
            Unit7_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_BatchName", out bResult);
            Unit7_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_ID", out bResult);
            Unit7_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_Type", out bResult);
            Unit7_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_ST", out bResult);
            Unit7_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_PortID", out bResult);
            Unit7_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC7_EtoC_Assem1_2_State", out bResult);
            Unit7_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit7_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit7_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit7_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit7_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit7_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit7_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit7_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit7_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit7_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC7_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();

            Unit8_MAT_BAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_BatchID", out bResult);
            Unit8_MAT_BAT_NAME[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_BatchName", out bResult);
            Unit8_MAT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_ID", out bResult);
            Unit8_MAT_TYPE[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_Type", out bResult);
            Unit8_MAT_ST[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_ST", out bResult);
            Unit8_MAT_PORT_ID[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_PortID", out bResult);
            Unit8_MAT_STATE[0] = DataManager.Instance.GET_STRING_DATA("iPLC8_EtoC_Assem1_2_State", out bResult);
            Unit8_MAT_TOTAL_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_TotalQty", out bResult).ToString();
            Unit8_MAT_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_UseQty", out bResult).ToString();
            Unit8_MAT_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_AssemQty", out bResult).ToString();
            Unit8_MAT_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_NGQty", out bResult).ToString();
            Unit8_MAT_MAINQTY_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_RemainQty", out bResult).ToString();
            Unit8_MAT_PRODUCT_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_ProductQty", out bResult).ToString();
            Unit8_MAT_PROCE_USE_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_ProcessUseQty", out bResult).ToString();
            Unit8_MAT_PROCE_ASSEM_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_ProcessAssemQty", out bResult).ToString();
            Unit8_MAT_PROC_NG_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_ProcessNGQty", out bResult).ToString();
            Unit8_MAT_SUPPLY_QUEST_QTY[0] = DataManager.Instance.GET_INT_DATA("iPLC8_EtoC_Assem1_2_SupplyReqQty", out bResult).ToString();
            #endregion


            //m_lnkMachineInfo.JobTabDataTransfer1(Unit1_MAT_BAT_ID, Unit1_MAT_ST, Unit1_MAT_PORT_ID, Unit1_MAT_TOTAL_QTY, Unit1_MAT_MAINQTY_QTY,
            //        Unit1_MAT_USE_QTY, Unit1_MAT_ASSEM_QTY, Unit1_MAT_NG_QTY, Unit1_aTemp, sCELLID);


            if (aUNITID[0] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[0], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit1MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit1MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit1_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit1_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit1_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit1_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit1MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit1_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit1_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit1_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit1_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit1_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit1_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit1_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit1_MAT_TYPE[i];
                //    logMsg.sUSE = Unit1_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit1_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit1_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit1_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit1_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit1_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit1_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit1_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit1_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[1] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[1], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit2MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit2MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit2_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit2_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit2_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit2_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit2MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit2_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit2_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit2_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit2_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit2_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit2_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit2_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit2_MAT_TYPE[i];
                //    logMsg.sUSE = Unit2_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit2_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit2_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit2_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit2_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit2_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit2_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit2_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit2_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[2] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[2], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit3MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit3MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit3_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit3_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit3_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit3_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit3MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit3_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit3_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit3_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit3_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit3_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit3_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit3_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit3_MAT_TYPE[i];
                //    logMsg.sUSE = Unit3_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit3_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit3_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit3_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit3_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit3_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit3_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit3_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit3_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[3] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[3], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit4MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit4MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit4_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit4_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit4_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit4_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit4MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit4_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit4_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit4_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit4_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit4_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit4_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit4_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit4_MAT_TYPE[i];
                //    logMsg.sUSE = Unit4_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit4_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit4_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit4_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit4_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit4_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit4_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit4_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit4_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[4] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[4], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit5MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit5MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit5_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit5_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit5_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit5_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit5MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit5_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit5_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit5_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit5_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit5_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit5_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit5_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit5_MAT_TYPE[i];
                //    logMsg.sUSE = Unit5_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit5_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit5_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit5_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit5_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit5_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit5_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit5_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit5_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[5] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[5], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit6MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit6MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit6_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit6_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit6_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit6_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit6MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit6_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit6_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit6_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit6_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit6_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit6_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit6_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit6_MAT_TYPE[i];
                //    logMsg.sUSE = Unit6_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit6_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit6_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit6_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit6_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit6_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit6_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit6_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit6_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[6] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[6], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit7MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit7MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit7_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit7_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit7_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit7_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit7MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit7_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit7_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit7_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit7_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit7_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit7_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit7_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit7_MAT_TYPE[i];
                //    logMsg.sUSE = Unit7_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit7_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit7_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit7_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit7_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit7_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit7_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit7_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit7_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }

            else if (aUNITID[7] == sUNITID)
            {
                #region S6F11(MaterialProcessChange), CEID: 215

                SecsMessage msg = new SecsMessage(stream, function, true, Convert.ToInt32(SecsDriver.DeviceID));

                msg.AddList(3);                                                                             //L3  EQP Status Event Info
                {
                    msg.AddAscii(AppUtil.ToAscii(sDataID, gDefine.DEF_DATAID_SIZE));                            //A4  Data ID
                    msg.AddAscii(AppUtil.ToAscii(sCEID, gDefine.DEF_CEID_SIZE));                                //A3  Collection Event ID
                    msg.AddList(3);                                                                             //L3  RPTID Set
                    {
                        msg.AddList(2);                                                                             //L2  RPTID 100 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("100", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="100"
                            msg.AddList(2);                                                                             //L2  EQP Control State Set     
                            {
                                msg.AddAscii(AppUtil.ToAscii(sEQPID, gDefine.DEF_EQPID_SIZE));                              //A40 HOST REQ EQPID 
                                msg.AddAscii(sCRST);                                                                        //A1  Online Control State
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 300 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("300", gDefine.DEF_RPTID_SIZE));                               //A3  RPTID="300"
                            msg.AddList(4);                                                                             //L4  Cell Info Set 
                            {
                                msg.AddAscii(AppUtil.ToAscii(sCELLID, gDefine.DEF_CELLID_SIZE));                            //A40 Cell Unique ID
                                msg.AddAscii(AppUtil.ToAscii(sPPID, gDefine.DEF_PPID_SIZE));                                //A40 LOT 지PPID
                                msg.AddAscii(AppUtil.ToAscii(sPRODUCTID, gDefine.DEF_PRODUCTID_SIZE));                      //A40 CELL Product Info
                                msg.AddAscii(AppUtil.ToAscii(sSTEPID, gDefine.DEF_STEPID_SIZE));                            //A40 CELL STEP ID Info
                            }
                        }
                        msg.AddList(2);                                                                             //L2  RPTID 201 Set
                        {
                            msg.AddAscii(AppUtil.ToAscii("201", gDefine.DEF_RPTID_SIZE));                               //A3 RPTID="201"
                            msg.AddList(2);                                                                             //L2  RPTID 300 Set
                            {
                                msg.AddAscii(AppUtil.ToAscii(aUNITID[7], gDefine.DEF_UNITID_SIZE));                              //A40 HOST REQ UNITID 
                                msg.AddList(nUnit8MaterialList);                                                                       //La Material No
                                {
                                    for (int i = 0; i < nUnit8MaterialList; i++)
                                    {
                                        msg.AddList(17);                                                                        //L17 Material Info Set
                                        {
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_BAT_ID[i], 40));                                   //A40 Batch MaterialID 
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_BAT_NAME[i], 40));                                 //A40 Batch Material Code
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_ID[i], 40));                                       //A40 Material ID
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_TYPE[i], 20));                                     //A20 Material Type
                                            msg.AddAscii(Unit8_MAT_ST[i]);                                                            //A1  Material State Info
                                            msg.AddAscii(Unit8_MAT_PORT_ID[i]);                                                       //A1  Material Port ID Info
                                            msg.AddAscii(Unit8_MAT_STATE[i]);                                                         //A1  Material Avilability State Info
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_TOTAL_QTY[i], 10));                                //A10 Material Batch Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_USE_QTY[i], 10));                                  //A10 Material Batch Cumulative Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_ASSEM_QTY[i], 10));                                //A10 Material Batch Cumulative Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_NG_QTY[i], 10));                                   //A10 Material Batch Cumulative NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_MAINQTY_QTY[i], 10));                              //A10 Remaining Material Quantity 
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_PRODUCT_QTY[i], 10));                              //A10 Material Process Standard Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_PROCE_USE_QTY[i], 10));                            //A10 Material Process Usage Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_PROCE_ASSEM_QTY[i], 10));                          //A10 Material Process Attachment Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_PROC_NG_QTY[i], 10));                              //A10 Material Process NG Quantity
                                            msg.AddAscii(AppUtil.ToAscii(Unit8_MAT_SUPPLY_QUEST_QTY[i], 10));                         //A10 Material Supply Quest Quantity
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                this.SecsDriver.Send(msg);
                #endregion
                #region Write Log Message
                //for (int i = 0; i < nUnit8MaterialList; i++)
                //{
                //    MaterialLogData logMsg = new MaterialLogData();

                //    logMsg.sCEID = sCEID;
                //    logMsg.sCELL_ID = sCELLID;
                //    logMsg.sMT_BATCH_ID = Unit8_MAT_BAT_ID[i];
                //    logMsg.sMT_BATCH_NAME = Unit8_MAT_BAT_NAME[i];
                //    logMsg.sMT_ID = Unit8_MAT_ID[i];
                //    logMsg.sMT_PORT = Unit8_MAT_PORT_ID[i];
                //    logMsg.sMT_ST = Unit8_MAT_ST[i];
                //    logMsg.sMT_STATE = Unit8_MAT_STATE[i];
                //    logMsg.sMT_TOTAL = Unit8_MAT_TOTAL_QTY[i];
                //    logMsg.sMT_TYPE = Unit8_MAT_TYPE[i];
                //    logMsg.sUSE = Unit8_MAT_USE_QTY[i];
                //    logMsg.sPRODUCT = Unit8_MAT_PRODUCT_QTY[i];
                //    logMsg.sASSEMBLE = Unit8_MAT_ASSEM_QTY[i];
                //    logMsg.sNG = Unit8_MAT_NG_QTY[i];
                //    logMsg.sP_SUPPLY = Unit8_MAT_SUPPLY_QUEST_QTY[i];
                //    logMsg.sP_USE = Unit8_MAT_PROCE_USE_QTY[i];
                //    logMsg.sP_NG = Unit8_MAT_PROC_NG_QTY[i];
                //    logMsg.sP_ASSEMBLE = Unit8_MAT_PROCE_ASSEM_QTY[i];
                //    logMsg.sREMAIN = Unit8_MAT_MAINQTY_QTY[i];

                //    SetMaterialLog(logMsg);
                //}
                #endregion
            }
        }
    }
}
