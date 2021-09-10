
using System;
using System.Drawing;
using System.IO;

namespace Scanlab.Sirius
{
    /// <summary>비트맵 유틸리티</summary>
    internal static class BitmapHelper
    {
        /// <summary>인코딩 : Bitmap 이미지 데이타 -&gt; base64 문자열 포맷</summary>
        /// <param name="bitmap">비트맵 이미지</param>
        /// <returns>압축후 Base64로 인코딩된 문자열</returns>
        public static string EncodeTo(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
                return string.Empty;
            byte[] inArray = (byte[])new ImageConverter().ConvertTo((object)bitmap, typeof(byte[]));
            string empty = string.Empty;
            return Convert.ToBase64String(inArray);
        }

        /// <summary>base64 문자열 -&gt; Bitmap 이미지 생성</summary>
        /// <param name="base64String">압축후 Base64로 인코딩된 문자열</param>
        /// <returns>비트맵 이미지</returns>
        public static System.Drawing.Bitmap DecodeFrom(string base64String)
        {
            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)null;
            if (string.IsNullOrEmpty(base64String))
                return bitmap;
            byte[] buffer = Convert.FromBase64String(base64String);
            if (buffer.Length != 0)
            {
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                    bitmap = new System.Drawing.Bitmap(Image.FromStream((Stream)memoryStream));
            }
            return bitmap;
        }

        /// <summary>지정된 파일을 Bitmap 이미지로 변경</summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ConvertToBitmap(string fileName)
        {
            using (Stream stream = (Stream)File.Open(fileName, FileMode.Open))
                return new System.Drawing.Bitmap(Image.FromStream(stream));
        }

        /// <summary>지정된 스트림을 Bitmap 이미지로 변경</summary>
        /// <param name="stream">스트림</param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ConvertToBitmap(Stream stream) => new System.Drawing.Bitmap(Image.FromStream(stream));
    }
}
