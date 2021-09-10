// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Action
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using netDxf;
using netDxf.Collections;
using netDxf.Entities;
using netDxf.Header;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 액션 객체 (사용자의 모든 이벤트를 기록하여 undo/redo 처리하기 위한 행위 객체)
    /// 사용자의 행위를 Undo 하기 위해서는 모든 사용자 명령이 Action 내의 Undo / Redo Stack 에서 관리되어야 한다 !
    /// </summary>
    public sealed class Action
    {
        internal static List<IEntity> clipboard = new List<IEntity>();
        private IDocument owner;
        private List<IEntity> selectedEntityList;
        private readonly Stack<IUndoRedo> undoStack;
        private readonly Stack<IUndoRedo> redoStack;
        /// <summary>새로운 레이어 생성시 증가되는 전역 레이어 번호</summary>
        public int NewLayerIndex;
        private RtcVirtual rtcLaserPath;
        private List<IEntity> entityLaserPath;
        private Action.LaserPathSimSpped simulateSpeed;
        private bool isSimulatorTerminated;
        private Thread threadSimulator;
        private IView targetView;

        /// <summary>선택된 엔티티 변경 통지 이벤트</summary>
        public event Action.EntitySelectedChangedEvent OnEntitySelectedChanged;

        /// <summary>선택된 엔티티 리스트</summary>
        public List<IEntity> SelectedEntity
        {
            get => this.selectedEntityList;
            set
            {
                this.selectedEntityList = value != null ? value : new List<IEntity>();
                Delegate[] invocationList = this.OnEntitySelectedChanged?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (Action.EntitySelectedChangedEvent selectedChangedEvent in invocationList)
                    selectedChangedEvent(this.owner, this.selectedEntityList);
            }
        }

        /// <summary>클립보드 (복사, 잘라내기, 붙혀넣기를 위한 임시 공간)</summary>
        public static List<IEntity> ClipBoard
        {
            get => Action.clipboard;
            set => Action.clipboard = value;
        }

        /// <summary>Undo/Redo 사용 여부</summary>
        internal bool UndoRedoEnable { get; set; }

        /// <summary>constructor (생성자)</summary>
        /// <param name="owner">부모 문서(document)</param>
        internal Action(IDocument owner)
        {
            this.owner = owner;
            this.selectedEntityList = new List<IEntity>();
            this.redoStack = new Stack<IUndoRedo>();
            this.undoStack = new Stack<IUndoRedo>();
            this.UndoRedoEnable = true;
        }

        /// <summary>소멸자</summary>
        ~Action() => this.ActEntityLaserPathSimulateStop();

        /// <summary>Undo/Redo 스택에 명령 삽입 (UndoRedoEnable 으로 삽입 여부 처리가능)</summary>
        /// <param name="ur"></param>
        internal void Insert(IUndoRedo ur)
        {
            if (this.UndoRedoEnable)
                this.undoStack.Push(ur);
            ur.Execute();
            while (this.redoStack.Count > 0)
                this.redoStack.Pop();
            foreach (IView view in this.owner.Views)
                view.Render();
            if (Config.UndoStackSize <= 0 || this.undoStack.Count <= Config.UndoStackSize)
                return;
            this.undoStack.Pop();
        }

        /// <summary>Undo/Redo 스택 정리</summary>
        public void UndoRedoClear()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
            GC.Collect();
        }

        /// <summary>Undo 명령</summary>
        public void ActUndo()
        {
            if (this.undoStack.Count <= 0)
                return;
            IUndoRedo undoRedo = this.undoStack.Pop();
            this.redoStack.Push(undoRedo);
            undoRedo.Undo();
            this.SelectedEntity = (List<IEntity>)null;
            foreach (IView view in this.owner.Views)
                view.Render();
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action undo: " + undoRedo.Name, Array.Empty<object>());
        }

        /// <summary>Redo 명령</summary>
        public void ActRedo()
        {
            if (this.redoStack.Count <= 0)
                return;
            IUndoRedo undoRedo = this.redoStack.Pop();
            this.undoStack.Push(undoRedo);
            undoRedo.Redo();
            this.SelectedEntity = (List<IEntity>)null;
            foreach (IView view in this.owner.Views)
                view.Render();
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action redo: " + undoRedo.Name, Array.Empty<object>());
        }

        /// <summary>신규 문서(Doc) 생성</summary>
        /// <returns></returns>
        public bool ActNew()
        {
            this.owner.New();
            this.ActEntityLaserPathSimulateStop();
            return true;
        }

        /// <summary>문서(Doc) 저장</summary>
        /// <param name="fileName">파일이름</param>
        /// <returns></returns>
        public bool ActSave(string fileName)
        {
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action trying to save: " + fileName, Array.Empty<object>());
            return DocumentSerializer.Save(this.owner, fileName);
        }

        /// <summary>dxf 파일 가져오기</summary>
        /// <param name="fileName">파일이름</param>
        /// <param name="importedEntity">생성된 개체</param>
        /// <returns></returns>
        public bool ActImportDxf(string fileName, out IEntity importedEntity)
        {
            importedEntity = (IEntity)null;
            DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(fileName, out bool _);
            if (dxfVersion == DxfVersion.Unknown)
                return false;
            if (dxfVersion < DxfVersion.AutoCad2000)
            {
                Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": version mismatched. fail to import: " + fileName, Array.Empty<object>());
                int num = (int)MessageBox.Show("Not support DXF file format : " + dxfVersion.ToString(), "DXF File Version", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            DxfDocument dxfDocument = DxfDocument.Load(fileName);
            if (dxfDocument == null)
            {
                Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": fail to import: " + fileName, Array.Empty<object>());
                Logger.Log(Logger.Type.Error, "fail to import file: " + fileName, Array.Empty<object>());
                return false;
            }
            Scanlab.Sirius.Blocks blocks = new Scanlab.Sirius.Blocks(this.owner);
            foreach (netDxf.Blocks.Block block1 in (TableObjects<netDxf.Blocks.Block>)dxfDocument.Blocks)
            {
                if (string.Compare(block1.Name, "*MODEL_SPACE", true) != 0)
                {
                    Block block2 = new Block();
                    block2.Name = block1.Name;
                    foreach (EntityObject entity in block1.Entities)
                    {
                        switch (entity.Type)
                        {
                            case EntityType.Arc:
                                IEntity arc = CreateArc(entity as netDxf.Entities.Arc);
                                block2.Add(arc);
                                continue;
                            case EntityType.Circle:
                                IEntity circle = CreateCircle(entity as netDxf.Entities.Circle);
                                block2.Add(circle);
                                continue;
                            case EntityType.Ellipse:
                                IEntity ellipse = CreateEllipse(entity as netDxf.Entities.Ellipse);
                                block2.Add(ellipse);
                                continue;
                            case EntityType.LightWeightPolyline:
                                LwPolyline lwPolyline = CreateLwPolyline(entity as netDxf.Entities.LwPolyline);
                                block2.Add((IEntity)lwPolyline);
                                continue;
                            case EntityType.Line:
                                IEntity line = CreateLine(entity as netDxf.Entities.Line);
                                block2.Add(line);
                                continue;
                            case EntityType.Point:
                                IEntity point = CreatePoint(entity as netDxf.Entities.Point);
                                block2.Add(point);
                                continue;
                            case EntityType.Polyline:
                                IEntity polyline = CreatePolyline(entity as Polyline);
                                block2.Add(polyline);
                                continue;
                            case EntityType.Spline:
                                IEntity spline = CreateSpline(entity as Spline);
                                block2.Add(spline);
                                continue;
                            default:
                                continue;
                        }
                    }
                    if (block2.Count > 0)
                        blocks.Add(block2);
                }
            }
            Group group = new Group();
            group.IsEnableFastRendering = true;
            group.Name = Path.GetFileName(fileName);
            foreach (netDxf.Objects.Layout layout in (TableObjects<netDxf.Objects.Layout>)dxfDocument.Layouts)
            {
                foreach (DxfObject reference in dxfDocument.Layouts.GetReferences(layout))
                {
                    if (reference is EntityObject entityObject2)
                    {
                        switch (entityObject2.Type)
                        {
                            case EntityType.Arc:
                                IEntity arc = CreateArc(entityObject2 as netDxf.Entities.Arc);
                                group.Add(arc);
                                continue;
                            case EntityType.Circle:
                                IEntity circle = CreateCircle(entityObject2 as netDxf.Entities.Circle);
                                group.Add(circle);
                                continue;
                            case EntityType.Ellipse:
                                IEntity ellipse = CreateEllipse(entityObject2 as netDxf.Entities.Ellipse);
                                group.Add(ellipse);
                                continue;
                            case EntityType.Insert:
                                BlockInsert insert = CreateInsert(entityObject2 as netDxf.Entities.Insert);
                                blocks.NameOf(insert.MasterBlockName);
                                BlockInsert blockInsert = new BlockInsert(insert.MasterBlockName, insert.Offset);
                                group.Add((IEntity)blockInsert);
                                continue;
                            case EntityType.LightWeightPolyline:
                                LwPolyline lwPolyline = CreateLwPolyline(entityObject2 as netDxf.Entities.LwPolyline);
                                group.Add((IEntity)lwPolyline);
                                continue;
                            case EntityType.Line:
                                IEntity line = CreateLine(entityObject2 as netDxf.Entities.Line);
                                group.Add(line);
                                continue;
                            case EntityType.Point:
                                IEntity point = CreatePoint(entityObject2 as netDxf.Entities.Point);
                                group.Add(point);
                                continue;
                            case EntityType.Polyline:
                                IEntity polyline = CreatePolyline(entityObject2 as Polyline);
                                group.Add(polyline);
                                continue;
                            case EntityType.Spline:
                                IEntity spline = CreateSpline(entityObject2 as Spline);
                                group.Add(spline);
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            this.ActEntityAdd((IEntity)group);
            importedEntity = (IEntity)group;
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action imported dxf file: " + fileName, Array.Empty<object>());
            return true;          
        }

        static IEntity CreateLine(netDxf.Entities.Line e)
        {
            Line line = new Line();
            netDxf.Vector3 startPoint = e.StartPoint;
            double x1 = startPoint.X;
            startPoint = e.StartPoint;
            double y1 = startPoint.Y;
            line.Start = new System.Numerics.Vector2((float)x1, (float)y1);
            netDxf.Vector3 endPoint = e.EndPoint;
            double x2 = endPoint.X;
            endPoint = e.EndPoint;
            double y2 = endPoint.Y;
            line.End = new System.Numerics.Vector2((float)x2, (float)y2);
            line.Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index);
            line.IsVisible = e.IsVisible;
            return (IEntity)line;
        }

        static IEntity CreatePoint(netDxf.Entities.Point e)
        {
            Point point = new Point();
            netDxf.Vector3 position = e.Position;
            double x = position.X;
            position = e.Position;
            double y = position.Y;
            point.Location = new System.Numerics.Vector2((float)x, (float)y);
            point.Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index);
            point.IsVisible = e.IsVisible;
            return (IEntity)point;
        }

        static IEntity CreateArc(netDxf.Entities.Arc e)
        {
            Arc arc = new Arc();
            netDxf.Vector3 center = e.Center;
            double x = center.X;
            center = e.Center;
            double y = center.Y;
            arc.Center = new System.Numerics.Vector2((float)x, (float)y);
            arc.StartAngle = (float)e.StartAngle;
            arc.SweepAngle = e.EndAngle > e.StartAngle ? (float)(e.EndAngle - e.StartAngle) : (float)(360.0 + e.EndAngle - e.StartAngle);
            arc.Radius = (float)e.Radius;
            arc.Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index);
            arc.IsVisible = e.IsVisible;
            return (IEntity)arc;
        }

        static IEntity CreateCircle(netDxf.Entities.Circle e)
        {
            Circle circle = new Circle();
            netDxf.Vector3 center = e.Center;
            double x = center.X;
            center = e.Center;
            double y = center.Y;
            circle.Center = new System.Numerics.Vector2((float)x, (float)y);
            circle.Radius = (float)e.Radius;
            circle.Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index);
            circle.IsVisible = e.IsVisible;
            return (IEntity)circle;
        }

        static IEntity CreateEllipse(netDxf.Entities.Ellipse e)
        {
            double num = Math.Abs(e.EndAngle - e.StartAngle);
            return (IEntity)CreateLwPolyline(e.ToPolyline((int)(num / (double)Config.AngleFactor)));
        }

        static LwPolyline CreateLwPolyline(netDxf.Entities.LwPolyline e)
        {
            List<netDxf.Vector2> vector2List = e.PolygonalVertexes(10, 0.001, 0.05);
            LwPolyline lwPolyline = new LwPolyline()
            {
                Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index),
                IsClosed = e.IsClosed
            };
            foreach (netDxf.Vector2 vector2 in vector2List)
                lwPolyline.Add(new LwPolyLineVertex((float)vector2.X, (float)vector2.Y));
            if (lwPolyline.Count > 2)
            {
                LwPolyLineVertex lwPolyLineVertex1 = lwPolyline[0];
                LwPolyLineVertex lwPolyLineVertex2 = lwPolyline[lwPolyline.Count - 1];
                if (MathHelper.IsEqual(lwPolyLineVertex1.X, lwPolyLineVertex2.X, Config.ClosedPathDistance) && MathHelper.IsEqual(lwPolyLineVertex1.Y, lwPolyLineVertex2.Y, Config.ClosedPathDistance))
                    lwPolyline.IsClosed = true;
            }
            return lwPolyline;
        }

        static BlockInsert CreateInsert(netDxf.Entities.Insert e)
        {
            BlockInsert blockInsert = new BlockInsert();
            blockInsert.Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index);
            blockInsert.MasterBlockName = e.Block.Name;
            double x1 = e.Position.X;
            double y1 = e.Position.Y;
            netDxf.Vector3 scale = e.Scale;
            double x2 = scale.X;
            scale = e.Scale;
            double y2 = scale.Y;
            double rotation = e.Rotation;
            blockInsert.Offset = new InsertVertex((float)x1, (float)y1, (float)x2, (float)y2, (float)rotation);
            return blockInsert;
        }

        static IEntity CreatePolyline(Polyline e)
        {
            LwPolyline lwPolyline1 = new LwPolyline()
            {
                Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index),
                IsClosed = e.IsClosed
            };
            foreach (PolylineVertex vertex in e.Vertexes)
            {
                LwPolyline lwPolyline2 = lwPolyline1;
                netDxf.Vector3 position = vertex.Position;
                double x = position.X;
                position = vertex.Position;
                double y = position.Y;
                LwPolyLineVertex lwPolyLineVertex = new LwPolyLineVertex((float)x, (float)y);
                lwPolyline2.Add(lwPolyLineVertex);
            }
            if (lwPolyline1.Count > 2)
            {
                LwPolyLineVertex lwPolyLineVertex1 = lwPolyline1[0];
                LwPolyLineVertex lwPolyLineVertex2 = lwPolyline1[lwPolyline1.Count - 1];
                if (MathHelper.IsEqual(lwPolyLineVertex1.X, lwPolyLineVertex2.X, Config.ClosedPathDistance) && MathHelper.IsEqual(lwPolyLineVertex1.Y, lwPolyLineVertex2.Y, Config.ClosedPathDistance))
                    lwPolyline1.IsClosed = true;
            }
            return (IEntity)lwPolyline1;
        }

        static IEntity CreateSpline(Spline e)
        {
            List<netDxf.Vector3> vector3List = e.PolygonalVertexes(e.ControlPoints.Count * 3);
            LwPolyline lwPolyline = new LwPolyline()
            {
                Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index),
                IsClosed = e.IsClosed
            };
            foreach (netDxf.Vector3 vector3 in vector3List)
                lwPolyline.Add(new LwPolyLineVertex((float)vector3.X, (float)vector3.Y));
            if (lwPolyline.Count > 2)
            {
                LwPolyLineVertex lwPolyLineVertex1 = lwPolyline[0];
                LwPolyLineVertex lwPolyLineVertex2 = lwPolyline[lwPolyline.Count - 1];
                if (MathHelper.IsEqual(lwPolyLineVertex1.X, lwPolyLineVertex2.X, 1f / 500f) && MathHelper.IsEqual(lwPolyLineVertex1.Y, lwPolyLineVertex2.Y, 1f / 500f))
                    lwPolyline.IsClosed = true;
            }
            return (IEntity)lwPolyline;
        }

        /// <summary>sirius 파일 가져오기</summary>
        /// <param name="fileName">파일이름</param>
        /// <param name="importedEntity">생성된 개체</param>
        /// <returns></returns>
        public bool ActImportSirius(string fileName, out IEntity importedEntity)
        {
            importedEntity = (IEntity)null;
            Group group = new Group();
            group.IsEnableFastRendering = false;
            group.Name = Path.GetFileName(fileName);
            IDocument document = DocumentSerializer.OpenSirius(fileName);
            if (document == null)
            {
                Logger.Log(Logger.Type.Error, "document " + this.owner.Name + ": fail to import file: " + fileName, Array.Empty<object>());
                return false;
            }
            foreach (ObservableList<IEntity> layer in (ObservableList<Layer>)document.Layers)
            {
                foreach (IEntity entity in layer)
                    group.Add(entity);
            }
            this.ActEntityAdd((IEntity)group);
            importedEntity = (IEntity)group;
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action imported sirius file: " + fileName, Array.Empty<object>());
            return true;
        }

        /// <summary>HPGL 파일 가져오기</summary>
        /// <param name="fileName"></param>
        /// <param name="importedEntity">생성된 개체</param>
        /// <returns></returns>
        public bool ActImportHPGL(string fileName, out IEntity importedEntity) => this.ActImportHPGLByCpp(fileName, out importedEntity);

        /// <summary>HPGL 파일 가져오기 (신규 버전)</summary>
        /// <param name="fileName"></param>
        /// <param name="importedEntity">생성된 개체</param>
        /// <returns></returns>
        public bool ActImportHPGLByCpp(string fileName, out IEntity importedEntity)
        {
            importedEntity = (IEntity)null;
            if (!HPGLCppHelper.ImportHPGLFile(fileName))
            {
                Logger.Log(Logger.Type.Error, "fail to open hpgl file: " + fileName, Array.Empty<object>());
                return false;
            }
            int num1 = HPGLCppHelper.HPGLPolyLineCount();
            Group group = new Group();
            group.IsEnableFastRendering = true;
            group.Name = Path.GetFileName(fileName);
            LwPolyLineVertex other = new LwPolyLineVertex(float.MaxValue, float.MaxValue);
            LwPolyline lwPolyline1 = new LwPolyline();
            for (int polyLineIndex = 0; polyLineIndex < num1; ++polyLineIndex)
            {
                int num2 = HPGLCppHelper.HPGLPolyLineVertexCount(polyLineIndex);
                if (polyLineIndex > 0)
                {
                    HPGLCppHelper.HPGLVertex hpglVertex1 = HPGLCppHelper.HPGLPolyLineVertexData(polyLineIndex, 0);
                    if (new LwPolyLineVertex(hpglVertex1.x, hpglVertex1.y).Equals(other))
                    {
                        for (int index = 1; index < num2; ++index)
                        {
                            HPGLCppHelper.HPGLVertex hpglVertex2 = HPGLCppHelper.HPGLPolyLineVertexData(polyLineIndex, index);
                            LwPolyLineVertex lwPolyLineVertex = new LwPolyLineVertex(hpglVertex2.x, hpglVertex2.y);
                            if (!lwPolyLineVertex.Equals(other))
                            {
                                lwPolyline1.Add(lwPolyLineVertex);
                                other = lwPolyLineVertex;
                            }
                        }
                    }
                    else
                    {
                        group.Add((IEntity)lwPolyline1);
                        lwPolyline1 = new LwPolyline();
                        for (int index = 0; index < num2; ++index)
                        {
                            HPGLCppHelper.HPGLVertex hpglVertex2 = HPGLCppHelper.HPGLPolyLineVertexData(polyLineIndex, index);
                            LwPolyLineVertex lwPolyLineVertex = new LwPolyLineVertex(hpglVertex2.x, hpglVertex2.y);
                            if (!lwPolyLineVertex.Equals(other))
                            {
                                lwPolyline1.Add(lwPolyLineVertex);
                                other = lwPolyLineVertex;
                            }
                        }
                    }
                }
                else
                {
                    for (int index = 0; index < num2; ++index)
                    {
                        HPGLCppHelper.HPGLVertex hpglVertex = HPGLCppHelper.HPGLPolyLineVertexData(polyLineIndex, index);
                        LwPolyLineVertex lwPolyLineVertex = new LwPolyLineVertex(hpglVertex.x, hpglVertex.y);
                        if (!lwPolyLineVertex.Equals(other))
                        {
                            lwPolyline1.Add(lwPolyLineVertex);
                            other = lwPolyLineVertex;
                        }
                    }
                }
            }
            group.Add((IEntity)lwPolyline1);
            for (int index = 0; index < group.Items.Length; ++index)
            {
                LwPolyline lwPolyline2 = group.Items[index] as LwPolyline;
                if (lwPolyline2.Count > 2)
                {
                    LwPolyLineVertex lwPolyLineVertex1 = lwPolyline2[0];
                    LwPolyLineVertex lwPolyLineVertex2 = lwPolyline2[lwPolyline2.Count - 1];
                    if (MathHelper.IsEqual(lwPolyLineVertex1.X, lwPolyLineVertex2.X, Config.ClosedPathDistance) && MathHelper.IsEqual(lwPolyLineVertex1.Y, lwPolyLineVertex2.Y, Config.ClosedPathDistance))
                        lwPolyline2.IsClosed = true;
                }
            }
            this.ActEntityAdd((IEntity)group);
            importedEntity = (IEntity)group;
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action imported hpgl file: " + fileName, Array.Empty<object>());
            return true;
        }

        public bool ActImportSTL(string fileName, out IEntity importedEntity)
        {
            STLHelper stlHelper = new STLHelper(fileName);
            Stereolithography stereolithography = new Stereolithography();
            stereolithography.Name = Path.GetFileName(fileName);
            stereolithography.Items = stlHelper.Facets.ToArray();
            stereolithography.Width = stlHelper.Width;
            stereolithography.Height = stlHelper.Height;
            stereolithography.Depth = stlHelper.Depth;
            stereolithography.Location = System.Numerics.Vector3.Negate(stlHelper.Center);
            this.ActEntityAdd((IEntity)stereolithography);
            importedEntity = (IEntity)stereolithography;
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action imported stl file: " + fileName, Array.Empty<object>());
            return true;
        }

        /// <summary>DOC 의 속성 변경 명령</summary>
        /// <param name="doc">문서</param>
        /// <param name="propName">속성이름</param>
        /// <param name="oldValue">변경전 값</param>
        /// <param name="newValue">변경후 값</param>
        /// <returns></returns>
        public bool ActDocumentPropertyChanged(
          IDocument doc,
          string propName,
          object oldValue,
          object newValue)
        {
            if (doc == null)
                return false;
            this.Insert((IUndoRedo)new UndoRedoDocumentPropertyChanged(this.owner, propName, oldValue, newValue));
            Action.EntitySelectedChangedEvent entitySelectedChanged = this.OnEntitySelectedChanged;
            if (entitySelectedChanged != null)
                entitySelectedChanged(this.owner, this.selectedEntityList);
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action document property changed: " + propName + ", " + oldValue?.ToString() + " -> " + newValue?.ToString(), Array.Empty<object>());
            return true;
        }

        /// <summary>활성화 레이어 변경 명령</summary>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActLayerActive(Layer layer)
        {
            if (layer == null)
                return false;
            this.Insert((IUndoRedo)new UndoRedoLayerActive(this.owner, layer));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action layer active to: " + layer.Name, Array.Empty<object>());
            return true;
        }

        /// <summary>레이어의 순서를 변경 명령</summary>
        /// <param name="layer">현재 레이어</param>
        /// <param name="targetLayer">대상 레이어</param>
        /// <param name="targetIndex">순서 인덱스 번호</param>
        /// <returns></returns>
        public bool ActLayerDragMove(Layer layer, Layer targetLayer, int targetIndex)
        {
            switch (layer)
            {
                case null:
                    return false;
                default:
                    this.Insert((IUndoRedo)new UndoRedoLayerMove(this.owner, layer, targetLayer, targetIndex));
                    Logger.Log(Logger.Type.Debug, string.Format("document {0}: action layer move from {1} to {2} at {3}", (object)this.owner.Name, (object)layer.Name, (object)targetLayer.Name, (object)targetIndex), Array.Empty<object>());
                    return true;
            }
        }

        /// <summary>단일 엔티티 추가 명령</summary>
        /// <param name="entity">선택 엔티티</param>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActEntityAdd(IEntity entity, Layer layer = null)
        {
            if (entity == null)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityAdd(this.owner, layer, entity));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity added: " + entity.Name, Array.Empty<object>());
            return true;
        }

        /// <summary>다중 엔티티 추가 명령</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActEntityAdd(List<IEntity> entities, Layer layer = null)
        {
            if (entities == null || entities.Count == 0)
                return false;
            UndoRedoMultiple undoRedoMultiple = new UndoRedoMultiple();
            foreach (IEntity entity in entities)
            {
                UndoRedoEntityAdd undoRedoEntityAdd = new UndoRedoEntityAdd(this.owner, layer, entity);
                undoRedoMultiple.Add((IUndoRedo)undoRedoEntityAdd);
            }
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entities added counts= {1}", (object)this.owner.Name, (object)entities.Count), Array.Empty<object>());
            this.Insert((IUndoRedo)undoRedoMultiple);
            return true;
        }

        /// <summary>엔티티 선택 명령</summary>
        /// <param name="eneities">선택 엔티티 리스트</param>
        /// <returns></returns>
        public bool ActEntitySelect(List<IEntity> eneities)
        {
            if (eneities == null)
                return false;
            if (eneities.SequenceEqual<IEntity>((IEnumerable<IEntity>)this.selectedEntityList))
                return true;
            new UndoRedoEntitySelect(this.owner, eneities).Execute();
            foreach (IView view in this.owner.Views)
                view.Render();
            return true;
        }

        /// <summary>지정된 엔티티들을 그룹으로 변환 명령</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActEntityGroup(List<IEntity> entities, Layer layer = null)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityGroup(this.owner, entities, layer));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action create entities to group", Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 그룹들을 개별 엔티티로 변환 명령</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActEntityUngroup(List<IEntity> entities, Layer layer = null)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityUnGroup(this.owner, entities, layer));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action group to ungroup", Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티 삭제 명령</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <returns></returns>
        public bool ActEntityDelete(List<IEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityDelete(this.owner, entities));
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entities deleted: {1}", (object)this.owner.Name, (object)entities.Count), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 지정된 엔티티 위치 이동 명령
        /// 복수의 객체의 경우 해당 객체들을 둘러싸는 영역(BoundRect)을 기준으로 동작
        /// 하나의 객체의 경우 Document 의 가로 세로 크기를 기준으로 동작
        /// </summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="align">정렬 기준</param>
        /// <returns></returns>
        public bool ActEntityAlign(List<IEntity> entities, EntityAlign align)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityAlign(this.owner, entities, align));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity aligned: " + align.ToString(), Array.Empty<object>());
            return true;
        }

        /// <summary>엔티티 정렬하기</summary>
        /// <param name="entities">대상 엔티티 리스트</param>
        /// <param name="layer">레이어</param>
        /// <param name="sort">정렬 방법</param>
        /// <returns></returns>
        public bool ActEntitySort(List<IEntity> entities, Layer layer, EntitySort sort)
        {
            if (layer == null || entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntitySort(this.owner, layer, entities, sort));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity sort: " + sort.ToString(), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티의 위치를 특정한 양 만큼 이동 명령</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="dx">X 이동량 </param>
        /// <param name="dy">Y 이동량 </param>
        /// <returns></returns>
        public bool ActEntityTransit(List<IEntity> entities, float dx, float dy)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityTransit(this.owner, entities, new System.Numerics.Vector2(dx, dy)));
            Action.EntitySelectedChangedEvent entitySelectedChanged = this.OnEntitySelectedChanged;
            if (entitySelectedChanged != null)
                entitySelectedChanged(this.owner, this.selectedEntityList);
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity transit: dx={1:F3}, dy={2:F3}", (object)this.owner.Name, (object)dx, (object)dy), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티의 순서를 변경하는 명령</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="targetLayer">대상 레이어</param>
        /// <param name="targetIndex">대상 레이어 인덱스 번호</param>
        /// <returns></returns>
        public bool ActEntityDragMove(List<IEntity> entities, Layer targetLayer, int targetIndex)
        {
            if (entities == null || entities.Count == 0)
                return false;
            if (1 == entities.Count)
            {
                this.Insert((IUndoRedo)new UndoRedoEntityMove(this.owner, entities[0], targetLayer, targetIndex));
            }
            else
            {
                foreach (IEntity entity in entities)
                {
                    if (entity is Layer && DialogResult.Yes != MessageBox.Show("Please un-select layer entity !\r\n레이어가 선택되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand))
                        return false;
                }
                Layer layer = (Layer)null;
                foreach (IEntity entity in entities)
                {
                    if (layer != null && !layer.Equals((object)(entity.Owner as Layer)) && DialogResult.Yes != MessageBox.Show("Please select entities within a single layer !\r\n대상이 하나의 레이어안에서만 선택되어 있어야 합니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand))
                        return false;
                    layer = entity.Owner as Layer;
                }
                this.Insert((IUndoRedo)new UndoRedoEntitiesMove(this.owner, entities, targetLayer, targetIndex));
            }
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity drag moved to {1} at {2}", (object)this.owner.Name, (object)targetLayer.Name, (object)targetIndex), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티를 분해하여 추가하고 원본 엔티티는 삭제함</summary>
        /// <param name="entities">선택 엔티티 리스트</param>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActEntityExplode(List<IEntity> entities, Layer layer = null)
        {
            if (entities == null || entities.Count == 0 || 1 != entities.Count && DialogResult.Yes != MessageBox.Show("Please select only one entity to explode !\r\n하나의 대상만 선택하여 주십시오", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand))
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityExplode(this.owner, entities[0], layer));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity has exploded", Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티가 닫힌 형태라면 해치 패턴을 추가하고 이를 신규 그룹 개체로 생성한다.</summary>
        /// <param name="entities">엔티티 리스트</param>
        /// <param name="mode">해치 모드</param>
        /// <param name="angle">해치 각도</param>
        /// <param name="angle2">크로스 해치 모드 시 두번째 해치 각도</param>
        /// <param name="interval">해치 간격</param>
        /// <param name="exclude">해치 외곽 제외 영역</param>
        /// <param name="layer">대상 레이어</param>
        /// <returns></returns>
        public bool ActEntityHatch(
          List<IEntity> entities,
          HatchMode mode,
          float angle,
          float angle2,
          float interval,
          float exclude,
          Layer layer = null)
        {
            if (entities == null || entities.Count == 0 || 1 != entities.Count && DialogResult.Yes != MessageBox.Show("Please select only one entity to hatch !\r\n하나의 대상만 선택하여 주십시오", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand))
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityHatch(this.owner, entities[0], layer, mode, angle, angle2, interval, exclude));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity has hatched", Array.Empty<object>());
            return true;
        }

        /// <summary>선택된 엔티티들을 cx, cy 를 중심으로 회전한다</summary>
        /// <param name="entities">엔티티 리스트</param>
        /// <param name="angle">각도</param>
        /// <param name="cx">중심 X</param>
        /// <param name="cy">중심 Y</param>
        /// <returns></returns>
        public bool ActEntityRotate(List<IEntity> entities, float angle, float cx, float cy)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityRotate(this.owner, entities, angle, new System.Numerics.Vector2(cx, cy)));
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity rotated {1:F3}º at cx={2:F3}, cy={3:F3}", (object)this.owner.Name, (object)angle, (object)cx, (object)cy), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티들을 Cut 하는 명령 (삭제후 클립보드로 복사됨)</summary>
        /// <param name="entities">엔티티 리스트</param>
        /// <returns></returns>
        public bool ActEntityCut(List<IEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityCut(this.owner, entities));
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity has cut to clipboard", Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티들을 Copy 하는 명령(삭제되지 않고 클립보드로 복사됨)</summary>
        /// <param name="entities">엔티티 리스트</param>
        /// <returns></returns>
        public bool ActEntityCopy(List<IEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return false;
            Action.ClipBoard.Clear();
            Action.ClipBoard.AddRange((IEnumerable<IEntity>)entities);
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity has copied to clipboard", Array.Empty<object>());
            return true;
        }

        /// <summary>클립보드에 있는 엔티티 를 복제하여 붙여넣기 함 (신규 엔티티가 생성됨)</summary>
        /// <param name="layer">레이어</param>
        /// <param name="x">X 위치</param>
        /// <param name="y">Y 위치</param>
        /// <returns></returns>
        public bool ActEntityPaste(Layer layer, float x = 0.0f, float y = 0.0f)
        {
            if (Action.ClipBoard.Count == 0)
                return false;
            UndoRedoEntityPaste undoRedoEntityPaste = new UndoRedoEntityPaste(this.owner, layer, new System.Numerics.Vector2(x, y));
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity pasted from clipboard at x={1:F3}, y={2:F3}", (object)this.owner.Name, (object)x, (object)y), Array.Empty<object>());
            this.Insert((IUndoRedo)undoRedoEntityPaste);
            return true;
        }

        /// <summary>
        /// 클립보드에 있는 엔티티 를 복제하여 위치를 변경하지 않고 같은 위치에 붙여넣기 함 (신규 엔티티가 생성됨)
        /// </summary>
        /// <param name="layer">레이어</param>
        /// <returns></returns>
        public bool ActEntityPasteClone(Layer layer)
        {
            if (Action.ClipBoard.Count == 0)
                return false;
            UndoRedoEntityPaste undoRedoEntityPaste = new UndoRedoEntityPaste(this.owner, layer);
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity cloned pasted from clipboard", Array.Empty<object>());
            this.Insert((IUndoRedo)undoRedoEntityPaste);
            return true;
        }

        /// <summary>클립보드에 있는 엔티티를 복제하여 배열 붙혀넣기 (신규 엔티티가 생성됨)</summary>
        /// <param name="layer">레이어</param>
        /// <param name="targetLocations">대상 위치 목록</param>
        /// <param name="x">기준 X 위치</param>
        /// <param name="y">기준 Y 위치</param>
        /// <returns></returns>
        public bool ActEntityPasteArray(Layer layer, List<System.Numerics.Vector2> targetLocations, float x = 0.0f, float y = 0.0f)
        {
            if (Action.ClipBoard.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityPasteArray(this.owner, layer, new System.Numerics.Vector2(x, y), targetLocations));
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity array pasted from clipboard at x={1:F3}, y={2:F3} with {3} counts", (object)this.owner.Name, (object)x, (object)y, (object)targetLocations.Count), Array.Empty<object>());
            return true;
        }

        /// <summary>지정된 엔티티의 속성을 변경하는 명령</summary>
        /// <param name="entities">엔티티 리스트</param>
        /// <param name="propName">속성 이름</param>
        /// <param name="oldValue">변경전 값</param>
        /// <param name="newValue">변경후 값</param>
        /// <returns></returns>
        public bool ActEntityPropertyChanged(
          List<IEntity> entities,
          string propName,
          object oldValue,
          object newValue)
        {
            if (entities == null || entities.Count == 0)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityPropertyChanged(this.owner, entities, propName, oldValue, newValue));
            Action.EntitySelectedChangedEvent entitySelectedChanged = this.OnEntitySelectedChanged;
            if (entitySelectedChanged != null)
                entitySelectedChanged(this.owner, this.selectedEntityList);
            Logger.Log(Logger.Type.Debug, "document " + this.owner.Name + ": action entity propert changed " + propName + ", " + oldValue?.ToString() + " -> " + newValue?.ToString(), Array.Empty<object>());
            return true;
        }

        /// <summary>대상 레이어에 있는 엔티티를 신규 엔티티로 대체한다</summary>
        /// <param name="targetLayer">대상 레이어</param>
        /// <param name="targetEntity">대상 엔티티</param>
        /// <param name="replaceEntity">교체할 엔티티</param>
        /// <returns></returns>
        public bool ActEntityReplace(Layer targetLayer, IEntity targetEntity, IEntity replaceEntity)
        {
            if (targetLayer == null || targetEntity == null || replaceEntity == null)
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityReplace(this.owner, targetLayer, targetEntity, replaceEntity));
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity repleace from {1}-> {2}", (object)this.owner.Name, (object)targetEntity, (object)replaceEntity), Array.Empty<object>());
            return true;
        }

        /// <summary>
        /// 지정된 그룹 엔티티를 지정된 사각영역에 포함되어 있는 엔티티들로 골라 분해 복제하고, 대상 그룹은 삭제한다
        /// (experimental)
        /// </summary>
        /// <param name="targets">대상 엔티티 </param>
        /// <param name="rects">사각 영역 집합</param>
        /// <returns></returns>
        public bool ActEntityDivide(List<IEntity> targets, List<BoundRect> rects)
        {
            if (targets == null || targets.Count == 0 || (rects == null || rects.Count == 0))
                return false;
            this.Insert((IUndoRedo)new UndoRedoEntityDivide(this.owner, this.owner.Layers.Active, targets, rects));
            Logger.Log(Logger.Type.Debug, string.Format("document {0}: action entity divide into {1} area(s)", (object)this.owner.Name, (object)rects.Count), Array.Empty<object>());
            return true;
        }

        /// <summary>마우스 클릭등의 한점을 지정하여 엔티티 선택 명령 (Undo/Redo 스택에 처리 않됨)</summary>
        /// <param name="keys">특수키(ctrl, alt, shift, ...)</param>
        /// <param name="x">X 위치</param>
        /// <param name="y">Y 위치</param>
        /// <param name="threshold">문턱값</param>
        /// <returns></returns>
        public int HitTest(Keys keys, float x, float y, float threshold = 0.02f)
        {
            List<IEntity> eneities = new List<IEntity>();
            bool flag = keys == Keys.Control;
            foreach (Layer layer in (ObservableList<Layer>)this.owner.Layers)
            {
                if (layer.IsVisible)
                {
                    foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    {
                        if (entity is IDrawable drawable3 && drawable3.HitTest(x, y, threshold))
                        {
                            if (flag)
                            {
                                if (!entity.IsSelected)
                                    eneities.Add(entity);
                            }
                            else
                                eneities.Add(entity);
                        }
                    }
                }
            }
            this.ActEntitySelect(eneities);
            return eneities.Count;
        }

        /// <summary>
        /// 마우스를 이용해 사각 영역(Rubber Band)을 지정하여 엔티티 선택 명령 (Undo/Redo 스택에 처리 않됨)
        /// </summary>
        /// <param name="keys">특수키(ctrl, alt, shift, ...)</param>
        /// <param name="downX">시작 영역 X</param>
        /// <param name="downY">시작 영역 Y</param>
        /// <param name="currentX">현재 영역 X</param>
        /// <param name="currentY">현재 영역 Y</param>
        /// <param name="threshold">문턱값</param>
        /// <returns></returns>
        public int HitTest(
          Keys keys,
          float downX,
          float downY,
          float currentX,
          float currentY,
          float threshold = 0.02f)
        {
            List<IEntity> eneities = new List<IEntity>();
            bool flag1 = keys == Keys.Shift;
            bool flag2 = keys == Keys.Alt;
            foreach (Layer layer in (ObservableList<Layer>)this.owner.Layers)
            {
                if (layer.IsVisible)
                {
                    foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    {
                        if (entity is IDrawable drawable3)
                        {
                            float left = (double)downX < (double)currentX ? downX : currentX;
                            float right = (double)downX > (double)currentX ? downX : currentX;
                            float top = (double)downY > (double)currentY ? downY : currentY;
                            float bottom = (double)downY < (double)currentY ? downY : currentY;
                            if (drawable3.HitTest(left, top, right, bottom, threshold))
                            {
                                if (!flag2)
                                    eneities.Add(entity);
                            }
                            else if ((flag1 || flag2) && entity.IsSelected)
                                eneities.Add(entity);
                        }
                    }
                }
            }
            this.ActEntitySelect(eneities);
            return eneities.Count;
        }

        public bool ActEntityLaserPathSimulateStart(
          List<IEntity> list,
          IView targetView,
          Action.LaserPathSimSpped speed)
        {
            if (this.threadSimulator != null)
                this.ActEntityLaserPathSimulateStop();
            if (list.Count == 0)
            {
                int num = (int)MessageBox.Show("Please select entitie(s) to simulate the laser path !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            this.targetView = targetView;
            this.simulateSpeed = speed;
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in list)
                boundRect.Union(entity.BoundRect);
            this.rtcLaserPath = new RtcVirtual(0U);
            this.rtcLaserPath.OnJumpTo += new RtcJumpTo(this.OnJumpTo);
            this.rtcLaserPath.OnMarkTo += new RtcMarkTo(this.OnMarkTo);
            this.entityLaserPath = new List<IEntity>(list.Count);
            foreach (IEntity entity1 in list)
            {
                if (entity1 is ICloneable cloneable1)
                {
                    IEntity entity2 = (IEntity)cloneable1.Clone();
                    entity2.Regen();
                    this.entityLaserPath.Add(entity2);
                }
            }
            this.isSimulatorTerminated = false;
            this.threadSimulator = new Thread(new ParameterizedThreadStart(this.OnLaserPathTask));
            this.threadSimulator.Start();
            return true;
        }

        public bool ActEntityLaserPathSimulateStop()
        {
            if (this.threadSimulator != null)
            {
                this.isSimulatorTerminated = true;
                this.rtcLaserPath?.CtlAbort();
                this.threadSimulator.Join(100);
                this.rtcLaserPath?.Dispose();
                this.rtcLaserPath = (RtcVirtual)null;
                this.threadSimulator = (Thread)null;
            }
            if (this.targetView is ViewDefault targetView)
                targetView.LaserSpot = System.Numerics.Vector2.Zero;
            return true;
        }

        private void OnLaserPathTask(object args)
        {
            bool flag = true;
            MarkerArgDefault markerArgDefault = new MarkerArgDefault()
            {
                Rtc = (IRtc)this.rtcLaserPath,
                StartTime = DateTime.Now
            };
            markerArgDefault.Offsets.Add(Offset.Zero);
            this.rtcLaserPath.ListBegin((ILaser)null, ListType.Auto);
            foreach (IEntity entity in this.entityLaserPath)
            {
                if (entity is IMarkerable markerable2)
                {
                    flag &= markerable2.Mark((IMarkerArg)markerArgDefault);
                    if (!flag)
                        break;
                }
                if (this.isSimulatorTerminated)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                this.rtcLaserPath?.ListEnd();
                this.rtcLaserPath?.ListExecute(true);
            }
            if (!(this.targetView is ViewDefault))
                return;
            ((ViewDefault)this.targetView).LaserSpot = System.Numerics.Vector2.Zero;
        }

        delegate void OnJumpToDelegate();

        private void OnJumpTo(object sender, System.Numerics.Vector2 v)
        {
            ViewDefault viewDefault = this.targetView as ViewDefault;

            OnJumpToDelegate onJumpTo = delegate
            {
                viewDefault.LaserSpot = System.Numerics.Vector2.Zero;
                viewDefault.OnCameraMove(v.X, v.Y);
            };

            try
            {
                viewDefault.glControl.BeginInvoke(onJumpTo);
                
                Application.DoEvents();
            }
            catch (Exception ex)
            {
            }
            switch (this.simulateSpeed)
            {
                case Action.LaserPathSimSpped.Slow:
                    Thread.Sleep(80);
                    break;
                case Action.LaserPathSimSpped.Normal:
                    Thread.Sleep(50);
                    break;
                case Action.LaserPathSimSpped.Fast:
                    Thread.Sleep(20);
                    break;
            }
        }

        delegate void OnMarkToDelegate();

        private void OnMarkTo(object sender, System.Numerics.Vector2 v)
        {
            ViewDefault viewDefault = this.targetView as ViewDefault;
            try
            {
                OnMarkToDelegate onMarkTo = delegate
                {
                    viewDefault.LaserSpot = v;
                    viewDefault.OnCameraMove(v.X, v.Y);
                };

                viewDefault.glControl.BeginInvoke(onMarkTo);

                Application.DoEvents();
            }
            catch (Exception ex)
            {
            }
            switch (this.simulateSpeed)
            {
                case Action.LaserPathSimSpped.Slow:
                    Thread.Sleep(60);
                    break;
                case Action.LaserPathSimSpped.Normal:
                    Thread.Sleep(50);
                    break;
                case Action.LaserPathSimSpped.Fast:
                    Thread.Sleep(20);
                    break;
            }
        }

        /// <summary>선택된 엔티티 목록이 변경된것을 통지하는 이벤트 델리게이트</summary>
        /// <param name="doc"></param>
        /// <param name="list"></param>
        public delegate void EntitySelectedChangedEvent(IDocument doc, List<IEntity> list);

        public enum LaserPathSimSpped
        {
            Slow,
            Normal,
            Fast,
        }
    }
}
