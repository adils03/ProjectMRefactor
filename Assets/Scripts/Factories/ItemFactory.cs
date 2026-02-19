using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private ItemView itemViewPrefab;

    public IEntityRuntime CreateRuntime(ItemSO data, IEntity owner)
    {
        switch (data.itemType)
        {
            case ItemPlaceType.Slot:
                return new SlotRuntime(data, owner);
            case ItemPlaceType.Item:
                return new ItemRuntime(data, owner);
            default:
                Debug.LogError($"Unsupported item type: {data.itemType}");
                return null;
        }
    }

    public ItemView CreateView(
        IEntityRuntime runtime,
        Transform parent)
    {
        var view = Instantiate(itemViewPrefab, parent);
        view.Bind(runtime, null);
        return view;
    }

    public IPlaced CreatePlaced(ItemSO data, IEntityRuntime runtime)
    {
        switch (data.itemType)
        {
            case ItemPlaceType.Slot:
                return new SlotPlaced(data, runtime);
            case ItemPlaceType.Item:
                return new ItemPlaced(data, runtime);
            default:
                Debug.LogError($"Unsupported item type: {data.itemType}");
                return null;
        }
    }

    public IPlaced  CreateFull(
        ItemSO data,
        IEntity owner,
        Transform parent,
        Vector3 worldPosition)
    {
        var runtime = CreateRuntime(data, owner);

        var placed = CreatePlaced(data, runtime);

        var view = Instantiate(itemViewPrefab, worldPosition, Quaternion.identity, parent);

        view.Bind(runtime, placed);

        return placed;
    }
}
