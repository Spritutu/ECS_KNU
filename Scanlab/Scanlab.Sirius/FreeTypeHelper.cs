using SharpFont;
using System;
using System.Numerics;

namespace Scanlab.Sirius
{
    internal class FreeTypeHelper
    {
        public System.Collections.Generic.List<IEntity> List = new System.Collections.Generic.List<IEntity>();
        private const float scaleFactor = 1000f;
        private LwPolyline polyline;
        private Vector2 vCurrent = Vector2.Zero;
        private float baseX;
        private float baseY;
        private static Library lib;

        public FreeTypeHelper(
          string fontName,
          string text,
          float width,
          float height,
          float gapFactor = 1f)
        {
            if (FreeTypeHelper.lib == null)
                FreeTypeHelper.lib = new Library();
            this.baseX = this.baseY = 0.0f;
            string str = AppDomain.CurrentDomain.BaseDirectory + "fonts\\" + fontName;
            this.List.Add((IEntity)new Point()
            {
                IsVisible = false,
                IsMarkerable = false,
                Location = new Vector2(0.0f, 0.0f)
            });
            using (Face face = new Face(FreeTypeHelper.lib, str))
            {
                foreach (char ch in text)
                {
                    uint charIndex = face.GetCharIndex((uint)ch);
                    face.SetCharSize(new Fixed26Dot6(width * 1000f), new Fixed26Dot6(height * 1000f), 96U, 96U);
                    face.LoadGlyph(charIndex, (LoadFlags)8, (LoadTarget)262144);
                    this.vCurrent = Vector2.Zero;
                    // ISSUE: method pointer
                    // ISSUE: method pointer
                    // ISSUE: method pointer
                    // ISSUE: method pointer
                    //using (OutlineFuncs outlineFuncs = new OutlineFuncs(new MoveToFunc(, MyMoveToFunc), new LineToFunc((object)this, __methodptr(MyLineToFunc)), new ConicToFunc((object)this, __methodptr(MyConicToFunc)), new CubicToFunc((object)this, __methodptr(MyCubicToFunc)), 0, 0))
                    //    face.Glyph.Outline.Decompose(outlineFuncs, IntPtr.Zero);
                    if (this.polyline != null)
                    {
                        if (this.polyline.Count > 2 && MathHelper.IsEqual(this.vCurrent.X + this.baseX, this.polyline[this.polyline.Count - 1].X) && MathHelper.IsEqual(this.vCurrent.Y + this.baseY, this.polyline[this.polyline.Count - 1].Y))
                        {
                            this.polyline.RemoveAt(this.polyline.Count - 1);
                            this.polyline.IsClosed = true;
                        }
                        this.List.Add((IEntity)this.polyline);
                        this.polyline = (LwPolyline)null;
                    }
                    double baseX = (double)this.baseX;
                    FTVector26Dot6 advance = face.Glyph.Advance;
                    Fixed26Dot6 x = ((FTVector26Dot6)advance).X;
                    double num1 = (double)((Fixed26Dot6)x).ToSingle() / 1000.0;
                    this.baseX = (float)(baseX + num1);
                    double baseY = (double)this.baseY;
                    advance = face.Glyph.Advance;
                    Fixed26Dot6 y = ((FTVector26Dot6)advance).Y;
                    double num2 = (double)((Fixed26Dot6)y).ToSingle() / 1000.0;
                    this.baseY = (float)(baseY + num2);
                    this.baseX += width * gapFactor - width;
                    Fixed26Dot6 horizontalBearingY = face.Glyph.Metrics.HorizontalBearingY;
                    double num3 = (double)((Fixed26Dot6)horizontalBearingY).ToSingle() / 1000.0;
                    Fixed26Dot6 verticalBearingY = face.Glyph.Metrics.VerticalBearingY;
                    double num4 = -(double)((Fixed26Dot6)verticalBearingY).ToSingle() / 1000.0;
                    this.List.Add((IEntity)new Point()
                    {
                        IsVisible = false,
                        IsMarkerable = false,
                        Location = new Vector2(this.baseX, this.baseY)
                    });
                }
            }
        }

