using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Item/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public ItemShapeSO itemShape;
    public ItemSynergySO itemSynergy;
    public ItemPlaceType itemType;
    public ItemStatsSO itemStats;
    public List<ItemEffectSO> effects = new();
    public List<SynergyRuleSO> synergyRules = new();
}

