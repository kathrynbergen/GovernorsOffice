using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public MinigameController MinigameController;
    
    private Camera camera;
    private Ray ray;
    private InputAction clickAction;

    void Awake()
    {
        clickAction = new InputAction(
            name: "Click",
            type: InputActionType.Button,
            binding: "<Mouse>/leftButton"
        );
    }

    void Start()
    {
        camera = GetComponent<Camera>();
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
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        ray = camera.ScreenPointToRay(mousePosition);
        CheckForColliders();
    }

    private void CheckForColliders()
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name + " was clicked");
            if (hit.collider.gameObject.tag == "NPC")
            {
                MinigameController.StartMinigame();
            } 
        }
    }
}