using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop2D : MonoBehaviour
{
    [SerializeField] private InputActionReference pointerPosition;
    [SerializeField] private InputActionReference pointerPress;
    [SerializeField] private LayerMask draggableLayer;
    public event Action<Transform, Vector3> OnDropped;
    public event Action<Transform> OnDragStarted;
    private Camera cam;
    private Transform selectedObject;
    private Vector3 offset;
    private Vector2 currentPointerPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        pointerPosition.action.Enable();
        pointerPress.action.Enable();

        pointerPosition.action.performed += OnPointerMove;
        pointerPress.action.started += OnPointerDown;
        pointerPress.action.canceled += OnPointerUp;
    }

    private void OnDisable()
    {
        pointerPosition.action.performed -= OnPointerMove;
        pointerPress.action.started -= OnPointerDown;
        pointerPress.action.canceled -= OnPointerUp;

        pointerPosition.action.Disable();
        pointerPress.action.Disable();
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        currentPointerPos = context.ReadValue<Vector2>();

        if (selectedObject == null) return;

        Vector3 worldPos = cam.ScreenToWorldPoint(currentPointerPos);
        worldPos.z = 0;

        selectedObject.position = worldPos + offset;
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        currentPointerPos = pointerPosition.action.ReadValue<Vector2>();
        Vector3 worldPos = cam.ScreenToWorldPoint(currentPointerPos);
        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        Collider2D hit = Physics2D.OverlapPoint(worldPos2D, draggableLayer);

        if (hit != null)
        {
            selectedObject = hit.transform;
            offset = selectedObject.position - worldPos;
            OnDragStarted?.Invoke(selectedObject);
        }
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        if (selectedObject != null)
        {
            OnDropped?.Invoke(selectedObject, selectedObject.position);
            selectedObject = null;
        }
    }

    public void RefreshOffset()
    {
        if (selectedObject == null) return;

        Vector3 worldPos = cam.ScreenToWorldPoint(currentPointerPos);
        worldPos.z = 0;

        offset = selectedObject.position - worldPos;
    }

}
