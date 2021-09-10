using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Scanlab.Sirius
{
    internal class CxfHelper
    {
        public Dictionary<int, Group> Face = new Dictionary<int, Group>();

        public string FileName { get; private set; }

        /// <summary>대문자 높이</summary>
        public float CapitalHeight
        {
            get
            {
                BoundRect boundRect = new BoundRect();
                if (this.Face.ContainsKey(65))
                {
                    Group group = this.Face[65];
                    boundRect.Union(group.BoundRect);
                }
                if (this.Face.ContainsKey(87))
                {
                    Group group = this.Face[87];
                    boundRect.Union(group.BoundRect);
                }
                return boundRect.Height;
            }
        }

        /// <summary>소문자 높이</summary>
        public float XHeight
        {
            get
            {
                float num = 0.0f;
                char ch = 'x';
                if (this.Face.ContainsKey((int)ch))
                    num = this.Face[(int)ch].BoundRect.Height;
                return num;
            }
        }

        /// <summary>
        /// https://medium.com/edinbed/calarts-funds-of-graphic-design-w2-2-2-words-and-spacing-b0ce031a3c77
        /// </summary>
        public float Ascender => this.BBox.Top - this.CapitalHeight;

        /// <summary>
        /// https://medium.com/edinbed/calarts-funds-of-graphic-design-w2-2-2-words-and-spacing-b0ce031a3c77
        /// </summary>
        public float Descender => Math.Abs(this.BBox.Bottom);

        /// <summary>폰트 파일에 지정된 저자</summary>
        public string Author { get; private set; }

        /// <summary>폰트파일에 지정된 이름</summary>
        public string Name { get; private set; }

        /// <summary>폰트 파일에 지정된 엔코딩 포맷</summary>
        public string Encoding { get; private set; }

        /// <summary>폰트 파일에 지정된 글자간 간격</summary>
        public float LetterSpacing { get; private set; }

        /// <summary>폰트 파일에 지정된 단어간 간격 (like as space bar)</summary>
        public float WordSpacing { get; private set; }

        /// <summary>폰트 파일에 지정된 줄간 간격 (new line space)</summary>
        public float LineSpacing { get; private set; }

        /// <summary>Bound Box</summary>
        public BoundRect BBox { get; private set; }

        public CxfHelper(string fontFileName)
        {
            this.FileName = fontFileName;
            this.BBox = new BoundRect();
            Line line1 = new Line();
            line1.Start = new Vector2(-999f, -999f);
            line1.End = new Vector2(-999f, -999f);
            using (StreamReader streamReader = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "siriusfonts", fontFileName)))
            {
                while (!streamReader.EndOfStream)
                {
                    string source1 = streamReader.ReadLine();
                    if (!string.IsNullOrEmpty(source1))
                    {
                        if ('#' == source1.ElementAt<char>(0))
                        {
                            string[] strArray = source1.Split('#', ':');
                            if (3 == strArray.Length)
                            {
                                string strB = strArray[1].Trim();
                                string s = strArray[2].Trim();
                                if (string.Compare("letterspacing", strB, true) == 0)
                                    this.LetterSpacing = float.Parse(s);
                                else if (string.Compare("wordspacing", strB, true) == 0)
                                    this.WordSpacing = float.Parse(s);
                                else if (string.Compare("linespacingfactor", strB, true) == 0)
                                    this.LineSpacing = float.Parse(s);
                                else if (string.Compare("author", strB, true) == 0)
                                    this.Author = s;
                                else if (string.Compare("name", strB, true) == 0)
                                    this.Name = s;
                                else if (string.Compare("encoding", strB, true) == 0)
                                    this.Encoding = s;
                            }
                        }
                        else if ('[' == source1.ElementAt<char>(0))
                        {
                            int num1 = source1.LastIndexOf(']');
                            if (num1 >= 0)
                            {
                                int int32;
                                if (7 == source1.Length)
                                {
                                    string str = source1.Substring(2, 4);
                                    try
                                    {
                                        int32 = Convert.ToInt32(str, 16);
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                    if (int32 > (int)byte.MaxValue)
                                        continue;
                                }
                                else
                                {
                                    string s = source1.Substring(1, num1 - 1);
                                    try
                                    {
                                        int32 = (int)char.Parse(s);
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                    if (int32 > (int)byte.MaxValue)
                                        continue;
                                }
                                Group group = new Group();
                                group.Align = Alignment.LeftBottom;
                                string source2;
                                do
                                {
                                    source2 = streamReader.ReadLine();
                                    if (string.IsNullOrEmpty(source2))
                                    {
                                        group.Regen();
                                        this.BBox.Union(group.BoundRect);
                                        this.Face.Add(int32, group);
                                        break;
                                    }
                                    if (' ' == source2[source2.Length - 1])
                                        source2 = source2.Remove(source2.Length - 1, 1);
                                    if (source2.ElementAt<char>(0) == 'L')
                                    {
                                        string[] strArray = source2.Split(',', ' ');
                                        float num2 = float.Parse(strArray[1]);
                                        float num3 = float.Parse(strArray[2]);
                                        float num4 = float.Parse(strArray[3]);
                                        float num5 = float.Parse(strArray[4]);
                                        if ((!MathHelper.IsEqual(num2, num4) || !MathHelper.IsEqual(num3, num5)) && (!MathHelper.IsEqual(line1.End.X, num2) || !MathHelper.IsEqual(line1.End.Y, num3) || (!MathHelper.IsEqual(line1.Start.X, num4) || !MathHelper.IsEqual(line1.Start.Y, num5))))
                                        {
                                            Line line2 = new Line();
                                            line2.Start = new Vector2(num2, num3);
                                            line2.End = new Vector2(num4, num5);
                                            group.Add((IEntity)line2);
                                            line1 = line2;
                                        }
                                    }
                                    else if (source2.ElementAt<char>(0) == 'A')
                                    {
                                        string[] strArray = source2.Split(',', ' ');
                                        float num2 = float.Parse(strArray[1]);
                                        float num3 = float.Parse(strArray[2]);
                                        float num4 = float.Parse(strArray[3]);
                                        float num5 = float.Parse(strArray[4]);
                                        float num6 = float.Parse(strArray[5]);
                                        float num7 = (double)num6 > (double)num5 ? num6 - num5 : 360f + num6 - num5;
                                        if (source2.ElementAt<char>(1) == 'R')
                                        {
                                            num5 = float.Parse(strArray[5]);
                                            float num8 = float.Parse(strArray[4]);
                                            num7 = (double)num8 > (double)num5 ? num8 - num5 : 360f + num8 - num5;
                                        }
                                        LwPolyline lwPolyline = new LwPolyline();
                                        double num9 = Math.Cos((double)num5 * (Math.PI / 180.0)) * (double)num4 + (double)num2;
                                        double num10 = Math.Sin((double)num5 * (Math.PI / 180.0)) * (double)num4 + (double)num3;
                                        if ((double)num7 > 0.0)
                                        {
                                            for (double num8 = (double)num5; num8 < (double)num5 + (double)num7; num8 += (double)Config.AngleFactor)
                                            {
                                                double num11 = Math.Cos(num8 * (Math.PI / 180.0)) * (double)num4 + (double)num2;
                                                double num12 = Math.Sin(num8 * (Math.PI / 180.0)) * (double)num4 + (double)num3;
                                                lwPolyline.Add(new LwPolyLineVertex((float)num11, (float)num12));
                                            }
                                        }
                                        else
                                        {
                                            for (double num8 = (double)num5; num8 > (double)num5 + (double)num7; num8 -= (double)Config.AngleFactor)
                                            {
                                                double num11 = Math.Cos(num8 * (Math.PI / 180.0)) * (double)num4 + (double)num2;
                                                double num12 = Math.Sin(num8 * (Math.PI / 180.0)) * (double)num4 + (double)num3;
                                                lwPolyline.Add(new LwPolyLineVertex((float)num11, (float)num12));
                                            }
                                        }
                                        double num13 = Math.Cos(((double)num5 + (double)num7) * (Math.PI / 180.0)) * (double)num4 + (double)num2;
                                        double num14 = Math.Sin(((double)num5 + (double)num7) * (Math.PI / 180.0)) * (double)num4 + (double)num3;
                                        lwPolyline.Add(new LwPolyLineVertex((float)num13, (float)num14));
                                        lwPolyline.Regen();
                                        lwPolyline.Align = Alignment.LeftBottom;
                                        group.Add((IEntity)lwPolyline);
                                    }
                                }
                                while (!string.IsNullOrEmpty(source2));
                            }
                        }
                    }
                }
            }
        }
    }
}
