using UnityEngine;

public interface IPlaced
{
    ItemView ItemView { get; }
    void DestroySelf();
    void PlaceItem(Vector2Int requestedOrigin, Direction dir, GridSystem<GridObject> grid);
}
