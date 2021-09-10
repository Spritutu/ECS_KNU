using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Scanlab.Sirius
{
    /// <summary>Write Data Ext16 의 목록 변환기</summary>
    internal class WriteDataExt16StringConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override TypeConverter.StandardValuesCollection GetStandardValues(
          ITypeDescriptorContext context)
        {
            List<string> stringList = new List<string>();
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "extio.ini");
            for (int index = 0; index < 16; ++index)
                stringList.Add(NativeMethods.ReadIni<string>(fileName, "DOUT", string.Format("{0}", (object)index)));
            return new TypeConverter.StandardValuesCollection((ICollection)stringList);
        }

        public static string GetFirst() => NativeMethods.ReadIni<string>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "extio.ini"), "DOUT", "0");
    }
}
