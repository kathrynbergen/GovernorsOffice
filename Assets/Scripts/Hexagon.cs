using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{
    public SymbolType Symbol;
    public Sprite HexagonNeutralSprite;
    public Sprite HexagonSelectedSprite;

    public bool isSelected = true;
    
    private Image HexagonImage;
    // store adjacent hexagons in a list of hexagons
    private List<Hexagon> adjacentHexagons;

    void Start()
    {
        HexagonImage = gameObject.GetComponent<Image>();
    }
    public void Awake()
    {
        adjacentHexagons = new List<Hexagon>(); // add to list as new hexagons are added next to this one
        Symbol = SymbolType.None;
    }

    public void Selected()
    {
        if (IsEndOfChain())
        {
            HexagonImage.sprite = HexagonSelectedSprite;
            print("Selected");
        }
    }
    public void Deselected()
    {
        if (IsEndOfChain())
        {
            HexagonImage.sprite = HexagonNeutralSprite;
        }
    }

    public bool IsEndOfChain()
    {
        // FIX ME
        return true;
    }
    public void FindAdjacentHexagons()
    {
        adjacentHexagons.Clear();

        float checkRadius = 0.1f; 

        foreach (Vector3 dir in GameParameters.DistancesToAdjacentHexagons)
        {
            Vector3 targetPos = transform.position + dir;
            Debug.DrawLine(transform.position, transform.position + dir, Color.red, 5f);

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
}
