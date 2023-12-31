namespace FakersGame.Data
{
    public class Player
    {
        public string ConnectionId { get; set; }
        public string? Name { get; set; }
        public bool IsFaker { get; set; }
        public bool IsReady { get; set; } = false;
        public bool HasAnswered { get; set; } = false;
        public string[] Questions { get; set; } = new string[3];
        public string[] Answers { get; set; } = new string[3];
        public List<Player> Votes { get; set; } = new List<Player>();

        public Player(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public void SetAnswer(int index, string answer)
        {
            Answers[index] = answer;
        }
    }
}
