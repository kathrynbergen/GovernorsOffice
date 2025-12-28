using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexagonMinigameController : MonoBehaviour
{
    public Hexagon[] AllHexes;
    public List<SymbolType> AllowedSymbols;
    
    private Camera camera;
    private Ray ray;
    private InputAction clickAction;

    private List<Hexagon> hexagonChain; // to be used to track the chain of selected hexagons

    private int numberOfSafeSymbols = 2;
    

    void Awake()
    {
        hexagonChain = new List<Hexagon>();
        clickAction = new InputAction(
            name: "Click",
            type: InputActionType.Button,
            binding: "<Mouse>/leftButton"
        );
        if (AllHexes == null || AllHexes.Length == 0)
        {
            AllHexes = FindObjectsOfType<Hexagon>();
        }
    }

    void Start()
    {
        camera = Camera.main;
        GameSetup(); // temp for testing
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

    public void Reset()
    {
        if (hexagonChain != null)
        {
            foreach (Hexagon hex in hexagonChain)
            {
                hex.ResetHex();
                
            }
            hexagonChain.Clear();
        }
        GameSetup();
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
    

    private void GameSetup()
    {
        // Activate all hexes and set random symbols
        foreach (Hexagon hex in AllHexes)
        {
            hex.gameObject.SetActive(true);
            hex.SetRandomSymbol();
        }

        // Assign neighbors manually
        SetupManualNeighbors();

        // Pick starting hex
        Hexagon startHex = null;
        foreach (Hexagon hex in AllHexes)
        {
            if (hex.GetHexName() == "Hexagon 0") 
            {
                startHex = hex;
                break;
            }
        }

        if (startHex != null)
        {
            startHex.HexagonImage.sprite = startHex.HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.Blank];
            startHex.isSelected = true;
            AddToHexChain(startHex);
        }

        PickSafeSymbols();
    }

    private void PickSafeSymbols()
    {
        while (AllowedSymbols.Count < numberOfSafeSymbols) // loops until numberOfSafeSymbols symbols are picked
        {
            SymbolType randomSymbol = (SymbolType)Random.Range(1, System.Enum.GetValues(typeof(SymbolType)).Length);
            if (!AllowedSymbols.Contains(randomSymbol))
            {
                AllowedSymbols.Add(randomSymbol);
                print(randomSymbol+ " added");
            }
        }
    }

    private void SetupManualNeighbors()
{
    // organize hexes into rows
    List<List<Hexagon>> hexRows = new List<List<Hexagon>>();

    // even rows 7, odd rows 6
    int index = 0;
    int[] rowLengths = new int[] {7,6,7,6,7,6,7}; 
    foreach (int length in rowLengths)
    {
        List<Hexagon> row = new List<Hexagon>();
        for (int i = 0; i < length; i++)
        {
            row.Add(GetHexByName("Hexagon " + index));
            index++;
        }
        hexRows.Add(row);
    }

    // assign neighbors for each hex
    for (int r = 0; r < hexRows.Count; r++)
    {
        bool isEvenRow = (r % 2 == 0);
        List<Hexagon> row = hexRows[r];

        for (int c = 0; c < row.Count; c++)
        {
            Hexagon hex = row[c];
            List<Hexagon> neighbors = new List<Hexagon>();

            // Same row neighbors
            if (c > 0) neighbors.Add(row[c - 1]); // left
            if (c < row.Count - 1) neighbors.Add(row[c + 1]); // right

            // Upper row neighbors
            if (r > 0)
            {
                List<Hexagon> upperRow = hexRows[r - 1];
                if (isEvenRow)
                {
                    if (c - 1 >= 0 && c - 1 < upperRow.Count) neighbors.Add(upperRow[c - 1]);
                    if (c < upperRow.Count) neighbors.Add(upperRow[c]);
                }
                else
                {
                    if (c < upperRow.Count) neighbors.Add(upperRow[c]);
                    if (c + 1 < upperRow.Count) neighbors.Add(upperRow[c + 1]);
                }
            }

            // Lower row neighbors
            if (r < hexRows.Count - 1)
            {
                List<Hexagon> lowerRow = hexRows[r + 1];
                if (isEvenRow)
                {
                    if (c - 1 >= 0 && c - 1 < lowerRow.Count) neighbors.Add(lowerRow[c - 1]);
                    if (c < lowerRow.Count) neighbors.Add(lowerRow[c]);
                }
                else
                {
                    if (c < lowerRow.Count) neighbors.Add(lowerRow[c]);
                    if (c + 1 < lowerRow.Count) neighbors.Add(lowerRow[c + 1]);
                }
            }

            hex.adjacentHexagons = neighbors;
        }
    }
}

    private Hexagon GetHexByName(string hexName)
    {
        foreach (Hexagon hex in AllHexes)
        {
            if (hex.GetHexName() == hexName)
                return hex;
        }
        Debug.LogWarning($"Hex {hexName} not found!");
        return null;
    }

    public Hexagon GetEndOfChain()
    {
        if (hexagonChain.Count == 0) return null;
        return hexagonChain[hexagonChain.Count - 1];
    }

    public void AddToHexChain(Hexagon endHexagon)
    {
        hexagonChain.Add(endHexagon);
        if (endHexagon.name == "Hexagon 45")
        {
            GameWin();
        }
    }

    public void RemoveFromEndOfHexChain(Hexagon endHexagon)
    {
        if(hexagonChain.Count > 1) // do not let remove start of chain
            hexagonChain.Remove(endHexagon);
    }

    private void GameWin()
    {
        print("you win!");
    }
    
}
