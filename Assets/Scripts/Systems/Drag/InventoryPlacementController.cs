using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryPlacementController : MonoBehaviour
{
    [SerializeField] private DragDrop2D dragDrop;
    private Direction previousDirection;
    private Vector2Int previousOrigin;
    private Vector3 previousPosition;
    private ItemView currentlyDraggingItem;

    private List<IPlacementRule> placementRules;
    void Awake()
    {
        placementRules = new List<IPlacementRule>()
        {
            new ItemGridPlacementRule(),
            new SlotGridPlacementRule(),
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

    void Update()
    {
        if(currentlyDraggingItem != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentlyDraggingItem.RotateAroundMouse();
            dragDrop.RefreshOffset();
        }
    }

    private void HandleDragStarted(Transform item)
    {
        var itemView = item.GetComponent<ItemView>();
        var placedItem = itemView?.ItemPlaced;
        if (placedItem != null)
        {
            currentlyDraggingItem = itemView;
            placedItem.ClearSlots();
            previousDirection = placedItem.GetDirection();
            previousOrigin = placedItem.GetRequestedOrigin();
            previousPosition = item.transform.position;
        }
    }
    private void HandleItemDropped(Transform item, Vector3 dropPosition)
    {
        currentlyDraggingItem = null;

        var itemView = item.GetComponent<ItemView>();
        var placedItem = itemView?.ItemPlaced;
        if (placedItem == null) return;

        var grid = GridManager.Instance.GetGridFromWorldPosition(dropPosition);
        if (grid == null)
        {
            ReturnToPreviousDirection(placedItem, item);
            return;
        }

        grid.GetXY(dropPosition, out int X, out int Y);

        foreach (var rule in placementRules)
        {
            if (!rule.CanPlace(placedItem, grid, new Vector2Int(X, Y)))
            {
                ReturnToPreviousDirection(placedItem, item);
                return;
            }
        }
        placedItem.PlaceItem(new Vector2Int(X, Y), placedItem.GetDirection(), grid);

        foreach (var rule in placementRules)
        {
            rule.OnPlaced(placedItem);
        }
    }

    private void ReturnToPreviousDirection(IPlaced item,Transform itemTransform = null)
    {
        if (item.GetGrid() != null)
        {
            item.PlaceItem(previousOrigin, previousDirection, item.GetGrid());
            return;
        }

        itemTransform.transform.position = previousPosition;
        item.SetDirection(previousDirection);

    }
}
