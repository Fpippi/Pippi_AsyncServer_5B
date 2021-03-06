﻿using System;
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
        bool continua;
        List<TcpClient> mclient = new List<TcpClient> { };

        // Mette in ascolto il server
        public async void InizioAscolto(IPAddress ipaddr = null, int port = 23000)
        {
            //faccio dei controlli su IPAddress e sulle porte
            if (ipaddr == null)
            {
                //mIP = IPAddress.Any;
                ipaddr = IPAddress.Any;
            }
            if (port < 0 || port > 65535)
            {
                //mPort = 23000;
                port = 23000;
            }

            mIP = ipaddr;//aggiunte
            mPort = port;//aggiunte

            Debug.WriteLine("Avvio il server. IP: {0} - Porta: {1}", mIP.ToString(), mPort.ToString());
            //creare l'oggetto server
            mServer = new TcpListener(mIP, mPort);

            //avviare il server
            mServer.Start();
            continua = true;
            while (continua)
            {
                // mettermi in ascolto
                TcpClient client = await mServer.AcceptTcpClientAsync();
                Debug.WriteLine("Client connesso: " + client.Client.RemoteEndPoint);
                mclient.Add(client);
                RiceviMessaggi(client);
            }
        }
        public async void RiceviMessaggi(TcpClient client)
        {

            NetworkStream stream = null;
            StreamReader reader = null;
            try
            {
                stream = client.GetStream();
                reader = new StreamReader(stream);
                char[] buff = new char[512];

                //ricezione effettiva
                while (continua)
                {
                    Debug.WriteLine("Pronto ad ascoltare...");
                    int nBytes = await reader.ReadAsync(buff, 0, buff.Length);
                    if (nBytes == 0)
                    {
                        Debug.WriteLine("Client disconnesso.");
                        break;
                    }
                    string recvMessage = new string(buff);
                    Debug.WriteLine("Returned bytes: {0}. Messaggio: {1}", nBytes, recvMessage);
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        private void rimuoviClient(TcpClient client)
        {
            if (mclient.Contains(client))
            {
                mclient.Remove(client);
            }
        }
        public void inviaATutti(string messaggio)
        {
            try
            {
                foreach (var client in mclient)
                {

                    byte[] buff = Encoding.ASCII.GetBytes(messaggio);
                    client.GetStream().WriteAsync(buff, 0, buff.Length);

                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Errore: " + ex);
            }
            
        }

        public void disconetti()
        {
            try
            {
                foreach (var client in mclient)
                {
                    client.Close();
                    rimuoviClient(client);
                }
                mServer.Stop();
                mServer = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
            
    }
}
