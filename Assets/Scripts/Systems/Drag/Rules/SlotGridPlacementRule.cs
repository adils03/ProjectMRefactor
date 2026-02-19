using UnityEngine;

public class SlotGridPlacementRule : IPlacementRule
{
    public bool CanPlace(IPlaced item, GridSystem<GridObject> grid, Vector2Int origin)
    {
        if (item is SlotPlaced)
        {
            var dir = item.GetDirection();
            var anchored = ItemPlacementHelper.ChooseAnchorPosition(origin.x, origin.y, dir);
            var positions = item.GetGridPositionList(anchored);

            foreach (var p in positions)
            {
                var obj = grid.GetGridObject(p.x, p.y);
                if (obj == null || !obj.CanBuild() || obj.IsSlot())
                    return false;
            }
        }
        return true;
    }

    public void OnPlaced(IPlaced item) { }
}