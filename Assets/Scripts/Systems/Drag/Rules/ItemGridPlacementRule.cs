using UnityEngine;

public class ItemGridPlacementRule : IPlacementRule
{
    public bool CanPlace(IPlaced item, GridSystem<GridObject> grid, Vector2Int origin)
    {
        if (item is ItemPlaced)
        {
            var dir = item.GetDirection();
            var anchored = ItemPlacementHelper.ChooseAnchorPosition(origin.x, origin.y, dir);
            var positions = item.GetGridPositionList(anchored);

            foreach (var p in positions)
            {
                var obj = grid.GetGridObject(p.x, p.y);
                if (obj == null || !obj.CanBuild()|| !obj.IsSlot())
                    return false;
            }
        }
        return true;
    }

    public void OnPlaced(IPlaced item)
    {
        if (!(item is ItemPlaced)) return;
        var positions = item.GetGridPositionList();
        var grid = item.GetGrid();
        foreach (var p in positions)
        {
            var obj = grid.GetGridObject(p.x, p.y);
            if (obj != null)
            {
                SlotRuntime slot = obj.GetPlacedSlot().GetRuntime() as SlotRuntime;
                if (slot != null)
                {
                    slot.ConnectItem(item.GetRuntime() as ItemRuntime);
                }
            }
        }
    }
}
