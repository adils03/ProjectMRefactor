using UnityEngine;

public class GridPlacementRule : IPlacementRule
{
    public bool CanPlace(ItemPlaced item, GridSystem<GridObject> grid, Vector2Int origin)
    {
        var dir = item.GetDirection();
        var anchored = ItemPlacementHelper.ChooseAnchorPosition(origin.x, origin.y, dir);
        var positions = item.GetGridPositionList(anchored);

        foreach (var p in positions)
        {
            var obj = grid.GetGridObject(p.x, p.y);
            if (obj == null || !obj.CanBuild())
                return false;
        }

        return true;
    }

    public void OnPlaced(ItemPlaced item) { }
}
