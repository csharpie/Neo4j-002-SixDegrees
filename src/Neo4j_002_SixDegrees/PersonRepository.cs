using Neo4j.Driver;

namespace Neo4j_002_SixDegrees;

public class PersonRepository : IDisposable
{
  private readonly IDriver _driver;

  public PersonRepository(IDriver driver, bool shouldDeleteNodeAndRelationships = true, bool shouldSeedData = true)
  {
    _driver = driver;

    if (shouldDeleteNodeAndRelationships) DeleteAllNodesAndRelationships();
    if (shouldSeedData) SeedData();
  }

  public Person CreatePerson(string name)
  {
    using var session = _driver.Session(WithDatabase);

    return session.ExecuteWrite(transaction =>
    {
      var personNode = transaction.Run(@"
                        CREATE (they:Person)
                        SET they.name = $name
                        RETURN they.name AS Name, id(they) AS Id",
              new { name }
          ).Single();

      return new Person(
              personNode["Id"].As<int>(),
              personNode["Name"].As<string>()
          );
    });
  }

  public Person FindPerson(string name)
  {
    using var session = _driver.Session(WithDatabase);

    return session.ExecuteRead(transaction =>
    {
      var personNode = transaction.Run(@"
                        MATCH (they:Person)
                        WHERE they.name = $name
                        RETURN they.name AS Name, id(they) AS Id",
              new { name }
          ).Single();

      return new Person(
              personNode["Id"].As<int>(),
              personNode["Name"].As<string>()
          );
    });
  }

  public void MakeMutualFriends(Person they, Person them)
  {
    using var session = _driver.Session(WithDatabase);

    session.ExecuteWrite(transaction =>
    {
      return transaction.Run(@"
                       MATCH (they:Person), (them:Person)
                       WHERE they.name = $theysName AND them.name = $themsName
                       CREATE (they)-[fw:FRIENDS]->(them) RETURN fw",
              new Dictionary<string, object> { { "theysName", they.Name }, { "themsName", them.Name } }
          );
    });
  }

  public List<string> SuggestFriends(Person they)
  {
    using var session = _driver.Session(WithDatabase);

    return session.ExecuteRead(transaction =>
    {
      return transaction.Run(@"
                       MATCH (they:Person)-[f:FRIENDS]->(friend:Person)-[:FRIENDS*1..2]->(friendOfFriend:Person)
                       WHERE they.name = $name
                       RETURN friendOfFriend.name AS Name",
              new Dictionary<string, object> { { "name", they.Name } }
          ).Select(suggestedFriendNode => suggestedFriendNode["Name"].As<string>()).ToList();
    });
  }

  public List<string> DegreesOfSeparationByAllSimplePaths(Person they, Person them, int depth)
  {
    using var session = _driver.Session(WithDatabase);

    return session.ExecuteRead(transaction =>
    {
      return transaction.Run(@"
                       MATCH (they:Person)
                       MATCH (them:Person)
                       WHERE they.name = $theysName AND them.name = $themsName
                       CALL apoc.algo.allSimplePaths(they, them, 'FRIENDS', $depth)
                       YIELD path
                       RETURN path",
              new Dictionary<string, object> { { "theysName", they.Name }, { "themsName", them.Name }, { "depth", depth } }
          ).Select(suggestedFriendNode => suggestedFriendNode["Name"].As<string>()).ToList();
    });
  }

  public List<string> DegreesOfSeparationByShortestPath(Person they, Person them)
  {
    using var session = _driver.Session(WithDatabase);

    return session.ExecuteRead(transaction =>
    {
      return transaction.Run(@"
                      MATCH
                        (they:Person { name: $theysName }),
                        (them: Person { name: $themsName }),
                        path = (they)-[:FRIENDS *..4]-(them)
                      RETURN path AS shortestPath,
                        reduce(distance = 0, r in relationships(path) | distance + r.distance) AS totalDistance
                      ORDER BY totalDistance ASC
                      LIMIT 1",
              new Dictionary<string, object> { { "theysName", they.Name }, { "themsName", them.Name } }
          ).Select(suggestedFriendNode => suggestedFriendNode["Name"].As<string>()).ToList();
    });
  }

  public void Dispose()
  {
    _driver?.Dispose();
  }

  private void SeedData()
  {
    var johnathan = CreatePerson("Johnathan");
    var mark = CreatePerson("Mark");
    var phil = CreatePerson("Phil");
    var mary = CreatePerson("Mary");

    MakeMutualFriends(johnathan, mark);
    MakeMutualFriends(mark, mary);
    MakeMutualFriends(phil, mary);
    MakeMutualFriends(mark, mary);
  }

  private void DeleteAllNodesAndRelationships()
  {
    using var session = _driver.Session(WithDatabase);

    session.ExecuteWrite(transaction =>
    {
      return transaction.Run(@"
                MATCH (n) DETACH DELETE n"
          );
    });
  }

  private static void WithDatabase(SessionConfigBuilder sessionConfigBuilder)
  {
    sessionConfigBuilder.WithDatabase(Database());
  }

  private static string Database()
  {
    return Environment.GetEnvironmentVariable("NEO4J_DATABASE");
  }
}
