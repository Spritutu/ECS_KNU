
namespace Scanlab.Sirius
{
    /// <summary>엔티티 타입 종류</summary>
    public enum EType
    {
        Unknown = 0,
        Layer = 1,
        Point = 2,
        Points = 3,
        Line = 4,
        Arc = 5,
        Circle = 6,
        Ellipse = 7,
        Rectangle = 8,
        Trepan = 9,
        Spiral = 10, // 0x0000000A
        Hpgl = 11, // 0x0000000B
        LWPolyline = 12, // 0x0000000C
        Raster = 13, // 0x0000000D
        RasterLine = 14, // 0x0000000E
        Bitmap = 15, // 0x0000000F
        StitchedImage = 16, // 0x00000010
        BlockInsert = 17, // 0x00000011
        Text = 18, // 0x00000012
        TextArc = 19, // 0x00000013
        TextDate = 20, // 0x00000014
        TextSerial = 21, // 0x00000015
        TextTime = 22, // 0x00000016
        SiriusText = 23, // 0x00000017
        SiriusTextArc = 24, // 0x00000018
        SiriusTextDate = 25, // 0x00000019
        SiriusTextSerial = 26, // 0x0000001A
        SiriusTextTime = 27, // 0x0000001B
        Barcode1D = 28, // 0x0000001C
        BarcodeDataMatrix = 29, // 0x0000001D
        BarcodeQRCode = 30, // 0x0000001E
        Stereolithography = 31, // 0x0000001F
        Group = 32, // 0x00000020
        Timer = 100, // 0x00000064
        MotfBegin = 101, // 0x00000065
        MotfEnd = 102, // 0x00000066
        MotfExtStartDelay = 103, // 0x00000067
        MotfWait = 104, // 0x00000068
        VectorBegin = 105, // 0x00000069
        VectorEnd = 106, // 0x0000006A
        AlcVectorBegin = 107, // 0x0000006B
        AlcVectorEnd = 108, // 0x0000006C
        PenReturn = 200, // 0x000000C8
        PenDefault = 201, // 0x000000C9
        PenMotf = 202, // 0x000000CA
        WriteData = 300, // 0x0000012C
        WriteDataExt16 = 301, // 0x0000012D
        UserCustom1 = 1000, // 0x000003E8
        UserCustom2 = 1001, // 0x000003E9
        UserCustom3 = 1002, // 0x000003EA
        UserCustom4 = 1003, // 0x000003EB
        UserCustom5 = 1004, // 0x000003EC
        UserCustom6 = 1005, // 0x000003ED
        UserCustom7 = 1006, // 0x000003EE
        UserCustom8 = 1007, // 0x000003EF
        UserCustom9 = 1008, // 0x000003F0
        UserCustom10 = 1009, // 0x000003F1
    }
}
