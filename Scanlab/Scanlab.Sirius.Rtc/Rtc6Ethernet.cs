
using RTC6Import;
using System;
using System.Net;

namespace Scanlab.Sirius
{
    /// <summary>RTC6 이더넷 제품용 객체</summary>
    public class Rtc6Ethernet : Rtc6
    {
        /// <summary>IP 주소</summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>서브넷 마스크</summary>
        public IPAddress SubNetMask { get; set; }

        /// <summary>생성자</summary>
        public Rtc6Ethernet()
        {
        }

        /// <summary>생성자</summary>
        /// <param name="index"></param>
        /// <param name="ipAddress">IP 주소</param>
        /// <param name="subNetMask">서브넷 마스크</param>
        /// <param name="outputFileName">명령을 기록할 파일 이름</param>
        public Rtc6Ethernet(uint index, string ipAddress, string subNetMask = "255.255.255.0", string outputFileName = "")
          : base(index, outputFileName)
        {
            this.IpAddress = IPAddress.Parse(ipAddress);
            this.SubNetMask = IPAddress.Parse(subNetMask);
        }

        /// <summary>RTC6Ethernet 카드 통신 초기화</summary>
        /// <param name="kFactor"></param>
        /// <param name="laserMode"></param>
        /// <param name="ctbFileName"></param>
        /// <returns></returns>
        public override bool Initialize(float kFactor, LaserMode laserMode, string ctbFileName)
        {
            if (1U == Rtc6.Count)
            {
                uint dll_init_result = RTC6Wrap.init_rtc6_dll();
                if (!Convert.ToBoolean(dll_init_result & 1U))
                {
                    if (Convert.ToBoolean(dll_init_result & 256U))
                    {
                        Logger.Log(Logger.Type.Warn, string.Format("rtc6e [{0}]: program version mismatched. so trying to reload ...", (object)this.Index), Array.Empty<object>());
                    }
                    else
                    {
                        Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: {1}", (object)this.Index, (object)init_error(dll_init_result)), Array.Empty<object>());
                        return false;
                    }
                }

                string init_error(uint code)
                {
                    if (Convert.ToBoolean(dll_init_result & 1U))
                        return "no rtc board founded via init_rtc_dll";
                    if (Convert.ToBoolean(dll_init_result & 2U))
                        return "access denied via init_rtc_dll, select, acquire_rtc";
                    if (Convert.ToBoolean(dll_init_result & 4U))
                        return "command not forwarded. PCI or driver error";
                    if (Convert.ToBoolean(dll_init_result & 8U))
                        return "rtc timed out. no response from board";
                    if (Convert.ToBoolean(dll_init_result & 16U))
                        return "invalid parameter";
                    if (Convert.ToBoolean(dll_init_result & 32U))
                        return "List processing is (not) active";
                    if (Convert.ToBoolean(dll_init_result & 64U))
                        return "list command rejected, illegal input pointer";
                    if (Convert.ToBoolean(dll_init_result & 128U))
                        return "list command wad converted to a List_mop";
                    if (Convert.ToBoolean(dll_init_result & 256U))
                        return "dll, rtc or hex version error";
                    if (Convert.ToBoolean(dll_init_result & 512U))
                        return "download verification error. load_program_file ?";
                    if (Convert.ToBoolean(dll_init_result & 1024U))
                        return "DSP version is too old";
                    if (Convert.ToBoolean(dll_init_result & 2048U))
                        return "out of memeory. dll internal windows memory request failed";
                    if (Convert.ToBoolean(dll_init_result & 4096U))
                        return "EEPROM read or write error";
                    return Convert.ToBoolean(dll_init_result & 65536U) ? "error reading PCI configuration reqister druing init_rtc_dll" : string.Format("unknown error code : {0}", (object)dll_init_result);
                }
            }
            if ((long)(this.Index + 1U) != (long)RTC6Wrap.eth_assign_card_ip(RTC6Wrap.eth_convert_string_to_ip(this.IpAddress.ToString()), this.Index + 1U))
            {
                Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: fail to eth_assign_card_ip", (object)this.Index), Array.Empty<object>());
                return false;
            }
            if ((int)this.Index + 1 != (int)RTC6Wrap.select_rtc(this.Index + 1U))
            {
                Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: fail to select_rtc", (object)this.Index), Array.Empty<object>());
                return false;
            }
            if (1U != RTC6Wrap.n_eth_check_connection(this.Index + 1U))
            {
                Logger.Log(Logger.Type.Info, string.Format("rtc6e [{0}]: communication is not established : {1}", (object)this.Index, (object)this.CtlGetErrMsg(RTC6Wrap.n_eth_get_last_error(this.Index + 1U))), Array.Empty<object>());
                return false;
            }
            RTC6Wrap.n_stop_execution(this.Index + 1U);
            switch (RTC6Wrap.n_load_program_file(this.Index + 1U, string.Empty))
            {
                case 0:
                    uint Code = RTC6Wrap.n_eth_get_last_error(this.Index + 1U);
                    if (Code != 0U)
                        RTC6Wrap.n_reset_error(this.Index + 1U, Code);
                    int num1 = (int)RTC6Wrap.eth_count_cards();
                    int num2 = (int)RTC6Wrap.get_dll_version();
                    uint[] Ptr = new uint[16];
                    RTC6Wrap.eth_get_card_info(this.Index + 1U, Ptr);
                    uint num3 = Ptr[0];
                    uint num4 = Ptr[1];
                    this.KFactor = kFactor;
                    this.IsMOTF = Convert.ToBoolean(num3 & 256U);
                    this.Is2ndHead = Convert.ToBoolean(num3 & 512U);
                    this.Is3D = Convert.ToBoolean(num3 & 1024U);
                    RTC6Wrap.n_set_laser_control(this.Index + 1U, 0U);
                    if (!this.CtlLoadCorrectionFile(CorrectionTableIndex.Table1, ctbFileName))
                        return false;
                    if (!this.Is3D)
                    {
                        if (!this.CtlSelectCorrection(CorrectionTableIndex.Table1, CorrectionTableIndex.None))
                            return false;
                    }
                    else if (!this.CtlSelectCorrection(CorrectionTableIndex.Table1, CorrectionTableIndex.Table1))
                        return false;
                    this.KZFactor = kFactor;
                    RTC6Wrap.n_set_laser_mode(this.Index + 1U, (uint)laserMode);
                    RTC6Wrap.n_set_firstpulse_killer(this.Index + 1U, 0U);
                    RTC6Wrap.n_set_standby(this.Index + 1U, 0U, 0U);
                    RTC6Wrap.n_time_update(this.Index + 1U);
                    RTC6Wrap.n_config_list(this.Index + 1U, this.RTC6_LIST_BUFFER_MAX * 2U, this.RTC6_LIST_BUFFER_MAX * 2U);
                    Logger.Log(Logger.Type.Info, string.Format("rtc6e [{0}]: serial no = {1} initialized", (object)this.Index, (object)num4), Array.Empty<object>());
                    return true;
                case 2:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: BOARD NOT RUNNING. If a renewed call does not bring success, then a PowerCycle is necessary.", (object)this.Index), Array.Empty<object>());
                    break;
                case 3:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: RTC6DAT.dat file or RTC6RBF.rbf file not found.", (object)this.Index), Array.Empty<object>());
                    break;
                case 5:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Not enough WINDOWS memory", (object)this.Index), Array.Empty<object>());
                    break;
                case 6:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Access error: the board is reserved for another user program.", (object)this.Index), Array.Empty<object>());
                    break;
                case 7:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Version error: DLL version (RTC6DLL.dll), RTC version(firmware file RTC6RBF.rbf) and HEX version(DSP program file RTC6OUT.out) incompatible with another.", (object)this.Index), Array.Empty<object>());
                    break;
                case 8:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: System driver not found.", (object)this.Index), Array.Empty<object>());
                    break;
                case 9:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Loading of RTC6OUT.out file failed or RTC6OUT.out has incorrect format or other error.", (object)this.Index), Array.Empty<object>());
                    break;
                case 11:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Firmware error: Loading of RTC6RBF.rbf file failed.", (object)this.Index), Array.Empty<object>());
                    break;
                case 12:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Error opening/reading file (RTC6DAT.dat).", (object)this.Index), Array.Empty<object>());
                    break;
                case 14:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: DSP memory error", (object)this.Index), Array.Empty<object>());
                    break;
                case 15:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Verify memory error.", (object)this.Index), Array.Empty<object>());
                    break;
                case 17:
                    Logger.Log(Logger.Type.Error, string.Format("rtc6e [{0}]: Ethernet error : {1}", (object)this.Index, (object)this.CtlGetErrMsg(RTC6Wrap.n_eth_get_last_error(this.Index + 1U))), Array.Empty<object>());
                    break;
            }
            return false;
        }