        private int MyMoveToFunc(ref FTVector to, IntPtr user)
        {
            if (this.polyline != null)
            {
                if (this.polyline.Count > 2)
                {
                    double num1 = (double)this.vCurrent.X + (double)this.baseX;
                    LwPolyLineVertex lwPolyLineVertex = this.polyline[this.polyline.Count - 1];
                    double x = (double)lwPolyLineVertex.X;
                    if (MathHelper.IsEqual((float)num1, (float)x))
                    {
                        double num2 = (double)this.vCurrent.Y + (double)this.baseY;
                        lwPolyLineVertex = this.polyline[this.polyline.Count - 1];
                        double y = (double)lwPolyLineVertex.Y;
                        if (MathHelper.IsEqual((float)num2, (float)y))
                        {
                            this.polyline.RemoveAt(this.polyline.Count - 1);
                            this.polyline.IsClosed = true;
                        }
                    }
                }
                this.List.Add((IEntity)this.polyline);
                this.polyline = (LwPolyline)null;
            }
            this.polyline = new LwPolyline();
            LwPolyline polyline = this.polyline;
            Fixed16Dot16 fixed16Dot16_1 = ((FTVector)to).X;
            double num3 = (double)((Fixed16Dot16)fixed16Dot16_1).ToSingle() + (double)this.baseX;
            fixed16Dot16_1 = ((FTVector)to).Y;
            double num4 = (double)((Fixed16Dot16)fixed16Dot16_1).ToSingle() + (double)this.baseY;
            LwPolyLineVertex lwPolyLineVertex1 = new LwPolyLineVertex((float)num3, (float)num4);
            polyline.Add(lwPolyLineVertex1);
            Fixed16Dot16 fixed16Dot16_2 = ((FTVector)to).X;
            double single1 = (double)((Fixed16Dot16)fixed16Dot16_2).ToSingle();
            fixed16Dot16_2 = ((FTVector)to).Y;
            double single2 = (double)((Fixed16Dot16)fixed16Dot16_2).ToSingle();
            this.vCurrent = new Vector2((float)single1, (float)single2);
            return 0;
        }

        private int MyLineToFunc(ref FTVector to, IntPtr user)
        {
            double x1 = (double)this.vCurrent.X;
            Fixed16Dot16 x2 = ((FTVector)to).X;
            double single1 = (double)((Fixed16Dot16)x2).ToSingle();
            if (MathHelper.IsEqual((float)x1, (float)single1))
            {
                double y1 = (double)this.vCurrent.Y;
                Fixed16Dot16 y2 = ((FTVector)to).Y;
                double single2 = (double)((Fixed16Dot16)y2).ToSingle();
                if (MathHelper.IsEqual((float)y1, (float)single2))
                    return 0;
            }
            Fixed16Dot16 fixed16Dot16;
            if (this.polyline.Count == 2)
            {
                double x3 = (double)this.polyline[0].X;
                fixed16Dot16 = ((FTVector)to).X;
                double num1 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() + (double)this.baseX;
                if (MathHelper.IsEqual((float)x3, (float)num1))
                {
                    double y = (double)this.polyline[0].Y;
                    fixed16Dot16 = ((FTVector)to).Y;
                    double num2 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() + (double)this.baseY;
                    if (MathHelper.IsEqual((float)y, (float)num2))
                        return 0;
                }
            }
            LwPolyline polyline = this.polyline;
            fixed16Dot16 = ((FTVector)to).X;
            double num3 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() + (double)this.baseX;
            fixed16Dot16 = ((FTVector)to).Y;
            double num4 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() + (double)this.baseY;
            LwPolyLineVertex lwPolyLineVertex = new LwPolyLineVertex((float)num3, (float)num4);
            polyline.Add(lwPolyLineVertex);
            fixed16Dot16 = ((FTVector)to).X;
            double single3 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle();
            fixed16Dot16 = ((FTVector)to).Y;
            double single4 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle();
            this.vCurrent = new Vector2((float)single3, (float)single4);
            return 0;
        }

