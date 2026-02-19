using System.Collections.Generic;
using UnityEngine;

public class InventoryBuilder
{
    public GridSystem<GridObject> Build(
       int width,
       int height,
       float cellSize,
       Vector3 origin)
    {
        GridSystem<GridObject> grid = null;

        grid = new GridSystem<GridObject>(
            width,
            height,
            cellSize,
            origin,
            (g, x, y) => new GridObject(g, x, y)
        );

        return grid;
    }

    public List<IPlaced> SpawnLayoutItems(
        InventoryLayoutSO layout,
        GridSystem<GridObject> grid,
        ItemFactory factory,
        IEntity owner,
        InventoryRuntime runtimeInventory,
        Transform parent)
    {
        var result = new List<IPlaced>();
        if (layout == null || layout.placedItems == null)
            return result;

        foreach (var data in layout.placedItems)
        {

            var placed = factory.CreateFull(
                data.itemType,
                owner,
                parent,
                Vector3.zero
            );
            placed.PlaceItem(data.origin, data.dir, grid);
            result.Add(placed);
            runtimeInventory.AddItem(placed);   
        }

        return result;
    }
}
