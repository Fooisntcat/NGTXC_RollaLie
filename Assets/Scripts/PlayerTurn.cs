#region Old Script (Logic Error)
/*
using UnityEngine;
using TiltFive;
using TimerCountdown;

public class PlayerTurn : MonoBehaviour
{
    // Singleton instance for easy access
    // get; → Anyone can read the value (e.g., var turn = PlayerTurn.Instance;).
    // private set; → Only the class itself can change the value of Instance. No outside code can assign it.
    public static PlayerTurn Instance { get; private set; }
    // Player-specific data
    public int CurrentPlayerTurn;

    public int PlayerCount = 2;
    public int p1Score;
    public int p2Score;
    public int p1Money;
    public int p2Money;
    public int p1MoneyDeduct;
    public int p2MoneyDeduct;
    public int MoneyPool;
    public int roundWinner;
    public int roundsPlayed;
    public int gameWinner;
    public bool p1PumpNDump;
    public bool p2PumpNDump;
    public int PumpNDumpRound;

    // Previous Round Data
    public int lastRoundWinner;
    public int lastP1Money;
    public int lastP2Money;

    // Script References
    private DiceThrowerScript diceThrower;
    private TimerCountdown.Timer timerCountdown;

    private void Awake()
    {
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        timerCountdown = FindFirstObjectByType<TimerCountdown.Timer>();

        if (Instance == null)
        {
            Instance = this;
            CurrentPlayerTurn = 1;
        }
        else
        {
            Destroy(gameObject);
        }
        if (roundsPlayed < 0)
        {
            roundsPlayed = 0;
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

        // Player 1
        if (diceThrower._finishedDiceCount == 2)
        {
            if (PumpNDumpRound == roundsPlayed && p1PumpNDump)  // PumpNDump in the current round
            {
                p1Score *= 2;
            }
            else if (PumpNDumpRound != roundsPlayed && p1PumpNDump)  // PumpNDump in a previous round (side effects the round now)
            {
                p1PumpNDump = false;
                p1MoneyDeduct *= 3;
            }
            lastP1Money = p1Money;
            p1Money -= p1MoneyDeduct;
            MoneyPool += p1MoneyDeduct;
            p1MoneyDeduct = roundsPlayed * 3; // Resets back the deduction (from PumpNDump side effects)
            NextTurn();
        }
        // Player 2
        if (diceThrower._finishedDiceCount >= 4)
        {
            if (PumpNDumpRound == roundsPlayed && p2PumpNDump)
            {
                p2Score *= 2;
            }
            else if (PumpNDumpRound != roundsPlayed && p2PumpNDump)
            {
                p2PumpNDump = false;
                p2MoneyDeduct *= 3;
            }
            lastP2Money = p2Money;
            p2Money -= p2MoneyDeduct;
            MoneyPool += p2MoneyDeduct;
            p2MoneyDeduct = roundsPlayed*3; // Resets back the deduction (from PumpNDump side effects)
            lastRoundWinner = roundWinner;
            CheckForWinner();
            CheckForLoser();
            ResetScores();
            NextTurn();
        }
        // Debug.Log("finished dice count: " + diceThrower._finishedDiceCount);
    }

    public void PumpNDump(int player, int currentPlayerTurn)
    {
        PumpNDumpRound = roundsPlayed;
        if (player == 1)
        {
            p1PumpNDump = true;
        }
        else if (player == 2)
        {
            p2PumpNDump = true;
        }
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
            roundsPlayed++;
        }
    }

    private void CheckForWinner()
    {
        // if (p1Score >= 10)
        if (p1Score > p2Score)
        {
            roundWinner = 1;
            p1Money += MoneyPool;
            // Debug.Log("Player 1 wins!");
        }
        else if (p2Score > p1Score)
        {
            roundWinner = 2;
            p2Money += MoneyPool;
            // Debug.Log("Player 2 wins!");
        }
        else
        {
            roundWinner = 0; // It's a tie
            p1Money += MoneyPool / 2;
            p2Money += MoneyPool / 2;
            // Debug.Log("It's a tie!");
        }
    }

    private void CheckForLoser()
    {
        if (p1Money <= 0)
        {
            gameWinner = 2;
        }
        else if (p2Money <= 0)
        {
            gameWinner = 1;
        }
    }
    private void ResetScores()
    {
        p1Score = 0;
        p2Score = 0;
        p1MoneyDeduct += 3;
        p2MoneyDeduct += 3;
        MoneyPool = 0;
        diceThrower._finishedDiceCount = 0;
        // diceThrower.ResetRound();
    }
    public void RerollDice()
    {
        p1Money = lastP1Money;
        p2Money = lastP2Money;
        roundWinner = lastRoundWinner;
        roundsPlayed--;
        p1MoneyDeduct -= 3;
        p2MoneyDeduct -= 3;
        ResetScores();
    }
}
*/
#endregion
#region Fixed Logic (still some bugs)
/*
using UnityEngine;
using TiltFive;
using TimerCountdown;

public class PlayerTurn : MonoBehaviour
{
    public static PlayerTurn Instance { get; private set; }

    public int CurrentPlayerTurn;
    public int PlayerCount = 2;

    // Scores / money
    public int p1Score;
    public int p2Score;
    public int p1Money;
    public int p2Money;

    // incremental deduction that grows each round (you used +=3 in ResetScores)
    public int p1MoneyDeduct = 3;
    public int p2MoneyDeduct = 3;

    public int MoneyPool;
    public int roundWinner;
    public int roundsPlayed;
    public int gameWinner;

    // Pump & tracking: store the round number the pump was placed (or -1 if none)
    public int p1PumpPlacedRound = -1;
    public int p2PumpPlacedRound = -1;

    // Previous Round Data (for reroll)
    public int lastRoundWinner;
    public int lastP1Money;
    public int lastP2Money;

    private DiceThrowerScript diceThrower;
    private TimerCountdown.Timer timerCountdown;

    private void Awake()
    {
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        timerCountdown = FindFirstObjectByType<TimerCountdown.Timer>();

        if (Instance == null)
        {
            Instance = this;
            CurrentPlayerTurn = 1;
        }
        else
        {
            Destroy(gameObject);
        }

        if (roundsPlayed < 0) roundsPlayed = 0;
    }

    // Call AddPoint for each player's completed roll (points = sum of that player's dice this turn)
    public void AddPoint(int points)
    {
        // Apply PumpNDump doubling if this player placed pump in this round
        int appliedPoints = points;
        if (CurrentPlayerTurn == 1)
        {
            if (p1PumpPlacedRound == roundsPlayed) // pump placed this round
            {
                appliedPoints = points * 2;
            }
            p1Score += appliedPoints;
        }
        else if (CurrentPlayerTurn == 2)
        {
            if (p2PumpPlacedRound == roundsPlayed)
            {
                appliedPoints = points * 2;
            }
            p2Score += appliedPoints;
        }

        // When two dice finished -> player 1 done. When 4 dice finished -> player 2 done (both).
        // IMPORTANT: do not nest these checks (player2 must be processed separately).
        if (diceThrower._finishedDiceCount == 2 && CurrentPlayerTurn == 1)
        {
            // Save last money before deduction
            lastP1Money = p1Money;
            // no deduction yet — we deduct after round ends so both players are compared
            NextTurn(); // move to player 2
        }
        else if (diceThrower._finishedDiceCount >= 4 && CurrentPlayerTurn == 2)
        {
            // Both players have rolled -> resolve round
            // Save previous-money snapshots (so RerollDice can restore)
            lastP1Money = p1Money;
            lastP2Money = p2Money;

            // Determine winner and distribute MoneyPool
            CheckForWinner();

            // Apply deduction to both players now that round outcome is known.
            // If a player had placed PumpNDump in the previous round (roundsPlayed - 1) then their deduction
            // for THIS loss should be tripled (once). After applying, clear their placed-round marker.
            ApplyDeductionsAndPumpPenalties();

            // Save lastRoundWinner for reroll
            lastRoundWinner = roundWinner;

            CheckForLoser();
            ResetScores();

            // End of full turn: advance to next player (wraps back to player 1) and increments roundsPlayed inside NextTurn
            NextTurn();
        }
    }

    // Place PumpNDump for a player (call BEFORE they roll). We record the round number (roundsPlayed).
    public void PumpNDump(int player)
    {
        if (player == 1)
        {
            p1PumpPlacedRound = roundsPlayed; // mark that pump was placed this round
        }
        else if (player == 2)
        {
            p2PumpPlacedRound = roundsPlayed;
        }
    }

    public void NextTurn()
    {
        CurrentPlayerTurn++;
        if (CurrentPlayerTurn > PlayerCount)
        {
            CurrentPlayerTurn = 1;
            roundsPlayed++; // completed a full round
        }
    }

    private void CheckForWinner()
    {
        if (p1Score > p2Score)
        {
            roundWinner = 1;
            p1Money += MoneyPool;
        }
        else if (p2Score > p1Score)
        {
            roundWinner = 2;
            p2Money += MoneyPool;
        }
        else
        {
            roundWinner = 0;
            p1Money += MoneyPool / 2;
            p2Money += MoneyPool / 2;
        }
    }

    private void ApplyDeductionsAndPumpPenalties()
    {
        // Determine whether p1 or p2 lost this round (based on roundWinner),
        // and whether they had placed pump last round (placedRound == roundsPlayed - 1).
        // If they did, triple that player's deduction for this loss only, then clear their placedRound marker.
        bool p1WasLoser = (roundWinner == 2);
        bool p2WasLoser = (roundWinner == 1);

        // Player 1 deduction
        int p1ThisDeduct = p1MoneyDeduct;
        if (p1WasLoser && p1PumpPlacedRound == (roundsPlayed - 1))
        {
            p1ThisDeduct *= 3; // triple deduction once
            p1PumpPlacedRound = -1; // clear pump effect after it fires for next-round loss
        }
        // Apply deduction
        p1Money -= p1ThisDeduct;
        MoneyPool += p1ThisDeduct;

        // Player 2 deduction
        int p2ThisDeduct = p2MoneyDeduct;
        if (p2WasLoser && p2PumpPlacedRound == (roundsPlayed - 1))
        {
            p2ThisDeduct *= 3;
            p2PumpPlacedRound = -1;
        }
        p2Money -= p2ThisDeduct;
        MoneyPool += p2ThisDeduct;
    }

    private void CheckForLoser()
    {
        if (p1Money <= 0) gameWinner = 2;
        else if (p2Money <= 0) gameWinner = 1;
    }

    private void ResetScores()
    {
        p1Score = 0;
        p2Score = 0;
        // Increase the regular deduction by 3 each completed round (you had this in original code)
        p1MoneyDeduct += 3;
        p2MoneyDeduct += 3;
        MoneyPool = 0;
        diceThrower._finishedDiceCount = 0;
    }

    public void RerollDice()
    {
        // restore previous money state and undo last increment to rounds/deductions
        p1Money = lastP1Money;
        p2Money = lastP2Money;
        roundWinner = lastRoundWinner;

        if (roundsPlayed > 0) roundsPlayed--;
        p1MoneyDeduct = Mathf.Max(3, p1MoneyDeduct - 3); // don't let it drop below base
        p2MoneyDeduct = Mathf.Max(3, p2MoneyDeduct - 3);

        ResetScores();
    }
}
*/
#endregion

