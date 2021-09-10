using INNO6.Core.Communication.Socket;
using Mina.Core.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEV.LEDControl
{
    public class TcpClient : SocketClient
    {
        private ConcurrentQueue<byte[]> receivedQueue = new ConcurrentQueue<byte[]>();
        private static object critical_section = new object();
        private IoSession _IoSession;


        public TcpClient(string ipAddress, int portNumber)
        {
            this.IpAddress = ipAddress;
            this.Port = portNumber;
            

            this.Initialize();

            
        }

        public override void MessageReceived(IoSession session, object message)
        {
            _IoSession = session;

            if (IsOpen && session.ReadBytes > 0)
            {
                string readData = Encoding.UTF8.GetString(message as byte[]);

                if (readData.StartsWith("\0")) return;
                receivedQueue.Enqueue(message as byte[]);
            }
        }

        public bool ReadBuffer(out byte[] data)
        {
            return receivedQueue.TryDequeue(out data);
        }

        public virtual void FlushBuffer()
        {
            while (!receivedQueue.IsEmpty)
            {
                receivedQueue.TryDequeue(out byte[] _);
            }
        }

        public void SendMessage(string message)
        {
            WriteData(message);
        }

        public void SendMessage(byte[] message)
        {
            Send(message);
        }

        private void WriteData(string data)
        {
            lock (critical_section)
            {
                FlushBuffer();
                if (this.IsOpen)
                {
                    try
                    {
                        // Send the binary data out the port
                        byte[] hexstring = Encoding.ASCII.GetBytes(data);
                        //There is a intermitant problem that I came across
                        //If I write more than one byte in succesion without a 
                        //delay the PIC i'm communicating with will Crash
                        //I expect this id due to PC timing issues ad they are
                        //not directley connected to the COM port the solution
                        //Is a ver small 1 millisecound delay between chracters

                        Send(hexstring);
                        //foreach (byte hexval in hexstring)
                        //{
                        //    byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                        //    _serialPort.Write(_hexval, 0, 3);
                        //}
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
