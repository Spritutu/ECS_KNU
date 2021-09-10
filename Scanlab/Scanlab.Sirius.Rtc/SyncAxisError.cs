
using System;

namespace Scanlab.Sirius
{
    internal sealed class SyncAxisError
    {
        private SyncAxisError.Code code;

        internal SyncAxisError(int value) => this.code = (SyncAxisError.Code)value;

        public void Add(SyncAxisError.Code flag) => this.code |= flag;

        public void Remove(SyncAxisError.Code flag) => this.code &= ~flag;

        public bool Contains(SyncAxisError.Code flag) => Convert.ToBoolean((object)(this.code & flag));

        public override string ToString() => this.code.ToString();

        [Flags]
        public enum Code
        {
            Ok = 0,
            InErrorState = 1,
            ErrorOccured = 2,
            NotAllowedWithoutInitialization = 4,
            NotAllowedInExecuting = 8,
            BufferFull = 16, // 0x00000010
            NotReadyForExecution = 32, // 0x00000020
            UnplausibleOrUnknownParameter = 64, // 0x00000040
            JobStructureNotValid = 128, // 0x00000080
            NotAllowedInCurrentConfiguration = 512, // 0x00000200
            NotReady = 1024, // 0x00000400
            NotAllowedInCurrentMode = 2048, // 0x00000800
            InvalidPosition = 4096, // 0x00001000
            Timeout = 8192, // 0x00002000
            XmlLoadError = 16384, // 0x00004000
            InitializationFailed = 32768, // 0x00008000
            HandshakeFailed = 262144, // 0x00040000
            UnknownDevice = 268435456, // 0x10000000
            MaxInstancesReached = 1073741824, // 0x40000000
            InvalidOrMissingDongle = -2147483648, // 0x80000000
        }
    }
}
