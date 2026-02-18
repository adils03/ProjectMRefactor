using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Synergy")]
public class ItemSynergySO : ScriptableObject
{
    public int width;
    public int height;
    public Vector2Int itemOrigin;
    public ItemShapeSO baseItemShape;
    public List<Vector2Int> syngergyCells = new();
}