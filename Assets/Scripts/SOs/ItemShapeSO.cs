using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Shape")]
public class ItemShapeSO : ScriptableObject
{
    public int width = 5;
    public int height = 5;
    public List<Vector2Int> itemCells = new();
}