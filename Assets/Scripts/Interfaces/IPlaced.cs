using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaced
{
    event EventHandler OnPlaced;
    event EventHandler OnRotated;
    event EventHandler OnDestroyed;
    void DestroySelf();
    void PlaceItem(Vector2Int requestedOrigin, Direction dir, GridSystem<GridObject> grid);

    void ClearSlots();
    IEntityRuntime GetRuntime();
    Direction GetDirection();
    void SetDirection(Direction dir);
    Vector2Int GetRequestedOrigin();
    GridSystem<GridObject> GetGrid();
    List<Vector2Int> GetGridPositionList(Vector2Int? overrideOrigin = null);
    List<Vector2Int> GetCellOffsets();
    Vector3 GetWorldPosition();
    float GetRotation();
    void RotateClockwise();
}

