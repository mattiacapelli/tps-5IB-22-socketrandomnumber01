using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer
{
    public partial class Form1 : Form
    {
        public static string data = null;

            public int genNumber()
            {
                //Generate a Random Number
                Random rnd = new Random();
                int num = rnd.Next(1, 100);
                return num;
            }
            public bool checkauth(string data)
            {
                string[] dataarray = data.Split(';');
                if (dataarray[0] == "auth")
                {
                    if (dataarray[1] == "admin" && dataarray[2] == "admin")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
// Data buffer for incoming data.  
                byte[] bytes = new Byte[1024];

                // Establish the local endpoint for the socket.  
                // Dns.GetHostName returns the name of the   
                // host running the application.  
                IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);

                // Create a TCP/IP socket.  
                Socket listener = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and   
                // listen for incoming connections.  
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(10);

                    // Start listening for connections.  
                    while (true)
                    {
                        Console.WriteLine("Waiting for a connection...");
                        lbl_status.Text = "Waiting for a connection...";
                        // Program is suspended while waiting for an incoming connection.  
                        Socket handler = listener.Accept();
                        data = null;

                        // An incoming connection needs to be processed.  
                        while (true)
                        {
                            int bytesRec = handler.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (data.IndexOf("<EOF>") > -1)
                            {
                                break;
                            }
                        }

                        Console.WriteLine("Text received : {0}", data);
                        if(checkauth(data)){
                            int ngen = genNumber();
                            label1.Text = ngen.ToString();
                            byte[] msg = Encoding.ASCII.GetBytes(ngen.ToString());
                            handler.Send(msg);
                        }
                        // Echo the data back to the client.  
                    }

                }
                catch (Exception a)
                {
                    Console.WriteLine(a.ToString());
                }            
        }
    }
}