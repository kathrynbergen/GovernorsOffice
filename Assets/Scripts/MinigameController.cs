using UnityEngine;

public class MinigameController : MonoBehaviour
{
    public Canvas CanvasMinigame;
    
    public void StartMinigame()
    {
        print("Start minigame");
        // if secretary (who gives you sanity pills) end the level immediately
        // check what kind of NPC it is and run respective minigame accordingly
        //CanvasMinigame.gameObject.SetActive(true);
    }
}
