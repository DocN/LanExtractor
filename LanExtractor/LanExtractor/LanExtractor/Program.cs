using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LanExtractor
{
    class Program
    {
        private static string SERVER_ADDRESS = "192.168.1.3";
        private static string filedir = "";
        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                ipAddress = System.Net.IPAddress.Parse(SERVER_ADDRESS);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    byte[] msg = Encoding.ASCII.GetBytes(filedir);

                    // Send the data through the socket.  
                    int bytesSent = sender.Send(msg);
                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    while (true)
                    {
                        //System.Threading.Thread.Sleep(500);
                        // Receive the response from the remote device.  
                        int bytesRec = sender.Receive(bytes);
                        //Console.WriteLine(bytesRec);
                        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                        Console.WriteLine(watch.ElapsedMilliseconds);

                        //if no response in 2 seconds terminate
                        if (bytesRec == 0)
                        {
                            if (watch.ElapsedMilliseconds >= 2000)
                            {
                                watch.Stop();
                                break;
                            }
                        }
                        if(bytesRec != 0)
                        {
                            watch.Restart();
                        }
                        if (Encoding.ASCII.GetString(bytes, 0, bytesRec).Equals("All OK"))
                        {
                            break;
                        }
                    }

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            //C:\Users\DrN\AppData\Roaming\Microsoft\Windows\SendTo
            if(args.Length <= 0)
            {
                Console.WriteLine("No File");
                System.Threading.Thread.Sleep(1000);
                return 0;
            }

            Console.WriteLine(args[0]);
            filedir = args[0];
            
            StartClient();
            Console.WriteLine("Extraction complete, press any key to close");
            Console.ReadLine();
            return 0;
        }
    }
}
