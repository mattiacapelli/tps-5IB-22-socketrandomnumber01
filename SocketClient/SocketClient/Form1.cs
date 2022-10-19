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


namespace SocketClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5000);

                // Create a TCP/IP  socket.  
                Socket sender_ = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender_.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender_.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.  
                    string msg = "auth;" + text_username.Text + ";" + text_password.Text;

                    // Send the data through the socket.  
                    int bytesSent = sender_.Send(Encoding.ASCII.GetBytes(msg));

                    // Receive the response from the remote device.  
                    int bytesRec = sender_.Receive(bytes);
                        label2.Text = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    // Release the socket.  
                    sender_.Shutdown(SocketShutdown.Both);
                    sender_.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception a)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception a)
            {
                Console.WriteLine(a.ToString());
            }
        }
    }
}