using UnityEngine;

public static class GameParameters
{
    public static int CurrentLevel = 1;

    private static float HorizontalDistToHexagon = 0.75f;
    private static float VerticalDistToHexagon = 0.8856f;

    public static Vector3[] DistancesToAdjacentHexagons =
    {
        new Vector3(HorizontalDistToHexagon,VerticalDistToHexagon,0),
        new Vector3(HorizontalDistToHexagon,0,0),
        new Vector3(HorizontalDistToHexagon,-VerticalDistToHexagon,0),
        new Vector3(-HorizontalDistToHexagon,-VerticalDistToHexagon,0),
        new Vector3(-HorizontalDistToHexagon,0,0),
        new Vector3(-HorizontalDistToHexagon,VerticalDistToHexagon,0)
    };
    
}
