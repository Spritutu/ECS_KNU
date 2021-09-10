// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.AciColor
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 색상 객체
    /// AutoCad 의 색상 객체를 표현 (ACI color - AutoCAD Color Index)
    /// </summary>
    [Obsolete("AciColor 클래스는 이제 더이상 지원되지 않습니다")]
    public class AciColor : ICloneable, IEquatable<AciColor>
    {
        private static readonly IReadOnlyDictionary<byte, byte[]> indexRgb = (IReadOnlyDictionary<byte, byte[]>)new Dictionary<byte, byte[]>()
    {
      {
        (byte) 1,
        new byte[3]{ byte.MaxValue, (byte) 0, (byte) 0 }
      },
      {
        (byte) 2,
        new byte[3]{ byte.MaxValue, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 3,
        new byte[3]{ (byte) 0, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 4,
        new byte[3]{ (byte) 0, byte.MaxValue, byte.MaxValue }
      },
      {
        (byte) 5,
        new byte[3]{ (byte) 0, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 6,
        new byte[3]{ byte.MaxValue, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 7,
        new byte[3]{ byte.MaxValue, byte.MaxValue, byte.MaxValue }
      },
      {
        (byte) 8,
        new byte[3]{ (byte) 128, (byte) 128, (byte) 128 }
      },
      {
        (byte) 9,
        new byte[3]{ (byte) 192, (byte) 192, (byte) 192 }
      },
      {
        (byte) 10,
        new byte[3]{ byte.MaxValue, (byte) 0, (byte) 0 }
      },
      {
        (byte) 11,
        new byte[3]{ byte.MaxValue, (byte) 127, (byte) 127 }
      },
      {
        (byte) 12,
        new byte[3]{ (byte) 165, (byte) 0, (byte) 0 }
      },
      {
        (byte) 13,
        new byte[3]{ (byte) 165, (byte) 82, (byte) 82 }
      },
      {
        (byte) 14,
        new byte[3]{ (byte) 127, (byte) 0, (byte) 0 }
      },
      {
        (byte) 15,
        new byte[3]{ (byte) 127, (byte) 63, (byte) 63 }
      },
      {
        (byte) 16,
        new byte[3]{ (byte) 76, (byte) 0, (byte) 0 }
      },
      {
        (byte) 17,
        new byte[3]{ (byte) 76, (byte) 38, (byte) 38 }
      },
      {
        (byte) 19,
        new byte[3]{ (byte) 38, (byte) 19, (byte) 19 }
      },
      {
        (byte) 20,
        new byte[3]{ byte.MaxValue, (byte) 63, (byte) 0 }
      },
      {
        (byte) 21,
        new byte[3]{ byte.MaxValue, (byte) 159, (byte) 127 }
      },
      {
        (byte) 22,
        new byte[3]{ (byte) 165, (byte) 41, (byte) 0 }
      },
      {
        (byte) 23,
        new byte[3]{ (byte) 165, (byte) 103, (byte) 82 }
      },
      {
        (byte) 24,
        new byte[3]{ (byte) 127, (byte) 31, (byte) 0 }
      },
      {
        (byte) 25,
        new byte[3]{ (byte) 127, (byte) 79, (byte) 63 }
      },
      {
        (byte) 26,
        new byte[3]{ (byte) 76, (byte) 19, (byte) 0 }
      },
      {
        (byte) 27,
        new byte[3]{ (byte) 76, (byte) 47, (byte) 38 }
      },
      {
        (byte) 28,
        new byte[3]{ (byte) 38, (byte) 9, (byte) 0 }
      },
      {
        (byte) 29,
        new byte[3]{ (byte) 38, (byte) 23, (byte) 19 }
      },
      {
        (byte) 30,
        new byte[3]{ byte.MaxValue, (byte) 127, (byte) 0 }
      },
      {
        (byte) 31,
        new byte[3]{ byte.MaxValue, (byte) 191, (byte) 127 }
      },
      {
        (byte) 32,
        new byte[3]{ (byte) 165, (byte) 82, (byte) 0 }
      },
      {
        (byte) 33,
        new byte[3]{ (byte) 165, (byte) 124, (byte) 82 }
      },
      {
        (byte) 34,
        new byte[3]{ (byte) 127, (byte) 63, (byte) 0 }
      },
      {
        (byte) 35,
        new byte[3]{ (byte) 127, (byte) 95, (byte) 63 }
      },
      {
        (byte) 36,
        new byte[3]{ (byte) 76, (byte) 38, (byte) 0 }
      },
      {
        (byte) 37,
        new byte[3]{ (byte) 76, (byte) 57, (byte) 38 }
      },
      {
        (byte) 38,
        new byte[3]{ (byte) 38, (byte) 19, (byte) 0 }
      },
      {
        (byte) 39,
        new byte[3]{ (byte) 38, (byte) 28, (byte) 19 }
      },
      {
        (byte) 40,
        new byte[3]{ byte.MaxValue, (byte) 191, (byte) 0 }
      },
      {
        (byte) 41,
        new byte[3]{ byte.MaxValue, (byte) 223, (byte) 127 }
      },
      {
        (byte) 42,
        new byte[3]{ (byte) 165, (byte) 124, (byte) 0 }
      },
      {
        (byte) 43,
        new byte[3]{ (byte) 165, (byte) 145, (byte) 82 }
      },
      {
        (byte) 44,
        new byte[3]{ (byte) 127, (byte) 95, (byte) 0 }
      },
      {
        (byte) 45,
        new byte[3]{ (byte) 127, (byte) 111, (byte) 63 }
      },
      {
        (byte) 46,
        new byte[3]{ (byte) 76, (byte) 57, (byte) 0 }
      },
      {
        (byte) 47,
        new byte[3]{ (byte) 76, (byte) 66, (byte) 38 }
      },
      {
        (byte) 48,
        new byte[3]{ (byte) 38, (byte) 28, (byte) 0 }
      },
      {
        (byte) 49,
        new byte[3]{ (byte) 38, (byte) 33, (byte) 19 }
      },
      {
        (byte) 50,
        new byte[3]{ byte.MaxValue, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 51,
        new byte[3]{ byte.MaxValue, byte.MaxValue, (byte) 127 }
      },
      {
        (byte) 52,
        new byte[3]{ (byte) 165, (byte) 165, (byte) 0 }
      },
      {
        (byte) 53,
        new byte[3]{ (byte) 165, (byte) 165, (byte) 82 }
      },
      {
        (byte) 54,
        new byte[3]{ (byte) 127, (byte) 127, (byte) 0 }
      },
      {
        (byte) 55,
        new byte[3]{ (byte) 127, (byte) 127, (byte) 63 }
      },
      {
        (byte) 56,
        new byte[3]{ (byte) 76, (byte) 76, (byte) 0 }
      },
      {
        (byte) 57,
        new byte[3]{ (byte) 76, (byte) 76, (byte) 38 }
      },
      {
        (byte) 58,
        new byte[3]{ (byte) 38, (byte) 38, (byte) 0 }
      },
      {
        (byte) 59,
        new byte[3]{ (byte) 38, (byte) 38, (byte) 19 }
      },
      {
        (byte) 60,
        new byte[3]{ (byte) 191, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 61,
        new byte[3]{ (byte) 223, byte.MaxValue, (byte) 127 }
      },
      {
        (byte) 62,
        new byte[3]{ (byte) 124, (byte) 165, (byte) 0 }
      },
      {
        (byte) 63,
        new byte[3]{ (byte) 145, (byte) 165, (byte) 82 }
      },
      {
        (byte) 64,
        new byte[3]{ (byte) 95, (byte) 127, (byte) 0 }
      },
      {
        (byte) 65,
        new byte[3]{ (byte) 111, (byte) 127, (byte) 63 }
      },
      {
        (byte) 66,
        new byte[3]{ (byte) 57, (byte) 76, (byte) 0 }
      },
      {
        (byte) 67,
        new byte[3]{ (byte) 66, (byte) 76, (byte) 38 }
      },
      {
        (byte) 68,
        new byte[3]{ (byte) 28, (byte) 38, (byte) 0 }
      },
      {
        (byte) 69,
        new byte[3]{ (byte) 33, (byte) 38, (byte) 19 }
      },
      {
        (byte) 70,
        new byte[3]{ (byte) 127, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 71,
        new byte[3]{ (byte) 191, byte.MaxValue, (byte) 127 }
      },
      {
        (byte) 72,
        new byte[3]{ (byte) 82, (byte) 165, (byte) 0 }
      },
      {
        (byte) 73,
        new byte[3]{ (byte) 124, (byte) 165, (byte) 82 }
      },
      {
        (byte) 74,
        new byte[3]{ (byte) 63, (byte) 127, (byte) 0 }
      },
      {
        (byte) 75,
        new byte[3]{ (byte) 95, (byte) 127, (byte) 63 }
      },
      {
        (byte) 76,
        new byte[3]{ (byte) 38, (byte) 76, (byte) 0 }
      },
      {
        (byte) 77,
        new byte[3]{ (byte) 57, (byte) 76, (byte) 38 }
      },
      {
        (byte) 78,
        new byte[3]{ (byte) 19, (byte) 38, (byte) 0 }
      },
      {
        (byte) 79,
        new byte[3]{ (byte) 28, (byte) 38, (byte) 19 }
      },
      {
        (byte) 80,
        new byte[3]{ (byte) 63, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 81,
        new byte[3]{ (byte) 159, byte.MaxValue, (byte) 127 }
      },
      {
        (byte) 82,
        new byte[3]{ (byte) 41, (byte) 165, (byte) 0 }
      },
      {
        (byte) 83,
        new byte[3]{ (byte) 103, (byte) 165, (byte) 82 }
      },
      {
        (byte) 84,
        new byte[3]{ (byte) 31, (byte) 127, (byte) 0 }
      },
      {
        (byte) 85,
        new byte[3]{ (byte) 79, (byte) 127, (byte) 63 }
      },
      {
        (byte) 86,
        new byte[3]{ (byte) 19, (byte) 76, (byte) 0 }
      },
      {
        (byte) 87,
        new byte[3]{ (byte) 47, (byte) 76, (byte) 38 }
      },
      {
        (byte) 88,
        new byte[3]{ (byte) 9, (byte) 38, (byte) 0 }
      },
      {
        (byte) 89,
        new byte[3]{ (byte) 23, (byte) 38, (byte) 19 }
      },
      {
        (byte) 90,
        new byte[3]{ (byte) 0, byte.MaxValue, (byte) 0 }
      },
      {
        (byte) 91,
        new byte[3]{ (byte) 127, byte.MaxValue, (byte) 127 }
      },
      {
        (byte) 92,
        new byte[3]{ (byte) 0, (byte) 165, (byte) 0 }
      },
      {
        (byte) 93,
        new byte[3]{ (byte) 82, (byte) 165, (byte) 82 }
      },
      {
        (byte) 94,
        new byte[3]{ (byte) 0, (byte) 127, (byte) 0 }
      },
      {
        (byte) 95,
        new byte[3]{ (byte) 63, (byte) 127, (byte) 63 }
      },
      {
        (byte) 96,
        new byte[3]{ (byte) 0, (byte) 76, (byte) 0 }
      },
      {
        (byte) 97,
        new byte[3]{ (byte) 38, (byte) 76, (byte) 38 }
      },
      {
        (byte) 98,
        new byte[3]{ (byte) 0, (byte) 38, (byte) 0 }
      },
      {
        (byte) 99,
        new byte[3]{ (byte) 19, (byte) 38, (byte) 19 }
      },
      {
        (byte) 100,
        new byte[3]{ (byte) 0, byte.MaxValue, (byte) 63 }
      },
      {
        (byte) 101,
        new byte[3]{ (byte) 127, byte.MaxValue, (byte) 159 }
      },
      {
        (byte) 102,
        new byte[3]{ (byte) 0, (byte) 165, (byte) 41 }
      },
      {
        (byte) 103,
        new byte[3]{ (byte) 82, (byte) 165, (byte) 103 }
      },
      {
        (byte) 104,
        new byte[3]{ (byte) 0, (byte) 127, (byte) 31 }
      },
      {
        (byte) 105,
        new byte[3]{ (byte) 63, (byte) 127, (byte) 79 }
      },
      {
        (byte) 106,
        new byte[3]{ (byte) 0, (byte) 76, (byte) 19 }
      },
      {
        (byte) 107,
        new byte[3]{ (byte) 38, (byte) 76, (byte) 47 }
      },
      {
        (byte) 108,
        new byte[3]{ (byte) 0, (byte) 38, (byte) 9 }
      },
      {
        (byte) 109,
        new byte[3]{ (byte) 19, (byte) 38, (byte) 23 }
      },
      {
        (byte) 110,
        new byte[3]{ (byte) 0, byte.MaxValue, (byte) 127 }
      },
      {
        (byte) 111,
        new byte[3]{ (byte) 127, byte.MaxValue, (byte) 191 }
      },
      {
        (byte) 112,
        new byte[3]{ (byte) 0, (byte) 165, (byte) 82 }
      },
      {
        (byte) 113,
        new byte[3]{ (byte) 82, (byte) 165, (byte) 124 }
      },
      {
        (byte) 114,
        new byte[3]{ (byte) 0, (byte) 127, (byte) 63 }
      },
      {
        (byte) 115,
        new byte[3]{ (byte) 63, (byte) 127, (byte) 95 }
      },
      {
        (byte) 116,
        new byte[3]{ (byte) 0, (byte) 76, (byte) 38 }
      },
      {
        (byte) 117,
        new byte[3]{ (byte) 38, (byte) 76, (byte) 57 }
      },
      {
        (byte) 118,
        new byte[3]{ (byte) 0, (byte) 38, (byte) 19 }
      },
      {
        (byte) 119,
        new byte[3]{ (byte) 19, (byte) 38, (byte) 28 }
      },
      {
        (byte) 120,
        new byte[3]{ (byte) 0, byte.MaxValue, (byte) 191 }
      },
      {
        (byte) 121,
        new byte[3]{ (byte) 127, byte.MaxValue, (byte) 223 }
      },
      {
        (byte) 122,
        new byte[3]{ (byte) 0, (byte) 165, (byte) 124 }
      },
      {
        (byte) 123,
        new byte[3]{ (byte) 82, (byte) 165, (byte) 145 }
      },
      {
        (byte) 124,
        new byte[3]{ (byte) 0, (byte) 127, (byte) 95 }
      },
      {
        (byte) 125,
        new byte[3]{ (byte) 63, (byte) 127, (byte) 111 }
      },
      {
        (byte) 126,
        new byte[3]{ (byte) 0, (byte) 76, (byte) 57 }
      },
      {
        (byte) 127,
        new byte[3]{ (byte) 38, (byte) 76, (byte) 66 }
      },
      {
        (byte) 128,
        new byte[3]{ (byte) 0, (byte) 38, (byte) 28 }
      },
      {
        (byte) 129,
        new byte[3]{ (byte) 19, (byte) 38, (byte) 33 }
      },
      {
        (byte) 130,
        new byte[3]{ (byte) 0, byte.MaxValue, byte.MaxValue }
      },
      {
        (byte) 131,
        new byte[3]{ (byte) 127, byte.MaxValue, byte.MaxValue }
      },
      {
        (byte) 132,
        new byte[3]{ (byte) 0, (byte) 165, (byte) 165 }
      },
      {
        (byte) 133,
        new byte[3]{ (byte) 82, (byte) 165, (byte) 165 }
      },
      {
        (byte) 134,
        new byte[3]{ (byte) 0, (byte) 127, (byte) 127 }
      },
      {
        (byte) 135,
        new byte[3]{ (byte) 63, (byte) 127, (byte) 127 }
      },
      {
        (byte) 136,
        new byte[3]{ (byte) 0, (byte) 76, (byte) 76 }
      },
      {
        (byte) 137,
        new byte[3]{ (byte) 38, (byte) 76, (byte) 76 }
      },
      {
        (byte) 138,
        new byte[3]{ (byte) 0, (byte) 38, (byte) 38 }
      },
      {
        (byte) 139,
        new byte[3]{ (byte) 19, (byte) 38, (byte) 38 }
      },
      {
        (byte) 140,
        new byte[3]{ (byte) 0, (byte) 191, byte.MaxValue }
      },
      {
        (byte) 141,
        new byte[3]{ (byte) 127, (byte) 223, byte.MaxValue }
      },
      {
        (byte) 142,
        new byte[3]{ (byte) 0, (byte) 124, (byte) 165 }
      },
      {
        (byte) 143,
        new byte[3]{ (byte) 82, (byte) 145, (byte) 165 }
      },
      {
        (byte) 144,
        new byte[3]{ (byte) 0, (byte) 95, (byte) 127 }
      },
      {
        (byte) 145,
        new byte[3]{ (byte) 63, (byte) 111, (byte) 127 }
      },
      {
        (byte) 146,
        new byte[3]{ (byte) 0, (byte) 57, (byte) 76 }
      },
      {
        (byte) 147,
        new byte[3]{ (byte) 38, (byte) 66, (byte) 76 }
      },
      {
        (byte) 148,
        new byte[3]{ (byte) 0, (byte) 28, (byte) 38 }
      },
      {
        (byte) 149,
        new byte[3]{ (byte) 19, (byte) 33, (byte) 38 }
      },
      {
        (byte) 150,
        new byte[3]{ (byte) 0, (byte) 127, byte.MaxValue }
      },
      {
        (byte) 151,
        new byte[3]{ (byte) 127, (byte) 191, byte.MaxValue }
      },
      {
        (byte) 152,
        new byte[3]{ (byte) 0, (byte) 82, (byte) 165 }
      },
      {
        (byte) 153,
        new byte[3]{ (byte) 82, (byte) 124, (byte) 165 }
      },
      {
        (byte) 154,
        new byte[3]{ (byte) 0, (byte) 63, (byte) 127 }
      },
      {
        (byte) 155,
        new byte[3]{ (byte) 63, (byte) 95, (byte) 127 }
      },
      {
        (byte) 156,
        new byte[3]{ (byte) 0, (byte) 38, (byte) 76 }
      },
      {
        (byte) 157,
        new byte[3]{ (byte) 38, (byte) 57, (byte) 76 }
      },
      {
        (byte) 158,
        new byte[3]{ (byte) 0, (byte) 19, (byte) 38 }
      },
      {
        (byte) 159,
        new byte[3]{ (byte) 19, (byte) 28, (byte) 38 }
      },
      {
        (byte) 160,
        new byte[3]{ (byte) 0, (byte) 63, byte.MaxValue }
      },
      {
        (byte) 161,
        new byte[3]{ (byte) 127, (byte) 159, byte.MaxValue }
      },
      {
        (byte) 162,
        new byte[3]{ (byte) 0, (byte) 41, (byte) 165 }
      },
      {
        (byte) 163,
        new byte[3]{ (byte) 82, (byte) 103, (byte) 165 }
      },
      {
        (byte) 164,
        new byte[3]{ (byte) 0, (byte) 31, (byte) 127 }
      },
      {
        (byte) 165,
        new byte[3]{ (byte) 63, (byte) 79, (byte) 127 }
      },
      {
        (byte) 166,
        new byte[3]{ (byte) 0, (byte) 19, (byte) 76 }
      },
      {
        (byte) 167,
        new byte[3]{ (byte) 38, (byte) 47, (byte) 76 }
      },
      {
        (byte) 168,
        new byte[3]{ (byte) 0, (byte) 9, (byte) 38 }
      },
      {
        (byte) 169,
        new byte[3]{ (byte) 19, (byte) 23, (byte) 38 }
      },
      {
        (byte) 170,
        new byte[3]{ (byte) 0, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 171,
        new byte[3]{ (byte) 127, (byte) 127, byte.MaxValue }
      },
      {
        (byte) 172,
        new byte[3]{ (byte) 0, (byte) 0, (byte) 165 }
      },
      {
        (byte) 173,
        new byte[3]{ (byte) 82, (byte) 82, (byte) 165 }
      },
      {
        (byte) 174,
        new byte[3]{ (byte) 0, (byte) 0, (byte) 127 }
      },
      {
        (byte) 175,
        new byte[3]{ (byte) 63, (byte) 63, (byte) 127 }
      },
      {
        (byte) 176,
        new byte[3]{ (byte) 0, (byte) 0, (byte) 76 }
      },
      {
        (byte) 177,
        new byte[3]{ (byte) 38, (byte) 38, (byte) 76 }
      },
      {
        (byte) 178,
        new byte[3]{ (byte) 0, (byte) 0, (byte) 38 }
      },
      {
        (byte) 179,
        new byte[3]{ (byte) 19, (byte) 19, (byte) 38 }
      },
      {
        (byte) 180,
        new byte[3]{ (byte) 63, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 181,
        new byte[3]{ (byte) 159, (byte) 127, byte.MaxValue }
      },
      {
        (byte) 182,
        new byte[3]{ (byte) 41, (byte) 0, (byte) 165 }
      },
      {
        (byte) 183,
        new byte[3]{ (byte) 103, (byte) 82, (byte) 165 }
      },
      {
        (byte) 184,
        new byte[3]{ (byte) 31, (byte) 0, (byte) 127 }
      },
      {
        (byte) 185,
        new byte[3]{ (byte) 79, (byte) 63, (byte) 127 }
      },
      {
        (byte) 186,
        new byte[3]{ (byte) 19, (byte) 0, (byte) 76 }
      },
      {
        (byte) 187,
        new byte[3]{ (byte) 47, (byte) 38, (byte) 76 }
      },
      {
        (byte) 188,
        new byte[3]{ (byte) 9, (byte) 0, (byte) 38 }
      },
      {
        (byte) 189,
        new byte[3]{ (byte) 23, (byte) 19, (byte) 38 }
      },
      {
        (byte) 190,
        new byte[3]{ (byte) 127, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 191,
        new byte[3]{ (byte) 191, (byte) 127, byte.MaxValue }
      },
      {
        (byte) 192,
        new byte[3]{ (byte) 82, (byte) 0, (byte) 165 }
      },
      {
        (byte) 193,
        new byte[3]{ (byte) 124, (byte) 82, (byte) 165 }
      },
      {
        (byte) 194,
        new byte[3]{ (byte) 63, (byte) 0, (byte) 127 }
      },
      {
        (byte) 195,
        new byte[3]{ (byte) 95, (byte) 63, (byte) 127 }
      },
      {
        (byte) 196,
        new byte[3]{ (byte) 38, (byte) 0, (byte) 76 }
      },
      {
        (byte) 197,
        new byte[3]{ (byte) 57, (byte) 38, (byte) 76 }
      },
      {
        (byte) 198,
        new byte[3]{ (byte) 19, (byte) 0, (byte) 38 }
      },
      {
        (byte) 199,
        new byte[3]{ (byte) 28, (byte) 19, (byte) 38 }
      },
      {
        (byte) 200,
        new byte[3]{ (byte) 191, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 201,
        new byte[3]{ (byte) 223, (byte) 127, byte.MaxValue }
      },
      {
        (byte) 202,
        new byte[3]{ (byte) 124, (byte) 0, (byte) 165 }
      },
      {
        (byte) 203,
        new byte[3]{ (byte) 145, (byte) 82, (byte) 165 }
      },
      {
        (byte) 204,
        new byte[3]{ (byte) 95, (byte) 0, (byte) 127 }
      },
      {
        (byte) 205,
        new byte[3]{ (byte) 111, (byte) 63, (byte) 127 }
      },
      {
        (byte) 206,
        new byte[3]{ (byte) 57, (byte) 0, (byte) 76 }
      },
      {
        (byte) 207,
        new byte[3]{ (byte) 66, (byte) 38, (byte) 76 }
      },
      {
        (byte) 208,
        new byte[3]{ (byte) 28, (byte) 0, (byte) 38 }
      },
      {
        (byte) 209,
        new byte[3]{ (byte) 33, (byte) 19, (byte) 38 }
      },
      {
        (byte) 210,
        new byte[3]{ byte.MaxValue, (byte) 0, byte.MaxValue }
      },
      {
        (byte) 211,
        new byte[3]{ byte.MaxValue, (byte) 127, byte.MaxValue }
      },
      {
        (byte) 212,
        new byte[3]{ (byte) 165, (byte) 0, (byte) 165 }
      },
      {
        (byte) 213,
        new byte[3]{ (byte) 165, (byte) 82, (byte) 165 }
      },
      {
        (byte) 214,
        new byte[3]{ (byte) 127, (byte) 0, (byte) 127 }
      },
      {
        (byte) 215,
        new byte[3]{ (byte) 127, (byte) 63, (byte) 127 }
      },
      {
        (byte) 216,
        new byte[3]{ (byte) 76, (byte) 0, (byte) 76 }
      },
      {
        (byte) 217,
        new byte[3]{ (byte) 76, (byte) 38, (byte) 76 }
      },
      {
        (byte) 218,
        new byte[3]{ (byte) 38, (byte) 0, (byte) 38 }
      },
      {
        (byte) 219,
        new byte[3]{ (byte) 38, (byte) 19, (byte) 38 }
      },
      {
        (byte) 220,
        new byte[3]{ byte.MaxValue, (byte) 0, (byte) 191 }
      },
      {
        (byte) 221,
        new byte[3]{ byte.MaxValue, (byte) 127, (byte) 223 }
      },
      {
        (byte) 222,
        new byte[3]{ (byte) 165, (byte) 0, (byte) 124 }
      },
      {
        (byte) 223,
        new byte[3]{ (byte) 165, (byte) 82, (byte) 145 }
      },
      {
        (byte) 224,
        new byte[3]{ (byte) 127, (byte) 0, (byte) 95 }
      },
      {
        (byte) 225,
        new byte[3]{ (byte) 127, (byte) 63, (byte) 111 }
      },
      {
        (byte) 226,
        new byte[3]{ (byte) 76, (byte) 0, (byte) 57 }
      },
      {
        (byte) 227,
        new byte[3]{ (byte) 76, (byte) 38, (byte) 66 }
      },
      {
        (byte) 228,
        new byte[3]{ (byte) 38, (byte) 0, (byte) 28 }
      },
      {
        (byte) 229,
        new byte[3]{ (byte) 38, (byte) 19, (byte) 33 }
      },
      {
        (byte) 230,
        new byte[3]{ byte.MaxValue, (byte) 0, (byte) 127 }
      },
      {
        (byte) 231,
        new byte[3]{ byte.MaxValue, (byte) 127, (byte) 191 }
      },
      {
        (byte) 232,
        new byte[3]{ (byte) 165, (byte) 0, (byte) 82 }
      },
      {
        (byte) 233,
        new byte[3]{ (byte) 165, (byte) 82, (byte) 124 }
      },
      {
        (byte) 234,
        new byte[3]{ (byte) 127, (byte) 0, (byte) 63 }
      },
      {
        (byte) 235,
        new byte[3]{ (byte) 127, (byte) 63, (byte) 95 }
      },
      {
        (byte) 236,
        new byte[3]{ (byte) 76, (byte) 0, (byte) 38 }
      },
      {
        (byte) 237,
        new byte[3]{ (byte) 76, (byte) 38, (byte) 57 }
      },
      {
        (byte) 238,
        new byte[3]{ (byte) 38, (byte) 0, (byte) 19 }
      },
      {
        (byte) 239,
        new byte[3]{ (byte) 38, (byte) 19, (byte) 28 }
      },
      {
        (byte) 240,
        new byte[3]{ byte.MaxValue, (byte) 0, (byte) 63 }
      },
      {
        (byte) 241,
        new byte[3]{ byte.MaxValue, (byte) 127, (byte) 159 }
      },
      {
        (byte) 242,
        new byte[3]{ (byte) 165, (byte) 0, (byte) 41 }
      },
      {
        (byte) 243,
        new byte[3]{ (byte) 165, (byte) 82, (byte) 103 }
      },
      {
        (byte) 244,
        new byte[3]{ (byte) 127, (byte) 0, (byte) 31 }
      },
      {
        (byte) 245,
        new byte[3]{ (byte) 127, (byte) 63, (byte) 79 }
      },
      {
        (byte) 246,
        new byte[3]{ (byte) 76, (byte) 0, (byte) 19 }
      },
      {
        (byte) 247,
        new byte[3]{ (byte) 76, (byte) 38, (byte) 47 }
      },
      {
        (byte) 248,
        new byte[3]{ (byte) 38, (byte) 0, (byte) 9 }
      },
      {
        (byte) 249,
        new byte[3]{ (byte) 38, (byte) 19, (byte) 23 }
      },
      {
        (byte) 250,
        new byte[3]
      },
      {
        (byte) 251,
        new byte[3]{ (byte) 51, (byte) 51, (byte) 51 }
      },
      {
        (byte) 252,
        new byte[3]{ (byte) 102, (byte) 102, (byte) 102 }
      },
      {
        (byte) 253,
        new byte[3]{ (byte) 153, (byte) 153, (byte) 153 }
      },
      {
        (byte) 254,
        new byte[3]{ (byte) 204, (byte) 204, (byte) 204 }
      },
      {
        byte.MaxValue,
        new byte[3]{ byte.MaxValue, byte.MaxValue, byte.MaxValue }
      }
    };
        private short index;
        private byte r;
        private byte g;
        private byte b;

        /// <summary>Gets the ByLayer color.</summary>
        public static AciColor ByLayer => new AciColor()
        {
            index = 256
        };

        /// <summary>Gets the ByBlock color.</summary>
        public static AciColor ByBlock => new AciColor()
        {
            index = 0
        };

        /// <summary>Defines a default red color.</summary>
        public static AciColor Red => new AciColor((short)1);

        /// <summary>Defines a default yellow color.</summary>
        public static AciColor Yellow => new AciColor((short)2);

        /// <summary>Defines a default green color.</summary>
        public static AciColor Green => new AciColor((short)3);

        /// <summary>Defines a default cyan color.</summary>
        public static AciColor Cyan => new AciColor((short)4);

        /// <summary>Defines a default blue color.</summary>
        public static AciColor Blue => new AciColor((short)5);

        /// <summary>Defines a default magenta color.</summary>
        public static AciColor Magenta => new AciColor((short)6);

        /// <summary>Defines a default white/black color.</summary>
        public static AciColor Default => new AciColor((short)7);

        /// <summary>Defines a default dark gray color.</summary>
        public static AciColor DarkGray => new AciColor((short)8);

        /// <summary>Defines a default light gray color.</summary>
        public static AciColor LightGray => new AciColor((short)9);

        /// <summary>
        /// A dictionary that contains the indexed colors, the key represents the color index and the value the RGB components of the color.
        /// </summary>
        public static IReadOnlyDictionary<byte, byte[]> IndexRgb => AciColor.indexRgb;

        /// <summary>
        /// Initializes a new instance of the <c>AciColor</c> class with black/white color index 7.
        /// </summary>
        public AciColor()
          : this((short)7)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>AciColor</c> class.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        public AciColor(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.index = (short)AciColor.RgbToAci(this.r, this.g, this.b);
        }

        /// <summary>
        /// Initializes a new instance of the <c>AciColor</c> class.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <remarks>By default the UseTrueColor will be set to true.</remarks>
        public AciColor(float r, float g, float b)
          : this((byte)((double)r * (double)byte.MaxValue), (byte)((double)g * (double)byte.MaxValue), (byte)((double)b * (double)byte.MaxValue))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>AciColor</c> class.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <remarks>By default the UseTrueColor will be set to true.</remarks>
        public AciColor(double r, double g, double b)
          : this((byte)(r * (double)byte.MaxValue), (byte)(g * (double)byte.MaxValue), (byte)(b * (double)byte.MaxValue))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>AciColor</c> class.
        /// </summary>
        /// <param name="color">A <see cref="T:System.Drawing.Color">color</see>.</param>
        /// <remarks>By default the UseTrueColor will be set to true.</remarks>
        public AciColor(Color color)
          : this(color.R, color.G, color.B)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>AciColor</c> class.
        /// </summary>
        /// <param name="index">Color index.</param>
        /// <remarks>
        /// By default the UseTrueColor will be set to false.<br />
        /// Accepted color index values range from 1 to 255.<br />
        /// Indexes from 1 to 255 represents a color, the index 0 and 256 are reserved for ByLayer and ByBlock colors.
        /// </remarks>
        public AciColor(short index)
        {
            byte[] numArray = index > (short)0 && index < (short)256 ? AciColor.IndexRgb[(byte)index] : throw new ArgumentOutOfRangeException(nameof(index), (object)index, "Accepted color index values range from 1 to 255.");
            this.r = numArray[0];
            this.g = numArray[1];
            this.b = numArray[2];
            this.index = index;
        }

        /// <summary>Defines if the color is defined by layer.</summary>
        public bool IsByLayer => this.index == (short)256;

        /// <summary>Defines if the color is defined by block.</summary>
        public bool IsByBlock => this.index == (short)0;

        /// <summary>Gets the red component of the AciColor.</summary>
        public byte R => this.r;

        /// <summary>Gets the green component of the AciColor.</summary>
        public byte G => this.g;

        /// <summary>Gets the blue component of the AciColor.</summary>
        public byte B => this.b;

        /// <summary>Gets or sets the color index.</summary>
        /// <remarks>
        /// Accepted color index values range from 1 to 255.
        /// Indexes from 1 to 255 represents a color, the index 0 and 256 are reserved for ByLayer and ByBlock colors.
        /// </remarks>
        public short Index
        {
            get => this.index;
            set
            {
                if (value == (short)0 || value == (short)256)
                {
                    this.index = (short)256;
                }
                else
                {
                    this.index = value > (short)0 && value < (short)256 ? value : throw new ArgumentOutOfRangeException(nameof(value), (object)value, "Accepted color index values range from 1 to 255.");
                    byte[] numArray = AciColor.IndexRgb[(byte)this.index];
                    this.r = numArray[0];
                    this.g = numArray[1];
                    this.b = numArray[2];
                }
            }
        }

        /// <summary>
        /// Converts HSL (hue, saturation, lightness) value to an <see cref="T:SpiralLab.Sirius.AciColor">AciColor</see>.
        /// </summary>
        /// <param name="hsl">A Vector3 containing the hue, saturation, and lightness components.</param>
        /// <returns>An <see cref="T:System.Drawing.Color">AciColor</see> that represents the actual HSL value.</returns>
        public static AciColor FromHsl(Vector3 hsl) => AciColor.FromHsl((double)hsl.X, (double)hsl.Y, (double)hsl.Z);

        /// <summary>
        /// Converts HSL (hue, saturation, lightness) value to an <see cref="T:SpiralLab.Sirius.AciColor">AciColor</see>.
        /// </summary>
        /// <param name="hue">Hue (input values range from 0 to 1).</param>
        /// <param name="saturation">Saturation percentage (input values range from 0 to 1).</param>
        /// <param name="lightness">Lightness percentage (input values range from 0 to 1).</param>
        /// <returns>An <see cref="T:System.Drawing.Color">AciColor</see> that represents the actual HSL value.</returns>
        public static AciColor FromHsl(double hue, double saturation, double lightness)
        {
            double r = lightness;
            double g = lightness;
            double b = lightness;
            double num1 = lightness <= 0.5 ? lightness * (1.0 + saturation) : lightness + saturation - lightness * saturation;
            if (num1 > 0.0)
            {
                double num2 = lightness + lightness - num1;
                double num3 = (num1 - num2) / num1;
                hue *= 6.0;
                int num4 = (int)hue;
                double num5 = hue - (double)num4;
                double num6 = num1 * num3 * num5;
                double num7 = num2 + num6;
                double num8 = num1 - num6;
                switch (num4)
                {
                    case 1:
                        r = num8;
                        g = num1;
                        b = num2;
                        break;
                    case 2:
                        r = num2;
                        g = num1;
                        b = num7;
                        break;
                    case 3:
                        r = num2;
                        g = num8;
                        b = num1;
                        break;
                    case 4:
                        r = num7;
                        g = num2;
                        b = num1;
                        break;
                    case 5:
                        r = num1;
                        g = num2;
                        b = num8;
                        break;
                    case 6:
                        r = num1;
                        g = num7;
                        b = num2;
                        break;
                }
            }
            return new AciColor(r, g, b);
        }

        /// <summary>
        /// Converts the AciColor to a <see cref="T:System.Drawing.Color">color</see>.
        /// </summary>
        /// <returns>A <see cref="T:System.Drawing.Color">System.Drawing.Color</see> that represents the actual AciColor.</returns>
        /// <remarks>A default color white will be used for ByLayer and ByBlock colors.</remarks>
        public Color ToColor() => this.index < (short)1 || this.index > (short)byte.MaxValue ? Color.White : Color.FromArgb((int)this.r, (int)this.g, (int)this.b);

        /// <summary>
        /// Converts a <see cref="T:System.Drawing.Color">color</see> to an <see cref="T:System.Drawing.Color">AciColor</see>.
        /// </summary>
        /// <param name="color">A <see cref="T:System.Drawing.Color">color</see>.</param>
        public void FromColor(Color color)
        {
            this.r = color.R;
            this.g = color.G;
            this.b = color.B;
            this.index = (short)AciColor.RgbToAci(this.r, this.g, this.b);
        }

        /// <summary>Gets the 24-bit color value from an AciColor.</summary>
        /// <param name="color">A <see cref="T:SpiralLab.Sirius.AciColor">color</see>.</param>
        /// <returns>A 24-bit color value (BGR order).</returns>
        public static int ToTrueColor(AciColor color)
        {
            if (color == null)
                throw new ArgumentNullException(nameof(color));
            return BitConverter.ToInt32(new byte[4]
            {
        color.B,
        color.G,
        color.R,
        (byte) 0
            }, 0);
        }

        /// <summary>
        /// Gets the <see cref="T:SpiralLab.Sirius.AciColor">color</see> from a 24-bit color value.
        /// </summary>
        /// <param name="value">A 24-bit color value (BGR order).</param>
        /// <returns>A <see cref="T:SpiralLab.Sirius.AciColor">color</see>.</returns>
        public static AciColor FromTrueColor(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return new AciColor(bytes[2], bytes[1], bytes[0]);
        }

        /// <summary>
        /// Gets the <see cref="T:SpiralLab.Sirius.AciColor">color</see> from an index.
        /// </summary>
        /// <param name="index">A CAD indexed AciColor index.</param>
        /// <returns>A <see cref="T:SpiralLab.Sirius.AciColor">color</see>.</returns>
        /// <remarks>
        /// Accepted index values range from 0 to 256. An index 0 represents a ByBlock color and an index 256 is a ByLayer color;
        /// any other value will return one of the 255 indexed AciColors.
        /// </remarks>
        public static AciColor FromCadIndex(short index)
        {
            if (index < (short)0 || index > (short)256)
                throw new ArgumentOutOfRangeException(nameof(index), (object)index, "Accepted CAD indexed AciColor values range from 0 to 256.");
            if (index == (short)0)
                return AciColor.ByBlock;
            return index == (short)256 ? AciColor.ByLayer : new AciColor(index);
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            if (this.index == (short)0)
                return "ByBlock";
            return this.index == (short)256 ? "ByLayer" : string.Format("{0}, {1}, {2}", (object)this.r, (object)this.g, (object)this.b);
        }

        /// <summary>
        /// Creates a new color that is a copy of the current instance.
        /// </summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new AciColor()
        {
            r = this.r,
            g = this.g,
            b = this.b,
            index = this.index
        };

        /// <summary>Check if the components of two colors are equal.</summary>
        /// <param name="other">Another color to compare to.</param>
        /// <returns>True if the three components are equal or false in any other case.</returns>
        public bool Equals(AciColor other) => other != null && (int)other.r == (int)this.r && (int)other.g == (int)this.g && (int)other.b == (int)this.b;

        /// <summary>
        /// Obtains the approximate color index from the RGB components.
        /// </summary>
        /// <param name="r">Red component.</param>
        /// <param name="g">Green component.</param>
        /// <param name="b">Blue component.</param>
        /// <returns>The approximate color index from the RGB components</returns>
        /// <remarks>This conversion will never be accurate.</remarks>
        private static byte RgbToAci(byte r, byte g, byte b)
        {
            double num1 = double.MaxValue;
            byte num2 = 0;
            foreach (byte key in AciColor.IndexRgb.Keys)
            {
                byte[] numArray = AciColor.IndexRgb[key];
                double num3 = Math.Abs(0.3 * (double)((int)r - (int)numArray[0]) + 0.59 * (double)((int)g - (int)numArray[1]) + 0.11 * (double)((int)b - (int)numArray[2]));
                if (num3 < num1)
                {
                    num1 = num3;
                    num2 = key;
                }
            }
            return num2;
        }

        public static Color FromCadIndexToColor(short index)
        {
            if (index < (short)0 || index > (short)256)
                throw new ArgumentOutOfRangeException(nameof(index), (object)index, "Accepted CAD indexed AciColor values range from 0 to 256.");
            return index == (short)0 || index == (short)256 ? Config.DefaultColor : new AciColor(index).ToColor();
        }
    }
}
