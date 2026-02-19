using UnityEngine;

public interface IPlacementRule
{
    bool CanPlace(ItemPlaced item, GridSystem<GridObject> grid, Vector2Int origin);
    void OnPlaced(ItemPlaced item);
}
