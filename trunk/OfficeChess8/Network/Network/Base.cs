using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Globals;
using System.Runtime.InteropServices;

namespace Network
{
    abstract public class Base
    {
        // CONST MEMBERS /////////////////////////////////////////////////////////////////////////
        protected const Int32   MAX_PACKET_SIZE = 256;
        protected const Int32   MAX_CLIENTS = 1;
        protected const String  DEFAULT_IP = "127.0.0.1";
        protected const Int32   DEFAULT_PORT = 12345;
        protected const Int32   MAX_CONNECTION_QUEUE = 1;

        // MISC MEMBERS //////////////////////////////////////////////////////////////////////////
        protected byte[]        m_LastReceivedData = new byte[MAX_PACKET_SIZE];
        protected Int32         m_ConnectionID = 0;

        // EVENTS  ///////////////////////////////////////////////////////////////////////////////
        public delegate void NotifyNetworkError(String errorMsg);
        public NotifyNetworkError OnNetworkError;
        
        // METHODS ///////////////////////////////////////////////////////////////////////////////

        // returns current connection id
        public int GetConnectionID()
        {
            return m_ConnectionID;
        }

        // returns current connection id
        public void SetConnectionID(int connID)
        {
            m_ConnectionID = connID;
        }
    }
}
