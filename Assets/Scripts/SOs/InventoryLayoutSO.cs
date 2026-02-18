using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Inventory Layout")]
public class InventoryLayoutSO : ScriptableObject
{
    public int width = 10;
    public int height = 6;
    public List<PlacedShapeData> placedItems = new();
}

[System.Serializable]
public class PlacedShapeData
{
    public ItemSO itemType;
    public Vector2Int origin;
    public Direction dir;
}