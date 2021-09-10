// Decompiled with JetBrains decompiler
// Type: RTC4Import.RTC4Wrap
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.rtc.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RTC4Import
{
    /// <summary>
    /// Static RTC4 Wrapper class.
    /// Notice that the construction of the RTC4Wrap object or an initial
    /// call of any RTC4Wrap method may throw a TypeInitializationException
    /// exception, which indicates that the required DLL is missing or the
    /// import of a particular DLL function failed. In order to analyze and
    /// properly handle such an error condition you need to catch that
    /// TypeInitializationException type exception.
    /// </summary>
    public class RTC4Wrap
    {
        /// <summary>dll 초기화 여부</summary>
        public static bool Initialized;
        private const int SampleArraySize = 32768;
        private const string RTC4DLLx86 = "RTC4DLL.dll";
        private const string RTC4DLLx64 = "RTC4DLLx64.dll";
        /// <summary>short getmemory(ushort adr);</summary>
        public static RTC4Wrap.getmemoryDelegate getmemory = (RTC4Wrap.getmemoryDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.getmemoryDelegate>(nameof(getmemory));
        /// <summary>
        ///  n_get_waveform(ushort n, ushort channel, ushort istop, short[] memptr);
        /// </summary>
        public static RTC4Wrap.n_get_waveformDelegate n_get_waveform = (RTC4Wrap.n_get_waveformDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_waveformDelegate>(nameof(n_get_waveform));
        /// <summary>
        ///  get_waveform(ushort channel, ushort istop, short[] memptr);
        /// </summary>
        public static RTC4Wrap.get_waveformDelegate get_waveform = (RTC4Wrap.get_waveformDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_waveformDelegate>(nameof(get_waveform));
        /// <summary>
        ///  n_measurement_status(ushort n, out ushort busy, out ushort position);
        /// </summary>
        public static RTC4Wrap.n_measurement_statusDelegate n_measurement_status = (RTC4Wrap.n_measurement_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_measurement_statusDelegate>(nameof(n_measurement_status));
        /// <summary>
        ///  measurement_status(out ushort busy, out ushort position);
        /// </summary>
        public static RTC4Wrap.measurement_statusDelegate measurement_status = (RTC4Wrap.measurement_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.measurement_statusDelegate>(nameof(measurement_status));
        /// <summary>
        ///  short n_load_varpolydelay(ushort n, string stbfilename, ushort tableno);
        /// </summary>
        public static RTC4Wrap.n_load_varpolydelayDelegate n_load_varpolydelay = (RTC4Wrap.n_load_varpolydelayDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_load_varpolydelayDelegate>(nameof(n_load_varpolydelay));
        /// <summary>
        ///  short load_varpolydelay(string stbfilename, ushort tableno);
        /// </summary>
        public static RTC4Wrap.load_varpolydelayDelegate load_varpolydelay = (RTC4Wrap.load_varpolydelayDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.load_varpolydelayDelegate>(nameof(load_varpolydelay));
        /// <summary>short n_load_program_file(ushort n, string name);</summary>
        public static RTC4Wrap.n_load_program_fileDelegate n_load_program_file = (RTC4Wrap.n_load_program_fileDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_load_program_fileDelegate>(nameof(n_load_program_file));
        /// <summary>short load_program_file(string name);</summary>
        public static RTC4Wrap.load_program_fileDelegate load_program_file = (RTC4Wrap.load_program_fileDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.load_program_fileDelegate>(nameof(load_program_file));
        /// <summary>
        ///  short n_load_correction_file(ushort n, string filename, short cortable, double kx, double ky, double phi, double xoffset, double yoffset);
        /// </summary>
        public static RTC4Wrap.n_load_correction_fileDelegate n_load_correction_file = (RTC4Wrap.n_load_correction_fileDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_load_correction_fileDelegate>(nameof(n_load_correction_file));
        /// <summary>
        ///  short load_correction_file(string filename, short cortable, double kx, double ky, double phi, double xoffset, double yoffset);
        /// </summary>
        public static RTC4Wrap.load_correction_fileDelegate load_correction_file = (RTC4Wrap.load_correction_fileDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.load_correction_fileDelegate>(nameof(load_correction_file));
        /// <summary>
        ///  short n_load_z_table(ushort n, double a, double b, double c);
        /// </summary>
        public static RTC4Wrap.n_load_z_tableDelegate n_load_z_table = (RTC4Wrap.n_load_z_tableDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_load_z_tableDelegate>(nameof(n_load_z_table));
        /// <summary>short load_z_table(double a, double b, double c);</summary>
        public static RTC4Wrap.load_z_tableDelegate load_z_table = (RTC4Wrap.load_z_tableDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.load_z_tableDelegate>(nameof(load_z_table));
        /// <summary>n_list_nop(ushort n);</summary>
        public static RTC4Wrap.n_list_nopDelegate n_list_nop = (RTC4Wrap.n_list_nopDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_list_nopDelegate>(nameof(n_list_nop));
        /// <summary>list_nop();</summary>
        public static RTC4Wrap.list_nopDelegate list_nop = (RTC4Wrap.list_nopDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.list_nopDelegate>(nameof(list_nop));
        /// <summary>n_set_end_of_list(ushort n);</summary>
        public static RTC4Wrap.n_set_end_of_listDelegate n_set_end_of_list = (RTC4Wrap.n_set_end_of_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_end_of_listDelegate>(nameof(n_set_end_of_list));
        /// <summary>set_end_of_list();</summary>
        public static RTC4Wrap.set_end_of_listDelegate set_end_of_list = (RTC4Wrap.set_end_of_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_end_of_listDelegate>(nameof(set_end_of_list));
        /// <summary>n_jump_abs_3d(ushort n, short x, short y, short z);</summary>
        public static RTC4Wrap.n_jump_abs_3dDelegate n_jump_abs_3d = (RTC4Wrap.n_jump_abs_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_jump_abs_3dDelegate>(nameof(n_jump_abs_3d));
        /// <summary>jump_abs_3d(short x, short y, short z);</summary>
        public static RTC4Wrap.jump_abs_3dDelegate jump_abs_3d = (RTC4Wrap.jump_abs_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.jump_abs_3dDelegate>(nameof(jump_abs_3d));
        /// <summary>n_jump_abs(ushort n, short x, short y);</summary>
        public static RTC4Wrap.n_jump_absDelegate n_jump_abs = (RTC4Wrap.n_jump_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_jump_absDelegate>(nameof(n_jump_abs));
        /// <summary>jump_abs(short x, short y);</summary>
        public static RTC4Wrap.jump_absDelegate jump_abs = (RTC4Wrap.jump_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.jump_absDelegate>(nameof(jump_abs));
        /// <summary>n_mark_abs_3d(ushort n, short x, short y, short z);</summary>
        public static RTC4Wrap.n_mark_abs_3dDelegate n_mark_abs_3d = (RTC4Wrap.n_mark_abs_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_mark_abs_3dDelegate>(nameof(n_mark_abs_3d));
        /// <summary>mark_abs_3d(short x, short y, short z);</summary>
        public static RTC4Wrap.mark_abs_3dDelegate mark_abs_3d = (RTC4Wrap.mark_abs_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.mark_abs_3dDelegate>(nameof(mark_abs_3d));
        /// <summary>n_mark_abs(ushort n, short x, short y);</summary>
        public static RTC4Wrap.n_mark_absDelegate n_mark_abs = (RTC4Wrap.n_mark_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_mark_absDelegate>(nameof(n_mark_abs));
        /// <summary>mark_abs(short x, short y);</summary>
        public static RTC4Wrap.mark_absDelegate mark_abs = (RTC4Wrap.mark_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.mark_absDelegate>(nameof(mark_abs));
        /// <summary>n_jump_rel_3d(ushort n, short dx, short dy, short dz);</summary>
        public static RTC4Wrap.n_jump_rel_3dDelegate n_jump_rel_3d = (RTC4Wrap.n_jump_rel_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_jump_rel_3dDelegate>(nameof(n_jump_rel_3d));
        /// <summary>jump_rel_3d(short dx, short dy, short dz);</summary>
        public static RTC4Wrap.jump_rel_3dDelegate jump_rel_3d = (RTC4Wrap.jump_rel_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.jump_rel_3dDelegate>(nameof(jump_rel_3d));
        /// <summary>n_jump_rel(ushort n, short dx, short dy);</summary>
        public static RTC4Wrap.n_jump_relDelegate n_jump_rel = (RTC4Wrap.n_jump_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_jump_relDelegate>(nameof(n_jump_rel));
        /// <summary>jump_rel(short dx, short dy);</summary>
        public static RTC4Wrap.jump_relDelegate jump_rel = (RTC4Wrap.jump_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.jump_relDelegate>(nameof(jump_rel));
        /// <summary>n_mark_rel_3d(ushort n, short dx, short dy, short dz);</summary>
        public static RTC4Wrap.n_mark_rel_3dDelegate n_mark_rel_3d = (RTC4Wrap.n_mark_rel_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_mark_rel_3dDelegate>(nameof(n_mark_rel_3d));
        /// <summary>mark_rel_3d(short dx, short dy, short dz);</summary>
        public static RTC4Wrap.mark_rel_3dDelegate mark_rel_3d = (RTC4Wrap.mark_rel_3dDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.mark_rel_3dDelegate>(nameof(mark_rel_3d));
        /// <summary>n_mark_rel(ushort n, short dx, short dy);</summary>
        public static RTC4Wrap.n_mark_relDelegate n_mark_rel = (RTC4Wrap.n_mark_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_mark_relDelegate>(nameof(n_mark_rel));
        /// <summary>mark_rel(short dx, short dy);</summary>
        public static RTC4Wrap.mark_relDelegate mark_rel = (RTC4Wrap.mark_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.mark_relDelegate>(nameof(mark_rel));
        /// <summary>n_write_8bit_port_list(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_8bit_port_listDelegate n_write_8bit_port_list = (RTC4Wrap.n_write_8bit_port_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_8bit_port_listDelegate>(nameof(n_write_8bit_port_list));
        /// <summary>write_8bit_port_list(ushort value);</summary>
        public static RTC4Wrap.write_8bit_port_listDelegate write_8bit_port_list = (RTC4Wrap.write_8bit_port_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_8bit_port_listDelegate>(nameof(write_8bit_port_list));
        /// <summary>n_write_da_1_list(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_da_1_listDelegate n_write_da_1_list = (RTC4Wrap.n_write_da_1_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_da_1_listDelegate>(nameof(n_write_da_1_list));
        /// <summary>write_da_1_list(ushort value);</summary>
        public static RTC4Wrap.write_da_1_listDelegate write_da_1_list = (RTC4Wrap.write_da_1_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_da_1_listDelegate>(nameof(write_da_1_list));
        /// <summary>n_write_da_2_list(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_da_2_listDelegate n_write_da_2_list = (RTC4Wrap.n_write_da_2_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_da_2_listDelegate>(nameof(n_write_da_2_list));
        /// <summary>write_da_2_list(ushort value);</summary>
        public static RTC4Wrap.write_da_2_listDelegate write_da_2_list = (RTC4Wrap.write_da_2_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_da_2_listDelegate>(nameof(write_da_2_list));
        /// <summary>
        ///  n_set_matrix_list(ushort n, ushort i, ushort j, double mij);
        /// </summary>
        public static RTC4Wrap.n_set_matrix_listDelegate n_set_matrix_list = (RTC4Wrap.n_set_matrix_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_matrix_listDelegate>(nameof(n_set_matrix_list));
        /// <summary>set_matrix_list(ushort i, ushort j, double mij);</summary>
        public static RTC4Wrap.set_matrix_listDelegate set_matrix_list = (RTC4Wrap.set_matrix_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_matrix_listDelegate>(nameof(set_matrix_list));
        /// <summary>n_set_defocus_list(ushort n, short value);</summary>
        public static RTC4Wrap.n_set_defocus_listDelegate n_set_defocus_list = (RTC4Wrap.n_set_defocus_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_defocus_listDelegate>(nameof(n_set_defocus_list));
        /// <summary>set_defocus_list(short value);</summary>
        public static RTC4Wrap.set_defocus_listDelegate set_defocus_list = (RTC4Wrap.set_defocus_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_defocus_listDelegate>(nameof(set_defocus_list));
        /// <summary>n_set_control_mode_list(ushort n, ushort mode);</summary>
        public static RTC4Wrap.n_set_control_mode_listDelegate n_set_control_mode_list = (RTC4Wrap.n_set_control_mode_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_control_mode_listDelegate>(nameof(n_set_control_mode_list));
        /// <summary>set_control_mode_list(ushort mode);</summary>
        public static RTC4Wrap.set_control_mode_listDelegate set_control_mode_list = (RTC4Wrap.set_control_mode_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_control_mode_listDelegate>(nameof(set_control_mode_list));
        /// <summary>
        ///  n_set_offset_list(ushort n, short xoffset, short yoffset);
        /// </summary>
        public static RTC4Wrap.n_set_offset_listDelegate n_set_offset_list = (RTC4Wrap.n_set_offset_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_offset_listDelegate>(nameof(n_set_offset_list));
        /// <summary>set_offset_list(short xoffset, short yoffset);</summary>
        public static RTC4Wrap.set_offset_listDelegate set_offset_list = (RTC4Wrap.set_offset_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_offset_listDelegate>(nameof(set_offset_list));
        /// <summary>n_long_delay(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_long_delayDelegate n_long_delay = (RTC4Wrap.n_long_delayDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_long_delayDelegate>(nameof(n_long_delay));
        /// <summary>long_delay(ushort value);</summary>
        public static RTC4Wrap.long_delayDelegate long_delay = (RTC4Wrap.long_delayDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.long_delayDelegate>(nameof(long_delay));
        /// <summary>n_laser_on_list(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_laser_on_listDelegate n_laser_on_list = (RTC4Wrap.n_laser_on_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_laser_on_listDelegate>(nameof(n_laser_on_list));
        /// <summary>laser_on_list(ushort value);</summary>
        public static RTC4Wrap.laser_on_listDelegate laser_on_list = (RTC4Wrap.laser_on_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.laser_on_listDelegate>(nameof(laser_on_list));
        /// <summary>n_set_jump_speed(ushort n, double speed);</summary>
        public static RTC4Wrap.n_set_jump_speedDelegate n_set_jump_speed = (RTC4Wrap.n_set_jump_speedDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_jump_speedDelegate>(nameof(n_set_jump_speed));
        /// <summary>set_jump_speed(double speed);</summary>
        public static RTC4Wrap.set_jump_speedDelegate set_jump_speed = (RTC4Wrap.set_jump_speedDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_jump_speedDelegate>(nameof(set_jump_speed));
        /// <summary>n_set_mark_speed(ushort n, double speed);</summary>
        public static RTC4Wrap.n_set_mark_speedDelegate n_set_mark_speed = (RTC4Wrap.n_set_mark_speedDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_mark_speedDelegate>(nameof(n_set_mark_speed));
        /// <summary>set_mark_speed(double speed);</summary>
        public static RTC4Wrap.set_mark_speedDelegate set_mark_speed = (RTC4Wrap.set_mark_speedDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_mark_speedDelegate>(nameof(set_mark_speed));
        /// <summary>
        ///  n_set_laser_delays(ushort n, short ondelay, short offdelay);
        /// </summary>
        public static RTC4Wrap.n_set_laser_delaysDelegate n_set_laser_delays = (RTC4Wrap.n_set_laser_delaysDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_laser_delaysDelegate>(nameof(n_set_laser_delays));
        /// <summary>set_laser_delays(short ondelay, short offdelay);</summary>
        public static RTC4Wrap.set_laser_delaysDelegate set_laser_delays = (RTC4Wrap.set_laser_delaysDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_laser_delaysDelegate>(nameof(set_laser_delays));
        /// <summary>
        ///  n_set_scanner_delays(ushort n, ushort jumpdelay, ushort markdelay, ushort polydelay);
        /// </summary>
        public static RTC4Wrap.n_set_scanner_delaysDelegate n_set_scanner_delays = (RTC4Wrap.n_set_scanner_delaysDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_scanner_delaysDelegate>(nameof(n_set_scanner_delays));
        /// <summary>
        ///  set_scanner_delays(ushort jumpdelay, ushort markdelay, ushort polydelay);
        /// </summary>
        public static RTC4Wrap.set_scanner_delaysDelegate set_scanner_delays = (RTC4Wrap.set_scanner_delaysDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_scanner_delaysDelegate>(nameof(set_scanner_delays));
        /// <summary>n_set_list_jump(ushort n, ushort position);</summary>
        public static RTC4Wrap.n_set_list_jumpDelegate n_set_list_jump = (RTC4Wrap.n_set_list_jumpDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_list_jumpDelegate>(nameof(n_set_list_jump));
        /// <summary>set_list_jump(ushort position);</summary>
        public static RTC4Wrap.set_list_jumpDelegate set_list_jump = (RTC4Wrap.set_list_jumpDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_list_jumpDelegate>(nameof(set_list_jump));
        /// <summary>n_set_input_pointer(ushort n, ushort pointer);</summary>
        public static RTC4Wrap.n_set_input_pointerDelegate n_set_input_pointer = (RTC4Wrap.n_set_input_pointerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_input_pointerDelegate>(nameof(n_set_input_pointer));
        /// <summary>set_input_pointer(ushort pointer);</summary>
        public static RTC4Wrap.set_input_pointerDelegate set_input_pointer = (RTC4Wrap.set_input_pointerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_input_pointerDelegate>(nameof(set_input_pointer));
        /// <summary>n_list_call(ushort n, ushort position);</summary>
        public static RTC4Wrap.n_list_callDelegate n_list_call = (RTC4Wrap.n_list_callDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_list_callDelegate>(nameof(n_list_call));
        /// <summary>list_call(ushort position);</summary>
        public static RTC4Wrap.list_callDelegate list_call = (RTC4Wrap.list_callDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.list_callDelegate>(nameof(list_call));
        /// <summary>n_list_return(ushort n);</summary>
        public static RTC4Wrap.n_list_returnDelegate n_list_return = (RTC4Wrap.n_list_returnDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_list_returnDelegate>(nameof(n_list_return));
        /// <summary>list_return();</summary>
        public static RTC4Wrap.list_returnDelegate list_return = (RTC4Wrap.list_returnDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.list_returnDelegate>(nameof(list_return));
        /// <summary>n_z_out_list(ushort n, short z);</summary>
        public static RTC4Wrap.n_z_out_listDelegate n_z_out_list = (RTC4Wrap.n_z_out_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_z_out_listDelegate>(nameof(n_z_out_list));
        /// <summary>z_out_list(short z);</summary>
        public static RTC4Wrap.z_out_listDelegate z_out_list = (RTC4Wrap.z_out_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.z_out_listDelegate>(nameof(z_out_list));
        /// <summary>
        ///  n_set_standby_list(ushort n, ushort half_period, ushort pulse);
        /// </summary>
        public static RTC4Wrap.n_set_standby_listDelegate n_set_standby_list = (RTC4Wrap.n_set_standby_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_standby_listDelegate>(nameof(n_set_standby_list));
        /// <summary>set_standby_list(ushort half_period, ushort pulse);</summary>
        public static RTC4Wrap.set_standby_listDelegate set_standby_list = (RTC4Wrap.set_standby_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_standby_listDelegate>(nameof(set_standby_list));
        /// <summary>
        ///  n_timed_jump_abs(ushort n, short x, short y, double time);
        /// </summary>
        public static RTC4Wrap.n_timed_jump_absDelegate n_timed_jump_abs = (RTC4Wrap.n_timed_jump_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_timed_jump_absDelegate>(nameof(n_timed_jump_abs));
        /// <summary>timed_jump_abs(short x, short y, double time);</summary>
        public static RTC4Wrap.timed_jump_absDelegate timed_jump_abs = (RTC4Wrap.timed_jump_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.timed_jump_absDelegate>(nameof(timed_jump_abs));
        /// <summary>
        ///  n_timed_mark_abs(ushort n, short x, short y, double time);
        /// </summary>
        public static RTC4Wrap.n_timed_mark_absDelegate n_timed_mark_abs = (RTC4Wrap.n_timed_mark_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_timed_mark_absDelegate>(nameof(n_timed_mark_abs));
        /// <summary>timed_mark_abs(short x, short y, double time);</summary>
        public static RTC4Wrap.timed_mark_absDelegate timed_mark_abs = (RTC4Wrap.timed_mark_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.timed_mark_absDelegate>(nameof(timed_mark_abs));
        /// <summary>
        ///  n_timed_jump_rel(ushort n, short dx, short dy, double time);
        /// </summary>
        public static RTC4Wrap.n_timed_jump_relDelegate n_timed_jump_rel = (RTC4Wrap.n_timed_jump_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_timed_jump_relDelegate>(nameof(n_timed_jump_rel));
        /// <summary>timed_jump_rel(short dx, short dy, double time);</summary>
        public static RTC4Wrap.timed_jump_relDelegate timed_jump_rel = (RTC4Wrap.timed_jump_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.timed_jump_relDelegate>(nameof(timed_jump_rel));
        /// <summary>
        ///  n_timed_mark_rel(ushort n, short dx, short dy, double time);
        /// </summary>
        public static RTC4Wrap.n_timed_mark_relDelegate n_timed_mark_rel = (RTC4Wrap.n_timed_mark_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_timed_mark_relDelegate>(nameof(n_timed_mark_rel));
        /// <summary>timed_mark_rel(short dx, short dy, double time);</summary>
        public static RTC4Wrap.timed_mark_relDelegate timed_mark_rel = (RTC4Wrap.timed_mark_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.timed_mark_relDelegate>(nameof(timed_mark_rel));
        /// <summary>
        ///  n_set_laser_timing(ushort n, ushort halfperiod, ushort pulse1, ushort pulse2, ushort timebase);
        /// </summary>
        public static RTC4Wrap.n_set_laser_timingDelegate n_set_laser_timing = (RTC4Wrap.n_set_laser_timingDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_laser_timingDelegate>(nameof(n_set_laser_timing));
        /// <summary>
        ///  set_laser_timing(ushort halfperiod, ushort pulse1, ushort pulse2, ushort timebase);
        /// </summary>
        public static RTC4Wrap.set_laser_timingDelegate set_laser_timing = (RTC4Wrap.set_laser_timingDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_laser_timingDelegate>(nameof(set_laser_timing));
        /// <summary>
        ///  n_set_wobbel_xy(ushort n, ushort long_wob, ushort trans_wob, double frequency);
        /// </summary>
        public static RTC4Wrap.n_set_wobbel_xyDelegate n_set_wobbel_xy = (RTC4Wrap.n_set_wobbel_xyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_wobbel_xyDelegate>(nameof(n_set_wobbel_xy));
        /// <summary>
        ///  set_wobbel_xy(ushort long_wob, ushort trans_wob, double frequency);
        /// </summary>
        public static RTC4Wrap.set_wobbel_xyDelegate set_wobbel_xy = (RTC4Wrap.set_wobbel_xyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_wobbel_xyDelegate>(nameof(set_wobbel_xy));
        /// <summary>
        ///  n_set_wobbel(ushort n, ushort amplitude, double frequency);
        /// </summary>
        public static RTC4Wrap.n_set_wobbelDelegate n_set_wobbel = (RTC4Wrap.n_set_wobbelDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_wobbelDelegate>(nameof(n_set_wobbel));
        /// <summary>set_wobbel(ushort amplitude, double frequency);</summary>
        public static RTC4Wrap.set_wobbelDelegate set_wobbel = (RTC4Wrap.set_wobbelDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_wobbelDelegate>(nameof(set_wobbel));
        /// <summary>n_set_fly_x(ushort n, double kx);</summary>
        public static RTC4Wrap.n_set_fly_xDelegate n_set_fly_x = (RTC4Wrap.n_set_fly_xDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_fly_xDelegate>(nameof(n_set_fly_x));
        /// <summary>set_fly_x(double kx);</summary>
        public static RTC4Wrap.set_fly_xDelegate set_fly_x = (RTC4Wrap.set_fly_xDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_fly_xDelegate>(nameof(set_fly_x));
        /// <summary>n_set_fly_y(ushort n, double ky);</summary>
        public static RTC4Wrap.n_set_fly_yDelegate n_set_fly_y = (RTC4Wrap.n_set_fly_yDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_fly_yDelegate>(nameof(n_set_fly_y));
        /// <summary>set_fly_y(double ky);</summary>
        public static RTC4Wrap.set_fly_yDelegate set_fly_y = (RTC4Wrap.set_fly_yDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_fly_yDelegate>(nameof(set_fly_y));
        /// <summary>n_set_fly_rot(ushort n, double resolution);</summary>
        public static RTC4Wrap.n_set_fly_rotDelegate n_set_fly_rot = (RTC4Wrap.n_set_fly_rotDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_fly_rotDelegate>(nameof(n_set_fly_rot));
        /// <summary>set_fly_rot(double resolution);</summary>
        public static RTC4Wrap.set_fly_rotDelegate set_fly_rot = (RTC4Wrap.set_fly_rotDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_fly_rotDelegate>(nameof(set_fly_rot));
        /// <summary>n_fly_return(ushort n, short x, short y);</summary>
        public static RTC4Wrap.n_fly_returnDelegate n_fly_return = (RTC4Wrap.n_fly_returnDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_fly_returnDelegate>(nameof(n_fly_return));
        /// <summary>fly_return(short x, short y);</summary>
        public static RTC4Wrap.fly_returnDelegate fly_return = (RTC4Wrap.fly_returnDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.fly_returnDelegate>(nameof(fly_return));
        /// <summary>
        ///  n_calculate_fly(ushort n, ushort direction, double distance);
        /// </summary>
        public static RTC4Wrap.n_calculate_flyDelegate n_calculate_fly = (RTC4Wrap.n_calculate_flyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_calculate_flyDelegate>(nameof(n_calculate_fly));
        /// <summary>calculate_fly(ushort direction, double distance);</summary>
        public static RTC4Wrap.calculate_flyDelegate calculate_fly = (RTC4Wrap.calculate_flyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.calculate_flyDelegate>(nameof(calculate_fly));
        /// <summary>n_write_io_port_list(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_io_port_listDelegate n_write_io_port_list = (RTC4Wrap.n_write_io_port_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_io_port_listDelegate>(nameof(n_write_io_port_list));
        /// <summary>write_io_port_list(ushort value);</summary>
        public static RTC4Wrap.write_io_port_listDelegate write_io_port_list = (RTC4Wrap.write_io_port_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_io_port_listDelegate>(nameof(write_io_port_list));
        /// <summary>
        ///  n_select_cor_table_list(ushort n, ushort heada, ushort headb);
        /// </summary>
        public static RTC4Wrap.n_select_cor_table_listDelegate n_select_cor_table_list = (RTC4Wrap.n_select_cor_table_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_select_cor_table_listDelegate>(nameof(n_select_cor_table_list));
        /// <summary>select_cor_table_list(ushort heada, ushort headb);</summary>
        public static RTC4Wrap.select_cor_table_listDelegate select_cor_table_list = (RTC4Wrap.select_cor_table_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.select_cor_table_listDelegate>(nameof(select_cor_table_list));
        /// <summary>n_set_wait(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_set_waitDelegate n_set_wait = (RTC4Wrap.n_set_waitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_waitDelegate>(nameof(n_set_wait));
        /// <summary>set_wait(ushort value);</summary>
        public static RTC4Wrap.set_waitDelegate set_wait = (RTC4Wrap.set_waitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_waitDelegate>(nameof(set_wait));
        /// <summary>
        ///  n_simulate_ext_start(ushort n, short delay, short encoder);
        /// </summary>
        public static RTC4Wrap.n_simulate_ext_startDelegate n_simulate_ext_start = (RTC4Wrap.n_simulate_ext_startDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_simulate_ext_startDelegate>(nameof(n_simulate_ext_start));
        /// <summary>simulate_ext_start(short delay, short encoder);</summary>
        public static RTC4Wrap.simulate_ext_startDelegate simulate_ext_start = (RTC4Wrap.simulate_ext_startDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.simulate_ext_startDelegate>(nameof(simulate_ext_start));
        /// <summary>n_write_da_x_list(ushort n, ushort x, ushort value);</summary>
        public static RTC4Wrap.n_write_da_x_listDelegate n_write_da_x_list = (RTC4Wrap.n_write_da_x_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_da_x_listDelegate>(nameof(n_write_da_x_list));
        /// <summary>write_da_x_list(ushort x, ushort value);</summary>
        public static RTC4Wrap.write_da_x_listDelegate write_da_x_list = (RTC4Wrap.write_da_x_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_da_x_listDelegate>(nameof(write_da_x_list));
        /// <summary>
        ///  n_set_pixel_line(ushort n, ushort pixelmode, ushort pixelperiod, double dx, double dy);
        /// </summary>
        public static RTC4Wrap.n_set_pixel_lineDelegate n_set_pixel_line = (RTC4Wrap.n_set_pixel_lineDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_pixel_lineDelegate>(nameof(n_set_pixel_line));
        /// <summary>
        ///  set_pixel_line(ushort pixelmode, ushort pixelperiod, double dx, double dy);
        /// </summary>
        public static RTC4Wrap.set_pixel_lineDelegate set_pixel_line = (RTC4Wrap.set_pixel_lineDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_pixel_lineDelegate>(nameof(set_pixel_line));
        /// <summary>
        ///  n_set_pixel(ushort n, ushort pulswidth, ushort davalue, ushort adchannel);
        /// </summary>
        public static RTC4Wrap.n_set_pixelDelegate n_set_pixel = (RTC4Wrap.n_set_pixelDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_pixelDelegate>(nameof(n_set_pixel));
        /// <summary>
        ///  set_pixel(ushort pulswidth, ushort davalue, ushort adchannel);
        /// </summary>
        public static RTC4Wrap.set_pixelDelegate set_pixel = (RTC4Wrap.set_pixelDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_pixelDelegate>(nameof(set_pixel));
        /// <summary>n_set_extstartpos_list(ushort n, ushort position);</summary>
        public static RTC4Wrap.n_set_extstartpos_listDelegate n_set_extstartpos_list = (RTC4Wrap.n_set_extstartpos_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_extstartpos_listDelegate>(nameof(n_set_extstartpos_list));
        /// <summary>set_extstartpos_list(ushort position);</summary>
        public static RTC4Wrap.set_extstartpos_listDelegate set_extstartpos_list = (RTC4Wrap.set_extstartpos_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_extstartpos_listDelegate>(nameof(set_extstartpos_list));
        /// <summary>n_laser_signal_on_list(ushort n);</summary>
        public static RTC4Wrap.n_laser_signal_on_listDelegate n_laser_signal_on_list = (RTC4Wrap.n_laser_signal_on_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_laser_signal_on_listDelegate>(nameof(n_laser_signal_on_list));
        /// <summary>laser_signal_on_list();</summary>
        public static RTC4Wrap.laser_signal_on_listDelegate laser_signal_on_list = (RTC4Wrap.laser_signal_on_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.laser_signal_on_listDelegate>(nameof(laser_signal_on_list));
        /// <summary>n_laser_signal_off_list(ushort n);</summary>
        public static RTC4Wrap.n_laser_signal_off_listDelegate n_laser_signal_off_list = (RTC4Wrap.n_laser_signal_off_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_laser_signal_off_listDelegate>(nameof(n_laser_signal_off_list));
        /// <summary>laser_signal_off_list();</summary>
        public static RTC4Wrap.laser_signal_off_listDelegate laser_signal_off_list = (RTC4Wrap.laser_signal_off_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.laser_signal_off_listDelegate>(nameof(laser_signal_off_list));
        /// <summary>n_set_firstpulse_killer_list(ushort n, ushort fpk);</summary>
        public static RTC4Wrap.n_set_firstpulse_killer_listDelegate n_set_firstpulse_killer_list = (RTC4Wrap.n_set_firstpulse_killer_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_firstpulse_killer_listDelegate>(nameof(n_set_firstpulse_killer_list));
        /// <summary>set_firstpulse_killer_list(ushort fpk);</summary>
        public static RTC4Wrap.set_firstpulse_killer_listDelegate set_firstpulse_killer_list = (RTC4Wrap.set_firstpulse_killer_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_firstpulse_killer_listDelegate>(nameof(set_firstpulse_killer_list));
        /// <summary>
        ///  n_set_io_cond_list(ushort n, ushort mask_1, ushort mask_0, ushort mask_set);
        /// </summary>
        public static RTC4Wrap.n_set_io_cond_listDelegate n_set_io_cond_list = (RTC4Wrap.n_set_io_cond_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_io_cond_listDelegate>(nameof(n_set_io_cond_list));
        /// <summary>
        ///  set_io_cond_list(ushort mask_1, ushort mask_0, ushort mask_set);
        /// </summary>
        public static RTC4Wrap.set_io_cond_listDelegate set_io_cond_list = (RTC4Wrap.set_io_cond_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_io_cond_listDelegate>(nameof(set_io_cond_list));
        /// <summary>
        ///  n_clear_io_cond_list(ushort n, ushort mask_1, ushort mask_0, ushort mask_clear);
        /// </summary>
        public static RTC4Wrap.n_clear_io_cond_listDelegate n_clear_io_cond_list = (RTC4Wrap.n_clear_io_cond_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_clear_io_cond_listDelegate>(nameof(n_clear_io_cond_list));
        /// <summary>
        ///  clear_io_cond_list(ushort mask_1, ushort mask_0, ushort mask_clear);
        /// </summary>
        public static RTC4Wrap.clear_io_cond_listDelegate clear_io_cond_list = (RTC4Wrap.clear_io_cond_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.clear_io_cond_listDelegate>(nameof(clear_io_cond_list));
        /// <summary>
        ///  n_list_jump_cond(ushort n, ushort mask_1, ushort mask_0, ushort position);
        /// </summary>
        public static RTC4Wrap.n_list_jump_condDelegate n_list_jump_cond = (RTC4Wrap.n_list_jump_condDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_list_jump_condDelegate>(nameof(n_list_jump_cond));
        /// <summary>
        ///  list_jump_cond(ushort mask_1, ushort mask_0, ushort position);
        /// </summary>
        public static RTC4Wrap.list_jump_condDelegate list_jump_cond = (RTC4Wrap.list_jump_condDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.list_jump_condDelegate>(nameof(list_jump_cond));
        /// <summary>
        ///  n_list_call_cond(ushort n, ushort mask_1, ushort mask_0, ushort position);
        /// </summary>
        public static RTC4Wrap.n_list_call_condDelegate n_list_call_cond = (RTC4Wrap.n_list_call_condDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_list_call_condDelegate>(nameof(n_list_call_cond));
        /// <summary>
        ///  list_call_cond(ushort mask_1, ushort mask_0, ushort position);
        /// </summary>
        public static RTC4Wrap.list_call_condDelegate list_call_cond = (RTC4Wrap.list_call_condDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.list_call_condDelegate>(nameof(list_call_cond));
        /// <summary>n_save_and_restart_timer(ushort n);</summary>
        public static RTC4Wrap.n_save_and_restart_timerDelegate n_save_and_restart_timer = (RTC4Wrap.n_save_and_restart_timerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_save_and_restart_timerDelegate>(nameof(n_save_and_restart_timer));
        /// <summary>save_and_restart_timer();</summary>
        public static RTC4Wrap.save_and_restart_timerDelegate save_and_restart_timer = (RTC4Wrap.save_and_restart_timerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.save_and_restart_timerDelegate>(nameof(save_and_restart_timer));
        /// <summary>
        ///  n_set_ext_start_delay_list(ushort n, short delay, short encoder);
        /// </summary>
        public static RTC4Wrap.n_set_ext_start_delay_listDelegate n_set_ext_start_delay_list = (RTC4Wrap.n_set_ext_start_delay_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_ext_start_delay_listDelegate>(nameof(n_set_ext_start_delay_list));
        /// <summary>set_ext_start_delay_list(short delay, short encoder);</summary>
        public static RTC4Wrap.set_ext_start_delay_listDelegate set_ext_start_delay_list = (RTC4Wrap.set_ext_start_delay_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_ext_start_delay_listDelegate>(nameof(set_ext_start_delay_list));
        /// <summary>
        ///  n_set_trigger(ushort n, ushort sampleperiod, ushort channel1, ushort channel2);
        /// </summary>
        public static RTC4Wrap.n_set_triggerDelegate n_set_trigger = (RTC4Wrap.n_set_triggerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_triggerDelegate>(nameof(n_set_trigger));
        /// <summary>
        ///  set_trigger(ushort sampleperiod, ushort signal1, ushort signal2);
        /// </summary>
        public static RTC4Wrap.set_triggerDelegate set_trigger = (RTC4Wrap.set_triggerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_triggerDelegate>(nameof(set_trigger));
        /// <summary>n_arc_rel(ushort n, short dx, short dy, double angle);</summary>
        public static RTC4Wrap.n_arc_relDelegate n_arc_rel = (RTC4Wrap.n_arc_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_arc_relDelegate>(nameof(n_arc_rel));
        /// <summary>arc_rel(short dx, short dy, double angle);</summary>
        public static RTC4Wrap.arc_relDelegate arc_rel = (RTC4Wrap.arc_relDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.arc_relDelegate>(nameof(arc_rel));
        /// <summary>n_arc_abs(ushort n, short x, short y, double angle);</summary>
        public static RTC4Wrap.n_arc_absDelegate n_arc_abs = (RTC4Wrap.n_arc_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_arc_absDelegate>(nameof(n_arc_abs));
        /// <summary>arc_abs(short x, short y, double angle);</summary>
        public static RTC4Wrap.arc_absDelegate arc_abs = (RTC4Wrap.arc_absDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.arc_absDelegate>(nameof(arc_abs));
        /// <summary>drilling(short pulsewidth, short relencoderdelay);</summary>
        public static RTC4Wrap.drillingDelegate drilling = (RTC4Wrap.drillingDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.drillingDelegate>(nameof(drilling));
        /// <summary>regulation();</summary>
        public static RTC4Wrap.regulationDelegate regulation = (RTC4Wrap.regulationDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.regulationDelegate>(nameof(regulation));
        /// <summary>flyline(short encoderdelay);</summary>
        public static RTC4Wrap.flylineDelegate flyline = (RTC4Wrap.flylineDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.flylineDelegate>(nameof(flyline));
        /// <summary>ushort n_get_input_pointer(ushort n);</summary>
        public static RTC4Wrap.n_get_input_pointerDelegate n_get_input_pointer = (RTC4Wrap.n_get_input_pointerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_input_pointerDelegate>(nameof(n_get_input_pointer));
        /// <summary>ushort get_input_pointer();</summary>
        public static RTC4Wrap.get_input_pointerDelegate get_input_pointer = (RTC4Wrap.get_input_pointerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_input_pointerDelegate>(nameof(get_input_pointer));
        /// <summary>select_rtc(ushort cardno);</summary>
        public static RTC4Wrap.select_rtcDelegate select_rtc = (RTC4Wrap.select_rtcDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.select_rtcDelegate>(nameof(select_rtc));
        /// <summary>ushort rtc4_count_cards();</summary>
        public static RTC4Wrap.rtc4_count_cardsDelegate rtc4_count_cards = (RTC4Wrap.rtc4_count_cardsDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.rtc4_count_cardsDelegate>(nameof(rtc4_count_cards));
        /// <summary>
        ///  n_get_status(ushort n, out ushort busy, out ushort position);
        /// </summary>
        public static RTC4Wrap.n_get_statusDelegate n_get_status = (RTC4Wrap.n_get_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_statusDelegate>(nameof(n_get_status));
        /// <summary>get_status(out ushort busy, out ushort position);</summary>
        public static RTC4Wrap.get_statusDelegate get_status = (RTC4Wrap.get_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_statusDelegate>(nameof(get_status));
        /// <summary>ushort n_read_status(ushort n);</summary>
        public static RTC4Wrap.n_read_statusDelegate n_read_status = (RTC4Wrap.n_read_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_read_statusDelegate>(nameof(n_read_status));
        /// <summary>ushort read_status();</summary>
        public static RTC4Wrap.read_statusDelegate read_status = (RTC4Wrap.read_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.read_statusDelegate>(nameof(read_status));
        /// <summary>ushort n_get_startstop_info(ushort n);</summary>
        public static RTC4Wrap.n_get_startstop_infoDelegate n_get_startstop_info = (RTC4Wrap.n_get_startstop_infoDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_startstop_infoDelegate>(nameof(n_get_startstop_info));
        /// <summary>ushort get_startstop_info();</summary>
        public static RTC4Wrap.get_startstop_infoDelegate get_startstop_info = (RTC4Wrap.get_startstop_infoDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_startstop_infoDelegate>(nameof(get_startstop_info));
        /// <summary>ushort n_get_marking_info(ushort n);</summary>
        public static RTC4Wrap.n_get_marking_infoDelegate n_get_marking_info = (RTC4Wrap.n_get_marking_infoDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_marking_infoDelegate>(nameof(n_get_marking_info));
        /// <summary>ushort get_marking_info();</summary>
        public static RTC4Wrap.get_marking_infoDelegate get_marking_info = (RTC4Wrap.get_marking_infoDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_marking_infoDelegate>(nameof(get_marking_info));
        /// <summary>ushort get_dll_version();</summary>
        public static RTC4Wrap.get_dll_versionDelegate get_dll_version = (RTC4Wrap.get_dll_versionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_dll_versionDelegate>(nameof(get_dll_version));
        /// <summary>n_set_start_list_1(ushort n);</summary>
        public static RTC4Wrap.n_set_start_list_1Delegate n_set_start_list_1 = (RTC4Wrap.n_set_start_list_1Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_start_list_1Delegate>(nameof(n_set_start_list_1));
        /// <summary>set_start_list_1();</summary>
        public static RTC4Wrap.set_start_list_1Delegate set_start_list_1 = (RTC4Wrap.set_start_list_1Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_start_list_1Delegate>(nameof(set_start_list_1));
        /// <summary>n_set_start_list_2(ushort n);</summary>
        public static RTC4Wrap.n_set_start_list_2Delegate n_set_start_list_2 = (RTC4Wrap.n_set_start_list_2Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_start_list_2Delegate>(nameof(n_set_start_list_2));
        /// <summary>set_start_list_2();</summary>
        public static RTC4Wrap.set_start_list_2Delegate set_start_list_2 = (RTC4Wrap.set_start_list_2Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_start_list_2Delegate>(nameof(set_start_list_2));
        /// <summary>n_set_start_list(ushort n, ushort listno);</summary>
        public static RTC4Wrap.n_set_start_listDelegate n_set_start_list = (RTC4Wrap.n_set_start_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_start_listDelegate>(nameof(n_set_start_list));
        /// <summary>set_start_list(ushort listno);</summary>
        public static RTC4Wrap.set_start_listDelegate set_start_list = (RTC4Wrap.set_start_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_start_listDelegate>(nameof(set_start_list));
        /// <summary>n_execute_list_1(ushort n);</summary>
        public static RTC4Wrap.n_execute_list_1Delegate n_execute_list_1 = (RTC4Wrap.n_execute_list_1Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_execute_list_1Delegate>(nameof(n_execute_list_1));
        /// <summary>execute_list_1();</summary>
        public static RTC4Wrap.execute_list_1Delegate execute_list_1 = (RTC4Wrap.execute_list_1Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.execute_list_1Delegate>(nameof(execute_list_1));
        /// <summary>n_execute_list_2(ushort n);</summary>
        public static RTC4Wrap.n_execute_list_2Delegate n_execute_list_2 = (RTC4Wrap.n_execute_list_2Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_execute_list_2Delegate>(nameof(n_execute_list_2));
        /// <summary>execute_list_2();</summary>
        public static RTC4Wrap.execute_list_2Delegate execute_list_2 = (RTC4Wrap.execute_list_2Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.execute_list_2Delegate>(nameof(execute_list_2));
        /// <summary>n_execute_list(ushort n, ushort listno);</summary>
        public static RTC4Wrap.n_execute_listDelegate n_execute_list = (RTC4Wrap.n_execute_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_execute_listDelegate>(nameof(n_execute_list));
        /// <summary>execute_list(ushort listno);</summary>
        public static RTC4Wrap.execute_listDelegate execute_list = (RTC4Wrap.execute_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.execute_listDelegate>(nameof(execute_list));
        /// <summary>n_write_8bit_port(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_8bit_portDelegate n_write_8bit_port = (RTC4Wrap.n_write_8bit_portDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_8bit_portDelegate>(nameof(n_write_8bit_port));
        /// <summary>write_8bit_port(ushort value);</summary>
        public static RTC4Wrap.write_8bit_portDelegate write_8bit_port = (RTC4Wrap.write_8bit_portDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_8bit_portDelegate>(nameof(write_8bit_port));
        /// <summary>n_write_io_port(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_io_portDelegate n_write_io_port = (RTC4Wrap.n_write_io_portDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_io_portDelegate>(nameof(n_write_io_port));
        /// <summary>write_io_port(ushort value);</summary>
        public static RTC4Wrap.write_io_portDelegate write_io_port = (RTC4Wrap.write_io_portDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_io_portDelegate>(nameof(write_io_port));
        /// <summary>n_auto_change(ushort n);</summary>
        public static RTC4Wrap.n_auto_changeDelegate n_auto_change = (RTC4Wrap.n_auto_changeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_auto_changeDelegate>(nameof(n_auto_change));
        /// <summary>auto_change();</summary>
        public static RTC4Wrap.auto_changeDelegate auto_change = (RTC4Wrap.auto_changeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.auto_changeDelegate>(nameof(auto_change));
        /// <summary>n_auto_change_pos(ushort n, ushort start);</summary>
        public static RTC4Wrap.n_auto_change_posDelegate n_auto_change_pos = (RTC4Wrap.n_auto_change_posDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_auto_change_posDelegate>(nameof(n_auto_change_pos));
        /// <summary>auto_change_pos(ushort start);</summary>
        public static RTC4Wrap.auto_change_posDelegate auto_change_pos = (RTC4Wrap.auto_change_posDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.auto_change_posDelegate>(nameof(auto_change_pos));
        /// <summary>aut_change();</summary>
        public static RTC4Wrap.aut_changeDelegate aut_change = (RTC4Wrap.aut_changeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.aut_changeDelegate>(nameof(aut_change));
        /// <summary>n_start_loop(ushort n);</summary>
        public static RTC4Wrap.n_start_loopDelegate n_start_loop = (RTC4Wrap.n_start_loopDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_start_loopDelegate>(nameof(n_start_loop));
        /// <summary>start_loop();</summary>
        public static RTC4Wrap.start_loopDelegate start_loop = (RTC4Wrap.start_loopDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.start_loopDelegate>(nameof(start_loop));
        /// <summary>n_quit_loop(ushort n);</summary>
        public static RTC4Wrap.n_quit_loopDelegate n_quit_loop = (RTC4Wrap.n_quit_loopDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_quit_loopDelegate>(nameof(n_quit_loop));
        /// <summary>quit_loop();</summary>
        public static RTC4Wrap.quit_loopDelegate quit_loop = (RTC4Wrap.quit_loopDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.quit_loopDelegate>(nameof(quit_loop));
        /// <summary>n_set_list_mode(ushort n, ushort mode);</summary>
        public static RTC4Wrap.n_set_list_modeDelegate n_set_list_mode = (RTC4Wrap.n_set_list_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_list_modeDelegate>(nameof(n_set_list_mode));
        /// <summary>set_list_mode(ushort mode);</summary>
        public static RTC4Wrap.set_list_modeDelegate set_list_mode = (RTC4Wrap.set_list_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_list_modeDelegate>(nameof(set_list_mode));
        /// <summary>n_stop_execution(ushort n);</summary>
        public static RTC4Wrap.n_stop_executionDelegate n_stop_execution = (RTC4Wrap.n_stop_executionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_stop_executionDelegate>(nameof(n_stop_execution));
        /// <summary>stop_execution();</summary>
        public static RTC4Wrap.stop_executionDelegate stop_execution = (RTC4Wrap.stop_executionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.stop_executionDelegate>(nameof(stop_execution));
        /// <summary>ushort n_read_io_port(ushort n);</summary>
        public static RTC4Wrap.n_read_io_portDelegate n_read_io_port = (RTC4Wrap.n_read_io_portDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_read_io_portDelegate>(nameof(n_read_io_port));
        /// <summary>ushort read_io_port();</summary>
        public static RTC4Wrap.read_io_portDelegate read_io_port = (RTC4Wrap.read_io_portDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.read_io_portDelegate>(nameof(read_io_port));
        /// <summary>n_write_da_1(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_da_1Delegate n_write_da_1 = (RTC4Wrap.n_write_da_1Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_da_1Delegate>(nameof(n_write_da_1));
        /// <summary>write_da_1(ushort value);</summary>
        public static RTC4Wrap.write_da_1Delegate write_da_1 = (RTC4Wrap.write_da_1Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_da_1Delegate>(nameof(write_da_1));
        /// <summary>n_write_da_2(ushort n, ushort value);</summary>
        public static RTC4Wrap.n_write_da_2Delegate n_write_da_2 = (RTC4Wrap.n_write_da_2Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_da_2Delegate>(nameof(n_write_da_2));
        /// <summary>write_da_2(ushort value);</summary>
        public static RTC4Wrap.write_da_2Delegate write_da_2 = (RTC4Wrap.write_da_2Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_da_2Delegate>(nameof(write_da_2));
        /// <summary>n_set_max_counts(ushort n, int counts);</summary>
        public static RTC4Wrap.n_set_max_countsDelegate n_set_max_counts = (RTC4Wrap.n_set_max_countsDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_max_countsDelegate>(nameof(n_set_max_counts));
        /// <summary>set_max_counts(int counts);</summary>
        public static RTC4Wrap.set_max_countsDelegate set_max_counts = (RTC4Wrap.set_max_countsDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_max_countsDelegate>(nameof(set_max_counts));
        /// <summary>int n_get_counts(ushort n);</summary>
        public static RTC4Wrap.n_get_countsDelegate n_get_counts = (RTC4Wrap.n_get_countsDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_countsDelegate>(nameof(n_get_counts));
        /// <summary>int get_counts();</summary>
        public static RTC4Wrap.get_countsDelegate get_counts = (RTC4Wrap.get_countsDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_countsDelegate>(nameof(get_counts));
        /// <summary>
        ///  n_set_matrix(ushort n, double m11, double m12, double m21, double m22);
        /// </summary>
        public static RTC4Wrap.n_set_matrixDelegate n_set_matrix = (RTC4Wrap.n_set_matrixDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_matrixDelegate>(nameof(n_set_matrix));
        /// <summary>
        ///  set_matrix(double m11, double m12, double m21, double m22);
        /// </summary>
        public static RTC4Wrap.set_matrixDelegate set_matrix = (RTC4Wrap.set_matrixDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_matrixDelegate>(nameof(set_matrix));
        /// <summary>n_set_offset(ushort n, short xoffset, short yoffset);</summary>
        public static RTC4Wrap.n_set_offsetDelegate n_set_offset = (RTC4Wrap.n_set_offsetDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_offsetDelegate>(nameof(n_set_offset));
        /// <summary>set_offset(short xoffset, short yoffset);</summary>
        public static RTC4Wrap.set_offsetDelegate set_offset = (RTC4Wrap.set_offsetDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_offsetDelegate>(nameof(set_offset));
        /// <summary>n_goto_xyz(ushort n, short x, short y, short z);</summary>
        public static RTC4Wrap.n_goto_xyzDelegate n_goto_xyz = (RTC4Wrap.n_goto_xyzDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_goto_xyzDelegate>(nameof(n_goto_xyz));
        /// <summary>goto_xyz(short x, short y, short z);</summary>
        public static RTC4Wrap.goto_xyzDelegate goto_xyz = (RTC4Wrap.goto_xyzDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.goto_xyzDelegate>(nameof(goto_xyz));
        /// <summary>n_goto_xy(ushort n, short x, short y);</summary>
        public static RTC4Wrap.n_goto_xyDelegate n_goto_xy = (RTC4Wrap.n_goto_xyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_goto_xyDelegate>(nameof(n_goto_xy));
        /// <summary>goto_xy(short x, short y);</summary>
        public static RTC4Wrap.goto_xyDelegate goto_xy = (RTC4Wrap.goto_xyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.goto_xyDelegate>(nameof(goto_xy));
        /// <summary>ushort n_get_hex_version(ushort n);</summary>
        public static RTC4Wrap.n_get_hex_versionDelegate n_get_hex_version = (RTC4Wrap.n_get_hex_versionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_hex_versionDelegate>(nameof(n_get_hex_version));
        /// <summary>ushort get_hex_version();</summary>
        public static RTC4Wrap.get_hex_versionDelegate get_hex_version = (RTC4Wrap.get_hex_versionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_hex_versionDelegate>(nameof(get_hex_version));
        /// <summary>n_disable_laser(ushort n);</summary>
        public static RTC4Wrap.n_disable_laserDelegate n_disable_laser = (RTC4Wrap.n_disable_laserDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_disable_laserDelegate>(nameof(n_disable_laser));
        /// <summary>disable_laser();</summary>
        public static RTC4Wrap.disable_laserDelegate disable_laser = (RTC4Wrap.disable_laserDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.disable_laserDelegate>(nameof(disable_laser));
        /// <summary>n_enable_laser(ushort n);</summary>
        public static RTC4Wrap.n_enable_laserDelegate n_enable_laser = (RTC4Wrap.n_enable_laserDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_enable_laserDelegate>(nameof(n_enable_laser));
        /// <summary>enable_laser();</summary>
        public static RTC4Wrap.enable_laserDelegate enable_laser = (RTC4Wrap.enable_laserDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.enable_laserDelegate>(nameof(enable_laser));
        /// <summary>n_stop_list(ushort n);</summary>
        public static RTC4Wrap.n_stop_listDelegate n_stop_list = (RTC4Wrap.n_stop_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_stop_listDelegate>(nameof(n_stop_list));
        /// <summary>stop_list();</summary>
        public static RTC4Wrap.stop_listDelegate stop_list = (RTC4Wrap.stop_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.stop_listDelegate>(nameof(stop_list));
        /// <summary>n_restart_list(ushort n);</summary>
        public static RTC4Wrap.n_restart_listDelegate n_restart_list = (RTC4Wrap.n_restart_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_restart_listDelegate>(nameof(n_restart_list));
        /// <summary>restart_list();</summary>
        public static RTC4Wrap.restart_listDelegate restart_list = (RTC4Wrap.restart_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.restart_listDelegate>(nameof(restart_list));
        /// <summary>
        ///  n_get_xyz_pos(ushort n, out short x, out short y, out short z);
        /// </summary>
        public static RTC4Wrap.n_get_xyz_posDelegate n_get_xyz_pos = (RTC4Wrap.n_get_xyz_posDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_xyz_posDelegate>(nameof(n_get_xyz_pos));
        /// <summary>get_xyz_pos(out short x, out short y, out short z);</summary>
        public static RTC4Wrap.get_xyz_posDelegate get_xyz_pos = (RTC4Wrap.get_xyz_posDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_xyz_posDelegate>(nameof(get_xyz_pos));
        /// <summary>n_get_xy_pos(ushort n, out short x, out short y);</summary>
        public static RTC4Wrap.n_get_xy_posDelegate n_get_xy_pos = (RTC4Wrap.n_get_xy_posDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_xy_posDelegate>(nameof(n_get_xy_pos));
        /// <summary>get_xy_pos(out short x, out short y);</summary>
        public static RTC4Wrap.get_xy_posDelegate get_xy_pos = (RTC4Wrap.get_xy_posDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_xy_posDelegate>(nameof(get_xy_pos));
        /// <summary>n_select_list(ushort n, ushort list_2);</summary>
        public static RTC4Wrap.n_select_listDelegate n_select_list = (RTC4Wrap.n_select_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_select_listDelegate>(nameof(n_select_list));
        /// <summary>select_list(ushort list_2);</summary>
        public static RTC4Wrap.select_listDelegate select_list = (RTC4Wrap.select_listDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.select_listDelegate>(nameof(select_list));
        /// <summary>n_z_out(ushort n, short z);</summary>
        public static RTC4Wrap.n_z_outDelegate n_z_out = (RTC4Wrap.n_z_outDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_z_outDelegate>(nameof(n_z_out));
        /// <summary>z_out(short z);</summary>
        public static RTC4Wrap.z_outDelegate z_out = (RTC4Wrap.z_outDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.z_outDelegate>(nameof(z_out));
        /// <summary>n_set_firstpulse_killer(ushort n, ushort fpk);</summary>
        public static RTC4Wrap.n_set_firstpulse_killerDelegate n_set_firstpulse_killer = (RTC4Wrap.n_set_firstpulse_killerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_firstpulse_killerDelegate>(nameof(n_set_firstpulse_killer));
        /// <summary>set_firstpulse_killer(ushort fpk);</summary>
        public static RTC4Wrap.set_firstpulse_killerDelegate set_firstpulse_killer = (RTC4Wrap.set_firstpulse_killerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_firstpulse_killerDelegate>(nameof(set_firstpulse_killer));
        /// <summary>
        ///  n_set_standby(ushort n, ushort half_period, ushort pulse);
        /// </summary>
        public static RTC4Wrap.n_set_standbyDelegate n_set_standby = (RTC4Wrap.n_set_standbyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_standbyDelegate>(nameof(n_set_standby));
        /// <summary>set_standby(ushort half_period, ushort pulse);</summary>
        public static RTC4Wrap.set_standbyDelegate set_standby = (RTC4Wrap.set_standbyDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_standbyDelegate>(nameof(set_standby));
        /// <summary>n_laser_signal_on(ushort n);</summary>
        public static RTC4Wrap.n_laser_signal_onDelegate n_laser_signal_on = (RTC4Wrap.n_laser_signal_onDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_laser_signal_onDelegate>(nameof(n_laser_signal_on));
        /// <summary>laser_signal_on();</summary>
        public static RTC4Wrap.laser_signal_onDelegate laser_signal_on = (RTC4Wrap.laser_signal_onDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.laser_signal_onDelegate>(nameof(laser_signal_on));
        /// <summary>n_laser_signal_off(ushort n);</summary>
        public static RTC4Wrap.n_laser_signal_offDelegate n_laser_signal_off = (RTC4Wrap.n_laser_signal_offDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_laser_signal_offDelegate>(nameof(n_laser_signal_off));
        /// <summary>laser_signal_off();</summary>
        public static RTC4Wrap.laser_signal_offDelegate laser_signal_off = (RTC4Wrap.laser_signal_offDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.laser_signal_offDelegate>(nameof(laser_signal_off));
        /// <summary>
        ///  n_set_delay_mode(ushort n, ushort varpoly, ushort directmove3d, ushort edgelevel, ushort minjumpdelay, ushort jumplengthlimit);
        /// </summary>
        public static RTC4Wrap.n_set_delay_modeDelegate n_set_delay_mode = (RTC4Wrap.n_set_delay_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_delay_modeDelegate>(nameof(n_set_delay_mode));
        /// <summary>
        ///  set_delay_mode(ushort varpoly, ushort directmove3d, ushort edgelevel, ushort minjumpdelay, ushort jumplengthlimit);
        /// </summary>
        public static RTC4Wrap.set_delay_modeDelegate set_delay_mode = (RTC4Wrap.set_delay_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_delay_modeDelegate>(nameof(set_delay_mode));
        /// <summary>n_set_piso_control(ushort n, ushort l1, ushort l2);</summary>
        public static RTC4Wrap.n_set_piso_controlDelegate n_set_piso_control = (RTC4Wrap.n_set_piso_controlDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_piso_controlDelegate>(nameof(n_set_piso_control));
        /// <summary>set_piso_control(ushort l1, ushort l2);</summary>
        public static RTC4Wrap.set_piso_controlDelegate set_piso_control = (RTC4Wrap.set_piso_controlDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_piso_controlDelegate>(nameof(set_piso_control));
        /// <summary>n_select_status(ushort n, ushort mode);</summary>
        public static RTC4Wrap.n_select_statusDelegate n_select_status = (RTC4Wrap.n_select_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_select_statusDelegate>(nameof(n_select_status));
        /// <summary>select_status(ushort mode);</summary>
        public static RTC4Wrap.select_statusDelegate select_status = (RTC4Wrap.select_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.select_statusDelegate>(nameof(select_status));
        /// <summary>n_get_encoder(ushort n, out short zx, out short zy);</summary>
        public static RTC4Wrap.n_get_encoderDelegate n_get_encoder = (RTC4Wrap.n_get_encoderDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_encoderDelegate>(nameof(n_get_encoder));
        /// <summary>get_encoder(out short zx, out short zy);</summary>
        public static RTC4Wrap.get_encoderDelegate get_encoder = (RTC4Wrap.get_encoderDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_encoderDelegate>(nameof(get_encoder));
        /// <summary>
        ///  n_select_cor_table(ushort n, ushort heada, ushort headb);
        /// </summary>
        public static RTC4Wrap.n_select_cor_tableDelegate n_select_cor_table = (RTC4Wrap.n_select_cor_tableDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_select_cor_tableDelegate>(nameof(n_select_cor_table));
        /// <summary>select_cor_table(ushort heada, ushort headb);</summary>
        public static RTC4Wrap.select_cor_tableDelegate select_cor_table = (RTC4Wrap.select_cor_tableDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.select_cor_tableDelegate>(nameof(select_cor_table));
        /// <summary>n_execute_at_pointer(ushort n, ushort position);</summary>
        public static RTC4Wrap.n_execute_at_pointerDelegate n_execute_at_pointer = (RTC4Wrap.n_execute_at_pointerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_execute_at_pointerDelegate>(nameof(n_execute_at_pointer));
        /// <summary>execute_at_pointer(ushort position);</summary>
        public static RTC4Wrap.execute_at_pointerDelegate execute_at_pointer = (RTC4Wrap.execute_at_pointerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.execute_at_pointerDelegate>(nameof(execute_at_pointer));
        /// <summary>ushort n_get_head_status(ushort n, ushort head);</summary>
        public static RTC4Wrap.n_get_head_statusDelegate n_get_head_status = (RTC4Wrap.n_get_head_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_head_statusDelegate>(nameof(n_get_head_status));
        /// <summary>ushort get_head_status(ushort head);</summary>
        public static RTC4Wrap.get_head_statusDelegate get_head_status = (RTC4Wrap.get_head_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_head_statusDelegate>(nameof(get_head_status));
        /// <summary>n_simulate_encoder(ushort n, ushort channel);</summary>
        public static RTC4Wrap.n_simulate_encoderDelegate n_simulate_encoder = (RTC4Wrap.n_simulate_encoderDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_simulate_encoderDelegate>(nameof(n_simulate_encoder));
        /// <summary>simulate_encoder(ushort channel);</summary>
        public static RTC4Wrap.simulate_encoderDelegate simulate_encoder = (RTC4Wrap.simulate_encoderDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.simulate_encoderDelegate>(nameof(simulate_encoder));
        /// <summary>
        ///  n_set_hi(ushort n, double galvogainx, double galvogainy, short galvooffsetx, short galvooffsety, short head);
        /// </summary>
        public static RTC4Wrap.n_set_hiDelegate n_set_hi = (RTC4Wrap.n_set_hiDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_hiDelegate>(nameof(n_set_hi));
        /// <summary>
        ///  set_hi(double galvogainx, double galvogainy, short galvooffsetx, short galvooffsety, short head);
        /// </summary>
        public static RTC4Wrap.set_hiDelegate set_hi = (RTC4Wrap.set_hiDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_hiDelegate>(nameof(set_hi));
        /// <summary>n_release_wait(ushort n);</summary>
        public static RTC4Wrap.n_release_waitDelegate n_release_wait = (RTC4Wrap.n_release_waitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_release_waitDelegate>(nameof(n_release_wait));
        /// <summary>release_wait();</summary>
        public static RTC4Wrap.release_waitDelegate release_wait = (RTC4Wrap.release_waitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.release_waitDelegate>(nameof(release_wait));
        /// <summary>ushort n_get_wait_status(ushort n);</summary>
        public static RTC4Wrap.n_get_wait_statusDelegate n_get_wait_status = (RTC4Wrap.n_get_wait_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_wait_statusDelegate>(nameof(n_get_wait_status));
        /// <summary>ushort get_wait_status();</summary>
        public static RTC4Wrap.get_wait_statusDelegate get_wait_status = (RTC4Wrap.get_wait_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_wait_statusDelegate>(nameof(get_wait_status));
        /// <summary>n_set_control_mode(ushort n, ushort mode);</summary>
        public static RTC4Wrap.n_set_control_modeDelegate n_set_control_mode = (RTC4Wrap.n_set_control_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_control_modeDelegate>(nameof(n_set_control_mode));
        /// <summary>set_control_mode(ushort mode);</summary>
        public static RTC4Wrap.set_control_modeDelegate set_control_mode = (RTC4Wrap.set_control_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_control_modeDelegate>(nameof(set_control_mode));
        /// <summary>n_set_laser_mode(ushort n, ushort mode);</summary>
        public static RTC4Wrap.n_set_laser_modeDelegate n_set_laser_mode = (RTC4Wrap.n_set_laser_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_laser_modeDelegate>(nameof(n_set_laser_mode));
        /// <summary>set_laser_mode(ushort mode);</summary>
        public static RTC4Wrap.set_laser_modeDelegate set_laser_mode = (RTC4Wrap.set_laser_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_laser_modeDelegate>(nameof(set_laser_mode));
        /// <summary>
        ///  n_set_ext_start_delay(ushort n, short delay, short encoder);
        /// </summary>
        public static RTC4Wrap.n_set_ext_start_delayDelegate n_set_ext_start_delay = (RTC4Wrap.n_set_ext_start_delayDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_ext_start_delayDelegate>(nameof(n_set_ext_start_delay));
        /// <summary>set_ext_start_delay(short delay, short encoder);</summary>
        public static RTC4Wrap.set_ext_start_delayDelegate set_ext_start_delay = (RTC4Wrap.set_ext_start_delayDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_ext_start_delayDelegate>(nameof(set_ext_start_delay));
        /// <summary>n_home_position(ushort n, short xhome, short yhome);</summary>
        public static RTC4Wrap.n_home_positionDelegate n_home_position = (RTC4Wrap.n_home_positionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_home_positionDelegate>(nameof(n_home_position));
        /// <summary>home_position(short xhome, short yhome);</summary>
        public static RTC4Wrap.home_positionDelegate home_position = (RTC4Wrap.home_positionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.home_positionDelegate>(nameof(home_position));
        /// <summary>
        ///  n_set_rot_center(ushort n, int center_x, int center_y);
        /// </summary>
        public static RTC4Wrap.n_set_rot_centerDelegate n_set_rot_center = (RTC4Wrap.n_set_rot_centerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_rot_centerDelegate>(nameof(n_set_rot_center));
        /// <summary>set_rot_center(int center_x, int center_y);</summary>
        public static RTC4Wrap.set_rot_centerDelegate set_rot_center = (RTC4Wrap.set_rot_centerDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_rot_centerDelegate>(nameof(set_rot_center));
        /// <summary>n_dsp_start(ushort n);</summary>
        public static RTC4Wrap.n_dsp_startDelegate n_dsp_start = (RTC4Wrap.n_dsp_startDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_dsp_startDelegate>(nameof(n_dsp_start));
        /// <summary>dsp_start();</summary>
        public static RTC4Wrap.dsp_startDelegate dsp_start = (RTC4Wrap.dsp_startDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.dsp_startDelegate>(nameof(dsp_start));
        /// <summary>n_write_da_x(ushort n, ushort x, ushort value);</summary>
        public static RTC4Wrap.n_write_da_xDelegate n_write_da_x = (RTC4Wrap.n_write_da_xDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_write_da_xDelegate>(nameof(n_write_da_x));
        /// <summary>write_da_x(ushort x, ushort value);</summary>
        public static RTC4Wrap.write_da_xDelegate write_da_x = (RTC4Wrap.write_da_xDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.write_da_xDelegate>(nameof(write_da_x));
        /// <summary>ushort n_read_ad_x(ushort n, ushort x);</summary>
        public static RTC4Wrap.n_read_ad_xDelegate n_read_ad_x = (RTC4Wrap.n_read_ad_xDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_read_ad_xDelegate>(nameof(n_read_ad_x));
        /// <summary>ushort read_ad_x(ushort x);</summary>
        public static RTC4Wrap.read_ad_xDelegate read_ad_x = (RTC4Wrap.read_ad_xDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.read_ad_xDelegate>(nameof(read_ad_x));
        /// <summary>ushort n_read_pixel_ad(ushort n, ushort pos);</summary>
        public static RTC4Wrap.n_read_pixel_adDelegate n_read_pixel_ad = (RTC4Wrap.n_read_pixel_adDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_read_pixel_adDelegate>(nameof(n_read_pixel_ad));
        /// <summary>ushort read_pixel_ad(ushort pos);</summary>
        public static RTC4Wrap.read_pixel_adDelegate read_pixel_ad = (RTC4Wrap.read_pixel_adDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.read_pixel_adDelegate>(nameof(read_pixel_ad));
        /// <summary>
        ///  short n_get_z_distance(ushort n, short x, short y, short z);
        /// </summary>
        public static RTC4Wrap.n_get_z_distanceDelegate n_get_z_distance = (RTC4Wrap.n_get_z_distanceDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_z_distanceDelegate>(nameof(n_get_z_distance));
        /// <summary>short get_z_distance(short x, short y, short z);</summary>
        public static RTC4Wrap.get_z_distanceDelegate get_z_distance = (RTC4Wrap.get_z_distanceDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_z_distanceDelegate>(nameof(get_z_distance));
        /// <summary>ushort n_get_io_status(ushort n);</summary>
        public static RTC4Wrap.n_get_io_statusDelegate n_get_io_status = (RTC4Wrap.n_get_io_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_io_statusDelegate>(nameof(n_get_io_status));
        /// <summary>ushort get_io_status();</summary>
        public static RTC4Wrap.get_io_statusDelegate get_io_status = (RTC4Wrap.get_io_statusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_io_statusDelegate>(nameof(get_io_status));
        /// <summary>double n_get_time(ushort n);</summary>
        public static RTC4Wrap.n_get_timeDelegate n_get_time = (RTC4Wrap.n_get_timeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_timeDelegate>(nameof(n_get_time));
        /// <summary>double get_time();</summary>
        public static RTC4Wrap.get_timeDelegate get_time = (RTC4Wrap.get_timeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_timeDelegate>(nameof(get_time));
        /// <summary>n_set_defocus(ushort n, short value);</summary>
        public static RTC4Wrap.n_set_defocusDelegate n_set_defocus = (RTC4Wrap.n_set_defocusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_defocusDelegate>(nameof(n_set_defocus));
        /// <summary>set_defocus(short value);</summary>
        public static RTC4Wrap.set_defocusDelegate set_defocus = (RTC4Wrap.set_defocusDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_defocusDelegate>(nameof(set_defocus));
        /// <summary>
        ///  n_set_softstart_mode(ushort n, ushort mode, ushort number, ushort restartdelay);
        /// </summary>
        public static RTC4Wrap.n_set_softstart_modeDelegate n_set_softstart_mode = (RTC4Wrap.n_set_softstart_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_softstart_modeDelegate>(nameof(n_set_softstart_mode));
        /// <summary>
        ///  set_softstart_mode(ushort mode, ushort number, ushort resetdelay);
        /// </summary>
        public static RTC4Wrap.set_softstart_modeDelegate set_softstart_mode = (RTC4Wrap.set_softstart_modeDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_softstart_modeDelegate>(nameof(set_softstart_mode));
        /// <summary>
        ///  n_set_softstart_level(ushort n, ushort index, ushort level);
        /// </summary>
        public static RTC4Wrap.n_set_softstart_levelDelegate n_set_softstart_level = (RTC4Wrap.n_set_softstart_levelDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_softstart_levelDelegate>(nameof(n_set_softstart_level));
        /// <summary>set_softstart_level(ushort index, ushort level);</summary>
        public static RTC4Wrap.set_softstart_levelDelegate set_softstart_level = (RTC4Wrap.set_softstart_levelDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_softstart_levelDelegate>(nameof(set_softstart_level));
        /// <summary>
        ///  n_control_command(ushort n, ushort head, ushort axis, ushort data);
        /// </summary>
        public static RTC4Wrap.n_control_commandDelegate n_control_command = (RTC4Wrap.n_control_commandDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_control_commandDelegate>(nameof(n_control_command));
        /// <summary>
        ///  control_command(ushort head, ushort axis, ushort data);
        /// </summary>
        public static RTC4Wrap.control_commandDelegate control_command = (RTC4Wrap.control_commandDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.control_commandDelegate>(nameof(control_command));
        /// <summary>short load_cor(string filename);</summary>
        public static RTC4Wrap.load_corDelegate load_cor = (RTC4Wrap.load_corDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.load_corDelegate>(nameof(load_cor));
        /// <summary>short load_pro(string filename);</summary>
        public static RTC4Wrap.load_proDelegate load_pro = (RTC4Wrap.load_proDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.load_proDelegate>(nameof(load_pro));
        /// <summary>ushort n_get_serial_number(ushort n);</summary>
        public static RTC4Wrap.n_get_serial_numberDelegate n_get_serial_number = (RTC4Wrap.n_get_serial_numberDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_serial_numberDelegate>(nameof(n_get_serial_number));
        /// <summary>ushort get_serial_number();</summary>
        public static RTC4Wrap.get_serial_numberDelegate get_serial_number = (RTC4Wrap.get_serial_numberDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_serial_numberDelegate>(nameof(get_serial_number));
        /// <summary>int n_get_serial_number_32(ushort n);</summary>
        public static RTC4Wrap.n_get_serial_number_32Delegate n_get_serial_number_32 = (RTC4Wrap.n_get_serial_number_32Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_serial_number_32Delegate>(nameof(n_get_serial_number_32));
        /// <summary>int get_serial_number_32();</summary>
        public static RTC4Wrap.get_serial_number_32Delegate get_serial_number_32 = (RTC4Wrap.get_serial_number_32Delegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_serial_number_32Delegate>(nameof(get_serial_number_32));
        /// <summary>ushort n_get_rtc_version(ushort n);</summary>
        public static RTC4Wrap.n_get_rtc_versionDelegate n_get_rtc_version = (RTC4Wrap.n_get_rtc_versionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_rtc_versionDelegate>(nameof(n_get_rtc_version));
        /// <summary>ushort get_rtc_version();</summary>
        public static RTC4Wrap.get_rtc_versionDelegate get_rtc_version = (RTC4Wrap.get_rtc_versionDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_rtc_versionDelegate>(nameof(get_rtc_version));
        /// <summary>
        ///  get_hi_data(out ushort x1, out ushort x2, out ushort y1, out ushort y2);
        /// </summary>
        public static RTC4Wrap.get_hi_dataDelegate get_hi_data = (RTC4Wrap.get_hi_dataDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_hi_dataDelegate>(nameof(get_hi_data));
        /// <summary>
        ///  short n_auto_cal(ushort n, ushort head, ushort command);
        /// </summary>
        public static RTC4Wrap.n_auto_calDelegate n_auto_cal = (RTC4Wrap.n_auto_calDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_auto_calDelegate>(nameof(n_auto_cal));
        /// <summary>short auto_cal(ushort head, ushort command);</summary>
        public static RTC4Wrap.auto_calDelegate auto_cal = (RTC4Wrap.auto_calDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.auto_calDelegate>(nameof(auto_cal));
        /// <summary>ushort n_get_list_space(ushort n);</summary>
        public static RTC4Wrap.n_get_list_spaceDelegate n_get_list_space = (RTC4Wrap.n_get_list_spaceDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_list_spaceDelegate>(nameof(n_get_list_space));
        /// <summary>ushort get_list_space();</summary>
        public static RTC4Wrap.get_list_spaceDelegate get_list_space = (RTC4Wrap.get_list_spaceDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_list_spaceDelegate>(nameof(get_list_space));
        /// <summary>
        ///  short teachin(string filename, short xin, short yin, short zin, double ll0, out short xout, out short yout, out short zout);
        /// </summary>
        public static RTC4Wrap.teachinDelegate teachin = (RTC4Wrap.teachinDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.teachinDelegate>(nameof(teachin));
        /// <summary>short n_get_value(ushort n, ushort signal);</summary>
        public static RTC4Wrap.n_get_valueDelegate n_get_value = (RTC4Wrap.n_get_valueDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_get_valueDelegate>(nameof(n_get_value));
        /// <summary>short get_value(ushort signal);</summary>
        public static RTC4Wrap.get_valueDelegate get_value = (RTC4Wrap.get_valueDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.get_valueDelegate>(nameof(get_value));
        /// <summary>n_set_io_bit(ushort n, ushort mask1);</summary>
        public static RTC4Wrap.n_set_io_bitDelegate n_set_io_bit = (RTC4Wrap.n_set_io_bitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_set_io_bitDelegate>(nameof(n_set_io_bit));
        /// <summary>set_io_bit(ushort mask1);</summary>
        public static RTC4Wrap.set_io_bitDelegate set_io_bit = (RTC4Wrap.set_io_bitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_io_bitDelegate>(nameof(set_io_bit));
        /// <summary>n_clear_io_bit(ushort n, ushort mask0);</summary>
        public static RTC4Wrap.n_clear_io_bitDelegate n_clear_io_bit = (RTC4Wrap.n_clear_io_bitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_clear_io_bitDelegate>(nameof(n_clear_io_bit));
        /// <summary>clear_io_bit(ushort mask0);</summary>
        public static RTC4Wrap.clear_io_bitDelegate clear_io_bit = (RTC4Wrap.clear_io_bitDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.clear_io_bitDelegate>(nameof(clear_io_bit));
        /// <summary>set_duty_cycle_table(ushort index, ushort dutycycle);</summary>
        public static RTC4Wrap.set_duty_cycle_tableDelegate set_duty_cycle_table = (RTC4Wrap.set_duty_cycle_tableDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.set_duty_cycle_tableDelegate>(nameof(set_duty_cycle_table));
        /// <summary>n_move_to(ushort n, ushort position);</summary>
        public static RTC4Wrap.n_move_toDelegate n_move_to = (RTC4Wrap.n_move_toDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.n_move_toDelegate>(nameof(n_move_to));
        /// <summary>move_to(ushort position);</summary>
        public static RTC4Wrap.move_toDelegate move_to = (RTC4Wrap.move_toDelegate)RTC4Wrap.FunctionImporter.Import<RTC4Wrap.move_toDelegate>(nameof(move_to));

        private class FunctionImporter
        {
            private static string RTC4DLL;
            private static IntPtr hModule;
            private static RTC4Wrap.FunctionImporter instance;

            [DllImport("Kernel32.dll")]
            private static extern IntPtr LoadLibrary(string path);

            [DllImport("kernel32.dll")]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("Kernel32.dll")]
            private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            protected FunctionImporter(string DllName) => RTC4Wrap.FunctionImporter.hModule = RTC4Wrap.FunctionImporter.LoadLibrary(DllName);

            ~FunctionImporter()
            {
                if (!(RTC4Wrap.FunctionImporter.hModule != IntPtr.Zero))
                    return;
                RTC4Wrap.FunctionImporter.FreeLibrary(RTC4Wrap.FunctionImporter.hModule);
            }

            public static Delegate Import<T>(string functionName)
            {
                if (RTC4Wrap.FunctionImporter.instance == null)
                {
                    RTC4Wrap.FunctionImporter.RTC4DLL = Marshal.SizeOf(typeof(IntPtr)) == 4 ? "RTC4DLL.dll" : "RTC4DLLx64.dll";
                    RTC4Wrap.FunctionImporter.instance = new RTC4Wrap.FunctionImporter(RTC4Wrap.FunctionImporter.RTC4DLL);
                    if (RTC4Wrap.FunctionImporter.hModule == IntPtr.Zero)
                        throw new FileNotFoundException(RTC4Wrap.FunctionImporter.RTC4DLL + " not found. ");
                }
                IntPtr procAddress = RTC4Wrap.FunctionImporter.GetProcAddress(RTC4Wrap.FunctionImporter.hModule, functionName);
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

        public delegate short getmemoryDelegate(ushort adr);

        public delegate void n_get_waveformDelegate(
          ushort n,
          ushort channel,
          ushort istop,
          [MarshalAs(UnmanagedType.LPArray, SizeConst = 32768)] short[] memptr);

        public delegate void get_waveformDelegate(ushort channel, ushort istop, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32768)] short[] memptr);

        public delegate void n_measurement_statusDelegate(
          ushort n,
          out ushort busy,
          out ushort position);

        public delegate void measurement_statusDelegate(out ushort busy, out ushort position);

        public delegate short n_load_varpolydelayDelegate(ushort n, string stbfilename, ushort tableno);

        public delegate short load_varpolydelayDelegate(string stbfilename, ushort tableno);

        public delegate short n_load_program_fileDelegate(ushort n, string name);

        public delegate short load_program_fileDelegate(string name);

        public delegate short n_load_correction_fileDelegate(
          ushort n,
          string filename,
          short cortable,
          double kx,
          double ky,
          double phi,
          double xoffset,
          double yoffset);

        public delegate short load_correction_fileDelegate(
          string filename,
          short cortable,
          double kx,
          double ky,
          double phi,
          double xoffset,
          double yoffset);

        public delegate short n_load_z_tableDelegate(ushort n, double a, double b, double c);

        public delegate short load_z_tableDelegate(double a, double b, double c);

        public delegate void n_list_nopDelegate(ushort n);

        public delegate void list_nopDelegate();

        public delegate void n_set_end_of_listDelegate(ushort n);

        public delegate void set_end_of_listDelegate();

        public delegate void n_jump_abs_3dDelegate(ushort n, short x, short y, short z);

        public delegate void jump_abs_3dDelegate(short x, short y, short z);

        public delegate void n_jump_absDelegate(ushort n, short x, short y);

        public delegate void jump_absDelegate(short x, short y);

        public delegate void n_mark_abs_3dDelegate(ushort n, short x, short y, short z);

        public delegate void mark_abs_3dDelegate(short x, short y, short z);

        public delegate void n_mark_absDelegate(ushort n, short x, short y);

        public delegate void mark_absDelegate(short x, short y);

        public delegate void n_jump_rel_3dDelegate(ushort n, short dx, short dy, short dz);

        public delegate void jump_rel_3dDelegate(short dx, short dy, short dz);

        public delegate void n_jump_relDelegate(ushort n, short dx, short dy);

        public delegate void jump_relDelegate(short dx, short dy);

        public delegate void n_mark_rel_3dDelegate(ushort n, short dx, short dy, short dz);

        public delegate void mark_rel_3dDelegate(short dx, short dy, short dz);

        public delegate void n_mark_relDelegate(ushort n, short dx, short dy);

        public delegate void mark_relDelegate(short dx, short dy);

        public delegate void n_write_8bit_port_listDelegate(ushort n, ushort value);

        public delegate void write_8bit_port_listDelegate(ushort value);

        public delegate void n_write_da_1_listDelegate(ushort n, ushort value);

        public delegate void write_da_1_listDelegate(ushort value);

        public delegate void n_write_da_2_listDelegate(ushort n, ushort value);

        public delegate void write_da_2_listDelegate(ushort value);

        public delegate void n_set_matrix_listDelegate(ushort n, ushort i, ushort j, double mij);

        public delegate void set_matrix_listDelegate(ushort i, ushort j, double mij);

        public delegate void n_set_defocus_listDelegate(ushort n, short value);

        public delegate void set_defocus_listDelegate(short value);

        public delegate void n_set_control_mode_listDelegate(ushort n, ushort mode);

        public delegate void set_control_mode_listDelegate(ushort mode);

        public delegate void n_set_offset_listDelegate(ushort n, short xoffset, short yoffset);

        public delegate void set_offset_listDelegate(short xoffset, short yoffset);

        public delegate void n_long_delayDelegate(ushort n, ushort value);

        public delegate void long_delayDelegate(ushort value);

        public delegate void n_laser_on_listDelegate(ushort n, ushort value);

        public delegate void laser_on_listDelegate(ushort value);

        public delegate void n_set_jump_speedDelegate(ushort n, double speed);

        public delegate void set_jump_speedDelegate(double speed);

        public delegate void n_set_mark_speedDelegate(ushort n, double speed);

        public delegate void set_mark_speedDelegate(double speed);

        public delegate void n_set_laser_delaysDelegate(ushort n, short ondelay, short offdelay);

        public delegate void set_laser_delaysDelegate(short ondelay, short offdelay);

        public delegate void n_set_scanner_delaysDelegate(
          ushort n,
          ushort jumpdelay,
          ushort markdelay,
          ushort polydelay);

        public delegate void set_scanner_delaysDelegate(
          ushort jumpdelay,
          ushort markdelay,
          ushort polydelay);

        public delegate void n_set_list_jumpDelegate(ushort n, ushort position);

        public delegate void set_list_jumpDelegate(ushort position);

        public delegate void n_set_input_pointerDelegate(ushort n, ushort pointer);

        public delegate void set_input_pointerDelegate(ushort pointer);

        public delegate void n_list_callDelegate(ushort n, ushort position);

        public delegate void list_callDelegate(ushort position);

        public delegate void n_list_returnDelegate(ushort n);

        public delegate void list_returnDelegate();

        public delegate void n_z_out_listDelegate(ushort n, short z);

        public delegate void z_out_listDelegate(short z);

        public delegate void n_set_standby_listDelegate(ushort n, ushort half_period, ushort pulse);

        public delegate void set_standby_listDelegate(ushort half_period, ushort pulse);

        public delegate void n_timed_jump_absDelegate(ushort n, short x, short y, double time);

        public delegate void timed_jump_absDelegate(short x, short y, double time);

        public delegate void n_timed_mark_absDelegate(ushort n, short x, short y, double time);

        public delegate void timed_mark_absDelegate(short x, short y, double time);

        public delegate void n_timed_jump_relDelegate(ushort n, short dx, short dy, double time);

        public delegate void timed_jump_relDelegate(short dx, short dy, double time);

        public delegate void n_timed_mark_relDelegate(ushort n, short dx, short dy, double time);

        public delegate void timed_mark_relDelegate(short dx, short dy, double time);

        public delegate void n_set_laser_timingDelegate(
          ushort n,
          ushort halfperiod,
          ushort pulse1,
          ushort pulse2,
          ushort timebase);

        public delegate void set_laser_timingDelegate(
          ushort halfperiod,
          ushort pulse1,
          ushort pulse2,
          ushort timebase);

        public delegate void n_set_wobbel_xyDelegate(
          ushort n,
          ushort long_wob,
          ushort trans_wob,
          double frequency);

        public delegate void set_wobbel_xyDelegate(ushort long_wob, ushort trans_wob, double frequency);

        public delegate void n_set_wobbelDelegate(ushort n, ushort amplitude, double frequency);

        public delegate void set_wobbelDelegate(ushort amplitude, double frequency);

        public delegate void n_set_fly_xDelegate(ushort n, double kx);

        public delegate void set_fly_xDelegate(double kx);

        public delegate void n_set_fly_yDelegate(ushort n, double ky);

        public delegate void set_fly_yDelegate(double ky);

        public delegate void n_set_fly_rotDelegate(ushort n, double resolution);

        public delegate void set_fly_rotDelegate(double resolution);

        public delegate void n_fly_returnDelegate(ushort n, short x, short y);

        public delegate void fly_returnDelegate(short x, short y);

        public delegate void n_calculate_flyDelegate(ushort n, ushort direction, double distance);

        public delegate void calculate_flyDelegate(ushort direction, double distance);

        public delegate void n_write_io_port_listDelegate(ushort n, ushort value);

        public delegate void write_io_port_listDelegate(ushort value);

        public delegate void n_select_cor_table_listDelegate(ushort n, ushort heada, ushort headb);

        public delegate void select_cor_table_listDelegate(ushort heada, ushort headb);

        public delegate void n_set_waitDelegate(ushort n, ushort value);

        public delegate void set_waitDelegate(ushort value);

        public delegate void n_simulate_ext_startDelegate(ushort n, short delay, short encoder);

        public delegate void simulate_ext_startDelegate(short delay, short encoder);

        public delegate void n_write_da_x_listDelegate(ushort n, ushort x, ushort value);

        public delegate void write_da_x_listDelegate(ushort x, ushort value);

        public delegate void n_set_pixel_lineDelegate(
          ushort n,
          ushort pixelmode,
          ushort pixelperiod,
          double dx,
          double dy);

        public delegate void set_pixel_lineDelegate(
          ushort pixelmode,
          ushort pixelperiod,
          double dx,
          double dy);

        public delegate void n_set_pixelDelegate(
          ushort n,
          ushort pulswidth,
          ushort davalue,
          ushort adchannel);

        public delegate void set_pixelDelegate(ushort pulswidth, ushort davalue, ushort adchannel);

        public delegate void n_set_extstartpos_listDelegate(ushort n, ushort position);

        public delegate void set_extstartpos_listDelegate(ushort position);

        public delegate void n_laser_signal_on_listDelegate(ushort n);

        public delegate void laser_signal_on_listDelegate();

        public delegate void n_laser_signal_off_listDelegate(ushort n);

        public delegate void laser_signal_off_listDelegate();

        public delegate void n_set_firstpulse_killer_listDelegate(ushort n, ushort fpk);

        public delegate void set_firstpulse_killer_listDelegate(ushort fpk);

        public delegate void n_set_io_cond_listDelegate(
          ushort n,
          ushort mask_1,
          ushort mask_0,
          ushort mask_set);

        public delegate void set_io_cond_listDelegate(ushort mask_1, ushort mask_0, ushort mask_set);

        public delegate void n_clear_io_cond_listDelegate(
          ushort n,
          ushort mask_1,
          ushort mask_0,
          ushort mask_clear);

        public delegate void clear_io_cond_listDelegate(
          ushort mask_1,
          ushort mask_0,
          ushort mask_clear);

        public delegate void n_list_jump_condDelegate(
          ushort n,
          ushort mask_1,
          ushort mask_0,
          ushort position);

        public delegate void list_jump_condDelegate(ushort mask_1, ushort mask_0, ushort position);

        public delegate void n_list_call_condDelegate(
          ushort n,
          ushort mask_1,
          ushort mask_0,
          ushort position);

        public delegate void list_call_condDelegate(ushort mask_1, ushort mask_0, ushort position);

        public delegate void n_save_and_restart_timerDelegate(ushort n);

        public delegate void save_and_restart_timerDelegate();

        public delegate void n_set_ext_start_delay_listDelegate(ushort n, short delay, short encoder);

        public delegate void set_ext_start_delay_listDelegate(short delay, short encoder);

        public delegate void n_set_triggerDelegate(
          ushort n,
          ushort sampleperiod,
          ushort channel1,
          ushort channel2);

        public delegate void set_triggerDelegate(ushort sampleperiod, ushort signal1, ushort signal2);

        public delegate void n_arc_relDelegate(ushort n, short dx, short dy, double angle);

        public delegate void arc_relDelegate(short dx, short dy, double angle);

        public delegate void n_arc_absDelegate(ushort n, short x, short y, double angle);

        public delegate void arc_absDelegate(short x, short y, double angle);

        public delegate void drillingDelegate(short pulsewidth, short relencoderdelay);

        public delegate void regulationDelegate();

        public delegate void flylineDelegate(short encoderdelay);

        public delegate ushort n_get_input_pointerDelegate(ushort n);

        public delegate ushort get_input_pointerDelegate();

        public delegate void select_rtcDelegate(ushort cardno);

        public delegate ushort rtc4_count_cardsDelegate();

        public delegate void n_get_statusDelegate(ushort n, out ushort busy, out ushort position);

        public delegate void get_statusDelegate(out ushort busy, out ushort position);

        public delegate ushort n_read_statusDelegate(ushort n);

        public delegate ushort read_statusDelegate();

        public delegate ushort n_get_startstop_infoDelegate(ushort n);

        public delegate ushort get_startstop_infoDelegate();

        public delegate ushort n_get_marking_infoDelegate(ushort n);

        public delegate ushort get_marking_infoDelegate();

        public delegate ushort get_dll_versionDelegate();

        public delegate void n_set_start_list_1Delegate(ushort n);

        public delegate void set_start_list_1Delegate();

        public delegate void n_set_start_list_2Delegate(ushort n);

        public delegate void set_start_list_2Delegate();

        public delegate void n_set_start_listDelegate(ushort n, ushort listno);

        public delegate void set_start_listDelegate(ushort listno);

        public delegate void n_execute_list_1Delegate(ushort n);

        public delegate void execute_list_1Delegate();

        public delegate void n_execute_list_2Delegate(ushort n);

        public delegate void execute_list_2Delegate();

        public delegate void n_execute_listDelegate(ushort n, ushort listno);

        public delegate void execute_listDelegate(ushort listno);

        public delegate void n_write_8bit_portDelegate(ushort n, ushort value);

        public delegate void write_8bit_portDelegate(ushort value);

        public delegate void n_write_io_portDelegate(ushort n, ushort value);

        public delegate void write_io_portDelegate(ushort value);

        public delegate void n_auto_changeDelegate(ushort n);

        public delegate void auto_changeDelegate();

        public delegate void n_auto_change_posDelegate(ushort n, ushort start);

        public delegate void auto_change_posDelegate(ushort start);

        public delegate void aut_changeDelegate();

        public delegate void n_start_loopDelegate(ushort n);

        public delegate void start_loopDelegate();

        public delegate void n_quit_loopDelegate(ushort n);

        public delegate void quit_loopDelegate();

        public delegate void n_set_list_modeDelegate(ushort n, ushort mode);

        public delegate void set_list_modeDelegate(ushort mode);

        public delegate void n_stop_executionDelegate(ushort n);

        public delegate void stop_executionDelegate();

        public delegate ushort n_read_io_portDelegate(ushort n);

        public delegate ushort read_io_portDelegate();

        public delegate void n_write_da_1Delegate(ushort n, ushort value);

        public delegate void write_da_1Delegate(ushort value);

        public delegate void n_write_da_2Delegate(ushort n, ushort value);

        public delegate void write_da_2Delegate(ushort value);

        public delegate void n_set_max_countsDelegate(ushort n, int counts);

        public delegate void set_max_countsDelegate(int counts);

        public delegate int n_get_countsDelegate(ushort n);

        public delegate int get_countsDelegate();

        public delegate void n_set_matrixDelegate(
          ushort n,
          double m11,
          double m12,
          double m21,
          double m22);

        public delegate void set_matrixDelegate(double m11, double m12, double m21, double m22);

        public delegate void n_set_offsetDelegate(ushort n, short xoffset, short yoffset);

        public delegate void set_offsetDelegate(short xoffset, short yoffset);

        public delegate void n_goto_xyzDelegate(ushort n, short x, short y, short z);

        public delegate void goto_xyzDelegate(short x, short y, short z);

        public delegate void n_goto_xyDelegate(ushort n, short x, short y);

        public delegate void goto_xyDelegate(short x, short y);

        public delegate ushort n_get_hex_versionDelegate(ushort n);

        public delegate ushort get_hex_versionDelegate();

        public delegate void n_disable_laserDelegate(ushort n);

        public delegate void disable_laserDelegate();

        public delegate void n_enable_laserDelegate(ushort n);

        public delegate void enable_laserDelegate();

        public delegate void n_stop_listDelegate(ushort n);

        public delegate void stop_listDelegate();

        public delegate void n_restart_listDelegate(ushort n);

        public delegate void restart_listDelegate();

        public delegate void n_get_xyz_posDelegate(ushort n, out short x, out short y, out short z);

        public delegate void get_xyz_posDelegate(out short x, out short y, out short z);

        public delegate void n_get_xy_posDelegate(ushort n, out short x, out short y);

        public delegate void get_xy_posDelegate(out short x, out short y);

        public delegate void n_select_listDelegate(ushort n, ushort list_2);

        public delegate void select_listDelegate(ushort list_2);

        public delegate void n_z_outDelegate(ushort n, short z);

        public delegate void z_outDelegate(short z);

        public delegate void n_set_firstpulse_killerDelegate(ushort n, ushort fpk);

        public delegate void set_firstpulse_killerDelegate(ushort fpk);

        public delegate void n_set_standbyDelegate(ushort n, ushort half_period, ushort pulse);

        public delegate void set_standbyDelegate(ushort half_period, ushort pulse);

        public delegate void n_laser_signal_onDelegate(ushort n);

        public delegate void laser_signal_onDelegate();

        public delegate void n_laser_signal_offDelegate(ushort n);

        public delegate void laser_signal_offDelegate();

        public delegate void n_set_delay_modeDelegate(
          ushort n,
          ushort varpoly,
          ushort directmove3d,
          ushort edgelevel,
          ushort minjumpdelay,
          ushort jumplengthlimit);

        public delegate void set_delay_modeDelegate(
          ushort varpoly,
          ushort directmove3d,
          ushort edgelevel,
          ushort minjumpdelay,
          ushort jumplengthlimit);

        public delegate void n_set_piso_controlDelegate(ushort n, ushort l1, ushort l2);

        public delegate void set_piso_controlDelegate(ushort l1, ushort l2);

        public delegate void n_select_statusDelegate(ushort n, ushort mode);

        public delegate void select_statusDelegate(ushort mode);

        public delegate void n_get_encoderDelegate(ushort n, out short zx, out short zy);

        public delegate void get_encoderDelegate(out short zx, out short zy);

        public delegate void n_select_cor_tableDelegate(ushort n, ushort heada, ushort headb);

        public delegate void select_cor_tableDelegate(ushort heada, ushort headb);

        public delegate void n_execute_at_pointerDelegate(ushort n, ushort position);

        public delegate void execute_at_pointerDelegate(ushort position);

        public delegate ushort n_get_head_statusDelegate(ushort n, ushort head);

        public delegate ushort get_head_statusDelegate(ushort head);

        public delegate void n_simulate_encoderDelegate(ushort n, ushort channel);

        public delegate void simulate_encoderDelegate(ushort channel);

        public delegate void n_set_hiDelegate(
          ushort n,
          double galvogainx,
          double galvogainy,
          short galvooffsetx,
          short galvooffsety,
          short head);

        public delegate void set_hiDelegate(
          double galvogainx,
          double galvogainy,
          short galvooffsetx,
          short galvooffsety,
          short head);

        public delegate void n_release_waitDelegate(ushort n);

        public delegate void release_waitDelegate();

        public delegate ushort n_get_wait_statusDelegate(ushort n);

        public delegate ushort get_wait_statusDelegate();

        public delegate void n_set_control_modeDelegate(ushort n, ushort mode);

        public delegate void set_control_modeDelegate(ushort mode);

        public delegate void n_set_laser_modeDelegate(ushort n, ushort mode);

        public delegate void set_laser_modeDelegate(ushort mode);

        public delegate void n_set_ext_start_delayDelegate(ushort n, short delay, short encoder);

        public delegate void set_ext_start_delayDelegate(short delay, short encoder);

        public delegate void n_home_positionDelegate(ushort n, short xhome, short yhome);

        public delegate void home_positionDelegate(short xhome, short yhome);

        public delegate void n_set_rot_centerDelegate(ushort n, int center_x, int center_y);

        public delegate void set_rot_centerDelegate(int center_x, int center_y);

        public delegate void n_dsp_startDelegate(ushort n);

        public delegate void dsp_startDelegate();

        public delegate void n_write_da_xDelegate(ushort n, ushort x, ushort value);

        public delegate void write_da_xDelegate(ushort x, ushort value);

        public delegate ushort n_read_ad_xDelegate(ushort n, ushort x);

        public delegate ushort read_ad_xDelegate(ushort x);

        public delegate ushort n_read_pixel_adDelegate(ushort n, ushort pos);

        public delegate ushort read_pixel_adDelegate(ushort pos);

        public delegate short n_get_z_distanceDelegate(ushort n, short x, short y, short z);

        public delegate short get_z_distanceDelegate(short x, short y, short z);

        public delegate ushort n_get_io_statusDelegate(ushort n);

        public delegate ushort get_io_statusDelegate();

        public delegate double n_get_timeDelegate(ushort n);

        public delegate double get_timeDelegate();

        public delegate void n_set_defocusDelegate(ushort n, short value);

        public delegate void set_defocusDelegate(short value);

        public delegate void n_set_softstart_modeDelegate(
          ushort n,
          ushort mode,
          ushort number,
          ushort restartdelay);

        public delegate void set_softstart_modeDelegate(ushort mode, ushort number, ushort resetdelay);

        public delegate void n_set_softstart_levelDelegate(ushort n, ushort index, ushort level);

        public delegate void set_softstart_levelDelegate(ushort index, ushort level);

        public delegate void n_control_commandDelegate(
          ushort n,
          ushort head,
          ushort axis,
          ushort data);

        public delegate void control_commandDelegate(ushort head, ushort axis, ushort data);

        public delegate short load_corDelegate(string filename);

        public delegate short load_proDelegate(string filename);

        public delegate ushort n_get_serial_numberDelegate(ushort n);

        public delegate ushort get_serial_numberDelegate();

        public delegate int n_get_serial_number_32Delegate(ushort n);

        public delegate int get_serial_number_32Delegate();

        public delegate ushort n_get_rtc_versionDelegate(ushort n);

        public delegate ushort get_rtc_versionDelegate();

        public delegate void get_hi_dataDelegate(
          out ushort x1,
          out ushort x2,
          out ushort y1,
          out ushort y2);

        public delegate short n_auto_calDelegate(ushort n, ushort head, ushort command);

        public delegate short auto_calDelegate(ushort head, ushort command);

        public delegate ushort n_get_list_spaceDelegate(ushort n);

        public delegate ushort get_list_spaceDelegate();

        public delegate short teachinDelegate(
          string filename,
          short xin,
          short yin,
          short zin,
          double ll0,
          out short xout,
          out short yout,
          out short zout);

        public delegate short n_get_valueDelegate(ushort n, ushort signal);

        public delegate short get_valueDelegate(ushort signal);

        public delegate void n_set_io_bitDelegate(ushort n, ushort mask1);

        public delegate void set_io_bitDelegate(ushort mask1);

        public delegate void n_clear_io_bitDelegate(ushort n, ushort mask0);

        public delegate void clear_io_bitDelegate(ushort mask0);

        public delegate void set_duty_cycle_tableDelegate(ushort index, ushort dutycycle);

        public delegate void n_move_toDelegate(ushort n, ushort position);

        public delegate void move_toDelegate(ushort position);
    }
}
