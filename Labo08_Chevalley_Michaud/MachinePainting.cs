using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Labo08_Chevalley_Michaud
{
    public class MachinePainting
    {
        
            UdpClient udpClient;
            IPEndPoint sender;
            private bool _connected;
            public bool Connected { get { return _connected; } }
            public MachinePainting(string IP, int port)
            {
                udpClient = new UdpClient(IP, port);
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
                sender = new IPEndPoint(IPAddress.Any, 0);
                _connected = true;
            }

            public string SendReciveCommande(string command)
            {
                byte[] _commandBytes, _answerBytes;
                string answer = "";
                // clean all pending messages from receive buffer before sending command
                while (udpClient.Available > 0)
                    udpClient.Receive(ref sender);
                // send command as a packet of bytes
                _commandBytes = Encoding.ASCII.GetBytes(command);
                udpClient.Send(_commandBytes, _commandBytes.Length);
                try
                {
                    // wait and get answer as a packet of bytes, convert to string
                    _answerBytes = udpClient.Receive(ref sender);
                    answer = Encoding.ASCII.GetString(_answerBytes);
                    Console.WriteLine(answer);
                    _connected = true;
                }
                catch (SocketException)
                {
                    _connected = false;
                }
                return answer;
            }
            public bool ConveyorOn
            {
                get
                {
                    if (SendReciveCommande("ConveyorMoving") == "True")
                        return true;
                    else
                        return false;
                }
                set
                {
                    if (value)
                        SendReciveCommande("ConveyorStart");
                    else
                        SendReciveCommande("ConveyorStop");
                }
            }
            //BucketLocked
            public bool BucketLocked
            {
                get
                {
                    if (SendReciveCommande("BucketLocked") == "True")
                        return true;
                    else
                        return false;
                }
            }
            public bool BucketsLoadingEnabled
            {
                set
                {
                    if (value)
                        SendReciveCommande("EnableBucketsLoading");
                    else
                        SendReciveCommande("DisableBucketsLoading");
                }
            }

            public PigmentType PigmentDispenced
            {
                set
                {
                    switch (value)
                    {
                        case PigmentType.None:
                            SendReciveCommande("PaintNone");
                            break;
                        case PigmentType.A:
                            SendReciveCommande("PaintA");
                            break;
                        case PigmentType.B:
                            SendReciveCommande("PaintB");
                            break;
                        case PigmentType.C:
                            SendReciveCommande("PaintC");
                            break;
                        case PigmentType.D:
                            SendReciveCommande("PaintD");
                            break;
                    }
                }
            }
        
    }
}
