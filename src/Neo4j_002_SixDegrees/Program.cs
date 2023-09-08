using Neo4j.Driver;

namespace Neo4j_002_SixDegrees;

class Program
{
  static void Main(string[] args)
  {
    var driver = GraphDatabase.Driver(
                Environment.GetEnvironmentVariable("NEO4J_URI"),
                AuthTokens.Basic(
                    Environment.GetEnvironmentVariable("NEO4J_USER"),
                    Environment.GetEnvironmentVariable("NEO4J_PASSWORD")
                )
            );

    var repository = new PersonRepository(driver);

    var johnathan = repository.FindPerson("Johnathan");
    var suggestedFriendsForJohnathan = repository.SuggestFriends(johnathan);

    Console.WriteLine($"{johnathan.Name} should become friends with {string.Join(", ", suggestedFriendsForJohnathan)}");

    Console.ReadKey();
  }
}
