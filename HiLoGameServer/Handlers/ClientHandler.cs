using System.Net.Sockets;
using System.Text;
using HiLoGameServer.Classes;

namespace HiLoGameServer.Handlers
{
    /// <Summary>
    /// This class contains the handler for the connected clients
    /// </Summary>
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private NetworkStream _stream = null!;
        private StreamReader _reader = null!;
        private StreamWriter _writer = null!;
        private readonly HiLoServer _server;

        public ClientHandler(TcpClient client, HiLoServer server)
        {
            this._client = client;
            this._server = server;
        }

        /// <summary>Handles the client until he disconnects.</summary>
        /// <returns></returns>
        /// <param></param>
        public void HandleClient()
        {
            try
            {
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
                
                int playerNumber = _server._clients.IndexOf(this) + 1;

                SendMessage("Welcome to Hi-Lo Game, Player "+playerNumber+"! Waiting for other players to join...");
                SendMessage("The mystery number is in the interval ["+_server.Min+", "+_server.Max+"]");

                while (true)
                {
                    string message = _reader.ReadLine() ?? string.Empty;
                    _server.HandleClientMessage(this, message);
                }
            }
            catch (Exception)
            {
                _server.RemoveClient(this);
            }
            finally
            {
                _writer.Close();
                _reader.Close();
                _stream.Close();
                _client.Close();
            }
        }

        /// <summary>Sends a message to the client.</summary>
        /// <returns></returns>
        /// <param name="message">The message to send.</param>
        public void SendMessage(string message)
        {
            _writer.WriteLine(message);
        }
    }
}