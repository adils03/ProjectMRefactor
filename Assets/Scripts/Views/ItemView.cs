using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Transform cellPrefab;
    [SerializeField] private Transform cellsParent;
    [SerializeField] private bool debugTick;

    public IEntityRuntime Runtime { get; private set; }
    public IPlaced ItemPlaced { get; private set; }

    private List<Transform> cellInstances = new();

    public void Bind(IEntityRuntime runtime, IPlaced placed)
    {
        Runtime = runtime;
        ItemPlaced = placed;

        spriteRenderer.sprite = runtime.Data.itemImage;
        spriteRenderer.sortingOrder = 1;


        placed.OnPlaced += HandlePlaced;
        placed.OnRotated += HandleRotated;
        placed.OnDestroyed += HandleDestroyed;

        GenerateCollidersAndCells();
        RefreshCooldownText();
    }

    void Update()
    {
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            if(Runtime is ItemRuntime itemRuntime && debugTick)
            {
                itemRuntime.Tick();
            }
        }
    }

    void GenerateCollidersAndCells()
    {
        var offsets = ItemPlaced.GetCellOffsets();
        float size = GridManager.Instance.CellSize;

        foreach (var off in offsets)
        {
            var col = gameObject.AddComponent<BoxCollider2D>();
            col.size = Vector2.one * size;
            col.offset = new Vector2(
                off.x * size + size * .5f,
                off.y * size + size * .5f);

            var cell = Instantiate(cellPrefab, cellsParent);
            cell.localScale = new Vector3(size, size, 1);
            cell.localPosition = new Vector3(
                off.x * size + size * .5f,
                off.y * size + size * .5f,
                0);

            cellInstances.Add(cell);
        }
    }

    void HandlePlaced(object sender, System.EventArgs e)
    {
        transform.SetPositionAndRotation(ItemPlaced.GetWorldPosition(), Quaternion.Euler(0, 0, ItemPlaced.GetRotation()));
    }

    void HandleRotated(object sender, System.EventArgs e)
    {
        transform.rotation = Quaternion.Euler(0, 0, ItemPlaced.GetRotation());
    }

    public void RotateAroundMouse()
    {
        Vector3 pivot = UtilsClass.GetMouseWorldPosition();
        float angle = -90f;

        transform.RotateAround(
            pivot,
            Vector3.forward,
            angle
        );
        ItemPlaced.RotateClockwise();
    }


    void HandleDestroyed(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (ItemPlaced != null)
        {
            ItemPlaced.OnPlaced -= HandlePlaced;
            ItemPlaced.OnRotated -= HandleRotated;
            ItemPlaced.OnDestroyed -= HandleDestroyed;
        }

        if (Runtime is ItemRuntime itemRuntime)
            itemRuntime.DisposeRuntimeSynergyRules();
    }

    private void RefreshCooldownText()
    {
        if (cooldownText == null || Runtime == null) return;

        if (Runtime is not ItemRuntime itemRuntime || !itemRuntime.IsCooldownItem)
        {
            cooldownText.text = "";
            return;
        }

        cooldownText.text = itemRuntime.CurrentCooldown > 0
            ? Mathf.Ceil(itemRuntime.CurrentCooldown).ToString()
            : "";
    }
}