#region Maybe Fixed
/*
using UnityEngine;
using TiltFive;
using TimerCountdown;
// using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class PlayerTurn : MonoBehaviour
{
    #region Variables
    public static PlayerTurn Instance { get; private set; }

    public int CurrentPlayerTurn = 1;
    public int PlayerCount = 2;

    // Scores & money
    public int p1Score;
    public int p2Score;
    public int p1Money = 100;
    public int p2Money = 100;

    // Per-player base deduction that increases each round (base 3)
    public int p1MoneyDeduct = 3;
    public int p2MoneyDeduct = 3;

    public int MoneyPool;
    public int roundWinner;
    public int roundsPlayed;
    public int gameWinner;
    private bool isGameOverTriggered = false;

    // Pump: store the round number when pump was placed (-1 = none)
    public int p1PumpPlacedRound = -1;
    public int p2PumpPlacedRound = -1;

    // Snapshots for reroll
    public int lastRoundWinner;
    public int lastP1Money;
    public int lastP2Money;
    private bool hasMoneySnapshot = false;

    // refs
    private DiceThrowerScript diceThrower;
    private TimerCountdown.Timer timerCountdown;
    #endregion

    private void Awake()
    {   
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        timerCountdown = FindFirstObjectByType<TimerCountdown.Timer>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (roundsPlayed < 0) roundsPlayed = 0;

        // initialize last-money snapshot so RerollDice won't zero-out on first round
        lastP1Money = p1Money;
        lastP2Money = p2Money;
        hasMoneySnapshot = true;
    }

    // Call AddPoint(points) once per player's completed roll (points = sum of that player's dice this turn)
    public void AddPoint(int points)
    {
        // defensive: ensure diceThrower exists
        if (diceThrower == null)
        {
            Debug.LogWarning("AddPoint called but diceThrower == null");
            return;
        }

        // Apply pump doubling only to this roll's points (not entire accumulated score)
        int appliedPoints = points;
        if (CurrentPlayerTurn == 1 && p1PumpPlacedRound == roundsPlayed)
            appliedPoints = points * 2;
        else if (CurrentPlayerTurn == 2 && p2PumpPlacedRound == roundsPlayed)
            appliedPoints = points * 2;

        // Add to correct player's score
        if (CurrentPlayerTurn == 1)
            p1Score += appliedPoints;
        else if (CurrentPlayerTurn == 2)
            p2Score += appliedPoints;

        Debug.Log($"Player {CurrentPlayerTurn} rolled {points} (applied {appliedPoints}). Scores: P1={p1Score} P2={p2Score} finishedDiceCount={diceThrower._finishedDiceCount}");

        // If player 1 finished their two dice, advance to player 2 (but only once)
        if (CurrentPlayerTurn == 1 && diceThrower._finishedDiceCount >= 2)
        {
            // move to player 2 (don't resolve round yet)
            NextTurn();
            return;
        }

        // If both players have rolled (global finishedDiceCount >= 4) AND we are on player 2's turn,
        // resolve the round here.
        if (CurrentPlayerTurn == 2 && diceThrower._finishedDiceCount >= 4)
        {
            ResolveRound();
        }
    }

    // Called when a player places PumpNDump BEFORE they roll
    public void PumpNDump(int player)
    {
        if (player == 1)
        {
            p1PumpPlacedRound = roundsPlayed;
            Debug.Log($"P1 Pump placed in round {roundsPlayed}");
        }
        else if (player == 2)
        {
            p2PumpPlacedRound = roundsPlayed;
            Debug.Log($"P2 Pump placed in round {roundsPlayed}");
        }
    }

    // Advance the turn; if wrapped, increment roundsPlayed.
    public void NextTurn()
    {
        CurrentPlayerTurn++;
        if (CurrentPlayerTurn > PlayerCount)
        {
            CurrentPlayerTurn = 1;
            // roundsPlayed++;
            Debug.Log($"New full round started. roundsPlayed = {roundsPlayed}");
        }
    }

    // Full round resolution (called when both players rolled)
    private void ResolveRound()
    {
        Debug.Log("Resolving round: computing winner/deductions...");

        // Snapshot money for Reroll safety
        lastP1Money = p1Money;
        lastP2Money = p2Money;
        hasMoneySnapshot = true;

        // 1) Determine winner (based on scores) but DON'T give MoneyPool yet.
        if (p1Score > p2Score)
        {
            roundWinner = 1;
        }
        else if (p2Score > p1Score)
        {
            roundWinner = 2;
        }
        else
        {
            roundWinner = 0; // tie
        }

        // Determine losers (needed for pump triple rule)
        bool p1WasLoser = (roundWinner == 2);
        bool p2WasLoser = (roundWinner == 1);

        // 2) Calculate deductions for each player (including pump triple-if-they-lose)
        int p1ThisDeduct = p1MoneyDeduct;
        if (p1WasLoser && p1PumpPlacedRound == (roundsPlayed - 1))
        {
            p1ThisDeduct *= 3; // triple one time because they had placed pump in previous round and lost this round
            p1PumpPlacedRound = -1; // clear pump effect after it fires
            Debug.Log("P1 pump penalty (triple) applied.");
        }

        int p2ThisDeduct = p2MoneyDeduct;
        if (p2WasLoser && p2PumpPlacedRound == (roundsPlayed - 1))
        {
            p2ThisDeduct *= 3;
            p2PumpPlacedRound = -1;
            Debug.Log("P2 pump penalty (triple) applied.");
        }

        // 3) Apply deductions to players and add to MoneyPool
        p1Money -= p1ThisDeduct;
        p2Money -= p2ThisDeduct;
        MoneyPool = p1ThisDeduct + p2ThisDeduct;

        Debug.Log($"Deductions: P1 -{p1ThisDeduct}, P2 -{p2ThisDeduct}. MoneyPool now {MoneyPool}. Money after deduction: P1={p1Money} P2={p2Money}");

        // 4) Award MoneyPool to winner (or split on tie)
        if (roundWinner == 1)
        {
            p1Money += MoneyPool;
            Debug.Log($"Player 1 wins round and receives {MoneyPool}. P1 money now {p1Money}.");
        }
        else if (roundWinner == 2)
        {
            p2Money += MoneyPool;
            Debug.Log($"Player 2 wins round and receives {MoneyPool}. P2 money now {p2Money}.");
        }
        else // tie
        {
            p1Money += MoneyPool / 2;
            p2Money += MoneyPool / 2;
            Debug.Log($"Round tie, each receives {MoneyPool/2}.");
        }

        // 5) Save lastRoundWinner and check game-over
        lastRoundWinner = roundWinner;
        CheckForLoser();

        // 6) Reset round scores and increment the base deduction per your original plan.
        ResetScores();

        // Only now increment roundsPlayed, AFTER deductions/penalties are done
        roundsPlayed++;
        Debug.Log($"Full round completed. roundsPlayed = {roundsPlayed}");

        // After full-round resolution, move to next turn (this will wrap and increment roundsPlayed)
        NextTurn();
    }

    
        private void CheckForLoser()
        {
            if (p1Money <= 0)
            {
                gameWinner = 2;
                Debug.Log("Player 2 wins the game!");
                DontDestroyOnLoad(gameObject); // <-- Keep alive between scenes
                SceneManager.LoadScene("GameOver"); // Load game over scene or similar
            }
            else if (p2Money <= 0)
            {
                gameWinner = 1;
                Debug.Log("Player 1 wins the game!");
                DontDestroyOnLoad(gameObject); // <-- Keep alive between scenes
                SceneManager.LoadScene("GameOver"); // Load game over scene or similar
            }
        }
        
        private void CheckForLoser()
    {
        if (isGameOverTriggered) return; // guard: only trigger once

        // handle both-lose case explicitly
        bool p1Dead = p1Money <= 0;
        bool p2Dead = p2Money <= 0;

        if (p1Dead && p2Dead)
        {
            isGameOverTriggered = true;
            gameWinner = 0; // 0 = draw / special
            Debug.Log("Both players have <= 0 money — draw condition.");
            // optionally set DontDestroyOnLoad(gameObject); if you need it in GameOver scene
            SceneManager.LoadScene("GameOver");
            return;
        }
        else if (p1Dead)
        {
            isGameOverTriggered = true;
            gameWinner = 2;
            Debug.Log("Player 2 wins the game!");
            // optionally call DontDestroyOnLoad(gameObject); // if needed
            SceneManager.LoadScene("GameOver");
            return;
        }
        else if (p2Dead)
        {
            isGameOverTriggered = true;
            gameWinner = 1;
            Debug.Log("Player 1 wins the game!");
            // optionally call DontDestroyOnLoad(gameObject); // if needed
            SceneManager.LoadScene("GameOver");
            return;
        }
        // otherwise no loser yet
    }

    private void ResetScores()
    {
        p1Score = 0;
        p2Score = 0;

        // increase base deduction by 3 every completed round
        p1MoneyDeduct += 3;
        p2MoneyDeduct += 3;

        // reset temporary pool & dice count
        MoneyPool = 0;
        if (diceThrower != null)
            diceThrower._finishedDiceCount = 0;

        Debug.Log($"Reset scores. p1MoneyDeduct={p1MoneyDeduct} p2MoneyDeduct={p2MoneyDeduct}");
    }

    // Restore last snapshot. Only allow if we actually have a snapshot (avoid zeroing out by accident).
    public void RerollDice()
    {
        if (!hasMoneySnapshot)
        {
            Debug.LogWarning("RerollDice called but no valid snapshot exists. Ignoring.");
            return;
        }

        p1Money = lastP1Money;
        p2Money = lastP2Money;
        roundWinner = lastRoundWinner;

        // reverse the last per-round increment of deduction (safeguarded)
        if (roundsPlayed > 0)
        {
            roundsPlayed--;
            p1MoneyDeduct = Mathf.Max(3, p1MoneyDeduct - 3);
            p2MoneyDeduct = Mathf.Max(3, p2MoneyDeduct - 3);
        }

        ResetScores();
        Debug.Log("RerollDice: restored previous snapshot and reset scores.");
    }
}
*/
#endregion

