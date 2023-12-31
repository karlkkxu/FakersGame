using fakersGame.Services;
using FakersGame.Data;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace fakersGame.Hubs
{
    public class FakersHub : Hub
    {
        private readonly GameService _gameService;

        public FakersHub(GameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var player = new Player(Context.ConnectionId);
                _gameService.AddConnection(player);
                await base.OnConnectedAsync();
            }
            catch (InvalidOperationException ex)
            {
                await Clients.Caller.SendAsync("Error", ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = _gameService.GetPlayer(Context.ConnectionId);
            _gameService.RemoveConnection(player);
            await Clients.All.SendAsync("PlayerDisconnected");
            await base.OnDisconnectedAsync(exception!);
        }

        public void SetName(string name)
        {
            _gameService.SetPlayerName(Context.ConnectionId, name);
            Clients.All.SendAsync("SetName");
        } 

        public Player GetPlayer()
        {
            return _gameService.GetPlayer(Context.ConnectionId);
        }

        public List<Player> GetPlayers()
        {
            var players = _gameService.GetPlayers();
            Clients.All.SendAsync("GetPlayers");
            return players;
        }

        public void ToggleReady()
        {
            _gameService.TogglePlayerReady(Context.ConnectionId);
            Clients.All.SendAsync("ToggleReady");
            if (_gameService._gameRunning)
            {
                Clients.All.SendAsync("GameStarted");
            }
            if (_gameService._roundRunning)
            {
                Clients.All.SendAsync("RoundStarted");
            }
        }
        public async Task StartRound()
        {
            await _gameService.StartRound();
            await Clients.All.SendAsync("StartRound");
        }
    }
}
