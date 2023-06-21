using System.Net.Sockets;
using System.Text;
using HiLoGameClient.Helpers;

namespace HiLoGameClient.Classes
{
    /// <Summary>
    /// This class contains the logic of the client
    /// </Summary>
    class HiLoClient
    {
        private TcpClient _client = null!;
        private NetworkStream _stream = null!;
        private StreamReader _reader = null!;
        private StreamWriter _writer = null!;

        /// <summary>Starts the server and asks for a guess when it's the client's turn.</summary>
        /// <returns></returns>
        /// <param></param>
        public void Start()
        {
            try
            {
                _client = new TcpClient(ClientConfigHelper.ServerHost, ClientConfigHelper.ServerPort);
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();

                while (true)
                {
                    string input = GetValidGuess();
                    SendMessage(input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                _writer.Close();
                _reader.Close();
                _stream.Close();
                _client.Close();
            }
        }

        /// <summary>Receives a message from the server and prints it.</summary>
        /// <returns></returns>
        /// <param></param>
        /// <param></param>
        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    string message = _reader.ReadLine() ?? string.Empty;
                    Console.WriteLine("Server: " + message);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("There seems to be some technical difficulties with the server. Please restart the game.");
            }
        }

        /// <summary>Sends a message to the server.</summary>
        /// <returns></returns>
        /// <param name="message">The message.</param>
        private void SendMessage(string message)
        {
            _writer.WriteLine(message);
        }
        
        /// <summary>Checks if the input is correct. If not, asks for a new input.</summary>
        /// <returns>A valid guess</returns>
        /// <param></param>
        private string GetValidGuess()
        {
            int guess;
            while (!int.TryParse(Console.ReadLine(), out guess))
                Console.Write($"Invalid guess. Please enter a valid number: ");
            
            return guess.ToString();
        }
    }
}