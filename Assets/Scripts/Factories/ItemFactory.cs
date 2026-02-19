using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private ItemPlaced itemPlacedPrefab;
    [SerializeField] private ItemView itemViewPrefab;

    public ItemRuntime CreateRuntime(ItemSO data, IEntity owner)
    {
        return new ItemRuntime(data, owner);
    }

    public ItemView CreateView(ItemRuntime runtime, Transform parent)
    {
        var view = Instantiate(itemViewPrefab, parent);
        view.Bind(runtime, null);
        return view;
    }

    public ItemPlaced CreatePlaced(
        ItemSO data,
        ItemRuntime runtime,
        Transform parent)
    {
        var placed = Instantiate(itemPlacedPrefab, parent);

        var view = Instantiate(itemViewPrefab, placed.transform);
        view.Bind(runtime, placed);

        placed.Initialize(data, view);

        return placed;
    }

    public (ItemRuntime runtime, ItemPlaced placed, ItemView view) CreateFull(
        ItemSO data,
        IEntity owner,
        Transform parent,
        Vector3 position = default)
    {
        var runtime = CreateRuntime(data, owner);

        var placed = Instantiate(itemPlacedPrefab, position, Quaternion.identity, parent);
        var view = Instantiate(itemViewPrefab, placed.transform);

        view.Bind(runtime, placed);
        placed.Initialize(data, view);

        return (runtime, placed, view);
    }
}
