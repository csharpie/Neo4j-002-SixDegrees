namespace Neo4j_002_SixDegrees
{
  public class DegreesOfSeparation
  {
    public List<string> Names { get; set; }
    public int Degrees { get; set; }

    public DegreesOfSeparation(List<string> names, int degrees)
    {
      Names = names;
      Degrees = degrees;
    }
  }
}
