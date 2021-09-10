// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.StitchedImage
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>stitched entity</summary>
    public class StitchedImage : IEntity, IDrawable, ICloneable, IDisposable
    {
        [JsonIgnore]
        [Browsable(false)]
        internal Dictionary<IView, Texture[]> Views = new Dictionary<IView, Texture[]>();
        private string name;
        private bool isVisible;
        private bool isLocked;
        private bool isHitTest;
        private Alignment align;
        private Vector2 location;
        private float width;
        private float height;
        private int cols;
        private int rows;
        private System.Drawing.Bitmap[] images;
        private byte transparancy;
        private float angle;
        private int imageIndex;
        private string imaageFileName;
        private bool isRegen;
        private int regenIndex = -1;
        private bool disposed;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.StitchedImage;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("엔티티의 이름")]
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("엔티티에 대한 설명")]
        public string Description { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public AciColor Color { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Color")]
        [System.ComponentModel.Description("색상")]
        public System.Drawing.Color Color2 { get; set; } = Config.DefaultColor;

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Bound")]
        [System.ComponentModel.Description("외각 영역")]
        public BoundRect BoundRect { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Selected")]
        [System.ComponentModel.Description("선택여부")]
        public bool IsSelected { get; set; }

        [Browsable(false)]
        public bool IsHighlighted { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Visible")]
        [System.ComponentModel.Description("스크린에 출력 여부")]
        public bool IsVisible
        {
            get => this.isVisible;
            set => this.isVisible = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Mark Path")]
        [System.ComponentModel.Description("가공 경로를 표시")]
        public bool IsDrawPath { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Locked")]
        [System.ComponentModel.Description("편집 금지 여부")]
        public bool IsLocked
        {
            get => this.isLocked;
            set => this.isLocked = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Hit Test")]
        [System.ComponentModel.Description("마우스 선택 기능")]
        public bool IsHitTest
        {
            get => this.isHitTest;
            set => this.isHitTest = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Align")]
        [System.ComponentModel.Description("정렬 기준위치")]
        public Alignment Align
        {
            get => this.align;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null && this.align != value)
                    this.location = this.BoundRect.LocationByAlign(value);
                this.align = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Location")]
        [System.ComponentModel.Description("위치 좌표값")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Location
        {
            get => this.location;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                Vector2 delta = Vector2.Subtract(value, this.location);
                if (this.Owner != null)
                    this.Transit(delta);
                this.location = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("폭")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Width
        {
            get => this.width;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.width = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Height")]
        [System.ComponentModel.Description("높이")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Height
        {
            get => this.height;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.height = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Cols")]
        [System.ComponentModel.Description("열 개수")]
        public int Cols
        {
            get => this.cols;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.cols = value;
                this.Clear();
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Rows")]
        [System.ComponentModel.Description("행 개수")]
        public int Rows
        {
            get => this.rows;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.rows = value;
                this.Clear();
            }
        }

        /// <summary>
        /// 좌 상단 부터 인덱스 번호 시작
        /// 0 1 2 3 4
        /// 5 6 7 8 9
        /// ...
        /// </summary>
        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Images")]
        [System.ComponentModel.Description("이미지 배열")]
        public System.Drawing.Bitmap[] Images
        {
            get => this.images;
            private set => this.images = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Transparent")]
        [System.ComponentModel.Description("이미지의 투명도 (0~255)")]
        public byte Transparancy
        {
            get => this.transparancy;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.transparancy = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Angle")]
        [System.ComponentModel.Description("회전 각도")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Angle
        {
            get => this.angle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                float angle = value - this.angle;
                if (this.Owner != null)
                    this.Rotate(angle);
                this.angle = value;
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Image Index")]
        [System.ComponentModel.Description("업데이트할 이미지의 인덱스 번호 (0:좌상단)")]
        public int ImageIndex
        {
            get => this.imageIndex;
            set
            {
                if (this.rows * this.cols <= value)
                    return;
                this.imageIndex = value;
                this.imaageFileName = string.Empty;
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Image File")]
        [System.ComponentModel.Description("지정된 인덱스 위치에 업데이트할 이미지 파일")]
        [Editor(typeof(ImageFileBrowser), typeof(UITypeEditor))]
        public string ImageFileName
        {
            get => this.imaageFileName;
            set
            {
                if (this.Owner != null && this.isLocked || (value == null || !File.Exists(value)))
                    return;
                this.imaageFileName = value;
                this.UpdateImage(this.ImageIndex, this.imaageFileName);
                ++this.ImageIndex;
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public object Tag { get; set; }

        public override string ToString() => string.Format("{0}: {1:F3}x{2:F3}", (object)this.Name, (object)this.Width, (object)this.Height);

        public StitchedImage()
        {
            this.Node = new TreeNode();
            this.Name = "Stitched Image";
            this.IsSelected = false;
            this.isVisible = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.location = Vector2.Zero;
            this.align = Alignment.Center;
            this.cols = 12;
            this.rows = 9;
            this.width = 72f;
            this.height = 36f;
            this.isRegen = true;
            this.transparancy = byte.MaxValue;
            this.isHitTest = true;
            this.Clear();
        }

        public StitchedImage(int cols, int rows, float width, float height)
          : this()
        {
            this.Width = width;
            this.Height = height;
            this.Rows = rows;
            this.Cols = cols;
        }

        ~StitchedImage()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                foreach (KeyValuePair<IView, Texture[]> view in this.Views)
                {
                    OpenGL renderer = (view.Key as ViewDefault).Renderer;
                    foreach (Texture texture in view.Value)
                        texture?.Destroy(renderer);
                }
                foreach (System.Drawing.Bitmap image in this.Images)
                    image?.Dispose();
            }
            this.disposed = true;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone()
        {
            StitchedImage stitchedImage = new StitchedImage()
            {
                Name = this.Name,
                Description = this.Description,
                Owner = this.Owner,
                IsSelected = this.IsSelected,
                IsHighlighted = this.IsHighlighted,
                isVisible = this.isVisible,
                isLocked = this.IsLocked,
                isHitTest = this.IsHitTest,
                Color2 = this.Color2,
                BoundRect = this.BoundRect.Clone(),
                align = this.align,
                location = this.location,
                width = this.width,
                height = this.height,
                rows = this.rows,
                cols = this.cols,
                Angle = this.Angle,
                Transparancy = this.transparancy,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            if (this.Images != null)
            {
                stitchedImage.Images = new System.Drawing.Bitmap[this.Images.Length];
                for (int index = 0; index < this.Images.Length; ++index)
                {
                    if (this.Images[index] != null)
                        stitchedImage.Images[index] = (System.Drawing.Bitmap)this.Images[index].Clone();
                }
            }
            return (object)stitchedImage;
        }

        /// <summary>모든 인덱스의 이미지 삭제</summary>
        public void Clear()
        {
            int length = this.rows * this.cols;
            if (this.Images != null)
            {
                foreach (System.Drawing.Bitmap image in this.Images)
                    image?.Dispose();
            }
            this.Images = new System.Drawing.Bitmap[length];
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                OpenGL renderer = key.Renderer;
                Texture[] view = this.Views[key];
                if (view != null)
                {
                    foreach (Texture texture in view)
                        texture?.Destroy(renderer);
                }
                this.Views[key] = new Texture[length];
            }
            this.isRegen = true;
        }

        /// <summary>인덱스 위치에 이미지 로드</summary>
        /// <param name="index">좌 상단 0</param>
        /// <param name="fileName">파일이름</param>
        /// <returns></returns>
        public bool UpdateImage(int index, string fileName)
        {
            this.Images[index]?.Dispose();
            this.Images[index] = BitmapHelper.ConvertToBitmap(fileName);
            this.regenIndex = index;
            this.RegenVertextList();
            this.regenIndex = -1;
            return true;
        }

        /// <summary>인덱스 위치에 이미지 로드</summary>
        /// <param name="index">좌 상단 0</param>
        /// <param name="bytes">바이트 배열</param>
        /// <returns></returns>
        public bool UpdateImage(int index, byte[] bytes)
        {
            this.Images[index]?.Dispose();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                this.Images[index] = new System.Drawing.Bitmap((Stream)memoryStream);
            }
            this.regenIndex = index;
            this.RegenVertextList();
            this.regenIndex = -1;
            return true;
        }

        /// <summary>인덱스 위치에 이미지 로드</summary>
        /// <param name="index">좌 상단 0</param>
        /// <param name="stream">스트림</param>
        /// <returns></returns>
        public bool UpdateImage(int index, Stream stream)
        {
            byte[] numArray = new byte[10485760];
            stream.Read(numArray, 0, numArray.Length);
            return this.UpdateImage(index, numArray);
        }

        private void RegenVertextList()
        {
            if (this.regenIndex < 0)
                return;
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                OpenGL renderer = key.Renderer;
                renderer.Enable(3553U);
                Texture[] textureArray = this.Views[key];
                if (textureArray == null || textureArray.Length != this.Images.Length)
                    textureArray = new Texture[this.Images.Length];
                if (this.Images[this.regenIndex] != null)
                {
                    System.Drawing.Bitmap image = (System.Drawing.Bitmap)this.Images[this.regenIndex].Clone();
                    textureArray[this.regenIndex]?.Destroy(renderer);
                    Texture texture = new Texture();
                    texture.Create(renderer, image);
                    textureArray[this.regenIndex] = texture;
                    renderer.Disable(3553U);
                    this.Views[key] = textureArray;
                }
            }
            this.regenIndex = -1;
        }

        private void RegenBoundRect()
        {
            Rectangle rectangle = new Rectangle(0.0f, 0.0f, this.width, this.height);
            rectangle.Owner = (IEntity)this;
            rectangle.Regen();
            rectangle.Align = this.Align;
            rectangle.Location = this.Location;
            rectangle.Rotate(this.Angle);
            this.BoundRect = rectangle.BoundRect.Clone();
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.isRegen = false;
        }

        private Vector2 TransitByAlign(Alignment target)
        {
            switch (this.align)
            {
                case Alignment.LeftTop:
                    return new Vector2(0.0f, -this.height);
                case Alignment.MiddleTop:
                    return new Vector2((float)(-(double)this.width / 2.0), -this.height);
                case Alignment.RightTop:
                    return new Vector2(-this.width, -this.height);
                case Alignment.LeftMiddle:
                    return new Vector2(0.0f, (float)(-(double)this.height / 2.0));
                case Alignment.Center:
                    return new Vector2((float)(-(double)this.width / 2.0), (float)(-(double)this.height / 2.0));
                case Alignment.RightMiddle:
                    return new Vector2(-this.width, (float)(-(double)this.height / 2.0));
                case Alignment.LeftBottom:
                    return Vector2.Zero;
                case Alignment.MiddleBottom:
                    return new Vector2((float)(-(double)this.width / 2.0), 0.0f);
                case Alignment.RightBottom:
                    return new Vector2(-this.width, 0.0f);
                default:
                    throw new InvalidOperationException("invalid alignment value !");
            }
        }

        public bool Draw(IView v)
        {
            if (!this.Views.ContainsKey(v))
            {
                this.Views.Add(v, (Texture[])null);
                this.isRegen = true;
            }
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            float num1 = this.Width / (float)this.cols;
            float num2 = this.Height / (float)this.rows;
            foreach (IView key in this.Views.Keys)
            {
                OpenGL renderer = key.Renderer;
                OpenGL openGl = renderer;
                System.Drawing.Color color2 = this.Color2;
                int r = (int)color2.R;
                color2 = this.Color2;
                int g = (int)color2.G;
                color2 = this.Color2;
                int b = (int)color2.B;
                int transparancy = (int)this.Transparancy;
                openGl.Color((byte)r, (byte)g, (byte)b, (byte)transparancy);
                renderer.Enable(3553U);
                Texture[] view = this.Views[key];
                if (view != null)
                {
                    for (int index = 0; index < view.Length; ++index)
                    {
                        Texture texture = view[index];
                        if (texture != null)
                        {
                            renderer.PushMatrix();
                            renderer.Translate(this.location.X, this.location.Y, -0.2f);
                            renderer.Rotate(0.0f, 0.0f, this.angle);
                            Vector2 vector2 = this.TransitByAlign(this.align);
                            renderer.Translate(vector2.X, vector2.Y, 0.0f);
                            texture.Bind(renderer);
                            renderer.TexParameter(3553U, 10241U, 9728f);
                            renderer.TexParameter(3553U, 10240U, 9728f);
                            renderer.Begin(7U);
                            float left;
                            float bottom;
                            this.GetCellIndexPosition(index, out left, out bottom);
                            renderer.TexCoord(0.0f, 0.0f);
                            renderer.Vertex(left, bottom + num2);
                            renderer.TexCoord(1f, 0.0f);
                            renderer.Vertex(left + num1, bottom + num2);
                            renderer.TexCoord(1f, 1f);
                            renderer.Vertex(left + num1, bottom);
                            renderer.TexCoord(0.0f, 1f);
                            renderer.Vertex(left, bottom);
                            renderer.End();
                            renderer.PopMatrix();
                        }
                    }
                }
                renderer.Disable(3553U);
            }
            if (this.IsSelected)
            {
                OpenGL renderer = (v as ViewDefault).Renderer;
                renderer.Color(Config.EntitySelectedColor);
                for (int index = 0; index < this.Images.Length; ++index)
                {
                    renderer.PushMatrix();
                    renderer.Translate(this.location.X, this.location.Y, 0.0f);
                    renderer.Rotate(0.0f, 0.0f, this.angle);
                    Vector2 vector2 = this.TransitByAlign(this.align);
                    renderer.Translate(vector2.X, vector2.Y + this.Height, 0.0f);
                    renderer.Scale(1f, -1f, 1f);
                    renderer.Begin(2U);
                    float left;
                    float bottom;
                    this.GetCellIndexPosition(index, out left, out bottom);
                    renderer.Vertex(left, bottom);
                    renderer.Vertex(left + num1, bottom);
                    renderer.Vertex(left + num1, bottom + num2);
                    renderer.Vertex(left, bottom + num2);
                    renderer.End();
                    renderer.PopMatrix();
                }
            }
            return true;
        }

        private void GetCellIndexPosition(int index, out float left, out float bottom)
        {
            float num1 = this.Width / (float)this.cols;
            float num2 = this.Height / (float)this.rows;
            int num3 = index / this.cols;
            int num4 = index % this.cols;
            left = num1 * (float)num4;
            bottom = this.Height - num2 * (float)(num3 + 1);
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.location = Vector2.Add(this.location, delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.width *= scale.X;
            this.height *= scale.Y;
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.location = (this.location - scaleCenter) * scale + scaleCenter;
            this.width *= scale.X;
            this.height *= scale.Y;
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold) => this.IsVisible && this.IsHitTest && this.BoundRect.HitTest(x, y, threshold);

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.IsHitTest && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold) => this.IsVisible && this.IsHitTest && this.BoundRect.HitTest(br, threshold);
    }
}
