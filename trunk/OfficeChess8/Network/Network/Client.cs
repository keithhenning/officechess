using System;
using System.Collections.Generic;
using System.Text;
using Network;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Globals;

namespace Network
{
    public class Client : Base
    {

        private Socket      m_ClientSocket = null;
        private Thread      m_tClientThread = null;
        private IPAddress   m_TargetIP;
        private Int32       m_TargetPort;

        public Client()
        {
            // get unique hash code for this connection
            m_ConnectionID = this.GetHashCode();
        }

        // set target IP for this client
        public void SetTargetIP(String ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out m_TargetIP))
            {
                OnNetworkError("Unable to parse IP address.");
            }
        }

        // set target port for this client
        public void SetTargetPort(int portNumber)
        {
            m_TargetPort = portNumber;
        }

        // send object to connected server
        public bool Send(object objectToSend)
        {
            try
            {
                // start listening thread
                m_tClientThread = new Thread(new ParameterizedThreadStart(SendTask));
                m_tClientThread.IsBackground = true;
                m_tClientThread.Name = "OfficeChessClientSendThread";
                m_tClientThread.Start(objectToSend);
            }
            catch (Exception e)
            {
                OnNetworkError("SocketException: " + e.Message);
                return false;
            }

            return true;
        }

        // send thread
        private void SendTask(object objectToSend)
        {
            try
            {
                // convert object to byte array
                byte[] dataToSend = Etc.ObjectToByteArray(objectToSend);

                // make sure data is converted correctly
                if (dataToSend != null)
                {
                    // check for connection
                    if (m_ClientSocket == null || !m_ClientSocket.Connected)
                    {
                        // connect tcp client
                        m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        m_ClientSocket.Connect(m_TargetIP, m_TargetPort);
                    }

                    if (m_ClientSocket.Connected && dataToSend.Length > 0)
                    {
                        // send data
                        m_ClientSocket.Send(dataToSend, dataToSend.Length, SocketFlags.None);

                        // disconnect
                        m_ClientSocket.Disconnect(true);
                    }
                }
            }
            catch (SocketException se)
            {
                OnNetworkError("SocketException: " + se.Message);
            }
        }
    }
}