#region Maybe Fixed (with centralized money changes)

using UnityEngine;
using TiltFive;
using TimerCountdown;
// using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class PlayerTurn : MonoBehaviour
{
    #region Variables

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip player1Won;
    [SerializeField] private AudioClip player2Won;
    [SerializeField] private AudioClip gameTied;
    private AudioClip activeClip;

    [Header("Player Turn")]
    public static PlayerTurn Instance { get; private set; }

    public int CurrentPlayerTurn = 1;
    public int PlayerCount = 2;

    // Scores
    public int p1Score = 0;
    public int p2Score = 0;

    // Backing fields for money (use ChangeMoney to mutate)
    [Header("Player Money")]
    [SerializeField] private int _p1Money = 100;
    [SerializeField] private int _p2Money = 100;
    [SerializeField] private MoneyStack player1Stack;
    [SerializeField] private MoneyStack player2Stack;

    // Public read-only accessors so other classes can read values
    public int P1Money => _p1Money;
    public int P2Money => _p2Money;

    // Per-player base deduction that increases each round (base 3)
    public int p1MoneyDeduct = 3;
    public int p2MoneyDeduct = 3;

    public int MoneyPool = 0;
    public int roundWinner = 0;
    public int roundsPlayed = 0;
    public int gameWinner = 0;
    private bool isGameOverTriggered = false;

    // Pump: store the round number when pump was placed (-1 = none)
    public int p1PumpPlacedRound = -1;
    public int p2PumpPlacedRound = -1;
    

    // Snapshots for reroll
    public int lastRoundWinner = 0;
    public int lastP1Money = 0;
    public int lastP2Money = 0;
    private bool hasMoneySnapshot = false;

    // refs
    private DiceThrowerScript diceThrower;
    private TimerCountdown.Timer timerCountdown;
    private TextController textController;
    private MoneyStack moneyStack;
    private UI_PlayerStatsOnTable UI_PlayerStatsOnTable;
    private PlayerRoundWinFlash playerRoundWinFlash;
    private PlayerHandUI playerHandUI;

    // static vars for next scene (gameOver)
    public static int GameWinner { get; private set; }

    // Internal suppression flag to avoid running CheckForLoser while restoring snapshots
    private bool suppressCheckForLoser = false;
    #endregion

    private void Awake()
    {
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        timerCountdown = FindFirstObjectByType<TimerCountdown.Timer>();
        textController = FindFirstObjectByType<TextController>();
        moneyStack = FindFirstObjectByType<MoneyStack>();
        UI_PlayerStatsOnTable = FindFirstObjectByType<UI_PlayerStatsOnTable>();
        playerRoundWinFlash = FindFirstObjectByType<PlayerRoundWinFlash>();
        playerHandUI = FindFirstObjectByType<PlayerHandUI>();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (roundsPlayed < 0) roundsPlayed = 0;

        // initialize last-money snapshot so RerollDice won't zero-out on first round
        lastP1Money = _p1Money;
        lastP2Money = _p2Money;
        hasMoneySnapshot = true;
        
        // ResetScores(); // ensure initial state is clean
    }

    void Start()
    {
        player1Stack.UpdateChips(_p1Money);
        player2Stack.UpdateChips(_p2Money);
    } 

    // ---------------------------
    // Centralized money API
    // ---------------------------
    /// <summary>
    /// Change a player's money by delta (positive or negative).
    /// Always call this instead of editing _p1Money/_p2Money directly.
    /// </summary>
    public void ChangeMoney(int player, int delta)
    {
        if (isGameOverTriggered) return; // ignore changes after game over

        if (player == 1)
        {
            _p1Money += delta;
            player1Stack.UpdateChips(delta);
            playerHandUI.HandUIUpdate(1, delta);
            // Debug.Log($"ChangeMoney: P1 {(delta >= 0 ? "+" : "")}{delta} => {_p1Money}");
        }
        else if (player == 2)
        {
            _p2Money += delta;
            player2Stack.UpdateChips(delta);
            playerHandUI.HandUIUpdate(2, delta);
            // Debug.Log($"ChangeMoney: P2 {(delta >= 0 ? "+" : "")}{delta} => {_p2Money}");
        }
        else
        {
            Debug.LogWarning("ChangeMoney called with invalid player id: " + player);
            return;
        }

        // Check for loser immediate after money change
        CheckForLoser();
    }

    /// <summary>
    /// Directly set money for a player (internal use). Optionally suppress CheckForLoser.
    /// </summary>
    private void SetMoneyDirect(int player, int value, bool suppressCheck = false)
    {
        bool prevSuppress = suppressCheckForLoser;
        suppressCheckForLoser = suppressCheckForLoser || suppressCheck;

        if (player == 1)
        {
            _p1Money = value;
            // Debug.Log($"SetMoneyDirect: P1 => {_p1Money}");
        }
        else if (player == 2)
        {
            _p2Money = value;
            // Debug.Log($"SetMoneyDirect: P2 => {_p2Money}");
        }

        // restore previous suppression state
        suppressCheckForLoser = prevSuppress;
        
        if (!suppressCheckForLoser)
            CheckForLoser();
    }

    // ---------------------------
    // Game logic
    // ---------------------------
    // Call AddPoint(points) once per player's completed roll (points = sum of that player's dice this turn)
    public void AddPoint(int points)
    {
        // defensive: ensure diceThrower exists
        if (diceThrower == null)
        {
            Debug.LogWarning("AddPoint called but diceThrower == null");
            return;
        }

        // Apply pump doubling only to this roll's points (not entire accumulated score)
        int appliedPoints = points;
        if (CurrentPlayerTurn == 1 && p1PumpPlacedRound == roundsPlayed)
            appliedPoints = points * 2;
        else if (CurrentPlayerTurn == 2 && p2PumpPlacedRound == roundsPlayed)
            appliedPoints = points * 2;

        // Add to correct player's score
        if (CurrentPlayerTurn == 1)
            p1Score += appliedPoints;
        else if (CurrentPlayerTurn == 2)
            p2Score += appliedPoints;

        // Debug.Log($"Player {CurrentPlayerTurn} rolled {points} (applied {appliedPoints}). Scores: P1={p1Score} P2={p2Score} finishedDiceCount={diceThrower._finishedDiceCount}");

        // If player 1 finished their two dice, advance to player 2 (but only once)
        if (CurrentPlayerTurn == 1 && diceThrower._finishedDiceCount >= 2)
        {
            // move to player 2 (don't resolve round yet)
            NextTurn();
            return;
        }

        // If both players have rolled (global finishedDiceCount >= 4) AND we are on player 2's turn,
        // resolve the round here.
        if (CurrentPlayerTurn == 2 && diceThrower._finishedDiceCount >= 4)
        {
            ResolveRound();
        }
    }

    // Called when a player places PumpNDump BEFORE they roll
    public void PumpNDump(int player)
    {
        textController.ShowText("PumpNDumpActivated", player);
        if (player == 1)
        {
            p1PumpPlacedRound = roundsPlayed;
            // Debug.Log($"P1 Pump placed in round {roundsPlayed}");
        }
        else if (player == 2)
        {
            p2PumpPlacedRound = roundsPlayed;
            // Debug.Log($"P2 Pump placed in round {roundsPlayed}");
        }
    }

    // Advance the turn; if wrapped, increment roundsPlayed.
    public void NextTurn()
    {
        CurrentPlayerTurn++;
        if (CurrentPlayerTurn > PlayerCount)
        {
            CurrentPlayerTurn = 1;
            // roundsPlayed++;
            // Debug.Log($"New full round started. roundsPlayed = {roundsPlayed}");
        }
        UI_PlayerStatsOnTable.PlayerTurnFinished(CurrentPlayerTurn);
    }

    // Full round resolution (called when both players rolled)
    private void ResolveRound()
    {
        // Debug.Log("Resolving round: computing winner/deductions...");

        // Snapshot money for Reroll safety (use current backing fields)
        lastP1Money = _p1Money;
        lastP2Money = _p2Money;
        hasMoneySnapshot = true;

        // 1) Determine winner (based on scores) but DON'T give MoneyPool yet.
        if (p1Score > p2Score)
        {
            roundWinner = 1;
            // UI_PlayerStatsOnTable.roundFinished(1);
        }
        else if (p2Score > p1Score)
        {
            roundWinner = 2;
            // UI_PlayerStatsOnTable.roundFinished(2);
        }
        else
        {
            roundWinner = 0; // tie
            
        }

        // Determine losers (needed for pump triple rule)
        bool p1WasLoser = (roundWinner == 2);
        bool p2WasLoser = (roundWinner == 1);

        // 2) Calculate deductions for each player (including pump triple-if-they-lose)
        int p1ThisDeduct = p1MoneyDeduct;
        if (p1WasLoser && p1PumpPlacedRound >= 0 && p1PumpPlacedRound == (roundsPlayed - 1))
        {
            p1ThisDeduct *= 3; // triple one time because they had placed pump in previous round and lost this round
            p1PumpPlacedRound = -1; // clear pump effect after it fires
            // Debug.Log("P1 pump penalty (triple) applied.");
        }

        int p2ThisDeduct = p2MoneyDeduct;
        if (p2WasLoser && p2PumpPlacedRound >= 0 && p2PumpPlacedRound == (roundsPlayed - 1))
        {
            p2ThisDeduct *= 3;
            p2PumpPlacedRound = -1;
            // Debug.Log("P2 pump penalty (triple) applied.");
        }

        // 3) Apply deductions to players and add to MoneyPool
        // Use ChangeMoney so it triggers CheckForLoser (but we are still mid-round; that's ok)
        ChangeMoney(1, -p1ThisDeduct);
        ChangeMoney(2, -p2ThisDeduct);
        MoneyPool = p1ThisDeduct + p2ThisDeduct;

        // Debug.Log($"Deductions: P1 -{p1ThisDeduct}, P2 -{p2ThisDeduct}. MoneyPool now {MoneyPool}. Money after deduction: P1={_p1Money} P2={_p2Money}");

        // 4) Award MoneyPool to winner (or split on tie)
        if (roundWinner == 1)
        {
            ChangeMoney(1, MoneyPool);
            activeClip = player1Won;
            audioSource.PlayOneShot(activeClip);
            // Debug.Log($"Player 1 wins round and receives {MoneyPool}. P1 money now {_p1Money}.");
            UI_PlayerStatsOnTable.roundFinished(1, MoneyPool);
            playerRoundWinFlash.StartFlashEffect(1);
        }
        else if (roundWinner == 2)
        {
            ChangeMoney(2, MoneyPool);
            activeClip = player2Won;
            audioSource.PlayOneShot(activeClip);
            // Debug.Log($"Player 2 wins round and receives {MoneyPool}. P2 money now {_p2Money}.");
            UI_PlayerStatsOnTable.roundFinished(2, MoneyPool);
            playerRoundWinFlash.StartFlashEffect(2);
        }
        else // tie
        {
            int half = MoneyPool / 2;
            ChangeMoney(1, half);
            ChangeMoney(2, half);
            activeClip = gameTied;
            audioSource.PlayOneShot(activeClip);
            // Debug.Log($"Round tie, each receives {half}.");
            UI_PlayerStatsOnTable.roundFinished(0, MoneyPool / 2);
            playerRoundWinFlash.StartFlashEffect(0);
        }

        // 5) Save lastRoundWinner and check game-over (CheckForLoser called by ChangeMoney already)
        lastRoundWinner = roundWinner;

        // 6) Reset round scores and increment the base deduction per your original plan.
        ResetScores();

        // Only now increment roundsPlayed, AFTER deductions/penalties are done
        roundsPlayed++;
        // Debug.Log($"Full round completed. roundsPlayed = {roundsPlayed}");

        // After full-round resolution, move to next turn (this will wrap and increment roundsPlayed)
        // NextTurn();
    }

    private void CheckForLoser()
    {
        if (isGameOverTriggered) return; // guard: only trigger once
        if (suppressCheckForLoser) return; // don't trigger while restoring snapshots

        // handle both-lose case explicitly
        bool p1Dead = _p1Money <= 0;
        bool p2Dead = _p2Money <= 0;

        if (p1Dead && p2Dead)
        {
            isGameOverTriggered = true;
            gameWinner = 0; // 0 = draw / special
            PlayerTurn.GameWinner = gameWinner; // set static for GameOver scene
            // Debug.Log("Both players have <= 0 money — draw condition.");
            // optionally set DontDestroyOnLoad(gameObject); if you need it in GameOver scene
            // DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("GameOver");
            return;
        }
        else if (p1Dead)
        {
            isGameOverTriggered = true;
            gameWinner = 2;
            PlayerTurn.GameWinner = gameWinner; // set static for GameOver scene
            // Debug.Log("Player 2 wins the game!");
            // DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("GameOver");
            return;
        }
        else if (p2Dead)
        {
            isGameOverTriggered = true;
            gameWinner = 1;
            PlayerTurn.GameWinner = gameWinner; // set static for GameOver scene
            // Debug.Log("Player 1 wins the game!");
            // DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("GameOver");
            return;
        }
        // otherwise no loser yet
    }

    private void ResetScores()
    {
        CurrentPlayerTurn = 1;
        // diceThrower.p1RolledDice = false;
        // diceThrower.p2RolledDice = false;
        p1Score = 0;
        p2Score = 0;

        // increase base deduction by 3 every completed round
        p1MoneyDeduct += 3;
        p2MoneyDeduct += 3;

        // reset temporary pool & dice count
        MoneyPool = 0;
        if (diceThrower != null)
        {
            diceThrower._finishedDiceCount = 0;
        }

        // Debug.Log($"currentPlayerTurn={CurrentPlayerTurn}");
        // Debug.Log($"Reset scores. p1MoneyDeduct={p1MoneyDeduct} p2MoneyDeduct={p2MoneyDeduct}");
    }

    // Restore last snapshot. Only allow if we actually have a snapshot (avoid zeroing out by accident).
    public void RerollDice()
    {
        if (!hasMoneySnapshot)
        {
            // Debug.LogWarning("RerollDice called but no valid snapshot exists. Ignoring.");
            return;
        }

        // Suppress CheckForLoser while we restore state so we don't immediately trigger scene load
        bool prevSuppress = suppressCheckForLoser;
        suppressCheckForLoser = true;

        _p1Money = lastP1Money;
        _p2Money = lastP2Money;
        roundWinner = lastRoundWinner;

        // reverse the last per-round increment of deduction (safeguarded)
        if (roundsPlayed > 0)
        {
            roundsPlayed--;
            p1MoneyDeduct = Mathf.Max(3, p1MoneyDeduct - 3);
            p2MoneyDeduct = Mathf.Max(3, p2MoneyDeduct - 3);
        }

        ResetScores();

        // restore previous suppression state and run one check now
        suppressCheckForLoser = prevSuppress;
        CheckForLoser();

        textController.ShowText("diceRerollActivated", 0);

        // UI_PlayerStatsOnTable.PlayerTurnFinished(1);
        // Debug.Log("RerollDice: restored previous snapshot and reset scores.");
    }
}

#endregion