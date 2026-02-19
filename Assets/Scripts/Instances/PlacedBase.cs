using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacedBase : IPlaced
{
    public event EventHandler OnPlaced;
    public event EventHandler OnRotated;
    public event EventHandler OnDestroyed;

    public IEntityRuntime Runtime { get; protected set; }
    protected ItemSO itemSO;
    protected Vector2Int origin;
    protected Vector2Int requestedOrigin;
    protected Direction dir;
    protected GridSystem<GridObject> grid;
    protected List<GridObject> slots = new();

    public IEntityRuntime GetRuntime() => Runtime;
    public Direction GetDirection() => dir;
    public Vector2Int GetRequestedOrigin() => requestedOrigin;
    public GridSystem<GridObject> GetGrid() => grid;

    public PlacedBase(ItemSO itemSO, IEntityRuntime runtime)
    {
        this.itemSO = itemSO;
        this.Runtime = runtime;
    }

    public virtual void PlaceItem(Vector2Int requestedOrigin, Direction dir, GridSystem<GridObject> grid)
    {
        this.dir = dir;
        this.grid = grid;
        this.requestedOrigin = requestedOrigin;

        ClearSlots();

        origin = ItemPlacementHelper.ChooseAnchorPosition(
            requestedOrigin.x,
            requestedOrigin.y,
            dir);

        BindToGridSlots();
        OnPlaced?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void BindToGridSlots()
    {

    }

    public virtual void ClearSlots()
    {

    }

    public virtual void DestroySelf()
    {
        ClearSlots();
        OnDestroyed?.Invoke(this, EventArgs.Empty);
    }



    public List<Vector2Int> GetGridPositionList(Vector2Int? overrideOrigin = null)
    {
        return ItemPlacementHelper.GetGridPositionList(
            overrideOrigin ?? origin,
            dir,
            itemSO.itemShape.itemCells);
    }

    public List<Vector2Int> GetCellOffsets()
    {
        return ItemPlacementHelper.GetGridPositionList(
            Vector2Int.zero,
            Direction.Down,
            itemSO.itemShape.itemCells);
    }



    public Vector3 GetWorldPosition()
    {
        return grid.GetWorldPosition(origin.x, origin.y);
    }

    public float GetRotation()
    {
        return -ItemPlacementHelper.GetRotationAngle(dir);
    }
    public void RotateClockwise()
    {
        dir = ItemPlacementHelper.GetNextDir(dir);
    }

    public override string ToString()
    {
        return itemSO.itemName;
    }

    public void SetDirection(Direction dir)
    {
        this.dir = dir;
        OnRotated?.Invoke(this, EventArgs.Empty);
    }
}
