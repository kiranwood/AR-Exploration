using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class ARController : MonoBehaviour
{
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private Camera arCamera;

    private void Update()
    {
        if (TryGetPointerPlaced(out Vector2 screenPos))
        {
            HandleInput(screenPos);
        }
    }

    // Checks if point hit a target
    private void HandleInput(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Target target = hitInfo.collider.GetComponent<Target>();
            if (target != null)
            {
                GameManager.Instance.AddPoint();
                Destroy(target.gameObject);
            }
        }
    }

    // Gets pointer position and returns if pointer was pressed
    private bool TryGetPointerPlaced(out Vector2 position)
    {
        Pointer pointer = Pointer.current;
        if (pointer != null && pointer.press.wasPressedThisFrame)
        {
            position = pointer.position.ReadValue();
            return true;
        }

        position = default;
        return false;
    }
}
