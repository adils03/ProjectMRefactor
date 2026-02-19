using UnityEngine;

public class InventoryPlacementController : MonoBehaviour
{
    [SerializeField] private DragDrop2D dragDrop;
    private Direction previousDirection;
    private Vector2Int previousOrigin;
    private void OnEnable()
    {
        dragDrop.OnDropped += HandleItemDropped;
        dragDrop.OnDragStarted += HandleDragStarted;
    }

    private void OnDisable()
    {
        dragDrop.OnDropped -= HandleItemDropped;
        dragDrop.OnDragStarted -= HandleDragStarted;
    }

    private void HandleDragStarted(Transform item)
    {
        var placedItem = item.GetComponent<ItemPlaced>();
        if (placedItem != null)
        {
            placedItem.ClearSlots();
            previousDirection = placedItem.GetDirection();
            previousOrigin = placedItem.GetRequestedOrigin();
        }
    }
    private void HandleItemDropped(Transform item, Vector3 dropPosition)
    {
        var placedItem = item.GetComponent<ItemPlaced>();
        if (placedItem == null) return;

        var grid = GridManager.Instance.GetGridFromWorldPosition(dropPosition);
        if (grid == null)
        {
            ReturnToPreviousDirection(placedItem);
            return;
        }

        grid.GetXY(dropPosition, out int X, out int Y);
        var ancoredOrigin = ItemPlacementHelper.ChooseAnchorPosition(X, Y, placedItem.GetDirection());
        var possibleSlotLocations = placedItem.GetGridPositionList(ancoredOrigin);

        foreach (var p in possibleSlotLocations)
        {
            var obj = grid.GetGridObject(p.x, p.y);
            Debug.Log($"Checking grid object at {p.x}, {p.y}");
            if (obj == null || !obj.CanBuild()) 
            {
                ReturnToPreviousDirection(placedItem);
                return;
            }
        }
        placedItem.PlaceItem(new Vector2Int(X, Y), placedItem.GetDirection(), grid);
    }

    private void ReturnToPreviousDirection(ItemPlaced item)
    {
        item.PlaceItem(previousOrigin, previousDirection, item.GetGrid());
    }
}
