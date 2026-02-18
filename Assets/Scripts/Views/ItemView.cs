using TMPro;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshProUGUI cooldownText;

    public ItemRuntime Runtime { get; private set; }
    public ItemPlaced ItemPlaced { get; private set; }

    public void Bind(ItemRuntime runtime, ItemPlaced placedItem)
    {
        Runtime = runtime;
        ItemPlaced = placedItem;

        spriteRenderer.sprite = runtime.Data.itemImage;
        spriteRenderer.sortingOrder = 1;

        runtime.OnCooldownChanged += HandleCooldownChanged;

        if (ItemPlaced != null)
            ItemPlaced.OnMoved += HandleGridMoved;

        RefreshCooldownText();
    }

    public void Unbind()
    {
        if (Runtime != null)
            Runtime.OnCooldownChanged -= HandleCooldownChanged;

        if (ItemPlaced != null)
            ItemPlaced.OnMoved -= HandleGridMoved;
    }

    private void HandleCooldownChanged(object sender, System.EventArgs e)
    {
        RefreshCooldownText();
    }

    private void HandleGridMoved(object sender, System.EventArgs e)
    {
    }

    private void RefreshCooldownText()
    {
        if (cooldownText == null || Runtime == null) return;
        cooldownText.text = Runtime.CurrentCooldown > 0
            ? Mathf.Ceil(Runtime.CurrentCooldown).ToString()
            : "";
    }

    private void OnDestroy()
    {
        Unbind();
        Runtime?.DisposeRuntimeSynergyRules();
    }
}
