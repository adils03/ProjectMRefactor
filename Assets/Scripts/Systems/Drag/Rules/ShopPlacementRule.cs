using UnityEngine;

public class ShopPlacementRule : IPlacementRule
{
    // private ShopSystem shop;

    // public ShopPlacementRule(ShopSystem shop)
    // {
    //     this.shop = shop;
    // }

    // public bool CanPlace(ItemPlaced item, GridSystem<GridObject> grid, Vector2Int origin)
    // {
    //     return shop.HasEnoughMoney(item.GetPrice());
    // }

    // public void OnPlaced(ItemPlaced item)
    // {
    //     shop.Spend(item.GetPrice());
    // }
    public bool CanPlace(ItemPlaced item, GridSystem<GridObject> grid, Vector2Int origin)
    {
        return true;
    }

    public void OnPlaced(ItemPlaced item)
    {
        return;
    }
}
