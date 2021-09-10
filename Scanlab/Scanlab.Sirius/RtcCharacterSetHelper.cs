
using System;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Character Set 을 최대 4개까지 유지하기 위한 헬퍼
    /// SiriusTextDate, Time, Serial 을 사용할 경우 항상
    /// IMarker Ready 실행시 Clear 를 하여 RTC 리스트 3 버퍼가 삭제되도록 유도한다
    /// </summary>
    public static class RtcCharacterSetHelper
    {
        private const int rtcCharacterSetMaxCount = 4;
        private static ICharacterSetInfo[] Infos = new ICharacterSetInfo[4];
        private static CharacterSet CurrentCharacterSet;

        /// <summary>
        /// RTC 에 해당 색인 문자 집합을 다운로드 한다
        /// 이때 4개의 문자집합중 하나로 설정된다
        /// </summary>
        /// <param name="rtc">IRtcCharacter 인터페이스</param>
        /// <param name="info">다운로드할 문자집합 정보</param>
        /// <param name="characterSet">문자집합 인덱스 번호 (0,1,2,3)</param>
        /// <returns></returns>
        internal static bool Regen(IRtc rtc, ICharacterSetInfo info, out CharacterSet characterSet)
        {
            characterSet = CharacterSet._0;
            for (int index = 0; index < RtcCharacterSetHelper.Infos.Length; ++index)
            {
                if (RtcCharacterSetHelper.Infos[index] == null)
                {
                    switch (info)
                    {
                        case CxfCharacterSetInfo _:
                            CxfCharacterSetInfo info1 = info as CxfCharacterSetInfo;
                            if (RtcCharacterSetHelper.RegisterCxf(rtc, info1, RtcCharacterSetHelper.CurrentCharacterSet))
                            {
                                RtcCharacterSetHelper.Infos[index] = info;
                                characterSet = RtcCharacterSetHelper.CurrentCharacterSet;
                                ++RtcCharacterSetHelper.CurrentCharacterSet;
                                break;
                            }
                            break;
                        case TTFCharacterSetInfo _:
                            TTFCharacterSetInfo info2 = info as TTFCharacterSetInfo;
                            if (RtcCharacterSetHelper.RegisterTTF(rtc, info2, RtcCharacterSetHelper.CurrentCharacterSet))
                            {
                                RtcCharacterSetHelper.Infos[index] = info;
                                characterSet = RtcCharacterSetHelper.CurrentCharacterSet;
                                ++RtcCharacterSetHelper.CurrentCharacterSet;
                                break;
                            }
                            break;
                    }
                    return true;
                }
                if (RtcCharacterSetHelper.Infos[index] is CxfCharacterSetInfo && info is CxfCharacterSetInfo)
                {
                    CxfCharacterSetInfo info1 = RtcCharacterSetHelper.Infos[index] as CxfCharacterSetInfo;
                    if ((info as CxfCharacterSetInfo).Equals(info1))
                    {
                        characterSet = (CharacterSet)index;
                        return true;
                    }
                }
                else if (RtcCharacterSetHelper.Infos[index] is TTFCharacterSetInfo && info is TTFCharacterSetInfo)
                {
                    TTFCharacterSetInfo info1 = RtcCharacterSetHelper.Infos[index] as TTFCharacterSetInfo;
                    if ((info as TTFCharacterSetInfo).Equals(info1))
                    {
                        characterSet = (CharacterSet)index;
                        return true;
                    }
                }
            }
            Logger.Log(Logger.Type.Error, "fail to create character set any more. max limits 4 counts", Array.Empty<object>());
            return false;
        }

        /// <summary>
        /// 문자집합 초기화
        /// RTC내의 메모리도 초기화
        /// </summary>
        /// <param name="rtc">IRtcCharacter 인터페이스</param>
        public static void Clear(IRtc rtc)
        {
            RtcCharacterSetHelper.Infos = new ICharacterSetInfo[4];
            RtcCharacterSetHelper.CurrentCharacterSet = CharacterSet._0;
            if (rtc is IRtcCharacterSet rtcCharacterSet)
            {
                rtcCharacterSet.CtlCharacterSetClear(CharacterSet._0);
                rtcCharacterSet?.CtlCharacterSetClear(CharacterSet._1);
                rtcCharacterSet?.CtlCharacterSetClear(CharacterSet._2);
                rtcCharacterSet?.CtlCharacterSetClear(CharacterSet._3);
            }
        }

        /// <summary>실제 RTC에 다운로드 하는 루틴</summary>
        /// <param name="rtc"></param>
        /// <param name="info"></param>
        /// <param name="characterSet"></param>
        /// <returns></returns>
        private static bool RegisterCxf(IRtc rtc, CxfCharacterSetInfo info, CharacterSet characterSet)
        {
            if (!(rtc is IRtcCharacterSet rtcCharacterSet))
                return false;
            IMatrixStack matrixStack = (IMatrixStack)rtc.MatrixStack.Clone();
            rtc.MatrixStack.Clear();
            bool flag1 = true;
            for (uint asciiCode = 0; asciiCode < (uint)byte.MaxValue; ++asciiCode)
            {
                SiriusText siriusText = new SiriusText(string.Format("{0}", (object)(char)asciiCode))
                {
                    FontName = info.FontName,
                    Width = info.Width,
                    CapHeight = info.CapHeight,
                    LetterSpace = info.LetterSpace,
                    LetterSpacing = info.LetterSpacing,
                    Angle = info.Angle
                };
                siriusText.Regen();
                if (!siriusText.IsNoVertices)
                {
                    bool flag2 = flag1 & rtcCharacterSet.CtlCharacterBegin(asciiCode, characterSet);
                    rtc.ListJump(Vector2.Zero);
                    MarkerArgDefault markerArgDefault = new MarkerArgDefault()
                    {
                        Rtc = rtc
                    };
                    bool flag3 = flag2 & siriusText.Mark((IMarkerArg)markerArgDefault);
                    Vector2 originRightLocation = siriusText.OriginRightLocation;
                    flag1 = flag3 & rtc.ListJump(originRightLocation) & rtcCharacterSet.CtlCharacterEnd();
                    if (!flag1)
                        break;
                }
            }
            rtc.MatrixStack = matrixStack;
            if (flag1)
                Logger.Log(Logger.Type.Info, string.Format("success to register sirius text dynamic character set= {0}", (object)characterSet), Array.Empty<object>());
            else
                Logger.Log(Logger.Type.Error, string.Format("fail to sirius text dynamic registered character set= {0}", (object)characterSet), Array.Empty<object>());
            return flag1;
        }

        private static bool RegisterTTF(IRtc rtc, TTFCharacterSetInfo info, CharacterSet characterSet)
        {
            if (!(rtc is IRtcCharacterSet rtcCharacterSet))
                return false;
            IMatrixStack matrixStack = (IMatrixStack)rtc.MatrixStack.Clone();
            rtc.MatrixStack.Clear();
            bool flag1 = true;
            for (uint asciiCode = 0; asciiCode < (uint)byte.MaxValue; ++asciiCode)
            {
                Text text = new Text(string.Format("{0}", (object)(char)asciiCode))
                {
                    FontName = info.FontName,
                    Width = info.Width,
                    CapHeight = info.CapHeight,
                    LetterSpace = info.LetterSpace,
                    LetterSpacing = info.LetterSpacing,
                    Angle = info.Angle,
                    IsHatchable = info.IsHatchable,
                    HatchMode = info.HatchMode,
                    HatchAngle = info.HatchAngle,
                    HatchInterval = info.HatchInterval,
                    HatchExclude = info.HatchExclude
                };
                text.Regen();
                bool flag2 = flag1 & rtcCharacterSet.CtlCharacterBegin(asciiCode, characterSet);
                rtc.ListJump(Vector2.Zero);
                MarkerArgDefault markerArgDefault = new MarkerArgDefault()
                {
                    Rtc = rtc
                };
                bool flag3 = flag2 & text.Mark((IMarkerArg)markerArgDefault);
                Vector2 originRightLocation = text.OriginRightLocation;
                flag1 = flag3 & rtc.ListJump(originRightLocation) & rtcCharacterSet.CtlCharacterEnd();
                if (!flag1)
                    break;
            }
            rtc.MatrixStack = matrixStack;
            if (flag1)
                Logger.Log(Logger.Type.Info, string.Format("success to register ttf text dynamic character set= {0}", (object)characterSet), Array.Empty<object>());
            else
                Logger.Log(Logger.Type.Error, string.Format("fail to ttf text dynamic registered character set= {0}", (object)characterSet), Array.Empty<object>());
            return flag1;
        }
    }
}
