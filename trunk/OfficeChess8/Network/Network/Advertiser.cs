using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Network;

namespace Network
{
    public class Advertiser : Base
    {
        private Thread m_tAdvertiseServer = null;
        private const Int32 m_nPortNumber = 12346;

        // start broadcasting server details
        public void Start()
        {
            // start advertising this server
            if (m_tAdvertiseServer == null)
            {
                m_tAdvertiseServer = new Thread(new ThreadStart(AdvertiseServer));
                m_tAdvertiseServer.IsBackground = true;
                m_tAdvertiseServer.Start();
            }
        }

        // stop broadcasting server details
        public void Stop()
        {
            // start advertising this server
            if (m_tAdvertiseServer != null)
            {
                m_tAdvertiseServer.Abort();
                m_tAdvertiseServer = null;
            }
        }

        // advertizing thread
        private void AdvertiseServer()
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, m_nPortNumber);
            byte[] hostname = Encoding.ASCII.GetBytes(Dns.GetHostName());
            while (true)
            {
                server.SendTo(hostname, iep);
                Thread.Sleep(1000);
            }
        }
    }
}
