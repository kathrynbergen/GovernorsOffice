using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexagonMinigameController : MonoBehaviour
{
    private Camera camera;
    private Ray ray;
    private InputAction clickAction;

    private List<Hexagon> hexagonChain; // to be used to track the chain of selected hexagons

    void Awake()
    {
        hexagonChain = new List<Hexagon>();
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
            if (hit.collider.CompareTag("Hexagon"))
            {
                Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
                if (hexagon != null)
                {
                    if (hexagon.isSelected)
                    {
                        print("trytodeselect");
                        hexagon.TryToDeselect(hexagon);
                    }
                    else
                    {
                        hexagon.TryToSelect();
                    }
                }
            } 
        }
    }

    public void GameSetup()
    {
        Hexagon[] hexagons = Object.FindObjectsByType<Hexagon>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        Hexagon startHex = null;
        
        foreach (Hexagon hex in hexagons)
        {
            
            hex.gameObject.SetActive(true);
            hex.FindAdjacentHexagons();
            hex.SetRandomSymbol();
            
            if (hex.GetHexName()=="Hexagon 0") // temp - sets top left hex to be start always - change to random later (make sure its safe to add from key)
            {
                startHex = hex;
            }
        }
        if (startHex != null)
        {
            startHex.Select();
            AddToHexChain(startHex);
        }
        // create NxN grid where N = level + 4
        // activate hexagons that are on the grid based on N
        
        // create a puzzle solution based on the "safe" tiles
        
        
    }

    public Hexagon GetEndOfChain()
    {
        return hexagonChain[hexagonChain.Count - 1];
    }

    public void AddToHexChain(Hexagon endHexagon)
    {
        hexagonChain.Add(endHexagon);
        print(hexagonChain);
    }

    public void RemoveFromEndOfHexChain(Hexagon endHexagon)
    {
        if(hexagonChain.Count > 1)
            hexagonChain.Remove(endHexagon);
        print(hexagonChain);
    }
    
}
