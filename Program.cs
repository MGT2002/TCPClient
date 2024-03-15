using System.Net;
using System.Net.Sockets;
using System.Text;

if (args.Length < 2 || args.Length > 3)
{
    args = new string[3];
    Console.Write("Server = ");
    args[0] = Console.ReadLine();
    Console.Write("Word = ");
    args[1] = Console.ReadLine();
    Console.Write("Port = ");
    args[2] = Console.ReadLine();
}

string server = args[0];//Server

byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);//Word

int servPort = args.Length == 3 ? int.Parse(args[2]) : 7;

try
{
    using Socket sock = new(AddressFamily.InterNetwork, SocketType.Stream,
       ProtocolType.Tcp);

    sock.Connect(server, servPort);
    Console.WriteLine($"Connected to server {sock.RemoteEndPoint}!");
    Console.WriteLine("Sending echo string...");

    sock.Send(byteBuffer, 0, byteBuffer.Length, SocketFlags.None);
    Console.WriteLine("Sent {0} bytes to server...", byteBuffer.Length);

    int totalRcvd = 0;
    int bytesRcvd = 0;
    byte[] response = new byte[byteBuffer.Length];

    while (totalRcvd < byteBuffer.Length)
    {
        if ((bytesRcvd = sock.Receive(response, totalRcvd, response.Length,
            SocketFlags.None)) == 0)
        {
            Console.WriteLine("Connection closed prematurely.");
            break;
        }
        totalRcvd += bytesRcvd;
    }

    Console.WriteLine("Received {0} bytes from server: {1}", totalRcvd,
        Encoding.ASCII.GetString(response));
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}