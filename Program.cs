﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Socket Listener acts as a server and listens to the incoming
// messages on the specified port and protocol.
public class SocketListener
{
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }

    public static void StartServer()
    {
        // Get Host IP Address that is used to establish a connection
        // In this case, we get one IP address of localhost that is IP : 127.0.0.1
        // If a host has multiple addresses, you will get a list of addresses
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {

            // Create a Socket that will use Tcp protocol
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.
            // We will listen 10 requests at a time
            //listener.Listen(10);
            listener.Listen(1);
            Console.WriteLine("Waiting for a connection...");
            Socket handler = listener.Accept();  /* orijinal hali soldaki comment idi. Socket var i burada tanımlayıp listener.accept i döngü içne aldım*/

            // Incoming data from the client.
            string data = null;
            //byte[] bytes = null;

            while (true)
            {
                //listener.Listen(1);
               // handler = listener.Accept();// bu satırı döngü dışından yukardan buraya aldım
               // string data = null; /*bu 2 satırı ekledim*/
                byte[] bytes = null;
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                //data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine("Text received : {0}", data);
                data = data.ToUpper();
                //Console.WriteLine("Text received : {0}", data); /*bu dahil alttaki 3 satır while döngüsü dışında idi*/
                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                if (data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

        //    Console.WriteLine("Text received : {0}", data);

        //    byte[] msg = Encoding.ASCII.GetBytes(data);
       //     handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\n Press any key to continue...");
        Console.ReadKey();
    }
}