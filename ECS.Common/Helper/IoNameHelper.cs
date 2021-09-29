using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Common.Helper
{
    public class IoNameHelper
    {    
        public const string V_STR_SYS_OPERATION_MODE = "vSys.sEqp.OperationMode";
        public const string V_STR_SYS_SIMULATION_MODE = "vSys.sEqp.SimulationMode";

        public const string V_INT_SYS_SIGNAL_TOWER_YELLOW = "vSys.iSignalTower.Yellow";
        public const string V_INT_SYS_SIGNAL_TOWER_WHITE = "vSys.iSignalTower.White";
        public const string V_INT_SYS_SIGNAL_TOWER_RED = "vSys.iSignalTower.Red";
        public const string V_INT_SYS_SIGNAL_TOWER_GREEN = "vSys.iSignalTower.Green";
        public const string V_STR_SYS_RECIPE_FILE_FOLDER = "vSys.sEqp.RecipeFileFolder";
        public const string V_STR_SYS_CURRENT_RECIPE = "vSys.sEqp.CurrentRecipe";

        public const string V_INT_SYS_EQP_RUN = "vSys.iEqp.Run";
        public const string V_INT_SYS_EQP_MOVE = "vSys.iEqp.Move";
        public const string V_INT_SYS_EQP_INTERLOCK = "vSys.iEqp.Interlock";
        public const string V_INT_SYS_EQP_AVAILABILITY = "vSys.iEqp.Availability";
        public const string V_INT_SYS_EQP_ALARM = "vSys.iEqp.Alarm";
        public const string V_INT_SYS_EQP_EMOSTOP = "vSys.iEqp.EmoStop";
        public const string V_INT_SYS_EQP_LASER_WARNING_ONOFF = "vSys.iEqp.LaserWarningOnOff";

        public const string V_INT_GUI_CURRENT_STEPID = "vGUI.iRcp.CurrentStepId";

        public const string IN_INT_LED_DATA_CH1 = "iLed.iData.Ch1";
        public const string IN_INT_LED_DATA_CH2 = "iLed.iData.Ch2";
        public const string IN_INT_LED_DATA_CH3 = "iLed.iData.Ch3";
        public const string IN_INT_LED_DATA_CH4 = "iLed.iData.Ch4";

        public const string IN_INT_LED_ONOFF_CH1 = "iLed.iOnOff.Ch1";
        public const string IN_INT_LED_ONOFF_CH2 = "iLed.iOnOff.Ch2";
        public const string IN_INT_LED_ONOFF_CH3 = "iLed.iOnOff.Ch3";
        public const string IN_INT_LED_ONOFF_CH4 = "iLed.iOnOff.Ch4";

        public const string IN_DBL_PMAC_R_JOGVEL = "iPMAC.dAxisR.JogVel";
        public const string IN_DBL_PMAC_R_POSITION = "iPMAC.dAxisR.Position";
        public const string IN_DBL_PMAC_R_VELOCITY = "iPMAC.dAxisR.Velocity";

        public const string IN_DBL_PMAC_T_JOGVEL = "iPMAC.dAxisT.JogVel";
        public const string IN_DBL_PMAC_T_POSITION = "iPMAC.dAxisT.Position";
        public const string IN_DBL_PMAC_T_VELOCITY = "iPMAC.dAxisT.Velocity";


        public const string IN_DBL_PMAC_X_JOGVEL = "iPMAC.dAxisX.JogVel";
        public const string IN_DBL_PMAC_X_POSITION = "iPMAC.dAxisX.Position";
        public const string IN_DBL_PMAC_X_VELOCITY = "iPMAC.dAxisX.Velocity";

        public const string IN_DBL_PMAC_Y_JOGVEL = "iPMAC.dAxisY.JogVel";
        public const string IN_DBL_PMAC_Y_POSITION = "iPMAC.dAxisY.Position";
        public const string IN_DBL_PMAC_Y_VELOCITY = "iPMAC.dAxisY.Velocity";

        public const string IN_DBL_PMAC_Z_JOGVEL = "iPMAC.dAxisZ.JogVel";
        public const string IN_DBL_PMAC_Z_POSITION = "iPMAC.dAxisZ.Position";
        public const string IN_DBL_PMAC_Z_VELOCITY = "iPMAC.dAxisZ.Velocity";


        public const string IN_INT_PMAC_R_ISHOME = "iPMAC.iAxisR.IsHome";
        public const string IN_INT_PMAC_R_ISHOMMING = "iPMAC.iAxisR.IsHomming";
        public const string IN_INT_PMAC_R_ISMOVING = "iPMAC.iAxisR.IsMoving";

        public const string IN_INT_PMAC_T_ISHOME = "iPMAC.iAxisT.IsHome";
        public const string IN_INT_PMAC_T_ISHOMMING = "iPMAC.iAxisT.IsHomming";
        public const string IN_INT_PMAC_T_ISMOVING = "iPMAC.iAxisT.IsMoving";

        public const string IN_INT_PMAC_X_ISHOME = "iPMAC.iAxisX.IsHome";
        public const string IN_INT_PMAC_X_ISHOMMING = "iPMAC.iAxisX.IsHomming";
        public const string IN_INT_PMAC_X_ISMOVING = "iPMAC.iAxisX.IsMoving";

        public const string IN_INT_PMAC_Y_ISHOME = "iPMAC.iAxisY.IsHome";
        public const string IN_INT_PMAC_Y_ISHOMMING = "iPMAC.iAxisY.IsHomming";
        public const string IN_INT_PMAC_Y_ISMOVING = "iPMAC.iAxisY.IsMoving";

        public const string IN_INT_PMAC_Z_ISHOME = "iPMAC.iAxisZ.IsHome";
        public const string IN_INT_PMAC_Z_ISHOMMING = "iPMAC.iAxisZ.IsHomming";
        public const string IN_INT_PMAC_Z_ISMOVING = "iPMAC.iAxisZ.IsMoving";

        public const string IN_INT_PMAC_COVER_OPENCLOSE = "iPMAC.iCover.OpenClose";
        public const string IN_INT_PMAC_CPBOX_OPENCLOSE = "iPMAC.iCpBox.OpenClose";

        public const string IN_INT_PMAC_DOOR1_FRONT = "iPMAC.iDoor1.Front";
        public const string IN_INT_PMAC_DOOR2_LEFT = "iPMAC.iDoor2.Left";
        public const string IN_INT_PMAC_DOOR3_RIGHT = "iPMAC.iDoor3.Right";

        public const string IN_INT_LASER_ASSIGNED = "iPMAC.iLaser.Assigned";
        public const string IN_INT_LASER_FAULT = "iPMAC.iLaser.Fault";
        public const string IN_INT_LASER_ISON = "iPMAC.iLaser.IsOn";
        public const string IN_INT_LASER_LIGHT_PATHNO = "iPMAC.iLaser.LightPathNo";
        public const string IN_INT_LASER_PILOT_ISON = "iPMAC.iLaser.PilotLaserIsOn";
        public const string IN_INT_LASER_PROGRAM_ACTIVE = "iPMAC.iLaser.ProgActive";
        public const string IN_INT_LASER_PROGRAM_COMPLETED = "iPMAC.iLaser.ProgCompleted";
        public const string IN_INT_LASER_READY = "iPMAC.iLaser.Ready";
        public const string IN_INT_SWITCH_RESET = "iPMAC.iSwitch.Reset";
        public const string IN_INT_SWITCH_START = "iPMAC.iSwitch.Start";
        public const string IN_INT_SWITCH_STOP = "iPMAC.iSwitch.Stop";

        public const string IN_INT_SCAN_BUSY_STATUS = "iRTC.iScan.BusyStatus";
        public const string IN_INT_SCAN_ERROR_STATUS = "iRTC.iScan.ErrorStatus";
        public const string IN_INT_SCAN_POSITION_ACK_STATUS = "iRTC.iScan.PosAckStatus";
        public const string IN_INT_SCAN_POWER_STATUS = "iRTC.iScan.PowerStatus";
        public const string IN_INT_SCAN_TEMP_STATUS = "iRTC.iScan.TempStatus";
        public const string IN_INT_RTC_SCAN_PROCESS_REPEAT = "iRTC.iScan.ProcessRepeat";
        public const string IN_DBL_RTC_LASER_POWER_PERCENT = "iRTC.dLaser.ProcessPowerPercent";

        public const string OUT_INT_LED_DATA_ALL = "oLed.iDataSet.ChAll";
        public const string OUT_INT_LED_DATA_CH1 = "oLed.iDataSet.Ch1";
        public const string OUT_INT_LED_DATA_CH2 = "oLed.iDataSet.Ch2";
        public const string OUT_INT_LED_DATA_CH3 = "oLed.iDataSet.Ch3";
        public const string OUT_INT_LED_DATA_CH4 = "oLed.iDataSet.Ch4";

        public const string OUT_INT_LED_ONOFF_CH1 = "oLed.iOnOff.Ch1";
        public const string OUT_INT_LED_ONOFF_CH2 = "oLed.iOnOff.Ch2";
        public const string OUT_INT_LED_ONOFF_CH3 = "oLed.iOnOff.Ch3";
        public const string OUT_INT_LED_ONOFF_CH4 = "oLed.iOnOff.Ch4";

        public const string OUT_DBL_PMAC_R_SETVELOCITY = "oPMAC.dAxisR.SetVelocity";
        public const string OUT_DBL_PMAC_R_SETPOSITION = "oPMAC.dAxisR.SetPosition";
        public const string OUT_DBL_PMAC_R_SETDISTANCE = "oPMAC.dAxisR.SetDistance";
        public const string OUT_DBL_PMAC_R_JOGVELOCITY = "oPMAC.dAxisR.JogVel";

        public const string OUT_DBL_PMAC_T_SETVELOCITY = "oPMAC.dAxisT.SetVelocity";
        public const string OUT_DBL_PMAC_T_SETPOSITION = "oPMAC.dAxisT.SetPosition";
        public const string OUT_DBL_PMAC_T_SETDISTANCE = "oPMAC.dAxisT.SetDistance";
        public const string OUT_DBL_PMAC_T_JOGVELOCITY = "oPMAC.dAxisT.JogVel";

        public const string OUT_DBL_PMAC_X_SETVELOCITY = "oPMAC.dAxisX.SetVelocity";
        public const string OUT_DBL_PMAC_X_SETPOSITION = "oPMAC.dAxisX.SetPosition";
        public const string OUT_DBL_PMAC_X_SETDISTANCE = "oPMAC.dAxisX.SetDistance";
        public const string OUT_DBL_PMAC_X_JOGVELOCITY = "oPMAC.dAxisX.JogVel";

        public const string OUT_DBL_PMAC_Y_SETVELOCITY = "oPMAC.dAxisY.SetVelocity";
        public const string OUT_DBL_PMAC_Y_SETPOSITION = "oPMAC.dAxisY.SetPosition";
        public const string OUT_DBL_PMAC_Y_SETDISTANCE = "oPMAC.dAxisY.SetDistance";
        public const string OUT_DBL_PMAC_Y_JOGVELOCITY = "oPMAC.dAxisY.JogVel";

        public const string OUT_DBL_PMAC_Z_SETVELOCITY = "oPMAC.dAxisZ.SetVelocity";
        public const string OUT_DBL_PMAC_Z_SETPOSITION = "oPMAC.dAxisZ.SetPosition";
        public const string OUT_DBL_PMAC_Z_SETDISTANCE = "oPMAC.dAxisZ.SetDistance";
        public const string OUT_DBL_PMAC_Z_JOGVELOCITY = "oPMAC.dAxisZ.JogVel";

        public const string OUT_INT_PMAC_AIR_CLEANING = "oPMAC.iAir.Cleaning";

        public const string OUT_INT_PMAC_R_MOVETOSETPOS = "oPMAC.iAxisR.MoveToSetPos";
        public const string OUT_INT_PMAC_R_MOVETOSETDIS = "oPMAC.iAxisR.MoveToSetDis";
        public const string OUT_INT_PMAC_R_MOVESTOP = "oPMAC.iAxisR.MoveStop";
        public const string OUT_INT_PMAC_R_JOGSTOP = "oPMAC.iAxisR.JogStop";
        public const string OUT_INT_PMAC_R_JOGFWD = "oPMAC.iAxisR.JogFwd";
        public const string OUT_INT_PMAC_R_JOGBWD = "oPMAC.iAxisR.JogBwd";
        public const string OUT_INT_PMAC_R_HOMMING = "oPMAC.iAxisR.Homming";
        public const string OUT_INT_PMAC_R_HOMESTOP = "oPMAC.iAxisR.HomeStop";


        public const string OUT_INT_PMAC_T_MOVETOSETPOS = "oPMAC.iAxisT.MoveToSetPos";
        public const string OUT_INT_PMAC_T_MOVETOSETDIS = "oPMAC.iAxisT.MoveToSetDis";
        public const string OUT_INT_PMAC_T_MOVESTOP = "oPMAC.iAxisT.MoveStop";
        public const string OUT_INT_PMAC_T_JOGSTOP = "oPMAC.iAxisT.JogStop";
        public const string OUT_INT_PMAC_T_JOGFWD = "oPMAC.iAxisT.JogFwd";
        public const string OUT_INT_PMAC_T_JOGBWD = "oPMAC.iAxisT.JogBwd";
        public const string OUT_INT_PMAC_T_HOMMING = "oPMAC.iAxisT.Homming";
        public const string OUT_INT_PMAC_T_HOMESTOP = "oPMAC.iAxisT.HomeStop";

        public const string OUT_INT_PMAC_X_MOVETOSETPOS = "oPMAC.iAxisX.MoveToSetPos";
        public const string OUT_INT_PMAC_X_MOVETOSETDIS = "oPMAC.iAxisX.MoveToSetDis";
        public const string OUT_INT_PMAC_X_MOVESTOP = "oPMAC.iAxisX.MoveStop";
        public const string OUT_INT_PMAC_X_JOGSTOP = "oPMAC.iAxisX.JogStop";
        public const string OUT_INT_PMAC_X_JOGFWD = "oPMAC.iAxisX.JogFwd";
        public const string OUT_INT_PMAC_X_JOGBWD = "oPMAC.iAxisX.JogBwd";
        public const string OUT_INT_PMAC_X_HOMMING = "oPMAC.iAxisX.Homming";
        public const string OUT_INT_PMAC_X_HOMESTOP = "oPMAC.iAxisX.HomeStop";

        public const string OUT_INT_PMAC_Y_MOVETOSETPOS = "oPMAC.iAxisY.MoveToSetPos";
        public const string OUT_INT_PMAC_Y_MOVETOSETDIS = "oPMAC.iAxisY.MoveToSetDis";
        public const string OUT_INT_PMAC_Y_MOVESTOP = "oPMAC.iAxisY.MoveStop";
        public const string OUT_INT_PMAC_Y_JOGSTOP = "oPMAC.iAxisY.JogStop";
        public const string OUT_INT_PMAC_Y_JOGFWD = "oPMAC.iAxisY.JogFwd";
        public const string OUT_INT_PMAC_Y_JOGBWD = "oPMAC.iAxisY.JogBwd";
        public const string OUT_INT_PMAC_Y_HOMMING = "oPMAC.iAxisY.Homming";
        public const string OUT_INT_PMAC_Y_HOMESTOP = "oPMAC.iAxisY.HomeStop";

        public const string OUT_INT_PMAC_Z_MOVETOSETPOS = "oPMAC.iAxisZ.MoveToSetPos";
        public const string OUT_INT_PMAC_Z_MOVETOSETDIS = "oPMAC.iAxisZ.MoveToSetDis";
        public const string OUT_INT_PMAC_Z_MOVESTOP = "oPMAC.iAxisZ.MoveStop";
        public const string OUT_INT_PMAC_Z_JOGSTOP = "oPMAC.iAxisZ.JogStop";
        public const string OUT_INT_PMAC_Z_JOGFWD = "oPMAC.iAxisZ.JogFwd";
        public const string OUT_INT_PMAC_Z_JOGBWD = "oPMAC.iAxisZ.JogBwd";
        public const string OUT_INT_PMAC_Z_HOMMING = "oPMAC.iAxisZ.Homming";
        public const string OUT_INT_PMAC_Z_HOMESTOP = "oPMAC.iAxisZ.HomeStop";

        public const string OUT_INT_LAMP_RESET = "oPMAC.iLamp.Reset";
        public const string OUT_INT_LAMP_START = "oPMAC.iLamp.Start";
        public const string OUT_INT_LAMP_STOP = "oPMAC.iLamp.Stop";

        public const string OUT_INT_LASER_EXT_ACTIVATION = "oPMAC.iLaser.ExtActivation";
        public const string OUT_INT_LASER_ON = "oPMAC.iLaser.On";
        public const string OUT_INT_LASER_PILOTLASER_ON = "oPMAC.iLaser.PilotLaserOn";
        public const string OUT_INT_LASER_PROGRAM_NO = "oPMAC.iLaser.ProgramNo";
        public const string OUT_INT_LASER_PROGRAM_START_DYNAMIC = "oPMAC.iLaser.PStartDyn";
        public const string OUT_INT_LASER_PROGRAM_START_STATICAL = "oPMAC.iLaser.PStartStatical";
        public const string OUT_INT_LASER_REQUSET = "oPMAC.iLaser.RequestLaser";
        public const string OUT_INT_LASER_RESET = "oPMAC.iLaser.Reset";
        public const string OUT_INT_LASER_STANDBY = "oPMAC.iLaser.Standby";
        public const string OUT_INT_LEDLIGHT_ONOFF = "oPMAC.iLedLight.OnOff";
        public const string OUT_INT_SCAN_AIR_BLOW = "oPMAC.iScan.Blow";

        public const string OUT_INT_PMAC_TOWERLAMP_YELLOW = "oPMAC.iTowerLamp.Yellow";
        public const string OUT_INT_PMAC_TOWERLAMP_RED = "oPMAC.iTowerLamp.Red";
        public const string OUT_INT_PMAC_TOWERLAMP_GREEN = "oPMAC.iTowerLamp.Green";
        public const string OUT_INT_PMAC_BUZZER_ONOFF = "oPMAC.iBuzzer.OnOff";

        public const string OUT_DBL_RTC_LASER_FREQUENCY = "oRTC.dLaser.Frequency";
        public const string OUT_DBL_RTC_LASER_MAXPOWER = "oRTC.dLaser.MaxPower";
        public const string OUT_DBL_RTC_LASER_OFF_DELAY = "oRTC.dLaser.OffDelay";
        public const string OUT_DBL_RTC_LASER_ON_DELAY = "oRTC.dLaser.OnDelay";

        public const string OUT_DBL_RTC_LASER_PROCESS_POWER = "oRTC.dLaser.ProcessPower";
        public const string OUT_DBL_RTC_LASER_PROCESS_POWER_PERCENT = "oRTC.dLaser.ProcessPowerPercent";
        public const string OUT_DBL_RTC_LASER_PULSE_WIDTH = "oRTC.dLaser.PulseWidth";
        public const string OUT_DBL_RTC_SCAN_JUMP_DELAY = "oRTC.dScan.JumpDelay";
        public const string OUT_DBL_RTC_SCAN_JUMP_SPEED = "oRTC.dScan.JumpSpeed";

        public const string OUT_DBL_RTC_SCAN_MARK_DELAY = "oRTC.dScan.MarkDelay";
        public const string OUT_DBL_RTC_SCAN_MARK_SPEED = "oRTC.dScan.MarkSpeed";

        public const string OUT_DBL_RTC_SCAN_POLYGON_DELAY = "oRTC.dScan.PolygonDelay";
        public const string OUT_INT_RTC_LASER_SHUTTER_OPENCLOSE = "oRTC.iLaser.ShutterOpnCls";
        public const string OUT_INT_RTC_CORRECTION_FILE_RELOAD = "oRTC.iScan.CorrFileReload";

        public const string OUT_INT_RTC_SCAN_LASER_ON = "oRTC.iScan.LaserOn";
        public const string OUT_INT_RTC_PARAMETER_RELOAD = "oRTC.iScan.ParamReload";

        public const string OUT_INT_RTC_SCAN_PROCESS_ABORT = "oRTC.iScan.ProcessAbort";
        public const string OUT_INT_RTC_SCAN_PROCESS_REPEAT = "oRTC.iScan.ProcessRepeat";
        public const string OUT_INT_RTC_SCAN_PROCESS_START = "oRTC.iScan.ProcessStart";

        public const string OUT_INT_RTC_SCAN_RECIPE_NO = "oRTC.iScan.RecipeNo";

        public const string OUT_OBJ_RTC_SCAN_DOCUMENT = "oRTC.oScan.Document";

        public const string OUT_OBJ_RTC_SCAN_LAYEROBJECT = "oRTC.oScan.LayerObject";

        public const string V_DBL_SET_R_VISION_POSITION = "vSet.dAxisR.VisionPosition";
        public const string V_DBL_SET_R_REL_VELOCITY = "vSet.dAxisR.RelVelocity";
        public const string V_DBL_SET_R_REL_DISTANCE = "vSet.dAxisR.RelDistance";
        public const string V_DBL_SET_R_PROCESS_POSITION = "vSet.dAxisR.ProcessPosition";
        public const string V_DBL_SET_R_MIN_VELOCITY = "vSet.dAxisR.MinVelocity";
        public const string V_DBL_SET_R_MAX_VELOCITY = "vSet.dAxisR.MaxVelocity";
        public const string V_DBL_SET_R_MIN_POSITION = "vSet.dAxisR.MinPosition";
        public const string V_DBL_SET_R_MAX_POSITION = "vSet.dAxisR.MaxPosition";
        public const string V_DBL_SET_R_JOGVEL_LOW = "vSet.dAxisR.JogVelLow";
        public const string V_DBL_SET_R_JOGVEL_HIGH = "vSet.dAxisR.JogVelHigh";
        public const string V_DBL_SET_R_INPOS_RANGE = "vSet.dAxisR.InPosRange";
        public const string V_DBL_SET_R_ABS_VELOCITY = "vSet.dAxisR.AbsVelocity";
        public const string V_DBL_SET_R_ABS_POSITION = "vSet.dAxisR.AbsPosition";


        public const string V_DBL_SET_T_VISION_POSITION = "vSet.dAxisT.VisionPosition";
        public const string V_DBL_SET_T_REL_VELOCITY = "vSet.dAxisT.RelVelocity";
        public const string V_DBL_SET_T_REL_DISTANCE = "vSet.dAxisT.RelDistance";
        public const string V_DBL_SET_T_PROCESS_POSITION = "vSet.dAxisT.ProcessPosition";
        public const string V_DBL_SET_T_MIN_VELOCITY = "vSet.dAxisT.MinVelocity";
        public const string V_DBL_SET_T_MAX_VELOCITY = "vSet.dAxisT.MaxVelocity";
        public const string V_DBL_SET_T_MIN_POSITION = "vSet.dAxisT.MinPosition";
        public const string V_DBL_SET_T_MAX_POSITION = "vSet.dAxisT.MaxPosition";
        public const string V_DBL_SET_T_JOGVEL_LOW = "vSet.dAxisT.JogVelLow";
        public const string V_DBL_SET_T_JOGVEL_HIGH = "vSet.dAxisT.JogVelHigh";
        public const string V_DBL_SET_T_INPOS_RANGE = "vSet.dAxisT.InPosRange";
        public const string V_DBL_SET_T_ABS_VELOCITY = "vSet.dAxisT.AbsVelocity";
        public const string V_DBL_SET_T_ABS_POSITION = "vSet.dAxisT.AbsPosition";

        public const string V_DBL_SET_X_VISION_POSITION = "vSet.dAxisX.VisionPosition";
        public const string V_DBL_SET_X_REL_VELOCITY = "vSet.dAxisX.RelVelocity";
        public const string V_DBL_SET_X_REL_DISTANCE = "vSet.dAxisX.RelDistance";
        public const string V_DBL_SET_X_PROCESS_POSITION = "vSet.dAxisX.ProcessPosition";
        public const string V_DBL_SET_X_MIN_VELOCITY = "vSet.dAxisX.MinVelocity";
        public const string V_DBL_SET_X_MAX_VELOCITY = "vSet.dAxisX.MaxVelocity";
        public const string V_DBL_SET_X_MIN_POSITION = "vSet.dAxisX.MinPosition";
        public const string V_DBL_SET_X_MAX_POSITION = "vSet.dAxisX.MaxPosition";
        public const string V_DBL_SET_X_JOGVEL_LOW = "vSet.dAxisX.JogVelLow";
        public const string V_DBL_SET_X_JOGVEL_HIGH = "vSet.dAxisX.JogVelHigh";
        public const string V_DBL_SET_X_INPOS_RANGE = "vSet.dAxisX.InPosRange";
        public const string V_DBL_SET_X_ABS_VELOCITY = "vSet.dAxisX.AbsVelocity";
        public const string V_DBL_SET_X_ABS_POSITION = "vSet.dAxisX.AbsPosition";

        public const string V_DBL_SET_Y_VISION_POSITION = "vSet.dAxisY.VisionPosition";
        public const string V_DBL_SET_Y_REL_VELOCITY = "vSet.dAxisY.RelVelocity";
        public const string V_DBL_SET_Y_REL_DISTANCE = "vSet.dAxisY.RelDistance";
        public const string V_DBL_SET_Y_PROCESS_POSITION = "vSet.dAxisY.ProcessPosition";
        public const string V_DBL_SET_Y_MIN_VELOCITY = "vSet.dAxisY.MinVelocity";
        public const string V_DBL_SET_Y_MAX_VELOCITY = "vSet.dAxisY.MaxVelocity";
        public const string V_DBL_SET_Y_MIN_POSITION = "vSet.dAxisY.MinPosition";
        public const string V_DBL_SET_Y_MAX_POSITION = "vSet.dAxisY.MaxPosition";
        public const string V_DBL_SET_Y_JOGVEL_LOW = "vSet.dAxisY.JogVelLow";
        public const string V_DBL_SET_Y_JOGVEL_HIGH = "vSet.dAxisY.JogVelHigh";
        public const string V_DBL_SET_Y_INPOS_RANGE = "vSet.dAxisY.InPosRange";
        public const string V_DBL_SET_Y_ABS_VELOCITY = "vSet.dAxisY.AbsVelocity";
        public const string V_DBL_SET_Y_ABS_POSITION = "vSet.dAxisY.AbsPosition";

        public const string V_DBL_SET_Z_VISION_POSITION = "vSet.dAxisZ.VisionPosition";
        public const string V_DBL_SET_Z_REL_VELOCITY = "vSet.dAxisZ.RelVelocity";
        public const string V_DBL_SET_Z_REL_DISTANCE = "vSet.dAxisZ.RelDistance";
        public const string V_DBL_SET_Z_PROCESS_POSITION = "vSet.dAxisZ.ProcessPosition";
        public const string V_DBL_SET_Z_MIN_VELOCITY = "vSet.dAxisZ.MinVelocity";
        public const string V_DBL_SET_Z_MAX_VELOCITY = "vSet.dAxisZ.MaxVelocity";
        public const string V_DBL_SET_Z_MIN_POSITION = "vSet.dAxisZ.MinPosition";
        public const string V_DBL_SET_Z_MAX_POSITION = "vSet.dAxisZ.MaxPosition";
        public const string V_DBL_SET_Z_JOGVEL_LOW = "vSet.dAxisZ.JogVelLow";
        public const string V_DBL_SET_Z_JOGVEL_HIGH = "vSet.dAxisZ.JogVelHigh";
        public const string V_DBL_SET_Z_INPOS_RANGE = "vSet.dAxisZ.InPosRange";
        public const string V_DBL_SET_Z_ABS_VELOCITY = "vSet.dAxisZ.AbsVelocity";
        public const string V_DBL_SET_Z_ABS_POSITION = "vSet.dAxisZ.AbsPosition";

        public const string V_INT_SET_LASER_PROGRAM_NO = "vSet.iLaser.ProgramNo";
        public const string V_OBJ_SET_SCAN_DOCUMENT = "vSet.oScan.Document";
        public const string V_OBJ_SET_SCAN_TEMP_OBJECT = "vSet.oScan.TempObject";
        public const string V_STR_SET_SCAN_DOCUMENT_FILEPATH = "vSet.sScan.DocFilePath";

        public const string V_STR_SET_R_JOGVEL_MODE = "vSet.sAxisR.JogVelMode";
        public const string V_STR_SET_T_JOGVEL_MODE = "vSet.sAxisT.JogVelMode";
        public const string V_STR_SET_X_JOGVEL_MODE = "vSet.sAxisX.JogVelMode";
        public const string V_STR_SET_Y_JOGVEL_MODE = "vSet.sAxisY.JogVelMode";
        public const string V_STR_SET_Z_JOGVEL_MODE = "vSet.sAxisZ.JogVelMode";

        public const string V_DBL_SET_X_POSITION_OFFSET = "vSet.dAxisX.Offset";
        public const string V_DBL_SET_Y_POSITION_OFFSET = "vSet.dAxisY.Offset";
        public const string V_DBL_SET_Z_POSITION_OFFSET = "vSet.dAxisZ.Offset";
        public const string V_DBL_SET_R_POSITION_OFFSET = "vSet.dAxisR.Offset";
        public const string V_DBL_SET_T_POSITION_OFFSET = "vSet.dAxisT.Offset";

    }
}
