
using netDxf;
using netDxf.Collections;
using netDxf.Entities;
using netDxf.Header;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>문서 객체 저장/복원용</summary>
    public sealed class DocumentSerializer
    {
        /// <summary>sirius 파일 열기</summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IDocument OpenSirius(string fileName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Binder = (SerializationBinder)new SiriusTypeNameSerializationBinder("Scanlab.Sirius.{0}"),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            TextReader textReader = (TextReader)null;
            try
            {
                textReader = (TextReader)new StreamReader(fileName);
                IDocument document = JsonConvert.DeserializeObject<IDocument>(textReader.ReadToEnd(), settings);
                if (document == null)
                    return (IDocument)null;
                foreach (Layer layer in (ObservableList<Layer>)document.Layers)
                {
                    foreach (IEntity entity in (ObservableList<IEntity>)layer)
                        entity.Owner = (IEntity)layer;
                }
                if (document.Layers.Count > 0)
                    document.Layers.Active = document.Layers[0];
                if (!document.Dimension.IsEmpty)
                {
                    document.Dimension.Width /= 2f;
                    document.Dimension.Height /= 2f;
                }
                return document;
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Type.Error, (object)ex);
            }
            finally
            {
                textReader?.Close();
            }
            return (IDocument)null;
        }

        /// <summary>dxf 파일 열기</summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IDocument OpenDxf(string fileName)
        {
            DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(fileName, out bool _);
            if (dxfVersion == DxfVersion.Unknown)
                return (IDocument)null;
            if (dxfVersion < DxfVersion.AutoCad2000)
            {
                Logger.Log(Logger.Type.Debug, "fail to open dxf file. version mismatched : " + fileName, Array.Empty<object>());
                int num = (int)MessageBox.Show("Not support DXF file format : " + dxfVersion.ToString(), "DXF File Version", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return (IDocument)null;
            }
            DxfDocument dxfDocument = DxfDocument.Load(fileName);
            if (dxfDocument == null)
            {
                Logger.Log(Logger.Type.Error, "fail to open file: " + fileName, Array.Empty<object>());
                return (IDocument)null;
            }
            DocumentDefault documentDefault = new DocumentDefault();
            documentDefault.FileName = fileName;
            foreach (netDxf.Tables.Layer layer1 in (TableObjects<netDxf.Tables.Layer>)dxfDocument.Layers)
            {
                Layer layer2 = new Layer(layer1.Name)
                {
                    IsVisible = layer1.IsVisible,
                    IsLocked = layer1.IsLocked
                };
                documentDefault.Layers.Add(layer2);
            }
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
                            case EntityType.Spline:
                                IEntity spline = CreateSpline(entity as Spline);
                                block2.Add(spline);
                                continue;
                            default:
                                continue;
                        }
                    }
                    documentDefault.Blocks.Add(block2);
                }
            }
            foreach (netDxf.Objects.Layout layout in (TableObjects<netDxf.Objects.Layout>)dxfDocument.Layouts)
            {
                foreach (DxfObject reference in dxfDocument.Layouts.GetReferences(layout))
                {
                    if (reference is EntityObject entityObject2)
                    {
                        Layer layer = documentDefault.Layers.NameOf(entityObject2.Layer.Name);
                        switch (entityObject2.Type)
                        {
                            case EntityType.Arc:
                                IEntity arc = CreateArc(entityObject2 as netDxf.Entities.Arc);
                                arc.Owner = (IEntity)layer;
                                layer.Add(arc);
                                continue;
                            case EntityType.Circle:
                                IEntity circle = CreateCircle(entityObject2 as netDxf.Entities.Circle);
                                circle.Owner = (IEntity)layer;
                                layer.Add(circle);
                                continue;
                            case EntityType.Ellipse:
                                IEntity ellipse = CreateEllipse(entityObject2 as netDxf.Entities.Ellipse);
                                ellipse.Owner = (IEntity)layer;
                                layer.Add(ellipse);
                                continue;
                            case EntityType.Insert:
                                CreateInsert(entityObject2 as Insert);
                                continue;
                            case EntityType.LightWeightPolyline:
                                LwPolyline lwPolyline = CreateLwPolyline(entityObject2 as netDxf.Entities.LwPolyline);
                                lwPolyline.Owner = (IEntity)layer;
                                layer.Add((IEntity)lwPolyline);
                                continue;
                            case EntityType.Line:
                                IEntity line = CreateLine(entityObject2 as netDxf.Entities.Line);
                                line.Owner = (IEntity)layer;
                                layer.Add(line);
                                continue;
                            case EntityType.Point:
                                IEntity point = CreatePoint(entityObject2 as netDxf.Entities.Point);
                                point.Owner = (IEntity)layer;
                                layer.Add(point);
                                continue;
                            case EntityType.Polyline:
                                IEntity polyline = CreatePolyline(entityObject2 as Polyline);
                                polyline.Owner = (IEntity)layer;
                                layer.Add(polyline);
                                continue;
                            case EntityType.Spline:
                                IEntity spline = CreateSpline(entityObject2 as Spline);
                                spline.Owner = (IEntity)layer;
                                layer.Add(spline);
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            if (documentDefault.Layers.Count > 0)
                documentDefault.Layers.Active = documentDefault.Layers[0];
            foreach (IView view in documentDefault.Views)
            {
                view.Render();
                view.OnZoomFit();
            }
            documentDefault.Action.UndoRedoClear();
            return (IDocument)documentDefault;

            
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

        static LwPolyline CreateLwPolyline(netDxf.Entities.LwPolyline e)
        {
            LwPolyline lwPolyline1 = new LwPolyline()
            {
                Color2 = e.Color.IsByLayer ? Config.DefaultColor : AciColor.FromCadIndexToColor(e.Color.Index),
                IsClosed = e.IsClosed
            };
            foreach (LwPolylineVertex vertex in e.Vertexes)
            {
                LwPolyline lwPolyline2 = lwPolyline1;
                netDxf.Vector2 position = vertex.Position;
                double x = position.X;
                position = vertex.Position;
                double y = position.Y;
                double bulge = vertex.Bulge;
                LwPolyLineVertex lwPolyLineVertex = new LwPolyLineVertex((float)x, (float)y, (float)bulge);
                lwPolyline2.Add(lwPolyLineVertex);
            }
            return lwPolyline1;
        }

        static IEntity CreateInsert(Insert e)
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
            return (IEntity)blockInsert;
        }

        static IEntity CreateEllipse(netDxf.Entities.Ellipse e)
        {
            double num = Math.Abs(e.EndAngle - e.StartAngle);
            return (IEntity)CreateLwPolyline(e.ToPolyline((int)(num / (double)Config.AngleFactor)));
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
            return (IEntity)lwPolyline;
        }

        /// <summary>sirius 파일로 저장</summary>
        /// <param name="doc"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Save(IDocument doc, string fileName)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Binder = (SerializationBinder)new SiriusTypeNameSerializationBinder("Scanlab.Sirius.{0}"),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            TextWriter textWriter = (TextWriter)null;
            doc.FileName = fileName;
            try
            {
                string str = JsonConvert.SerializeObject((object)doc, Formatting.Indented, settings);
                textWriter = (TextWriter)new StreamWriter(fileName, false);
                textWriter.Write(str);
            }
            finally
            {
                textWriter?.Close();
            }
            return true;
        }
    }
}
