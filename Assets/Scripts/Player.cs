using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction clickAction;

    void Awake()
    {
        clickAction = new InputAction(
            name: "Click",
            type: InputActionType.Button,
            binding: "<Mouse>/leftButton"
        );
    }

    void OnEnable()
    {
        clickAction.Enable();
        clickAction.performed += OnClick;
    }

    void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("clicked");
    }
}