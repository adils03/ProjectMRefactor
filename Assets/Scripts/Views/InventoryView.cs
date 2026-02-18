using UnityEngine;


public class InventoryView : MonoBehaviour
{
    [SerializeField] private Transform cellPrefab;
    [SerializeField] private Color color;

    public void Build(GridSystem<GridObject> grid, float cellSize,Transform parent = null)
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        for (int y = 0; y < grid.GetHeight(); y++)
        {
            var pos = grid.GetWorldPosition(x, y) + Vector3.one * cellSize * 0.5f;

            var cell = Instantiate(cellPrefab, pos, Quaternion.identity, parent);
            cell.localScale = Vector3.one * (cellSize - 0.4f);

            var sr = cell.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = color;
        }
    }
}

