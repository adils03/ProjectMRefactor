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

    public List<ItemPlaced> SpawnLayoutItems(
        InventoryLayoutSO layout,
        GridSystem<GridObject> grid,
        ItemFactory factory,
        IEntity owner,
        InventoryRuntime runtimeInventory,
        Transform parent)
    {
        var result = new List<ItemPlaced>();
        if (layout == null || layout.placedItems == null)
            return result;

        foreach (var data in layout.placedItems)
        {
            var runtime = factory.CreateRuntime(data.itemType, owner);

            var placed = factory.CreatePlaced(
                data.itemType,
                runtime,
                parent
            );

            placed.PlaceItem(data.origin, data.dir, grid);
            result.Add(placed);
            runtimeInventory.AddItem(placed);   
        }

        return result;
    }
}
