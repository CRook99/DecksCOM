public interface IOutlineDecisionStrategy
{
    bool ShouldPlaceOnEdge(Tile tile);
}
    
public class MovementStrategy : IOutlineDecisionStrategy
{
    public bool ShouldPlaceOnEdge(Tile neighbour)
    {
        return neighbour == null || !neighbour.Visited || !neighbour.Walkable();
    }
}
    
public class TargetingStrategy : IOutlineDecisionStrategy
{
    public bool ShouldPlaceOnEdge(Tile neighbour)
    {
        return neighbour == null || !neighbour.Visited;
    }
}
