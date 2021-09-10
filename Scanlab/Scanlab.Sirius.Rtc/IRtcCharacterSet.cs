
namespace Scanlab.Sirius
{
    /// <summary>RTC IRtcCharacterSet 인터페이스</summary>
    public interface IRtcCharacterSet
    {
        /// <summary>시작 시리얼 번호</summary>
        uint SerialStartNo { get; }

        /// <summary>증가 시리얼 번호값 (CtlSerialReset 함수에 의해 설정)</summary>
        uint SerialIncrementStep { get; }

        /// <summary>현재 시리얼 번호값 (외부 /START 에 의해 증가된 값)</summary>
        uint SerialCurrentNo { get; }

        /// <summary>
        /// 특정 색인 문자 좌표 정보 저장 시작
        /// 이 명령 이후 해당 문자(character)에 대한 리스트 명령 (jump, mark, arc) 명령 호출 필요
        /// </summary>
        /// <param name="asciiCode">아스키 코드 (0~255)</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool CtlCharacterBegin(uint asciiCode, CharacterSet characterSet = CharacterSet._0);

        /// <summary>
        /// 보호된 리스트 버퍼(3) 영역에 색인 문자 저장 완료
        /// CtlCharacterSetBegin 함수와 짝이 되어 문자 리스트 명령 기록 완료시 호출
        /// </summary>
        /// <returns></returns>
        bool CtlCharacterEnd();

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자가 있는지 여부</summary>
        /// <param name="asciiCode">아스키 코드 (0~255)</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool CtlCharacterSetIsExist(uint asciiCode, CharacterSet characterSet = CharacterSet._0);

        /// <summary>지정된 색인 문자열 집합을 삭제합니다</summary>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool CtlCharacterSetClear(CharacterSet characterSet);

        /// <summary>시리얼 번호 리셋</summary>
        /// <param name="serialNo">시작 번호</param>
        /// <param name="incrementStep">증가 값</param>
        /// <returns></returns>
        bool CtlSerialReset(uint serialNo, uint incrementStep = 1);

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 문자열 마킹</summary>
        /// <param name="text">문자열</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool ListText(string text, CharacterSet characterSet = CharacterSet._0);

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 날짜 마킹</summary>
        /// <param name="dateFormat">DateFormat 열거형</param>
        /// <param name="leadingWithZero">앞선 공간을 0 으로 채우기</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool ListDate(DateFormat dateFormat, bool leadingWithZero, CharacterSet characterSet = CharacterSet._0);

        /// <summary>
        /// 보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 시간 마킹
        /// 호출 시점의 윈도우즈 시스템 시간을 사용함
        /// </summary>
        /// <param name="timeFormat">TimeFormat 열거형</param>
        /// <param name="leadingWithZero">앞선 공간을 0 으로 채우기</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool ListTime(TimeFormat timeFormat, bool leadingWithZero, CharacterSet characterSet = CharacterSet._0);

        /// <summary>보호된 리스트 버퍼(3) 영역에 색인된 문자를 이용해 시리얼 번호 마킹</summary>
        /// <param name="numOfDigits">최대 자리수 (최대 15자)</param>
        /// <param name="serialFormat">SerialFormat 열거형</param>
        /// <param name="characterSet">CharacterSet 열거형</param>
        /// <returns></returns>
        bool ListSerial(uint numOfDigits, SerialFormat serialFormat, CharacterSet characterSet = CharacterSet._0);
    }
}
