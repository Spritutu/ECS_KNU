
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Scanlab.Sirius
{
    /// <summary>cxf 폰트 디렉토리 목록 변환기</summary>
    internal class SiriusFontStringConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override TypeConverter.StandardValuesCollection GetStandardValues(
          ITypeDescriptorContext context)
        {
            IEnumerable<string> strings = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory + "siriusfonts", "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>)(s => s.EndsWith(".cxf")));
            List<string> stringList = new List<string>();
            foreach (string path in strings)
                stringList.Add(Path.GetFileName(path));
            return new TypeConverter.StandardValuesCollection((ICollection)stringList);
        }

        public static string GetFirst() => Path.GetFileName(Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory + "siriusfonts", "*.*", SearchOption.AllDirectories).Where<string>((Func<string, bool>)(s => s.EndsWith(".cxf"))).ElementAt<string>(0));
    }
}