        private int MyConicToFunc(ref FTVector control, ref FTVector to, IntPtr user)
        {
            Fixed16Dot16 x1 = ((FTVector)to).X;
            float single1 = ((Fixed16Dot16)x1).ToSingle();
            Fixed16Dot16 y1 = ((FTVector)to).Y;
            float single2 = ((Fixed16Dot16)y1).ToSingle();
            if ((double)Vector2.Distance(this.vCurrent, new Vector2(single1, single2)) > (double)Config.BezierSplineMicroStepDistance)
            {
                float num1 = 4f;
                float num2 = 1f / num1;
                float t = 0.0f;
                for (int index = 0; (double)index < (double)num1 - 1.0; ++index)
                {
                    double num3 = (double)this.vCurrent.X * (double)this.CONIC_B1(t);
                    Fixed16Dot16 x2 = ((FTVector)control).X;
                    double num4 = (double)((Fixed16Dot16)x2).ToSingle() * (double)this.CONIC_B2(t);
                    double num5 = num3 + num4;
                    Fixed16Dot16 x3 = ((FTVector)to).X;
                    double num6 = (double)((Fixed16Dot16)x3).ToSingle() * (double)this.CONIC_B3(t);
                    float num7 = (float)(num5 + num6);
                    double num8 = (double)this.vCurrent.Y * (double)this.CONIC_B1(t);
                    Fixed16Dot16 y2 = ((FTVector)control).Y;
                    double num9 = (double)((Fixed16Dot16)y2).ToSingle() * (double)this.CONIC_B2(t);
                    double num10 = num8 + num9;
                    Fixed16Dot16 y3 = ((FTVector)to).Y;
                    double num11 = (double)((Fixed16Dot16)y3).ToSingle() * (double)this.CONIC_B3(t);
                    float num12 = (float)(num10 + num11);
                    this.polyline.Add(new LwPolyLineVertex(num7 + this.baseX, num12 + this.baseY));
                    t += num2;
                }
            }
            Fixed16Dot16 x4 = ((FTVector)to).X;
            float single3 = ((Fixed16Dot16)x4).ToSingle();
            Fixed16Dot16 y4 = ((FTVector)to).Y;
            float single4 = ((Fixed16Dot16)y4).ToSingle();
            this.polyline.Add(new LwPolyLineVertex(single3 + this.baseX, single4 + this.baseY));
            this.vCurrent = new Vector2(single3, single4);
            return 0;
        }

        private float CONIC_B1(float t) => (float)((1.0 - (double)t) * (1.0 - (double)t));

        private float CONIC_B2(float t) => (float)(2.0 * (double)t * (1.0 - (double)t));

        private float CONIC_B3(float t) => t * t;

        private int MyCubicToFunc(
          ref FTVector control1,
          ref FTVector control2,
          ref FTVector to,
          IntPtr user)
        {
            Fixed16Dot16 x = ((FTVector)to).X;
            float single1 = ((Fixed16Dot16)x).ToSingle();
            Fixed16Dot16 y = ((FTVector)to).Y;
            float single2 = ((Fixed16Dot16)y).ToSingle();
            Fixed16Dot16 fixed16Dot16;
            if ((double)Vector2.Distance(this.vCurrent, new Vector2(single1, single2)) > (double)Config.BezierSplineMicroStepDistance)
            {
                float num1 = 5f;
                float num2 = 1f / num1;
                float t = 0.0f;
                for (int index = 0; (double)index < (double)num1 - 1.0; ++index)
                {
                    double num3 = (double)this.vCurrent.X * (double)this.CUBIC_B1(t);
                    fixed16Dot16 = ((FTVector)control1).X;
                    double num4 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() * (double)this.CUBIC_B2(t);
                    double num5 = num3 + num4;
                    fixed16Dot16 = ((FTVector)control2).X;
                    double num6 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() * (double)this.CUBIC_B3(t);
                    double num7 = num5 + num6;
                    fixed16Dot16 = ((FTVector)to).X;
                    double num8 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() * (double)this.CUBIC_B4(t);
                    float num9 = (float)(num7 + num8);
                    double num10 = (double)this.vCurrent.Y * (double)this.CUBIC_B1(t);
                    fixed16Dot16 = ((FTVector)control1).Y;
                    double num11 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() * (double)this.CUBIC_B2(t);
                    double num12 = num10 + num11;
                    fixed16Dot16 = ((FTVector)control2).Y;
                    double num13 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() * (double)this.CUBIC_B3(t);
                    double num14 = num12 + num13;
                    fixed16Dot16 = ((FTVector)to).Y;
                    double num15 = (double)((Fixed16Dot16)fixed16Dot16).ToSingle() * (double)this.CUBIC_B4(t);
                    float num16 = (float)(num14 + num15);
                    this.polyline.Add(new LwPolyLineVertex(num9 + this.baseX, num16 + this.baseY));
                    t += num2;
                }
            }
            fixed16Dot16 = ((FTVector)to).X;
            float single3 = ((Fixed16Dot16)fixed16Dot16).ToSingle();
            fixed16Dot16 = ((FTVector)to).Y;
            float single4 = ((Fixed16Dot16)fixed16Dot16).ToSingle();
            this.polyline.Add(new LwPolyLineVertex(single3 + this.baseX, single4 + this.baseY));
            this.vCurrent = new Vector2(single3, single4);
            return 0;
        }

        private float CUBIC_B1(float t) => (float)((1.0 - (double)t) * (1.0 - (double)t) * (1.0 - (double)t));

        private float CUBIC_B2(float t) => (float)(3.0 * (double)t * (1.0 - (double)t) * (1.0 - (double)t));

        private float CUBIC_B3(float t) => (float)(3.0 * (double)t * (double)t * (1.0 - (double)t));

        private float CUBIC_B4(float t) => t * t * t;
    }
}
