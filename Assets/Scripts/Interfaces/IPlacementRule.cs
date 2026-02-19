using UnityEngine;

public interface IPlacementRule
{
    bool CanPlace(IPlaced item, GridSystem<GridObject> grid, Vector2Int origin);
    void OnPlaced(IPlaced item);
}
