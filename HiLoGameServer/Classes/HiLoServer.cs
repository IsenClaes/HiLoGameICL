using System.Net;
using System.Net.Sockets;
using HiLoGameServer.Handlers;
using HiLoGameServer.Enums;
using HiLoGameServer.Helpers;

namespace HiLoGameServer.Classes
{
    /// <Summary>
    /// This class contains the logic of the server
    /// </Summary>
    public class HiLoServer
    {
        private TcpListener _listener = null!;
        public readonly List<ClientHandler> _clients;
        private int _currentPlayerIndex;
        private readonly HiLoGame _game;

        public int Min { get; set; }
        public int Max { get; set; }

        public HiLoServer()
        {
            _clients = new List<ClientHandler>();
            _currentPlayerIndex = 0;
            _game = new HiLoGame();
            Min = ServerConfigHelper.LowerBound;
            Max = ServerConfigHelper.UpperBound;
        }

        /// <summary>Starts the server and listen for clients.</summary>
        /// <returns></returns>
        /// <param></param>
        public void Start()
        {
            Console.WriteLine("Hi-Lo Game Server");

            _game.GenerateMysteryNumber(Min, Max);

            _listener = new TcpListener(IPAddress.Any, ServerConfigHelper.ServerPort);
            _listener.Start();

            Console.WriteLine("Game started. Waiting for clients...");

            while (true)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("New client connected.");

                ClientHandler clientHandler = new ClientHandler(client, this);
                _clients.Add(clientHandler);

                Thread clientThread = new Thread(clientHandler.HandleClient);
                clientThread.Start();
            }
        }

        /// <summary>Handles the messages received from the client.</summary>
        /// <returns></returns>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public void HandleClientMessage(ClientHandler sender, string message)
        {
            int guess = Convert.ToInt32(message);
            int playerIndex = _clients.IndexOf(sender);

            EGuessResult result = _game.ProcessGuess(guess);

            if (result == EGuessResult.Correct)
            {
                BroadcastMessage("Player " + (playerIndex + 1) + " guessed the mystery number (" + guess + ")!");
                RestartGame();
            }
            else
            {
                string resultString = GetGuessResultString(result);
                BroadcastMessage("Player " + (playerIndex + 1) + " guessed " + guess + ". Result: " + resultString);
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _clients.Count;
                _clients[_currentPlayerIndex].SendMessage("Your turn to guess!");
                for (int i = 0; i < _clients.Count; i++)
                {
                    if(i != _currentPlayerIndex)
                        _clients[i].SendMessage("Wait for your turn...");
                }
            }
        }

        /// <summary>Sends a message to all the clients.</summary>
        /// <returns></returns>
        /// <param name="message">The message to send.</param>
        private void BroadcastMessage(string message)
        {
            foreach (ClientHandler client in _clients)
            {
                client.SendMessage(message);
            }
        }

        /// <summary>Removes a client from the clients list.</summary>
        /// <returns></returns>
        /// <param name="client">The client to remove.</param>
        public void RemoveClient(ClientHandler client)
        {
            _clients.Remove(client);
            Console.WriteLine("Client disconnected.");
            BroadcastMessage("A player has left the game. Your player number might be decremented.");
            if(!_clients.Any())
                RestartGame();
        }

        /// <summary>Restarts a new game when the previous one is over.</summary>
        /// <returns></returns>
        /// <param></param>
        private void RestartGame()
        {
            Console.WriteLine("Game over. Starting a new game.");
            _game.GenerateMysteryNumber(Min, Max);
            _currentPlayerIndex = 0;

            foreach (ClientHandler client in _clients)
            {
                client.SendMessage("Game over.\n\nStarting a new game.");
                client.SendMessage("The mystery number is in the interval ["+Min+", "+Max+"]");
            }
        }

        /// <summary>Gets the answer to send to the client depending on his guest.</summary>
        /// <returns></returns>
        /// <param name="result">The result of client's guest.</param>
        private string GetGuessResultString(EGuessResult result)
        {
            switch (result)
            {
                case EGuessResult.Correct:
                    return "Correct";
                case EGuessResult.Higher:
                    return "Higher";
                case EGuessResult.Lower:
                    return "Lower";
                default:
                    return string.Empty;
            }
        }
    }
}
