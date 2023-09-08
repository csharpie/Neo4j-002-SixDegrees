namespace Neo4j_002_SixDegrees
{
  public class Person
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Person(int id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}
