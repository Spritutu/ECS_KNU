
namespace Scanlab.Sirius
{
    internal enum HpglErrorType
    {
        Info,
        Warning,
        Error,
    }

    internal class HpglError
    {
        private HpglErrorType m_type;
        private int m_line;
        private string m_message;

        public HpglError(HpglErrorType type, int line, string message)
        {
            this.m_type = type;
            this.m_line = line;
            this.m_message = message;
        }

        public HpglErrorType Type => this.m_type;

        public int Line => this.m_line;

        public string Message => this.m_message;
    }
}
