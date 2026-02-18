using UnityEngine;

public class InventoryFactory
{
    private readonly InventoryBuilder builder;
    private readonly ItemFactory itemFactory;

    public InventoryFactory(InventoryBuilder builder, ItemFactory itemFactory)
    {
        this.builder = builder;
        this.itemFactory = itemFactory;
    }

    public InventoryRuntime CreateInventory(
        InventoryLayoutSO layout,
        IEntity owner,
        Vector3 origin,
        Transform viewParent,
        InventoryView view)
    {
        var grid = builder.Build(
            layout.width,
            layout.height,
            GridManager.Instance.CellSize,
            origin
        );

        var runtime = new InventoryRuntime(owner, grid);

        GridManager.Instance.RegisterItemGrid(grid, runtime);

        if (view != null)
            view.Build(grid, GridManager.Instance.CellSize, viewParent);

        builder.SpawnLayoutItems(
            layout,
            grid,
            itemFactory,
            owner,
            runtime,
            viewParent
        );

        return runtime;
    }
}
