// Decompiled with JetBrains decompiler
// Type: RTC6Import.RTC6Wrap
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.rtc.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RTC6Import
{
    /// <summary>
    /// Notice that the construction of the RTC6Import object or an initial
    /// call of any RTC6Import method may throw a TypeInitializationException
    /// exception, which indicates that the required DLL is missing or the
    /// import of a particular DLL function failed. In order to analyze and
    /// properly handle such an error condition you need to catch that
    /// TypeInitializationException type exception.
    /// </summary>
    public class RTC6Wrap
    {
        /// <summary>dll 초기화 여부</summary>
        public static bool Initialized;
        private const int TableSize = 1024;
        private const int SampleArraySize = 1048576;
        private const int SignalSize = 4;
        private const int TransformSize = 132130;
        private const int SignalSize2 = 8;
        private const string DLL_NAMEx86 = "RTC6DLL.dll";
        private const string DLL_NAMEx64 = "RTC6DLLx64.dll";
        /// <summary>uint init_rtc6_dll();</summary>
        public static RTC6Wrap.init_rtc6_dllDelegate init_rtc6_dll = (RTC6Wrap.init_rtc6_dllDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.init_rtc6_dllDelegate>(nameof(init_rtc6_dll));
        /// <summary>void free_rtc6_dll();</summary>
        public static RTC6Wrap.free_rtc6_dllDelegate free_rtc6_dll = (RTC6Wrap.free_rtc6_dllDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.free_rtc6_dllDelegate>(nameof(free_rtc6_dll));
        /// <summary>void set_rtc4_mode();</summary>
        public static RTC6Wrap.set_rtc4_modeDelegate set_rtc4_mode = (RTC6Wrap.set_rtc4_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_rtc4_modeDelegate>(nameof(set_rtc4_mode));
        /// <summary>void set_rtc5_mode();</summary>
        public static RTC6Wrap.set_rtc5_modeDelegate set_rtc5_mode = (RTC6Wrap.set_rtc5_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_rtc5_modeDelegate>(nameof(set_rtc5_mode));
        /// <summary>void set_rtc6_mode();</summary>
        public static RTC6Wrap.set_rtc6_modeDelegate set_rtc6_mode = (RTC6Wrap.set_rtc6_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_rtc6_modeDelegate>(nameof(set_rtc6_mode));
        /// <summary>uint get_rtc_mode();</summary>
        public static RTC6Wrap.get_rtc_modeDelegate get_rtc_mode = (RTC6Wrap.get_rtc_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_rtc_modeDelegate>(nameof(get_rtc_mode));
        /// <summary>uint n_get_error(uint CardNo);</summary>
        public static RTC6Wrap.n_get_errorDelegate n_get_error = (RTC6Wrap.n_get_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_errorDelegate>(nameof(n_get_error));
        /// <summary>uint n_get_last_error(uint CardNo);</summary>
        public static RTC6Wrap.n_get_last_errorDelegate n_get_last_error = (RTC6Wrap.n_get_last_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_last_errorDelegate>(nameof(n_get_last_error));
        /// <summary>void n_reset_error(uint CardNo, uint Code);</summary>
        public static RTC6Wrap.n_reset_errorDelegate n_reset_error = (RTC6Wrap.n_reset_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_reset_errorDelegate>(nameof(n_reset_error));
        /// <summary>uint n_set_verify(uint CardNo, uint Verify);</summary>
        public static RTC6Wrap.n_set_verifyDelegate n_set_verify = (RTC6Wrap.n_set_verifyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_verifyDelegate>(nameof(n_set_verify));
        /// <summary>uint get_error();</summary>
        public static RTC6Wrap.get_errorDelegate get_error = (RTC6Wrap.get_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_errorDelegate>(nameof(get_error));
        /// <summary>uint get_last_error();</summary>
        public static RTC6Wrap.get_last_errorDelegate get_last_error = (RTC6Wrap.get_last_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_last_errorDelegate>(nameof(get_last_error));
        /// <summary>void reset_error(uint Code);</summary>
        public static RTC6Wrap.reset_errorDelegate reset_error = (RTC6Wrap.reset_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.reset_errorDelegate>(nameof(reset_error));
        /// <summary>uint set_verify(uint Verify);</summary>
        public static RTC6Wrap.set_verifyDelegate set_verify = (RTC6Wrap.set_verifyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_verifyDelegate>(nameof(set_verify));
        /// <summary>uint verify_checksum(string Name);</summary>
        public static RTC6Wrap.verify_checksumDelegate verify_checksum = (RTC6Wrap.verify_checksumDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.verify_checksumDelegate>(nameof(verify_checksum));
        /// <summary>uint eth_count_cards();</summary>
        public static RTC6Wrap.eth_count_cardsDelegate eth_count_cards = (RTC6Wrap.eth_count_cardsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_count_cardsDelegate>(nameof(eth_count_cards));
        /// <summary>uint eth_found_cards();</summary>
        public static RTC6Wrap.eth_found_cardsDelegate eth_found_cards = (RTC6Wrap.eth_found_cardsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_found_cardsDelegate>(nameof(eth_found_cards));
        /// <summary>uint eth_max_card();</summary>
        public static RTC6Wrap.eth_max_cardDelegate eth_max_card = (RTC6Wrap.eth_max_cardDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_max_cardDelegate>(nameof(eth_max_card));
        /// <summary>int eth_remove_card(uint CardNo);</summary>
        public static RTC6Wrap.eth_remove_cardDelegate eth_remove_card = (RTC6Wrap.eth_remove_cardDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_remove_cardDelegate>(nameof(eth_remove_card));
        /// <summary>void eth_get_card_info(uint CardNo, uint[] Ptr);</summary>
        public static RTC6Wrap.eth_get_card_infoDelegate eth_get_card_info = (RTC6Wrap.eth_get_card_infoDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_card_infoDelegate>(nameof(eth_get_card_info));
        /// <summary>
        ///  void eth_get_card_info_search(uint SearchNo, uint[] Ptr);
        /// </summary>
        public static RTC6Wrap.eth_get_card_info_searchDelegate eth_get_card_info_search = (RTC6Wrap.eth_get_card_info_searchDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_card_info_searchDelegate>(nameof(eth_get_card_info_search));
        /// <summary>void eth_set_search_cards_timeout(uint TimeOut);</summary>
        public static RTC6Wrap.eth_set_search_cards_timeoutDelegate eth_set_search_cards_timeout = (RTC6Wrap.eth_set_search_cards_timeoutDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_set_search_cards_timeoutDelegate>(nameof(eth_set_search_cards_timeout));
        /// <summary>uint eth_search_cards(uint Ip, uint NetMask);</summary>
        public static RTC6Wrap.eth_search_cardsDelegate eth_search_cards = (RTC6Wrap.eth_search_cardsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_search_cardsDelegate>(nameof(eth_search_cards));
        /// <summary>uint eth_search_cards_range(uint StartIp, uint EndIp);</summary>
        public static RTC6Wrap.eth_search_cards_rangeDelegate eth_search_cards_range = (RTC6Wrap.eth_search_cards_rangeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_search_cards_rangeDelegate>(nameof(eth_search_cards_range));
        /// <summary>int eth_assign_card_ip(uint Ip, uint CardNo);</summary>
        public static RTC6Wrap.eth_assign_card_ipDelegate eth_assign_card_ip = (RTC6Wrap.eth_assign_card_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_assign_card_ipDelegate>(nameof(eth_assign_card_ip));
        /// <summary>int eth_assign_card(uint SearchNo, uint CardNo);</summary>
        public static RTC6Wrap.eth_assign_cardDelegate eth_assign_card = (RTC6Wrap.eth_assign_cardDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_assign_cardDelegate>(nameof(eth_assign_card));
        /// <summary>uint eth_convert_string_to_ip(string IpString);</summary>
        public static RTC6Wrap.eth_convert_string_to_ipDelegate eth_convert_string_to_ip = (RTC6Wrap.eth_convert_string_to_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_convert_string_to_ipDelegate>(nameof(eth_convert_string_to_ip));
        /// <summary>
        ///  void eth_convert_ip_to_string(uint Ip, uint[] IpString);
        /// </summary>
        public static RTC6Wrap.eth_convert_ip_to_stringDelegate eth_convert_ip_to_string = (RTC6Wrap.eth_convert_ip_to_stringDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_convert_ip_to_stringDelegate>(nameof(eth_convert_ip_to_string));
        /// <summary>uint eth_get_ip(uint CardNo);</summary>
        public static RTC6Wrap.eth_get_ipDelegate eth_get_ip = (RTC6Wrap.eth_get_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_ipDelegate>(nameof(eth_get_ip));
        /// <summary>uint eth_get_ip_search(uint SearchNo);</summary>
        public static RTC6Wrap.eth_get_ip_searchDelegate eth_get_ip_search = (RTC6Wrap.eth_get_ip_searchDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_ip_searchDelegate>(nameof(eth_get_ip_search));
        /// <summary>uint eth_get_serial_search(uint SearchNo);</summary>
        public static RTC6Wrap.eth_get_serial_searchDelegate eth_get_serial_search = (RTC6Wrap.eth_get_serial_searchDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_serial_searchDelegate>(nameof(eth_get_serial_search));
        /// <summary>uint n_eth_get_last_error(uint CardNo);</summary>
        public static RTC6Wrap.n_eth_get_last_errorDelegate n_eth_get_last_error = (RTC6Wrap.n_eth_get_last_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_get_last_errorDelegate>(nameof(n_eth_get_last_error));
        /// <summary>uint n_eth_get_error(uint CardNo);</summary>
        public static RTC6Wrap.n_eth_get_errorDelegate n_eth_get_error = (RTC6Wrap.n_eth_get_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_get_errorDelegate>(nameof(n_eth_get_error));
        /// <summary>uint n_eth_error_dump(uint CardNo, uint[] Dump);</summary>
        public static RTC6Wrap.n_eth_error_dumpDelegate n_eth_error_dump = (RTC6Wrap.n_eth_error_dumpDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_error_dumpDelegate>(nameof(n_eth_error_dump));
        /// <summary>
        ///  uint n_eth_set_static_ip(uint CardNo, uint Ip, uint NetMask, uint Gateway);
        /// </summary>
        public static RTC6Wrap.n_eth_set_static_ipDelegate n_eth_set_static_ip = (RTC6Wrap.n_eth_set_static_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_set_static_ipDelegate>(nameof(n_eth_set_static_ip));
        /// <summary>
        ///  uint n_eth_get_static_ip(uint CardNo, out uint Ip, out uint NetMask, out uint Gateway);
        /// </summary>
        public static RTC6Wrap.n_eth_get_static_ipDelegate n_eth_get_static_ip = (RTC6Wrap.n_eth_get_static_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_get_static_ipDelegate>(nameof(n_eth_get_static_ip));
        /// <summary>
        ///  uint n_eth_set_port_numbers(uint CardNo, uint UDPsearch, uint UDPexcl, uint TCP);
        /// </summary>
        public static RTC6Wrap.n_eth_set_port_numbersDelegate n_eth_set_port_numbers = (RTC6Wrap.n_eth_set_port_numbersDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_set_port_numbersDelegate>(nameof(n_eth_set_port_numbers));
        /// <summary>
        ///  uint n_eth_get_port_numbers(uint CardNo, out uint UDPsearch, out uint UDPexcl, out uint TCP);
        /// </summary>
        public static RTC6Wrap.n_eth_get_port_numbersDelegate n_eth_get_port_numbers = (RTC6Wrap.n_eth_get_port_numbersDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_get_port_numbersDelegate>(nameof(n_eth_get_port_numbers));
        /// <summary>
        ///  void n_eth_set_com_timeouts(uint CardNo, uint AcquireTimeout, uint AcquireMaxRetries, uint SendRecvTimeout, uint SendRecvMaxRetries, uint KeepAlive, uint KeepInterval);
        /// </summary>
        public static RTC6Wrap.n_eth_set_com_timeoutsDelegate n_eth_set_com_timeouts = (RTC6Wrap.n_eth_set_com_timeoutsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_set_com_timeoutsDelegate>(nameof(n_eth_set_com_timeouts));
        /// <summary>
        ///  void n_eth_get_com_timeouts(uint CardNo, out uint AcquireTimeout, out uint AcquireMaxRetries, out uint SendRecvTimeout, out uint SendRecvMaxRetries, out uint KeepAlive, out uint KeepInterval);
        /// </summary>
        public static RTC6Wrap.n_eth_get_com_timeoutsDelegate n_eth_get_com_timeouts = (RTC6Wrap.n_eth_get_com_timeoutsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_get_com_timeoutsDelegate>(nameof(n_eth_get_com_timeouts));
        /// <summary>uint n_eth_check_connection(uint CardNo);</summary>
        public static RTC6Wrap.n_eth_check_connectionDelegate n_eth_check_connection = (RTC6Wrap.n_eth_check_connectionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_check_connectionDelegate>(nameof(n_eth_check_connection));
        /// <summary>void n_set_eth_boot_control(uint CardNo, uint Ctrl);</summary>
        public static RTC6Wrap.n_set_eth_boot_controlDelegate n_set_eth_boot_control = (RTC6Wrap.n_set_eth_boot_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_eth_boot_controlDelegate>(nameof(n_set_eth_boot_control));
        /// <summary>void n_eth_boot_timeout(uint CardNo, uint Timeout);</summary>
        public static RTC6Wrap.n_eth_boot_timeoutDelegate n_eth_boot_timeout = (RTC6Wrap.n_eth_boot_timeoutDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_boot_timeoutDelegate>(nameof(n_eth_boot_timeout));
        /// <summary>void n_eth_boot_dcmd(uint CardNo);</summary>
        public static RTC6Wrap.n_eth_boot_dcmdDelegate n_eth_boot_dcmd = (RTC6Wrap.n_eth_boot_dcmdDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_eth_boot_dcmdDelegate>(nameof(n_eth_boot_dcmd));
        /// <summary>uint n_store_program(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_store_programDelegate n_store_program = (RTC6Wrap.n_store_programDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_store_programDelegate>(nameof(n_store_program));
        /// <summary>uint eth_get_last_error();</summary>
        public static RTC6Wrap.eth_get_last_errorDelegate eth_get_last_error = (RTC6Wrap.eth_get_last_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_last_errorDelegate>(nameof(eth_get_last_error));
        /// <summary>uint eth_get_error();</summary>
        public static RTC6Wrap.eth_get_errorDelegate eth_get_error = (RTC6Wrap.eth_get_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_errorDelegate>(nameof(eth_get_error));
        /// <summary>uint eth_error_dump(uint[] Dump);</summary>
        public static RTC6Wrap.eth_error_dumpDelegate eth_error_dump = (RTC6Wrap.eth_error_dumpDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_error_dumpDelegate>(nameof(eth_error_dump));
        /// <summary>
        ///  uint eth_set_static_ip(uint Ip, uint NetMask, uint Gateway);
        /// </summary>
        public static RTC6Wrap.eth_set_static_ipDelegate eth_set_static_ip = (RTC6Wrap.eth_set_static_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_set_static_ipDelegate>(nameof(eth_set_static_ip));
        /// <summary>
        ///  uint eth_get_static_ip(out uint Ip, out uint NetMask, out uint Gateway);
        /// </summary>
        public static RTC6Wrap.eth_get_static_ipDelegate eth_get_static_ip = (RTC6Wrap.eth_get_static_ipDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_static_ipDelegate>(nameof(eth_get_static_ip));
        /// <summary>
        ///  uint eth_set_port_numbers(uint UDPsearch, uint UDPexcl, uint TCP);
        /// </summary>
        public static RTC6Wrap.eth_set_port_numbersDelegate eth_set_port_numbers = (RTC6Wrap.eth_set_port_numbersDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_set_port_numbersDelegate>(nameof(eth_set_port_numbers));
        /// <summary>
        ///  uint eth_get_port_numbers(out uint UDPsearch, out uint UDPexcl, out uint TCP);
        /// </summary>
        public static RTC6Wrap.eth_get_port_numbersDelegate eth_get_port_numbers = (RTC6Wrap.eth_get_port_numbersDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_port_numbersDelegate>(nameof(eth_get_port_numbers));
        /// <summary>
        ///  void eth_set_com_timeouts(uint AcquireTimeout, uint AcquireMaxRetries, uint SendRecvTimeout, uint SendRecvMaxRetries, uint KeepAlive, uint KeepInterval);
        /// </summary>
        public static RTC6Wrap.eth_set_com_timeoutsDelegate eth_set_com_timeouts = (RTC6Wrap.eth_set_com_timeoutsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_set_com_timeoutsDelegate>(nameof(eth_set_com_timeouts));
        /// <summary>
        ///  void eth_get_com_timeouts(out uint AcquireTimeout, out uint AcquireMaxRetries, out uint SendRecvTimeout, out uint SendRecvMaxRetries, out uint KeepAlive, out uint KeepInterval);
        /// </summary>
        public static RTC6Wrap.eth_get_com_timeoutsDelegate eth_get_com_timeouts = (RTC6Wrap.eth_get_com_timeoutsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_get_com_timeoutsDelegate>(nameof(eth_get_com_timeouts));
        /// <summary>uint eth_check_connection();</summary>
        public static RTC6Wrap.eth_check_connectionDelegate eth_check_connection = (RTC6Wrap.eth_check_connectionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_check_connectionDelegate>(nameof(eth_check_connection));
        /// <summary>void set_eth_boot_control(uint Ctrl);</summary>
        public static RTC6Wrap.set_eth_boot_controlDelegate set_eth_boot_control = (RTC6Wrap.set_eth_boot_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_eth_boot_controlDelegate>(nameof(set_eth_boot_control));
        /// <summary>void eth_boot_timeout(uint Timeout);</summary>
        public static RTC6Wrap.eth_boot_timeoutDelegate eth_boot_timeout = (RTC6Wrap.eth_boot_timeoutDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_boot_timeoutDelegate>(nameof(eth_boot_timeout));
        /// <summary>void eth_boot_dcmd();</summary>
        public static RTC6Wrap.eth_boot_dcmdDelegate eth_boot_dcmd = (RTC6Wrap.eth_boot_dcmdDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.eth_boot_dcmdDelegate>(nameof(eth_boot_dcmd));
        /// <summary>uint store_program(uint Mode);</summary>
        public static RTC6Wrap.store_programDelegate store_program = (RTC6Wrap.store_programDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.store_programDelegate>(nameof(store_program));
        /// <summary>
        ///  uint read_abc_from_file(string Name, out double A, out double B, out double C);
        /// </summary>
        public static RTC6Wrap.read_abc_from_fileDelegate read_abc_from_file = (RTC6Wrap.read_abc_from_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_abc_from_fileDelegate>(nameof(read_abc_from_file));
        /// <summary>
        ///  uint write_abc_to_file(string Name, double A, double B, double C);
        /// </summary>
        public static RTC6Wrap.write_abc_to_fileDelegate write_abc_to_file = (RTC6Wrap.write_abc_to_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_abc_to_fileDelegate>(nameof(write_abc_to_file));
        /// <summary>uint n_create_dat_file(uint CardNo, int Flag);</summary>
        public static RTC6Wrap.n_create_dat_fileDelegate n_create_dat_file = (RTC6Wrap.n_create_dat_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_create_dat_fileDelegate>(nameof(n_create_dat_file));
        /// <summary>uint create_dat_file(int Flag);</summary>
        public static RTC6Wrap.create_dat_fileDelegate create_dat_file = (RTC6Wrap.create_dat_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.create_dat_fileDelegate>(nameof(create_dat_file));
        /// <summary>
        ///  uint transform(out int Sig1, out int Sig2, uint[] Ptr, uint Code);
        /// </summary>
        public static RTC6Wrap.transformDelegate transform = (RTC6Wrap.transformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.transformDelegate>(nameof(transform));
        /// <summary>uint rtc6_count_cards();</summary>
        public static RTC6Wrap.rtc6_count_cardsDelegate rtc6_count_cards = (RTC6Wrap.rtc6_count_cardsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.rtc6_count_cardsDelegate>(nameof(rtc6_count_cards));
        /// <summary>uint acquire_rtc(uint CardNo);</summary>
        public static RTC6Wrap.acquire_rtcDelegate acquire_rtc = (RTC6Wrap.acquire_rtcDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.acquire_rtcDelegate>(nameof(acquire_rtc));
        /// <summary>uint release_rtc(uint CardNo);</summary>
        public static RTC6Wrap.release_rtcDelegate release_rtc = (RTC6Wrap.release_rtcDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.release_rtcDelegate>(nameof(release_rtc));
        /// <summary>uint select_rtc(uint CardNo);</summary>
        public static RTC6Wrap.select_rtcDelegate select_rtc = (RTC6Wrap.select_rtcDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.select_rtcDelegate>(nameof(select_rtc));
        /// <summary>uint get_dll_version();</summary>
        public static RTC6Wrap.get_dll_versionDelegate get_dll_version = (RTC6Wrap.get_dll_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_dll_versionDelegate>(nameof(get_dll_version));
        /// <summary>uint n_get_card_type(uint CardNo);</summary>
        public static RTC6Wrap.n_get_card_typeDelegate n_get_card_type = (RTC6Wrap.n_get_card_typeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_card_typeDelegate>(nameof(n_get_card_type));
        /// <summary>uint n_get_serial_number(uint CardNo);</summary>
        public static RTC6Wrap.n_get_serial_numberDelegate n_get_serial_number = (RTC6Wrap.n_get_serial_numberDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_serial_numberDelegate>(nameof(n_get_serial_number));
        /// <summary>uint n_get_hex_version(uint CardNo);</summary>
        public static RTC6Wrap.n_get_hex_versionDelegate n_get_hex_version = (RTC6Wrap.n_get_hex_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_hex_versionDelegate>(nameof(n_get_hex_version));
        /// <summary>uint n_get_rtc_version(uint CardNo);</summary>
        public static RTC6Wrap.n_get_rtc_versionDelegate n_get_rtc_version = (RTC6Wrap.n_get_rtc_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_rtc_versionDelegate>(nameof(n_get_rtc_version));
        /// <summary>uint n_get_bios_version(uint CardNo);</summary>
        public static RTC6Wrap.n_get_bios_versionDelegate n_get_bios_version = (RTC6Wrap.n_get_bios_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_bios_versionDelegate>(nameof(n_get_bios_version));
        /// <summary>uint get_card_type();</summary>
        public static RTC6Wrap.get_card_typeDelegate get_card_type = (RTC6Wrap.get_card_typeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_card_typeDelegate>(nameof(get_card_type));
        /// <summary>uint get_serial_number();</summary>
        public static RTC6Wrap.get_serial_numberDelegate get_serial_number = (RTC6Wrap.get_serial_numberDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_serial_numberDelegate>(nameof(get_serial_number));
        /// <summary>uint get_hex_version();</summary>
        public static RTC6Wrap.get_hex_versionDelegate get_hex_version = (RTC6Wrap.get_hex_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_hex_versionDelegate>(nameof(get_hex_version));
        /// <summary>uint get_rtc_version();</summary>
        public static RTC6Wrap.get_rtc_versionDelegate get_rtc_version = (RTC6Wrap.get_rtc_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_rtc_versionDelegate>(nameof(get_rtc_version));
        /// <summary>uint get_bios_version();</summary>
        public static RTC6Wrap.get_bios_versionDelegate get_bios_version = (RTC6Wrap.get_bios_versionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_bios_versionDelegate>(nameof(get_bios_version));
        /// <summary>uint n_load_program_file(uint CardNo, string Path);</summary>
        public static RTC6Wrap.n_load_program_fileDelegate n_load_program_file = (RTC6Wrap.n_load_program_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_program_fileDelegate>(nameof(n_load_program_file));
        /// <summary>void n_sync_slaves(uint CardNo);</summary>
        public static RTC6Wrap.n_sync_slavesDelegate n_sync_slaves = (RTC6Wrap.n_sync_slavesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sync_slavesDelegate>(nameof(n_sync_slaves));
        /// <summary>uint n_get_sync_status(uint CardNo);</summary>
        public static RTC6Wrap.n_get_sync_statusDelegate n_get_sync_status = (RTC6Wrap.n_get_sync_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_sync_statusDelegate>(nameof(n_get_sync_status));
        /// <summary>void n_master_slave_config(uint CardNo, uint Flags);</summary>
        public static RTC6Wrap.n_master_slave_configDelegate n_master_slave_config = (RTC6Wrap.n_master_slave_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_master_slave_configDelegate>(nameof(n_master_slave_config));
        /// <summary>
        ///  uint n_load_correction_file(uint CardNo, string Name, uint No, uint Dim);
        /// </summary>
        public static RTC6Wrap.n_load_correction_fileDelegate n_load_correction_file = (RTC6Wrap.n_load_correction_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_correction_fileDelegate>(nameof(n_load_correction_file));
        /// <summary>
        ///  uint n_load_zoom_correction_file(uint CardNo, string Name, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_zoom_correction_fileDelegate n_load_zoom_correction_file = (RTC6Wrap.n_load_zoom_correction_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_zoom_correction_fileDelegate>(nameof(n_load_zoom_correction_file));
        /// <summary>
        ///  uint n_load_oct_table_no(uint CardNo, double A, double B, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_oct_table_noDelegate n_load_oct_table_no = (RTC6Wrap.n_load_oct_table_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_oct_table_noDelegate>(nameof(n_load_oct_table_no));
        /// <summary>
        ///  uint n_load_z_table_no(uint CardNo, double A, double B, double C, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_z_table_noDelegate n_load_z_table_no = (RTC6Wrap.n_load_z_table_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_z_table_noDelegate>(nameof(n_load_z_table_no));
        /// <summary>
        ///  uint n_load_z_table(uint CardNo, double A, double B, double C);
        /// </summary>
        public static RTC6Wrap.n_load_z_tableDelegate n_load_z_table = (RTC6Wrap.n_load_z_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_z_tableDelegate>(nameof(n_load_z_table));
        /// <summary>
        ///  void n_select_cor_table(uint CardNo, uint HeadA, uint HeadB);
        /// </summary>
        public static RTC6Wrap.n_select_cor_tableDelegate n_select_cor_table = (RTC6Wrap.n_select_cor_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_select_cor_tableDelegate>(nameof(n_select_cor_table));
        /// <summary>uint n_set_dsp_mode(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_set_dsp_modeDelegate n_set_dsp_mode = (RTC6Wrap.n_set_dsp_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_dsp_modeDelegate>(nameof(n_set_dsp_mode));
        /// <summary>
        ///  int n_load_stretch_table(uint CardNo, string Name, int No, uint TableNo);
        /// </summary>
        public static RTC6Wrap.n_load_stretch_tableDelegate n_load_stretch_table = (RTC6Wrap.n_load_stretch_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_stretch_tableDelegate>(nameof(n_load_stretch_table));
        /// <summary>
        ///  void n_number_of_correction_tables(uint CardNo, uint Number);
        /// </summary>
        public static RTC6Wrap.n_number_of_correction_tablesDelegate n_number_of_correction_tables = (RTC6Wrap.n_number_of_correction_tablesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_number_of_correction_tablesDelegate>(nameof(n_number_of_correction_tables));
        /// <summary>
        ///  double n_get_head_para(uint CardNo, uint HeadNo, uint ParaNo);
        /// </summary>
        public static RTC6Wrap.n_get_head_paraDelegate n_get_head_para = (RTC6Wrap.n_get_head_paraDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_head_paraDelegate>(nameof(n_get_head_para));
        /// <summary>
        ///  double n_get_table_para(uint CardNo, uint TableNo, uint ParaNo);
        /// </summary>
        public static RTC6Wrap.n_get_table_paraDelegate n_get_table_para = (RTC6Wrap.n_get_table_paraDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_table_paraDelegate>(nameof(n_get_table_para));
        /// <summary>uint load_program_file(string Path);</summary>
        public static RTC6Wrap.load_program_fileDelegate load_program_file = (RTC6Wrap.load_program_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_program_fileDelegate>(nameof(load_program_file));
        /// <summary>void sync_slaves();</summary>
        public static RTC6Wrap.sync_slavesDelegate sync_slaves = (RTC6Wrap.sync_slavesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sync_slavesDelegate>(nameof(sync_slaves));
        /// <summary>uint get_sync_status();</summary>
        public static RTC6Wrap.get_sync_statusDelegate get_sync_status = (RTC6Wrap.get_sync_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_sync_statusDelegate>(nameof(get_sync_status));
        /// <summary>void master_slave_config(uint Flags);</summary>
        public static RTC6Wrap.master_slave_configDelegate master_slave_config = (RTC6Wrap.master_slave_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.master_slave_configDelegate>(nameof(master_slave_config));
        /// <summary>
        ///  uint load_correction_file(string Name, uint No, uint Dim);
        /// </summary>
        public static RTC6Wrap.load_correction_fileDelegate load_correction_file = (RTC6Wrap.load_correction_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_correction_fileDelegate>(nameof(load_correction_file));
        /// <summary>uint load_zoom_correction_file(string Name, uint No);</summary>
        public static RTC6Wrap.load_zoom_correction_fileDelegate load_zoom_correction_file = (RTC6Wrap.load_zoom_correction_fileDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_zoom_correction_fileDelegate>(nameof(load_zoom_correction_file));
        /// <summary>uint load_oct_table_no(double A, double B, uint No);</summary>
        public static RTC6Wrap.load_oct_table_noDelegate load_oct_table_no = (RTC6Wrap.load_oct_table_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_oct_table_noDelegate>(nameof(load_oct_table_no));
        /// <summary>
        ///  uint load_z_table_no(double A, double B, double C, uint No);
        /// </summary>
        public static RTC6Wrap.load_z_table_noDelegate load_z_table_no = (RTC6Wrap.load_z_table_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_z_table_noDelegate>(nameof(load_z_table_no));
        /// <summary>uint load_z_table(double A, double B, double C);</summary>
        public static RTC6Wrap.load_z_tableDelegate load_z_table = (RTC6Wrap.load_z_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_z_tableDelegate>(nameof(load_z_table));
        /// <summary>void select_cor_table(uint HeadA, uint HeadB);</summary>
        public static RTC6Wrap.select_cor_tableDelegate select_cor_table = (RTC6Wrap.select_cor_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.select_cor_tableDelegate>(nameof(select_cor_table));
        /// <summary>uint set_dsp_mode(uint Mode);</summary>
        public static RTC6Wrap.set_dsp_modeDelegate set_dsp_mode = (RTC6Wrap.set_dsp_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_dsp_modeDelegate>(nameof(set_dsp_mode));
        /// <summary>
        ///  int load_stretch_table(string Name, int No, uint TableNo);
        /// </summary>
        public static RTC6Wrap.load_stretch_tableDelegate load_stretch_table = (RTC6Wrap.load_stretch_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_stretch_tableDelegate>(nameof(load_stretch_table));
        /// <summary>void number_of_correction_tables(uint Number);</summary>
        public static RTC6Wrap.number_of_correction_tablesDelegate number_of_correction_tables = (RTC6Wrap.number_of_correction_tablesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.number_of_correction_tablesDelegate>(nameof(number_of_correction_tables));
        /// <summary>double get_head_para(uint HeadNo, uint ParaNo);</summary>
        public static RTC6Wrap.get_head_paraDelegate get_head_para = (RTC6Wrap.get_head_paraDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_head_paraDelegate>(nameof(get_head_para));
        /// <summary>double get_table_para(uint TableNo, uint ParaNo);</summary>
        public static RTC6Wrap.get_table_paraDelegate get_table_para = (RTC6Wrap.get_table_paraDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_table_paraDelegate>(nameof(get_table_para));
        /// <summary>void n_config_list(uint CardNo, uint Mem1, uint Mem2);</summary>
        public static RTC6Wrap.n_config_listDelegate n_config_list = (RTC6Wrap.n_config_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_config_listDelegate>(nameof(n_config_list));
        /// <summary>void n_get_config_list(uint CardNo);</summary>
        public static RTC6Wrap.n_get_config_listDelegate n_get_config_list = (RTC6Wrap.n_get_config_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_config_listDelegate>(nameof(n_get_config_list));
        /// <summary>uint n_save_disk(uint CardNo, string Name, uint Mode);</summary>
        public static RTC6Wrap.n_save_diskDelegate n_save_disk = (RTC6Wrap.n_save_diskDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_save_diskDelegate>(nameof(n_save_disk));
        /// <summary>uint n_load_disk(uint CardNo, string Name, uint Mode);</summary>
        public static RTC6Wrap.n_load_diskDelegate n_load_disk = (RTC6Wrap.n_load_diskDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_diskDelegate>(nameof(n_load_disk));
        /// <summary>uint n_get_list_space(uint CardNo);</summary>
        public static RTC6Wrap.n_get_list_spaceDelegate n_get_list_space = (RTC6Wrap.n_get_list_spaceDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_list_spaceDelegate>(nameof(n_get_list_space));
        /// <summary>void config_list(uint Mem1, uint Mem2);</summary>
        public static RTC6Wrap.config_listDelegate config_list = (RTC6Wrap.config_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.config_listDelegate>(nameof(config_list));
        /// <summary>void get_config_list();</summary>
        public static RTC6Wrap.get_config_listDelegate get_config_list = (RTC6Wrap.get_config_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_config_listDelegate>(nameof(get_config_list));
        /// <summary>uint save_disk(string Name, uint Mode);</summary>
        public static RTC6Wrap.save_diskDelegate save_disk = (RTC6Wrap.save_diskDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.save_diskDelegate>(nameof(save_disk));
        /// <summary>uint load_disk(string Name, uint Mode);</summary>
        public static RTC6Wrap.load_diskDelegate load_disk = (RTC6Wrap.load_diskDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_diskDelegate>(nameof(load_disk));
        /// <summary>uint get_list_space();</summary>
        public static RTC6Wrap.get_list_spaceDelegate get_list_space = (RTC6Wrap.get_list_spaceDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_list_spaceDelegate>(nameof(get_list_space));
        /// <summary>
        ///  void n_set_start_list_pos(uint CardNo, uint ListNo, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_set_start_list_posDelegate n_set_start_list_pos = (RTC6Wrap.n_set_start_list_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_start_list_posDelegate>(nameof(n_set_start_list_pos));
        /// <summary>void n_set_start_list(uint CardNo, uint ListNo);</summary>
        public static RTC6Wrap.n_set_start_listDelegate n_set_start_list = (RTC6Wrap.n_set_start_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_start_listDelegate>(nameof(n_set_start_list));
        /// <summary>void n_set_start_list_1(uint CardNo);</summary>
        public static RTC6Wrap.n_set_start_list_1Delegate n_set_start_list_1 = (RTC6Wrap.n_set_start_list_1Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_start_list_1Delegate>(nameof(n_set_start_list_1));
        /// <summary>void n_set_start_list_2(uint CardNo);</summary>
        public static RTC6Wrap.n_set_start_list_2Delegate n_set_start_list_2 = (RTC6Wrap.n_set_start_list_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_start_list_2Delegate>(nameof(n_set_start_list_2));
        /// <summary>void n_set_input_pointer(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_set_input_pointerDelegate n_set_input_pointer = (RTC6Wrap.n_set_input_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_input_pointerDelegate>(nameof(n_set_input_pointer));
        /// <summary>uint n_load_list(uint CardNo, uint ListNo, uint Pos);</summary>
        public static RTC6Wrap.n_load_listDelegate n_load_list = (RTC6Wrap.n_load_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_listDelegate>(nameof(n_load_list));
        /// <summary>void n_load_sub(uint CardNo, uint Index);</summary>
        public static RTC6Wrap.n_load_subDelegate n_load_sub = (RTC6Wrap.n_load_subDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_subDelegate>(nameof(n_load_sub));
        /// <summary>void n_load_char(uint CardNo, uint Char);</summary>
        public static RTC6Wrap.n_load_charDelegate n_load_char = (RTC6Wrap.n_load_charDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_charDelegate>(nameof(n_load_char));
        /// <summary>void n_load_text_table(uint CardNo, uint Index);</summary>
        public static RTC6Wrap.n_load_text_tableDelegate n_load_text_table = (RTC6Wrap.n_load_text_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_text_tableDelegate>(nameof(n_load_text_table));
        /// <summary>
        ///  void n_get_list_pointer(uint CardNo, out uint ListNo, out uint Pos);
        /// </summary>
        public static RTC6Wrap.n_get_list_pointerDelegate n_get_list_pointer = (RTC6Wrap.n_get_list_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_list_pointerDelegate>(nameof(n_get_list_pointer));
        /// <summary>uint n_get_input_pointer(uint CardNo);</summary>
        public static RTC6Wrap.n_get_input_pointerDelegate n_get_input_pointer = (RTC6Wrap.n_get_input_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_input_pointerDelegate>(nameof(n_get_input_pointer));
        /// <summary>void set_start_list_pos(uint ListNo, uint Pos);</summary>
        public static RTC6Wrap.set_start_list_posDelegate set_start_list_pos = (RTC6Wrap.set_start_list_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_start_list_posDelegate>(nameof(set_start_list_pos));
        /// <summary>void set_start_list(uint ListNo);</summary>
        public static RTC6Wrap.set_start_listDelegate set_start_list = (RTC6Wrap.set_start_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_start_listDelegate>(nameof(set_start_list));
        /// <summary>void set_start_list_1();</summary>
        public static RTC6Wrap.set_start_list_1Delegate set_start_list_1 = (RTC6Wrap.set_start_list_1Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_start_list_1Delegate>(nameof(set_start_list_1));
        /// <summary>void set_start_list_2();</summary>
        public static RTC6Wrap.set_start_list_2Delegate set_start_list_2 = (RTC6Wrap.set_start_list_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_start_list_2Delegate>(nameof(set_start_list_2));
        /// <summary>void set_input_pointer(uint Pos);</summary>
        public static RTC6Wrap.set_input_pointerDelegate set_input_pointer = (RTC6Wrap.set_input_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_input_pointerDelegate>(nameof(set_input_pointer));
        /// <summary>uint load_list(uint ListNo, uint Pos);</summary>
        public static RTC6Wrap.load_listDelegate load_list = (RTC6Wrap.load_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_listDelegate>(nameof(load_list));
        /// <summary>void load_sub(uint Index);</summary>
        public static RTC6Wrap.load_subDelegate load_sub = (RTC6Wrap.load_subDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_subDelegate>(nameof(load_sub));
        /// <summary>void load_char(uint Char);</summary>
        public static RTC6Wrap.load_charDelegate load_char = (RTC6Wrap.load_charDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_charDelegate>(nameof(load_char));
        /// <summary>void load_text_table(uint Index);</summary>
        public static RTC6Wrap.load_text_tableDelegate load_text_table = (RTC6Wrap.load_text_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_text_tableDelegate>(nameof(load_text_table));
        /// <summary>void get_list_pointer(out uint ListNo, out uint Pos);</summary>
        public static RTC6Wrap.get_list_pointerDelegate get_list_pointer = (RTC6Wrap.get_list_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_list_pointerDelegate>(nameof(get_list_pointer));
        /// <summary>uint get_input_pointer();</summary>
        public static RTC6Wrap.get_input_pointerDelegate get_input_pointer = (RTC6Wrap.get_input_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_input_pointerDelegate>(nameof(get_input_pointer));
        /// <summary>
        ///  void n_execute_list_pos(uint CardNo, uint ListNo, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_execute_list_posDelegate n_execute_list_pos = (RTC6Wrap.n_execute_list_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_execute_list_posDelegate>(nameof(n_execute_list_pos));
        /// <summary>void n_execute_at_pointer(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_execute_at_pointerDelegate n_execute_at_pointer = (RTC6Wrap.n_execute_at_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_execute_at_pointerDelegate>(nameof(n_execute_at_pointer));
        /// <summary>void n_execute_list(uint CardNo, uint ListNo);</summary>
        public static RTC6Wrap.n_execute_listDelegate n_execute_list = (RTC6Wrap.n_execute_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_execute_listDelegate>(nameof(n_execute_list));
        /// <summary>void n_execute_list_1(uint CardNo);</summary>
        public static RTC6Wrap.n_execute_list_1Delegate n_execute_list_1 = (RTC6Wrap.n_execute_list_1Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_execute_list_1Delegate>(nameof(n_execute_list_1));
        /// <summary>void n_execute_list_2(uint CardNo);</summary>
        public static RTC6Wrap.n_execute_list_2Delegate n_execute_list_2 = (RTC6Wrap.n_execute_list_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_execute_list_2Delegate>(nameof(n_execute_list_2));
        /// <summary>uint n_list_jump_rel_ctrl(uint CardNo, int Pos);</summary>
        public static RTC6Wrap.n_list_jump_rel_ctrlDelegate n_list_jump_rel_ctrl = (RTC6Wrap.n_list_jump_rel_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_jump_rel_ctrlDelegate>(nameof(n_list_jump_rel_ctrl));
        /// <summary>
        ///  void n_get_out_pointer(uint CardNo, out uint ListNo, out uint Pos);
        /// </summary>
        public static RTC6Wrap.n_get_out_pointerDelegate n_get_out_pointer = (RTC6Wrap.n_get_out_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_out_pointerDelegate>(nameof(n_get_out_pointer));
        /// <summary>void execute_list_pos(uint ListNo, uint Pos);</summary>
        public static RTC6Wrap.execute_list_posDelegate execute_list_pos = (RTC6Wrap.execute_list_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.execute_list_posDelegate>(nameof(execute_list_pos));
        /// <summary>void execute_at_pointer(uint Pos);</summary>
        public static RTC6Wrap.execute_at_pointerDelegate execute_at_pointer = (RTC6Wrap.execute_at_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.execute_at_pointerDelegate>(nameof(execute_at_pointer));
        /// <summary>void execute_list(uint ListNo);</summary>
        public static RTC6Wrap.execute_listDelegate execute_list = (RTC6Wrap.execute_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.execute_listDelegate>(nameof(execute_list));
        /// <summary>void execute_list_1();</summary>
        public static RTC6Wrap.execute_list_1Delegate execute_list_1 = (RTC6Wrap.execute_list_1Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.execute_list_1Delegate>(nameof(execute_list_1));
        /// <summary>void execute_list_2();</summary>
        public static RTC6Wrap.execute_list_2Delegate execute_list_2 = (RTC6Wrap.execute_list_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.execute_list_2Delegate>(nameof(execute_list_2));
        /// <summary>uint list_jump_rel_ctrl(int Pos);</summary>
        public static RTC6Wrap.list_jump_rel_ctrlDelegate list_jump_rel_ctrl = (RTC6Wrap.list_jump_rel_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_jump_rel_ctrlDelegate>(nameof(list_jump_rel_ctrl));
        /// <summary>void get_out_pointer(out uint ListNo, out uint Pos);</summary>
        public static RTC6Wrap.get_out_pointerDelegate get_out_pointer = (RTC6Wrap.get_out_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_out_pointerDelegate>(nameof(get_out_pointer));
        /// <summary>void n_auto_change_pos(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_auto_change_posDelegate n_auto_change_pos = (RTC6Wrap.n_auto_change_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_auto_change_posDelegate>(nameof(n_auto_change_pos));
        /// <summary>void n_start_loop(uint CardNo);</summary>
        public static RTC6Wrap.n_start_loopDelegate n_start_loop = (RTC6Wrap.n_start_loopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_start_loopDelegate>(nameof(n_start_loop));
        /// <summary>void n_quit_loop(uint CardNo);</summary>
        public static RTC6Wrap.n_quit_loopDelegate n_quit_loop = (RTC6Wrap.n_quit_loopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_quit_loopDelegate>(nameof(n_quit_loop));
        /// <summary>void n_pause_list(uint CardNo);</summary>
        public static RTC6Wrap.n_pause_listDelegate n_pause_list = (RTC6Wrap.n_pause_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_pause_listDelegate>(nameof(n_pause_list));
        /// <summary>void n_restart_list(uint CardNo);</summary>
        public static RTC6Wrap.n_restart_listDelegate n_restart_list = (RTC6Wrap.n_restart_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_restart_listDelegate>(nameof(n_restart_list));
        /// <summary>void n_release_wait(uint CardNo);</summary>
        public static RTC6Wrap.n_release_waitDelegate n_release_wait = (RTC6Wrap.n_release_waitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_release_waitDelegate>(nameof(n_release_wait));
        /// <summary>void n_stop_execution(uint CardNo);</summary>
        public static RTC6Wrap.n_stop_executionDelegate n_stop_execution = (RTC6Wrap.n_stop_executionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stop_executionDelegate>(nameof(n_stop_execution));
        /// <summary>
        ///  void n_set_pause_list_cond(uint CardNo, uint Mask1, uint Mask0);
        /// </summary>
        public static RTC6Wrap.n_set_pause_list_condDelegate n_set_pause_list_cond = (RTC6Wrap.n_set_pause_list_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pause_list_condDelegate>(nameof(n_set_pause_list_cond));
        /// <summary>
        ///  void n_set_pause_list_not_cond(uint CardNo, uint Mask1, uint Mask0);
        /// </summary>
        public static RTC6Wrap.n_set_pause_list_not_condDelegate n_set_pause_list_not_cond = (RTC6Wrap.n_set_pause_list_not_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pause_list_not_condDelegate>(nameof(n_set_pause_list_not_cond));
        /// <summary>void n_auto_change(uint CardNo);</summary>
        public static RTC6Wrap.n_auto_changeDelegate n_auto_change = (RTC6Wrap.n_auto_changeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_auto_changeDelegate>(nameof(n_auto_change));
        /// <summary>void n_stop_list(uint CardNo);</summary>
        public static RTC6Wrap.n_stop_listDelegate n_stop_list = (RTC6Wrap.n_stop_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stop_listDelegate>(nameof(n_stop_list));
        /// <summary>uint n_get_wait_status(uint CardNo);</summary>
        public static RTC6Wrap.n_get_wait_statusDelegate n_get_wait_status = (RTC6Wrap.n_get_wait_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_wait_statusDelegate>(nameof(n_get_wait_status));
        /// <summary>uint n_read_status(uint CardNo);</summary>
        public static RTC6Wrap.n_read_statusDelegate n_read_status = (RTC6Wrap.n_read_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_statusDelegate>(nameof(n_read_status));
        /// <summary>
        ///  void n_get_status(uint CardNo, out uint Status, out uint Pos);
        /// </summary>
        public static RTC6Wrap.n_get_statusDelegate n_get_status = (RTC6Wrap.n_get_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_statusDelegate>(nameof(n_get_status));
        /// <summary>void auto_change_pos(uint Pos);</summary>
        public static RTC6Wrap.auto_change_posDelegate auto_change_pos = (RTC6Wrap.auto_change_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.auto_change_posDelegate>(nameof(auto_change_pos));
        /// <summary>void start_loop();</summary>
        public static RTC6Wrap.start_loopDelegate start_loop = (RTC6Wrap.start_loopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.start_loopDelegate>(nameof(start_loop));
        /// <summary>void quit_loop();</summary>
        public static RTC6Wrap.quit_loopDelegate quit_loop = (RTC6Wrap.quit_loopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.quit_loopDelegate>(nameof(quit_loop));
        /// <summary>void pause_list();</summary>
        public static RTC6Wrap.pause_listDelegate pause_list = (RTC6Wrap.pause_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.pause_listDelegate>(nameof(pause_list));
        /// <summary>void restart_list();</summary>
        public static RTC6Wrap.restart_listDelegate restart_list = (RTC6Wrap.restart_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.restart_listDelegate>(nameof(restart_list));
        /// <summary>void release_wait();</summary>
        public static RTC6Wrap.release_waitDelegate release_wait = (RTC6Wrap.release_waitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.release_waitDelegate>(nameof(release_wait));
        /// <summary>void stop_execution();</summary>
        public static RTC6Wrap.stop_executionDelegate stop_execution = (RTC6Wrap.stop_executionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stop_executionDelegate>(nameof(stop_execution));
        /// <summary>void set_pause_list_cond(uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.set_pause_list_condDelegate set_pause_list_cond = (RTC6Wrap.set_pause_list_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pause_list_condDelegate>(nameof(set_pause_list_cond));
        /// <summary>void set_pause_list_not_cond(uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.set_pause_list_not_condDelegate set_pause_list_not_cond = (RTC6Wrap.set_pause_list_not_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pause_list_not_condDelegate>(nameof(set_pause_list_not_cond));
        /// <summary>void auto_change();</summary>
        public static RTC6Wrap.auto_changeDelegate auto_change = (RTC6Wrap.auto_changeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.auto_changeDelegate>(nameof(auto_change));
        /// <summary>void stop_list();</summary>
        public static RTC6Wrap.stop_listDelegate stop_list = (RTC6Wrap.stop_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stop_listDelegate>(nameof(stop_list));
        /// <summary>uint get_wait_status();</summary>
        public static RTC6Wrap.get_wait_statusDelegate get_wait_status = (RTC6Wrap.get_wait_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_wait_statusDelegate>(nameof(get_wait_status));
        /// <summary>uint read_status();</summary>
        public static RTC6Wrap.read_statusDelegate read_status = (RTC6Wrap.read_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_statusDelegate>(nameof(read_status));
        /// <summary>void get_status(out uint Status, out uint Pos);</summary>
        public static RTC6Wrap.get_statusDelegate get_status = (RTC6Wrap.get_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_statusDelegate>(nameof(get_status));
        /// <summary>void n_set_extstartpos(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_set_extstartposDelegate n_set_extstartpos = (RTC6Wrap.n_set_extstartposDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_extstartposDelegate>(nameof(n_set_extstartpos));
        /// <summary>void n_set_max_counts(uint CardNo, uint Counts);</summary>
        public static RTC6Wrap.n_set_max_countsDelegate n_set_max_counts = (RTC6Wrap.n_set_max_countsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_max_countsDelegate>(nameof(n_set_max_counts));
        /// <summary>void n_set_control_mode(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_set_control_modeDelegate n_set_control_mode = (RTC6Wrap.n_set_control_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_control_modeDelegate>(nameof(n_set_control_mode));
        /// <summary>void n_simulate_ext_stop(uint CardNo);</summary>
        public static RTC6Wrap.n_simulate_ext_stopDelegate n_simulate_ext_stop = (RTC6Wrap.n_simulate_ext_stopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_simulate_ext_stopDelegate>(nameof(n_simulate_ext_stop));
        /// <summary>void n_simulate_ext_start_ctrl(uint CardNo);</summary>
        public static RTC6Wrap.n_simulate_ext_start_ctrlDelegate n_simulate_ext_start_ctrl = (RTC6Wrap.n_simulate_ext_start_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_simulate_ext_start_ctrlDelegate>(nameof(n_simulate_ext_start_ctrl));
        /// <summary>uint n_get_counts(uint CardNo);</summary>
        public static RTC6Wrap.n_get_countsDelegate n_get_counts = (RTC6Wrap.n_get_countsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_countsDelegate>(nameof(n_get_counts));
        /// <summary>uint n_get_startstop_info(uint CardNo);</summary>
        public static RTC6Wrap.n_get_startstop_infoDelegate n_get_startstop_info = (RTC6Wrap.n_get_startstop_infoDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_startstop_infoDelegate>(nameof(n_get_startstop_info));
        /// <summary>void set_extstartpos(uint Pos);</summary>
        public static RTC6Wrap.set_extstartposDelegate set_extstartpos = (RTC6Wrap.set_extstartposDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_extstartposDelegate>(nameof(set_extstartpos));
        /// <summary>void set_max_counts(uint Counts);</summary>
        public static RTC6Wrap.set_max_countsDelegate set_max_counts = (RTC6Wrap.set_max_countsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_max_countsDelegate>(nameof(set_max_counts));
        /// <summary>void set_control_mode(uint Mode);</summary>
        public static RTC6Wrap.set_control_modeDelegate set_control_mode = (RTC6Wrap.set_control_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_control_modeDelegate>(nameof(set_control_mode));
        /// <summary>void simulate_ext_stop();</summary>
        public static RTC6Wrap.simulate_ext_stopDelegate simulate_ext_stop = (RTC6Wrap.simulate_ext_stopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.simulate_ext_stopDelegate>(nameof(simulate_ext_stop));
        /// <summary>void simulate_ext_start_ctrl();</summary>
        public static RTC6Wrap.simulate_ext_start_ctrlDelegate simulate_ext_start_ctrl = (RTC6Wrap.simulate_ext_start_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.simulate_ext_start_ctrlDelegate>(nameof(simulate_ext_start_ctrl));
        /// <summary>uint get_counts();</summary>
        public static RTC6Wrap.get_countsDelegate get_counts = (RTC6Wrap.get_countsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_countsDelegate>(nameof(get_counts));
        /// <summary>uint get_startstop_info();</summary>
        public static RTC6Wrap.get_startstop_infoDelegate get_startstop_info = (RTC6Wrap.get_startstop_infoDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_startstop_infoDelegate>(nameof(get_startstop_info));
        /// <summary>
        ///  void n_copy_dst_src(uint CardNo, uint Dst, uint Src, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_copy_dst_srcDelegate n_copy_dst_src = (RTC6Wrap.n_copy_dst_srcDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_copy_dst_srcDelegate>(nameof(n_copy_dst_src));
        /// <summary>
        ///  void n_set_char_pointer(uint CardNo, uint Char, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_set_char_pointerDelegate n_set_char_pointer = (RTC6Wrap.n_set_char_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_char_pointerDelegate>(nameof(n_set_char_pointer));
        /// <summary>
        ///  void n_set_sub_pointer(uint CardNo, uint Index, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_set_sub_pointerDelegate n_set_sub_pointer = (RTC6Wrap.n_set_sub_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sub_pointerDelegate>(nameof(n_set_sub_pointer));
        /// <summary>
        ///  void n_set_text_table_pointer(uint CardNo, uint Index, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_set_text_table_pointerDelegate n_set_text_table_pointer = (RTC6Wrap.n_set_text_table_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_text_table_pointerDelegate>(nameof(n_set_text_table_pointer));
        /// <summary>
        ///  void n_set_char_table(uint CardNo, uint Index, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_set_char_tableDelegate n_set_char_table = (RTC6Wrap.n_set_char_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_char_tableDelegate>(nameof(n_set_char_table));
        /// <summary>uint n_get_char_pointer(uint CardNo, uint Char);</summary>
        public static RTC6Wrap.n_get_char_pointerDelegate n_get_char_pointer = (RTC6Wrap.n_get_char_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_char_pointerDelegate>(nameof(n_get_char_pointer));
        /// <summary>uint n_get_sub_pointer(uint CardNo, uint Index);</summary>
        public static RTC6Wrap.n_get_sub_pointerDelegate n_get_sub_pointer = (RTC6Wrap.n_get_sub_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_sub_pointerDelegate>(nameof(n_get_sub_pointer));
        /// <summary>
        ///  uint n_get_text_table_pointer(uint CardNo, uint Index);
        /// </summary>
        public static RTC6Wrap.n_get_text_table_pointerDelegate n_get_text_table_pointer = (RTC6Wrap.n_get_text_table_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_text_table_pointerDelegate>(nameof(n_get_text_table_pointer));
        /// <summary>void copy_dst_src(uint Dst, uint Src, uint Mode);</summary>
        public static RTC6Wrap.copy_dst_srcDelegate copy_dst_src = (RTC6Wrap.copy_dst_srcDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.copy_dst_srcDelegate>(nameof(copy_dst_src));
        /// <summary>void set_char_pointer(uint Char, uint Pos);</summary>
        public static RTC6Wrap.set_char_pointerDelegate set_char_pointer = (RTC6Wrap.set_char_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_char_pointerDelegate>(nameof(set_char_pointer));
        /// <summary>void set_sub_pointer(uint Index, uint Pos);</summary>
        public static RTC6Wrap.set_sub_pointerDelegate set_sub_pointer = (RTC6Wrap.set_sub_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sub_pointerDelegate>(nameof(set_sub_pointer));
        /// <summary>void set_text_table_pointer(uint Index, uint Pos);</summary>
        public static RTC6Wrap.set_text_table_pointerDelegate set_text_table_pointer = (RTC6Wrap.set_text_table_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_text_table_pointerDelegate>(nameof(set_text_table_pointer));
        /// <summary>void set_char_table(uint Index, uint Pos);</summary>
        public static RTC6Wrap.set_char_tableDelegate set_char_table = (RTC6Wrap.set_char_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_char_tableDelegate>(nameof(set_char_table));
        /// <summary>uint get_char_pointer(uint Char);</summary>
        public static RTC6Wrap.get_char_pointerDelegate get_char_pointer = (RTC6Wrap.get_char_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_char_pointerDelegate>(nameof(get_char_pointer));
        /// <summary>uint get_sub_pointer(uint Index);</summary>
        public static RTC6Wrap.get_sub_pointerDelegate get_sub_pointer = (RTC6Wrap.get_sub_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_sub_pointerDelegate>(nameof(get_sub_pointer));
        /// <summary>uint get_text_table_pointer(uint Index);</summary>
        public static RTC6Wrap.get_text_table_pointerDelegate get_text_table_pointer = (RTC6Wrap.get_text_table_pointerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_text_table_pointerDelegate>(nameof(get_text_table_pointer));
        /// <summary>void n_time_update(uint CardNo);</summary>
        public static RTC6Wrap.n_time_updateDelegate n_time_update = (RTC6Wrap.n_time_updateDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_time_updateDelegate>(nameof(n_time_update));
        /// <summary>void n_time_control_eth(uint CardNo, double PPM);</summary>
        public static RTC6Wrap.n_time_control_ethDelegate n_time_control_eth = (RTC6Wrap.n_time_control_ethDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_time_control_ethDelegate>(nameof(n_time_control_eth));
        /// <summary>
        ///  void n_set_serial_step(uint CardNo, uint No, uint Step);
        /// </summary>
        public static RTC6Wrap.n_set_serial_stepDelegate n_set_serial_step = (RTC6Wrap.n_set_serial_stepDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_serial_stepDelegate>(nameof(n_set_serial_step));
        /// <summary>void n_select_serial_set(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_select_serial_setDelegate n_select_serial_set = (RTC6Wrap.n_select_serial_setDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_select_serial_setDelegate>(nameof(n_select_serial_set));
        /// <summary>void n_set_serial(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_set_serialDelegate n_set_serial = (RTC6Wrap.n_set_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_serialDelegate>(nameof(n_set_serial));
        /// <summary>double n_get_serial(uint CardNo);</summary>
        public static RTC6Wrap.n_get_serialDelegate n_get_serial = (RTC6Wrap.n_get_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_serialDelegate>(nameof(n_get_serial));
        /// <summary>double n_get_list_serial(uint CardNo, out uint SetNo);</summary>
        public static RTC6Wrap.n_get_list_serialDelegate n_get_list_serial = (RTC6Wrap.n_get_list_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_list_serialDelegate>(nameof(n_get_list_serial));
        /// <summary>void time_update();</summary>
        public static RTC6Wrap.time_updateDelegate time_update = (RTC6Wrap.time_updateDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.time_updateDelegate>(nameof(time_update));
        /// <summary>void time_control_eth(double PPM);</summary>
        public static RTC6Wrap.time_control_ethDelegate time_control_eth = (RTC6Wrap.time_control_ethDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.time_control_ethDelegate>(nameof(time_control_eth));
        /// <summary>void set_serial_step(uint No, uint Step);</summary>
        public static RTC6Wrap.set_serial_stepDelegate set_serial_step = (RTC6Wrap.set_serial_stepDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_serial_stepDelegate>(nameof(set_serial_step));
        /// <summary>void select_serial_set(uint No);</summary>
        public static RTC6Wrap.select_serial_setDelegate select_serial_set = (RTC6Wrap.select_serial_setDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.select_serial_setDelegate>(nameof(select_serial_set));
        /// <summary>void set_serial(uint No);</summary>
        public static RTC6Wrap.set_serialDelegate set_serial = (RTC6Wrap.set_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_serialDelegate>(nameof(set_serial));
        /// <summary>double get_serial();</summary>
        public static RTC6Wrap.get_serialDelegate get_serial = (RTC6Wrap.get_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_serialDelegate>(nameof(get_serial));
        /// <summary>double get_list_serial(out uint SetNo);</summary>
        public static RTC6Wrap.get_list_serialDelegate get_list_serial = (RTC6Wrap.get_list_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_list_serialDelegate>(nameof(get_list_serial));
        /// <summary>
        ///  void n_write_io_port_mask(uint CardNo, uint Value, uint Mask);
        /// </summary>
        public static RTC6Wrap.n_write_io_port_maskDelegate n_write_io_port_mask = (RTC6Wrap.n_write_io_port_maskDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_io_port_maskDelegate>(nameof(n_write_io_port_mask));
        /// <summary>void n_write_8bit_port(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_8bit_portDelegate n_write_8bit_port = (RTC6Wrap.n_write_8bit_portDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_8bit_portDelegate>(nameof(n_write_8bit_port));
        /// <summary>uint n_read_io_port(uint CardNo);</summary>
        public static RTC6Wrap.n_read_io_portDelegate n_read_io_port = (RTC6Wrap.n_read_io_portDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_io_portDelegate>(nameof(n_read_io_port));
        /// <summary>
        ///  uint n_read_io_port_buffer(uint CardNo, uint Index, out uint Value, out int XPos, out int YPos, out uint Time);
        /// </summary>
        public static RTC6Wrap.n_read_io_port_bufferDelegate n_read_io_port_buffer = (RTC6Wrap.n_read_io_port_bufferDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_io_port_bufferDelegate>(nameof(n_read_io_port_buffer));
        /// <summary>uint n_get_io_status(uint CardNo);</summary>
        public static RTC6Wrap.n_get_io_statusDelegate n_get_io_status = (RTC6Wrap.n_get_io_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_io_statusDelegate>(nameof(n_get_io_status));
        /// <summary>uint n_read_analog_in(uint CardNo);</summary>
        public static RTC6Wrap.n_read_analog_inDelegate n_read_analog_in = (RTC6Wrap.n_read_analog_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_analog_inDelegate>(nameof(n_read_analog_in));
        /// <summary>void n_write_da_x(uint CardNo, uint x, uint Value);</summary>
        public static RTC6Wrap.n_write_da_xDelegate n_write_da_x = (RTC6Wrap.n_write_da_xDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_da_xDelegate>(nameof(n_write_da_x));
        /// <summary>
        ///  void n_set_laser_off_default(uint CardNo, uint AnalogOut1, uint AnalogOut2, uint DigitalOut);
        /// </summary>
        public static RTC6Wrap.n_set_laser_off_defaultDelegate n_set_laser_off_default = (RTC6Wrap.n_set_laser_off_defaultDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_off_defaultDelegate>(nameof(n_set_laser_off_default));
        /// <summary>
        ///  void n_set_port_default(uint CardNo, uint Port, uint Value);
        /// </summary>
        public static RTC6Wrap.n_set_port_defaultDelegate n_set_port_default = (RTC6Wrap.n_set_port_defaultDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_port_defaultDelegate>(nameof(n_set_port_default));
        /// <summary>void n_write_io_port(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_io_portDelegate n_write_io_port = (RTC6Wrap.n_write_io_portDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_io_portDelegate>(nameof(n_write_io_port));
        /// <summary>void n_write_da_1(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_da_1Delegate n_write_da_1 = (RTC6Wrap.n_write_da_1Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_da_1Delegate>(nameof(n_write_da_1));
        /// <summary>void n_write_da_2(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_da_2Delegate n_write_da_2 = (RTC6Wrap.n_write_da_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_da_2Delegate>(nameof(n_write_da_2));
        /// <summary>void write_io_port_mask(uint Value, uint Mask);</summary>
        public static RTC6Wrap.write_io_port_maskDelegate write_io_port_mask = (RTC6Wrap.write_io_port_maskDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_io_port_maskDelegate>(nameof(write_io_port_mask));
        /// <summary>void write_8bit_port(uint Value);</summary>
        public static RTC6Wrap.write_8bit_portDelegate write_8bit_port = (RTC6Wrap.write_8bit_portDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_8bit_portDelegate>(nameof(write_8bit_port));
        /// <summary>uint read_io_port();</summary>
        public static RTC6Wrap.read_io_portDelegate read_io_port = (RTC6Wrap.read_io_portDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_io_portDelegate>(nameof(read_io_port));
        /// <summary>
        ///  uint read_io_port_buffer(uint Index, out uint Value, out int XPos, out int YPos, out uint Time);
        /// </summary>
        public static RTC6Wrap.read_io_port_bufferDelegate read_io_port_buffer = (RTC6Wrap.read_io_port_bufferDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_io_port_bufferDelegate>(nameof(read_io_port_buffer));
        /// <summary>uint get_io_status();</summary>
        public static RTC6Wrap.get_io_statusDelegate get_io_status = (RTC6Wrap.get_io_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_io_statusDelegate>(nameof(get_io_status));
        /// <summary>uint read_analog_in();</summary>
        public static RTC6Wrap.read_analog_inDelegate read_analog_in = (RTC6Wrap.read_analog_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_analog_inDelegate>(nameof(read_analog_in));
        /// <summary>void write_da_x(uint x, uint Value);</summary>
        public static RTC6Wrap.write_da_xDelegate write_da_x = (RTC6Wrap.write_da_xDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_da_xDelegate>(nameof(write_da_x));
        /// <summary>
        ///  void set_laser_off_default(uint AnalogOut1, uint AnalogOut2, uint DigitalOut);
        /// </summary>
        public static RTC6Wrap.set_laser_off_defaultDelegate set_laser_off_default = (RTC6Wrap.set_laser_off_defaultDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_off_defaultDelegate>(nameof(set_laser_off_default));
        /// <summary>void set_port_default(uint Port, uint Value);</summary>
        public static RTC6Wrap.set_port_defaultDelegate set_port_default = (RTC6Wrap.set_port_defaultDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_port_defaultDelegate>(nameof(set_port_default));
        /// <summary>void write_io_port(uint Value);</summary>
        public static RTC6Wrap.write_io_portDelegate write_io_port = (RTC6Wrap.write_io_portDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_io_portDelegate>(nameof(write_io_port));
        /// <summary>void write_da_1(uint Value);</summary>
        public static RTC6Wrap.write_da_1Delegate write_da_1 = (RTC6Wrap.write_da_1Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_da_1Delegate>(nameof(write_da_1));
        /// <summary>void write_da_2(uint Value);</summary>
        public static RTC6Wrap.write_da_2Delegate write_da_2 = (RTC6Wrap.write_da_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_da_2Delegate>(nameof(write_da_2));
        /// <summary>void n_disable_laser(uint CardNo);</summary>
        public static RTC6Wrap.n_disable_laserDelegate n_disable_laser = (RTC6Wrap.n_disable_laserDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_disable_laserDelegate>(nameof(n_disable_laser));
        /// <summary>void n_enable_laser(uint CardNo);</summary>
        public static RTC6Wrap.n_enable_laserDelegate n_enable_laser = (RTC6Wrap.n_enable_laserDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_enable_laserDelegate>(nameof(n_enable_laser));
        /// <summary>void n_laser_signal_on(uint CardNo);</summary>
        public static RTC6Wrap.n_laser_signal_onDelegate n_laser_signal_on = (RTC6Wrap.n_laser_signal_onDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_laser_signal_onDelegate>(nameof(n_laser_signal_on));
        /// <summary>void n_laser_signal_off(uint CardNo);</summary>
        public static RTC6Wrap.n_laser_signal_offDelegate n_laser_signal_off = (RTC6Wrap.n_laser_signal_offDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_laser_signal_offDelegate>(nameof(n_laser_signal_off));
        /// <summary>
        ///  void n_set_standby(uint CardNo, uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_set_standbyDelegate n_set_standby = (RTC6Wrap.n_set_standbyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_standbyDelegate>(nameof(n_set_standby));
        /// <summary>
        ///  void n_set_laser_pulses_ctrl(uint CardNo, uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_set_laser_pulses_ctrlDelegate n_set_laser_pulses_ctrl = (RTC6Wrap.n_set_laser_pulses_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_pulses_ctrlDelegate>(nameof(n_set_laser_pulses_ctrl));
        /// <summary>
        ///  void n_set_firstpulse_killer(uint CardNo, uint Length);
        /// </summary>
        public static RTC6Wrap.n_set_firstpulse_killerDelegate n_set_firstpulse_killer = (RTC6Wrap.n_set_firstpulse_killerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_firstpulse_killerDelegate>(nameof(n_set_firstpulse_killer));
        /// <summary>void n_set_qswitch_delay(uint CardNo, uint Delay);</summary>
        public static RTC6Wrap.n_set_qswitch_delayDelegate n_set_qswitch_delay = (RTC6Wrap.n_set_qswitch_delayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_qswitch_delayDelegate>(nameof(n_set_qswitch_delay));
        /// <summary>void n_set_laser_mode(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_set_laser_modeDelegate n_set_laser_mode = (RTC6Wrap.n_set_laser_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_modeDelegate>(nameof(n_set_laser_mode));
        /// <summary>void n_set_laser_control(uint CardNo, uint Ctrl);</summary>
        public static RTC6Wrap.n_set_laser_controlDelegate n_set_laser_control = (RTC6Wrap.n_set_laser_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_controlDelegate>(nameof(n_set_laser_control));
        /// <summary>void n_set_laser_pin_out(uint CardNo, uint Pins);</summary>
        public static RTC6Wrap.n_set_laser_pin_outDelegate n_set_laser_pin_out = (RTC6Wrap.n_set_laser_pin_outDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_pin_outDelegate>(nameof(n_set_laser_pin_out));
        /// <summary>uint n_get_laser_pin_in(uint CardNo);</summary>
        public static RTC6Wrap.n_get_laser_pin_inDelegate n_get_laser_pin_in = (RTC6Wrap.n_get_laser_pin_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_laser_pin_inDelegate>(nameof(n_get_laser_pin_in));
        /// <summary>
        ///  void n_set_softstart_level(uint CardNo, uint Index, uint Level);
        /// </summary>
        public static RTC6Wrap.n_set_softstart_levelDelegate n_set_softstart_level = (RTC6Wrap.n_set_softstart_levelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_softstart_levelDelegate>(nameof(n_set_softstart_level));
        /// <summary>
        ///  void n_set_softstart_mode(uint CardNo, uint Mode, uint Number, uint Delay);
        /// </summary>
        public static RTC6Wrap.n_set_softstart_modeDelegate n_set_softstart_mode = (RTC6Wrap.n_set_softstart_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_softstart_modeDelegate>(nameof(n_set_softstart_mode));
        /// <summary>
        ///  uint n_set_auto_laser_control(uint CardNo, uint Ctrl, uint Value, uint Mode, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.n_set_auto_laser_controlDelegate n_set_auto_laser_control = (RTC6Wrap.n_set_auto_laser_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_auto_laser_controlDelegate>(nameof(n_set_auto_laser_control));
        /// <summary>
        ///  uint n_set_auto_laser_params(uint CardNo, uint Ctrl, uint Value, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.n_set_auto_laser_paramsDelegate n_set_auto_laser_params = (RTC6Wrap.n_set_auto_laser_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_auto_laser_paramsDelegate>(nameof(n_set_auto_laser_params));
        /// <summary>
        ///  int n_load_auto_laser_control(uint CardNo, string Name, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_auto_laser_controlDelegate n_load_auto_laser_control = (RTC6Wrap.n_load_auto_laser_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_auto_laser_controlDelegate>(nameof(n_load_auto_laser_control));
        /// <summary>
        ///  int n_load_position_control(uint CardNo, string Name, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_position_controlDelegate n_load_position_control = (RTC6Wrap.n_load_position_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_position_controlDelegate>(nameof(n_load_position_control));
        /// <summary>
        ///  void n_set_default_pixel(uint CardNo, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_set_default_pixelDelegate n_set_default_pixel = (RTC6Wrap.n_set_default_pixelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_default_pixelDelegate>(nameof(n_set_default_pixel));
        /// <summary>
        ///  void n_get_standby(uint CardNo, out uint HalfPeriod, out uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_get_standbyDelegate n_get_standby = (RTC6Wrap.n_get_standbyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_standbyDelegate>(nameof(n_get_standby));
        /// <summary>void n_set_pulse_picking(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_set_pulse_pickingDelegate n_set_pulse_picking = (RTC6Wrap.n_set_pulse_pickingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pulse_pickingDelegate>(nameof(n_set_pulse_picking));
        /// <summary>
        ///  void n_set_pulse_picking_length(uint CardNo, uint Length);
        /// </summary>
        public static RTC6Wrap.n_set_pulse_picking_lengthDelegate n_set_pulse_picking_length = (RTC6Wrap.n_set_pulse_picking_lengthDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pulse_picking_lengthDelegate>(nameof(n_set_pulse_picking_length));
        /// <summary>void n_config_laser_signals(uint CardNo, uint Config);</summary>
        public static RTC6Wrap.n_config_laser_signalsDelegate n_config_laser_signals = (RTC6Wrap.n_config_laser_signalsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_config_laser_signalsDelegate>(nameof(n_config_laser_signals));
        /// <summary>
        ///  void n_set_laser_power(uint CardNo, uint Port, uint Value);
        /// </summary>
        public static RTC6Wrap.n_set_laser_powerDelegate n_set_laser_power = (RTC6Wrap.n_set_laser_powerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_powerDelegate>(nameof(n_set_laser_power));
        /// <summary>
        ///  void n_set_port_default_list(uint CardNo, uint Port, uint Value);
        /// </summary>
        public static RTC6Wrap.n_set_port_default_listDelegate n_set_port_default_list = (RTC6Wrap.n_set_port_default_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_port_default_listDelegate>(nameof(n_set_port_default_list));
        /// <summary>void n_spot_distance_ctrl(uint CardNo, double Dist);</summary>
        public static RTC6Wrap.n_spot_distance_ctrlDelegate n_spot_distance_ctrl = (RTC6Wrap.n_spot_distance_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_spot_distance_ctrlDelegate>(nameof(n_spot_distance_ctrl));
        /// <summary>void disable_laser();</summary>
        public static RTC6Wrap.disable_laserDelegate disable_laser = (RTC6Wrap.disable_laserDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.disable_laserDelegate>(nameof(disable_laser));
        /// <summary>void enable_laser();</summary>
        public static RTC6Wrap.enable_laserDelegate enable_laser = (RTC6Wrap.enable_laserDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.enable_laserDelegate>(nameof(enable_laser));
        /// <summary>void laser_signal_on();</summary>
        public static RTC6Wrap.laser_signal_onDelegate laser_signal_on = (RTC6Wrap.laser_signal_onDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.laser_signal_onDelegate>(nameof(laser_signal_on));
        /// <summary>void laser_signal_off();</summary>
        public static RTC6Wrap.laser_signal_offDelegate laser_signal_off = (RTC6Wrap.laser_signal_offDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.laser_signal_offDelegate>(nameof(laser_signal_off));
        /// <summary>void set_standby(uint HalfPeriod, uint PulseLength);</summary>
        public static RTC6Wrap.set_standbyDelegate set_standby = (RTC6Wrap.set_standbyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_standbyDelegate>(nameof(set_standby));
        /// <summary>
        ///  void set_laser_pulses_ctrl(uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.set_laser_pulses_ctrlDelegate set_laser_pulses_ctrl = (RTC6Wrap.set_laser_pulses_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_pulses_ctrlDelegate>(nameof(set_laser_pulses_ctrl));
        /// <summary>void set_firstpulse_killer(uint Length);</summary>
        public static RTC6Wrap.set_firstpulse_killerDelegate set_firstpulse_killer = (RTC6Wrap.set_firstpulse_killerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_firstpulse_killerDelegate>(nameof(set_firstpulse_killer));
        /// <summary>void set_qswitch_delay(uint Delay);</summary>
        public static RTC6Wrap.set_qswitch_delayDelegate set_qswitch_delay = (RTC6Wrap.set_qswitch_delayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_qswitch_delayDelegate>(nameof(set_qswitch_delay));
        /// <summary>void set_laser_mode(uint Mode);</summary>
        public static RTC6Wrap.set_laser_modeDelegate set_laser_mode = (RTC6Wrap.set_laser_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_modeDelegate>(nameof(set_laser_mode));
        /// <summary>void set_laser_control(uint Ctrl);</summary>
        public static RTC6Wrap.set_laser_controlDelegate set_laser_control = (RTC6Wrap.set_laser_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_controlDelegate>(nameof(set_laser_control));
        /// <summary>void set_laser_pin_out(uint Pins);</summary>
        public static RTC6Wrap.set_laser_pin_outDelegate set_laser_pin_out = (RTC6Wrap.set_laser_pin_outDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_pin_outDelegate>(nameof(set_laser_pin_out));
        /// <summary>uint get_laser_pin_in();</summary>
        public static RTC6Wrap.get_laser_pin_inDelegate get_laser_pin_in = (RTC6Wrap.get_laser_pin_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_laser_pin_inDelegate>(nameof(get_laser_pin_in));
        /// <summary>void set_softstart_level(uint Index, uint Level);</summary>
        public static RTC6Wrap.set_softstart_levelDelegate set_softstart_level = (RTC6Wrap.set_softstart_levelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_softstart_levelDelegate>(nameof(set_softstart_level));
        /// <summary>
        ///  void set_softstart_mode(uint Mode, uint Number, uint Delay);
        /// </summary>
        public static RTC6Wrap.set_softstart_modeDelegate set_softstart_mode = (RTC6Wrap.set_softstart_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_softstart_modeDelegate>(nameof(set_softstart_mode));
        /// <summary>
        ///  uint set_auto_laser_control(uint Ctrl, uint Value, uint Mode, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.set_auto_laser_controlDelegate set_auto_laser_control = (RTC6Wrap.set_auto_laser_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_auto_laser_controlDelegate>(nameof(set_auto_laser_control));
        /// <summary>
        ///  uint set_auto_laser_params(uint Ctrl, uint Value, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.set_auto_laser_paramsDelegate set_auto_laser_params = (RTC6Wrap.set_auto_laser_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_auto_laser_paramsDelegate>(nameof(set_auto_laser_params));
        /// <summary>int load_auto_laser_control(string Name, uint No);</summary>
        public static RTC6Wrap.load_auto_laser_controlDelegate load_auto_laser_control = (RTC6Wrap.load_auto_laser_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_auto_laser_controlDelegate>(nameof(load_auto_laser_control));
        /// <summary>int load_position_control(string Name, uint No);</summary>
        public static RTC6Wrap.load_position_controlDelegate load_position_control = (RTC6Wrap.load_position_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_position_controlDelegate>(nameof(load_position_control));
        /// <summary>void set_default_pixel(uint PulseLength);</summary>
        public static RTC6Wrap.set_default_pixelDelegate set_default_pixel = (RTC6Wrap.set_default_pixelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_default_pixelDelegate>(nameof(set_default_pixel));
        /// <summary>
        ///  void get_standby(out uint HalfPeriod, out uint PulseLength);
        /// </summary>
        public static RTC6Wrap.get_standbyDelegate get_standby = (RTC6Wrap.get_standbyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_standbyDelegate>(nameof(get_standby));
        /// <summary>void set_pulse_picking(uint No);</summary>
        public static RTC6Wrap.set_pulse_pickingDelegate set_pulse_picking = (RTC6Wrap.set_pulse_pickingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pulse_pickingDelegate>(nameof(set_pulse_picking));
        /// <summary>void set_pulse_picking_length(uint Length);</summary>
        public static RTC6Wrap.set_pulse_picking_lengthDelegate set_pulse_picking_length = (RTC6Wrap.set_pulse_picking_lengthDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pulse_picking_lengthDelegate>(nameof(set_pulse_picking_length));
        /// <summary>void config_laser_signals(uint Config);</summary>
        public static RTC6Wrap.config_laser_signalsDelegate config_laser_signals = (RTC6Wrap.config_laser_signalsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.config_laser_signalsDelegate>(nameof(config_laser_signals));
        /// <summary>void set_laser_power(uint Port, uint Value);</summary>
        public static RTC6Wrap.set_laser_powerDelegate set_laser_power = (RTC6Wrap.set_laser_powerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_powerDelegate>(nameof(set_laser_power));
        /// <summary>void set_port_default_list(uint Port, uint Value);</summary>
        public static RTC6Wrap.set_port_default_listDelegate set_port_default_list = (RTC6Wrap.set_port_default_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_port_default_listDelegate>(nameof(set_port_default_list));
        /// <summary>void spot_distance_ctrl(double Dist);</summary>
        public static RTC6Wrap.spot_distance_ctrlDelegate spot_distance_ctrl = (RTC6Wrap.spot_distance_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.spot_distance_ctrlDelegate>(nameof(spot_distance_ctrl));
        /// <summary>
        ///  void n_set_ext_start_delay(uint CardNo, int Delay, uint EncoderNo);
        /// </summary>
        public static RTC6Wrap.n_set_ext_start_delayDelegate n_set_ext_start_delay = (RTC6Wrap.n_set_ext_start_delayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_ext_start_delayDelegate>(nameof(n_set_ext_start_delay));
        /// <summary>void n_set_rot_center(uint CardNo, int X, int Y);</summary>
        public static RTC6Wrap.n_set_rot_centerDelegate n_set_rot_center = (RTC6Wrap.n_set_rot_centerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_rot_centerDelegate>(nameof(n_set_rot_center));
        /// <summary>void n_simulate_encoder(uint CardNo, uint EncoderNo);</summary>
        public static RTC6Wrap.n_simulate_encoderDelegate n_simulate_encoder = (RTC6Wrap.n_simulate_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_simulate_encoderDelegate>(nameof(n_simulate_encoder));
        /// <summary>uint n_get_marking_info(uint CardNo);</summary>
        public static RTC6Wrap.n_get_marking_infoDelegate n_get_marking_info = (RTC6Wrap.n_get_marking_infoDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_marking_infoDelegate>(nameof(n_get_marking_info));
        /// <summary>
        ///  void n_set_encoder_speed_ctrl(uint CardNo, uint EncoderNo, double Speed, double Smooth);
        /// </summary>
        public static RTC6Wrap.n_set_encoder_speed_ctrlDelegate n_set_encoder_speed_ctrl = (RTC6Wrap.n_set_encoder_speed_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_encoder_speed_ctrlDelegate>(nameof(n_set_encoder_speed_ctrl));
        /// <summary>void n_set_mcbsp_x(uint CardNo, double ScaleX);</summary>
        public static RTC6Wrap.n_set_mcbsp_xDelegate n_set_mcbsp_x = (RTC6Wrap.n_set_mcbsp_xDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_xDelegate>(nameof(n_set_mcbsp_x));
        /// <summary>void n_set_mcbsp_y(uint CardNo, double ScaleY);</summary>
        public static RTC6Wrap.n_set_mcbsp_yDelegate n_set_mcbsp_y = (RTC6Wrap.n_set_mcbsp_yDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_yDelegate>(nameof(n_set_mcbsp_y));
        /// <summary>void n_set_mcbsp_rot(uint CardNo, double Resolution);</summary>
        public static RTC6Wrap.n_set_mcbsp_rotDelegate n_set_mcbsp_rot = (RTC6Wrap.n_set_mcbsp_rotDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_rotDelegate>(nameof(n_set_mcbsp_rot));
        /// <summary>void n_set_mcbsp_matrix(uint CardNo);</summary>
        public static RTC6Wrap.n_set_mcbsp_matrixDelegate n_set_mcbsp_matrix = (RTC6Wrap.n_set_mcbsp_matrixDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_matrixDelegate>(nameof(n_set_mcbsp_matrix));
        /// <summary>
        ///  void n_set_mcbsp_in(uint CardNo, uint Mode, double Scale);
        /// </summary>
        public static RTC6Wrap.n_set_mcbsp_inDelegate n_set_mcbsp_in = (RTC6Wrap.n_set_mcbsp_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_inDelegate>(nameof(n_set_mcbsp_in));
        /// <summary>
        ///  void n_set_multi_mcbsp_in(uint CardNo, uint Ctrl, uint P, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_set_multi_mcbsp_inDelegate n_set_multi_mcbsp_in = (RTC6Wrap.n_set_multi_mcbsp_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_multi_mcbsp_inDelegate>(nameof(n_set_multi_mcbsp_in));
        /// <summary>
        ///  void n_set_fly_tracking_error(uint CardNo, uint TrackingErrorX, uint TrackingErrorY);
        /// </summary>
        public static RTC6Wrap.n_set_fly_tracking_errorDelegate n_set_fly_tracking_error = (RTC6Wrap.n_set_fly_tracking_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_tracking_errorDelegate>(nameof(n_set_fly_tracking_error));
        /// <summary>
        ///  int n_load_fly_2d_table(uint CardNo, string Name, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_fly_2d_tableDelegate n_load_fly_2d_table = (RTC6Wrap.n_load_fly_2d_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_fly_2d_tableDelegate>(nameof(n_load_fly_2d_table));
        /// <summary>
        ///  void n_init_fly_2d(uint CardNo, int OffsetX, int OffsetY, uint No);
        /// </summary>
        public static RTC6Wrap.n_init_fly_2dDelegate n_init_fly_2d = (RTC6Wrap.n_init_fly_2dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_init_fly_2dDelegate>(nameof(n_init_fly_2d));
        /// <summary>
        ///  void n_get_fly_2d_offset(uint CardNo, out int OffsetX, out int OffsetY);
        /// </summary>
        public static RTC6Wrap.n_get_fly_2d_offsetDelegate n_get_fly_2d_offset = (RTC6Wrap.n_get_fly_2d_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_fly_2d_offsetDelegate>(nameof(n_get_fly_2d_offset));
        /// <summary>
        ///  void n_get_encoder(uint CardNo, out int Encoder0, out int Encoder1);
        /// </summary>
        public static RTC6Wrap.n_get_encoderDelegate n_get_encoder = (RTC6Wrap.n_get_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_encoderDelegate>(nameof(n_get_encoder));
        /// <summary>
        ///  void n_read_encoder(uint CardNo, out int Encoder0_1, out int Encoder1_1, out int Encoder0_2, out int Encoder1_2);
        /// </summary>
        public static RTC6Wrap.n_read_encoderDelegate n_read_encoder = (RTC6Wrap.n_read_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_encoderDelegate>(nameof(n_read_encoder));
        /// <summary>int n_get_mcbsp(uint CardNo);</summary>
        public static RTC6Wrap.n_get_mcbspDelegate n_get_mcbsp = (RTC6Wrap.n_get_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_mcbspDelegate>(nameof(n_get_mcbsp));
        /// <summary>int n_read_mcbsp(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_read_mcbspDelegate n_read_mcbsp = (RTC6Wrap.n_read_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_mcbspDelegate>(nameof(n_read_mcbsp));
        /// <summary>int n_read_multi_mcbsp(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_read_multi_mcbspDelegate n_read_multi_mcbsp = (RTC6Wrap.n_read_multi_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_multi_mcbspDelegate>(nameof(n_read_multi_mcbsp));
        /// <summary>void set_ext_start_delay(int Delay, uint EncoderNo);</summary>
        public static RTC6Wrap.set_ext_start_delayDelegate set_ext_start_delay = (RTC6Wrap.set_ext_start_delayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_ext_start_delayDelegate>(nameof(set_ext_start_delay));
        /// <summary>void set_rot_center(int X, int Y);</summary>
        public static RTC6Wrap.set_rot_centerDelegate set_rot_center = (RTC6Wrap.set_rot_centerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_rot_centerDelegate>(nameof(set_rot_center));
        /// <summary>void simulate_encoder(uint EncoderNo);</summary>
        public static RTC6Wrap.simulate_encoderDelegate simulate_encoder = (RTC6Wrap.simulate_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.simulate_encoderDelegate>(nameof(simulate_encoder));
        /// <summary>uint get_marking_info();</summary>
        public static RTC6Wrap.get_marking_infoDelegate get_marking_info = (RTC6Wrap.get_marking_infoDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_marking_infoDelegate>(nameof(get_marking_info));
        /// <summary>
        ///  void set_encoder_speed_ctrl(uint EncoderNo, double Speed, double Smooth);
        /// </summary>
        public static RTC6Wrap.set_encoder_speed_ctrlDelegate set_encoder_speed_ctrl = (RTC6Wrap.set_encoder_speed_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_encoder_speed_ctrlDelegate>(nameof(set_encoder_speed_ctrl));
        /// <summary>void set_mcbsp_x(double ScaleX);</summary>
        public static RTC6Wrap.set_mcbsp_xDelegate set_mcbsp_x = (RTC6Wrap.set_mcbsp_xDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_xDelegate>(nameof(set_mcbsp_x));
        /// <summary>void set_mcbsp_y(double ScaleY);</summary>
        public static RTC6Wrap.set_mcbsp_yDelegate set_mcbsp_y = (RTC6Wrap.set_mcbsp_yDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_yDelegate>(nameof(set_mcbsp_y));
        /// <summary>void set_mcbsp_rot(double Resolution);</summary>
        public static RTC6Wrap.set_mcbsp_rotDelegate set_mcbsp_rot = (RTC6Wrap.set_mcbsp_rotDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_rotDelegate>(nameof(set_mcbsp_rot));
        /// <summary>void set_mcbsp_matrix();</summary>
        public static RTC6Wrap.set_mcbsp_matrixDelegate set_mcbsp_matrix = (RTC6Wrap.set_mcbsp_matrixDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_matrixDelegate>(nameof(set_mcbsp_matrix));
        /// <summary>void set_mcbsp_in(uint Mode, double Scale);</summary>
        public static RTC6Wrap.set_mcbsp_inDelegate set_mcbsp_in = (RTC6Wrap.set_mcbsp_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_inDelegate>(nameof(set_mcbsp_in));
        /// <summary>void set_multi_mcbsp_in(uint Ctrl, uint P, uint Mode);</summary>
        public static RTC6Wrap.set_multi_mcbsp_inDelegate set_multi_mcbsp_in = (RTC6Wrap.set_multi_mcbsp_inDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_multi_mcbsp_inDelegate>(nameof(set_multi_mcbsp_in));
        /// <summary>
        ///  void set_fly_tracking_error(uint TrackingErrorX, uint TrackingErrorY);
        /// </summary>
        public static RTC6Wrap.set_fly_tracking_errorDelegate set_fly_tracking_error = (RTC6Wrap.set_fly_tracking_errorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_tracking_errorDelegate>(nameof(set_fly_tracking_error));
        /// <summary>int load_fly_2d_table(string Name, uint No);</summary>
        public static RTC6Wrap.load_fly_2d_tableDelegate load_fly_2d_table = (RTC6Wrap.load_fly_2d_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_fly_2d_tableDelegate>(nameof(load_fly_2d_table));
        /// <summary>void init_fly_2d(int OffsetX, int OffsetY, uint No);</summary>
        public static RTC6Wrap.init_fly_2dDelegate init_fly_2d = (RTC6Wrap.init_fly_2dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.init_fly_2dDelegate>(nameof(init_fly_2d));
        /// <summary>
        ///  void get_fly_2d_offset(out int OffsetX, out int OffsetY);
        /// </summary>
        public static RTC6Wrap.get_fly_2d_offsetDelegate get_fly_2d_offset = (RTC6Wrap.get_fly_2d_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_fly_2d_offsetDelegate>(nameof(get_fly_2d_offset));
        /// <summary>void get_encoder(out int Encoder0, out int Encoder1);</summary>
        public static RTC6Wrap.get_encoderDelegate get_encoder = (RTC6Wrap.get_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_encoderDelegate>(nameof(get_encoder));
        /// <summary>
        ///  void read_encoder(out int Encoder0_1, out int Encoder1_1, out int Encoder0_2, out int Encoder1_2);
        /// </summary>
        public static RTC6Wrap.read_encoderDelegate read_encoder = (RTC6Wrap.read_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_encoderDelegate>(nameof(read_encoder));
        /// <summary>int get_mcbsp();</summary>
        public static RTC6Wrap.get_mcbspDelegate get_mcbsp = (RTC6Wrap.get_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_mcbspDelegate>(nameof(get_mcbsp));
        /// <summary>int read_mcbsp(uint No);</summary>
        public static RTC6Wrap.read_mcbspDelegate read_mcbsp = (RTC6Wrap.read_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_mcbspDelegate>(nameof(read_mcbsp));
        /// <summary>int read_multi_mcbsp(uint No);</summary>
        public static RTC6Wrap.read_multi_mcbspDelegate read_multi_mcbsp = (RTC6Wrap.read_multi_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_multi_mcbspDelegate>(nameof(read_multi_mcbsp));
        /// <summary>double n_get_time(uint CardNo);</summary>
        public static RTC6Wrap.n_get_timeDelegate n_get_time = (RTC6Wrap.n_get_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_timeDelegate>(nameof(n_get_time));
        /// <summary>double n_get_lap_time(uint CardNo);</summary>
        public static RTC6Wrap.n_get_lap_timeDelegate n_get_lap_time = (RTC6Wrap.n_get_lap_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_lap_timeDelegate>(nameof(n_get_lap_time));
        /// <summary>
        ///  void n_measurement_status(uint CardNo, out uint Busy, out uint Pos);
        /// </summary>
        public static RTC6Wrap.n_measurement_statusDelegate n_measurement_status = (RTC6Wrap.n_measurement_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_measurement_statusDelegate>(nameof(n_measurement_status));
        /// <summary>
        ///  void n_get_waveform_offset(uint CardNo, uint Channel, uint Offset, uint Number, int[] Ptr);
        /// </summary>
        public static RTC6Wrap.n_get_waveform_offsetDelegate n_get_waveform_offset = (RTC6Wrap.n_get_waveform_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_waveform_offsetDelegate>(nameof(n_get_waveform_offset));
        /// <summary>
        ///  void n_get_waveform(uint CardNo, uint Channel, uint Number, int[] Ptr);
        /// </summary>
        public static RTC6Wrap.n_get_waveformDelegate n_get_waveform = (RTC6Wrap.n_get_waveformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_waveformDelegate>(nameof(n_get_waveform));
        /// <summary>void n_bounce_supp(uint CardNo, uint Length);</summary>
        public static RTC6Wrap.n_bounce_suppDelegate n_bounce_supp = (RTC6Wrap.n_bounce_suppDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_bounce_suppDelegate>(nameof(n_bounce_supp));
        /// <summary>
        ///  void n_home_position_4(uint CardNo, int X0Home, int X1Home, int X2Home, int X3Home);
        /// </summary>
        public static RTC6Wrap.n_home_position_4Delegate n_home_position_4 = (RTC6Wrap.n_home_position_4Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_home_position_4Delegate>(nameof(n_home_position_4));
        /// <summary>
        ///  void n_get_home_position_4(uint CardNo, out int X0Home, out int X1Home, out int X2Home, out int X3Home);
        /// </summary>
        public static RTC6Wrap.n_get_home_position_4Delegate n_get_home_position_4 = (RTC6Wrap.n_get_home_position_4Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_home_position_4Delegate>(nameof(n_get_home_position_4));
        /// <summary>void n_set_home_4_return_time(uint CardNo, uint Time);</summary>
        public static RTC6Wrap.n_set_home_4_return_timeDelegate n_set_home_4_return_time = (RTC6Wrap.n_set_home_4_return_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_home_4_return_timeDelegate>(nameof(n_set_home_4_return_time));
        /// <summary>uint n_get_home_4_return_time(uint CardNo);</summary>
        public static RTC6Wrap.n_get_home_4_return_timeDelegate n_get_home_4_return_time = (RTC6Wrap.n_get_home_4_return_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_home_4_return_timeDelegate>(nameof(n_get_home_4_return_time));
        /// <summary>
        ///  void n_home_position_xyz(uint CardNo, int XHome, int YHome, int ZHome);
        /// </summary>
        public static RTC6Wrap.n_home_position_xyzDelegate n_home_position_xyz = (RTC6Wrap.n_home_position_xyzDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_home_position_xyzDelegate>(nameof(n_home_position_xyz));
        /// <summary>
        ///  void n_home_position(uint CardNo, int XHome, int YHome);
        /// </summary>
        public static RTC6Wrap.n_home_positionDelegate n_home_position = (RTC6Wrap.n_home_positionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_home_positionDelegate>(nameof(n_home_position));
        /// <summary>uint n_uart_config(uint CardNo, uint BaudRate);</summary>
        public static RTC6Wrap.n_uart_configDelegate n_uart_config = (RTC6Wrap.n_uart_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_uart_configDelegate>(nameof(n_uart_config));
        /// <summary>void n_rs232_config(uint CardNo, uint BaudRate);</summary>
        public static RTC6Wrap.n_rs232_configDelegate n_rs232_config = (RTC6Wrap.n_rs232_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_rs232_configDelegate>(nameof(n_rs232_config));
        /// <summary>void n_rs232_write_data(uint CardNo, uint Data);</summary>
        public static RTC6Wrap.n_rs232_write_dataDelegate n_rs232_write_data = (RTC6Wrap.n_rs232_write_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_rs232_write_dataDelegate>(nameof(n_rs232_write_data));
        /// <summary>void n_rs232_write_text(uint CardNo, string pData);</summary>
        public static RTC6Wrap.n_rs232_write_textDelegate n_rs232_write_text = (RTC6Wrap.n_rs232_write_textDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_rs232_write_textDelegate>(nameof(n_rs232_write_text));
        /// <summary>uint n_rs232_read_data(uint CardNo);</summary>
        public static RTC6Wrap.n_rs232_read_dataDelegate n_rs232_read_data = (RTC6Wrap.n_rs232_read_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_rs232_read_dataDelegate>(nameof(n_rs232_read_data));
        /// <summary>uint n_set_mcbsp_freq(uint CardNo, uint Freq);</summary>
        public static RTC6Wrap.n_set_mcbsp_freqDelegate n_set_mcbsp_freq = (RTC6Wrap.n_set_mcbsp_freqDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_freqDelegate>(nameof(n_set_mcbsp_freq));
        /// <summary>
        ///  void n_mcbsp_init(uint CardNo, uint XDelay, uint RDelay);
        /// </summary>
        public static RTC6Wrap.n_mcbsp_initDelegate n_mcbsp_init = (RTC6Wrap.n_mcbsp_initDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mcbsp_initDelegate>(nameof(n_mcbsp_init));
        /// <summary>
        ///  void n_mcbsp_init_spi(uint CardNo, uint ClockLevel, uint ClockDelay);
        /// </summary>
        public static RTC6Wrap.n_mcbsp_init_spiDelegate n_mcbsp_init_spi = (RTC6Wrap.n_mcbsp_init_spiDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mcbsp_init_spiDelegate>(nameof(n_mcbsp_init_spi));
        /// <summary>uint n_get_overrun(uint CardNo);</summary>
        public static RTC6Wrap.n_get_overrunDelegate n_get_overrun = (RTC6Wrap.n_get_overrunDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_overrunDelegate>(nameof(n_get_overrun));
        /// <summary>uint n_get_master_slave(uint CardNo);</summary>
        public static RTC6Wrap.n_get_master_slaveDelegate n_get_master_slave = (RTC6Wrap.n_get_master_slaveDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_master_slaveDelegate>(nameof(n_get_master_slave));
        /// <summary>
        ///  void n_get_transform(uint CardNo, uint Number, int[] Ptr1, int[] Ptr2, uint[] Ptr, uint Code);
        /// </summary>
        public static RTC6Wrap.n_get_transformDelegate n_get_transform = (RTC6Wrap.n_get_transformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_transformDelegate>(nameof(n_get_transform));
        /// <summary>void n_stop_trigger(uint CardNo);</summary>
        public static RTC6Wrap.n_stop_triggerDelegate n_stop_trigger = (RTC6Wrap.n_stop_triggerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stop_triggerDelegate>(nameof(n_stop_trigger));
        /// <summary>void n_move_to(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_move_toDelegate n_move_to = (RTC6Wrap.n_move_toDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_move_toDelegate>(nameof(n_move_to));
        /// <summary>
        ///  void n_set_enduring_wobbel(uint CardNo, uint CenterX, uint CenterY, uint CenterZ, uint LimitHi, uint LimitLo, double ScaleX, double ScaleY, double ScaleZ);
        /// </summary>
        public static RTC6Wrap.n_set_enduring_wobbelDelegate n_set_enduring_wobbel = (RTC6Wrap.n_set_enduring_wobbelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_enduring_wobbelDelegate>(nameof(n_set_enduring_wobbel));
        /// <summary>
        ///  void n_set_enduring_wobbel_2(uint CardNo, uint CenterX, uint CenterY, uint CenterZ, uint LimitHi, uint LimitLo, double ScaleX, double ScaleY, double ScaleZ);
        /// </summary>
        public static RTC6Wrap.n_set_enduring_wobbel_2Delegate n_set_enduring_wobbel_2 = (RTC6Wrap.n_set_enduring_wobbel_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_enduring_wobbel_2Delegate>(nameof(n_set_enduring_wobbel_2));
        /// <summary>
        ///  void n_set_free_variable(uint CardNo, uint VarNo, uint Value);
        /// </summary>
        public static RTC6Wrap.n_set_free_variableDelegate n_set_free_variable = (RTC6Wrap.n_set_free_variableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_free_variableDelegate>(nameof(n_set_free_variable));
        /// <summary>uint n_get_free_variable(uint CardNo, uint VarNo);</summary>
        public static RTC6Wrap.n_get_free_variableDelegate n_get_free_variable = (RTC6Wrap.n_get_free_variableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_free_variableDelegate>(nameof(n_get_free_variable));
        /// <summary>
        ///  void n_set_mcbsp_out_ptr(uint CardNo, uint Number, uint[] SignalPtr);
        /// </summary>
        public static RTC6Wrap.n_set_mcbsp_out_ptrDelegate n_set_mcbsp_out_ptr = (RTC6Wrap.n_set_mcbsp_out_ptrDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_out_ptrDelegate>(nameof(n_set_mcbsp_out_ptr));
        /// <summary>
        ///  void n_periodic_toggle(uint CardNo, uint Port, uint Mask, uint P1, uint P2, uint Count, uint Start);
        /// </summary>
        public static RTC6Wrap.n_periodic_toggleDelegate n_periodic_toggle = (RTC6Wrap.n_periodic_toggleDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_periodic_toggleDelegate>(nameof(n_periodic_toggle));
        /// <summary>
        ///  void n_multi_axis_config(uint CardNo, uint Cfg, uint[] Ptr);
        /// </summary>
        public static RTC6Wrap.n_multi_axis_configDelegate n_multi_axis_config = (RTC6Wrap.n_multi_axis_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_multi_axis_configDelegate>(nameof(n_multi_axis_config));
        /// <summary>
        ///  void n_quad_axis_init(uint CardNo, uint Idle, double X1, double Y1);
        /// </summary>
        public static RTC6Wrap.n_quad_axis_initDelegate n_quad_axis_init = (RTC6Wrap.n_quad_axis_initDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_quad_axis_initDelegate>(nameof(n_quad_axis_init));
        /// <summary>uint n_quad_axis_get_status(uint CardNo);</summary>
        public static RTC6Wrap.n_quad_axis_get_statusDelegate n_quad_axis_get_status = (RTC6Wrap.n_quad_axis_get_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_quad_axis_get_statusDelegate>(nameof(n_quad_axis_get_status));
        /// <summary>
        ///  void n_quad_axis_get_values(uint CardNo, out double X1, out double Y1, out uint Flags0, out uint Flags1);
        /// </summary>
        public static RTC6Wrap.n_quad_axis_get_valuesDelegate n_quad_axis_get_values = (RTC6Wrap.n_quad_axis_get_valuesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_quad_axis_get_valuesDelegate>(nameof(n_quad_axis_get_values));
        /// <summary>double get_time();</summary>
        public static RTC6Wrap.get_timeDelegate get_time = (RTC6Wrap.get_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_timeDelegate>(nameof(get_time));
        /// <summary>double get_lap_time();</summary>
        public static RTC6Wrap.get_lap_timeDelegate get_lap_time = (RTC6Wrap.get_lap_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_lap_timeDelegate>(nameof(get_lap_time));
        /// <summary>void measurement_status(out uint Busy, out uint Pos);</summary>
        public static RTC6Wrap.measurement_statusDelegate measurement_status = (RTC6Wrap.measurement_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.measurement_statusDelegate>(nameof(measurement_status));
        /// <summary>
        ///  void get_waveform_offset(uint Channel, uint Offset, uint Number, int[] Ptr);
        /// </summary>
        public static RTC6Wrap.get_waveform_offsetDelegate get_waveform_offset = (RTC6Wrap.get_waveform_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_waveform_offsetDelegate>(nameof(get_waveform_offset));
        /// <summary>
        ///  void get_waveform(uint Channel, uint Number, int[] Ptr);
        /// </summary>
        public static RTC6Wrap.get_waveformDelegate get_waveform = (RTC6Wrap.get_waveformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_waveformDelegate>(nameof(get_waveform));
        /// <summary>void bounce_supp(uint Length);</summary>
        public static RTC6Wrap.bounce_suppDelegate bounce_supp = (RTC6Wrap.bounce_suppDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.bounce_suppDelegate>(nameof(bounce_supp));
        /// <summary>
        ///  void home_position_4(int X0Home, int X1Home, int X2Home, int X3Home);
        /// </summary>
        public static RTC6Wrap.home_position_4Delegate home_position_4 = (RTC6Wrap.home_position_4Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.home_position_4Delegate>(nameof(home_position_4));
        /// <summary>
        ///  void get_home_position_4(out int X0Home, out int X1Home, out int X2Home, out int X3Home);
        /// </summary>
        public static RTC6Wrap.get_home_position_4Delegate get_home_position_4 = (RTC6Wrap.get_home_position_4Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_home_position_4Delegate>(nameof(get_home_position_4));
        /// <summary>void set_home_4_return_time(uint Time);</summary>
        public static RTC6Wrap.set_home_4_return_timeDelegate set_home_4_return_time = (RTC6Wrap.set_home_4_return_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_home_4_return_timeDelegate>(nameof(set_home_4_return_time));
        /// <summary>uint get_home_4_return_time();</summary>
        public static RTC6Wrap.get_home_4_return_timeDelegate get_home_4_return_time = (RTC6Wrap.get_home_4_return_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_home_4_return_timeDelegate>(nameof(get_home_4_return_time));
        /// <summary>
        ///  void home_position_xyz(int XHome, int YHome, int ZHome);
        /// </summary>
        public static RTC6Wrap.home_position_xyzDelegate home_position_xyz = (RTC6Wrap.home_position_xyzDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.home_position_xyzDelegate>(nameof(home_position_xyz));
        /// <summary>void home_position(int XHome, int YHome);</summary>
        public static RTC6Wrap.home_positionDelegate home_position = (RTC6Wrap.home_positionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.home_positionDelegate>(nameof(home_position));
        /// <summary>uint uart_config(uint BaudRate);</summary>
        public static RTC6Wrap.uart_configDelegate uart_config = (RTC6Wrap.uart_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.uart_configDelegate>(nameof(uart_config));
        /// <summary>void rs232_config(uint BaudRate);</summary>
        public static RTC6Wrap.rs232_configDelegate rs232_config = (RTC6Wrap.rs232_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.rs232_configDelegate>(nameof(rs232_config));
        /// <summary>void rs232_write_data(uint Data);</summary>
        public static RTC6Wrap.rs232_write_dataDelegate rs232_write_data = (RTC6Wrap.rs232_write_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.rs232_write_dataDelegate>(nameof(rs232_write_data));
        /// <summary>void rs232_write_text(string pData);</summary>
        public static RTC6Wrap.rs232_write_textDelegate rs232_write_text = (RTC6Wrap.rs232_write_textDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.rs232_write_textDelegate>(nameof(rs232_write_text));
        /// <summary>uint rs232_read_data();</summary>
        public static RTC6Wrap.rs232_read_dataDelegate rs232_read_data = (RTC6Wrap.rs232_read_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.rs232_read_dataDelegate>(nameof(rs232_read_data));
        /// <summary>uint set_mcbsp_freq(uint Freq);</summary>
        public static RTC6Wrap.set_mcbsp_freqDelegate set_mcbsp_freq = (RTC6Wrap.set_mcbsp_freqDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_freqDelegate>(nameof(set_mcbsp_freq));
        /// <summary>void mcbsp_init(uint XDelay, uint RDelay);</summary>
        public static RTC6Wrap.mcbsp_initDelegate mcbsp_init = (RTC6Wrap.mcbsp_initDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mcbsp_initDelegate>(nameof(mcbsp_init));
        /// <summary>void mcbsp_init_spi(uint ClockLevel, uint ClockDelay);</summary>
        public static RTC6Wrap.mcbsp_init_spiDelegate mcbsp_init_spi = (RTC6Wrap.mcbsp_init_spiDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mcbsp_init_spiDelegate>(nameof(mcbsp_init_spi));
        /// <summary>uint get_overrun();</summary>
        public static RTC6Wrap.get_overrunDelegate get_overrun = (RTC6Wrap.get_overrunDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_overrunDelegate>(nameof(get_overrun));
        /// <summary>uint get_master_slave();</summary>
        public static RTC6Wrap.get_master_slaveDelegate get_master_slave = (RTC6Wrap.get_master_slaveDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_master_slaveDelegate>(nameof(get_master_slave));
        /// <summary>
        ///  void get_transform(uint Number, int[] Ptr1, int[] Ptr2, uint[] Ptr, uint Code);
        /// </summary>
        public static RTC6Wrap.get_transformDelegate get_transform = (RTC6Wrap.get_transformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_transformDelegate>(nameof(get_transform));
        /// <summary>void stop_trigger();</summary>
        public static RTC6Wrap.stop_triggerDelegate stop_trigger = (RTC6Wrap.stop_triggerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stop_triggerDelegate>(nameof(stop_trigger));
        /// <summary>void move_to(uint Pos);</summary>
        public static RTC6Wrap.move_toDelegate move_to = (RTC6Wrap.move_toDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.move_toDelegate>(nameof(move_to));
        /// <summary>
        ///  void set_enduring_wobbel(uint CenterX, uint CenterY, uint CenterZ, uint LimitHi, uint LimitLo, double ScaleX, double ScaleY, double ScaleZ);
        /// </summary>
        public static RTC6Wrap.set_enduring_wobbelDelegate set_enduring_wobbel = (RTC6Wrap.set_enduring_wobbelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_enduring_wobbelDelegate>(nameof(set_enduring_wobbel));
        /// <summary>
        ///  void set_enduring_wobbel_2(uint CenterX, uint CenterY, uint CenterZ, uint LimitHi, uint LimitLo, double ScaleX, double ScaleY, double ScaleZ);
        /// </summary>
        public static RTC6Wrap.set_enduring_wobbel_2Delegate set_enduring_wobbel_2 = (RTC6Wrap.set_enduring_wobbel_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_enduring_wobbel_2Delegate>(nameof(set_enduring_wobbel_2));
        /// <summary>void set_free_variable(uint VarNo, uint Value);</summary>
        public static RTC6Wrap.set_free_variableDelegate set_free_variable = (RTC6Wrap.set_free_variableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_free_variableDelegate>(nameof(set_free_variable));
        /// <summary>uint get_free_variable(uint VarNo);</summary>
        public static RTC6Wrap.get_free_variableDelegate get_free_variable = (RTC6Wrap.get_free_variableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_free_variableDelegate>(nameof(get_free_variable));
        /// <summary>void set_mcbsp_out_ptr(uint Number, uint[] SignalPtr);</summary>
        public static RTC6Wrap.set_mcbsp_out_ptrDelegate set_mcbsp_out_ptr = (RTC6Wrap.set_mcbsp_out_ptrDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_out_ptrDelegate>(nameof(set_mcbsp_out_ptr));
        /// <summary>
        ///  void periodic_toggle(uint Port, uint Mask, uint P1, uint P2, uint Count, uint Start);
        /// </summary>
        public static RTC6Wrap.periodic_toggleDelegate periodic_toggle = (RTC6Wrap.periodic_toggleDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.periodic_toggleDelegate>(nameof(periodic_toggle));
        /// <summary>void multi_axis_config(uint Cfg, uint[] Ptr);</summary>
        public static RTC6Wrap.multi_axis_configDelegate multi_axis_config = (RTC6Wrap.multi_axis_configDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.multi_axis_configDelegate>(nameof(multi_axis_config));
        /// <summary>void quad_axis_init(uint Idle, double X1, double Y1);</summary>
        public static RTC6Wrap.quad_axis_initDelegate quad_axis_init = (RTC6Wrap.quad_axis_initDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.quad_axis_initDelegate>(nameof(quad_axis_init));
        /// <summary>uint quad_axis_get_status();</summary>
        public static RTC6Wrap.quad_axis_get_statusDelegate quad_axis_get_status = (RTC6Wrap.quad_axis_get_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.quad_axis_get_statusDelegate>(nameof(quad_axis_get_status));
        /// <summary>
        ///  void quad_axis_get_values(out double X1, out double Y1, out uint Flags0, out uint Flags1);
        /// </summary>
        public static RTC6Wrap.quad_axis_get_valuesDelegate quad_axis_get_values = (RTC6Wrap.quad_axis_get_valuesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.quad_axis_get_valuesDelegate>(nameof(quad_axis_get_values));
        /// <summary>void n_set_defocus(uint CardNo, int Shift);</summary>
        public static RTC6Wrap.n_set_defocusDelegate n_set_defocus = (RTC6Wrap.n_set_defocusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_defocusDelegate>(nameof(n_set_defocus));
        /// <summary>void n_set_defocus_offset(uint CardNo, int Shift);</summary>
        public static RTC6Wrap.n_set_defocus_offsetDelegate n_set_defocus_offset = (RTC6Wrap.n_set_defocus_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_defocus_offsetDelegate>(nameof(n_set_defocus_offset));
        /// <summary>void n_goto_xyz(uint CardNo, int X, int Y, int Z);</summary>
        public static RTC6Wrap.n_goto_xyzDelegate n_goto_xyz = (RTC6Wrap.n_goto_xyzDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_goto_xyzDelegate>(nameof(n_goto_xyz));
        /// <summary>void n_set_zoom(uint CardNo, uint Zoom);</summary>
        public static RTC6Wrap.n_set_zoomDelegate n_set_zoom = (RTC6Wrap.n_set_zoomDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_zoomDelegate>(nameof(n_set_zoom));
        /// <summary>void n_goto_xy(uint CardNo, int X, int Y);</summary>
        public static RTC6Wrap.n_goto_xyDelegate n_goto_xy = (RTC6Wrap.n_goto_xyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_goto_xyDelegate>(nameof(n_goto_xy));
        /// <summary>
        ///  int n_get_z_distance(uint CardNo, int X, int Y, int Z);
        /// </summary>
        public static RTC6Wrap.n_get_z_distanceDelegate n_get_z_distance = (RTC6Wrap.n_get_z_distanceDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_z_distanceDelegate>(nameof(n_get_z_distance));
        /// <summary>void set_defocus(int Shift);</summary>
        public static RTC6Wrap.set_defocusDelegate set_defocus = (RTC6Wrap.set_defocusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_defocusDelegate>(nameof(set_defocus));
        /// <summary>void set_defocus_offset(int Shift);</summary>
        public static RTC6Wrap.set_defocus_offsetDelegate set_defocus_offset = (RTC6Wrap.set_defocus_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_defocus_offsetDelegate>(nameof(set_defocus_offset));
        /// <summary>void goto_xyz(int X, int Y, int Z);</summary>
        public static RTC6Wrap.goto_xyzDelegate goto_xyz = (RTC6Wrap.goto_xyzDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.goto_xyzDelegate>(nameof(goto_xyz));
        /// <summary>void goto_xy(int X, int Y);</summary>
        public static RTC6Wrap.goto_xyDelegate goto_xy = (RTC6Wrap.goto_xyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.goto_xyDelegate>(nameof(goto_xy));
        /// <summary>void set_zoom(uint Zoom);</summary>
        public static RTC6Wrap.set_zoomDelegate set_zoom = (RTC6Wrap.set_zoomDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_zoomDelegate>(nameof(set_zoom));
        /// <summary>int get_z_distance(int X, int Y, int Z);</summary>
        public static RTC6Wrap.get_z_distanceDelegate get_z_distance = (RTC6Wrap.get_z_distanceDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_z_distanceDelegate>(nameof(get_z_distance));
        /// <summary>
        ///  void n_set_offset_xyz(uint CardNo, uint HeadNo, int XOffset, int YOffset, int ZOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_offset_xyzDelegate n_set_offset_xyz = (RTC6Wrap.n_set_offset_xyzDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_offset_xyzDelegate>(nameof(n_set_offset_xyz));
        /// <summary>
        ///  void n_set_offset(uint CardNo, uint HeadNo, int XOffset, int YOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_offsetDelegate n_set_offset = (RTC6Wrap.n_set_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_offsetDelegate>(nameof(n_set_offset));
        /// <summary>
        ///  void n_set_matrix(uint CardNo, uint HeadNo, double M11, double M12, double M21, double M22, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_matrixDelegate n_set_matrix = (RTC6Wrap.n_set_matrixDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_matrixDelegate>(nameof(n_set_matrix));
        /// <summary>
        ///  void n_set_angle(uint CardNo, uint HeadNo, double Angle, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_angleDelegate n_set_angle = (RTC6Wrap.n_set_angleDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_angleDelegate>(nameof(n_set_angle));
        /// <summary>
        ///  void n_set_scale(uint CardNo, uint HeadNo, double Scale, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_scaleDelegate n_set_scale = (RTC6Wrap.n_set_scaleDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scaleDelegate>(nameof(n_set_scale));
        /// <summary>
        ///  void n_apply_mcbsp(uint CardNo, uint HeadNo, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_apply_mcbspDelegate n_apply_mcbsp = (RTC6Wrap.n_apply_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_apply_mcbspDelegate>(nameof(n_apply_mcbsp));
        /// <summary>
        ///  uint n_upload_transform(uint CardNo, uint HeadNo, uint[] Ptr);
        /// </summary>
        public static RTC6Wrap.n_upload_transformDelegate n_upload_transform = (RTC6Wrap.n_upload_transformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_upload_transformDelegate>(nameof(n_upload_transform));
        /// <summary>
        ///  void set_offset_xyz(uint HeadNo, int XOffset, int YOffset, int ZOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_offset_xyzDelegate set_offset_xyz = (RTC6Wrap.set_offset_xyzDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_offset_xyzDelegate>(nameof(set_offset_xyz));
        /// <summary>
        ///  void set_offset(uint HeadNo, int XOffset, int YOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_offsetDelegate set_offset = (RTC6Wrap.set_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_offsetDelegate>(nameof(set_offset));
        /// <summary>
        ///  void set_matrix(uint HeadNo, double M11, double M12, double M21, double M22, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_matrixDelegate set_matrix = (RTC6Wrap.set_matrixDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_matrixDelegate>(nameof(set_matrix));
        /// <summary>
        ///  void set_angle(uint HeadNo, double Angle, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_angleDelegate set_angle = (RTC6Wrap.set_angleDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_angleDelegate>(nameof(set_angle));
        /// <summary>
        ///  void set_scale(uint HeadNo, double Scale, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_scaleDelegate set_scale = (RTC6Wrap.set_scaleDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scaleDelegate>(nameof(set_scale));
        /// <summary>void apply_mcbsp(uint HeadNo, uint at_once);</summary>
        public static RTC6Wrap.apply_mcbspDelegate apply_mcbsp = (RTC6Wrap.apply_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.apply_mcbspDelegate>(nameof(apply_mcbsp));
        /// <summary>uint upload_transform(uint HeadNo, uint[] Ptr);</summary>
        public static RTC6Wrap.upload_transformDelegate upload_transform = (RTC6Wrap.upload_transformDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.upload_transformDelegate>(nameof(upload_transform));
        /// <summary>
        ///  void n_set_delay_mode(uint CardNo, uint VarPoly, uint DirectMove3D, uint EdgeLevel, uint MinJumpDelay, uint JumpLengthLimit);
        /// </summary>
        public static RTC6Wrap.n_set_delay_modeDelegate n_set_delay_mode = (RTC6Wrap.n_set_delay_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_delay_modeDelegate>(nameof(n_set_delay_mode));
        /// <summary>void n_set_jump_speed_ctrl(uint CardNo, double Speed);</summary>
        public static RTC6Wrap.n_set_jump_speed_ctrlDelegate n_set_jump_speed_ctrl = (RTC6Wrap.n_set_jump_speed_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_jump_speed_ctrlDelegate>(nameof(n_set_jump_speed_ctrl));
        /// <summary>void n_set_mark_speed_ctrl(uint CardNo, double Speed);</summary>
        public static RTC6Wrap.n_set_mark_speed_ctrlDelegate n_set_mark_speed_ctrl = (RTC6Wrap.n_set_mark_speed_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mark_speed_ctrlDelegate>(nameof(n_set_mark_speed_ctrl));
        /// <summary>
        ///  void n_set_sky_writing_para(uint CardNo, double Timelag, int LaserOnShift, uint Nprev, uint Npost);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writing_paraDelegate n_set_sky_writing_para = (RTC6Wrap.n_set_sky_writing_paraDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_paraDelegate>(nameof(n_set_sky_writing_para));
        /// <summary>
        ///  void n_set_sky_writing_limit(uint CardNo, double CosAngle);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writing_limitDelegate n_set_sky_writing_limit = (RTC6Wrap.n_set_sky_writing_limitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_limitDelegate>(nameof(n_set_sky_writing_limit));
        /// <summary>void n_set_sky_writing_mode(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_set_sky_writing_modeDelegate n_set_sky_writing_mode = (RTC6Wrap.n_set_sky_writing_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_modeDelegate>(nameof(n_set_sky_writing_mode));
        /// <summary>
        ///  int n_load_varpolydelay(uint CardNo, string Name, uint No);
        /// </summary>
        public static RTC6Wrap.n_load_varpolydelayDelegate n_load_varpolydelay = (RTC6Wrap.n_load_varpolydelayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_varpolydelayDelegate>(nameof(n_load_varpolydelay));
        /// <summary>
        ///  void n_set_hi(uint CardNo, uint HeadNo, double GalvoGainX, double GalvoGainY, int GalvoOffsetX, int GalvoOffsetY);
        /// </summary>
        public static RTC6Wrap.n_set_hiDelegate n_set_hi = (RTC6Wrap.n_set_hiDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_hiDelegate>(nameof(n_set_hi));
        /// <summary>
        ///  void n_get_hi_pos(uint CardNo, uint HeadNo, out int X1, out int X2, out int Y1, out int Y2);
        /// </summary>
        public static RTC6Wrap.n_get_hi_posDelegate n_get_hi_pos = (RTC6Wrap.n_get_hi_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_hi_posDelegate>(nameof(n_get_hi_pos));
        /// <summary>
        ///  uint n_auto_cal(uint CardNo, uint HeadNo, uint Command);
        /// </summary>
        public static RTC6Wrap.n_auto_calDelegate n_auto_cal = (RTC6Wrap.n_auto_calDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_auto_calDelegate>(nameof(n_auto_cal));
        /// <summary>uint n_get_auto_cal(uint CardNo, uint HeadNo);</summary>
        public static RTC6Wrap.n_get_auto_calDelegate n_get_auto_cal = (RTC6Wrap.n_get_auto_calDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_auto_calDelegate>(nameof(n_get_auto_cal));
        /// <summary>
        ///  uint n_write_hi_pos(uint CardNo, uint HeadNo, int X1, int X2, int Y1, int Y2);
        /// </summary>
        public static RTC6Wrap.n_write_hi_posDelegate n_write_hi_pos = (RTC6Wrap.n_write_hi_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_hi_posDelegate>(nameof(n_write_hi_pos));
        /// <summary>
        ///  void n_set_timelag_compensation(uint CardNo, uint HeadNo, uint TimeLagXY, uint TimeLagZ);
        /// </summary>
        public static RTC6Wrap.n_set_timelag_compensationDelegate n_set_timelag_compensation = (RTC6Wrap.n_set_timelag_compensationDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_timelag_compensationDelegate>(nameof(n_set_timelag_compensation));
        /// <summary>
        ///  void n_set_sky_writing(uint CardNo, double Timelag, int LaserOnShift);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writingDelegate n_set_sky_writing = (RTC6Wrap.n_set_sky_writingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writingDelegate>(nameof(n_set_sky_writing));
        /// <summary>
        ///  void n_get_hi_data(uint CardNo, out int X1, out int X2, out int Y1, out int Y2);
        /// </summary>
        public static RTC6Wrap.n_get_hi_dataDelegate n_get_hi_data = (RTC6Wrap.n_get_hi_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_hi_dataDelegate>(nameof(n_get_hi_data));
        /// <summary>
        ///  void set_delay_mode(uint VarPoly, uint DirectMove3D, uint EdgeLevel, uint MinJumpDelay, uint JumpLengthLimit);
        /// </summary>
        public static RTC6Wrap.set_delay_modeDelegate set_delay_mode = (RTC6Wrap.set_delay_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_delay_modeDelegate>(nameof(set_delay_mode));
        /// <summary>void set_jump_speed_ctrl(double Speed);</summary>
        public static RTC6Wrap.set_jump_speed_ctrlDelegate set_jump_speed_ctrl = (RTC6Wrap.set_jump_speed_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_jump_speed_ctrlDelegate>(nameof(set_jump_speed_ctrl));
        /// <summary>void set_mark_speed_ctrl(double Speed);</summary>
        public static RTC6Wrap.set_mark_speed_ctrlDelegate set_mark_speed_ctrl = (RTC6Wrap.set_mark_speed_ctrlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mark_speed_ctrlDelegate>(nameof(set_mark_speed_ctrl));
        /// <summary>
        ///  void set_sky_writing_para(double Timelag, int LaserOnShift, uint Nprev, uint Npost);
        /// </summary>
        public static RTC6Wrap.set_sky_writing_paraDelegate set_sky_writing_para = (RTC6Wrap.set_sky_writing_paraDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_paraDelegate>(nameof(set_sky_writing_para));
        /// <summary>void set_sky_writing_limit(double CosAngle);</summary>
        public static RTC6Wrap.set_sky_writing_limitDelegate set_sky_writing_limit = (RTC6Wrap.set_sky_writing_limitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_limitDelegate>(nameof(set_sky_writing_limit));
        /// <summary>void set_sky_writing_mode(uint Mode);</summary>
        public static RTC6Wrap.set_sky_writing_modeDelegate set_sky_writing_mode = (RTC6Wrap.set_sky_writing_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_modeDelegate>(nameof(set_sky_writing_mode));
        /// <summary>int load_varpolydelay(string Name, uint No);</summary>
        public static RTC6Wrap.load_varpolydelayDelegate load_varpolydelay = (RTC6Wrap.load_varpolydelayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_varpolydelayDelegate>(nameof(load_varpolydelay));
        /// <summary>
        ///  void set_hi(uint HeadNo, double GalvoGainX, double GalvoGainY, int GalvoOffsetX, int GalvoOffsetY);
        /// </summary>
        public static RTC6Wrap.set_hiDelegate set_hi = (RTC6Wrap.set_hiDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_hiDelegate>(nameof(set_hi));
        /// <summary>
        ///  void get_hi_pos(uint HeadNo, out int X1, out int X2, out int Y1, out int Y2);
        /// </summary>
        public static RTC6Wrap.get_hi_posDelegate get_hi_pos = (RTC6Wrap.get_hi_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_hi_posDelegate>(nameof(get_hi_pos));
        /// <summary>uint auto_cal(uint HeadNo, uint Command);</summary>
        public static RTC6Wrap.auto_calDelegate auto_cal = (RTC6Wrap.auto_calDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.auto_calDelegate>(nameof(auto_cal));
        /// <summary>uint get_auto_cal(uint HeadNo);</summary>
        public static RTC6Wrap.get_auto_calDelegate get_auto_cal = (RTC6Wrap.get_auto_calDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_auto_calDelegate>(nameof(get_auto_cal));
        /// <summary>
        ///  uint write_hi_pos(uint HeadNo, int X1, int X2, int Y1, int Y2);
        /// </summary>
        public static RTC6Wrap.write_hi_posDelegate write_hi_pos = (RTC6Wrap.write_hi_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_hi_posDelegate>(nameof(write_hi_pos));
        /// <summary>
        ///  void set_timelag_compensation(uint HeadNo, uint TimeLagXY, uint TimeLagZ);
        /// </summary>
        public static RTC6Wrap.set_timelag_compensationDelegate set_timelag_compensation = (RTC6Wrap.set_timelag_compensationDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_timelag_compensationDelegate>(nameof(set_timelag_compensation));
        /// <summary>
        ///  void set_sky_writing(double Timelag, int LaserOnShift);
        /// </summary>
        public static RTC6Wrap.set_sky_writingDelegate set_sky_writing = (RTC6Wrap.set_sky_writingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writingDelegate>(nameof(set_sky_writing));
        /// <summary>
        ///  void get_hi_data(out int X1, out int X2, out int Y1, out int Y2);
        /// </summary>
        public static RTC6Wrap.get_hi_dataDelegate get_hi_data = (RTC6Wrap.get_hi_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_hi_dataDelegate>(nameof(get_hi_data));
        /// <summary>
        ///  void n_send_user_data(uint CardNo, uint Head, uint Axis, int Data0, int Data1, int Data2, int Data3, int Data4);
        /// </summary>
        public static RTC6Wrap.n_send_user_dataDelegate n_send_user_data = (RTC6Wrap.n_send_user_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_send_user_dataDelegate>(nameof(n_send_user_data));
        /// <summary>
        ///  int n_read_user_data(uint CardNo, uint Head, uint Axis, out int Data0, out int Data1, out int Data2, out int Data3, out int Data4);
        /// </summary>
        public static RTC6Wrap.n_read_user_dataDelegate n_read_user_data = (RTC6Wrap.n_read_user_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_user_dataDelegate>(nameof(n_read_user_data));
        /// <summary>
        ///  void n_control_command(uint CardNo, uint Head, uint Axis, uint Data);
        /// </summary>
        public static RTC6Wrap.n_control_commandDelegate n_control_command = (RTC6Wrap.n_control_commandDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_control_commandDelegate>(nameof(n_control_command));
        /// <summary>int n_get_value(uint CardNo, uint Signal);</summary>
        public static RTC6Wrap.n_get_valueDelegate n_get_value = (RTC6Wrap.n_get_valueDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_valueDelegate>(nameof(n_get_value));
        /// <summary>
        ///  void n_get_values(uint CardNo, uint[] SignalPtr, int[] ResultPtr);
        /// </summary>
        public static RTC6Wrap.n_get_valuesDelegate n_get_values = (RTC6Wrap.n_get_valuesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_valuesDelegate>(nameof(n_get_values));
        /// <summary>
        ///  void n_get_galvo_controls(uint CardNo, int[] SignalPtr, int[] ResultPtr);
        /// </summary>
        public static RTC6Wrap.n_get_galvo_controlsDelegate n_get_galvo_controls = (RTC6Wrap.n_get_galvo_controlsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_galvo_controlsDelegate>(nameof(n_get_galvo_controls));
        /// <summary>uint n_get_head_status(uint CardNo, uint Head);</summary>
        public static RTC6Wrap.n_get_head_statusDelegate n_get_head_status = (RTC6Wrap.n_get_head_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_head_statusDelegate>(nameof(n_get_head_status));
        /// <summary>
        ///  int n_set_jump_mode(uint CardNo, int Flag, uint Length, int VA1, int VA2, int VB1, int VB2, int JA1, int JA2, int JB1, int JB2);
        /// </summary>
        public static RTC6Wrap.n_set_jump_modeDelegate n_set_jump_mode = (RTC6Wrap.n_set_jump_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_jump_modeDelegate>(nameof(n_set_jump_mode));
        /// <summary>
        ///  int n_load_jump_table_offset(uint CardNo, string Name, uint No, uint PosAck, int Offset, uint MinDelay, uint MaxDelay, uint ListPos);
        /// </summary>
        public static RTC6Wrap.n_load_jump_table_offsetDelegate n_load_jump_table_offset = (RTC6Wrap.n_load_jump_table_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_jump_table_offsetDelegate>(nameof(n_load_jump_table_offset));
        /// <summary>uint n_get_jump_table(uint CardNo, ushort[] Ptr);</summary>
        public static RTC6Wrap.n_get_jump_tableDelegate n_get_jump_table = (RTC6Wrap.n_get_jump_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_jump_tableDelegate>(nameof(n_get_jump_table));
        /// <summary>uint n_set_jump_table(uint CardNo, ushort[] Ptr);</summary>
        public static RTC6Wrap.n_set_jump_tableDelegate n_set_jump_table = (RTC6Wrap.n_set_jump_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_jump_tableDelegate>(nameof(n_set_jump_table));
        /// <summary>
        ///  int n_load_jump_table(uint CardNo, string Name, uint No, uint PosAck, uint MinDelay, uint MaxDelay, uint ListPos);
        /// </summary>
        public static RTC6Wrap.n_load_jump_tableDelegate n_load_jump_table = (RTC6Wrap.n_load_jump_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_load_jump_tableDelegate>(nameof(n_load_jump_table));
        /// <summary>
        ///  void send_user_data(uint Head, uint Axis, int Data0, int Data1, int Data2, int Data3, int Data4);
        /// </summary>
        public static RTC6Wrap.send_user_dataDelegate send_user_data = (RTC6Wrap.send_user_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.send_user_dataDelegate>(nameof(send_user_data));
        /// <summary>
        ///  int read_user_data(uint Head, uint Axis, out int Data0, out int Data1, out int Data2, out int Data3, out int Data4);
        /// </summary>
        public static RTC6Wrap.read_user_dataDelegate read_user_data = (RTC6Wrap.read_user_dataDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_user_dataDelegate>(nameof(read_user_data));
        /// <summary>void control_command(uint Head, uint Axis, uint Data);</summary>
        public static RTC6Wrap.control_commandDelegate control_command = (RTC6Wrap.control_commandDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.control_commandDelegate>(nameof(control_command));
        /// <summary>int get_value(uint Signal);</summary>
        public static RTC6Wrap.get_valueDelegate get_value = (RTC6Wrap.get_valueDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_valueDelegate>(nameof(get_value));
        /// <summary>void get_values(uint[] SignalPtr, int[] ResultPtr);</summary>
        public static RTC6Wrap.get_valuesDelegate get_values = (RTC6Wrap.get_valuesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_valuesDelegate>(nameof(get_values));
        /// <summary>
        ///  void get_galvo_controls(int[] SignalPtr, int[] ResultPtr);
        /// </summary>
        public static RTC6Wrap.get_galvo_controlsDelegate get_galvo_controls = (RTC6Wrap.get_galvo_controlsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_galvo_controlsDelegate>(nameof(get_galvo_controls));
        /// <summary>uint get_head_status(uint Head);</summary>
        public static RTC6Wrap.get_head_statusDelegate get_head_status = (RTC6Wrap.get_head_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_head_statusDelegate>(nameof(get_head_status));
        /// <summary>
        ///  int set_jump_mode(int Flag, uint Length, int VA1, int VA2, int VB1, int VB2, int JA1, int JA2, int JB1, int JB2);
        /// </summary>
        public static RTC6Wrap.set_jump_modeDelegate set_jump_mode = (RTC6Wrap.set_jump_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_jump_modeDelegate>(nameof(set_jump_mode));
        /// <summary>
        ///  int load_jump_table_offset(string Name, uint No, uint PosAck, int Offset, uint MinDelay, uint MaxDelay, uint ListPos);
        /// </summary>
        public static RTC6Wrap.load_jump_table_offsetDelegate load_jump_table_offset = (RTC6Wrap.load_jump_table_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_jump_table_offsetDelegate>(nameof(load_jump_table_offset));
        /// <summary>uint get_jump_table(ushort[] Ptr);</summary>
        public static RTC6Wrap.get_jump_tableDelegate get_jump_table = (RTC6Wrap.get_jump_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_jump_tableDelegate>(nameof(get_jump_table));
        /// <summary>uint set_jump_table(ushort[] Ptr);</summary>
        public static RTC6Wrap.set_jump_tableDelegate set_jump_table = (RTC6Wrap.set_jump_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_jump_tableDelegate>(nameof(set_jump_table));
        /// <summary>
        ///  int load_jump_table(string Name, uint No, uint PosAck, uint MinDelay, uint MaxDelay, uint ListPos);
        /// </summary>
        public static RTC6Wrap.load_jump_tableDelegate load_jump_table = (RTC6Wrap.load_jump_tableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.load_jump_tableDelegate>(nameof(load_jump_table));
        /// <summary>
        ///  uint n_get_scanahead_params(uint CardNo, uint HeadNo, out uint PreViewTime, out uint Vmax, out double Amax);
        /// </summary>
        public static RTC6Wrap.n_get_scanahead_paramsDelegate n_get_scanahead_params = (RTC6Wrap.n_get_scanahead_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_scanahead_paramsDelegate>(nameof(n_get_scanahead_params));
        /// <summary>
        ///  int n_activate_scanahead_autodelays(uint CardNo, int Mode);
        /// </summary>
        public static RTC6Wrap.n_activate_scanahead_autodelaysDelegate n_activate_scanahead_autodelays = (RTC6Wrap.n_activate_scanahead_autodelaysDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_activate_scanahead_autodelaysDelegate>(nameof(n_activate_scanahead_autodelays));
        /// <summary>
        ///  void n_set_scanahead_laser_shifts(uint CardNo, int dLasOn, int dLasOff);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_laser_shiftsDelegate n_set_scanahead_laser_shifts = (RTC6Wrap.n_set_scanahead_laser_shiftsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_laser_shiftsDelegate>(nameof(n_set_scanahead_laser_shifts));
        /// <summary>
        ///  void n_set_scanahead_line_params(uint CardNo, uint CornerScale, uint EndScale, uint AccScale);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_line_paramsDelegate n_set_scanahead_line_params = (RTC6Wrap.n_set_scanahead_line_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_line_paramsDelegate>(nameof(n_set_scanahead_line_params));
        /// <summary>
        ///  void n_set_scanahead_line_params_ex(uint CardNo, uint CornerScale, uint EndScale, uint AccScale, uint JumpScale);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_line_params_exDelegate n_set_scanahead_line_params_ex = (RTC6Wrap.n_set_scanahead_line_params_exDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_line_params_exDelegate>(nameof(n_set_scanahead_line_params_ex));
        /// <summary>
        ///  uint n_set_scanahead_params(uint CardNo, uint Mode, uint HeadNo, uint TableNo, uint PreViewTime, uint Vmax, double Amax);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_paramsDelegate n_set_scanahead_params = (RTC6Wrap.n_set_scanahead_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_paramsDelegate>(nameof(n_set_scanahead_params));
        /// <summary>
        ///  void n_set_scanahead_speed_control(uint CardNo, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_speed_controlDelegate n_set_scanahead_speed_control = (RTC6Wrap.n_set_scanahead_speed_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_speed_controlDelegate>(nameof(n_set_scanahead_speed_control));
        /// <summary>
        ///  uint get_scanahead_params(uint HeadNo, out uint PreViewTime, out uint Vmax, out double Amax);
        /// </summary>
        public static RTC6Wrap.get_scanahead_paramsDelegate get_scanahead_params = (RTC6Wrap.get_scanahead_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_scanahead_paramsDelegate>(nameof(get_scanahead_params));
        /// <summary>int activate_scanahead_autodelays(int Mode);</summary>
        public static RTC6Wrap.activate_scanahead_autodelaysDelegate activate_scanahead_autodelays = (RTC6Wrap.activate_scanahead_autodelaysDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.activate_scanahead_autodelaysDelegate>(nameof(activate_scanahead_autodelays));
        /// <summary>
        ///  void set_scanahead_laser_shifts(int dLasOn, int dLasOff);
        /// </summary>
        public static RTC6Wrap.set_scanahead_laser_shiftsDelegate set_scanahead_laser_shifts = (RTC6Wrap.set_scanahead_laser_shiftsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_laser_shiftsDelegate>(nameof(set_scanahead_laser_shifts));
        /// <summary>
        ///  void set_scanahead_line_params(uint CornerScale, uint EndScale, uint AccScale);
        /// </summary>
        public static RTC6Wrap.set_scanahead_line_paramsDelegate set_scanahead_line_params = (RTC6Wrap.set_scanahead_line_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_line_paramsDelegate>(nameof(set_scanahead_line_params));
        /// <summary>
        ///  void set_scanahead_line_params_ex(uint CornerScale, uint EndScale, uint AccScale, uint JumpScale);
        /// </summary>
        public static RTC6Wrap.set_scanahead_line_params_exDelegate set_scanahead_line_params_ex = (RTC6Wrap.set_scanahead_line_params_exDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_line_params_exDelegate>(nameof(set_scanahead_line_params_ex));
        /// <summary>
        ///  uint set_scanahead_params(uint Mode, uint HeadNo, uint TableNo, uint PreViewTime, uint Vmax, double Amax);
        /// </summary>
        public static RTC6Wrap.set_scanahead_paramsDelegate set_scanahead_params = (RTC6Wrap.set_scanahead_paramsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_paramsDelegate>(nameof(set_scanahead_params));
        /// <summary>void set_scanahead_speed_control(uint Mode);</summary>
        public static RTC6Wrap.set_scanahead_speed_controlDelegate set_scanahead_speed_control = (RTC6Wrap.set_scanahead_speed_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_speed_controlDelegate>(nameof(set_scanahead_speed_control));
        /// <summary>
        ///  void n_stepper_init(uint CardNo, uint No, uint Period, int Dir, int Pos, uint Tol, uint Enable, uint WaitTime);
        /// </summary>
        public static RTC6Wrap.n_stepper_initDelegate n_stepper_init = (RTC6Wrap.n_stepper_initDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_initDelegate>(nameof(n_stepper_init));
        /// <summary>
        ///  void n_stepper_enable(uint CardNo, int Enable1, int Enable2);
        /// </summary>
        public static RTC6Wrap.n_stepper_enableDelegate n_stepper_enable = (RTC6Wrap.n_stepper_enableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_enableDelegate>(nameof(n_stepper_enable));
        /// <summary>
        ///  void n_stepper_disable_switch(uint CardNo, int Disable1, int Disable2);
        /// </summary>
        public static RTC6Wrap.n_stepper_disable_switchDelegate n_stepper_disable_switch = (RTC6Wrap.n_stepper_disable_switchDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_disable_switchDelegate>(nameof(n_stepper_disable_switch));
        /// <summary>
        ///  void n_stepper_control(uint CardNo, int Period1, int Period2);
        /// </summary>
        public static RTC6Wrap.n_stepper_controlDelegate n_stepper_control = (RTC6Wrap.n_stepper_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_controlDelegate>(nameof(n_stepper_control));
        /// <summary>
        ///  void n_stepper_abs_no(uint CardNo, uint No, int Pos, uint WaitTime);
        /// </summary>
        public static RTC6Wrap.n_stepper_abs_noDelegate n_stepper_abs_no = (RTC6Wrap.n_stepper_abs_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_abs_noDelegate>(nameof(n_stepper_abs_no));
        /// <summary>
        ///  void n_stepper_rel_no(uint CardNo, uint No, int dPos, uint WaitTime);
        /// </summary>
        public static RTC6Wrap.n_stepper_rel_noDelegate n_stepper_rel_no = (RTC6Wrap.n_stepper_rel_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_rel_noDelegate>(nameof(n_stepper_rel_no));
        /// <summary>
        ///  void n_stepper_abs(uint CardNo, int Pos1, int Pos2, uint WaitTime);
        /// </summary>
        public static RTC6Wrap.n_stepper_absDelegate n_stepper_abs = (RTC6Wrap.n_stepper_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_absDelegate>(nameof(n_stepper_abs));
        /// <summary>
        ///  void n_stepper_rel(uint CardNo, int dPos1, int dPos2, uint WaitTime);
        /// </summary>
        public static RTC6Wrap.n_stepper_relDelegate n_stepper_rel = (RTC6Wrap.n_stepper_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_relDelegate>(nameof(n_stepper_rel));
        /// <summary>
        ///  void n_get_stepper_status(uint CardNo, out uint Status1, out int Pos1, out uint Status2, out int Pos2);
        /// </summary>
        public static RTC6Wrap.n_get_stepper_statusDelegate n_get_stepper_status = (RTC6Wrap.n_get_stepper_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_stepper_statusDelegate>(nameof(n_get_stepper_status));
        /// <summary>
        ///  void stepper_init(uint No, uint Period, int Dir, int Pos, uint Tol, uint Enable, uint WaitTime);
        /// </summary>
        public static RTC6Wrap.stepper_initDelegate stepper_init = (RTC6Wrap.stepper_initDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_initDelegate>(nameof(stepper_init));
        /// <summary>void stepper_enable(int Enable1, int Enable2);</summary>
        public static RTC6Wrap.stepper_enableDelegate stepper_enable = (RTC6Wrap.stepper_enableDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_enableDelegate>(nameof(stepper_enable));
        /// <summary>
        ///  void stepper_disable_switch(int Disable1, int Disable2);
        /// </summary>
        public static RTC6Wrap.stepper_disable_switchDelegate stepper_disable_switch = (RTC6Wrap.stepper_disable_switchDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_disable_switchDelegate>(nameof(stepper_disable_switch));
        /// <summary>void stepper_control(int Period1, int Period2);</summary>
        public static RTC6Wrap.stepper_controlDelegate stepper_control = (RTC6Wrap.stepper_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_controlDelegate>(nameof(stepper_control));
        /// <summary>void stepper_abs_no(uint No, int Pos, uint WaitTime);</summary>
        public static RTC6Wrap.stepper_abs_noDelegate stepper_abs_no = (RTC6Wrap.stepper_abs_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_abs_noDelegate>(nameof(stepper_abs_no));
        /// <summary>void stepper_rel_no(uint No, int dPos, uint WaitTime);</summary>
        public static RTC6Wrap.stepper_rel_noDelegate stepper_rel_no = (RTC6Wrap.stepper_rel_noDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_rel_noDelegate>(nameof(stepper_rel_no));
        /// <summary>void stepper_abs(int Pos1, int Pos2, uint WaitTime);</summary>
        public static RTC6Wrap.stepper_absDelegate stepper_abs = (RTC6Wrap.stepper_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_absDelegate>(nameof(stepper_abs));
        /// <summary>void stepper_rel(int dPos1, int dPos2, uint WaitTime);</summary>
        public static RTC6Wrap.stepper_relDelegate stepper_rel = (RTC6Wrap.stepper_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_relDelegate>(nameof(stepper_rel));
        /// <summary>
        ///  void get_stepper_status(out uint Status1, out int Pos1, out uint Status2, out int Pos2);
        /// </summary>
        public static RTC6Wrap.get_stepper_statusDelegate get_stepper_status = (RTC6Wrap.get_stepper_statusDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_stepper_statusDelegate>(nameof(get_stepper_status));
        /// <summary>
        ///  void n_select_cor_table_list(uint CardNo, uint HeadA, uint HeadB);
        /// </summary>
        public static RTC6Wrap.n_select_cor_table_listDelegate n_select_cor_table_list = (RTC6Wrap.n_select_cor_table_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_select_cor_table_listDelegate>(nameof(n_select_cor_table_list));
        /// <summary>void select_cor_table_list(uint HeadA, uint HeadB);</summary>
        public static RTC6Wrap.select_cor_table_listDelegate select_cor_table_list = (RTC6Wrap.select_cor_table_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.select_cor_table_listDelegate>(nameof(select_cor_table_list));
        /// <summary>void n_list_nop(uint CardNo);</summary>
        public static RTC6Wrap.n_list_nopDelegate n_list_nop = (RTC6Wrap.n_list_nopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_nopDelegate>(nameof(n_list_nop));
        /// <summary>void n_list_continue(uint CardNo);</summary>
        public static RTC6Wrap.n_list_continueDelegate n_list_continue = (RTC6Wrap.n_list_continueDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_continueDelegate>(nameof(n_list_continue));
        /// <summary>void n_list_next(uint CardNo);</summary>
        public static RTC6Wrap.n_list_nextDelegate n_list_next = (RTC6Wrap.n_list_nextDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_nextDelegate>(nameof(n_list_next));
        /// <summary>void n_long_delay(uint CardNo, uint Delay);</summary>
        public static RTC6Wrap.n_long_delayDelegate n_long_delay = (RTC6Wrap.n_long_delayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_long_delayDelegate>(nameof(n_long_delay));
        /// <summary>void n_set_end_of_list(uint CardNo);</summary>
        public static RTC6Wrap.n_set_end_of_listDelegate n_set_end_of_list = (RTC6Wrap.n_set_end_of_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_end_of_listDelegate>(nameof(n_set_end_of_list));
        /// <summary>void n_set_wait(uint CardNo, uint WaitWord);</summary>
        public static RTC6Wrap.n_set_waitDelegate n_set_wait = (RTC6Wrap.n_set_waitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_waitDelegate>(nameof(n_set_wait));
        /// <summary>void n_list_jump_pos(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_list_jump_posDelegate n_list_jump_pos = (RTC6Wrap.n_list_jump_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_jump_posDelegate>(nameof(n_list_jump_pos));
        /// <summary>void n_list_jump_rel(uint CardNo, int Pos);</summary>
        public static RTC6Wrap.n_list_jump_relDelegate n_list_jump_rel = (RTC6Wrap.n_list_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_jump_relDelegate>(nameof(n_list_jump_rel));
        /// <summary>void n_list_repeat(uint CardNo);</summary>
        public static RTC6Wrap.n_list_repeatDelegate n_list_repeat = (RTC6Wrap.n_list_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_repeatDelegate>(nameof(n_list_repeat));
        /// <summary>void n_list_until(uint CardNo, uint Number);</summary>
        public static RTC6Wrap.n_list_untilDelegate n_list_until = (RTC6Wrap.n_list_untilDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_untilDelegate>(nameof(n_list_until));
        /// <summary>
        ///  void n_range_checking(uint CardNo, uint HeadNo, uint Mode, uint Data);
        /// </summary>
        public static RTC6Wrap.n_range_checkingDelegate n_range_checking = (RTC6Wrap.n_range_checkingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_range_checkingDelegate>(nameof(n_range_checking));
        /// <summary>void n_set_list_jump(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_set_list_jumpDelegate n_set_list_jump = (RTC6Wrap.n_set_list_jumpDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_list_jumpDelegate>(nameof(n_set_list_jump));
        /// <summary>void list_nop();</summary>
        public static RTC6Wrap.list_nopDelegate list_nop = (RTC6Wrap.list_nopDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_nopDelegate>(nameof(list_nop));
        /// <summary>void list_continue();</summary>
        public static RTC6Wrap.list_continueDelegate list_continue = (RTC6Wrap.list_continueDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_continueDelegate>(nameof(list_continue));
        /// <summary>void list_next();</summary>
        public static RTC6Wrap.list_nextDelegate list_next = (RTC6Wrap.list_nextDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_nextDelegate>(nameof(list_next));
        /// <summary>void long_delay(uint Delay);</summary>
        public static RTC6Wrap.long_delayDelegate long_delay = (RTC6Wrap.long_delayDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.long_delayDelegate>(nameof(long_delay));
        /// <summary>void set_end_of_list();</summary>
        public static RTC6Wrap.set_end_of_listDelegate set_end_of_list = (RTC6Wrap.set_end_of_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_end_of_listDelegate>(nameof(set_end_of_list));
        /// <summary>void set_wait(uint WaitWord);</summary>
        public static RTC6Wrap.set_waitDelegate set_wait = (RTC6Wrap.set_waitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_waitDelegate>(nameof(set_wait));
        /// <summary>void list_jump_pos(uint Pos);</summary>
        public static RTC6Wrap.list_jump_posDelegate list_jump_pos = (RTC6Wrap.list_jump_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_jump_posDelegate>(nameof(list_jump_pos));
        /// <summary>void list_jump_rel(int Pos);</summary>
        public static RTC6Wrap.list_jump_relDelegate list_jump_rel = (RTC6Wrap.list_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_jump_relDelegate>(nameof(list_jump_rel));
        /// <summary>void list_repeat();</summary>
        public static RTC6Wrap.list_repeatDelegate list_repeat = (RTC6Wrap.list_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_repeatDelegate>(nameof(list_repeat));
        /// <summary>void list_until(uint Number);</summary>
        public static RTC6Wrap.list_untilDelegate list_until = (RTC6Wrap.list_untilDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_untilDelegate>(nameof(list_until));
        /// <summary>
        ///  void range_checking(uint HeadNo, uint Mode, uint Data);
        /// </summary>
        public static RTC6Wrap.range_checkingDelegate range_checking = (RTC6Wrap.range_checkingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.range_checkingDelegate>(nameof(range_checking));
        /// <summary>void set_list_jump(uint Pos);</summary>
        public static RTC6Wrap.set_list_jumpDelegate set_list_jump = (RTC6Wrap.set_list_jumpDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_list_jumpDelegate>(nameof(set_list_jump));
        /// <summary>void n_set_extstartpos_list(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_set_extstartpos_listDelegate n_set_extstartpos_list = (RTC6Wrap.n_set_extstartpos_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_extstartpos_listDelegate>(nameof(n_set_extstartpos_list));
        /// <summary>void n_set_control_mode_list(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_set_control_mode_listDelegate n_set_control_mode_list = (RTC6Wrap.n_set_control_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_control_mode_listDelegate>(nameof(n_set_control_mode_list));
        /// <summary>
        ///  void n_simulate_ext_start(uint CardNo, int Delay, uint EncoderNo);
        /// </summary>
        public static RTC6Wrap.n_simulate_ext_startDelegate n_simulate_ext_start = (RTC6Wrap.n_simulate_ext_startDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_simulate_ext_startDelegate>(nameof(n_simulate_ext_start));
        /// <summary>void set_extstartpos_list(uint Pos);</summary>
        public static RTC6Wrap.set_extstartpos_listDelegate set_extstartpos_list = (RTC6Wrap.set_extstartpos_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_extstartpos_listDelegate>(nameof(set_extstartpos_list));
        /// <summary>void set_control_mode_list(uint Mode);</summary>
        public static RTC6Wrap.set_control_mode_listDelegate set_control_mode_list = (RTC6Wrap.set_control_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_control_mode_listDelegate>(nameof(set_control_mode_list));
        /// <summary>void simulate_ext_start(int Delay, uint EncoderNo);</summary>
        public static RTC6Wrap.simulate_ext_startDelegate simulate_ext_start = (RTC6Wrap.simulate_ext_startDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.simulate_ext_startDelegate>(nameof(simulate_ext_start));
        /// <summary>void n_list_return(uint CardNo);</summary>
        public static RTC6Wrap.n_list_returnDelegate n_list_return = (RTC6Wrap.n_list_returnDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_returnDelegate>(nameof(n_list_return));
        /// <summary>
        ///  void n_list_call_repeat(uint CardNo, uint Pos, uint Number);
        /// </summary>
        public static RTC6Wrap.n_list_call_repeatDelegate n_list_call_repeat = (RTC6Wrap.n_list_call_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_call_repeatDelegate>(nameof(n_list_call_repeat));
        /// <summary>
        ///  void n_list_call_abs_repeat(uint CardNo, uint Pos, uint Number);
        /// </summary>
        public static RTC6Wrap.n_list_call_abs_repeatDelegate n_list_call_abs_repeat = (RTC6Wrap.n_list_call_abs_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_call_abs_repeatDelegate>(nameof(n_list_call_abs_repeat));
        /// <summary>void n_list_call(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_list_callDelegate n_list_call = (RTC6Wrap.n_list_callDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_callDelegate>(nameof(n_list_call));
        /// <summary>void n_list_call_abs(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_list_call_absDelegate n_list_call_abs = (RTC6Wrap.n_list_call_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_call_absDelegate>(nameof(n_list_call_abs));
        /// <summary>
        ///  void n_sub_call_repeat(uint CardNo, uint Index, uint Number);
        /// </summary>
        public static RTC6Wrap.n_sub_call_repeatDelegate n_sub_call_repeat = (RTC6Wrap.n_sub_call_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sub_call_repeatDelegate>(nameof(n_sub_call_repeat));
        /// <summary>
        ///  void n_sub_call_abs_repeat(uint CardNo, uint Index, uint Number);
        /// </summary>
        public static RTC6Wrap.n_sub_call_abs_repeatDelegate n_sub_call_abs_repeat = (RTC6Wrap.n_sub_call_abs_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sub_call_abs_repeatDelegate>(nameof(n_sub_call_abs_repeat));
        /// <summary>void n_sub_call(uint CardNo, uint Index);</summary>
        public static RTC6Wrap.n_sub_callDelegate n_sub_call = (RTC6Wrap.n_sub_callDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sub_callDelegate>(nameof(n_sub_call));
        /// <summary>void n_sub_call_abs(uint CardNo, uint Index);</summary>
        public static RTC6Wrap.n_sub_call_absDelegate n_sub_call_abs = (RTC6Wrap.n_sub_call_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sub_call_absDelegate>(nameof(n_sub_call_abs));
        /// <summary>void list_return();</summary>
        public static RTC6Wrap.list_returnDelegate list_return = (RTC6Wrap.list_returnDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_returnDelegate>(nameof(list_return));
        /// <summary>void list_call_repeat(uint Pos, uint Number);</summary>
        public static RTC6Wrap.list_call_repeatDelegate list_call_repeat = (RTC6Wrap.list_call_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_call_repeatDelegate>(nameof(list_call_repeat));
        /// <summary>void list_call_abs_repeat(uint Pos, uint Number);</summary>
        public static RTC6Wrap.list_call_abs_repeatDelegate list_call_abs_repeat = (RTC6Wrap.list_call_abs_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_call_abs_repeatDelegate>(nameof(list_call_abs_repeat));
        /// <summary>void list_call(uint Pos);</summary>
        public static RTC6Wrap.list_callDelegate list_call = (RTC6Wrap.list_callDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_callDelegate>(nameof(list_call));
        /// <summary>void list_call_abs(uint Pos);</summary>
        public static RTC6Wrap.list_call_absDelegate list_call_abs = (RTC6Wrap.list_call_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_call_absDelegate>(nameof(list_call_abs));
        /// <summary>void sub_call_repeat(uint Index, uint Number);</summary>
        public static RTC6Wrap.sub_call_repeatDelegate sub_call_repeat = (RTC6Wrap.sub_call_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sub_call_repeatDelegate>(nameof(sub_call_repeat));
        /// <summary>void sub_call_abs_repeat(uint Index, uint Number);</summary>
        public static RTC6Wrap.sub_call_abs_repeatDelegate sub_call_abs_repeat = (RTC6Wrap.sub_call_abs_repeatDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sub_call_abs_repeatDelegate>(nameof(sub_call_abs_repeat));
        /// <summary>void sub_call(uint Index);</summary>
        public static RTC6Wrap.sub_callDelegate sub_call = (RTC6Wrap.sub_callDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sub_callDelegate>(nameof(sub_call));
        /// <summary>void sub_call_abs(uint Index);</summary>
        public static RTC6Wrap.sub_call_absDelegate sub_call_abs = (RTC6Wrap.sub_call_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sub_call_absDelegate>(nameof(sub_call_abs));
        /// <summary>
        ///  void n_list_call_cond(uint CardNo, uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_list_call_condDelegate n_list_call_cond = (RTC6Wrap.n_list_call_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_call_condDelegate>(nameof(n_list_call_cond));
        /// <summary>
        ///  void n_list_call_abs_cond(uint CardNo, uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_list_call_abs_condDelegate n_list_call_abs_cond = (RTC6Wrap.n_list_call_abs_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_call_abs_condDelegate>(nameof(n_list_call_abs_cond));
        /// <summary>
        ///  void n_sub_call_cond(uint CardNo, uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_sub_call_condDelegate n_sub_call_cond = (RTC6Wrap.n_sub_call_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sub_call_condDelegate>(nameof(n_sub_call_cond));
        /// <summary>
        ///  void n_sub_call_abs_cond(uint CardNo, uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_sub_call_abs_condDelegate n_sub_call_abs_cond = (RTC6Wrap.n_sub_call_abs_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_sub_call_abs_condDelegate>(nameof(n_sub_call_abs_cond));
        /// <summary>
        ///  void n_list_jump_pos_cond(uint CardNo, uint Mask1, uint Mask0, uint Index);
        /// </summary>
        public static RTC6Wrap.n_list_jump_pos_condDelegate n_list_jump_pos_cond = (RTC6Wrap.n_list_jump_pos_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_jump_pos_condDelegate>(nameof(n_list_jump_pos_cond));
        /// <summary>
        ///  void n_list_jump_rel_cond(uint CardNo, uint Mask1, uint Mask0, int Index);
        /// </summary>
        public static RTC6Wrap.n_list_jump_rel_condDelegate n_list_jump_rel_cond = (RTC6Wrap.n_list_jump_rel_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_jump_rel_condDelegate>(nameof(n_list_jump_rel_cond));
        /// <summary>void n_if_cond(uint CardNo, uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.n_if_condDelegate n_if_cond = (RTC6Wrap.n_if_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_condDelegate>(nameof(n_if_cond));
        /// <summary>
        ///  void n_if_not_cond(uint CardNo, uint Mask1, uint Mask0);
        /// </summary>
        public static RTC6Wrap.n_if_not_condDelegate n_if_not_cond = (RTC6Wrap.n_if_not_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_not_condDelegate>(nameof(n_if_not_cond));
        /// <summary>
        ///  void n_if_pin_cond(uint CardNo, uint Mask1, uint Mask0);
        /// </summary>
        public static RTC6Wrap.n_if_pin_condDelegate n_if_pin_cond = (RTC6Wrap.n_if_pin_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_pin_condDelegate>(nameof(n_if_pin_cond));
        /// <summary>
        ///  void n_if_not_pin_cond(uint CardNo, uint Mask1, uint Mask0);
        /// </summary>
        public static RTC6Wrap.n_if_not_pin_condDelegate n_if_not_pin_cond = (RTC6Wrap.n_if_not_pin_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_not_pin_condDelegate>(nameof(n_if_not_pin_cond));
        /// <summary>
        ///  void n_switch_ioport(uint CardNo, uint MaskBits, uint ShiftBits);
        /// </summary>
        public static RTC6Wrap.n_switch_ioportDelegate n_switch_ioport = (RTC6Wrap.n_switch_ioportDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_switch_ioportDelegate>(nameof(n_switch_ioport));
        /// <summary>
        ///  void n_list_jump_cond(uint CardNo, uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.n_list_jump_condDelegate n_list_jump_cond = (RTC6Wrap.n_list_jump_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_list_jump_condDelegate>(nameof(n_list_jump_cond));
        /// <summary>void list_call_cond(uint Mask1, uint Mask0, uint Pos);</summary>
        public static RTC6Wrap.list_call_condDelegate list_call_cond = (RTC6Wrap.list_call_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_call_condDelegate>(nameof(list_call_cond));
        /// <summary>
        ///  void list_call_abs_cond(uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.list_call_abs_condDelegate list_call_abs_cond = (RTC6Wrap.list_call_abs_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_call_abs_condDelegate>(nameof(list_call_abs_cond));
        /// <summary>
        ///  void sub_call_cond(uint Mask1, uint Mask0, uint Index);
        /// </summary>
        public static RTC6Wrap.sub_call_condDelegate sub_call_cond = (RTC6Wrap.sub_call_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sub_call_condDelegate>(nameof(sub_call_cond));
        /// <summary>
        ///  void sub_call_abs_cond(uint Mask1, uint Mask0, uint Index);
        /// </summary>
        public static RTC6Wrap.sub_call_abs_condDelegate sub_call_abs_cond = (RTC6Wrap.sub_call_abs_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.sub_call_abs_condDelegate>(nameof(sub_call_abs_cond));
        /// <summary>
        ///  void list_jump_pos_cond(uint Mask1, uint Mask0, uint Pos);
        /// </summary>
        public static RTC6Wrap.list_jump_pos_condDelegate list_jump_pos_cond = (RTC6Wrap.list_jump_pos_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_jump_pos_condDelegate>(nameof(list_jump_pos_cond));
        /// <summary>
        ///  void list_jump_rel_cond(uint Mask1, uint Mask0, int Pos);
        /// </summary>
        public static RTC6Wrap.list_jump_rel_condDelegate list_jump_rel_cond = (RTC6Wrap.list_jump_rel_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_jump_rel_condDelegate>(nameof(list_jump_rel_cond));
        /// <summary>void if_cond(uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.if_condDelegate if_cond = (RTC6Wrap.if_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_condDelegate>(nameof(if_cond));
        /// <summary>void if_not_cond(uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.if_not_condDelegate if_not_cond = (RTC6Wrap.if_not_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_not_condDelegate>(nameof(if_not_cond));
        /// <summary>void if_pin_cond(uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.if_pin_condDelegate if_pin_cond = (RTC6Wrap.if_pin_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_pin_condDelegate>(nameof(if_pin_cond));
        /// <summary>void if_not_pin_cond(uint Mask1, uint Mask0);</summary>
        public static RTC6Wrap.if_not_pin_condDelegate if_not_pin_cond = (RTC6Wrap.if_not_pin_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_not_pin_condDelegate>(nameof(if_not_pin_cond));
        /// <summary>void switch_ioport(uint MaskBits, uint ShiftBits);</summary>
        public static RTC6Wrap.switch_ioportDelegate switch_ioport = (RTC6Wrap.switch_ioportDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.switch_ioportDelegate>(nameof(switch_ioport));
        /// <summary>void list_jump_cond(uint Mask1, uint Mask0, uint Pos);</summary>
        public static RTC6Wrap.list_jump_condDelegate list_jump_cond = (RTC6Wrap.list_jump_condDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.list_jump_condDelegate>(nameof(list_jump_cond));
        /// <summary>void n_select_char_set(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_select_char_setDelegate n_select_char_set = (RTC6Wrap.n_select_char_setDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_select_char_setDelegate>(nameof(n_select_char_set));
        /// <summary>void n_mark_text(uint CardNo, string Text);</summary>
        public static RTC6Wrap.n_mark_textDelegate n_mark_text = (RTC6Wrap.n_mark_textDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_textDelegate>(nameof(n_mark_text));
        /// <summary>void n_mark_text_abs(uint CardNo, string Text);</summary>
        public static RTC6Wrap.n_mark_text_absDelegate n_mark_text_abs = (RTC6Wrap.n_mark_text_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_text_absDelegate>(nameof(n_mark_text_abs));
        /// <summary>void n_mark_char(uint CardNo, uint Char);</summary>
        public static RTC6Wrap.n_mark_charDelegate n_mark_char = (RTC6Wrap.n_mark_charDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_charDelegate>(nameof(n_mark_char));
        /// <summary>void n_mark_char_abs(uint CardNo, uint Char);</summary>
        public static RTC6Wrap.n_mark_char_absDelegate n_mark_char_abs = (RTC6Wrap.n_mark_char_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_char_absDelegate>(nameof(n_mark_char_abs));
        /// <summary>void select_char_set(uint No);</summary>
        public static RTC6Wrap.select_char_setDelegate select_char_set = (RTC6Wrap.select_char_setDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.select_char_setDelegate>(nameof(select_char_set));
        /// <summary>void mark_text(string Text);</summary>
        public static RTC6Wrap.mark_textDelegate mark_text = (RTC6Wrap.mark_textDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_textDelegate>(nameof(mark_text));
        /// <summary>void mark_text_abs(string Text);</summary>
        public static RTC6Wrap.mark_text_absDelegate mark_text_abs = (RTC6Wrap.mark_text_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_text_absDelegate>(nameof(mark_text_abs));
        /// <summary>void mark_char(uint Char);</summary>
        public static RTC6Wrap.mark_charDelegate mark_char = (RTC6Wrap.mark_charDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_charDelegate>(nameof(mark_char));
        /// <summary>void mark_char_abs(uint Char);</summary>
        public static RTC6Wrap.mark_char_absDelegate mark_char_abs = (RTC6Wrap.mark_char_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_char_absDelegate>(nameof(mark_char_abs));
        /// <summary>
        ///  void n_mark_serial(uint CardNo, uint Mode, uint Digits);
        /// </summary>
        public static RTC6Wrap.n_mark_serialDelegate n_mark_serial = (RTC6Wrap.n_mark_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_serialDelegate>(nameof(n_mark_serial));
        /// <summary>
        ///  void n_mark_serial_abs(uint CardNo, uint Mode, uint Digits);
        /// </summary>
        public static RTC6Wrap.n_mark_serial_absDelegate n_mark_serial_abs = (RTC6Wrap.n_mark_serial_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_serial_absDelegate>(nameof(n_mark_serial_abs));
        /// <summary>void n_mark_date(uint CardNo, uint Part, uint Mode);</summary>
        public static RTC6Wrap.n_mark_dateDelegate n_mark_date = (RTC6Wrap.n_mark_dateDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_dateDelegate>(nameof(n_mark_date));
        /// <summary>
        ///  void n_mark_date_abs(uint CardNo, uint Part, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_mark_date_absDelegate n_mark_date_abs = (RTC6Wrap.n_mark_date_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_date_absDelegate>(nameof(n_mark_date_abs));
        /// <summary>void n_mark_time(uint CardNo, uint Part, uint Mode);</summary>
        public static RTC6Wrap.n_mark_timeDelegate n_mark_time = (RTC6Wrap.n_mark_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_timeDelegate>(nameof(n_mark_time));
        /// <summary>
        ///  void n_mark_time_abs(uint CardNo, uint Part, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_mark_time_absDelegate n_mark_time_abs = (RTC6Wrap.n_mark_time_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_time_absDelegate>(nameof(n_mark_time_abs));
        /// <summary>void n_select_serial_set_list(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_select_serial_set_listDelegate n_select_serial_set_list = (RTC6Wrap.n_select_serial_set_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_select_serial_set_listDelegate>(nameof(n_select_serial_set_list));
        /// <summary>
        ///  void n_set_serial_step_list(uint CardNo, uint No, uint Step);
        /// </summary>
        public static RTC6Wrap.n_set_serial_step_listDelegate n_set_serial_step_list = (RTC6Wrap.n_set_serial_step_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_serial_step_listDelegate>(nameof(n_set_serial_step_list));
        /// <summary>
        ///  void n_time_fix_f_off(uint CardNo, uint FirstDay, uint Offset);
        /// </summary>
        public static RTC6Wrap.n_time_fix_f_offDelegate n_time_fix_f_off = (RTC6Wrap.n_time_fix_f_offDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_time_fix_f_offDelegate>(nameof(n_time_fix_f_off));
        /// <summary>void n_time_fix_f(uint CardNo, uint FirstDay);</summary>
        public static RTC6Wrap.n_time_fix_fDelegate n_time_fix_f = (RTC6Wrap.n_time_fix_fDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_time_fix_fDelegate>(nameof(n_time_fix_f));
        /// <summary>void n_time_fix(uint CardNo);</summary>
        public static RTC6Wrap.n_time_fixDelegate n_time_fix = (RTC6Wrap.n_time_fixDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_time_fixDelegate>(nameof(n_time_fix));
        /// <summary>void mark_serial(uint Mode, uint Digits);</summary>
        public static RTC6Wrap.mark_serialDelegate mark_serial = (RTC6Wrap.mark_serialDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_serialDelegate>(nameof(mark_serial));
        /// <summary>void mark_serial_abs(uint Mode, uint Digits);</summary>
        public static RTC6Wrap.mark_serial_absDelegate mark_serial_abs = (RTC6Wrap.mark_serial_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_serial_absDelegate>(nameof(mark_serial_abs));
        /// <summary>void mark_date(uint Part, uint Mode);</summary>
        public static RTC6Wrap.mark_dateDelegate mark_date = (RTC6Wrap.mark_dateDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_dateDelegate>(nameof(mark_date));
        /// <summary>void mark_date_abs(uint Part, uint Mode);</summary>
        public static RTC6Wrap.mark_date_absDelegate mark_date_abs = (RTC6Wrap.mark_date_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_date_absDelegate>(nameof(mark_date_abs));
        /// <summary>void mark_time(uint Part, uint Mode);</summary>
        public static RTC6Wrap.mark_timeDelegate mark_time = (RTC6Wrap.mark_timeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_timeDelegate>(nameof(mark_time));
        /// <summary>void mark_time_abs(uint Part, uint Mode);</summary>
        public static RTC6Wrap.mark_time_absDelegate mark_time_abs = (RTC6Wrap.mark_time_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_time_absDelegate>(nameof(mark_time_abs));
        /// <summary>void time_fix_f_off(uint FirstDay, uint Offset);</summary>
        public static RTC6Wrap.time_fix_f_offDelegate time_fix_f_off = (RTC6Wrap.time_fix_f_offDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.time_fix_f_offDelegate>(nameof(time_fix_f_off));
        /// <summary>void select_serial_set_list(uint No);</summary>
        public static RTC6Wrap.select_serial_set_listDelegate select_serial_set_list = (RTC6Wrap.select_serial_set_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.select_serial_set_listDelegate>(nameof(select_serial_set_list));
        /// <summary>void set_serial_step_list(uint No, uint Step);</summary>
        public static RTC6Wrap.set_serial_step_listDelegate set_serial_step_list = (RTC6Wrap.set_serial_step_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_serial_step_listDelegate>(nameof(set_serial_step_list));
        /// <summary>void time_fix_f(uint FirstDay);</summary>
        public static RTC6Wrap.time_fix_fDelegate time_fix_f = (RTC6Wrap.time_fix_fDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.time_fix_fDelegate>(nameof(time_fix_f));
        /// <summary>void time_fix();</summary>
        public static RTC6Wrap.time_fixDelegate time_fix = (RTC6Wrap.time_fixDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.time_fixDelegate>(nameof(time_fix));
        /// <summary>
        ///  void n_clear_io_cond_list(uint CardNo, uint Mask1, uint Mask0, uint Mask);
        /// </summary>
        public static RTC6Wrap.n_clear_io_cond_listDelegate n_clear_io_cond_list = (RTC6Wrap.n_clear_io_cond_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_clear_io_cond_listDelegate>(nameof(n_clear_io_cond_list));
        /// <summary>
        ///  void n_set_io_cond_list(uint CardNo, uint Mask1, uint Mask0, uint Mask);
        /// </summary>
        public static RTC6Wrap.n_set_io_cond_listDelegate n_set_io_cond_list = (RTC6Wrap.n_set_io_cond_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_io_cond_listDelegate>(nameof(n_set_io_cond_list));
        /// <summary>
        ///  void n_write_io_port_mask_list(uint CardNo, uint Value, uint Mask);
        /// </summary>
        public static RTC6Wrap.n_write_io_port_mask_listDelegate n_write_io_port_mask_list = (RTC6Wrap.n_write_io_port_mask_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_io_port_mask_listDelegate>(nameof(n_write_io_port_mask_list));
        /// <summary>void n_write_8bit_port_list(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_8bit_port_listDelegate n_write_8bit_port_list = (RTC6Wrap.n_write_8bit_port_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_8bit_port_listDelegate>(nameof(n_write_8bit_port_list));
        /// <summary>void n_read_io_port_list(uint CardNo);</summary>
        public static RTC6Wrap.n_read_io_port_listDelegate n_read_io_port_list = (RTC6Wrap.n_read_io_port_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_read_io_port_listDelegate>(nameof(n_read_io_port_list));
        /// <summary>
        ///  void n_write_da_x_list(uint CardNo, uint x, uint Value);
        /// </summary>
        public static RTC6Wrap.n_write_da_x_listDelegate n_write_da_x_list = (RTC6Wrap.n_write_da_x_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_da_x_listDelegate>(nameof(n_write_da_x_list));
        /// <summary>void n_write_io_port_list(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_io_port_listDelegate n_write_io_port_list = (RTC6Wrap.n_write_io_port_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_io_port_listDelegate>(nameof(n_write_io_port_list));
        /// <summary>void n_write_da_1_list(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_da_1_listDelegate n_write_da_1_list = (RTC6Wrap.n_write_da_1_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_da_1_listDelegate>(nameof(n_write_da_1_list));
        /// <summary>void n_write_da_2_list(uint CardNo, uint Value);</summary>
        public static RTC6Wrap.n_write_da_2_listDelegate n_write_da_2_list = (RTC6Wrap.n_write_da_2_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_write_da_2_listDelegate>(nameof(n_write_da_2_list));
        /// <summary>
        ///  void clear_io_cond_list(uint Mask1, uint Mask0, uint MaskClear);
        /// </summary>
        public static RTC6Wrap.clear_io_cond_listDelegate clear_io_cond_list = (RTC6Wrap.clear_io_cond_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.clear_io_cond_listDelegate>(nameof(clear_io_cond_list));
        /// <summary>
        ///  void set_io_cond_list(uint Mask1, uint Mask0, uint MaskSet);
        /// </summary>
        public static RTC6Wrap.set_io_cond_listDelegate set_io_cond_list = (RTC6Wrap.set_io_cond_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_io_cond_listDelegate>(nameof(set_io_cond_list));
        /// <summary>void write_io_port_mask_list(uint Value, uint Mask);</summary>
        public static RTC6Wrap.write_io_port_mask_listDelegate write_io_port_mask_list = (RTC6Wrap.write_io_port_mask_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_io_port_mask_listDelegate>(nameof(write_io_port_mask_list));
        /// <summary>void write_8bit_port_list(uint Value);</summary>
        public static RTC6Wrap.write_8bit_port_listDelegate write_8bit_port_list = (RTC6Wrap.write_8bit_port_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_8bit_port_listDelegate>(nameof(write_8bit_port_list));
        /// <summary>void read_io_port_list();</summary>
        public static RTC6Wrap.read_io_port_listDelegate read_io_port_list = (RTC6Wrap.read_io_port_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.read_io_port_listDelegate>(nameof(read_io_port_list));
        /// <summary>void write_da_x_list(uint x, uint Value);</summary>
        public static RTC6Wrap.write_da_x_listDelegate write_da_x_list = (RTC6Wrap.write_da_x_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_da_x_listDelegate>(nameof(write_da_x_list));
        /// <summary>void write_io_port_list(uint Value);</summary>
        public static RTC6Wrap.write_io_port_listDelegate write_io_port_list = (RTC6Wrap.write_io_port_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_io_port_listDelegate>(nameof(write_io_port_list));
        /// <summary>void write_da_1_list(uint Value);</summary>
        public static RTC6Wrap.write_da_1_listDelegate write_da_1_list = (RTC6Wrap.write_da_1_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_da_1_listDelegate>(nameof(write_da_1_list));
        /// <summary>void write_da_2_list(uint Value);</summary>
        public static RTC6Wrap.write_da_2_listDelegate write_da_2_list = (RTC6Wrap.write_da_2_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.write_da_2_listDelegate>(nameof(write_da_2_list));
        /// <summary>void n_laser_signal_on_list(uint CardNo);</summary>
        public static RTC6Wrap.n_laser_signal_on_listDelegate n_laser_signal_on_list = (RTC6Wrap.n_laser_signal_on_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_laser_signal_on_listDelegate>(nameof(n_laser_signal_on_list));
        /// <summary>void n_laser_signal_off_list(uint CardNo);</summary>
        public static RTC6Wrap.n_laser_signal_off_listDelegate n_laser_signal_off_list = (RTC6Wrap.n_laser_signal_off_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_laser_signal_off_listDelegate>(nameof(n_laser_signal_off_list));
        /// <summary>
        ///  void n_para_laser_on_pulses_list(uint CardNo, uint Period, uint Pulses, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_laser_on_pulses_listDelegate n_para_laser_on_pulses_list = (RTC6Wrap.n_para_laser_on_pulses_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_laser_on_pulses_listDelegate>(nameof(n_para_laser_on_pulses_list));
        /// <summary>
        ///  void n_laser_on_pulses_list(uint CardNo, uint Period, uint Pulses);
        /// </summary>
        public static RTC6Wrap.n_laser_on_pulses_listDelegate n_laser_on_pulses_list = (RTC6Wrap.n_laser_on_pulses_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_laser_on_pulses_listDelegate>(nameof(n_laser_on_pulses_list));
        /// <summary>void n_laser_on_list(uint CardNo, uint Period);</summary>
        public static RTC6Wrap.n_laser_on_listDelegate n_laser_on_list = (RTC6Wrap.n_laser_on_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_laser_on_listDelegate>(nameof(n_laser_on_list));
        /// <summary>
        ///  void n_set_laser_delays(uint CardNo, int LaserOnDelay, uint LaserOffDelay);
        /// </summary>
        public static RTC6Wrap.n_set_laser_delaysDelegate n_set_laser_delays = (RTC6Wrap.n_set_laser_delaysDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_delaysDelegate>(nameof(n_set_laser_delays));
        /// <summary>
        ///  void n_set_standby_list(uint CardNo, uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_set_standby_listDelegate n_set_standby_list = (RTC6Wrap.n_set_standby_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_standby_listDelegate>(nameof(n_set_standby_list));
        /// <summary>
        ///  void n_set_laser_pulses(uint CardNo, uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_set_laser_pulsesDelegate n_set_laser_pulses = (RTC6Wrap.n_set_laser_pulsesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_pulsesDelegate>(nameof(n_set_laser_pulses));
        /// <summary>
        ///  void n_set_firstpulse_killer_list(uint CardNo, uint Length);
        /// </summary>
        public static RTC6Wrap.n_set_firstpulse_killer_listDelegate n_set_firstpulse_killer_list = (RTC6Wrap.n_set_firstpulse_killer_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_firstpulse_killer_listDelegate>(nameof(n_set_firstpulse_killer_list));
        /// <summary>
        ///  void n_set_qswitch_delay_list(uint CardNo, uint Delay);
        /// </summary>
        public static RTC6Wrap.n_set_qswitch_delay_listDelegate n_set_qswitch_delay_list = (RTC6Wrap.n_set_qswitch_delay_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_qswitch_delay_listDelegate>(nameof(n_set_qswitch_delay_list));
        /// <summary>void n_set_laser_pin_out_list(uint CardNo, uint Pins);</summary>
        public static RTC6Wrap.n_set_laser_pin_out_listDelegate n_set_laser_pin_out_list = (RTC6Wrap.n_set_laser_pin_out_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_pin_out_listDelegate>(nameof(n_set_laser_pin_out_list));
        /// <summary>
        ///  void n_set_vector_control(uint CardNo, uint Ctrl, uint Value);
        /// </summary>
        public static RTC6Wrap.n_set_vector_controlDelegate n_set_vector_control = (RTC6Wrap.n_set_vector_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_vector_controlDelegate>(nameof(n_set_vector_control));
        /// <summary>
        ///  void n_set_default_pixel_list(uint CardNo, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.n_set_default_pixel_listDelegate n_set_default_pixel_list = (RTC6Wrap.n_set_default_pixel_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_default_pixel_listDelegate>(nameof(n_set_default_pixel_list));
        /// <summary>
        ///  void n_set_auto_laser_params_list(uint CardNo, uint Ctrl, uint Value, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.n_set_auto_laser_params_listDelegate n_set_auto_laser_params_list = (RTC6Wrap.n_set_auto_laser_params_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_auto_laser_params_listDelegate>(nameof(n_set_auto_laser_params_list));
        /// <summary>void n_set_pulse_picking_list(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_set_pulse_picking_listDelegate n_set_pulse_picking_list = (RTC6Wrap.n_set_pulse_picking_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pulse_picking_listDelegate>(nameof(n_set_pulse_picking_list));
        /// <summary>
        ///  void n_set_softstart_level_list(uint CardNo, uint Index, uint Level1, uint Level2, uint Level3);
        /// </summary>
        public static RTC6Wrap.n_set_softstart_level_listDelegate n_set_softstart_level_list = (RTC6Wrap.n_set_softstart_level_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_softstart_level_listDelegate>(nameof(n_set_softstart_level_list));
        /// <summary>
        ///  void n_set_softstart_mode_list(uint CardNo, uint Mode, uint Number, uint Delay);
        /// </summary>
        public static RTC6Wrap.n_set_softstart_mode_listDelegate n_set_softstart_mode_list = (RTC6Wrap.n_set_softstart_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_softstart_mode_listDelegate>(nameof(n_set_softstart_mode_list));
        /// <summary>
        ///  void n_config_laser_signals_list(uint CardNo, uint Config);
        /// </summary>
        public static RTC6Wrap.n_config_laser_signals_listDelegate n_config_laser_signals_list = (RTC6Wrap.n_config_laser_signals_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_config_laser_signals_listDelegate>(nameof(n_config_laser_signals_list));
        /// <summary>void n_spot_distance(uint CardNo, double Dist);</summary>
        public static RTC6Wrap.n_spot_distanceDelegate n_spot_distance = (RTC6Wrap.n_spot_distanceDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_spot_distanceDelegate>(nameof(n_spot_distance));
        /// <summary>
        ///  void n_set_laser_timing(uint CardNo, uint HalfPeriod, uint PulseLength1, uint PulseLength2, uint TimeBase);
        /// </summary>
        public static RTC6Wrap.n_set_laser_timingDelegate n_set_laser_timing = (RTC6Wrap.n_set_laser_timingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_laser_timingDelegate>(nameof(n_set_laser_timing));
        /// <summary>void laser_signal_on_list();</summary>
        public static RTC6Wrap.laser_signal_on_listDelegate laser_signal_on_list = (RTC6Wrap.laser_signal_on_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.laser_signal_on_listDelegate>(nameof(laser_signal_on_list));
        /// <summary>void laser_signal_off_list();</summary>
        public static RTC6Wrap.laser_signal_off_listDelegate laser_signal_off_list = (RTC6Wrap.laser_signal_off_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.laser_signal_off_listDelegate>(nameof(laser_signal_off_list));
        /// <summary>
        ///  void para_laser_on_pulses_list(uint Period, uint Pulses, uint P);
        /// </summary>
        public static RTC6Wrap.para_laser_on_pulses_listDelegate para_laser_on_pulses_list = (RTC6Wrap.para_laser_on_pulses_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_laser_on_pulses_listDelegate>(nameof(para_laser_on_pulses_list));
        /// <summary>void laser_on_pulses_list(uint Period, uint Pulses);</summary>
        public static RTC6Wrap.laser_on_pulses_listDelegate laser_on_pulses_list = (RTC6Wrap.laser_on_pulses_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.laser_on_pulses_listDelegate>(nameof(laser_on_pulses_list));
        /// <summary>void laser_on_list(uint Period);</summary>
        public static RTC6Wrap.laser_on_listDelegate laser_on_list = (RTC6Wrap.laser_on_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.laser_on_listDelegate>(nameof(laser_on_list));
        /// <summary>
        ///  void set_laser_delays(int LaserOnDelay, uint LaserOffDelay);
        /// </summary>
        public static RTC6Wrap.set_laser_delaysDelegate set_laser_delays = (RTC6Wrap.set_laser_delaysDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_delaysDelegate>(nameof(set_laser_delays));
        /// <summary>
        ///  void set_standby_list(uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.set_standby_listDelegate set_standby_list = (RTC6Wrap.set_standby_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_standby_listDelegate>(nameof(set_standby_list));
        /// <summary>
        ///  void set_laser_pulses(uint HalfPeriod, uint PulseLength);
        /// </summary>
        public static RTC6Wrap.set_laser_pulsesDelegate set_laser_pulses = (RTC6Wrap.set_laser_pulsesDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_pulsesDelegate>(nameof(set_laser_pulses));
        /// <summary>void set_firstpulse_killer_list(uint Length);</summary>
        public static RTC6Wrap.set_firstpulse_killer_listDelegate set_firstpulse_killer_list = (RTC6Wrap.set_firstpulse_killer_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_firstpulse_killer_listDelegate>(nameof(set_firstpulse_killer_list));
        /// <summary>void set_qswitch_delay_list(uint Delay);</summary>
        public static RTC6Wrap.set_qswitch_delay_listDelegate set_qswitch_delay_list = (RTC6Wrap.set_qswitch_delay_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_qswitch_delay_listDelegate>(nameof(set_qswitch_delay_list));
        /// <summary>void set_laser_pin_out_list(uint Pins);</summary>
        public static RTC6Wrap.set_laser_pin_out_listDelegate set_laser_pin_out_list = (RTC6Wrap.set_laser_pin_out_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_pin_out_listDelegate>(nameof(set_laser_pin_out_list));
        /// <summary>void set_vector_control(uint Ctrl, uint Value);</summary>
        public static RTC6Wrap.set_vector_controlDelegate set_vector_control = (RTC6Wrap.set_vector_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_vector_controlDelegate>(nameof(set_vector_control));
        /// <summary>void set_default_pixel_list(uint PulseLength);</summary>
        public static RTC6Wrap.set_default_pixel_listDelegate set_default_pixel_list = (RTC6Wrap.set_default_pixel_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_default_pixel_listDelegate>(nameof(set_default_pixel_list));
        /// <summary>
        ///  void set_auto_laser_params_list(uint Ctrl, uint Value, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.set_auto_laser_params_listDelegate set_auto_laser_params_list = (RTC6Wrap.set_auto_laser_params_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_auto_laser_params_listDelegate>(nameof(set_auto_laser_params_list));
        /// <summary>void set_pulse_picking_list(uint No);</summary>
        public static RTC6Wrap.set_pulse_picking_listDelegate set_pulse_picking_list = (RTC6Wrap.set_pulse_picking_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pulse_picking_listDelegate>(nameof(set_pulse_picking_list));
        /// <summary>
        ///  void set_softstart_level_list(uint Index, uint Level1, uint Level2, uint Level3);
        /// </summary>
        public static RTC6Wrap.set_softstart_level_listDelegate set_softstart_level_list = (RTC6Wrap.set_softstart_level_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_softstart_level_listDelegate>(nameof(set_softstart_level_list));
        /// <summary>
        ///  void set_softstart_mode_list(uint Mode, uint Number, uint Delay);
        /// </summary>
        public static RTC6Wrap.set_softstart_mode_listDelegate set_softstart_mode_list = (RTC6Wrap.set_softstart_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_softstart_mode_listDelegate>(nameof(set_softstart_mode_list));
        /// <summary>void config_laser_signals_list(uint Config);</summary>
        public static RTC6Wrap.config_laser_signals_listDelegate config_laser_signals_list = (RTC6Wrap.config_laser_signals_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.config_laser_signals_listDelegate>(nameof(config_laser_signals_list));
        /// <summary>void spot_distance(double Dist);</summary>
        public static RTC6Wrap.spot_distanceDelegate spot_distance = (RTC6Wrap.spot_distanceDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.spot_distanceDelegate>(nameof(spot_distance));
        /// <summary>
        ///  void set_laser_timing(uint HalfPeriod, uint PulseLength1, uint PulseLength2, uint TimeBase);
        /// </summary>
        public static RTC6Wrap.set_laser_timingDelegate set_laser_timing = (RTC6Wrap.set_laser_timingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_laser_timingDelegate>(nameof(set_laser_timing));
        /// <summary>void n_fly_return_z(uint CardNo, int X, int Y, int Z);</summary>
        public static RTC6Wrap.n_fly_return_zDelegate n_fly_return_z = (RTC6Wrap.n_fly_return_zDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_fly_return_zDelegate>(nameof(n_fly_return_z));
        /// <summary>void n_fly_return(uint CardNo, int X, int Y);</summary>
        public static RTC6Wrap.n_fly_returnDelegate n_fly_return = (RTC6Wrap.n_fly_returnDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_fly_returnDelegate>(nameof(n_fly_return));
        /// <summary>void n_set_rot_center_list(uint CardNo, int X, int Y);</summary>
        public static RTC6Wrap.n_set_rot_center_listDelegate n_set_rot_center_list = (RTC6Wrap.n_set_rot_center_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_rot_center_listDelegate>(nameof(n_set_rot_center_list));
        /// <summary>
        ///  void n_set_ext_start_delay_list(uint CardNo, int Delay, uint EncoderNo);
        /// </summary>
        public static RTC6Wrap.n_set_ext_start_delay_listDelegate n_set_ext_start_delay_list = (RTC6Wrap.n_set_ext_start_delay_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_ext_start_delay_listDelegate>(nameof(n_set_ext_start_delay_list));
        /// <summary>void n_set_fly_x(uint CardNo, double ScaleX);</summary>
        public static RTC6Wrap.n_set_fly_xDelegate n_set_fly_x = (RTC6Wrap.n_set_fly_xDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_xDelegate>(nameof(n_set_fly_x));
        /// <summary>void n_set_fly_y(uint CardNo, double ScaleY);</summary>
        public static RTC6Wrap.n_set_fly_yDelegate n_set_fly_y = (RTC6Wrap.n_set_fly_yDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_yDelegate>(nameof(n_set_fly_y));
        /// <summary>
        ///  void n_set_fly_z(uint CardNo, double ScaleZ, uint EndoderNo);
        /// </summary>
        public static RTC6Wrap.n_set_fly_zDelegate n_set_fly_z = (RTC6Wrap.n_set_fly_zDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_zDelegate>(nameof(n_set_fly_z));
        /// <summary>void n_set_fly_rot(uint CardNo, double Resolution);</summary>
        public static RTC6Wrap.n_set_fly_rotDelegate n_set_fly_rot = (RTC6Wrap.n_set_fly_rotDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_rotDelegate>(nameof(n_set_fly_rot));
        /// <summary>
        ///  void n_set_fly_2d(uint CardNo, double ScaleX, double ScaleY);
        /// </summary>
        public static RTC6Wrap.n_set_fly_2dDelegate n_set_fly_2d = (RTC6Wrap.n_set_fly_2dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_2dDelegate>(nameof(n_set_fly_2d));
        /// <summary>void n_set_fly_x_pos(uint CardNo, double ScaleX);</summary>
        public static RTC6Wrap.n_set_fly_x_posDelegate n_set_fly_x_pos = (RTC6Wrap.n_set_fly_x_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_x_posDelegate>(nameof(n_set_fly_x_pos));
        /// <summary>void n_set_fly_y_pos(uint CardNo, double ScaleY);</summary>
        public static RTC6Wrap.n_set_fly_y_posDelegate n_set_fly_y_pos = (RTC6Wrap.n_set_fly_y_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_y_posDelegate>(nameof(n_set_fly_y_pos));
        /// <summary>
        ///  void n_set_fly_rot_pos(uint CardNo, double Resolution);
        /// </summary>
        public static RTC6Wrap.n_set_fly_rot_posDelegate n_set_fly_rot_pos = (RTC6Wrap.n_set_fly_rot_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_rot_posDelegate>(nameof(n_set_fly_rot_pos));
        /// <summary>
        ///  void n_set_fly_limits(uint CardNo, int Xmin, int Xmax, int Ymin, int Ymax);
        /// </summary>
        public static RTC6Wrap.n_set_fly_limitsDelegate n_set_fly_limits = (RTC6Wrap.n_set_fly_limitsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_limitsDelegate>(nameof(n_set_fly_limits));
        /// <summary>
        ///  void n_set_fly_limits_z(uint CardNo, int Zmin, int Zmax);
        /// </summary>
        public static RTC6Wrap.n_set_fly_limits_zDelegate n_set_fly_limits_z = (RTC6Wrap.n_set_fly_limits_zDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_fly_limits_zDelegate>(nameof(n_set_fly_limits_z));
        /// <summary>void n_if_fly_x_overflow(uint CardNo, int Mode);</summary>
        public static RTC6Wrap.n_if_fly_x_overflowDelegate n_if_fly_x_overflow = (RTC6Wrap.n_if_fly_x_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_fly_x_overflowDelegate>(nameof(n_if_fly_x_overflow));
        /// <summary>void n_if_fly_y_overflow(uint CardNo, int Mode);</summary>
        public static RTC6Wrap.n_if_fly_y_overflowDelegate n_if_fly_y_overflow = (RTC6Wrap.n_if_fly_y_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_fly_y_overflowDelegate>(nameof(n_if_fly_y_overflow));
        /// <summary>void n_if_fly_z_overflow(uint CardNo, int Mode);</summary>
        public static RTC6Wrap.n_if_fly_z_overflowDelegate n_if_fly_z_overflow = (RTC6Wrap.n_if_fly_z_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_fly_z_overflowDelegate>(nameof(n_if_fly_z_overflow));
        /// <summary>void n_if_not_fly_x_overflow(uint CardNo, int Mode);</summary>
        public static RTC6Wrap.n_if_not_fly_x_overflowDelegate n_if_not_fly_x_overflow = (RTC6Wrap.n_if_not_fly_x_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_not_fly_x_overflowDelegate>(nameof(n_if_not_fly_x_overflow));
        /// <summary>void n_if_not_fly_y_overflow(uint CardNo, int Mode);</summary>
        public static RTC6Wrap.n_if_not_fly_y_overflowDelegate n_if_not_fly_y_overflow = (RTC6Wrap.n_if_not_fly_y_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_not_fly_y_overflowDelegate>(nameof(n_if_not_fly_y_overflow));
        /// <summary>void n_if_not_fly_z_overflow(uint CardNo, int Mode);</summary>
        public static RTC6Wrap.n_if_not_fly_z_overflowDelegate n_if_not_fly_z_overflow = (RTC6Wrap.n_if_not_fly_z_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_not_fly_z_overflowDelegate>(nameof(n_if_not_fly_z_overflow));
        /// <summary>void n_clear_fly_overflow(uint CardNo, uint Mode);</summary>
        public static RTC6Wrap.n_clear_fly_overflowDelegate n_clear_fly_overflow = (RTC6Wrap.n_clear_fly_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_clear_fly_overflowDelegate>(nameof(n_clear_fly_overflow));
        /// <summary>void n_set_mcbsp_x_list(uint CardNo, double ScaleX);</summary>
        public static RTC6Wrap.n_set_mcbsp_x_listDelegate n_set_mcbsp_x_list = (RTC6Wrap.n_set_mcbsp_x_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_x_listDelegate>(nameof(n_set_mcbsp_x_list));
        /// <summary>void n_set_mcbsp_y_list(uint CardNo, double ScaleY);</summary>
        public static RTC6Wrap.n_set_mcbsp_y_listDelegate n_set_mcbsp_y_list = (RTC6Wrap.n_set_mcbsp_y_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_y_listDelegate>(nameof(n_set_mcbsp_y_list));
        /// <summary>
        ///  void n_set_mcbsp_rot_list(uint CardNo, double Resolution);
        /// </summary>
        public static RTC6Wrap.n_set_mcbsp_rot_listDelegate n_set_mcbsp_rot_list = (RTC6Wrap.n_set_mcbsp_rot_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_rot_listDelegate>(nameof(n_set_mcbsp_rot_list));
        /// <summary>void n_set_mcbsp_matrix_list(uint CardNo);</summary>
        public static RTC6Wrap.n_set_mcbsp_matrix_listDelegate n_set_mcbsp_matrix_list = (RTC6Wrap.n_set_mcbsp_matrix_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_matrix_listDelegate>(nameof(n_set_mcbsp_matrix_list));
        /// <summary>
        ///  void n_set_mcbsp_in_list(uint CardNo, uint Mode, double Scale);
        /// </summary>
        public static RTC6Wrap.n_set_mcbsp_in_listDelegate n_set_mcbsp_in_list = (RTC6Wrap.n_set_mcbsp_in_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_in_listDelegate>(nameof(n_set_mcbsp_in_list));
        /// <summary>
        ///  void n_set_multi_mcbsp_in_list(uint CardNo, uint Ctrl, uint P, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_set_multi_mcbsp_in_listDelegate n_set_multi_mcbsp_in_list = (RTC6Wrap.n_set_multi_mcbsp_in_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_multi_mcbsp_in_listDelegate>(nameof(n_set_multi_mcbsp_in_list));
        /// <summary>
        ///  void n_wait_for_encoder_mode(uint CardNo, int Value, uint EncoderNo, int Mode);
        /// </summary>
        public static RTC6Wrap.n_wait_for_encoder_modeDelegate n_wait_for_encoder_mode = (RTC6Wrap.n_wait_for_encoder_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_wait_for_encoder_modeDelegate>(nameof(n_wait_for_encoder_mode));
        /// <summary>
        ///  void n_wait_for_mcbsp(uint CardNo, uint Axis, int Value, int Mode);
        /// </summary>
        public static RTC6Wrap.n_wait_for_mcbspDelegate n_wait_for_mcbsp = (RTC6Wrap.n_wait_for_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_wait_for_mcbspDelegate>(nameof(n_wait_for_mcbsp));
        /// <summary>
        ///  void n_set_encoder_speed(uint CardNo, uint EncoderNo, double Speed, double Smooth);
        /// </summary>
        public static RTC6Wrap.n_set_encoder_speedDelegate n_set_encoder_speed = (RTC6Wrap.n_set_encoder_speedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_encoder_speedDelegate>(nameof(n_set_encoder_speed));
        /// <summary>void n_get_mcbsp_list(uint CardNo);</summary>
        public static RTC6Wrap.n_get_mcbsp_listDelegate n_get_mcbsp_list = (RTC6Wrap.n_get_mcbsp_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_get_mcbsp_listDelegate>(nameof(n_get_mcbsp_list));
        /// <summary>void n_store_encoder(uint CardNo, uint Pos);</summary>
        public static RTC6Wrap.n_store_encoderDelegate n_store_encoder = (RTC6Wrap.n_store_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_store_encoderDelegate>(nameof(n_store_encoder));
        /// <summary>
        ///  void n_wait_for_encoder_in_range_mode(uint CardNo, int EncXmin, int EncXmax, int EncYmin, int EncYmax, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_wait_for_encoder_in_range_modeDelegate n_wait_for_encoder_in_range_mode = (RTC6Wrap.n_wait_for_encoder_in_range_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_wait_for_encoder_in_range_modeDelegate>(nameof(n_wait_for_encoder_in_range_mode));
        /// <summary>
        ///  void n_wait_for_encoder_in_range(uint CardNo, int EncXmin, int EncXmax, int EncYmin, int EncYmax);
        /// </summary>
        public static RTC6Wrap.n_wait_for_encoder_in_rangeDelegate n_wait_for_encoder_in_range = (RTC6Wrap.n_wait_for_encoder_in_rangeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_wait_for_encoder_in_rangeDelegate>(nameof(n_wait_for_encoder_in_range));
        /// <summary>
        ///  void n_activate_fly_xy(uint CardNo, double ScaleX, double ScaleY);
        /// </summary>
        public static RTC6Wrap.n_activate_fly_xyDelegate n_activate_fly_xy = (RTC6Wrap.n_activate_fly_xyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_activate_fly_xyDelegate>(nameof(n_activate_fly_xy));
        /// <summary>
        ///  void n_activate_fly_2d(uint CardNo, double ScaleX, double ScaleY);
        /// </summary>
        public static RTC6Wrap.n_activate_fly_2dDelegate n_activate_fly_2d = (RTC6Wrap.n_activate_fly_2dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_activate_fly_2dDelegate>(nameof(n_activate_fly_2d));
        /// <summary>
        ///  void n_activate_fly_xy_encoder(uint CardNo, double ScaleX, double ScaleY, int EncX, int EncY);
        /// </summary>
        public static RTC6Wrap.n_activate_fly_xy_encoderDelegate n_activate_fly_xy_encoder = (RTC6Wrap.n_activate_fly_xy_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_activate_fly_xy_encoderDelegate>(nameof(n_activate_fly_xy_encoder));
        /// <summary>
        ///  void n_activate_fly_2d_encoder(uint CardNo, double ScaleX, double ScaleY, int EncX, int EncY);
        /// </summary>
        public static RTC6Wrap.n_activate_fly_2d_encoderDelegate n_activate_fly_2d_encoder = (RTC6Wrap.n_activate_fly_2d_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_activate_fly_2d_encoderDelegate>(nameof(n_activate_fly_2d_encoder));
        /// <summary>void n_if_not_activated(uint CardNo);</summary>
        public static RTC6Wrap.n_if_not_activatedDelegate n_if_not_activated = (RTC6Wrap.n_if_not_activatedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_if_not_activatedDelegate>(nameof(n_if_not_activated));
        /// <summary>
        ///  void n_park_position(uint CardNo, uint Mode, int X, int Y);
        /// </summary>
        public static RTC6Wrap.n_park_positionDelegate n_park_position = (RTC6Wrap.n_park_positionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_park_positionDelegate>(nameof(n_park_position));
        /// <summary>
        ///  void n_park_return(uint CardNo, uint Mode, int X, int Y);
        /// </summary>
        public static RTC6Wrap.n_park_returnDelegate n_park_return = (RTC6Wrap.n_park_returnDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_park_returnDelegate>(nameof(n_park_return));
        /// <summary>
        ///  void n_fly_prediction(uint CardNo, uint PredictionX, uint PredictionY);
        /// </summary>
        public static RTC6Wrap.n_fly_predictionDelegate n_fly_prediction = (RTC6Wrap.n_fly_predictionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_fly_predictionDelegate>(nameof(n_fly_prediction));
        /// <summary>
        ///  void n_wait_for_encoder(uint CardNo, int Value, uint EncoderNo);
        /// </summary>
        public static RTC6Wrap.n_wait_for_encoderDelegate n_wait_for_encoder = (RTC6Wrap.n_wait_for_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_wait_for_encoderDelegate>(nameof(n_wait_for_encoder));
        /// <summary>void fly_return_z(int X, int Y, int Z);</summary>
        public static RTC6Wrap.fly_return_zDelegate fly_return_z = (RTC6Wrap.fly_return_zDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.fly_return_zDelegate>(nameof(fly_return_z));
        /// <summary>void fly_return(int X, int Y);</summary>
        public static RTC6Wrap.fly_returnDelegate fly_return = (RTC6Wrap.fly_returnDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.fly_returnDelegate>(nameof(fly_return));
        /// <summary>void set_rot_center_list(int X, int Y);</summary>
        public static RTC6Wrap.set_rot_center_listDelegate set_rot_center_list = (RTC6Wrap.set_rot_center_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_rot_center_listDelegate>(nameof(set_rot_center_list));
        /// <summary>
        ///  void set_ext_start_delay_list(int Delay, uint EncoderNo);
        /// </summary>
        public static RTC6Wrap.set_ext_start_delay_listDelegate set_ext_start_delay_list = (RTC6Wrap.set_ext_start_delay_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_ext_start_delay_listDelegate>(nameof(set_ext_start_delay_list));
        /// <summary>void set_fly_x(double ScaleX);</summary>
        public static RTC6Wrap.set_fly_xDelegate set_fly_x = (RTC6Wrap.set_fly_xDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_xDelegate>(nameof(set_fly_x));
        /// <summary>void set_fly_y(double ScaleY);</summary>
        public static RTC6Wrap.set_fly_yDelegate set_fly_y = (RTC6Wrap.set_fly_yDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_yDelegate>(nameof(set_fly_y));
        /// <summary>void set_fly_z(double ScaleZ, uint EncoderNo);</summary>
        public static RTC6Wrap.set_fly_zDelegate set_fly_z = (RTC6Wrap.set_fly_zDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_zDelegate>(nameof(set_fly_z));
        /// <summary>void set_fly_rot(double Resolution);</summary>
        public static RTC6Wrap.set_fly_rotDelegate set_fly_rot = (RTC6Wrap.set_fly_rotDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_rotDelegate>(nameof(set_fly_rot));
        /// <summary>void set_fly_2d(double ScaleX, double ScaleY);</summary>
        public static RTC6Wrap.set_fly_2dDelegate set_fly_2d = (RTC6Wrap.set_fly_2dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_2dDelegate>(nameof(set_fly_2d));
        /// <summary>void set_fly_x_pos(double ScaleX);</summary>
        public static RTC6Wrap.set_fly_x_posDelegate set_fly_x_pos = (RTC6Wrap.set_fly_x_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_x_posDelegate>(nameof(set_fly_x_pos));
        /// <summary>void set_fly_y_pos(double ScaleY);</summary>
        public static RTC6Wrap.set_fly_y_posDelegate set_fly_y_pos = (RTC6Wrap.set_fly_y_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_y_posDelegate>(nameof(set_fly_y_pos));
        /// <summary>void set_fly_rot_pos(double Resolution);</summary>
        public static RTC6Wrap.set_fly_rot_posDelegate set_fly_rot_pos = (RTC6Wrap.set_fly_rot_posDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_rot_posDelegate>(nameof(set_fly_rot_pos));
        /// <summary>
        ///  void set_fly_limits(int Xmin, int Xmax, int Ymin, int Ymax);
        /// </summary>
        public static RTC6Wrap.set_fly_limitsDelegate set_fly_limits = (RTC6Wrap.set_fly_limitsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_limitsDelegate>(nameof(set_fly_limits));
        /// <summary>void set_fly_limits_z(int Zmin, int Zmax);</summary>
        public static RTC6Wrap.set_fly_limits_zDelegate set_fly_limits_z = (RTC6Wrap.set_fly_limits_zDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_fly_limits_zDelegate>(nameof(set_fly_limits_z));
        /// <summary>void if_fly_x_overflow(int Mode);</summary>
        public static RTC6Wrap.if_fly_x_overflowDelegate if_fly_x_overflow = (RTC6Wrap.if_fly_x_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_fly_x_overflowDelegate>(nameof(if_fly_x_overflow));
        /// <summary>void if_fly_y_overflow(int Mode);</summary>
        public static RTC6Wrap.if_fly_y_overflowDelegate if_fly_y_overflow = (RTC6Wrap.if_fly_y_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_fly_y_overflowDelegate>(nameof(if_fly_y_overflow));
        /// <summary>void if_fly_z_overflow(int Mode);</summary>
        public static RTC6Wrap.if_fly_z_overflowDelegate if_fly_z_overflow = (RTC6Wrap.if_fly_z_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_fly_z_overflowDelegate>(nameof(if_fly_z_overflow));
        /// <summary>void if_not_fly_x_overflow(int Mode);</summary>
        public static RTC6Wrap.if_not_fly_x_overflowDelegate if_not_fly_x_overflow = (RTC6Wrap.if_not_fly_x_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_not_fly_x_overflowDelegate>(nameof(if_not_fly_x_overflow));
        /// <summary>void if_not_fly_y_overflow(int Mode);</summary>
        public static RTC6Wrap.if_not_fly_y_overflowDelegate if_not_fly_y_overflow = (RTC6Wrap.if_not_fly_y_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_not_fly_y_overflowDelegate>(nameof(if_not_fly_y_overflow));
        /// <summary>void if_not_fly_z_overflow(int Mode);</summary>
        public static RTC6Wrap.if_not_fly_z_overflowDelegate if_not_fly_z_overflow = (RTC6Wrap.if_not_fly_z_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_not_fly_z_overflowDelegate>(nameof(if_not_fly_z_overflow));
        /// <summary>void clear_fly_overflow(uint Mode);</summary>
        public static RTC6Wrap.clear_fly_overflowDelegate clear_fly_overflow = (RTC6Wrap.clear_fly_overflowDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.clear_fly_overflowDelegate>(nameof(clear_fly_overflow));
        /// <summary>void set_mcbsp_x_list(double ScaleX);</summary>
        public static RTC6Wrap.set_mcbsp_x_listDelegate set_mcbsp_x_list = (RTC6Wrap.set_mcbsp_x_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_x_listDelegate>(nameof(set_mcbsp_x_list));
        /// <summary>void set_mcbsp_y_list(double ScaleY);</summary>
        public static RTC6Wrap.set_mcbsp_y_listDelegate set_mcbsp_y_list = (RTC6Wrap.set_mcbsp_y_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_y_listDelegate>(nameof(set_mcbsp_y_list));
        /// <summary>void set_mcbsp_rot_list(double Resolution);</summary>
        public static RTC6Wrap.set_mcbsp_rot_listDelegate set_mcbsp_rot_list = (RTC6Wrap.set_mcbsp_rot_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_rot_listDelegate>(nameof(set_mcbsp_rot_list));
        /// <summary>void set_mcbsp_matrix_list();</summary>
        public static RTC6Wrap.set_mcbsp_matrix_listDelegate set_mcbsp_matrix_list = (RTC6Wrap.set_mcbsp_matrix_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_matrix_listDelegate>(nameof(set_mcbsp_matrix_list));
        /// <summary>void set_mcbsp_in_list(uint Mode, double Scale);</summary>
        public static RTC6Wrap.set_mcbsp_in_listDelegate set_mcbsp_in_list = (RTC6Wrap.set_mcbsp_in_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_in_listDelegate>(nameof(set_mcbsp_in_list));
        /// <summary>
        ///  void set_multi_mcbsp_in_list(uint Ctrl, uint P, uint Mode);
        /// </summary>
        public static RTC6Wrap.set_multi_mcbsp_in_listDelegate set_multi_mcbsp_in_list = (RTC6Wrap.set_multi_mcbsp_in_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_multi_mcbsp_in_listDelegate>(nameof(set_multi_mcbsp_in_list));
        /// <summary>
        ///  void wait_for_encoder_mode(int Value, uint EncoderNo, int Mode);
        /// </summary>
        public static RTC6Wrap.wait_for_encoder_modeDelegate wait_for_encoder_mode = (RTC6Wrap.wait_for_encoder_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.wait_for_encoder_modeDelegate>(nameof(wait_for_encoder_mode));
        /// <summary>void wait_for_mcbsp(uint Axis, int Value, int Mode);</summary>
        public static RTC6Wrap.wait_for_mcbspDelegate wait_for_mcbsp = (RTC6Wrap.wait_for_mcbspDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.wait_for_mcbspDelegate>(nameof(wait_for_mcbsp));
        /// <summary>
        ///  void set_encoder_speed(uint EncoderNo, double Speed, double Smooth);
        /// </summary>
        public static RTC6Wrap.set_encoder_speedDelegate set_encoder_speed = (RTC6Wrap.set_encoder_speedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_encoder_speedDelegate>(nameof(set_encoder_speed));
        /// <summary>void get_mcbsp_list();</summary>
        public static RTC6Wrap.get_mcbsp_listDelegate get_mcbsp_list = (RTC6Wrap.get_mcbsp_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.get_mcbsp_listDelegate>(nameof(get_mcbsp_list));
        /// <summary>void store_encoder(uint Pos);</summary>
        public static RTC6Wrap.store_encoderDelegate store_encoder = (RTC6Wrap.store_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.store_encoderDelegate>(nameof(store_encoder));
        /// <summary>
        ///  void wait_for_encoder_in_range_mode(int EncXmin, int EncXmax, int EncYmin, int EncYmax, uint Mode);
        /// </summary>
        public static RTC6Wrap.wait_for_encoder_in_range_modeDelegate wait_for_encoder_in_range_mode = (RTC6Wrap.wait_for_encoder_in_range_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.wait_for_encoder_in_range_modeDelegate>(nameof(wait_for_encoder_in_range_mode));
        /// <summary>
        ///  void wait_for_encoder_in_range(int EncXmin, int EncXmax, int EncYmin, int EncYmax);
        /// </summary>
        public static RTC6Wrap.wait_for_encoder_in_rangeDelegate wait_for_encoder_in_range = (RTC6Wrap.wait_for_encoder_in_rangeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.wait_for_encoder_in_rangeDelegate>(nameof(wait_for_encoder_in_range));
        /// <summary>void activate_fly_xy(double ScaleX, double ScaleY);</summary>
        public static RTC6Wrap.activate_fly_xyDelegate activate_fly_xy = (RTC6Wrap.activate_fly_xyDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.activate_fly_xyDelegate>(nameof(activate_fly_xy));
        /// <summary>void activate_fly_2d(double ScaleX, double ScaleY);</summary>
        public static RTC6Wrap.activate_fly_2dDelegate activate_fly_2d = (RTC6Wrap.activate_fly_2dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.activate_fly_2dDelegate>(nameof(activate_fly_2d));
        /// <summary>
        ///  void activate_fly_xy_encoder(double ScaleX, double ScaleY, int EncX, int EncY);
        /// </summary>
        public static RTC6Wrap.activate_fly_xy_encoderDelegate activate_fly_xy_encoder = (RTC6Wrap.activate_fly_xy_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.activate_fly_xy_encoderDelegate>(nameof(activate_fly_xy_encoder));
        /// <summary>
        ///  void activate_fly_2d_encoder(double ScaleX, double ScaleY, int EncX, int EncY);
        /// </summary>
        public static RTC6Wrap.activate_fly_2d_encoderDelegate activate_fly_2d_encoder = (RTC6Wrap.activate_fly_2d_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.activate_fly_2d_encoderDelegate>(nameof(activate_fly_2d_encoder));
        /// <summary>void if_not_activated();</summary>
        public static RTC6Wrap.if_not_activatedDelegate if_not_activated = (RTC6Wrap.if_not_activatedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.if_not_activatedDelegate>(nameof(if_not_activated));
        /// <summary>void park_position(uint Mode, int X, int Y);</summary>
        public static RTC6Wrap.park_positionDelegate park_position = (RTC6Wrap.park_positionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.park_positionDelegate>(nameof(park_position));
        /// <summary>void park_return(uint Mode, int X, int Y);</summary>
        public static RTC6Wrap.park_returnDelegate park_return = (RTC6Wrap.park_returnDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.park_returnDelegate>(nameof(park_return));
        /// <summary>
        ///  void fly_prediction(uint PredictionX, uint PredictionY);
        /// </summary>
        public static RTC6Wrap.fly_predictionDelegate fly_prediction = (RTC6Wrap.fly_predictionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.fly_predictionDelegate>(nameof(fly_prediction));
        /// <summary>void wait_for_encoder(int Value, uint EncoderNo);</summary>
        public static RTC6Wrap.wait_for_encoderDelegate wait_for_encoder = (RTC6Wrap.wait_for_encoderDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.wait_for_encoderDelegate>(nameof(wait_for_encoder));
        /// <summary>void n_save_and_restart_timer(uint CardNo);</summary>
        public static RTC6Wrap.n_save_and_restart_timerDelegate n_save_and_restart_timer = (RTC6Wrap.n_save_and_restart_timerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_save_and_restart_timerDelegate>(nameof(n_save_and_restart_timer));
        /// <summary>
        ///  void n_set_wobbel_mode_phase(uint CardNo, uint Transversal, uint Longitudinal, double Freq, int Mode, double Phase);
        /// </summary>
        public static RTC6Wrap.n_set_wobbel_mode_phaseDelegate n_set_wobbel_mode_phase = (RTC6Wrap.n_set_wobbel_mode_phaseDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbel_mode_phaseDelegate>(nameof(n_set_wobbel_mode_phase));
        /// <summary>
        ///  void n_set_wobbel_mode(uint CardNo, uint Transversal, uint Longitudinal, double Freq, int Mode);
        /// </summary>
        public static RTC6Wrap.n_set_wobbel_modeDelegate n_set_wobbel_mode = (RTC6Wrap.n_set_wobbel_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbel_modeDelegate>(nameof(n_set_wobbel_mode));
        /// <summary>
        ///  void n_set_wobbel(uint CardNo, uint Transversal, uint Longitudinal, double Freq);
        /// </summary>
        public static RTC6Wrap.n_set_wobbelDelegate n_set_wobbel = (RTC6Wrap.n_set_wobbelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbelDelegate>(nameof(n_set_wobbel));
        /// <summary>
        ///  void n_set_wobbel_direction(uint CardNo, int dX, int dY);
        /// </summary>
        public static RTC6Wrap.n_set_wobbel_directionDelegate n_set_wobbel_direction = (RTC6Wrap.n_set_wobbel_directionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbel_directionDelegate>(nameof(n_set_wobbel_direction));
        /// <summary>
        ///  void n_set_wobbel_control(uint CardNo, uint Ctrl, uint Value, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.n_set_wobbel_controlDelegate n_set_wobbel_control = (RTC6Wrap.n_set_wobbel_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbel_controlDelegate>(nameof(n_set_wobbel_control));
        /// <summary>
        ///  void n_set_wobbel_vector(uint CardNo, double dTrans, double dLong, uint Period, double dPower);
        /// </summary>
        public static RTC6Wrap.n_set_wobbel_vectorDelegate n_set_wobbel_vector = (RTC6Wrap.n_set_wobbel_vectorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbel_vectorDelegate>(nameof(n_set_wobbel_vector));
        /// <summary>
        ///  void n_set_wobbel_offset(uint CardNo, int OffsetTrans, int OffsetLong);
        /// </summary>
        public static RTC6Wrap.n_set_wobbel_offsetDelegate n_set_wobbel_offset = (RTC6Wrap.n_set_wobbel_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_wobbel_offsetDelegate>(nameof(n_set_wobbel_offset));
        /// <summary>
        ///  void n_set_trigger(uint CardNo, uint Period, uint Signal1, uint Signal2);
        /// </summary>
        public static RTC6Wrap.n_set_triggerDelegate n_set_trigger = (RTC6Wrap.n_set_triggerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_triggerDelegate>(nameof(n_set_trigger));
        /// <summary>
        ///  void n_set_trigger4(uint CardNo, uint Period, uint Signal1, uint Signal2, uint Signal3, uint Signal4);
        /// </summary>
        public static RTC6Wrap.n_set_trigger4Delegate n_set_trigger4 = (RTC6Wrap.n_set_trigger4Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_trigger4Delegate>(nameof(n_set_trigger4));
        /// <summary>
        ///  void n_set_pixel_line_3d(uint CardNo, uint Channel, uint HalfPeriod, double dX, double dY, double dZ);
        /// </summary>
        public static RTC6Wrap.n_set_pixel_line_3dDelegate n_set_pixel_line_3d = (RTC6Wrap.n_set_pixel_line_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pixel_line_3dDelegate>(nameof(n_set_pixel_line_3d));
        /// <summary>
        ///  void n_set_pixel_line(uint CardNo, uint Channel, uint HalfPeriod, double dX, double dY);
        /// </summary>
        public static RTC6Wrap.n_set_pixel_lineDelegate n_set_pixel_line = (RTC6Wrap.n_set_pixel_lineDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pixel_lineDelegate>(nameof(n_set_pixel_line));
        /// <summary>
        ///  void n_set_n_pixel(uint CardNo, uint PortOutValue1, uint PortOutValue2, uint Number);
        /// </summary>
        public static RTC6Wrap.n_set_n_pixelDelegate n_set_n_pixel = (RTC6Wrap.n_set_n_pixelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_n_pixelDelegate>(nameof(n_set_n_pixel));
        /// <summary>
        ///  void n_set_pixel(uint CardNo, uint PortOutValue1, uint PortOutValue2);
        /// </summary>
        public static RTC6Wrap.n_set_pixelDelegate n_set_pixel = (RTC6Wrap.n_set_pixelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_pixelDelegate>(nameof(n_set_pixel));
        /// <summary>
        ///  void n_rs232_write_text_list(uint CardNo, string pData);
        /// </summary>
        public static RTC6Wrap.n_rs232_write_text_listDelegate n_rs232_write_text_list = (RTC6Wrap.n_rs232_write_text_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_rs232_write_text_listDelegate>(nameof(n_rs232_write_text_list));
        /// <summary>
        ///  void n_set_mcbsp_out(uint CardNo, uint Signal1, uint Signal2);
        /// </summary>
        public static RTC6Wrap.n_set_mcbsp_outDelegate n_set_mcbsp_out = (RTC6Wrap.n_set_mcbsp_outDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mcbsp_outDelegate>(nameof(n_set_mcbsp_out));
        /// <summary>
        ///  void n_camming(uint CardNo, uint FirstPos, uint NPos, uint No, uint Ctrl, double Scale, uint Code);
        /// </summary>
        public static RTC6Wrap.n_cammingDelegate n_camming = (RTC6Wrap.n_cammingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_cammingDelegate>(nameof(n_camming));
        /// <summary>
        ///  void n_periodic_toggle_list(uint CardNo, uint Port, uint Mask, uint P1, uint P2, uint Count, uint Start);
        /// </summary>
        public static RTC6Wrap.n_periodic_toggle_listDelegate n_periodic_toggle_list = (RTC6Wrap.n_periodic_toggle_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_periodic_toggle_listDelegate>(nameof(n_periodic_toggle_list));
        /// <summary>
        ///  void n_micro_vector_abs_3d(uint CardNo, int X, int Y, int Z, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_abs_3dDelegate n_micro_vector_abs_3d = (RTC6Wrap.n_micro_vector_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_abs_3dDelegate>(nameof(n_micro_vector_abs_3d));
        /// <summary>
        ///  void n_micro_vector_rel_3d(uint CardNo, int dX, int dY, int dZ, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_rel_3dDelegate n_micro_vector_rel_3d = (RTC6Wrap.n_micro_vector_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_rel_3dDelegate>(nameof(n_micro_vector_rel_3d));
        /// <summary>
        ///  void n_micro_vector_abs(uint CardNo, int X, int Y, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_absDelegate n_micro_vector_abs = (RTC6Wrap.n_micro_vector_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_absDelegate>(nameof(n_micro_vector_abs));
        /// <summary>
        ///  void n_micro_vector_rel(uint CardNo, int dX, int dY, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_relDelegate n_micro_vector_rel = (RTC6Wrap.n_micro_vector_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_relDelegate>(nameof(n_micro_vector_rel));
        /// <summary>
        ///  void n_micro_vector_quad_axis_v_2(uint CardNo, int X0, int Y0, int X1, int Y1, int LasOn, int LasOff, uint Power, uint Port, uint Flags, double Velocity);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_quad_axis_v_2Delegate n_micro_vector_quad_axis_v_2 = (RTC6Wrap.n_micro_vector_quad_axis_v_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_quad_axis_v_2Delegate>(nameof(n_micro_vector_quad_axis_v_2));
        /// <summary>
        ///  void n_micro_vector_quad_axis_v(uint CardNo, int X0, int Y0, double X1, double Y1, int LasOn, int LasOff, uint Power, uint Port, uint Flags, double Velocity);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_quad_axis_vDelegate n_micro_vector_quad_axis_v = (RTC6Wrap.n_micro_vector_quad_axis_vDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_quad_axis_vDelegate>(nameof(n_micro_vector_quad_axis_v));
        /// <summary>
        ///  void n_micro_vector_quad_axis(uint CardNo, int X0, int Y0, double X1, double Y1, int LasOn, int LasOff, uint Power, uint Port, uint Flags);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_quad_axisDelegate n_micro_vector_quad_axis = (RTC6Wrap.n_micro_vector_quad_axisDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_quad_axisDelegate>(nameof(n_micro_vector_quad_axis));
        /// <summary>
        ///  void n_micro_vector_set_position(uint CardNo, int X0, int X1, int X2, int X3, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.n_micro_vector_set_positionDelegate n_micro_vector_set_position = (RTC6Wrap.n_micro_vector_set_positionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_micro_vector_set_positionDelegate>(nameof(n_micro_vector_set_position));
        /// <summary>void n_multi_axis_flags(uint CardNo, uint Flags);</summary>
        public static RTC6Wrap.n_multi_axis_flagsDelegate n_multi_axis_flags = (RTC6Wrap.n_multi_axis_flagsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_multi_axis_flagsDelegate>(nameof(n_multi_axis_flags));
        /// <summary>
        ///  void n_set_free_variable_list(uint CardNo, uint VarNo, uint Value);
        /// </summary>
        public static RTC6Wrap.n_set_free_variable_listDelegate n_set_free_variable_list = (RTC6Wrap.n_set_free_variable_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_free_variable_listDelegate>(nameof(n_set_free_variable_list));
        /// <summary>
        ///  void n_jump_abs_drill_2(uint CardNo, int X, int Y, uint DrillTime, int XOff, int YOff);
        /// </summary>
        public static RTC6Wrap.n_jump_abs_drill_2Delegate n_jump_abs_drill_2 = (RTC6Wrap.n_jump_abs_drill_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_abs_drill_2Delegate>(nameof(n_jump_abs_drill_2));
        /// <summary>
        ///  void n_jump_rel_drill_2(uint CardNo, int dX, int dY, uint DrillTime, int XOff, int YOff);
        /// </summary>
        public static RTC6Wrap.n_jump_rel_drill_2Delegate n_jump_rel_drill_2 = (RTC6Wrap.n_jump_rel_drill_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_rel_drill_2Delegate>(nameof(n_jump_rel_drill_2));
        /// <summary>
        ///  void n_jump_abs_drill(uint CardNo, int X, int Y, uint DrillTime);
        /// </summary>
        public static RTC6Wrap.n_jump_abs_drillDelegate n_jump_abs_drill = (RTC6Wrap.n_jump_abs_drillDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_abs_drillDelegate>(nameof(n_jump_abs_drill));
        /// <summary>
        ///  void n_jump_rel_drill(uint CardNo, int dX, int dY, uint DrillTime);
        /// </summary>
        public static RTC6Wrap.n_jump_rel_drillDelegate n_jump_rel_drill = (RTC6Wrap.n_jump_rel_drillDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_rel_drillDelegate>(nameof(n_jump_rel_drill));
        /// <summary>void save_and_restart_timer();</summary>
        public static RTC6Wrap.save_and_restart_timerDelegate save_and_restart_timer = (RTC6Wrap.save_and_restart_timerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.save_and_restart_timerDelegate>(nameof(save_and_restart_timer));
        /// <summary>
        ///  void set_wobbel_mode_phase(uint Transversal, uint Longitudinal, double Freq, int Mode, double Phase);
        /// </summary>
        public static RTC6Wrap.set_wobbel_mode_phaseDelegate set_wobbel_mode_phase = (RTC6Wrap.set_wobbel_mode_phaseDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbel_mode_phaseDelegate>(nameof(set_wobbel_mode_phase));
        /// <summary>
        ///  void set_wobbel_mode(uint Transversal, uint Longitudinal, double Freq, int Mode);
        /// </summary>
        public static RTC6Wrap.set_wobbel_modeDelegate set_wobbel_mode = (RTC6Wrap.set_wobbel_modeDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbel_modeDelegate>(nameof(set_wobbel_mode));
        /// <summary>
        ///  void set_wobbel(uint Transversal, uint Longitudinal, double Freq);
        /// </summary>
        public static RTC6Wrap.set_wobbelDelegate set_wobbel = (RTC6Wrap.set_wobbelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbelDelegate>(nameof(set_wobbel));
        /// <summary>void set_wobbel_direction(int dX, int dY);</summary>
        public static RTC6Wrap.set_wobbel_directionDelegate set_wobbel_direction = (RTC6Wrap.set_wobbel_directionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbel_directionDelegate>(nameof(set_wobbel_direction));
        /// <summary>
        ///  void set_wobbel_control(uint Ctrl, uint Value, uint MinValue, uint MaxValue);
        /// </summary>
        public static RTC6Wrap.set_wobbel_controlDelegate set_wobbel_control = (RTC6Wrap.set_wobbel_controlDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbel_controlDelegate>(nameof(set_wobbel_control));
        /// <summary>
        ///  void set_wobbel_vector(double dTrans, double dLong, uint Period, double dPower);
        /// </summary>
        public static RTC6Wrap.set_wobbel_vectorDelegate set_wobbel_vector = (RTC6Wrap.set_wobbel_vectorDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbel_vectorDelegate>(nameof(set_wobbel_vector));
        /// <summary>
        ///  void set_wobbel_offset(int OffsetTrans, int OffsetLong);
        /// </summary>
        public static RTC6Wrap.set_wobbel_offsetDelegate set_wobbel_offset = (RTC6Wrap.set_wobbel_offsetDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_wobbel_offsetDelegate>(nameof(set_wobbel_offset));
        /// <summary>
        ///  void set_trigger(uint Period, uint Signal1, uint Signal2);
        /// </summary>
        public static RTC6Wrap.set_triggerDelegate set_trigger = (RTC6Wrap.set_triggerDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_triggerDelegate>(nameof(set_trigger));
        /// <summary>
        ///  void set_trigger4(uint Period, uint Signal1, uint Signal2, uint Signal3, uint Signal4);
        /// </summary>
        public static RTC6Wrap.set_trigger4Delegate set_trigger4 = (RTC6Wrap.set_trigger4Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_trigger4Delegate>(nameof(set_trigger4));
        /// <summary>
        ///  void set_pixel_line_3d(uint Channel, uint HalfPeriod, double dX, double dY, double dZ);
        /// </summary>
        public static RTC6Wrap.set_pixel_line_3dDelegate set_pixel_line_3d = (RTC6Wrap.set_pixel_line_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pixel_line_3dDelegate>(nameof(set_pixel_line_3d));
        /// <summary>
        ///  void set_pixel_line(uint Channel, uint HalfPeriod, double dX, double dY);
        /// </summary>
        public static RTC6Wrap.set_pixel_lineDelegate set_pixel_line = (RTC6Wrap.set_pixel_lineDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pixel_lineDelegate>(nameof(set_pixel_line));
        /// <summary>
        ///  void set_n_pixel(uint PortOutValue1, uint PortOutValue2, uint Number);
        /// </summary>
        public static RTC6Wrap.set_n_pixelDelegate set_n_pixel = (RTC6Wrap.set_n_pixelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_n_pixelDelegate>(nameof(set_n_pixel));
        /// <summary>
        ///  void set_pixel(uint PortOutValue1, uint PortOutValue2);
        /// </summary>
        public static RTC6Wrap.set_pixelDelegate set_pixel = (RTC6Wrap.set_pixelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_pixelDelegate>(nameof(set_pixel));
        /// <summary>void rs232_write_text_list(string pData);</summary>
        public static RTC6Wrap.rs232_write_text_listDelegate rs232_write_text_list = (RTC6Wrap.rs232_write_text_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.rs232_write_text_listDelegate>(nameof(rs232_write_text_list));
        /// <summary>void set_mcbsp_out(uint Signal1, uint Signal2);</summary>
        public static RTC6Wrap.set_mcbsp_outDelegate set_mcbsp_out = (RTC6Wrap.set_mcbsp_outDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mcbsp_outDelegate>(nameof(set_mcbsp_out));
        /// <summary>
        ///  void camming(uint FirstPos, uint NPos, uint No, uint Ctrl, double Scale, uint Code);
        /// </summary>
        public static RTC6Wrap.cammingDelegate camming = (RTC6Wrap.cammingDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.cammingDelegate>(nameof(camming));
        /// <summary>
        ///  void periodic_toggle_list(uint Port, uint Mask, uint P1, uint P2, uint Count, uint Start);
        /// </summary>
        public static RTC6Wrap.periodic_toggle_listDelegate periodic_toggle_list = (RTC6Wrap.periodic_toggle_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.periodic_toggle_listDelegate>(nameof(periodic_toggle_list));
        /// <summary>
        ///  void micro_vector_abs_3d(int X, int Y, int Z, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.micro_vector_abs_3dDelegate micro_vector_abs_3d = (RTC6Wrap.micro_vector_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_abs_3dDelegate>(nameof(micro_vector_abs_3d));
        /// <summary>
        ///  void micro_vector_rel_3d(int dX, int dY, int dZ, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.micro_vector_rel_3dDelegate micro_vector_rel_3d = (RTC6Wrap.micro_vector_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_rel_3dDelegate>(nameof(micro_vector_rel_3d));
        /// <summary>
        ///  void micro_vector_abs(int X, int Y, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.micro_vector_absDelegate micro_vector_abs = (RTC6Wrap.micro_vector_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_absDelegate>(nameof(micro_vector_abs));
        /// <summary>
        ///  void micro_vector_rel(int dX, int dY, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.micro_vector_relDelegate micro_vector_rel = (RTC6Wrap.micro_vector_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_relDelegate>(nameof(micro_vector_rel));
        /// <summary>
        ///  void micro_vector_quad_axis_v_2(int X0, int Y0, int X1, int Y1, int LasOn, int LasOff, uint Power, uint Port, uint Flags, double Velocity);
        /// </summary>
        public static RTC6Wrap.micro_vector_quad_axis_v_2Delegate micro_vector_quad_axis_v_2 = (RTC6Wrap.micro_vector_quad_axis_v_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_quad_axis_v_2Delegate>(nameof(micro_vector_quad_axis_v_2));
        /// <summary>
        ///  void micro_vector_quad_axis_v(int X0, int Y0, double X1, double Y1, int LasOn, int LasOff, uint Power, uint Port, uint Flags, double Velocity);
        /// </summary>
        public static RTC6Wrap.micro_vector_quad_axis_vDelegate micro_vector_quad_axis_v = (RTC6Wrap.micro_vector_quad_axis_vDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_quad_axis_vDelegate>(nameof(micro_vector_quad_axis_v));
        /// <summary>
        ///  void micro_vector_quad_axis(int X0, int Y0, double X1, double Y1, int LasOn, int LasOff, uint Power, uint Port, uint Flags);
        /// </summary>
        public static RTC6Wrap.micro_vector_quad_axisDelegate micro_vector_quad_axis = (RTC6Wrap.micro_vector_quad_axisDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_quad_axisDelegate>(nameof(micro_vector_quad_axis));
        /// <summary>
        ///  void micro_vector_set_position(int X0, int X1, int X2, int X3, int LasOn, int LasOff);
        /// </summary>
        public static RTC6Wrap.micro_vector_set_positionDelegate micro_vector_set_position = (RTC6Wrap.micro_vector_set_positionDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.micro_vector_set_positionDelegate>(nameof(micro_vector_set_position));
        /// <summary>void multi_axis_flags(uint Flags);</summary>
        public static RTC6Wrap.multi_axis_flagsDelegate multi_axis_flags = (RTC6Wrap.multi_axis_flagsDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.multi_axis_flagsDelegate>(nameof(multi_axis_flags));
        /// <summary>void set_free_variable_list(uint VarNo, uint Value);</summary>
        public static RTC6Wrap.set_free_variable_listDelegate set_free_variable_list = (RTC6Wrap.set_free_variable_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_free_variable_listDelegate>(nameof(set_free_variable_list));
        /// <summary>
        ///  void jump_abs_drill_2(int X, int Y, uint DrillTime, int XOff, int YOff);
        /// </summary>
        public static RTC6Wrap.jump_abs_drill_2Delegate jump_abs_drill_2 = (RTC6Wrap.jump_abs_drill_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_abs_drill_2Delegate>(nameof(jump_abs_drill_2));
        /// <summary>
        ///  void jump_rel_drill_2(int dX, int dY, uint DrillTime, int XOff, int YOff);
        /// </summary>
        public static RTC6Wrap.jump_rel_drill_2Delegate jump_rel_drill_2 = (RTC6Wrap.jump_rel_drill_2Delegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_rel_drill_2Delegate>(nameof(jump_rel_drill_2));
        /// <summary>void jump_abs_drill(int X, int Y, uint DrillTime);</summary>
        public static RTC6Wrap.jump_abs_drillDelegate jump_abs_drill = (RTC6Wrap.jump_abs_drillDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_abs_drillDelegate>(nameof(jump_abs_drill));
        /// <summary>void jump_rel_drill(int dX, int dY, uint DrillTime);</summary>
        public static RTC6Wrap.jump_rel_drillDelegate jump_rel_drill = (RTC6Wrap.jump_rel_drillDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_rel_drillDelegate>(nameof(jump_rel_drill));
        /// <summary>
        ///  void n_timed_mark_abs_3d(uint CardNo, int X, int Y, int Z, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_mark_abs_3dDelegate n_timed_mark_abs_3d = (RTC6Wrap.n_timed_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_mark_abs_3dDelegate>(nameof(n_timed_mark_abs_3d));
        /// <summary>
        ///  void n_timed_mark_rel_3d(uint CardNo, int dX, int dY, int dZ, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_mark_rel_3dDelegate n_timed_mark_rel_3d = (RTC6Wrap.n_timed_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_mark_rel_3dDelegate>(nameof(n_timed_mark_rel_3d));
        /// <summary>
        ///  void n_timed_mark_abs(uint CardNo, int X, int Y, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_mark_absDelegate n_timed_mark_abs = (RTC6Wrap.n_timed_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_mark_absDelegate>(nameof(n_timed_mark_abs));
        /// <summary>
        ///  void n_timed_mark_rel(uint CardNo, int dX, int dY, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_mark_relDelegate n_timed_mark_rel = (RTC6Wrap.n_timed_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_mark_relDelegate>(nameof(n_timed_mark_rel));
        /// <summary>void timed_mark_abs_3d(int X, int Y, int Z, double T);</summary>
        public static RTC6Wrap.timed_mark_abs_3dDelegate timed_mark_abs_3d = (RTC6Wrap.timed_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_mark_abs_3dDelegate>(nameof(timed_mark_abs_3d));
        /// <summary>
        ///  void timed_mark_rel_3d(int dX, int dY, int dZ, double T);
        /// </summary>
        public static RTC6Wrap.timed_mark_rel_3dDelegate timed_mark_rel_3d = (RTC6Wrap.timed_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_mark_rel_3dDelegate>(nameof(timed_mark_rel_3d));
        /// <summary>void timed_mark_abs(int X, int Y, double T);</summary>
        public static RTC6Wrap.timed_mark_absDelegate timed_mark_abs = (RTC6Wrap.timed_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_mark_absDelegate>(nameof(timed_mark_abs));
        /// <summary>void timed_mark_rel(int dX, int dY, double T);</summary>
        public static RTC6Wrap.timed_mark_relDelegate timed_mark_rel = (RTC6Wrap.timed_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_mark_relDelegate>(nameof(timed_mark_rel));
        /// <summary>void n_mark_abs_3d(uint CardNo, int X, int Y, int Z);</summary>
        public static RTC6Wrap.n_mark_abs_3dDelegate n_mark_abs_3d = (RTC6Wrap.n_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_abs_3dDelegate>(nameof(n_mark_abs_3d));
        /// <summary>
        ///  void n_mark_rel_3d(uint CardNo, int dX, int dY, int dZ);
        /// </summary>
        public static RTC6Wrap.n_mark_rel_3dDelegate n_mark_rel_3d = (RTC6Wrap.n_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_rel_3dDelegate>(nameof(n_mark_rel_3d));
        /// <summary>void n_mark_abs(uint CardNo, int X, int Y);</summary>
        public static RTC6Wrap.n_mark_absDelegate n_mark_abs = (RTC6Wrap.n_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_absDelegate>(nameof(n_mark_abs));
        /// <summary>void n_mark_rel(uint CardNo, int dX, int dY);</summary>
        public static RTC6Wrap.n_mark_relDelegate n_mark_rel = (RTC6Wrap.n_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_relDelegate>(nameof(n_mark_rel));
        /// <summary>void mark_abs_3d(int X, int Y, int Z);</summary>
        public static RTC6Wrap.mark_abs_3dDelegate mark_abs_3d = (RTC6Wrap.mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_abs_3dDelegate>(nameof(mark_abs_3d));
        /// <summary>void mark_rel_3d(int dX, int dY, int dZ);</summary>
        public static RTC6Wrap.mark_rel_3dDelegate mark_rel_3d = (RTC6Wrap.mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_rel_3dDelegate>(nameof(mark_rel_3d));
        /// <summary>void mark_abs(int X, int Y);</summary>
        public static RTC6Wrap.mark_absDelegate mark_abs = (RTC6Wrap.mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_absDelegate>(nameof(mark_abs));
        /// <summary>void mark_rel(int dX, int dY);</summary>
        public static RTC6Wrap.mark_relDelegate mark_rel = (RTC6Wrap.mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_relDelegate>(nameof(mark_rel));
        /// <summary>
        ///  void n_timed_jump_abs_3d(uint CardNo, int X, int Y, int Z, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_jump_abs_3dDelegate n_timed_jump_abs_3d = (RTC6Wrap.n_timed_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_jump_abs_3dDelegate>(nameof(n_timed_jump_abs_3d));
        /// <summary>
        ///  void n_timed_jump_rel_3d(uint CardNo, int dX, int dY, int dZ, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_jump_rel_3dDelegate n_timed_jump_rel_3d = (RTC6Wrap.n_timed_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_jump_rel_3dDelegate>(nameof(n_timed_jump_rel_3d));
        /// <summary>
        ///  void n_timed_jump_abs(uint CardNo, int X, int Y, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_jump_absDelegate n_timed_jump_abs = (RTC6Wrap.n_timed_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_jump_absDelegate>(nameof(n_timed_jump_abs));
        /// <summary>
        ///  void n_timed_jump_rel(uint CardNo, int dX, int dY, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_jump_relDelegate n_timed_jump_rel = (RTC6Wrap.n_timed_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_jump_relDelegate>(nameof(n_timed_jump_rel));
        /// <summary>void timed_jump_abs_3d(int X, int Y, int Z, double T);</summary>
        public static RTC6Wrap.timed_jump_abs_3dDelegate timed_jump_abs_3d = (RTC6Wrap.timed_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_jump_abs_3dDelegate>(nameof(timed_jump_abs_3d));
        /// <summary>
        ///  void timed_jump_rel_3d(int dX, int dY, int dZ, double T);
        /// </summary>
        public static RTC6Wrap.timed_jump_rel_3dDelegate timed_jump_rel_3d = (RTC6Wrap.timed_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_jump_rel_3dDelegate>(nameof(timed_jump_rel_3d));
        /// <summary>void timed_jump_abs(int X, int Y, double T);</summary>
        public static RTC6Wrap.timed_jump_absDelegate timed_jump_abs = (RTC6Wrap.timed_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_jump_absDelegate>(nameof(timed_jump_abs));
        /// <summary>void timed_jump_rel(int dX, int dY, double T);</summary>
        public static RTC6Wrap.timed_jump_relDelegate timed_jump_rel = (RTC6Wrap.timed_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_jump_relDelegate>(nameof(timed_jump_rel));
        /// <summary>void n_jump_abs_3d(uint CardNo, int X, int Y, int Z);</summary>
        public static RTC6Wrap.n_jump_abs_3dDelegate n_jump_abs_3d = (RTC6Wrap.n_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_abs_3dDelegate>(nameof(n_jump_abs_3d));
        /// <summary>
        ///  void n_jump_rel_3d(uint CardNo, int dX, int dY, int dZ);
        /// </summary>
        public static RTC6Wrap.n_jump_rel_3dDelegate n_jump_rel_3d = (RTC6Wrap.n_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_rel_3dDelegate>(nameof(n_jump_rel_3d));
        /// <summary>void n_jump_abs(uint CardNo, int X, int Y);</summary>
        public static RTC6Wrap.n_jump_absDelegate n_jump_abs = (RTC6Wrap.n_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_absDelegate>(nameof(n_jump_abs));
        /// <summary>void n_jump_rel(uint CardNo, int dX, int dY);</summary>
        public static RTC6Wrap.n_jump_relDelegate n_jump_rel = (RTC6Wrap.n_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_jump_relDelegate>(nameof(n_jump_rel));
        /// <summary>void jump_abs_3d(int X, int Y, int Z);</summary>
        public static RTC6Wrap.jump_abs_3dDelegate jump_abs_3d = (RTC6Wrap.jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_abs_3dDelegate>(nameof(jump_abs_3d));
        /// <summary>void jump_rel_3d(int dX, int dY, int dZ);</summary>
        public static RTC6Wrap.jump_rel_3dDelegate jump_rel_3d = (RTC6Wrap.jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_rel_3dDelegate>(nameof(jump_rel_3d));
        /// <summary>void jump_abs(int X, int Y);</summary>
        public static RTC6Wrap.jump_absDelegate jump_abs = (RTC6Wrap.jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_absDelegate>(nameof(jump_abs));
        /// <summary>void jump_rel(int dX, int dY);</summary>
        public static RTC6Wrap.jump_relDelegate jump_rel = (RTC6Wrap.jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.jump_relDelegate>(nameof(jump_rel));
        /// <summary>
        ///  void n_para_mark_abs_3d(uint CardNo, int X, int Y, int Z, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_mark_abs_3dDelegate n_para_mark_abs_3d = (RTC6Wrap.n_para_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_mark_abs_3dDelegate>(nameof(n_para_mark_abs_3d));
        /// <summary>
        ///  void n_para_mark_rel_3d(uint CardNo, int dX, int dY, int dZ, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_mark_rel_3dDelegate n_para_mark_rel_3d = (RTC6Wrap.n_para_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_mark_rel_3dDelegate>(nameof(n_para_mark_rel_3d));
        /// <summary>
        ///  void n_para_mark_abs(uint CardNo, int X, int Y, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_mark_absDelegate n_para_mark_abs = (RTC6Wrap.n_para_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_mark_absDelegate>(nameof(n_para_mark_abs));
        /// <summary>
        ///  void n_para_mark_rel(uint CardNo, int dX, int dY, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_mark_relDelegate n_para_mark_rel = (RTC6Wrap.n_para_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_mark_relDelegate>(nameof(n_para_mark_rel));
        /// <summary>void para_mark_abs_3d(int X, int Y, int Z, uint P);</summary>
        public static RTC6Wrap.para_mark_abs_3dDelegate para_mark_abs_3d = (RTC6Wrap.para_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_mark_abs_3dDelegate>(nameof(para_mark_abs_3d));
        /// <summary>void para_mark_rel_3d(int dX, int dY, int dZ, uint P);</summary>
        public static RTC6Wrap.para_mark_rel_3dDelegate para_mark_rel_3d = (RTC6Wrap.para_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_mark_rel_3dDelegate>(nameof(para_mark_rel_3d));
        /// <summary>void para_mark_abs(int X, int Y, uint P);</summary>
        public static RTC6Wrap.para_mark_absDelegate para_mark_abs = (RTC6Wrap.para_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_mark_absDelegate>(nameof(para_mark_abs));
        /// <summary>void para_mark_rel(int dX, int dY, uint P);</summary>
        public static RTC6Wrap.para_mark_relDelegate para_mark_rel = (RTC6Wrap.para_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_mark_relDelegate>(nameof(para_mark_rel));
        /// <summary>
        ///  void n_para_jump_abs_3d(uint CardNo, int X, int Y, int Z, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_jump_abs_3dDelegate n_para_jump_abs_3d = (RTC6Wrap.n_para_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_jump_abs_3dDelegate>(nameof(n_para_jump_abs_3d));
        /// <summary>
        ///  void n_para_jump_rel_3d(uint CardNo, int dX, int dY, int dZ, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_jump_rel_3dDelegate n_para_jump_rel_3d = (RTC6Wrap.n_para_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_jump_rel_3dDelegate>(nameof(n_para_jump_rel_3d));
        /// <summary>
        ///  void n_para_jump_abs(uint CardNo, int X, int Y, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_jump_absDelegate n_para_jump_abs = (RTC6Wrap.n_para_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_jump_absDelegate>(nameof(n_para_jump_abs));
        /// <summary>
        ///  void n_para_jump_rel(uint CardNo, int dX, int dY, uint P);
        /// </summary>
        public static RTC6Wrap.n_para_jump_relDelegate n_para_jump_rel = (RTC6Wrap.n_para_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_para_jump_relDelegate>(nameof(n_para_jump_rel));
        /// <summary>void para_jump_abs_3d(int X, int Y, int Z, uint P);</summary>
        public static RTC6Wrap.para_jump_abs_3dDelegate para_jump_abs_3d = (RTC6Wrap.para_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_jump_abs_3dDelegate>(nameof(para_jump_abs_3d));
        /// <summary>void para_jump_rel_3d(int dX, int dY, int dZ, uint P);</summary>
        public static RTC6Wrap.para_jump_rel_3dDelegate para_jump_rel_3d = (RTC6Wrap.para_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_jump_rel_3dDelegate>(nameof(para_jump_rel_3d));
        /// <summary>void para_jump_abs(int X, int Y, uint P);</summary>
        public static RTC6Wrap.para_jump_absDelegate para_jump_abs = (RTC6Wrap.para_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_jump_absDelegate>(nameof(para_jump_abs));
        /// <summary>void para_jump_rel(int dX, int dY, uint P);</summary>
        public static RTC6Wrap.para_jump_relDelegate para_jump_rel = (RTC6Wrap.para_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.para_jump_relDelegate>(nameof(para_jump_rel));
        /// <summary>
        ///  void n_timed_para_mark_abs_3d(uint CardNo, int X, int Y, int Z, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_mark_abs_3dDelegate n_timed_para_mark_abs_3d = (RTC6Wrap.n_timed_para_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_mark_abs_3dDelegate>(nameof(n_timed_para_mark_abs_3d));
        /// <summary>
        ///  void n_timed_para_mark_rel_3d(uint CardNo, int dX, int dY, int dZ, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_mark_rel_3dDelegate n_timed_para_mark_rel_3d = (RTC6Wrap.n_timed_para_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_mark_rel_3dDelegate>(nameof(n_timed_para_mark_rel_3d));
        /// <summary>
        ///  void n_timed_para_jump_abs_3d(uint CardNo, int X, int Y, int Z, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_jump_abs_3dDelegate n_timed_para_jump_abs_3d = (RTC6Wrap.n_timed_para_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_jump_abs_3dDelegate>(nameof(n_timed_para_jump_abs_3d));
        /// <summary>
        ///  void n_timed_para_jump_rel_3d(uint CardNo, int dX, int dY, int dZ, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_jump_rel_3dDelegate n_timed_para_jump_rel_3d = (RTC6Wrap.n_timed_para_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_jump_rel_3dDelegate>(nameof(n_timed_para_jump_rel_3d));
        /// <summary>
        ///  void n_timed_para_mark_abs(uint CardNo, int X, int Y, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_mark_absDelegate n_timed_para_mark_abs = (RTC6Wrap.n_timed_para_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_mark_absDelegate>(nameof(n_timed_para_mark_abs));
        /// <summary>
        ///  void n_timed_para_mark_rel(uint CardNo, int dX, int dY, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_mark_relDelegate n_timed_para_mark_rel = (RTC6Wrap.n_timed_para_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_mark_relDelegate>(nameof(n_timed_para_mark_rel));
        /// <summary>
        ///  void n_timed_para_jump_abs(uint CardNo, int X, int Y, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_jump_absDelegate n_timed_para_jump_abs = (RTC6Wrap.n_timed_para_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_jump_absDelegate>(nameof(n_timed_para_jump_abs));
        /// <summary>
        ///  void n_timed_para_jump_rel(uint CardNo, int dX, int dY, uint P, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_para_jump_relDelegate n_timed_para_jump_rel = (RTC6Wrap.n_timed_para_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_para_jump_relDelegate>(nameof(n_timed_para_jump_rel));
        /// <summary>
        ///  void timed_para_mark_abs_3d(int X, int Y, int Z, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_mark_abs_3dDelegate timed_para_mark_abs_3d = (RTC6Wrap.timed_para_mark_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_mark_abs_3dDelegate>(nameof(timed_para_mark_abs_3d));
        /// <summary>
        ///  void timed_para_mark_rel_3d(int dX, int dY, int dZ, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_mark_rel_3dDelegate timed_para_mark_rel_3d = (RTC6Wrap.timed_para_mark_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_mark_rel_3dDelegate>(nameof(timed_para_mark_rel_3d));
        /// <summary>
        ///  void timed_para_jump_abs_3d(int X, int Y, int Z, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_jump_abs_3dDelegate timed_para_jump_abs_3d = (RTC6Wrap.timed_para_jump_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_jump_abs_3dDelegate>(nameof(timed_para_jump_abs_3d));
        /// <summary>
        ///  void timed_para_jump_rel_3d(int dX, int dY, int dZ, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_jump_rel_3dDelegate timed_para_jump_rel_3d = (RTC6Wrap.timed_para_jump_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_jump_rel_3dDelegate>(nameof(timed_para_jump_rel_3d));
        /// <summary>
        ///  void timed_para_mark_abs(int X, int Y, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_mark_absDelegate timed_para_mark_abs = (RTC6Wrap.timed_para_mark_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_mark_absDelegate>(nameof(timed_para_mark_abs));
        /// <summary>
        ///  void timed_para_mark_rel(int dX, int dY, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_mark_relDelegate timed_para_mark_rel = (RTC6Wrap.timed_para_mark_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_mark_relDelegate>(nameof(timed_para_mark_rel));
        /// <summary>
        ///  void timed_para_jump_abs(int X, int Y, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_jump_absDelegate timed_para_jump_abs = (RTC6Wrap.timed_para_jump_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_jump_absDelegate>(nameof(timed_para_jump_abs));
        /// <summary>
        ///  void timed_para_jump_rel(int dX, int dY, uint P, double T);
        /// </summary>
        public static RTC6Wrap.timed_para_jump_relDelegate timed_para_jump_rel = (RTC6Wrap.timed_para_jump_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_para_jump_relDelegate>(nameof(timed_para_jump_rel));
        /// <summary>void n_set_defocus_list(uint CardNo, int Shift);</summary>
        public static RTC6Wrap.n_set_defocus_listDelegate n_set_defocus_list = (RTC6Wrap.n_set_defocus_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_defocus_listDelegate>(nameof(n_set_defocus_list));
        /// <summary>
        ///  void n_set_defocus_offset_list(uint CardNo, int Shift);
        /// </summary>
        public static RTC6Wrap.n_set_defocus_offset_listDelegate n_set_defocus_offset_list = (RTC6Wrap.n_set_defocus_offset_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_defocus_offset_listDelegate>(nameof(n_set_defocus_offset_list));
        /// <summary>void n_set_zoom_list(uint CardNo, uint Zoom);</summary>
        public static RTC6Wrap.n_set_zoom_listDelegate n_set_zoom_list = (RTC6Wrap.n_set_zoom_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_zoom_listDelegate>(nameof(n_set_zoom_list));
        /// <summary>void set_defocus_list(int Shift);</summary>
        public static RTC6Wrap.set_defocus_listDelegate set_defocus_list = (RTC6Wrap.set_defocus_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_defocus_listDelegate>(nameof(set_defocus_list));
        /// <summary>void set_defocus_offset_list(int Shift);</summary>
        public static RTC6Wrap.set_defocus_offset_listDelegate set_defocus_offset_list = (RTC6Wrap.set_defocus_offset_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_defocus_offset_listDelegate>(nameof(set_defocus_offset_list));
        /// <summary>void set_zoom_list(uint Zoom);</summary>
        public static RTC6Wrap.set_zoom_listDelegate set_zoom_list = (RTC6Wrap.set_zoom_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_zoom_listDelegate>(nameof(set_zoom_list));
        /// <summary>
        ///  void n_timed_arc_abs(uint CardNo, int X, int Y, double Angle, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_arc_absDelegate n_timed_arc_abs = (RTC6Wrap.n_timed_arc_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_arc_absDelegate>(nameof(n_timed_arc_abs));
        /// <summary>
        ///  void n_timed_arc_rel(uint CardNo, int dX, int dY, double Angle, double T);
        /// </summary>
        public static RTC6Wrap.n_timed_arc_relDelegate n_timed_arc_rel = (RTC6Wrap.n_timed_arc_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_timed_arc_relDelegate>(nameof(n_timed_arc_rel));
        /// <summary>
        ///  void timed_arc_abs(int X, int Y, double Angle, double T);
        /// </summary>
        public static RTC6Wrap.timed_arc_absDelegate timed_arc_abs = (RTC6Wrap.timed_arc_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_arc_absDelegate>(nameof(timed_arc_abs));
        /// <summary>
        ///  void timed_arc_rel(int dX, int dY, double Angle, double T);
        /// </summary>
        public static RTC6Wrap.timed_arc_relDelegate timed_arc_rel = (RTC6Wrap.timed_arc_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.timed_arc_relDelegate>(nameof(timed_arc_rel));
        /// <summary>
        ///  void n_arc_abs_3d(uint CardNo, int X, int Y, int Z, double Angle);
        /// </summary>
        public static RTC6Wrap.n_arc_abs_3dDelegate n_arc_abs_3d = (RTC6Wrap.n_arc_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_arc_abs_3dDelegate>(nameof(n_arc_abs_3d));
        /// <summary>
        ///  void n_arc_rel_3d(uint CardNo, int dX, int dY, int dZ, double Angle);
        /// </summary>
        public static RTC6Wrap.n_arc_rel_3dDelegate n_arc_rel_3d = (RTC6Wrap.n_arc_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_arc_rel_3dDelegate>(nameof(n_arc_rel_3d));
        /// <summary>
        ///  void n_arc_abs(uint CardNo, int X, int Y, double Angle);
        /// </summary>
        public static RTC6Wrap.n_arc_absDelegate n_arc_abs = (RTC6Wrap.n_arc_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_arc_absDelegate>(nameof(n_arc_abs));
        /// <summary>
        ///  void n_arc_rel(uint CardNo, int dX, int dY, double Angle);
        /// </summary>
        public static RTC6Wrap.n_arc_relDelegate n_arc_rel = (RTC6Wrap.n_arc_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_arc_relDelegate>(nameof(n_arc_rel));
        /// <summary>
        ///  void n_set_ellipse(uint CardNo, uint A, uint B, double Phi0, double Phi);
        /// </summary>
        public static RTC6Wrap.n_set_ellipseDelegate n_set_ellipse = (RTC6Wrap.n_set_ellipseDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_ellipseDelegate>(nameof(n_set_ellipse));
        /// <summary>
        ///  void n_mark_ellipse_abs(uint CardNo, int X, int Y, double Alpha);
        /// </summary>
        public static RTC6Wrap.n_mark_ellipse_absDelegate n_mark_ellipse_abs = (RTC6Wrap.n_mark_ellipse_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_ellipse_absDelegate>(nameof(n_mark_ellipse_abs));
        /// <summary>
        ///  void n_mark_ellipse_rel(uint CardNo, int dX, int dY, double Alpha);
        /// </summary>
        public static RTC6Wrap.n_mark_ellipse_relDelegate n_mark_ellipse_rel = (RTC6Wrap.n_mark_ellipse_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_mark_ellipse_relDelegate>(nameof(n_mark_ellipse_rel));
        /// <summary>void arc_abs_3d(int X, int Y, int Z, double Angle);</summary>
        public static RTC6Wrap.arc_abs_3dDelegate arc_abs_3d = (RTC6Wrap.arc_abs_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.arc_abs_3dDelegate>(nameof(arc_abs_3d));
        /// <summary>void arc_rel_3d(int dX, int dY, int dZ, double Angle);</summary>
        public static RTC6Wrap.arc_rel_3dDelegate arc_rel_3d = (RTC6Wrap.arc_rel_3dDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.arc_rel_3dDelegate>(nameof(arc_rel_3d));
        /// <summary>void arc_abs(int X, int Y, double Angle);</summary>
        public static RTC6Wrap.arc_absDelegate arc_abs = (RTC6Wrap.arc_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.arc_absDelegate>(nameof(arc_abs));
        /// <summary>void arc_rel(int dX, int dY, double Angle);</summary>
        public static RTC6Wrap.arc_relDelegate arc_rel = (RTC6Wrap.arc_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.arc_relDelegate>(nameof(arc_rel));
        /// <summary>
        ///  void set_ellipse(uint A, uint B, double Phi0, double Phi);
        /// </summary>
        public static RTC6Wrap.set_ellipseDelegate set_ellipse = (RTC6Wrap.set_ellipseDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_ellipseDelegate>(nameof(set_ellipse));
        /// <summary>void mark_ellipse_abs(int X, int Y, double Alpha);</summary>
        public static RTC6Wrap.mark_ellipse_absDelegate mark_ellipse_abs = (RTC6Wrap.mark_ellipse_absDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_ellipse_absDelegate>(nameof(mark_ellipse_abs));
        /// <summary>void mark_ellipse_rel(int dX, int dY, double Alpha);</summary>
        public static RTC6Wrap.mark_ellipse_relDelegate mark_ellipse_rel = (RTC6Wrap.mark_ellipse_relDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.mark_ellipse_relDelegate>(nameof(mark_ellipse_rel));
        /// <summary>
        ///  void n_set_offset_xyz_list(uint CardNo, uint HeadNo, int XOffset, int YOffset, int ZOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_offset_xyz_listDelegate n_set_offset_xyz_list = (RTC6Wrap.n_set_offset_xyz_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_offset_xyz_listDelegate>(nameof(n_set_offset_xyz_list));
        /// <summary>
        ///  void n_set_offset_list(uint CardNo, uint HeadNo, int XOffset, int YOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_offset_listDelegate n_set_offset_list = (RTC6Wrap.n_set_offset_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_offset_listDelegate>(nameof(n_set_offset_list));
        /// <summary>
        ///  void n_set_matrix_list(uint CardNo, uint HeadNo, uint Ind1, uint Ind2, double Mij, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_matrix_listDelegate n_set_matrix_list = (RTC6Wrap.n_set_matrix_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_matrix_listDelegate>(nameof(n_set_matrix_list));
        /// <summary>
        ///  void n_set_angle_list(uint CardNo, uint HeadNo, double Angle, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_angle_listDelegate n_set_angle_list = (RTC6Wrap.n_set_angle_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_angle_listDelegate>(nameof(n_set_angle_list));
        /// <summary>
        ///  void n_set_scale_list(uint CardNo, uint HeadNo, double Scale, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_set_scale_listDelegate n_set_scale_list = (RTC6Wrap.n_set_scale_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scale_listDelegate>(nameof(n_set_scale_list));
        /// <summary>
        ///  void n_apply_mcbsp_list(uint CardNo, uint HeadNo, uint at_once);
        /// </summary>
        public static RTC6Wrap.n_apply_mcbsp_listDelegate n_apply_mcbsp_list = (RTC6Wrap.n_apply_mcbsp_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_apply_mcbsp_listDelegate>(nameof(n_apply_mcbsp_list));
        /// <summary>
        ///  void set_offset_xyz_list(uint HeadNo, int XOffset, int YOffset, int ZOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_offset_xyz_listDelegate set_offset_xyz_list = (RTC6Wrap.set_offset_xyz_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_offset_xyz_listDelegate>(nameof(set_offset_xyz_list));
        /// <summary>
        ///  void set_offset_list(uint HeadNo, int XOffset, int YOffset, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_offset_listDelegate set_offset_list = (RTC6Wrap.set_offset_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_offset_listDelegate>(nameof(set_offset_list));
        /// <summary>
        ///  void set_matrix_list(uint HeadNo, uint Ind1, uint Ind2, double Mij, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_matrix_listDelegate set_matrix_list = (RTC6Wrap.set_matrix_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_matrix_listDelegate>(nameof(set_matrix_list));
        /// <summary>
        ///  void set_angle_list(uint HeadNo, double Angle, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_angle_listDelegate set_angle_list = (RTC6Wrap.set_angle_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_angle_listDelegate>(nameof(set_angle_list));
        /// <summary>
        ///  void set_scale_list(uint HeadNo, double Scale, uint at_once);
        /// </summary>
        public static RTC6Wrap.set_scale_listDelegate set_scale_list = (RTC6Wrap.set_scale_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scale_listDelegate>(nameof(set_scale_list));
        /// <summary>void apply_mcbsp_list(uint HeadNo, uint at_once);</summary>
        public static RTC6Wrap.apply_mcbsp_listDelegate apply_mcbsp_list = (RTC6Wrap.apply_mcbsp_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.apply_mcbsp_listDelegate>(nameof(apply_mcbsp_list));
        /// <summary>void n_set_mark_speed(uint CardNo, double Speed);</summary>
        public static RTC6Wrap.n_set_mark_speedDelegate n_set_mark_speed = (RTC6Wrap.n_set_mark_speedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_mark_speedDelegate>(nameof(n_set_mark_speed));
        /// <summary>void n_set_jump_speed(uint CardNo, double Speed);</summary>
        public static RTC6Wrap.n_set_jump_speedDelegate n_set_jump_speed = (RTC6Wrap.n_set_jump_speedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_jump_speedDelegate>(nameof(n_set_jump_speed));
        /// <summary>
        ///  void n_set_sky_writing_para_list(uint CardNo, double Timelag, int LaserOnShift, uint Nprev, uint Npost);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writing_para_listDelegate n_set_sky_writing_para_list = (RTC6Wrap.n_set_sky_writing_para_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_para_listDelegate>(nameof(n_set_sky_writing_para_list));
        /// <summary>
        ///  void n_set_sky_writing_list(uint CardNo, double Timelag, int LaserOnShift);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writing_listDelegate n_set_sky_writing_list = (RTC6Wrap.n_set_sky_writing_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_listDelegate>(nameof(n_set_sky_writing_list));
        /// <summary>
        ///  void n_set_sky_writing_limit_list(uint CardNo, double CosAngle);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writing_limit_listDelegate n_set_sky_writing_limit_list = (RTC6Wrap.n_set_sky_writing_limit_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_limit_listDelegate>(nameof(n_set_sky_writing_limit_list));
        /// <summary>
        ///  void n_set_sky_writing_mode_list(uint CardNo, uint Mode);
        /// </summary>
        public static RTC6Wrap.n_set_sky_writing_mode_listDelegate n_set_sky_writing_mode_list = (RTC6Wrap.n_set_sky_writing_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_sky_writing_mode_listDelegate>(nameof(n_set_sky_writing_mode_list));
        /// <summary>
        ///  void n_set_scanner_delays(uint CardNo, uint Jump, uint Mark, uint Polygon);
        /// </summary>
        public static RTC6Wrap.n_set_scanner_delaysDelegate n_set_scanner_delays = (RTC6Wrap.n_set_scanner_delaysDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanner_delaysDelegate>(nameof(n_set_scanner_delays));
        /// <summary>void n_set_jump_mode_list(uint CardNo, int Flag);</summary>
        public static RTC6Wrap.n_set_jump_mode_listDelegate n_set_jump_mode_list = (RTC6Wrap.n_set_jump_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_jump_mode_listDelegate>(nameof(n_set_jump_mode_list));
        /// <summary>void n_enduring_wobbel(uint CardNo);</summary>
        public static RTC6Wrap.n_enduring_wobbelDelegate n_enduring_wobbel = (RTC6Wrap.n_enduring_wobbelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_enduring_wobbelDelegate>(nameof(n_enduring_wobbel));
        /// <summary>
        ///  void n_set_delay_mode_list(uint CardNo, uint VarPoly, uint DirectMove3D, uint EdgeLevel, uint MinJumpDelay, uint JumpLengthLimit);
        /// </summary>
        public static RTC6Wrap.n_set_delay_mode_listDelegate n_set_delay_mode_list = (RTC6Wrap.n_set_delay_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_delay_mode_listDelegate>(nameof(n_set_delay_mode_list));
        /// <summary>void set_mark_speed(double Speed);</summary>
        public static RTC6Wrap.set_mark_speedDelegate set_mark_speed = (RTC6Wrap.set_mark_speedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_mark_speedDelegate>(nameof(set_mark_speed));
        /// <summary>void set_jump_speed(double Speed);</summary>
        public static RTC6Wrap.set_jump_speedDelegate set_jump_speed = (RTC6Wrap.set_jump_speedDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_jump_speedDelegate>(nameof(set_jump_speed));
        /// <summary>
        ///  void set_sky_writing_para_list(double Timelag, int LaserOnShift, uint Nprev, uint Npost);
        /// </summary>
        public static RTC6Wrap.set_sky_writing_para_listDelegate set_sky_writing_para_list = (RTC6Wrap.set_sky_writing_para_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_para_listDelegate>(nameof(set_sky_writing_para_list));
        /// <summary>
        ///  void set_sky_writing_list(double Timelag, int LaserOnShift);
        /// </summary>
        public static RTC6Wrap.set_sky_writing_listDelegate set_sky_writing_list = (RTC6Wrap.set_sky_writing_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_listDelegate>(nameof(set_sky_writing_list));
        /// <summary>void set_sky_writing_limit_list(double CosAngle);</summary>
        public static RTC6Wrap.set_sky_writing_limit_listDelegate set_sky_writing_limit_list = (RTC6Wrap.set_sky_writing_limit_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_limit_listDelegate>(nameof(set_sky_writing_limit_list));
        /// <summary>void set_sky_writing_mode_list(uint Mode);</summary>
        public static RTC6Wrap.set_sky_writing_mode_listDelegate set_sky_writing_mode_list = (RTC6Wrap.set_sky_writing_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_sky_writing_mode_listDelegate>(nameof(set_sky_writing_mode_list));
        /// <summary>
        ///  void set_scanner_delays(uint Jump, uint Mark, uint Polygon);
        /// </summary>
        public static RTC6Wrap.set_scanner_delaysDelegate set_scanner_delays = (RTC6Wrap.set_scanner_delaysDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanner_delaysDelegate>(nameof(set_scanner_delays));
        /// <summary>void set_jump_mode_list(int Flag);</summary>
        public static RTC6Wrap.set_jump_mode_listDelegate set_jump_mode_list = (RTC6Wrap.set_jump_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_jump_mode_listDelegate>(nameof(set_jump_mode_list));
        /// <summary>void enduring_wobbel();</summary>
        public static RTC6Wrap.enduring_wobbelDelegate enduring_wobbel = (RTC6Wrap.enduring_wobbelDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.enduring_wobbelDelegate>(nameof(enduring_wobbel));
        /// <summary>
        ///  void set_delay_mode_list(uint VarPoly, uint DirectMove3D, uint EdgeLevel, uint MinJumpDelay, uint JumpLengthLimit);
        /// </summary>
        public static RTC6Wrap.set_delay_mode_listDelegate set_delay_mode_list = (RTC6Wrap.set_delay_mode_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_delay_mode_listDelegate>(nameof(set_delay_mode_list));
        /// <summary>
        ///  void n_activate_scanahead_autodelays_list(uint CardNo, int Mode);
        /// </summary>
        public static RTC6Wrap.n_activate_scanahead_autodelays_listDelegate n_activate_scanahead_autodelays_list = (RTC6Wrap.n_activate_scanahead_autodelays_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_activate_scanahead_autodelays_listDelegate>(nameof(n_activate_scanahead_autodelays_list));
        /// <summary>
        ///  void n_set_scanahead_laser_shifts_list(uint CardNo, int dLasOn, int dLasOff);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_laser_shifts_listDelegate n_set_scanahead_laser_shifts_list = (RTC6Wrap.n_set_scanahead_laser_shifts_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_laser_shifts_listDelegate>(nameof(n_set_scanahead_laser_shifts_list));
        /// <summary>
        ///  void n_set_scanahead_line_params_list(uint CardNo, uint CornerScale, uint EndScale, uint AccScale);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_line_params_listDelegate n_set_scanahead_line_params_list = (RTC6Wrap.n_set_scanahead_line_params_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_line_params_listDelegate>(nameof(n_set_scanahead_line_params_list));
        /// <summary>
        ///  void n_set_scanahead_line_params_ex_list(uint CardNo, uint CornerScale, uint EndScale, uint AccScale, uint JumpScale);
        /// </summary>
        public static RTC6Wrap.n_set_scanahead_line_params_ex_listDelegate n_set_scanahead_line_params_ex_list = (RTC6Wrap.n_set_scanahead_line_params_ex_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_set_scanahead_line_params_ex_listDelegate>(nameof(n_set_scanahead_line_params_ex_list));
        /// <summary>void activate_scanahead_autodelays_list(int Mode);</summary>
        public static RTC6Wrap.activate_scanahead_autodelays_listDelegate activate_scanahead_autodelays_list = (RTC6Wrap.activate_scanahead_autodelays_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.activate_scanahead_autodelays_listDelegate>(nameof(activate_scanahead_autodelays_list));
        /// <summary>
        ///  void set_scanahead_laser_shifts_list(int dLasOn, int dLasOff);
        /// </summary>
        public static RTC6Wrap.set_scanahead_laser_shifts_listDelegate set_scanahead_laser_shifts_list = (RTC6Wrap.set_scanahead_laser_shifts_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_laser_shifts_listDelegate>(nameof(set_scanahead_laser_shifts_list));
        /// <summary>
        ///  void set_scanahead_line_params_list(uint CornerScale, uint EndScale, uint AccScale);
        /// </summary>
        public static RTC6Wrap.set_scanahead_line_params_listDelegate set_scanahead_line_params_list = (RTC6Wrap.set_scanahead_line_params_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_line_params_listDelegate>(nameof(set_scanahead_line_params_list));
        /// <summary>
        ///  void set_scanahead_line_params_ex_list(uint CornerScale, uint EndScale, uint AccScale, uint JumpScale);
        /// </summary>
        public static RTC6Wrap.set_scanahead_line_params_ex_listDelegate set_scanahead_line_params_ex_list = (RTC6Wrap.set_scanahead_line_params_ex_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.set_scanahead_line_params_ex_listDelegate>(nameof(set_scanahead_line_params_ex_list));
        /// <summary>
        ///  void n_stepper_enable_list(uint CardNo, int Enable1, int Enable2);
        /// </summary>
        public static RTC6Wrap.n_stepper_enable_listDelegate n_stepper_enable_list = (RTC6Wrap.n_stepper_enable_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_enable_listDelegate>(nameof(n_stepper_enable_list));
        /// <summary>
        ///  void n_stepper_control_list(uint CardNo, int Period1, int Period2);
        /// </summary>
        public static RTC6Wrap.n_stepper_control_listDelegate n_stepper_control_list = (RTC6Wrap.n_stepper_control_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_control_listDelegate>(nameof(n_stepper_control_list));
        /// <summary>
        ///  void n_stepper_abs_no_list(uint CardNo, uint No, int Pos);
        /// </summary>
        public static RTC6Wrap.n_stepper_abs_no_listDelegate n_stepper_abs_no_list = (RTC6Wrap.n_stepper_abs_no_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_abs_no_listDelegate>(nameof(n_stepper_abs_no_list));
        /// <summary>
        ///  void n_stepper_rel_no_list(uint CardNo, uint No, int dPos);
        /// </summary>
        public static RTC6Wrap.n_stepper_rel_no_listDelegate n_stepper_rel_no_list = (RTC6Wrap.n_stepper_rel_no_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_rel_no_listDelegate>(nameof(n_stepper_rel_no_list));
        /// <summary>
        ///  void n_stepper_abs_list(uint CardNo, int Pos1, int Pos2);
        /// </summary>
        public static RTC6Wrap.n_stepper_abs_listDelegate n_stepper_abs_list = (RTC6Wrap.n_stepper_abs_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_abs_listDelegate>(nameof(n_stepper_abs_list));
        /// <summary>
        ///  void n_stepper_rel_list(uint CardNo, int dPos1, int dPos2);
        /// </summary>
        public static RTC6Wrap.n_stepper_rel_listDelegate n_stepper_rel_list = (RTC6Wrap.n_stepper_rel_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_rel_listDelegate>(nameof(n_stepper_rel_list));
        /// <summary>void n_stepper_wait(uint CardNo, uint No);</summary>
        public static RTC6Wrap.n_stepper_waitDelegate n_stepper_wait = (RTC6Wrap.n_stepper_waitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.n_stepper_waitDelegate>(nameof(n_stepper_wait));
        /// <summary>void stepper_enable_list(int Enable1, int Enable2);</summary>
        public static RTC6Wrap.stepper_enable_listDelegate stepper_enable_list = (RTC6Wrap.stepper_enable_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_enable_listDelegate>(nameof(stepper_enable_list));
        /// <summary>void stepper_control_list(int Period1, int Period2);</summary>
        public static RTC6Wrap.stepper_control_listDelegate stepper_control_list = (RTC6Wrap.stepper_control_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_control_listDelegate>(nameof(stepper_control_list));
        /// <summary>void stepper_abs_no_list(uint No, int Pos);</summary>
        public static RTC6Wrap.stepper_abs_no_listDelegate stepper_abs_no_list = (RTC6Wrap.stepper_abs_no_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_abs_no_listDelegate>(nameof(stepper_abs_no_list));
        /// <summary>void stepper_rel_no_list(uint No, int dPos);</summary>
        public static RTC6Wrap.stepper_rel_no_listDelegate stepper_rel_no_list = (RTC6Wrap.stepper_rel_no_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_rel_no_listDelegate>(nameof(stepper_rel_no_list));
        /// <summary>void stepper_abs_list(int Pos1, int Pos2);</summary>
        public static RTC6Wrap.stepper_abs_listDelegate stepper_abs_list = (RTC6Wrap.stepper_abs_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_abs_listDelegate>(nameof(stepper_abs_list));
        /// <summary>void stepper_rel_list(int dPos1, int dPos2);</summary>
        public static RTC6Wrap.stepper_rel_listDelegate stepper_rel_list = (RTC6Wrap.stepper_rel_listDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_rel_listDelegate>(nameof(stepper_rel_list));
        /// <summary>void stepper_wait(uint No);</summary>
        public static RTC6Wrap.stepper_waitDelegate stepper_wait = (RTC6Wrap.stepper_waitDelegate)RTC6Wrap.FunctionImporter.Import<RTC6Wrap.stepper_waitDelegate>(nameof(stepper_wait));

        private class FunctionImporter
        {
            private static string DllName;
            private static IntPtr hModule;
            private static RTC6Wrap.FunctionImporter instance;

            [DllImport("Kernel32.dll")]
            private static extern IntPtr LoadLibrary(string path);

            [DllImport("kernel32.dll")]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("Kernel32.dll")]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            protected FunctionImporter(string DllName) => RTC6Wrap.FunctionImporter.hModule = RTC6Wrap.FunctionImporter.LoadLibrary(DllName);

            ~FunctionImporter()
            {
                if (!(RTC6Wrap.FunctionImporter.hModule != IntPtr.Zero))
                    return;
                RTC6Wrap.FunctionImporter.FreeLibrary(RTC6Wrap.FunctionImporter.hModule);
            }

            public static Delegate Import<T>(string functionName)
            {
                if (RTC6Wrap.FunctionImporter.instance == null)
                {
                    RTC6Wrap.FunctionImporter.DllName = Marshal.SizeOf(typeof(IntPtr)) == 4 ? "RTC6DLL.dll" : "RTC6DLLx64.dll";
                    RTC6Wrap.FunctionImporter.instance = new RTC6Wrap.FunctionImporter(RTC6Wrap.FunctionImporter.DllName);
                    if (RTC6Wrap.FunctionImporter.hModule == IntPtr.Zero)
                        throw new FileNotFoundException(RTC6Wrap.FunctionImporter.DllName + " not found. ");
                }
                IntPtr procAddress = RTC6Wrap.FunctionImporter.GetProcAddress(RTC6Wrap.FunctionImporter.hModule, functionName);
                try
                {
                    return Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
                }
                catch (Exception ex)
                {
                    if ((ex is ArgumentException) || (ex is ArgumentNullException))
                        throw new EntryPointNotFoundException(functionName);
                    else throw;
                }
            }
        }

        public delegate uint init_rtc6_dllDelegate();

        public delegate void free_rtc6_dllDelegate();

        public delegate void set_rtc4_modeDelegate();

        public delegate void set_rtc5_modeDelegate();

        public delegate void set_rtc6_modeDelegate();

        public delegate uint get_rtc_modeDelegate();

        public delegate uint n_get_errorDelegate(uint CardNo);

        public delegate uint n_get_last_errorDelegate(uint CardNo);

        public delegate void n_reset_errorDelegate(uint CardNo, uint Code);

        public delegate uint n_set_verifyDelegate(uint CardNo, uint Verify);

        public delegate uint get_errorDelegate();

        public delegate uint get_last_errorDelegate();

        public delegate void reset_errorDelegate(uint Code);

        public delegate uint set_verifyDelegate(uint Verify);

        public delegate uint verify_checksumDelegate(string Name);

        public delegate uint eth_count_cardsDelegate();

        public delegate uint eth_found_cardsDelegate();

        public delegate uint eth_max_cardDelegate();

        public delegate int eth_remove_cardDelegate(uint CardNo);

        public delegate void eth_get_card_infoDelegate(uint CardNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] uint[] Ptr);

        public delegate void eth_get_card_info_searchDelegate(uint SearchNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 16)] uint[] Ptr);

        public delegate void eth_set_search_cards_timeoutDelegate(uint TimeOut);

        public delegate uint eth_search_cardsDelegate(uint Ip, uint NetMask);

        public delegate uint eth_search_cards_rangeDelegate(uint StartIp, uint EndIp);

        public delegate int eth_assign_card_ipDelegate(uint Ip, uint CardNo);

        public delegate int eth_assign_cardDelegate(uint SearchNo, uint CardNo);

        public delegate uint eth_convert_string_to_ipDelegate(string IpString);

        public delegate void eth_convert_ip_to_stringDelegate(uint Ip, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] IpString);

        public delegate uint eth_get_ipDelegate(uint CardNo);

        public delegate uint eth_get_ip_searchDelegate(uint SearchNo);

        public delegate uint eth_get_serial_searchDelegate(uint SearchNo);

        public delegate uint n_eth_get_last_errorDelegate(uint CardNo);

        public delegate uint n_eth_get_errorDelegate(uint CardNo);

        public delegate uint n_eth_error_dumpDelegate(uint CardNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] Dump);

        public delegate uint n_eth_set_static_ipDelegate(
          uint CardNo,
          uint Ip,
          uint NetMask,
          uint Gateway);

        public delegate uint n_eth_get_static_ipDelegate(
          uint CardNo,
          out uint Ip,
          out uint NetMask,
          out uint Gateway);

        public delegate uint n_eth_set_port_numbersDelegate(
          uint CardNo,
          uint UDPsearch,
          uint UDPexcl,
          uint TCP);

        public delegate uint n_eth_get_port_numbersDelegate(
          uint CardNo,
          out uint UDPsearch,
          out uint UDPexcl,
          out uint TCP);

        public delegate void n_eth_set_com_timeoutsDelegate(
          uint CardNo,
          uint AcquireTimeout,
          uint AcquireMaxRetries,
          uint SendRecvTimeout,
          uint SendRecvMaxRetries,
          uint KeepAlive,
          uint KeepInterval);

        public delegate void n_eth_get_com_timeoutsDelegate(
          uint CardNo,
          out uint AcquireTimeout,
          out uint AcquireMaxRetries,
          out uint SendRecvTimeout,
          out uint SendRecvMaxRetries,
          out uint KeepAlive,
          out uint KeepInterval);

        public delegate uint n_eth_check_connectionDelegate(uint CardNo);

        public delegate void n_set_eth_boot_controlDelegate(uint CardNo, uint Ctrl);

        public delegate void n_eth_boot_timeoutDelegate(uint CardNo, uint Timeout);

        public delegate void n_eth_boot_dcmdDelegate(uint CardNo);

        public delegate uint n_store_programDelegate(uint CardNo, uint Mode);

        public delegate uint eth_get_last_errorDelegate();

        public delegate uint eth_get_errorDelegate();

        public delegate uint eth_error_dumpDelegate([MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] Dump);

        public delegate uint eth_set_static_ipDelegate(uint Ip, uint NetMask, uint Gateway);

        public delegate uint eth_get_static_ipDelegate(out uint Ip, out uint NetMask, out uint Gateway);

        public delegate uint eth_set_port_numbersDelegate(uint UDPsearch, uint UDPexcl, uint TCP);

        public delegate uint eth_get_port_numbersDelegate(
          out uint UDPsearch,
          out uint UDPexcl,
          out uint TCP);

        public delegate void eth_set_com_timeoutsDelegate(
          uint AcquireTimeout,
          uint AcquireMaxRetries,
          uint SendRecvTimeout,
          uint SendRecvMaxRetries,
          uint KeepAlive,
          uint KeepInterval);

        public delegate void eth_get_com_timeoutsDelegate(
          out uint AcquireTimeout,
          out uint AcquireMaxRetries,
          out uint SendRecvTimeout,
          out uint SendRecvMaxRetries,
          out uint KeepAlive,
          out uint KeepInterval);

        public delegate uint eth_check_connectionDelegate();

        public delegate void set_eth_boot_controlDelegate(uint Ctrl);

        public delegate void eth_boot_timeoutDelegate(uint Timeout);

        public delegate void eth_boot_dcmdDelegate();

        public delegate uint store_programDelegate(uint Mode);

        public delegate uint read_abc_from_fileDelegate(
          string Name,
          out double A,
          out double B,
          out double C);

        public delegate uint write_abc_to_fileDelegate(string Name, double A, double B, double C);

        public delegate uint n_create_dat_fileDelegate(uint CardNo, int Flag);

        public delegate uint create_dat_fileDelegate(int Flag);

        public delegate uint transformDelegate(out int Sig1, out int Sig2, [MarshalAs(UnmanagedType.LPArray, SizeConst = 132130)] uint[] Ptr, uint Code);

        public delegate uint rtc6_count_cardsDelegate();

        public delegate uint acquire_rtcDelegate(uint CardNo);

        public delegate uint release_rtcDelegate(uint CardNo);

        public delegate uint select_rtcDelegate(uint CardNo);

        public delegate uint get_dll_versionDelegate();

        public delegate uint n_get_card_typeDelegate(uint CardNo);

        public delegate uint n_get_serial_numberDelegate(uint CardNo);

        public delegate uint n_get_hex_versionDelegate(uint CardNo);

        public delegate uint n_get_rtc_versionDelegate(uint CardNo);

        public delegate uint n_get_bios_versionDelegate(uint CardNo);

        public delegate uint get_card_typeDelegate();

        public delegate uint get_serial_numberDelegate();

        public delegate uint get_hex_versionDelegate();

        public delegate uint get_rtc_versionDelegate();

        public delegate uint get_bios_versionDelegate();

        public delegate uint n_load_program_fileDelegate(uint CardNo, string Path);

        public delegate void n_sync_slavesDelegate(uint CardNo);

        public delegate uint n_get_sync_statusDelegate(uint CardNo);

        public delegate void n_master_slave_configDelegate(uint CardNo, uint Flags);

        public delegate uint n_load_correction_fileDelegate(
          uint CardNo,
          string Name,
          uint No,
          uint Dim);

        public delegate uint n_load_zoom_correction_fileDelegate(uint CardNo, string Name, uint No);

        public delegate uint n_load_oct_table_noDelegate(uint CardNo, double A, double B, uint No);

        public delegate uint n_load_z_table_noDelegate(
          uint CardNo,
          double A,
          double B,
          double C,
          uint No);

        public delegate uint n_load_z_tableDelegate(uint CardNo, double A, double B, double C);

        public delegate void n_select_cor_tableDelegate(uint CardNo, uint HeadA, uint HeadB);

        public delegate uint n_set_dsp_modeDelegate(uint CardNo, uint Mode);

        public delegate int n_load_stretch_tableDelegate(
          uint CardNo,
          string Name,
          int No,
          uint TableNo);

        public delegate void n_number_of_correction_tablesDelegate(uint CardNo, uint Number);

        public delegate double n_get_head_paraDelegate(uint CardNo, uint HeadNo, uint ParaNo);

        public delegate double n_get_table_paraDelegate(uint CardNo, uint TableNo, uint ParaNo);

        public delegate uint load_program_fileDelegate(string Path);

        public delegate void sync_slavesDelegate();

        public delegate uint get_sync_statusDelegate();

        public delegate void master_slave_configDelegate(uint Flags);

        public delegate uint load_correction_fileDelegate(string Name, uint No, uint Dim);

        public delegate uint load_zoom_correction_fileDelegate(string Name, uint No);

        public delegate uint load_oct_table_noDelegate(double A, double B, uint No);

        public delegate uint load_z_table_noDelegate(double A, double B, double C, uint No);

        public delegate uint load_z_tableDelegate(double A, double B, double C);

        public delegate void select_cor_tableDelegate(uint HeadA, uint HeadB);

        public delegate uint set_dsp_modeDelegate(uint Mode);

        public delegate int load_stretch_tableDelegate(string Name, int No, uint TableNo);

        public delegate void number_of_correction_tablesDelegate(uint Number);

        public delegate double get_head_paraDelegate(uint HeadNo, uint ParaNo);

        public delegate double get_table_paraDelegate(uint TableNo, uint ParaNo);

        public delegate void n_config_listDelegate(uint CardNo, uint Mem1, uint Mem2);

        public delegate void n_get_config_listDelegate(uint CardNo);

        public delegate uint n_save_diskDelegate(uint CardNo, string Name, uint Mode);

        public delegate uint n_load_diskDelegate(uint CardNo, string Name, uint Mode);

        public delegate uint n_get_list_spaceDelegate(uint CardNo);

        public delegate void config_listDelegate(uint Mem1, uint Mem2);

        public delegate void get_config_listDelegate();

        public delegate uint save_diskDelegate(string Name, uint Mode);

        public delegate uint load_diskDelegate(string Name, uint Mode);

        public delegate uint get_list_spaceDelegate();

        public delegate void n_set_start_list_posDelegate(uint CardNo, uint ListNo, uint Pos);

        public delegate void n_set_start_listDelegate(uint CardNo, uint ListNo);

        public delegate void n_set_start_list_1Delegate(uint CardNo);

        public delegate void n_set_start_list_2Delegate(uint CardNo);

        public delegate void n_set_input_pointerDelegate(uint CardNo, uint Pos);

        public delegate uint n_load_listDelegate(uint CardNo, uint ListNo, uint Pos);

        public delegate void n_load_subDelegate(uint CardNo, uint Index);

        public delegate void n_load_charDelegate(uint CardNo, uint Char);

        public delegate void n_load_text_tableDelegate(uint CardNo, uint Index);

        public delegate void n_get_list_pointerDelegate(uint CardNo, out uint ListNo, out uint Pos);

        public delegate uint n_get_input_pointerDelegate(uint CardNo);

        public delegate void set_start_list_posDelegate(uint ListNo, uint Pos);

        public delegate void set_start_listDelegate(uint ListNo);

        public delegate void set_start_list_1Delegate();

        public delegate void set_start_list_2Delegate();

        public delegate void set_input_pointerDelegate(uint Pos);

        public delegate uint load_listDelegate(uint ListNo, uint Pos);

        public delegate void load_subDelegate(uint Index);

        public delegate void load_charDelegate(uint Char);

        public delegate void load_text_tableDelegate(uint Index);

        public delegate void get_list_pointerDelegate(out uint ListNo, out uint Pos);

        public delegate uint get_input_pointerDelegate();

        public delegate void n_execute_list_posDelegate(uint CardNo, uint ListNo, uint Pos);

        public delegate void n_execute_at_pointerDelegate(uint CardNo, uint Pos);

        public delegate void n_execute_listDelegate(uint CardNo, uint ListNo);

        public delegate void n_execute_list_1Delegate(uint CardNo);

        public delegate void n_execute_list_2Delegate(uint CardNo);

        public delegate uint n_list_jump_rel_ctrlDelegate(uint CardNo, int Pos);

        public delegate void n_get_out_pointerDelegate(uint CardNo, out uint ListNo, out uint Pos);

        public delegate void execute_list_posDelegate(uint ListNo, uint Pos);

        public delegate void execute_at_pointerDelegate(uint Pos);

        public delegate void execute_listDelegate(uint ListNo);

        public delegate void execute_list_1Delegate();

        public delegate void execute_list_2Delegate();

        public delegate uint list_jump_rel_ctrlDelegate(int Pos);

        public delegate void get_out_pointerDelegate(out uint ListNo, out uint Pos);

        public delegate void n_auto_change_posDelegate(uint CardNo, uint Pos);

        public delegate void n_start_loopDelegate(uint CardNo);

        public delegate void n_quit_loopDelegate(uint CardNo);

        public delegate void n_pause_listDelegate(uint CardNo);

        public delegate void n_restart_listDelegate(uint CardNo);

        public delegate void n_release_waitDelegate(uint CardNo);

        public delegate void n_stop_executionDelegate(uint CardNo);

        public delegate void n_set_pause_list_condDelegate(uint CardNo, uint Mask1, uint Mask0);

        public delegate void n_set_pause_list_not_condDelegate(uint CardNo, uint Mask1, uint Mask0);

        public delegate void n_auto_changeDelegate(uint CardNo);

        public delegate void n_stop_listDelegate(uint CardNo);

        public delegate uint n_get_wait_statusDelegate(uint CardNo);

        public delegate uint n_read_statusDelegate(uint CardNo);

        public delegate void n_get_statusDelegate(uint CardNo, out uint Status, out uint Pos);

        public delegate void auto_change_posDelegate(uint Pos);

        public delegate void start_loopDelegate();

        public delegate void quit_loopDelegate();

        public delegate void pause_listDelegate();

        public delegate void restart_listDelegate();

        public delegate void release_waitDelegate();

        public delegate void stop_executionDelegate();

        public delegate void set_pause_list_condDelegate(uint Mask1, uint Mask0);

        public delegate void set_pause_list_not_condDelegate(uint Mask1, uint Mask0);

        public delegate void auto_changeDelegate();

        public delegate void stop_listDelegate();

        public delegate uint get_wait_statusDelegate();

        public delegate uint read_statusDelegate();

        public delegate void get_statusDelegate(out uint Status, out uint Pos);

        public delegate void n_set_extstartposDelegate(uint CardNo, uint Pos);

        public delegate void n_set_max_countsDelegate(uint CardNo, uint Counts);

        public delegate void n_set_control_modeDelegate(uint CardNo, uint Mode);

        public delegate void n_simulate_ext_stopDelegate(uint CardNo);

        public delegate void n_simulate_ext_start_ctrlDelegate(uint CardNo);

        public delegate uint n_get_countsDelegate(uint CardNo);

        public delegate uint n_get_startstop_infoDelegate(uint CardNo);

        public delegate void set_extstartposDelegate(uint Pos);

        public delegate void set_max_countsDelegate(uint Counts);

        public delegate void set_control_modeDelegate(uint Mode);

        public delegate void simulate_ext_stopDelegate();

        public delegate void simulate_ext_start_ctrlDelegate();

        public delegate uint get_countsDelegate();

        public delegate uint get_startstop_infoDelegate();

        public delegate void n_copy_dst_srcDelegate(uint CardNo, uint Dst, uint Src, uint Mode);

        public delegate void n_set_char_pointerDelegate(uint CardNo, uint Char, uint Pos);

        public delegate void n_set_sub_pointerDelegate(uint CardNo, uint Index, uint Pos);

        public delegate void n_set_text_table_pointerDelegate(uint CardNo, uint Index, uint Pos);

        public delegate void n_set_char_tableDelegate(uint CardNo, uint Index, uint Pos);

        public delegate uint n_get_char_pointerDelegate(uint CardNo, uint Char);

        public delegate uint n_get_sub_pointerDelegate(uint CardNo, uint Index);

        public delegate uint n_get_text_table_pointerDelegate(uint CardNo, uint Index);

        public delegate void copy_dst_srcDelegate(uint Dst, uint Src, uint Mode);

        public delegate void set_char_pointerDelegate(uint Char, uint Pos);

        public delegate void set_sub_pointerDelegate(uint Index, uint Pos);

        public delegate void set_text_table_pointerDelegate(uint Index, uint Pos);

        public delegate void set_char_tableDelegate(uint Index, uint Pos);

        public delegate uint get_char_pointerDelegate(uint Char);

        public delegate uint get_sub_pointerDelegate(uint Index);

        public delegate uint get_text_table_pointerDelegate(uint Index);

        public delegate void n_time_updateDelegate(uint CardNo);

        public delegate void n_time_control_ethDelegate(uint CardNo, double PPM);

        public delegate void n_set_serial_stepDelegate(uint CardNo, uint No, uint Step);

        public delegate void n_select_serial_setDelegate(uint CardNo, uint No);

        public delegate void n_set_serialDelegate(uint CardNo, uint No);

        public delegate double n_get_serialDelegate(uint CardNo);

        public delegate double n_get_list_serialDelegate(uint CardNo, out uint SetNo);

        public delegate void time_updateDelegate();

        public delegate void time_control_ethDelegate(double PPM);

        public delegate void set_serial_stepDelegate(uint No, uint Step);

        public delegate void select_serial_setDelegate(uint No);

        public delegate void set_serialDelegate(uint No);

        public delegate double get_serialDelegate();

        public delegate double get_list_serialDelegate(out uint SetNo);

        public delegate void n_write_io_port_maskDelegate(uint CardNo, uint Value, uint Mask);

        public delegate void n_write_8bit_portDelegate(uint CardNo, uint Value);

        public delegate uint n_read_io_portDelegate(uint CardNo);

        public delegate uint n_read_io_port_bufferDelegate(
          uint CardNo,
          uint Index,
          out uint Value,
          out int XPos,
          out int YPos,
          out uint Time);

        public delegate uint n_get_io_statusDelegate(uint CardNo);

        public delegate uint n_read_analog_inDelegate(uint CardNo);

        public delegate void n_write_da_xDelegate(uint CardNo, uint x, uint Value);

        public delegate void n_set_laser_off_defaultDelegate(
          uint CardNo,
          uint AnalogOut1,
          uint AnalogOut2,
          uint DigitalOut);

        public delegate void n_set_port_defaultDelegate(uint CardNo, uint Port, uint Value);

        public delegate void n_write_io_portDelegate(uint CardNo, uint Value);

        public delegate void n_write_da_1Delegate(uint CardNo, uint Value);

        public delegate void n_write_da_2Delegate(uint CardNo, uint Value);

        public delegate void write_io_port_maskDelegate(uint Value, uint Mask);

        public delegate void write_8bit_portDelegate(uint Value);

        public delegate uint read_io_portDelegate();

        public delegate uint read_io_port_bufferDelegate(
          uint Index,
          out uint Value,
          out int XPos,
          out int YPos,
          out uint Time);

        public delegate uint get_io_statusDelegate();

        public delegate uint read_analog_inDelegate();

        public delegate void write_da_xDelegate(uint x, uint Value);

        public delegate void set_laser_off_defaultDelegate(
          uint AnalogOut1,
          uint AnalogOut2,
          uint DigitalOut);

        public delegate void set_port_defaultDelegate(uint Port, uint Value);

        public delegate void write_io_portDelegate(uint Value);

        public delegate void write_da_1Delegate(uint Value);

        public delegate void write_da_2Delegate(uint Value);

        public delegate void n_disable_laserDelegate(uint CardNo);

        public delegate void n_enable_laserDelegate(uint CardNo);

        public delegate void n_laser_signal_onDelegate(uint CardNo);

        public delegate void n_laser_signal_offDelegate(uint CardNo);

        public delegate void n_set_standbyDelegate(uint CardNo, uint HalfPeriod, uint PulseLength);

        public delegate void n_set_laser_pulses_ctrlDelegate(
          uint CardNo,
          uint HalfPeriod,
          uint PulseLength);

        public delegate void n_set_firstpulse_killerDelegate(uint CardNo, uint Length);

        public delegate void n_set_qswitch_delayDelegate(uint CardNo, uint Delay);

        public delegate void n_set_laser_modeDelegate(uint CardNo, uint Mode);

        public delegate void n_set_laser_controlDelegate(uint CardNo, uint Ctrl);

        public delegate void n_set_laser_pin_outDelegate(uint CardNo, uint Pins);

        public delegate uint n_get_laser_pin_inDelegate(uint CardNo);

        public delegate void n_set_softstart_levelDelegate(uint CardNo, uint Index, uint Level);

        public delegate void n_set_softstart_modeDelegate(
          uint CardNo,
          uint Mode,
          uint Number,
          uint Delay);

        public delegate uint n_set_auto_laser_controlDelegate(
          uint CardNo,
          uint Ctrl,
          uint Value,
          uint Mode,
          uint MinValue,
          uint MaxValue);

        public delegate uint n_set_auto_laser_paramsDelegate(
          uint CardNo,
          uint Ctrl,
          uint Value,
          uint MinValue,
          uint MaxValue);

        public delegate int n_load_auto_laser_controlDelegate(uint CardNo, string Name, uint No);

        public delegate int n_load_position_controlDelegate(uint CardNo, string Name, uint No);

        public delegate void n_set_default_pixelDelegate(uint CardNo, uint PulseLength);

        public delegate void n_get_standbyDelegate(
          uint CardNo,
          out uint HalfPeriod,
          out uint PulseLength);

        public delegate void n_set_pulse_pickingDelegate(uint CardNo, uint No);

        public delegate void n_set_pulse_picking_lengthDelegate(uint CardNo, uint Length);

        public delegate void n_config_laser_signalsDelegate(uint CardNo, uint Config);

        public delegate void n_set_laser_powerDelegate(uint CardNo, uint Port, uint Value);

        public delegate void n_set_port_default_listDelegate(uint CardNo, uint Port, uint Value);

        public delegate void n_spot_distance_ctrlDelegate(uint CardNo, double Dist);

        public delegate void disable_laserDelegate();

        public delegate void enable_laserDelegate();

        public delegate void laser_signal_onDelegate();

        public delegate void laser_signal_offDelegate();

        public delegate void set_standbyDelegate(uint HalfPeriod, uint PulseLength);

        public delegate void set_laser_pulses_ctrlDelegate(uint HalfPeriod, uint PulseLength);

        public delegate void set_firstpulse_killerDelegate(uint Length);

        public delegate void set_qswitch_delayDelegate(uint Delay);

        public delegate void set_laser_modeDelegate(uint Mode);

        public delegate void set_laser_controlDelegate(uint Ctrl);

        public delegate void set_laser_pin_outDelegate(uint Pins);

        public delegate uint get_laser_pin_inDelegate();

        public delegate void set_softstart_levelDelegate(uint Index, uint Level);

        public delegate void set_softstart_modeDelegate(uint Mode, uint Number, uint Delay);

        public delegate uint set_auto_laser_controlDelegate(
          uint Ctrl,
          uint Value,
          uint Mode,
          uint MinValue,
          uint MaxValue);

        public delegate uint set_auto_laser_paramsDelegate(
          uint Ctrl,
          uint Value,
          uint MinValue,
          uint MaxValue);

        public delegate int load_auto_laser_controlDelegate(string Name, uint No);

        public delegate int load_position_controlDelegate(string Name, uint No);

        public delegate void set_default_pixelDelegate(uint PulseLength);

        public delegate void get_standbyDelegate(out uint HalfPeriod, out uint PulseLength);

        public delegate void set_pulse_pickingDelegate(uint No);

        public delegate void set_pulse_picking_lengthDelegate(uint Length);

        public delegate void config_laser_signalsDelegate(uint Config);

        public delegate void set_laser_powerDelegate(uint Port, uint Value);

        public delegate void set_port_default_listDelegate(uint Port, uint Value);

        public delegate void spot_distance_ctrlDelegate(double Dist);

        public delegate void n_set_ext_start_delayDelegate(uint CardNo, int Delay, uint EncoderNo);

        public delegate void n_set_rot_centerDelegate(uint CardNo, int X, int Y);

        public delegate void n_simulate_encoderDelegate(uint CardNo, uint EncoderNo);

        public delegate uint n_get_marking_infoDelegate(uint CardNo);

        public delegate void n_set_encoder_speed_ctrlDelegate(
          uint CardNo,
          uint EncoderNo,
          double Speed,
          double Smooth);

        public delegate void n_set_mcbsp_xDelegate(uint CardNo, double ScaleX);

        public delegate void n_set_mcbsp_yDelegate(uint CardNo, double ScaleY);

        public delegate void n_set_mcbsp_rotDelegate(uint CardNo, double Resolution);

        public delegate void n_set_mcbsp_matrixDelegate(uint CardNo);

        public delegate void n_set_mcbsp_inDelegate(uint CardNo, uint Mode, double Scale);

        public delegate void n_set_multi_mcbsp_inDelegate(uint CardNo, uint Ctrl, uint P, uint Mode);

        public delegate void n_set_fly_tracking_errorDelegate(
          uint CardNo,
          uint TrackingErrorX,
          uint TrackingErrorY);

        public delegate int n_load_fly_2d_tableDelegate(uint CardNo, string Name, uint No);

        public delegate void n_init_fly_2dDelegate(uint CardNo, int OffsetX, int OffsetY, uint No);

        public delegate void n_get_fly_2d_offsetDelegate(uint CardNo, out int OffsetX, out int OffsetY);

        public delegate void n_get_encoderDelegate(uint CardNo, out int Encoder0, out int Encoder1);

        public delegate void n_read_encoderDelegate(
          uint CardNo,
          out int Encoder0_1,
          out int Encoder1_1,
          out int Encoder0_2,
          out int Encoder1_2);

        public delegate int n_get_mcbspDelegate(uint CardNo);

        public delegate int n_read_mcbspDelegate(uint CardNo, uint No);

        public delegate int n_read_multi_mcbspDelegate(uint CardNo, uint No);

        public delegate void set_ext_start_delayDelegate(int Delay, uint EncoderNo);

        public delegate void set_rot_centerDelegate(int X, int Y);

        public delegate void simulate_encoderDelegate(uint EncoderNo);

        public delegate uint get_marking_infoDelegate();

        public delegate void set_encoder_speed_ctrlDelegate(
          uint EncoderNo,
          double Speed,
          double Smooth);

        public delegate void set_mcbsp_xDelegate(double ScaleX);

        public delegate void set_mcbsp_yDelegate(double ScaleY);

        public delegate void set_mcbsp_rotDelegate(double Resolution);

        public delegate void set_mcbsp_matrixDelegate();

        public delegate void set_mcbsp_inDelegate(uint Mode, double Scale);

        public delegate void set_multi_mcbsp_inDelegate(uint Ctrl, uint P, uint Mode);

        public delegate void set_fly_tracking_errorDelegate(uint TrackingErrorX, uint TrackingErrorY);

        public delegate int load_fly_2d_tableDelegate(string Name, uint No);

        public delegate void init_fly_2dDelegate(int OffsetX, int OffsetY, uint No);

        public delegate void get_fly_2d_offsetDelegate(out int OffsetX, out int OffsetY);

        public delegate void get_encoderDelegate(out int Encoder0, out int Encoder1);

        public delegate void read_encoderDelegate(
          out int Encoder0_1,
          out int Encoder1_1,
          out int Encoder0_2,
          out int Encoder1_2);

        public delegate int get_mcbspDelegate();

        public delegate int read_mcbspDelegate(uint No);

        public delegate int read_multi_mcbspDelegate(uint No);

        public delegate double n_get_timeDelegate(uint CardNo);

        public delegate double n_get_lap_timeDelegate(uint CardNo);

        public delegate void n_measurement_statusDelegate(uint CardNo, out uint Busy, out uint Pos);

        public delegate void n_get_waveform_offsetDelegate(
          uint CardNo,
          uint Channel,
          uint Offset,
          uint Number,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 8388608)] int[] Ptr);

        public delegate void n_get_waveformDelegate(uint CardNo, uint Channel, uint Number, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8388608)] int[] Ptr);

        public delegate void n_bounce_suppDelegate(uint CardNo, uint Length);

        public delegate void n_home_position_4Delegate(
          uint CardNo,
          int X0Home,
          int X1Home,
          int X2Home,
          int X3Home);

        public delegate void n_get_home_position_4Delegate(
          uint CardNo,
          out int X0Home,
          out int X1Home,
          out int X2Home,
          out int X3Home);

        public delegate void n_set_home_4_return_timeDelegate(uint CardNo, uint Time);

        public delegate uint n_get_home_4_return_timeDelegate(uint CardNo);

        public delegate void n_home_position_xyzDelegate(uint CardNo, int XHome, int YHome, int ZHome);

        public delegate void n_home_positionDelegate(uint CardNo, int XHome, int YHome);

        public delegate uint n_uart_configDelegate(uint CardNo, uint BaudRate);

        public delegate void n_rs232_configDelegate(uint CardNo, uint BaudRate);

        public delegate void n_rs232_write_dataDelegate(uint CardNo, uint Data);

        public delegate void n_rs232_write_textDelegate(uint CardNo, string pData);

        public delegate uint n_rs232_read_dataDelegate(uint CardNo);

        public delegate uint n_set_mcbsp_freqDelegate(uint CardNo, uint Freq);

        public delegate void n_mcbsp_initDelegate(uint CardNo, uint XDelay, uint RDelay);

        public delegate void n_mcbsp_init_spiDelegate(uint CardNo, uint ClockLevel, uint ClockDelay);

        public delegate uint n_get_overrunDelegate(uint CardNo);

        public delegate uint n_get_master_slaveDelegate(uint CardNo);

        public delegate void n_get_transformDelegate(
          uint CardNo,
          uint Number,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 4194304)] int[] Ptr1,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 4194304)] int[] Ptr2,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 132130)] uint[] Ptr,
          uint Code);

        public delegate void n_stop_triggerDelegate(uint CardNo);

        public delegate void n_move_toDelegate(uint CardNo, uint Pos);

        public delegate void n_set_enduring_wobbelDelegate(
          uint CardNo,
          uint CenterX,
          uint CenterY,
          uint CenterZ,
          uint LimitHi,
          uint LimitLo,
          double ScaleX,
          double ScaleY,
          double ScaleZ);

        public delegate void n_set_enduring_wobbel_2Delegate(
          uint CardNo,
          uint CenterX,
          uint CenterY,
          uint CenterZ,
          uint LimitHi,
          uint LimitLo,
          double ScaleX,
          double ScaleY,
          double ScaleZ);

        public delegate void n_set_free_variableDelegate(uint CardNo, uint VarNo, uint Value);

        public delegate uint n_get_free_variableDelegate(uint CardNo, uint VarNo);

        public delegate void n_set_mcbsp_out_ptrDelegate(uint CardNo, uint Number, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] uint[] SignalPtr);

        public delegate void n_periodic_toggleDelegate(
          uint CardNo,
          uint Port,
          uint Mask,
          uint P1,
          uint P2,
          uint Count,
          uint Start);

        public delegate void n_multi_axis_configDelegate(uint CardNo, uint Cfg, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] Ptr);

        public delegate void n_quad_axis_initDelegate(uint CardNo, uint Idle, double X1, double Y1);

        public delegate uint n_quad_axis_get_statusDelegate(uint CardNo);

        public delegate void n_quad_axis_get_valuesDelegate(
          uint CardNo,
          out double X1,
          out double Y1,
          out uint Flags0,
          out uint Flags1);

        public delegate double get_timeDelegate();

        public delegate double get_lap_timeDelegate();

        public delegate void measurement_statusDelegate(out uint Busy, out uint Pos);

        public delegate void get_waveform_offsetDelegate(
          uint Channel,
          uint Offset,
          uint Number,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 8388608)] int[] Ptr);

        public delegate void get_waveformDelegate(uint Channel, uint Number, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8388608)] int[] Ptr);

        public delegate void bounce_suppDelegate(uint Length);

        public delegate void home_position_4Delegate(int X0Home, int X1Home, int X2Home, int X3Home);

        public delegate void get_home_position_4Delegate(
          out int X0Home,
          out int X1Home,
          out int X2Home,
          out int X3Home);

        public delegate void set_home_4_return_timeDelegate(uint Time);

        public delegate uint get_home_4_return_timeDelegate();

        public delegate void home_position_xyzDelegate(int XHome, int YHome, int ZHome);

        public delegate void home_positionDelegate(int XHome, int YHome);

        public delegate uint uart_configDelegate(uint BaudRate);

        public delegate void rs232_configDelegate(uint BaudRate);

        public delegate void rs232_write_dataDelegate(uint Data);

        public delegate void rs232_write_textDelegate(string pData);

        public delegate uint rs232_read_dataDelegate();

        public delegate uint set_mcbsp_freqDelegate(uint Freq);

        public delegate void mcbsp_initDelegate(uint XDelay, uint RDelay);

        public delegate void mcbsp_init_spiDelegate(uint ClockLevel, uint ClockDelay);

        public delegate uint get_overrunDelegate();

        public delegate uint get_master_slaveDelegate();

        public delegate void get_transformDelegate(
          uint Number,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 4194304)] int[] Ptr1,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 4194304)] int[] Ptr2,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 132130)] uint[] Ptr,
          uint Code);

        public delegate void stop_triggerDelegate();

        public delegate void move_toDelegate(uint Pos);

        public delegate void set_enduring_wobbelDelegate(
          uint CenterX,
          uint CenterY,
          uint CenterZ,
          uint LimitHi,
          uint LimitLo,
          double ScaleX,
          double ScaleY,
          double ScaleZ);

        public delegate void set_enduring_wobbel_2Delegate(
          uint CenterX,
          uint CenterY,
          uint CenterZ,
          uint LimitHi,
          uint LimitLo,
          double ScaleX,
          double ScaleY,
          double ScaleZ);

        public delegate void set_free_variableDelegate(uint VarNo, uint Value);

        public delegate uint get_free_variableDelegate(uint VarNo);

        public delegate void set_mcbsp_out_ptrDelegate(uint Number, [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] uint[] SignalPtr);

        public delegate void periodic_toggleDelegate(
          uint Port,
          uint Mask,
          uint P1,
          uint P2,
          uint Count,
          uint Start);

        public delegate void multi_axis_configDelegate(uint Cfg, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] Ptr);

        public delegate void quad_axis_initDelegate(uint Idle, double X1, double Y1);

        public delegate uint quad_axis_get_statusDelegate();

        public delegate void quad_axis_get_valuesDelegate(
          out double X1,
          out double Y1,
          out uint Flags0,
          out uint Flags1);

        public delegate void n_set_defocusDelegate(uint CardNo, int Shift);

        public delegate void n_set_defocus_offsetDelegate(uint CardNo, int Shift);

        public delegate void n_goto_xyzDelegate(uint CardNo, int X, int Y, int Z);

        public delegate void n_set_zoomDelegate(uint CardNo, uint Zoom);

        public delegate void n_goto_xyDelegate(uint CardNo, int X, int Y);

        public delegate int n_get_z_distanceDelegate(uint CardNo, int X, int Y, int Z);

        public delegate void set_defocusDelegate(int Shift);

        public delegate void set_defocus_offsetDelegate(int Shift);

        public delegate void goto_xyzDelegate(int X, int Y, int Z);

        public delegate void goto_xyDelegate(int X, int Y);

        public delegate void set_zoomDelegate(uint Zoom);

        public delegate int get_z_distanceDelegate(int X, int Y, int Z);

        public delegate void n_set_offset_xyzDelegate(
          uint CardNo,
          uint HeadNo,
          int XOffset,
          int YOffset,
          int ZOffset,
          uint at_once);

        public delegate void n_set_offsetDelegate(
          uint CardNo,
          uint HeadNo,
          int XOffset,
          int YOffset,
          uint at_once);

        public delegate void n_set_matrixDelegate(
          uint CardNo,
          uint HeadNo,
          double M11,
          double M12,
          double M21,
          double M22,
          uint at_once);

        public delegate void n_set_angleDelegate(uint CardNo, uint HeadNo, double Angle, uint at_once);

        public delegate void n_set_scaleDelegate(uint CardNo, uint HeadNo, double Scale, uint at_once);

        public delegate void n_apply_mcbspDelegate(uint CardNo, uint HeadNo, uint at_once);

        public delegate uint n_upload_transformDelegate(uint CardNo, uint HeadNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 132130)] uint[] Ptr);

        public delegate void set_offset_xyzDelegate(
          uint HeadNo,
          int XOffset,
          int YOffset,
          int ZOffset,
          uint at_once);

        public delegate void set_offsetDelegate(uint HeadNo, int XOffset, int YOffset, uint at_once);

        public delegate void set_matrixDelegate(
          uint HeadNo,
          double M11,
          double M12,
          double M21,
          double M22,
          uint at_once);

        public delegate void set_angleDelegate(uint HeadNo, double Angle, uint at_once);

        public delegate void set_scaleDelegate(uint HeadNo, double Scale, uint at_once);

        public delegate void apply_mcbspDelegate(uint HeadNo, uint at_once);

        public delegate uint upload_transformDelegate(uint HeadNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 132130)] uint[] Ptr);

        public delegate void n_set_delay_modeDelegate(
          uint CardNo,
          uint VarPoly,
          uint DirectMove3D,
          uint EdgeLevel,
          uint MinJumpDelay,
          uint JumpLengthLimit);

        public delegate void n_set_jump_speed_ctrlDelegate(uint CardNo, double Speed);

        public delegate void n_set_mark_speed_ctrlDelegate(uint CardNo, double Speed);

        public delegate void n_set_sky_writing_paraDelegate(
          uint CardNo,
          double Timelag,
          int LaserOnShift,
          uint Nprev,
          uint Npost);

        public delegate void n_set_sky_writing_limitDelegate(uint CardNo, double CosAngle);

        public delegate void n_set_sky_writing_modeDelegate(uint CardNo, uint Mode);

        public delegate int n_load_varpolydelayDelegate(uint CardNo, string Name, uint No);

        public delegate void n_set_hiDelegate(
          uint CardNo,
          uint HeadNo,
          double GalvoGainX,
          double GalvoGainY,
          int GalvoOffsetX,
          int GalvoOffsetY);

        public delegate void n_get_hi_posDelegate(
          uint CardNo,
          uint HeadNo,
          out int X1,
          out int X2,
          out int Y1,
          out int Y2);

        public delegate uint n_auto_calDelegate(uint CardNo, uint HeadNo, uint Command);

        public delegate uint n_get_auto_calDelegate(uint CardNo, uint HeadNo);

        public delegate uint n_write_hi_posDelegate(
          uint CardNo,
          uint HeadNo,
          int X1,
          int X2,
          int Y1,
          int Y2);

        public delegate void n_set_timelag_compensationDelegate(
          uint CardNo,
          uint HeadNo,
          uint TimeLagXY,
          uint TimeLagZ);

        public delegate void n_set_sky_writingDelegate(uint CardNo, double Timelag, int LaserOnShift);

        public delegate void n_get_hi_dataDelegate(
          uint CardNo,
          out int X1,
          out int X2,
          out int Y1,
          out int Y2);

        public delegate void set_delay_modeDelegate(
          uint VarPoly,
          uint DirectMove3D,
          uint EdgeLevel,
          uint MinJumpDelay,
          uint JumpLengthLimit);

        public delegate void set_jump_speed_ctrlDelegate(double Speed);

        public delegate void set_mark_speed_ctrlDelegate(double Speed);

        public delegate void set_sky_writing_paraDelegate(
          double Timelag,
          int LaserOnShift,
          uint Nprev,
          uint Npost);

        public delegate void set_sky_writing_limitDelegate(double CosAngle);

        public delegate void set_sky_writing_modeDelegate(uint Mode);

        public delegate int load_varpolydelayDelegate(string Name, uint No);

        public delegate void set_hiDelegate(
          uint HeadNo,
          double GalvoGainX,
          double GalvoGainY,
          int GalvoOffsetX,
          int GalvoOffsetY);

        public delegate void get_hi_posDelegate(
          uint HeadNo,
          out int X1,
          out int X2,
          out int Y1,
          out int Y2);

        public delegate uint auto_calDelegate(uint HeadNo, uint Command);

        public delegate uint get_auto_calDelegate(uint HeadNo);

        public delegate uint write_hi_posDelegate(uint HeadNo, int X1, int X2, int Y1, int Y2);

        public delegate void set_timelag_compensationDelegate(
          uint HeadNo,
          uint TimeLagXY,
          uint TimeLagZ);

        public delegate void set_sky_writingDelegate(double Timelag, int LaserOnShift);

        public delegate void get_hi_dataDelegate(out int X1, out int X2, out int Y1, out int Y2);

        public delegate void n_send_user_dataDelegate(
          uint CardNo,
          uint Head,
          uint Axis,
          int Data0,
          int Data1,
          int Data2,
          int Data3,
          int Data4);

        public delegate int n_read_user_dataDelegate(
          uint CardNo,
          uint Head,
          uint Axis,
          out int Data0,
          out int Data1,
          out int Data2,
          out int Data3,
          out int Data4);

        public delegate void n_control_commandDelegate(uint CardNo, uint Head, uint Axis, uint Data);

        public delegate int n_get_valueDelegate(uint CardNo, uint Signal);

        public delegate void n_get_valuesDelegate(uint CardNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] SignalPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] int[] ResultPtr);

        public delegate void n_get_galvo_controlsDelegate(
          uint CardNo,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] SignalPtr,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] int[] ResultPtr);

        public delegate uint n_get_head_statusDelegate(uint CardNo, uint Head);

        public delegate int n_set_jump_modeDelegate(
          uint CardNo,
          int Flag,
          uint Length,
          int VA1,
          int VA2,
          int VB1,
          int VB2,
          int JA1,
          int JA2,
          int JB1,
          int JB2);

        public delegate int n_load_jump_table_offsetDelegate(
          uint CardNo,
          string Name,
          uint No,
          uint PosAck,
          int Offset,
          uint MinDelay,
          uint MaxDelay,
          uint ListPos);

        public delegate uint n_get_jump_tableDelegate(uint CardNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] ushort[] Ptr);

        public delegate uint n_set_jump_tableDelegate(uint CardNo, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] ushort[] Ptr);

        public delegate int n_load_jump_tableDelegate(
          uint CardNo,
          string Name,
          uint No,
          uint PosAck,
          uint MinDelay,
          uint MaxDelay,
          uint ListPos);

        public delegate void send_user_dataDelegate(
          uint Head,
          uint Axis,
          int Data0,
          int Data1,
          int Data2,
          int Data3,
          int Data4);

        public delegate int read_user_dataDelegate(
          uint Head,
          uint Axis,
          out int Data0,
          out int Data1,
          out int Data2,
          out int Data3,
          out int Data4);

        public delegate void control_commandDelegate(uint Head, uint Axis, uint Data);

        public delegate int get_valueDelegate(uint Signal);

        public delegate void get_valuesDelegate([MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] uint[] SignalPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] int[] ResultPtr);

        public delegate void get_galvo_controlsDelegate([MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] SignalPtr, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)] int[] ResultPtr);

        public delegate uint get_head_statusDelegate(uint Head);

        public delegate int set_jump_modeDelegate(
          int Flag,
          uint Length,
          int VA1,
          int VA2,
          int VB1,
          int VB2,
          int JA1,
          int JA2,
          int JB1,
          int JB2);

        public delegate int load_jump_table_offsetDelegate(
          string Name,
          uint No,
          uint PosAck,
          int Offset,
          uint MinDelay,
          uint MaxDelay,
          uint ListPos);

        public delegate uint get_jump_tableDelegate([MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] ushort[] Ptr);

        public delegate uint set_jump_tableDelegate([MarshalAs(UnmanagedType.LPArray, SizeConst = 1024)] ushort[] Ptr);

        public delegate int load_jump_tableDelegate(
          string Name,
          uint No,
          uint PosAck,
          uint MinDelay,
          uint MaxDelay,
          uint ListPos);

        public delegate uint n_get_scanahead_paramsDelegate(
          uint CardNo,
          uint HeadNo,
          out uint PreViewTime,
          out uint Vmax,
          out double Amax);

        public delegate int n_activate_scanahead_autodelaysDelegate(uint CardNo, int Mode);

        public delegate void n_set_scanahead_laser_shiftsDelegate(uint CardNo, int dLasOn, int dLasOff);

        public delegate void n_set_scanahead_line_paramsDelegate(
          uint CardNo,
          uint CornerScale,
          uint EndScale,
          uint AccScale);

        public delegate void n_set_scanahead_line_params_exDelegate(
          uint CardNo,
          uint CornerScale,
          uint EndScale,
          uint AccScale,
          uint JumpScale);

        public delegate uint n_set_scanahead_paramsDelegate(
          uint CardNo,
          uint Mode,
          uint HeadNo,
          uint TableNo,
          uint PreViewTime,
          uint Vmax,
          double Amax);

        public delegate void n_set_scanahead_speed_controlDelegate(uint CardNo, uint Mode);

        public delegate uint get_scanahead_paramsDelegate(
          uint HeadNo,
          out uint PreViewTime,
          out uint Vmax,
          out double Amax);

        public delegate int activate_scanahead_autodelaysDelegate(int Mode);

        public delegate void set_scanahead_laser_shiftsDelegate(int dLasOn, int dLasOff);

        public delegate void set_scanahead_line_paramsDelegate(
          uint CornerScale,
          uint EndScale,
          uint AccScale);

        public delegate void set_scanahead_line_params_exDelegate(
          uint CornerScale,
          uint EndScale,
          uint AccScale,
          uint JumpScale);

        public delegate uint set_scanahead_paramsDelegate(
          uint Mode,
          uint HeadNo,
          uint TableNo,
          uint PreViewTime,
          uint Vmax,
          double Amax);

        public delegate void set_scanahead_speed_controlDelegate(uint Mode);

        public delegate void n_stepper_initDelegate(
          uint CardNo,
          uint No,
          uint Period,
          int Dir,
          int Pos,
          uint Tol,
          uint Enable,
          uint WaitTime);

        public delegate void n_stepper_enableDelegate(uint CardNo, int Enable1, int Enable2);

        public delegate void n_stepper_disable_switchDelegate(uint CardNo, int Disable1, int Disable2);

        public delegate void n_stepper_controlDelegate(uint CardNo, int Period1, int Period2);

        public delegate void n_stepper_abs_noDelegate(uint CardNo, uint No, int Pos, uint WaitTime);

        public delegate void n_stepper_rel_noDelegate(uint CardNo, uint No, int dPos, uint WaitTime);

        public delegate void n_stepper_absDelegate(uint CardNo, int Pos1, int Pos2, uint WaitTime);

        public delegate void n_stepper_relDelegate(uint CardNo, int dPos1, int dPos2, uint WaitTime);

        public delegate void n_get_stepper_statusDelegate(
          uint CardNo,
          out uint Status1,
          out int Pos1,
          out uint Status2,
          out int Pos2);

        public delegate void stepper_initDelegate(
          uint No,
          uint Period,
          int Dir,
          int Pos,
          uint Tol,
          uint Enable,
          uint WaitTime);

        public delegate void stepper_enableDelegate(int Enable1, int Enable2);

        public delegate void stepper_disable_switchDelegate(int Disable1, int Disable2);

        public delegate void stepper_controlDelegate(int Period1, int Period2);

        public delegate void stepper_abs_noDelegate(uint No, int Pos, uint WaitTime);

        public delegate void stepper_rel_noDelegate(uint No, int dPos, uint WaitTime);

        public delegate void stepper_absDelegate(int Pos1, int Pos2, uint WaitTime);

        public delegate void stepper_relDelegate(int dPos1, int dPos2, uint WaitTime);

        public delegate void get_stepper_statusDelegate(
          out uint Status1,
          out int Pos1,
          out uint Status2,
          out int Pos2);

        public delegate void n_select_cor_table_listDelegate(uint CardNo, uint HeadA, uint HeadB);

        public delegate void select_cor_table_listDelegate(uint HeadA, uint HeadB);

        public delegate void n_list_nopDelegate(uint CardNo);

        public delegate void n_list_continueDelegate(uint CardNo);

        public delegate void n_list_nextDelegate(uint CardNo);

        public delegate void n_long_delayDelegate(uint CardNo, uint Delay);

        public delegate void n_set_end_of_listDelegate(uint CardNo);

        public delegate void n_set_waitDelegate(uint CardNo, uint WaitWord);

        public delegate void n_list_jump_posDelegate(uint CardNo, uint Pos);

        public delegate void n_list_jump_relDelegate(uint CardNo, int Pos);

        public delegate void n_list_repeatDelegate(uint CardNo);

        public delegate void n_list_untilDelegate(uint CardNo, uint Number);

        public delegate void n_range_checkingDelegate(uint CardNo, uint HeadNo, uint Mode, uint Data);

        public delegate void n_set_list_jumpDelegate(uint CardNo, uint Pos);

        public delegate void list_nopDelegate();

        public delegate void list_continueDelegate();

        public delegate void list_nextDelegate();

        public delegate void long_delayDelegate(uint Delay);

        public delegate void set_end_of_listDelegate();

        public delegate void set_waitDelegate(uint WaitWord);

        public delegate void list_jump_posDelegate(uint Pos);

        public delegate void list_jump_relDelegate(int Pos);

        public delegate void list_repeatDelegate();

        public delegate void list_untilDelegate(uint Number);

        public delegate void range_checkingDelegate(uint HeadNo, uint Mode, uint Data);

        public delegate void set_list_jumpDelegate(uint Pos);

        public delegate void n_set_extstartpos_listDelegate(uint CardNo, uint Pos);

        public delegate void n_set_control_mode_listDelegate(uint CardNo, uint Mode);

        public delegate void n_simulate_ext_startDelegate(uint CardNo, int Delay, uint EncoderNo);

        public delegate void set_extstartpos_listDelegate(uint Pos);

        public delegate void set_control_mode_listDelegate(uint Mode);

        public delegate void simulate_ext_startDelegate(int Delay, uint EncoderNo);

        public delegate void n_list_returnDelegate(uint CardNo);

        public delegate void n_list_call_repeatDelegate(uint CardNo, uint Pos, uint Number);

        public delegate void n_list_call_abs_repeatDelegate(uint CardNo, uint Pos, uint Number);

        public delegate void n_list_callDelegate(uint CardNo, uint Pos);

        public delegate void n_list_call_absDelegate(uint CardNo, uint Pos);

        public delegate void n_sub_call_repeatDelegate(uint CardNo, uint Index, uint Number);

        public delegate void n_sub_call_abs_repeatDelegate(uint CardNo, uint Index, uint Number);

        public delegate void n_sub_callDelegate(uint CardNo, uint Index);

        public delegate void n_sub_call_absDelegate(uint CardNo, uint Index);

        public delegate void list_returnDelegate();

        public delegate void list_call_repeatDelegate(uint Pos, uint Number);

        public delegate void list_call_abs_repeatDelegate(uint Pos, uint Number);

        public delegate void list_callDelegate(uint Pos);

        public delegate void list_call_absDelegate(uint Pos);

        public delegate void sub_call_repeatDelegate(uint Index, uint Number);

        public delegate void sub_call_abs_repeatDelegate(uint Index, uint Number);

        public delegate void sub_callDelegate(uint Index);

        public delegate void sub_call_absDelegate(uint Index);

        public delegate void n_list_call_condDelegate(uint CardNo, uint Mask1, uint Mask0, uint Pos);

        public delegate void n_list_call_abs_condDelegate(
          uint CardNo,
          uint Mask1,
          uint Mask0,
          uint Pos);

        public delegate void n_sub_call_condDelegate(uint CardNo, uint Mask1, uint Mask0, uint Pos);

        public delegate void n_sub_call_abs_condDelegate(
          uint CardNo,
          uint Mask1,
          uint Mask0,
          uint Pos);

        public delegate void n_list_jump_pos_condDelegate(
          uint CardNo,
          uint Mask1,
          uint Mask0,
          uint Index);

        public delegate void n_list_jump_rel_condDelegate(
          uint CardNo,
          uint Mask1,
          uint Mask0,
          int Index);

        public delegate void n_if_condDelegate(uint CardNo, uint Mask1, uint Mask0);

        public delegate void n_if_not_condDelegate(uint CardNo, uint Mask1, uint Mask0);

        public delegate void n_if_pin_condDelegate(uint CardNo, uint Mask1, uint Mask0);

        public delegate void n_if_not_pin_condDelegate(uint CardNo, uint Mask1, uint Mask0);

        public delegate void n_switch_ioportDelegate(uint CardNo, uint MaskBits, uint ShiftBits);

        public delegate void n_list_jump_condDelegate(uint CardNo, uint Mask1, uint Mask0, uint Pos);

        public delegate void list_call_condDelegate(uint Mask1, uint Mask0, uint Pos);

        public delegate void list_call_abs_condDelegate(uint Mask1, uint Mask0, uint Pos);

        public delegate void sub_call_condDelegate(uint Mask1, uint Mask0, uint Index);

        public delegate void sub_call_abs_condDelegate(uint Mask1, uint Mask0, uint Index);

        public delegate void list_jump_pos_condDelegate(uint Mask1, uint Mask0, uint Pos);

        public delegate void list_jump_rel_condDelegate(uint Mask1, uint Mask0, int Pos);

        public delegate void if_condDelegate(uint Mask1, uint Mask0);

        public delegate void if_not_condDelegate(uint Mask1, uint Mask0);

        public delegate void if_pin_condDelegate(uint Mask1, uint Mask0);

        public delegate void if_not_pin_condDelegate(uint Mask1, uint Mask0);

        public delegate void switch_ioportDelegate(uint MaskBits, uint ShiftBits);

        public delegate void list_jump_condDelegate(uint Mask1, uint Mask0, uint Pos);

        public delegate void n_select_char_setDelegate(uint CardNo, uint No);

        public delegate void n_mark_textDelegate(uint CardNo, string Text);

        public delegate void n_mark_text_absDelegate(uint CardNo, string Text);

        public delegate void n_mark_charDelegate(uint CardNo, uint Char);

        public delegate void n_mark_char_absDelegate(uint CardNo, uint Char);

        public delegate void select_char_setDelegate(uint No);

        public delegate void mark_textDelegate(string Text);

        public delegate void mark_text_absDelegate(string Text);

        public delegate void mark_charDelegate(uint Char);

        public delegate void mark_char_absDelegate(uint Char);

        public delegate void n_mark_serialDelegate(uint CardNo, uint Mode, uint Digits);

        public delegate void n_mark_serial_absDelegate(uint CardNo, uint Mode, uint Digits);

        public delegate void n_mark_dateDelegate(uint CardNo, uint Part, uint Mode);

        public delegate void n_mark_date_absDelegate(uint CardNo, uint Part, uint Mode);

        public delegate void n_mark_timeDelegate(uint CardNo, uint Part, uint Mode);

        public delegate void n_mark_time_absDelegate(uint CardNo, uint Part, uint Mode);

        public delegate void n_select_serial_set_listDelegate(uint CardNo, uint No);

        public delegate void n_set_serial_step_listDelegate(uint CardNo, uint No, uint Step);

        public delegate void n_time_fix_f_offDelegate(uint CardNo, uint FirstDay, uint Offset);

        public delegate void n_time_fix_fDelegate(uint CardNo, uint FirstDay);

        public delegate void n_time_fixDelegate(uint CardNo);

        public delegate void mark_serialDelegate(uint Mode, uint Digits);

        public delegate void mark_serial_absDelegate(uint Mode, uint Digits);

        public delegate void mark_dateDelegate(uint Part, uint Mode);

        public delegate void mark_date_absDelegate(uint Part, uint Mode);

        public delegate void mark_timeDelegate(uint Part, uint Mode);

        public delegate void mark_time_absDelegate(uint Part, uint Mode);

        public delegate void time_fix_f_offDelegate(uint FirstDay, uint Offset);

        public delegate void select_serial_set_listDelegate(uint No);

        public delegate void set_serial_step_listDelegate(uint No, uint Step);

        public delegate void time_fix_fDelegate(uint FirstDay);

        public delegate void time_fixDelegate();

        public delegate void n_clear_io_cond_listDelegate(
          uint CardNo,
          uint Mask1,
          uint Mask0,
          uint Mask);

        public delegate void n_set_io_cond_listDelegate(
          uint CardNo,
          uint Mask1,
          uint Mask0,
          uint Mask);

        public delegate void n_write_io_port_mask_listDelegate(uint CardNo, uint Value, uint Mask);

        public delegate void n_write_8bit_port_listDelegate(uint CardNo, uint Value);

        public delegate void n_read_io_port_listDelegate(uint CardNo);

        public delegate void n_write_da_x_listDelegate(uint CardNo, uint x, uint Value);

        public delegate void n_write_io_port_listDelegate(uint CardNo, uint Value);

        public delegate void n_write_da_1_listDelegate(uint CardNo, uint Value);

        public delegate void n_write_da_2_listDelegate(uint CardNo, uint Value);

        public delegate void clear_io_cond_listDelegate(uint Mask1, uint Mask0, uint MaskClear);

        public delegate void set_io_cond_listDelegate(uint Mask1, uint Mask0, uint MaskSet);

        public delegate void write_io_port_mask_listDelegate(uint Value, uint Mask);

        public delegate void write_8bit_port_listDelegate(uint Value);

        public delegate void read_io_port_listDelegate();

        public delegate void write_da_x_listDelegate(uint x, uint Value);

        public delegate void write_io_port_listDelegate(uint Value);

        public delegate void write_da_1_listDelegate(uint Value);

        public delegate void write_da_2_listDelegate(uint Value);

        public delegate void n_laser_signal_on_listDelegate(uint CardNo);

        public delegate void n_laser_signal_off_listDelegate(uint CardNo);

        public delegate void n_para_laser_on_pulses_listDelegate(
          uint CardNo,
          uint Period,
          uint Pulses,
          uint P);

        public delegate void n_laser_on_pulses_listDelegate(uint CardNo, uint Period, uint Pulses);

        public delegate void n_laser_on_listDelegate(uint CardNo, uint Period);

        public delegate void n_set_laser_delaysDelegate(
          uint CardNo,
          int LaserOnDelay,
          uint LaserOffDelay);

        public delegate void n_set_standby_listDelegate(uint CardNo, uint HalfPeriod, uint PulseLength);

        public delegate void n_set_laser_pulsesDelegate(uint CardNo, uint HalfPeriod, uint PulseLength);

        public delegate void n_set_firstpulse_killer_listDelegate(uint CardNo, uint Length);

        public delegate void n_set_qswitch_delay_listDelegate(uint CardNo, uint Delay);

        public delegate void n_set_laser_pin_out_listDelegate(uint CardNo, uint Pins);

        public delegate void n_set_vector_controlDelegate(uint CardNo, uint Ctrl, uint Value);

        public delegate void n_set_default_pixel_listDelegate(uint CardNo, uint PulseLength);

        public delegate void n_set_auto_laser_params_listDelegate(
          uint CardNo,
          uint Ctrl,
          uint Value,
          uint MinValue,
          uint MaxValue);

        public delegate void n_set_pulse_picking_listDelegate(uint CardNo, uint No);

        public delegate void n_set_softstart_level_listDelegate(
          uint CardNo,
          uint Index,
          uint Level1,
          uint Level2,
          uint Level3);

        public delegate void n_set_softstart_mode_listDelegate(
          uint CardNo,
          uint Mode,
          uint Number,
          uint Delay);

        public delegate void n_config_laser_signals_listDelegate(uint CardNo, uint Config);

        public delegate void n_spot_distanceDelegate(uint CardNo, double Dist);

        public delegate void n_set_laser_timingDelegate(
          uint CardNo,
          uint HalfPeriod,
          uint PulseLength1,
          uint PulseLength2,
          uint TimeBase);

        public delegate void laser_signal_on_listDelegate();

        public delegate void laser_signal_off_listDelegate();

        public delegate void para_laser_on_pulses_listDelegate(uint Period, uint Pulses, uint P);

        public delegate void laser_on_pulses_listDelegate(uint Period, uint Pulses);

        public delegate void laser_on_listDelegate(uint Period);

        public delegate void set_laser_delaysDelegate(int LaserOnDelay, uint LaserOffDelay);

        public delegate void set_standby_listDelegate(uint HalfPeriod, uint PulseLength);

        public delegate void set_laser_pulsesDelegate(uint HalfPeriod, uint PulseLength);

        public delegate void set_firstpulse_killer_listDelegate(uint Length);

        public delegate void set_qswitch_delay_listDelegate(uint Delay);

        public delegate void set_laser_pin_out_listDelegate(uint Pins);

        public delegate void set_vector_controlDelegate(uint Ctrl, uint Value);

        public delegate void set_default_pixel_listDelegate(uint PulseLength);

        public delegate void set_auto_laser_params_listDelegate(
          uint Ctrl,
          uint Value,
          uint MinValue,
          uint MaxValue);

        public delegate void set_pulse_picking_listDelegate(uint No);

        public delegate void set_softstart_level_listDelegate(
          uint Index,
          uint Level1,
          uint Level2,
          uint Level3);

        public delegate void set_softstart_mode_listDelegate(uint Mode, uint Number, uint Delay);

        public delegate void config_laser_signals_listDelegate(uint Config);

        public delegate void spot_distanceDelegate(double Dist);

        public delegate void set_laser_timingDelegate(
          uint HalfPeriod,
          uint PulseLength1,
          uint PulseLength2,
          uint TimeBase);

        public delegate void n_fly_return_zDelegate(uint CardNo, int X, int Y, int Z);

        public delegate void n_fly_returnDelegate(uint CardNo, int X, int Y);

        public delegate void n_set_rot_center_listDelegate(uint CardNo, int X, int Y);

        public delegate void n_set_ext_start_delay_listDelegate(uint CardNo, int Delay, uint EncoderNo);

        public delegate void n_set_fly_xDelegate(uint CardNo, double ScaleX);

        public delegate void n_set_fly_yDelegate(uint CardNo, double ScaleY);

        public delegate void n_set_fly_zDelegate(uint CardNo, double ScaleZ, uint EndoderNo);

        public delegate void n_set_fly_rotDelegate(uint CardNo, double Resolution);

        public delegate void n_set_fly_2dDelegate(uint CardNo, double ScaleX, double ScaleY);

        public delegate void n_set_fly_x_posDelegate(uint CardNo, double ScaleX);

        public delegate void n_set_fly_y_posDelegate(uint CardNo, double ScaleY);

        public delegate void n_set_fly_rot_posDelegate(uint CardNo, double Resolution);

        public delegate void n_set_fly_limitsDelegate(
          uint CardNo,
          int Xmin,
          int Xmax,
          int Ymin,
          int Ymax);

        public delegate void n_set_fly_limits_zDelegate(uint CardNo, int Zmin, int Zmax);

        public delegate void n_if_fly_x_overflowDelegate(uint CardNo, int Mode);

        public delegate void n_if_fly_y_overflowDelegate(uint CardNo, int Mode);

        public delegate void n_if_fly_z_overflowDelegate(uint CardNo, int Mode);

        public delegate void n_if_not_fly_x_overflowDelegate(uint CardNo, int Mode);

        public delegate void n_if_not_fly_y_overflowDelegate(uint CardNo, int Mode);

        public delegate void n_if_not_fly_z_overflowDelegate(uint CardNo, int Mode);

        public delegate void n_clear_fly_overflowDelegate(uint CardNo, uint Mode);

        public delegate void n_set_mcbsp_x_listDelegate(uint CardNo, double ScaleX);

        public delegate void n_set_mcbsp_y_listDelegate(uint CardNo, double ScaleY);

        public delegate void n_set_mcbsp_rot_listDelegate(uint CardNo, double Resolution);

        public delegate void n_set_mcbsp_matrix_listDelegate(uint CardNo);

        public delegate void n_set_mcbsp_in_listDelegate(uint CardNo, uint Mode, double Scale);

        public delegate void n_set_multi_mcbsp_in_listDelegate(
          uint CardNo,
          uint Ctrl,
          uint P,
          uint Mode);

        public delegate void n_wait_for_encoder_modeDelegate(
          uint CardNo,
          int Value,
          uint EncoderNo,
          int Mode);

        public delegate void n_wait_for_mcbspDelegate(uint CardNo, uint Axis, int Value, int Mode);

        public delegate void n_set_encoder_speedDelegate(
          uint CardNo,
          uint EncoderNo,
          double Speed,
          double Smooth);

        public delegate void n_get_mcbsp_listDelegate(uint CardNo);

        public delegate void n_store_encoderDelegate(uint CardNo, uint Pos);

        public delegate void n_wait_for_encoder_in_range_modeDelegate(
          uint CardNo,
          int EncXmin,
          int EncXmax,
          int EncYmin,
          int EncYmax,
          uint Mode);

        public delegate void n_wait_for_encoder_in_rangeDelegate(
          uint CardNo,
          int EncXmin,
          int EncXmax,
          int EncYmin,
          int EncYmax);

        public delegate void n_activate_fly_xyDelegate(uint CardNo, double ScaleX, double ScaleY);

        public delegate void n_activate_fly_2dDelegate(uint CardNo, double ScaleX, double ScaleY);

        public delegate void n_activate_fly_xy_encoderDelegate(
          uint CardNo,
          double ScaleX,
          double ScaleY,
          int EncX,
          int EncY);

        public delegate void n_activate_fly_2d_encoderDelegate(
          uint CardNo,
          double ScaleX,
          double ScaleY,
          int EncX,
          int EncY);

        public delegate void n_if_not_activatedDelegate(uint CardNo);

        public delegate void n_park_positionDelegate(uint CardNo, uint Mode, int X, int Y);

        public delegate void n_park_returnDelegate(uint CardNo, uint Mode, int X, int Y);

        public delegate void n_fly_predictionDelegate(uint CardNo, uint PredictionX, uint PredictionY);

        public delegate void n_wait_for_encoderDelegate(uint CardNo, int Value, uint EncoderNo);

        public delegate void fly_return_zDelegate(int X, int Y, int Z);

        public delegate void fly_returnDelegate(int X, int Y);

        public delegate void set_rot_center_listDelegate(int X, int Y);

        public delegate void set_ext_start_delay_listDelegate(int Delay, uint EncoderNo);

        public delegate void set_fly_xDelegate(double ScaleX);

        public delegate void set_fly_yDelegate(double ScaleY);

        public delegate void set_fly_zDelegate(double ScaleZ, uint EncoderNo);

        public delegate void set_fly_rotDelegate(double Resolution);

        public delegate void set_fly_2dDelegate(double ScaleX, double ScaleY);

        public delegate void set_fly_x_posDelegate(double ScaleX);

        public delegate void set_fly_y_posDelegate(double ScaleY);

        public delegate void set_fly_rot_posDelegate(double Resolution);

        public delegate void set_fly_limitsDelegate(int Xmin, int Xmax, int Ymin, int Ymax);

        public delegate void set_fly_limits_zDelegate(int Zmin, int Zmax);

        public delegate void if_fly_x_overflowDelegate(int Mode);

        public delegate void if_fly_y_overflowDelegate(int Mode);

        public delegate void if_fly_z_overflowDelegate(int Mode);

        public delegate void if_not_fly_x_overflowDelegate(int Mode);

        public delegate void if_not_fly_y_overflowDelegate(int Mode);

        public delegate void if_not_fly_z_overflowDelegate(int Mode);

        public delegate void clear_fly_overflowDelegate(uint Mode);

        public delegate void set_mcbsp_x_listDelegate(double ScaleX);

        public delegate void set_mcbsp_y_listDelegate(double ScaleY);

        public delegate void set_mcbsp_rot_listDelegate(double Resolution);

        public delegate void set_mcbsp_matrix_listDelegate();

        public delegate void set_mcbsp_in_listDelegate(uint Mode, double Scale);

        public delegate void set_multi_mcbsp_in_listDelegate(uint Ctrl, uint P, uint Mode);

        public delegate void wait_for_encoder_modeDelegate(int Value, uint EncoderNo, int Mode);

        public delegate void wait_for_mcbspDelegate(uint Axis, int Value, int Mode);

        public delegate void set_encoder_speedDelegate(uint EncoderNo, double Speed, double Smooth);

        public delegate void get_mcbsp_listDelegate();

        public delegate void store_encoderDelegate(uint Pos);

        public delegate void wait_for_encoder_in_range_modeDelegate(
          int EncXmin,
          int EncXmax,
          int EncYmin,
          int EncYmax,
          uint Mode);

        public delegate void wait_for_encoder_in_rangeDelegate(
          int EncXmin,
          int EncXmax,
          int EncYmin,
          int EncYmax);

        public delegate void activate_fly_xyDelegate(double ScaleX, double ScaleY);

        public delegate void activate_fly_2dDelegate(double ScaleX, double ScaleY);

        public delegate void activate_fly_xy_encoderDelegate(
          double ScaleX,
          double ScaleY,
          int EncX,
          int EncY);

        public delegate void activate_fly_2d_encoderDelegate(
          double ScaleX,
          double ScaleY,
          int EncX,
          int EncY);

        public delegate void if_not_activatedDelegate();

        public delegate void park_positionDelegate(uint Mode, int X, int Y);

        public delegate void park_returnDelegate(uint Mode, int X, int Y);

        public delegate void fly_predictionDelegate(uint PredictionX, uint PredictionY);

        public delegate void wait_for_encoderDelegate(int Value, uint EncoderNo);

        public delegate void n_save_and_restart_timerDelegate(uint CardNo);

        public delegate void n_set_wobbel_mode_phaseDelegate(
          uint CardNo,
          uint Transversal,
          uint Longitudinal,
          double Freq,
          int Mode,
          double Phase);

        public delegate void n_set_wobbel_modeDelegate(
          uint CardNo,
          uint Transversal,
          uint Longitudinal,
          double Freq,
          int Mode);

        public delegate void n_set_wobbelDelegate(
          uint CardNo,
          uint Transversal,
          uint Longitudinal,
          double Freq);

        public delegate void n_set_wobbel_directionDelegate(uint CardNo, int dX, int dY);

        public delegate void n_set_wobbel_controlDelegate(
          uint CardNo,
          uint Ctrl,
          uint Value,
          uint MinValue,
          uint MaxValue);

        public delegate void n_set_wobbel_vectorDelegate(
          uint CardNo,
          double dTrans,
          double dLong,
          uint Period,
          double dPower);

        public delegate void n_set_wobbel_offsetDelegate(uint CardNo, int OffsetTrans, int OffsetLong);

        public delegate void n_set_triggerDelegate(
          uint CardNo,
          uint Period,
          uint Signal1,
          uint Signal2);

        public delegate void n_set_trigger4Delegate(
          uint CardNo,
          uint Period,
          uint Signal1,
          uint Signal2,
          uint Signal3,
          uint Signal4);

        public delegate void n_set_pixel_line_3dDelegate(
          uint CardNo,
          uint Channel,
          uint HalfPeriod,
          double dX,
          double dY,
          double dZ);

        public delegate void n_set_pixel_lineDelegate(
          uint CardNo,
          uint Channel,
          uint HalfPeriod,
          double dX,
          double dY);

        public delegate void n_set_n_pixelDelegate(
          uint CardNo,
          uint PortOutValue1,
          uint PortOutValue2,
          uint Number);

        public delegate void n_set_pixelDelegate(uint CardNo, uint PortOutValue1, uint PortOutValue2);

        public delegate void n_rs232_write_text_listDelegate(uint CardNo, string pData);

        public delegate void n_set_mcbsp_outDelegate(uint CardNo, uint Signal1, uint Signal2);

        public delegate void n_cammingDelegate(
          uint CardNo,
          uint FirstPos,
          uint NPos,
          uint No,
          uint Ctrl,
          double Scale,
          uint Code);

        public delegate void n_periodic_toggle_listDelegate(
          uint CardNo,
          uint Port,
          uint Mask,
          uint P1,
          uint P2,
          uint Count,
          uint Start);

        public delegate void n_micro_vector_abs_3dDelegate(
          uint CardNo,
          int X,
          int Y,
          int Z,
          int LasOn,
          int LasOff);

        public delegate void n_micro_vector_rel_3dDelegate(
          uint CardNo,
          int dX,
          int dY,
          int dZ,
          int LasOn,
          int LasOff);

        public delegate void n_micro_vector_absDelegate(
          uint CardNo,
          int X,
          int Y,
          int LasOn,
          int LasOff);

        public delegate void n_micro_vector_relDelegate(
          uint CardNo,
          int dX,
          int dY,
          int LasOn,
          int LasOff);

        public delegate void n_micro_vector_quad_axis_v_2Delegate(
          uint CardNo,
          int X0,
          int Y0,
          int X1,
          int Y1,
          int LasOn,
          int LasOff,
          uint Power,
          uint Port,
          uint Flags,
          double Velocity);

        public delegate void n_micro_vector_quad_axis_vDelegate(
          uint CardNo,
          int X0,
          int Y0,
          double X1,
          double Y1,
          int LasOn,
          int LasOff,
          uint Power,
          uint Port,
          uint Flags,
          double Velocity);

        public delegate void n_micro_vector_quad_axisDelegate(
          uint CardNo,
          int X0,
          int Y0,
          double X1,
          double Y1,
          int LasOn,
          int LasOff,
          uint Power,
          uint Port,
          uint Flags);

        public delegate void n_micro_vector_set_positionDelegate(
          uint CardNo,
          int X0,
          int X1,
          int X2,
          int X3,
          int LasOn,
          int LasOff);

        public delegate void n_multi_axis_flagsDelegate(uint CardNo, uint Flags);

        public delegate void n_set_free_variable_listDelegate(uint CardNo, uint VarNo, uint Value);

        public delegate void n_jump_abs_drill_2Delegate(
          uint CardNo,
          int X,
          int Y,
          uint DrillTime,
          int XOff,
          int YOff);

        public delegate void n_jump_rel_drill_2Delegate(
          uint CardNo,
          int dX,
          int dY,
          uint DrillTime,
          int XOff,
          int YOff);

        public delegate void n_jump_abs_drillDelegate(uint CardNo, int X, int Y, uint DrillTime);

        public delegate void n_jump_rel_drillDelegate(uint CardNo, int dX, int dY, uint DrillTime);

        public delegate void save_and_restart_timerDelegate();

        public delegate void set_wobbel_mode_phaseDelegate(
          uint Transversal,
          uint Longitudinal,
          double Freq,
          int Mode,
          double Phase);

        public delegate void set_wobbel_modeDelegate(
          uint Transversal,
          uint Longitudinal,
          double Freq,
          int Mode);

        public delegate void set_wobbelDelegate(uint Transversal, uint Longitudinal, double Freq);

        public delegate void set_wobbel_directionDelegate(int dX, int dY);

        public delegate void set_wobbel_controlDelegate(
          uint Ctrl,
          uint Value,
          uint MinValue,
          uint MaxValue);

        public delegate void set_wobbel_vectorDelegate(
          double dTrans,
          double dLong,
          uint Period,
          double dPower);

        public delegate void set_wobbel_offsetDelegate(int OffsetTrans, int OffsetLong);

        public delegate void set_triggerDelegate(uint Period, uint Signal1, uint Signal2);

        public delegate void set_trigger4Delegate(
          uint Period,
          uint Signal1,
          uint Signal2,
          uint Signal3,
          uint Signal4);

        public delegate void set_pixel_line_3dDelegate(
          uint Channel,
          uint HalfPeriod,
          double dX,
          double dY,
          double dZ);

        public delegate void set_pixel_lineDelegate(
          uint Channel,
          uint HalfPeriod,
          double dX,
          double dY);

        public delegate void set_n_pixelDelegate(uint PortOutValue1, uint PortOutValue2, uint Number);

        public delegate void set_pixelDelegate(uint PortOutValue1, uint PortOutValue2);

        public delegate void rs232_write_text_listDelegate(string pData);

        public delegate void set_mcbsp_outDelegate(uint Signal1, uint Signal2);

        public delegate void cammingDelegate(
          uint FirstPos,
          uint NPos,
          uint No,
          uint Ctrl,
          double Scale,
          uint Code);

        public delegate void periodic_toggle_listDelegate(
          uint Port,
          uint Mask,
          uint P1,
          uint P2,
          uint Count,
          uint Start);

        public delegate void micro_vector_abs_3dDelegate(int X, int Y, int Z, int LasOn, int LasOff);

        public delegate void micro_vector_rel_3dDelegate(
          int dX,
          int dY,
          int dZ,
          int LasOn,
          int LasOff);

        public delegate void micro_vector_absDelegate(int X, int Y, int LasOn, int LasOff);

        public delegate void micro_vector_relDelegate(int dX, int dY, int LasOn, int LasOff);

        public delegate void micro_vector_quad_axis_v_2Delegate(
          int X0,
          int Y0,
          int X1,
          int Y1,
          int LasOn,
          int LasOff,
          uint Power,
          uint Port,
          uint Flags,
          double Velocity);

        public delegate void micro_vector_quad_axis_vDelegate(
          int X0,
          int Y0,
          double X1,
          double Y1,
          int LasOn,
          int LasOff,
          uint Power,
          uint Port,
          uint Flags,
          double Velocity);

        public delegate void micro_vector_quad_axisDelegate(
          int X0,
          int Y0,
          double X1,
          double Y1,
          int LasOn,
          int LasOff,
          uint Power,
          uint Port,
          uint Flags);

        public delegate void micro_vector_set_positionDelegate(
          int X0,
          int X1,
          int X2,
          int X3,
          int LasOn,
          int LasOff);

        public delegate void multi_axis_flagsDelegate(uint Flags);

        public delegate void set_free_variable_listDelegate(uint VarNo, uint Value);

        public delegate void jump_abs_drill_2Delegate(
          int X,
          int Y,
          uint DrillTime,
          int XOff,
          int YOff);

        public delegate void jump_rel_drill_2Delegate(
          int dX,
          int dY,
          uint DrillTime,
          int XOff,
          int YOff);

        public delegate void jump_abs_drillDelegate(int X, int Y, uint DrillTime);

        public delegate void jump_rel_drillDelegate(int dX, int dY, uint DrillTime);

        public delegate void n_timed_mark_abs_3dDelegate(uint CardNo, int X, int Y, int Z, double T);

        public delegate void n_timed_mark_rel_3dDelegate(
          uint CardNo,
          int dX,
          int dY,
          int dZ,
          double T);

        public delegate void n_timed_mark_absDelegate(uint CardNo, int X, int Y, double T);

        public delegate void n_timed_mark_relDelegate(uint CardNo, int dX, int dY, double T);

        public delegate void timed_mark_abs_3dDelegate(int X, int Y, int Z, double T);

        public delegate void timed_mark_rel_3dDelegate(int dX, int dY, int dZ, double T);

        public delegate void timed_mark_absDelegate(int X, int Y, double T);

        public delegate void timed_mark_relDelegate(int dX, int dY, double T);

        public delegate void n_mark_abs_3dDelegate(uint CardNo, int X, int Y, int Z);

        public delegate void n_mark_rel_3dDelegate(uint CardNo, int dX, int dY, int dZ);

        public delegate void n_mark_absDelegate(uint CardNo, int X, int Y);

        public delegate void n_mark_relDelegate(uint CardNo, int dX, int dY);

        public delegate void mark_abs_3dDelegate(int X, int Y, int Z);

        public delegate void mark_rel_3dDelegate(int dX, int dY, int dZ);

        public delegate void mark_absDelegate(int X, int Y);

        public delegate void mark_relDelegate(int dX, int dY);

        public delegate void n_timed_jump_abs_3dDelegate(uint CardNo, int X, int Y, int Z, double T);

        public delegate void n_timed_jump_rel_3dDelegate(
          uint CardNo,
          int dX,
          int dY,
          int dZ,
          double T);

        public delegate void n_timed_jump_absDelegate(uint CardNo, int X, int Y, double T);

        public delegate void n_timed_jump_relDelegate(uint CardNo, int dX, int dY, double T);

        public delegate void timed_jump_abs_3dDelegate(int X, int Y, int Z, double T);

        public delegate void timed_jump_rel_3dDelegate(int dX, int dY, int dZ, double T);

        public delegate void timed_jump_absDelegate(int X, int Y, double T);

        public delegate void timed_jump_relDelegate(int dX, int dY, double T);

        public delegate void n_jump_abs_3dDelegate(uint CardNo, int X, int Y, int Z);

        public delegate void n_jump_rel_3dDelegate(uint CardNo, int dX, int dY, int dZ);

        public delegate void n_jump_absDelegate(uint CardNo, int X, int Y);

        public delegate void n_jump_relDelegate(uint CardNo, int dX, int dY);

        public delegate void jump_abs_3dDelegate(int X, int Y, int Z);

        public delegate void jump_rel_3dDelegate(int dX, int dY, int dZ);

        public delegate void jump_absDelegate(int X, int Y);

        public delegate void jump_relDelegate(int dX, int dY);

        public delegate void n_para_mark_abs_3dDelegate(uint CardNo, int X, int Y, int Z, uint P);

        public delegate void n_para_mark_rel_3dDelegate(uint CardNo, int dX, int dY, int dZ, uint P);

        public delegate void n_para_mark_absDelegate(uint CardNo, int X, int Y, uint P);

        public delegate void n_para_mark_relDelegate(uint CardNo, int dX, int dY, uint P);

        public delegate void para_mark_abs_3dDelegate(int X, int Y, int Z, uint P);

        public delegate void para_mark_rel_3dDelegate(int dX, int dY, int dZ, uint P);

        public delegate void para_mark_absDelegate(int X, int Y, uint P);

        public delegate void para_mark_relDelegate(int dX, int dY, uint P);

        public delegate void n_para_jump_abs_3dDelegate(uint CardNo, int X, int Y, int Z, uint P);

        public delegate void n_para_jump_rel_3dDelegate(uint CardNo, int dX, int dY, int dZ, uint P);

        public delegate void n_para_jump_absDelegate(uint CardNo, int X, int Y, uint P);

        public delegate void n_para_jump_relDelegate(uint CardNo, int dX, int dY, uint P);

        public delegate void para_jump_abs_3dDelegate(int X, int Y, int Z, uint P);

        public delegate void para_jump_rel_3dDelegate(int dX, int dY, int dZ, uint P);

        public delegate void para_jump_absDelegate(int X, int Y, uint P);

        public delegate void para_jump_relDelegate(int dX, int dY, uint P);

        public delegate void n_timed_para_mark_abs_3dDelegate(
          uint CardNo,
          int X,
          int Y,
          int Z,
          uint P,
          double T);

        public delegate void n_timed_para_mark_rel_3dDelegate(
          uint CardNo,
          int dX,
          int dY,
          int dZ,
          uint P,
          double T);

        public delegate void n_timed_para_jump_abs_3dDelegate(
          uint CardNo,
          int X,
          int Y,
          int Z,
          uint P,
          double T);

        public delegate void n_timed_para_jump_rel_3dDelegate(
          uint CardNo,
          int dX,
          int dY,
          int dZ,
          uint P,
          double T);

        public delegate void n_timed_para_mark_absDelegate(
          uint CardNo,
          int X,
          int Y,
          uint P,
          double T);

        public delegate void n_timed_para_mark_relDelegate(
          uint CardNo,
          int dX,
          int dY,
          uint P,
          double T);

        public delegate void n_timed_para_jump_absDelegate(
          uint CardNo,
          int X,
          int Y,
          uint P,
          double T);

        public delegate void n_timed_para_jump_relDelegate(
          uint CardNo,
          int dX,
          int dY,
          uint P,
          double T);

        public delegate void timed_para_mark_abs_3dDelegate(int X, int Y, int Z, uint P, double T);

        public delegate void timed_para_mark_rel_3dDelegate(int dX, int dY, int dZ, uint P, double T);

        public delegate void timed_para_jump_abs_3dDelegate(int X, int Y, int Z, uint P, double T);

        public delegate void timed_para_jump_rel_3dDelegate(int dX, int dY, int dZ, uint P, double T);

        public delegate void timed_para_mark_absDelegate(int X, int Y, uint P, double T);

        public delegate void timed_para_mark_relDelegate(int dX, int dY, uint P, double T);

        public delegate void timed_para_jump_absDelegate(int X, int Y, uint P, double T);

        public delegate void timed_para_jump_relDelegate(int dX, int dY, uint P, double T);

        public delegate void n_set_defocus_listDelegate(uint CardNo, int Shift);

        public delegate void n_set_defocus_offset_listDelegate(uint CardNo, int Shift);

        public delegate void n_set_zoom_listDelegate(uint CardNo, uint Zoom);

        public delegate void set_defocus_listDelegate(int Shift);

        public delegate void set_defocus_offset_listDelegate(int Shift);

        public delegate void set_zoom_listDelegate(uint Zoom);

        public delegate void n_timed_arc_absDelegate(
          uint CardNo,
          int X,
          int Y,
          double Angle,
          double T);

        public delegate void n_timed_arc_relDelegate(
          uint CardNo,
          int dX,
          int dY,
          double Angle,
          double T);

        public delegate void timed_arc_absDelegate(int X, int Y, double Angle, double T);

        public delegate void timed_arc_relDelegate(int dX, int dY, double Angle, double T);

        public delegate void n_arc_abs_3dDelegate(uint CardNo, int X, int Y, int Z, double Angle);

        public delegate void n_arc_rel_3dDelegate(uint CardNo, int dX, int dY, int dZ, double Angle);

        public delegate void n_arc_absDelegate(uint CardNo, int X, int Y, double Angle);

        public delegate void n_arc_relDelegate(uint CardNo, int dX, int dY, double Angle);

        public delegate void n_set_ellipseDelegate(
          uint CardNo,
          uint A,
          uint B,
          double Phi0,
          double Phi);

        public delegate void n_mark_ellipse_absDelegate(uint CardNo, int X, int Y, double Alpha);

        public delegate void n_mark_ellipse_relDelegate(uint CardNo, int dX, int dY, double Alpha);

        public delegate void arc_abs_3dDelegate(int X, int Y, int Z, double Angle);

        public delegate void arc_rel_3dDelegate(int dX, int dY, int dZ, double Angle);

        public delegate void arc_absDelegate(int X, int Y, double Angle);

        public delegate void arc_relDelegate(int dX, int dY, double Angle);

        public delegate void set_ellipseDelegate(uint A, uint B, double Phi0, double Phi);

        public delegate void mark_ellipse_absDelegate(int X, int Y, double Alpha);

        public delegate void mark_ellipse_relDelegate(int dX, int dY, double Alpha);

        public delegate void n_set_offset_xyz_listDelegate(
          uint CardNo,
          uint HeadNo,
          int XOffset,
          int YOffset,
          int ZOffset,
          uint at_once);

        public delegate void n_set_offset_listDelegate(
          uint CardNo,
          uint HeadNo,
          int XOffset,
          int YOffset,
          uint at_once);

        public delegate void n_set_matrix_listDelegate(
          uint CardNo,
          uint HeadNo,
          uint Ind1,
          uint Ind2,
          double Mij,
          uint at_once);

        public delegate void n_set_angle_listDelegate(
          uint CardNo,
          uint HeadNo,
          double Angle,
          uint at_once);

        public delegate void n_set_scale_listDelegate(
          uint CardNo,
          uint HeadNo,
          double Scale,
          uint at_once);

        public delegate void n_apply_mcbsp_listDelegate(uint CardNo, uint HeadNo, uint at_once);

        public delegate void set_offset_xyz_listDelegate(
          uint HeadNo,
          int XOffset,
          int YOffset,
          int ZOffset,
          uint at_once);

        public delegate void set_offset_listDelegate(
          uint HeadNo,
          int XOffset,
          int YOffset,
          uint at_once);

        public delegate void set_matrix_listDelegate(
          uint HeadNo,
          uint Ind1,
          uint Ind2,
          double Mij,
          uint at_once);

        public delegate void set_angle_listDelegate(uint HeadNo, double Angle, uint at_once);

        public delegate void set_scale_listDelegate(uint HeadNo, double Scale, uint at_once);

        public delegate void apply_mcbsp_listDelegate(uint HeadNo, uint at_once);

        public delegate void n_set_mark_speedDelegate(uint CardNo, double Speed);

        public delegate void n_set_jump_speedDelegate(uint CardNo, double Speed);

        public delegate void n_set_sky_writing_para_listDelegate(
          uint CardNo,
          double Timelag,
          int LaserOnShift,
          uint Nprev,
          uint Npost);

        public delegate void n_set_sky_writing_listDelegate(
          uint CardNo,
          double Timelag,
          int LaserOnShift);

        public delegate void n_set_sky_writing_limit_listDelegate(uint CardNo, double CosAngle);

        public delegate void n_set_sky_writing_mode_listDelegate(uint CardNo, uint Mode);

        public delegate void n_set_scanner_delaysDelegate(
          uint CardNo,
          uint Jump,
          uint Mark,
          uint Polygon);

        public delegate void n_set_jump_mode_listDelegate(uint CardNo, int Flag);

        public delegate void n_enduring_wobbelDelegate(uint CardNo);

        public delegate void n_set_delay_mode_listDelegate(
          uint CardNo,
          uint VarPoly,
          uint DirectMove3D,
          uint EdgeLevel,
          uint MinJumpDelay,
          uint JumpLengthLimit);

        public delegate void set_mark_speedDelegate(double Speed);

        public delegate void set_jump_speedDelegate(double Speed);

        public delegate void set_sky_writing_para_listDelegate(
          double Timelag,
          int LaserOnShift,
          uint Nprev,
          uint Npost);

        public delegate void set_sky_writing_listDelegate(double Timelag, int LaserOnShift);

        public delegate void set_sky_writing_limit_listDelegate(double CosAngle);

        public delegate void set_sky_writing_mode_listDelegate(uint Mode);

        public delegate void set_scanner_delaysDelegate(uint Jump, uint Mark, uint Polygon);

        public delegate void set_jump_mode_listDelegate(int Flag);

        public delegate void enduring_wobbelDelegate();

        public delegate void set_delay_mode_listDelegate(
          uint VarPoly,
          uint DirectMove3D,
          uint EdgeLevel,
          uint MinJumpDelay,
          uint JumpLengthLimit);

        public delegate void n_activate_scanahead_autodelays_listDelegate(uint CardNo, int Mode);

        public delegate void n_set_scanahead_laser_shifts_listDelegate(
          uint CardNo,
          int dLasOn,
          int dLasOff);

        public delegate void n_set_scanahead_line_params_listDelegate(
          uint CardNo,
          uint CornerScale,
          uint EndScale,
          uint AccScale);

        public delegate void n_set_scanahead_line_params_ex_listDelegate(
          uint CardNo,
          uint CornerScale,
          uint EndScale,
          uint AccScale,
          uint JumpScale);

        public delegate void activate_scanahead_autodelays_listDelegate(int Mode);

        public delegate void set_scanahead_laser_shifts_listDelegate(int dLasOn, int dLasOff);

        public delegate void set_scanahead_line_params_listDelegate(
          uint CornerScale,
          uint EndScale,
          uint AccScale);

        public delegate void set_scanahead_line_params_ex_listDelegate(
          uint CornerScale,
          uint EndScale,
          uint AccScale,
          uint JumpScale);

        public delegate void n_stepper_enable_listDelegate(uint CardNo, int Enable1, int Enable2);

        public delegate void n_stepper_control_listDelegate(uint CardNo, int Period1, int Period2);

        public delegate void n_stepper_abs_no_listDelegate(uint CardNo, uint No, int Pos);

        public delegate void n_stepper_rel_no_listDelegate(uint CardNo, uint No, int dPos);

        public delegate void n_stepper_abs_listDelegate(uint CardNo, int Pos1, int Pos2);

        public delegate void n_stepper_rel_listDelegate(uint CardNo, int dPos1, int dPos2);

        public delegate void n_stepper_waitDelegate(uint CardNo, uint No);

        public delegate void stepper_enable_listDelegate(int Enable1, int Enable2);

        public delegate void stepper_control_listDelegate(int Period1, int Period2);

        public delegate void stepper_abs_no_listDelegate(uint No, int Pos);

        public delegate void stepper_rel_no_listDelegate(uint No, int dPos);

        public delegate void stepper_abs_listDelegate(int Pos1, int Pos2);

        public delegate void stepper_rel_listDelegate(int dPos1, int dPos2);

        public delegate void stepper_waitDelegate(uint No);
    }
}
