using System.Collections.Generic;
using UnityEngine;

public class InventoryRuntime
{
    public IEntity Owner { get; private set; }
    public GridSystem<GridObject> Grid { get; private set; }

    private readonly List<ItemPlaced> items = new();

    public InventoryRuntime(IEntity owner, GridSystem<GridObject> grid)
    {
        Owner = owner;
        Grid = grid;
    }

    public void AddItem(ItemPlaced item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemPlaced item)
    {
        items.Remove(item);
    }

    public ItemRuntime GetRuntimeAt(Vector2Int pos)
    {
        var obj = Grid.GetGridObject(pos.x,pos.y);
        return obj?.GetPlacedObject()?.ItemView?.Runtime;
    }
}
