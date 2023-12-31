using System.Collections.Generic;
using FakersGame.Data;

namespace fakersGame.Services
{
    public class GameService
    {
        private readonly QuestionService _questionService;
        private readonly List<Player> _players = new List<Player>();
        private List<Question> _questions = new List<Question>();
        public bool _gameRunning = false;
        public bool _roundRunning = false;
        private Random rng = new Random();

        public GameService(QuestionService questionService)
        {
            _questionService = questionService;
        }
        public void AddConnection(Player player)
        {
            if (!_gameRunning)
                _players.Add(player);
            else
                throw new InvalidOperationException("Game already running, join next game");
        }

        public void RemoveConnection(Player player)
        {
            _players.Remove(player);
        }

        public Player GetPlayer(string connectionId)
        {
            var player = _players.Find(player => player.ConnectionId == connectionId);
            if (player == null)
            {
                throw new ArgumentException("Player not found", nameof(connectionId));
            }
            return player;
        }

        public void SetPlayerName(string connectionId, string name)
        {
            var player = GetPlayer(connectionId);
            player.Name = name;
        }

        public void SetPlayerReady(string connectionId)
        {
            var player = GetPlayer(connectionId);
            player.IsReady = true;
        }

        public void TogglePlayerReady(string connectionId)
        {
            var player = GetPlayer(connectionId);
            player.IsReady = !player.IsReady;
            AllPlayersReady();
        }

        public bool AllPlayersReady()
        {
            if (_players.Count < 2)
                return false;
            if (!_gameRunning)
            {
                _gameRunning = _players.All(player => player.IsReady);
                return _gameRunning;
            }
            _roundRunning = _players.All(player => player.IsReady);
            return _roundRunning;
        }

        public List<Player> GetPlayers()
        {
            return _players;
        }
        public async Task GenerateQuestions()
        {
            _questions = await _questionService.GetQuestions();
        }
        public async Task StartRound()
        {
            await GenerateQuestions();
            foreach (var player in _players)
            {
                player.HasAnswered = false;
                player.IsReady = false;
                player.IsFaker = false;
                player.Questions = new string[3];
                player.Answers = new string[3];
                player.Votes = new List<Player>();
            }
            _players[rng.Next(_players.Count)].IsFaker = true;
            foreach (var player in _players)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (player.IsFaker)
                        player.Questions[i] = _questions[i].FakerVersion;
                    else
                        player.Questions[i] = _questions[i].RealVersion;
                }
            }
        }
    }
}