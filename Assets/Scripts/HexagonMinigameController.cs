using UnityEngine;
using UnityEngine.InputSystem;

public class HexagonMinigameController : MonoBehaviour
{
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
        camera = Camera.main;
        GameSetup(); // temp for testing- method will be called from MinigameController eventually
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
            if (hit.collider.CompareTag("Hexagon"))
            {
                Hexagon hexagon = hit.collider.GetComponent<Hexagon>();

                if (hexagon != null)
                {
                    if (hexagon.isSelected)
                    {
                        hexagon.Deselected();
                    }
                    else
                    {
                        hexagon.Selected();
                    }
                }
            } 
        }
    }

    public void GameSetup()
    {
        print("setting up...");
        Hexagon[] hexagons = Object.FindObjectsByType<Hexagon>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (Hexagon hex in hexagons)
        {
            hex.gameObject.SetActive(true);
            hex.FindAdjacentHexagons();
        }
        // create NxN grid where N = level + 4
        // activate hexagons that are on the grid based on N
        
        // create a puzzle solution based on the "safe" tiles
        // fill in remaining empty tiles
    }
    
}