        /// <summary>RTC 카드의 상태 확인</summary>
        /// <param name="status">RtcStatus 열거형</param>
        /// <returns></returns>
        public override bool CtlGetStatus(RtcStatus status)
        {
            bool flag1;
            if (status == RtcStatus.NoError)
            {
                int num1 = this.CtlGetStatus(RtcStatus.Aborted) ? 1 : 0;
                uint num2 = RTC6Wrap.n_eth_get_last_error(this.Index + 1U);
                uint num3 = RTC6Wrap.n_eth_check_connection(this.Index + 1U);
                bool flag2 = num2 > 0U;
                flag1 = num1 == 0 && !flag2 && 1U == num3;
            }
            else
                flag1 = base.CtlGetStatus(status);
            return flag1;
        }

        /// <summary>에러상태를 해제</summary>
        /// <returns></returns>
        public override bool CtlReset()
        {
            uint Code = RTC6Wrap.n_eth_get_last_error(this.Index + 1U);
            if (Code != 0U)
                RTC6Wrap.n_reset_error(this.Index + 1U, Code);
            base.CtlReset();
            this.isAborted = false;
            return true;
        }

        /// <summary>RTC6 내부 에러코드에 해당하는 메시지</summary>
        /// <param name="errorCode">에러코드</param>
        /// <returns></returns>
        public override string CtlGetErrMsg(uint errorCode)
        {
            if (errorCode == 0U)
                return "";
            uint num = RTC6Wrap.n_eth_get_error(this.Index + 1U);
            if (Convert.ToBoolean(num & 1U))
                return "wsasstartup_failed";
            if (Convert.ToBoolean(num & 2U))
                return "wrong_winsock_version";
            if (Convert.ToBoolean(num & 4U))
                return "Reserved.";
            if (Convert.ToBoolean(num & 8U))
                return "udp_create_socket_error";
            if (Convert.ToBoolean(num & 16U))
                return "udp_bind_socket_error";
            if (Convert.ToBoolean(num & 32U))
                return "udp_connect_socket_error";
            if (Convert.ToBoolean(num & 64U))
                return "udp_excl_add_use_error";
            if (Convert.ToBoolean(num & 128U))
                return "udp_bc_ena_error";
            if (Convert.ToBoolean(num & 256U))
                return "Reserved.";
            if (Convert.ToBoolean(num & 512U))
                return "tcp_create_socket_error";
            if (Convert.ToBoolean(num & 1024U))
                return "tcp_excl_add_use_error";
            if (Convert.ToBoolean(num & 2048U))
                return "tcp_bind_socket_error";
            if (Convert.ToBoolean(num & 4096U))
                return "Reserved.";
            if (Convert.ToBoolean(num & 8192U))
                return "tcp_connect_socket_error";
            if (Convert.ToBoolean(num & 16384U))
                return "tcp_connect_sel_error";
            if (Convert.ToBoolean(num & 32768U))
                return "tcp_connect_fds_error";
            if (Convert.ToBoolean(num & 65536U))
                return "tcp_nodelay_error";
            if (Convert.ToBoolean(num & 131072U))
                return "create_thread_failed";
            if (Convert.ToBoolean(num & 262144U))
                return "udp_recv_error";
            if (Convert.ToBoolean(num & 524288U))
                return "udp_send_error";
            if (Convert.ToBoolean(num & 1048576U))
                return "udp_recv_timeout";
            if (Convert.ToBoolean(num & 2097152U))
                return "already_acquired";
            if (Convert.ToBoolean(num & 4194304U))
                return "not_acquired";
            if (Convert.ToBoolean(num & 8388608U))
                return "access_denied";
            if (Convert.ToBoolean(num & 16777216U))
                return "send_tgm_timeout";
            if (Convert.ToBoolean(num & 33554432U))
                return "card_not_found";
            return Convert.ToBoolean(num & 67108864U) ? "core1_timeout" : string.Format("unknown error code : {0}", (object)errorCode);
        }

        public enum RtcEthernetCardInfo
        {
            FirwareVersion,
            SerialNo,
            RtcIpAddress,
            RtcMACAddress,
            IsAcquired,
            ClientIpAddress,
            IsForcedDHCP,
            RtcStaticIPAddress,
            RtcStaticNetMask,
            RtcStaticGateWay,
            RtcUDPPortForSearching,
            RtcUPDPortForExclusive,
            RtcTCPPortForExclusive,
        }
    }
}
