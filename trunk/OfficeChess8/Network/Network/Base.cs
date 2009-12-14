using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Network
{
    abstract public class Base
    {
        // CONST MEMBERS //////////////////////////////////////////////////////////////////////////
        protected const Int16 PACKETSIZE = 256;
        protected const Int16 MAX_CLIENTS = 2;
        protected const String DEFAULT_IP = "127.0.0.1";
        protected const Int32 DEFAULT_PORT = 12345;
        protected const Int16 MAX_CONNECTION_QUEUE = 1;

        // SOCKET //////////////////////////////////////////////////////////////////////////
        public class SocketPacket
        {
            public SocketPacket(System.Net.Sockets.Socket socket, int clientNumber)
            {
                m_CurrentSocket = socket;
                m_ClientNumber = clientNumber;
            }

            public System.Net.Sockets.Socket    m_CurrentSocket;
            public int                          m_ClientNumber;
            public byte[]                       m_DataBuffer = new byte[PACKETSIZE];
        }

        // MISC MEMBERS //////////////////////////////////////////////////////////////////////////
        protected IPEndPoint    m_LocalIPEndPoint;
        protected AsyncCallback m_DataReceiverCallBack;
        protected byte[]        m_LastReceivedData = new byte[PACKETSIZE];
    }
}
