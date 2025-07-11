using System.Net;
using System.Net.Sockets;
using System.Text;

/*
public void DoSth()
{
    // IP и порт
    IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    int port = 9600;

    TcpListener listener = new TcpListener(ipAddress, port);

    try
    {
        listener.Start();
        Console.WriteLine($"Сервер запущен на {ipAddress}:{port}");

        while (true)
        {
            Console.WriteLine("Ожидание подключения клиента...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клиент подключен!");

            NetworkStream stream = client.GetStream();

            // Отправка handshake-сообщения клиенту
            string handshakeMessage = "HANDSHAKE_OK";
            byte[] handshakeBytes = Encoding.UTF8.GetBytes(handshakeMessage);
            stream.Write(handshakeBytes, 0, handshakeBytes.Length);
            Console.WriteLine("Handshake отправлен клиенту.");

            // Чтение данных от клиента
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string clientMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Получено от клиента: {clientMessage}");

            // Закрытие соединения
            stream.Close();
            client.Close();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
    finally
    {
        listener.Stop();
    }

}
*/
