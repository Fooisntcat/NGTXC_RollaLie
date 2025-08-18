using UnityEngine;
using TiltFive;


public class PlayerTurn : MonoBehaviour
{
    // Singleton instance for easy access
    // get; → Anyone can read the value (e.g., var turn = PlayerTurn.Instance;).
    // private set; → Only the class itself can change the value of Instance. No outside code can assign it.
    public static PlayerTurn Instance { get; private set; }
    // Player-specific data
    public int CurrentPlayerTurn;
    public int PlayerCount = 2;
    public int p1Score = 0;
    public int p2Score = 0;
    public int p1Money = 0;
    public int p2Money = 0;
    public int roundWinner = 0;
    private DiceThrowerScript diceThrower;

    private void Awake()
    {
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();

        if (Instance == null)
        {
            Instance = this;
            CurrentPlayerTurn = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }
    public void NextTurn()
    {
        // Debug.Log("Next Turn Called");
        CurrentPlayerTurn++;
        // Debug.Log("Current Player Turn: " + CurrentPlayerTurn);
        if (CurrentPlayerTurn > PlayerCount)
        {
            // Debug.Log("Current Player Turn: " + CurrentPlayerTurn);
            
            CurrentPlayerTurn = 1;
        }
    }
    public void AddPoint(int points)
    {
        if (CurrentPlayerTurn == 1)
        {
            p1Score += points;
            // Debug.Log("Player 1 Score: " + p1Score);
        }
        else if (CurrentPlayerTurn == 2)
        {
            p2Score += points;
            // Debug.Log("Player 2 Score: " + p2Score);
        }
        if (diceThrower._finishedDiceCount == 2)
        {
            NextTurn();
        }
        if (diceThrower._finishedDiceCount >= 4)
        {
            CheckForWinner();
            ResetScores();
            NextTurn();
        }
        // Debug.Log("finished dice count: " + diceThrower._finishedDiceCount);
    }

    private void CheckForWinner()
    {
        // if (p1Score >= 10)
        if (p1Score > p2Score)
        {
            roundWinner = 1;
            Debug.Log("Player 1 wins!");
        }
        else if (p2Score > p1Score)
        {
            roundWinner = 2;
            Debug.Log("Player 2 wins!");        }
        else
        {
            roundWinner = 0; // It's a tie
            Debug.Log("It's a tie!");
        }
    }
    
    private void ResetScores()
    {
        p1Score = 0;
        p2Score = 0;
        diceThrower._finishedDiceCount = 0;
    }
}
