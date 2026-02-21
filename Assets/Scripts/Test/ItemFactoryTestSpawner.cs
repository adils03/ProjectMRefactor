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
    [SerializeField] private ItemSO itemSO2;

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

    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            IEntity dummyOwner = new DummyEntity();
            itemFactory.CreateFull(itemSO, dummyOwner, viewParent, UtilsClass.GetMouseWorldPosition());
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            IEntity dummyOwner = new DummyEntity();
            itemFactory.CreateFull(itemSO2, dummyOwner, viewParent, UtilsClass.GetMouseWorldPosition());
        }
    }
}

public class DummyEntity : IEntity
{
}
