using UnityEngine;

[CreateAssetMenu(fileName = "ItemStatsSO", menuName = "Item/ItemStatsSO")]
public class ItemStatsSO : ScriptableObject
{
    public int cooldown;
    public int attackPower;
    public int defensePower;
}