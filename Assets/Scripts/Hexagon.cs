using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{
    public HexagonMinigameController HexagonMinigameController;
    
    public SymbolType Symbol;
    public Sprite HexagonNeutralSprite;
    public Sprite HexagonSelectedSprite;

    public SpriteRenderer SymbolSprite;
    public Sprite[] SymbolSprites;

    public bool isSelected = false;

    private string hexagonName;
    private Image hexagonImage;
    // store adjacent hexagons in a list of hexagons
    private List<Hexagon> adjacentHexagons;
    
    public void Awake()
    {
        adjacentHexagons = new List<Hexagon>(); 
        Symbol = SymbolType.None;
        
        HexagonMinigameController = FindObjectOfType<HexagonMinigameController>();
        hexagonImage = GetComponent<Image>();
        hexagonName = gameObject.name;
    }

    public void TryToSelect()
    {
        if (isAdjacentToEndOfChain()) 
        {
            Select();
            print("selected successfully");
        }
        else
        {
            print("unable to select - not adjacent to endofchain");
        }
    }

    private bool isAdjacentToEndOfChain()
    {
        Hexagon endOfChain = HexagonMinigameController.GetEndOfChain();
        if (endOfChain == null)
            return false;

        return endOfChain.GetAdjacentHexList().Contains(this);
    }


    public void Select()
    {
        hexagonImage.sprite = HexagonSelectedSprite;
        isSelected = true;
        HexagonMinigameController.AddToHexChain(this);
    }

    public void TryToDeselect(Hexagon hexagon)
    {
        if (IsEndOfChain(hexagon)) 
        {
            hexagonImage.sprite = HexagonNeutralSprite;
            isSelected = false;
            HexagonMinigameController.RemoveFromEndOfHexChain(this);
        }
    }

    public bool IsEndOfChain(Hexagon hexagon)
    {
        print("testing isendofchain");
        if (hexagon.isEqual(HexagonMinigameController.GetEndOfChain()))
        {
            print("true");
            return true;
        }
        return false;
    }
    public void FindAdjacentHexagons()
    {
        adjacentHexagons.Clear();

        float checkRadius = 0.1f; 

        foreach (Vector3 dir in GameParameters.DistancesToAdjacentHexagons)
        {
            Vector3 targetPos = transform.position + dir;
            Debug.DrawLine(transform.position, transform.position + dir, Color.red, 10f);

            // check for colliders at the target position
            Collider[] hitColliders = Physics.OverlapSphere(targetPos, checkRadius);
            foreach (Collider hit in hitColliders)
            {
                Hexagon hex = hit.GetComponent<Hexagon>();
                if (hex != null && hex != this)
                {
                    adjacentHexagons.Add(hex);
                    break; // only add the first hex found in this direction
                }
            }
        }
    }

    public string GetHexName()
    {
        return hexagonName;
    }
    public List<Hexagon> GetAdjacentHexList()
    {
        return adjacentHexagons;
    }
    public void SetRandomSymbol()
    {
        Symbol = (SymbolType)Random.Range(1, System.Enum.GetValues(typeof(SymbolType)).Length); // picks one randomly excluding none
        SetSymbolSprite();
    }

    private void SetSymbolSprite()
    {
        SymbolSprite.sprite = SymbolSprites[(int)Symbol];
    }

    private bool isEqual(Hexagon other)
    {
        return other != null && other == this;
    }

}

