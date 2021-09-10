using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>CXF 폰트 모음을 관리하는 정적 객체</summary>
    internal static class CxfFontCollectionHelper
    {
        internal static Dictionary<string, CxfHelper> Collection = new Dictionary<string, CxfHelper>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);

        public static CxfHelper Instance(string fontFileName)
        {
            if (CxfFontCollectionHelper.Collection.ContainsKey(fontFileName))
                return CxfFontCollectionHelper.Collection[fontFileName];
            CxfHelper cxfHelper = new CxfHelper(fontFileName);
            CxfFontCollectionHelper.Collection.Add(fontFileName, cxfHelper);
            return cxfHelper;
        }
    }
}
