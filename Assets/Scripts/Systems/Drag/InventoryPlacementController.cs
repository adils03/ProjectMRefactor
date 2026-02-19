using System.Collections.Generic;
using UnityEngine;

public class InventoryPlacementController : MonoBehaviour
{
    [SerializeField] private DragDrop2D dragDrop;
    private Direction previousDirection;
    private Vector2Int previousOrigin;

    private List<IPlacementRule> placementRules;
    void Awake()
    {
        placementRules = new List<IPlacementRule>()
        {
            new GridPlacementRule(),
            new ShopPlacementRule()
        };
    }

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
      
        foreach (var rule in placementRules)
        {
            if (!rule.CanPlace(placedItem, grid, new Vector2Int(X, Y)))
            {
                ReturnToPreviousDirection(placedItem);
                return;
            }
        }
        placedItem.PlaceItem(new Vector2Int(X, Y), placedItem.GetDirection(), grid);

        foreach (var rule in placementRules)
        {
            rule.OnPlaced(placedItem);
        }
    }

    private void ReturnToPreviousDirection(ItemPlaced item)
    {
        item.PlaceItem(previousOrigin, previousDirection, item.GetGrid());
    }
}
