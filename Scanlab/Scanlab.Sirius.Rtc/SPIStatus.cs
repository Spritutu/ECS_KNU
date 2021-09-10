
using System;

namespace Scanlab.Sirius
{
    /// <summary>SPI 레이저 상태 정보</summary>
    public sealed class SPIStatus
    {
        public SPIStatus.Status Word { get; set; }

        public void Add(SPIStatus.Status flag) => this.Word |= flag;

        public void Remove(SPIStatus.Status flag) => this.Word &= ~flag;

        public bool Contains(SPIStatus.Status flag) => Convert.ToBoolean((object)(this.Word & flag));

        [Flags]
        public enum Status : ushort
        {
            None = 0,
            LaserReady = 1,
            TaskStart = 2,
            Reserved1 = 4,
            CWMode = 8,
            ExternalControl = 16, // 0x0010
            Reserved2 = 32, // 0x0020
            Reserved3 = 64, // 0x0040
            Reserved4 = 128, // 0x0080
            AlignmentLaserEnable = 256, // 0x0100
            HardwareInterface = 512, // 0x0200
            Reserved5 = 1024, // 0x0400
            InterlockClosed = 2048, // 0x0800
            TemperatureAlarm = 4096, // 0x1000
            BeamDeliveryAlarm = 8192, // 0x2000
            SystemAlarm = 16384, // 0x4000
            Alarm = 32768, // 0x8000
        }
    }
}
