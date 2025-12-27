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
    public List<Hexagon> adjacentHexagons = new List<Hexagon>();

    private string hexagonName;
    private Image hexagonImage;

    void Awake()
    {
        HexagonMinigameController = FindObjectOfType<HexagonMinigameController>();
        hexagonImage = GetComponent<Image>();
        hexagonName = gameObject.name;
        Symbol = SymbolType.None;
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
        hexagonImage.sprite = HexagonSelectedSprite;
        isSelected = true;
        HexagonMinigameController.AddToHexChain(this);
        Debug.Log($"Hex {hexagonName} is new end of chain. Neighbors:");
        foreach (Hexagon h in adjacentHexagons)
            Debug.Log(h.GetHexName());
    }

    public void TryToDeselect(Hexagon hex)
    {
        if (hex == HexagonMinigameController.GetEndOfChain())
        {
            hexagonImage.sprite = HexagonNeutralSprite;
            isSelected = false;
            HexagonMinigameController.RemoveFromEndOfHexChain(this);
        }
    }

    public string GetHexName() => hexagonName;

    public void SetRandomSymbol()
    {
        Symbol = (SymbolType)Random.Range(1, System.Enum.GetValues(typeof(SymbolType)).Length);
        SymbolSprite.sprite = SymbolSprites[(int)Symbol];
    }
}
