using System;
using System.Net.Sockets;
using System.Text;

class SyncClient
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 8080);
        NetworkStream stream = client.GetStream();

        byte[] message = Encoding.UTF8.GetBytes("Hello from Client");
        stream.Write(message, 0, message.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Received: " + response);

        client.Close();
    }
}
