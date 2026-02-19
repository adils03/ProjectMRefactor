using UnityEngine;
using UnityEngine.InputSystem;

public class ItemFactoryTestSpawner : MonoBehaviour
{
    [SerializeField] private ItemFactory itemFactory;
    [SerializeField] private InventoryView view;
    [SerializeField] private InventoryLayoutSO layout;
    [SerializeField] private Transform viewParent;
    [SerializeField] private Vector3 origin;
    [SerializeField] private ItemSO itemSO;

    private void Start()
    {
        var builder = new InventoryBuilder();
        var factory = new InventoryFactory(builder, itemFactory);

        IEntity dummyOwner = new DummyEntity();

        var runtime = factory.CreateInventory(
            layout,
            dummyOwner,
            origin,
            viewParent,
            view
        );


        Debug.Log("Inventory oluşturuldu: "
            + runtime.Grid.GetWidth() + "x"
            + runtime.Grid.GetHeight());
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            IEntity dummyOwner = new DummyEntity();
            itemFactory.CreateFull(itemSO, dummyOwner, viewParent, UtilsClass.GetMouseWorldPosition());
        }

        
    }
}

public class DummyEntity : IEntity
{
}
