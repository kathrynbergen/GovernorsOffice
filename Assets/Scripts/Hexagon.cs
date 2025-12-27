using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{
    public HexagonMinigameController HexagonMinigameController;

    public SymbolType Symbol;
    public Sprite HexagonNeutralSprite;
    public Sprite[] HexagonSelectedDirectionalSprites;
    public SpriteRenderer SymbolSprite;
    public Sprite[] SymbolSprites;

    public bool isSelected;
    public List<Hexagon> adjacentHexagons;
    public int HexNumber;
    public Image HexagonImage;

    private string hexagonName;

    void Awake()
    {
        isSelected = false;
        adjacentHexagons = new List<Hexagon>();
        HexagonMinigameController = FindObjectOfType<HexagonMinigameController>();
        HexagonImage = GetComponent<Image>();
        hexagonName = gameObject.name;
        Symbol = SymbolType.None;
        if (!int.TryParse(hexagonName.Substring("Hexagon ".Length), out HexNumber))
        {
            Debug.LogError("Invalid hexagon name: " + hexagonName);
        }
    }

    public void TryToSelect()
    {
        if (IsAdjacentToEndOfChain())
            Select();
        else
            Debug.Log($"Cannot select {hexagonName} - not adjacent to end of chain");
    }

    private bool IsAdjacentToEndOfChain()
    {
        Hexagon end = HexagonMinigameController.GetEndOfChain();
        if (end == null) return false;
        return end.adjacentHexagons.Contains(this);
    }

    public void Select()
    {
        SetHexagonSpriteBasedOnDirection();
        isSelected = true;
        HexagonMinigameController.AddToHexChain(this);
    }

    private void SetHexagonSpriteBasedOnDirection()
    {
        // CURRENT SPRITE SHOULD BE BLANK
        // PREVIOUS END OF CHAIN SHOULD BE UPDATED WITH THE DIRECTION
        HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.Blank];
        /*
        if (HexNumber == 0) ; //2
        else if (HexNumber == 6) ;//2
        else if (HexNumber == 39) ;//2
        else if (HexNumber == 45) ;//2
        else if (HexNumber % 13 == 0) ; //3 (TR, R, BR)
        else if (HexNumber % 13 == 7) ; //5 (all but left)
        else if (HexNumber % 13 == 12) ;//5 (all but right)
        else if (HexNumber % 13 == 6) ; //3 (TL, L, BL)
        else
        {*/
            switch (HexNumber - HexagonMinigameController.GetEndOfChain().HexNumber)
            {
                case 7: // bottom right
                    print("bottom right");
                    HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.DownRight];
                    break;
                case 6: // bottom left
                    print("bottom left");
                    HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.DownLeft];
                    break;
                case 1: // right
                    print("right");
                    HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.Right];
                    break;
                case -1: // left
                    print("left");
                    HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.Left];
                    break;
                case -6: // top right
                    print("top right");
                    HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.UpRight];
                    break;
                case -7: // top left
                    HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.UpLeft];
                    print("top left");
                    break;
                default:
                    print("uh oh- "+(HexNumber - HexagonMinigameController.GetEndOfChain().HexNumber));
                    break;
            }
        //}
        

    }

    public void TryToDeselect(Hexagon hex)
    {
        if (hex == HexagonMinigameController.GetEndOfChain())
        {
            Deselect();
        }
    }

    private void Deselect()
    {
        HexagonImage.sprite = HexagonNeutralSprite;
        isSelected = false;
        HexagonMinigameController.RemoveFromEndOfHexChain(this);
        HexagonMinigameController.GetEndOfChain().HexagonImage.sprite = HexagonSelectedDirectionalSprites[(int)HexagonDirectionType.Blank];
    }

    public string GetHexName() => hexagonName;

    public void SetRandomSymbol()
    {
        Symbol = (SymbolType)Random.Range(1, System.Enum.GetValues(typeof(SymbolType)).Length);
        SymbolSprite.sprite = SymbolSprites[(int)Symbol];
    }
}
