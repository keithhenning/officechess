using System;
using System.Collections.Generic;
using System.Text;
using Network;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Globals;

namespace Network
{
    public class Server : Base
    {
        public delegate void NotifyNetworkReceivedData(NetworkPackage nwPackage);
        public NotifyNetworkReceivedData OnNetworkReceivedData;   

        private Thread                  m_tServerThread = null;
        private TcpListener             m_TCPListener = null;
        private Socket      	        m_ClientSocket = null;
        private IPAddress               m_ServerIP = IPAddress.Parse(DEFAULT_IP);
        private Int32                   m_ServerPort = DEFAULT_PORT;
        private volatile bool           m_bRunThread = true;

        // set server IP
        public void SetServerIP(String ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out m_ServerIP))
            {
                Exception e = new Exception("Unable to parse IP address.");
                OnNetworkError(e);
            }
        }

        // set server port
        public void SetServerPort(int portNumber)
        {
            m_ServerPort = portNumber;
        }

        // start server
        public bool Start()
        {
            try
            {
                // start listening thread
                m_bRunThread = true;
                m_tServerThread = new Thread(new ThreadStart(ServerTask));
                m_tServerThread.IsBackground = true;
                m_tServerThread.Name = "OfficeChessServerListenThread";
                m_tServerThread.Start();
            }
            catch (SystemException e)
            {
                OnNetworkError(e);
                return false;
            }

            return true;
        }

        // stops this server
        public bool Stop()
        {
            try
            {
                // stop listening thread
                m_bRunThread = false;
                m_TCPListener.Stop();
                Console.WriteLine("Listener has stopped...");
            }
            catch (SystemException e)
            {
                OnNetworkError(e);
                return false;
            }

            return true;
        }

        // listens for incoming transmissions
        private void ServerTask()
        {
			try
			{
				// initialize tcp listener
                m_TCPListener = new TcpListener(m_ServerIP, m_ServerPort);
				m_TCPListener.Start();
				Console.WriteLine("Listening for incoming data...");

                while (m_bRunThread)
				{
                    // block until connection established
                    m_ClientSocket = m_TCPListener.AcceptSocket();

                    // if client connected start receiving thread
                    if (m_ClientSocket.Connected)
                    {
                        Console.WriteLine("Receiving data from: " + m_ClientSocket.RemoteEndPoint.ToString());

                        // get bytes
                        int numbytes = m_ClientSocket.Receive(m_LastReceivedData);
                        Console.WriteLine("Received " + numbytes.ToString() + " bytes from: " + m_ClientSocket.RemoteEndPoint.ToString());

                        // disconnect
                        m_ClientSocket.Disconnect(true);

                        // try to parse incoming data to network package struct
                        NetworkPackage nwPackage = new NetworkPackage();
                        nwPackage = (NetworkPackage)Etc.ByteArrayToObject(m_LastReceivedData, nwPackage.GetType());

                        // trigger event
                        OnNetworkReceivedData(nwPackage);
                    }
				}
			}
			catch (SocketException se)
			{
                OnNetworkError(se);
			}
            catch (SystemException e)
            {
                OnNetworkError(e);
            }
        }
    }
}
