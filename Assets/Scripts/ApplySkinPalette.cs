using UnityEngine;

public class ApplySkinPalette : MonoBehaviour
{
    public Renderer player1Hand;
    public Renderer player2Hand;

    void Start()
    {
        // Load each player's saved skin
        player1Hand.material.color = SkinPalette.LoadColor(1);
        player2Hand.material.color = SkinPalette.LoadColor(2);
    }
}
