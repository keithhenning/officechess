using System;
using System.Collections.Generic;
using System.Text;
using Network;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Network
{
    public class Client : Base
    {

        private TcpClient   m_TCPClient = null;
        private IPAddress   m_TargetIP;
        private Int32       m_TargetPort;


        public bool Connect(String ipAddress, Int32 port)
        {
            try
            {
                // store connection data
                m_TargetIP = IPAddress.Parse(ipAddress);
                m_TargetPort = port;

                // connect tcp client
                m_TCPClient = new TcpClient(m_TargetIP.ToString(), m_TargetPort);
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.Message);
                return false;
            }

            return true;
        }

        // send string to connected server
        public bool Send(String stringToSend)
        {
            try
            {
                if (m_TCPClient.Connected && stringToSend.Length > 0)
                {
                    // convert to byte array
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    byte[] dataToSend = encoding.GetBytes(stringToSend);

                    // send data
                    m_TCPClient.Client.Send(dataToSend, dataToSend.Length, SocketFlags.None);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.Message);
                return false;
            }

            return true;
        }

        // send byte array to connected server
        public bool Send(byte[] dataToSend)
        {
            try
            {
                if (m_TCPClient.Connected && dataToSend != null)
                {
                    // send data
                    m_TCPClient.Client.Send(dataToSend, dataToSend.Length, SocketFlags.None);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException: {0}", se.Message);
                return false;
            }

            return true;
        }
    }
}
