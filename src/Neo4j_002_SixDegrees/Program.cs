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
    var mary = repository.FindPerson("Mary");

    var degreesOfSeparationByAllSimplePaths = repository.DegreesOfSeparationByAllSimplePaths(johnathan, mary, 4);
    var degreesOfSeparationByShortestPath = repository.DegreesOfSeparationByShortestPath(johnathan, mary);

    Console.WriteLine("All Simple Paths: ");
    foreach (var path in degreesOfSeparationByAllSimplePaths)
    {
      Console.WriteLine($"{path.Degrees} degrees: {string.Join(" => friends => ", path.Names)}");
    }

    Console.WriteLine("Shortest Path: ");
    Console.WriteLine($"{degreesOfSeparationByShortestPath.Degrees} degrees: {string.Join(" => friends => ", degreesOfSeparationByShortestPath.Names)}");

    Console.ReadKey();
  }
}
