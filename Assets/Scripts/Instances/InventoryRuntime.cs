using System.Collections.Generic;
using UnityEngine;

public class InventoryRuntime
{
    public IEntity Owner { get; private set; }
    public GridSystem<GridObject> Grid { get; private set; }

    private readonly List<IPlaced> items = new();

    public InventoryRuntime(IEntity owner, GridSystem<GridObject> grid)
    {
        Owner = owner;
        Grid = grid;
    }

    public void AddItem(IPlaced item)
    {
        items.Add(item);
    }

    public void RemoveItem(IPlaced item)
    {
        items.Remove(item);
    }

}
