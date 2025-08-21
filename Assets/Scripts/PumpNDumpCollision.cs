using UnityEngine;
using TiltFive;

public class PumpNDumpCollision : MonoBehaviour
{
    private PlayerTurn playerTurn;
    private WandFlashbang wandFlashbang;

    private void Start()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        wandFlashbang = FindFirstObjectByType<WandFlashbang>();
    }

    void Update()
    {
        if (transform.position.y < -10f) // Dice fell off the table
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wandFlashbang.HasDeducted) return;
        
        if (other.CompareTag("p1Hand"))
        {
            Debug.Log("Hand collision detected for Player 1.");
            if (TiltFive.Input.GetTrigger(ControllerIndex.Right, PlayerIndex.One) > 0.8f)
            {
                playerTurn.p1Money -= 15; // Deduct 15 from Player 1's money
                wandFlashbang.HasDeducted = true;
            }
        }
        if (other.CompareTag("p2Hand"))
        {
            Debug.Log("Hand collision detected for Player 2.");
            if (TiltFive.Input.GetTrigger(ControllerIndex.Right, PlayerIndex.Two) > 0.8f)
            {
                playerTurn.p2Money -= 15; // Deduct 15 from Player 2's money
                wandFlashbang.HasDeducted = true;
            }
        }
    }
}

