using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Pippi_AsyncSocketLib
{
    public class AsyncSocketServer
    {
        IPAddress mIP;
        int mPort;
        TcpListener mServer;

        //Server inizia ad ascoltare
        public async void InAscolto(IPAddress ipaddr=null,int port = 23000)
        {
            if (ipaddr==null)
            {
                ipaddr = IPAddress.Any;
            }
            if (port<0||port<65535)
            {
                port = 23000;
            }

            mIP = ipaddr;
            mPort = port;

            mServer = new TcpListener(mIP, mPort);

            Debug.WriteLine("Server in ascolto su IP: {0} - Porta: {1}", mIP.ToString(), mPort.ToString());

            mServer.Start();
            Debug.WriteLine("Server avviato...");
            TcpClient client = await mServer.AcceptTcpClientAsync();
            Debug.WriteLine("Client Connesso: " + client.Client.RemoteEndPoint);
            RiceviMessaggio(client);

        }

        public async void RiceviMessaggio(TcpClient client) 
        {
            NetworkStream stream = null;
            StreamReader reader = null;

            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[512];
                int nBytes = 0;
                while (true)
                {
                    Debug.WriteLine("In attesa di un messaggio");
                    nBytes = await reader.ReadAsync(buff, 0, buff.Length);

                    if (nBytes==0)
                    {
                        Debug.WriteLine("Client Disconnesso");
                        break;
                    }
                    string recvText = new string(buff);
                    Debug.WriteLine("N byte: {0}, Messaggio: {1}", nBytes, recvText);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Errore: "+ex.Message);
            }
        }
    }
}
