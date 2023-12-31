using Microsoft.Azure.Cosmos;
using FakersGame.Data;

public class QuestionService
{
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;
    public QuestionService(IConfiguration configuration)
    {
        string endpoint = configuration["CosmosDB:Endpoint"];
        string key = configuration["CosmosDB:Key"];
        _cosmosClient = new CosmosClient(endpoint, key);
        _container = _cosmosClient.GetContainer("Faustice", "fakersGame");
    }

    public async Task<List<Question>> GetQuestions()
    {
        var sqlQueryText = "SELECT TOP 3 * FROM c ORDER BY c.LastAccessedTime ASC";
        var queryDefinition = new QueryDefinition(sqlQueryText);
        var queryResultSetIterator = _container.GetItemQueryIterator<Question>(queryDefinition);

        List<Question> questions = new List<Question>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<Question> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (Question question in currentResultSet)
            {
                questions.Add(question);
            }
        }

        return questions;
    }
}