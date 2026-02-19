using System.Collections.Generic;
using UnityEngine;

public static class ItemPlacementHelper
{
    public static Direction GetNextDir(Direction dir)
    {
        switch (dir)
        {
            default:
            case Direction.Down: return Direction.Left;
            case Direction.Left: return Direction.Up;
            case Direction.Up: return Direction.Right;
            case Direction.Right: return Direction.Down;
        }
    }

    public static int GetRotationAngle(Direction dir)
    {
        switch (dir)
        {
            default:
            case Direction.Down: return 0;
            case Direction.Left: return 90;
            case Direction.Up: return 180;
            case Direction.Right: return 270;
        }
    }

    public static List<Vector2Int> GetGridPositionList(Vector2Int offset, Direction dir, List<Vector2Int> itemCells)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        foreach (Vector2Int gridPos in itemCells)
        {
            switch (dir)
            {
                default:
                case Direction.Down:
                    gridPositionList.Add(offset + new Vector2Int(gridPos.x, gridPos.y));
                    break;
                case Direction.Up:
                    gridPositionList.Add(offset + new Vector2Int(-gridPos.x - 1, -gridPos.y - 1));
                    break;
                case Direction.Left:
                    gridPositionList.Add(offset + new Vector2Int(gridPos.y, -gridPos.x - 1));
                    break;
                case Direction.Right:
                    gridPositionList.Add(offset + new Vector2Int(-gridPos.y - 1, gridPos.x));
                    break;
            }
        }
        return gridPositionList;
    }

    public static List<Vector3> GetWorldPositionList(Vector3 origin, Direction dir, float cellSize, List<Vector2Int> itemCells)
    {
        List<Vector3> worldPositionList = new List<Vector3>();
        foreach (Vector2Int gridPos in itemCells)
        {
            Vector3 cellSizeOffSet = new Vector3(cellSize, cellSize, 0) * .5f;
            switch (dir)
            {
                default:
                case Direction.Down:
                    worldPositionList.Add(origin + cellSizeOffSet + (new Vector3(gridPos.x, gridPos.y, 0) * cellSize));
                    break;
                case Direction.Up:
                    worldPositionList.Add(origin + cellSizeOffSet + (new Vector3(-gridPos.x - 1, -gridPos.y - 1, 0) * cellSize));
                    break;
                case Direction.Left:
                    worldPositionList.Add(origin + cellSizeOffSet + (new Vector3(gridPos.y, -gridPos.x - 1, 0) * cellSize));
                    break;
                case Direction.Right:
                    worldPositionList.Add(origin + cellSizeOffSet + (new Vector3(-gridPos.y - 1, gridPos.x, 0) * cellSize));
                    break;
            }
        }
        return worldPositionList;
    }

     public static List<Vector2Int> GetSynergyGridPositionList(Vector2Int offset, Direction dir,Vector2Int itemOrigin, List<Vector2Int> synergyCells)
    {
        List<Vector2Int> result = new();

        if (synergyCells == null)
            return result;

        foreach (Vector2Int gridPos in synergyCells)
        {
            Vector2Int local = gridPos - itemOrigin;

            switch (dir)
            {
                default:
                case Direction.Down:
                    result.Add(offset + new Vector2Int(local.x, local.y));
                    break;

                case Direction.Up:
                    result.Add(offset + new Vector2Int(-local.x - 1, -local.y - 1));
                    break;

                case Direction.Left:
                    result.Add(offset + new Vector2Int(local.y, -local.x - 1));
                    break;

                case Direction.Right:
                    result.Add(offset + new Vector2Int(-local.y - 1, local.x));
                    break;
            }
        }

        return result;
    }

    public static Vector2Int ChooseAnchorPosition(int x, int y, Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return new Vector2Int(x, y);
            case Direction.Left:
                return new Vector2Int(x, y + 1);
            case Direction.Up:
                return new Vector2Int(x + 1, y + 1);
            case Direction.Right:
                return new Vector2Int(x + 1, y);
        }
        return new Vector2Int(x, y);
    }

    
}
