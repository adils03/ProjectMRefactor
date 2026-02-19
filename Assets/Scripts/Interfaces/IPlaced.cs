using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaced
{
    public event EventHandler OnPlaced;
    public event EventHandler OnRotated;
    public event EventHandler OnDestroyed;
    void DestroySelf();
    void PlaceItem(Vector2Int requestedOrigin, Direction dir, GridSystem<GridObject> grid);

    void ClearSlots();

    IEntityRuntime GetRuntime();
    Direction GetDirection();
    void SetDirection(Direction dir);
    Vector2Int GetRequestedOrigin();
    GridSystem<GridObject> GetGrid();
    List<Vector2Int> GetGridPositionList(Vector2Int? overrideOrigin = null);
    public List<Vector2Int> GetCellOffsets();
    public Vector3 GetWorldPosition();
    public float GetRotation();
    void RotateClockwise();
}

