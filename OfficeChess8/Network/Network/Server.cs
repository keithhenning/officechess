using System;
using System.Collections.Generic;
using System.Text;
using Network;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Network
{
    public class Server : Base
    {
        private Thread                  m_tServerListenThread = null;
		private Thread                  m_tServerReceiveThread = null;
        private volatile TcpListener    m_TCPListener = null;
        private volatile Socket      	m_ClientSocket = null;
        private volatile bool           m_bAllowNewConnection = true;

        public bool StartListen()
        {
            try
            {
                // start listening thread
                m_tServerListenThread = new Thread(new ThreadStart(ServerTaskListen));
                m_tServerListenThread.IsBackground = true;
                m_tServerListenThread.Name = "OfficeChessServerListenThread";
                m_tServerListenThread.Start();
            }
            catch (SystemException se)
            {
                Console.WriteLine("Unable to start listening thread - " + se.Message);
                return false;
            }

            return true;
        }

        private void ServerTaskListen()
        {
			try
			{
				// initialize tcp listener
				m_TCPListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12345);
				m_TCPListener.Start();
				Console.WriteLine("Listening for incoming data...");

				while (true)
				{
                    // only do this when allowed
                    if (m_bAllowNewConnection)
                    {
                        // block until connection established
                        m_ClientSocket = m_TCPListener.AcceptSocket();

                        // if client connected start receiving thread
                        if (m_ClientSocket.Connected)
                        {
                            Console.WriteLine("Receiving data from: " + m_ClientSocket.RemoteEndPoint.ToString());

                            // start receive thread
                            m_tServerReceiveThread = new Thread(new ThreadStart(ServerTaskReceive));
                            m_tServerReceiveThread.IsBackground = true;
                            m_tServerReceiveThread.Name = "OfficeChessServerReceiveThread";
                            m_tServerReceiveThread.Start();
                            m_bAllowNewConnection = false;
                        }
                    }
				}
			}
			catch (SocketException se)
			{
				Console.WriteLine(se.Message);
			}
        }
		
		private void ServerTaskReceive()
		{
            byte[] bytes = new byte[256];
            int numbytes = 0;

            try
            {
                // get bytes
                numbytes = m_ClientSocket.Receive(bytes);
                Console.WriteLine("Received " + numbytes.ToString() + " bytes from: " + m_ClientSocket.RemoteEndPoint.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }

            // new connection allowed
            m_bAllowNewConnection = true;
		}
    }
}
