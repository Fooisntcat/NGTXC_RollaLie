using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TiltFive;
using TimerCountdown;
using UnityEngine.XR;

public class DiceThrowerScript : MonoBehaviour
{
    #region Variables
    [Header("Dice Settings")]
    public DiceRollScript dice;
    private TimerCountdown.Timer timerCountdown;

    // Players
    private bool p1RolledDice = false;
    private bool p2RolledDice = false;
    public bool p1Cheat = false;
    public bool p2Cheat = false;

    // Rolling Vars
    public int amountOfDice = 2;
    public float throwForce = 5f;
    public float rollForce = 10f;
    public int _finishedDiceCount = 0;
    public bool _isRolling = false;

    // Wand vars
    public Transform wandLocation; // Wand location to throw dice from
    public Transform P1WandLocation;
    public Transform P2WandLocation;
    private PlayerTurn playerTurn;
    private bool _nextTurnTriggered = false;
    private List<GameObject> _spawnedDice = new List<GameObject>();

    // Cheating Manager (unused)
    private Queue<(int playerId, bool cheated)> cheatLog = new Queue<(int, bool)>();
    private PlayerTurn playerTurnScript;
    private HashSet<int> playersLogged = new HashSet<int>();

    // Nudge Vars
    [Header("Nudge Settings")]
    public float nudgeForce = 2f;    // strength of joystick push
    public float nudgeCooldown = 0.1f; // prevent spamming force
    private float _lastNudgeTime = 0f;

    #endregion

    void Awake()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        timerCountdown = FindFirstObjectByType<TimerCountdown.Timer>();
        playerTurnScript = FindFirstObjectByType<PlayerTurn>();
    }

    private void Update()
    {
        // Debug.Log("Dice is rolling: " + _isRolling);

        // if (P1WandLocation != null)
        // {
        //     transform.position = P1WandLocation.position;
        //     transform.rotation = P1WandLocation.rotation;
        // }

        // Dice Throw Position
        if (playerTurn.CurrentPlayerTurn == 1 && P1WandLocation != null)
        {
            transform.position = P1WandLocation.position;
            transform.rotation = P1WandLocation.rotation;
        }
        else if (playerTurn.CurrentPlayerTurn == 2 && P2WandLocation != null)
        {
            transform.position = P2WandLocation.position;
            transform.rotation = P2WandLocation.rotation;
        }

        // Makes sure the dice stops rolling, the currentPlayerTurn gets to roll
        if (!_isRolling && playerTurn != null && timerCountdown.timeRemaining == 0)
        {
            // if (playerTurn.CurrentPlayerTurn == 1 && _finishedDiceCount == 0 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One) || (UnityEngine.Input.GetKey(KeyCode.Q))))
            if (playerTurn.CurrentPlayerTurn == 1 && _finishedDiceCount == 0 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.One) || (UnityEngine.Input.GetKey(KeyCode.Q))))
            {
                p1RolledDice = true;
                _isRolling = true; // lock rolling
                timerCountdown.ResetTimer();
                // _finishedDiceCount = 0;
                // playerTurn.p1Score = 0;
                _nextTurnTriggered = false;
                foreach (var die in _spawnedDice)
                {
                    Destroy(die);
                }
                _spawnedDice.Clear(); // clear list so no dead dice stay around

                RollDice();
            }

            if (playerTurn.CurrentPlayerTurn == 2 && _finishedDiceCount == 2 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.Two) || (UnityEngine.Input.GetKey(KeyCode.E))))
            {
                p2RolledDice = true;
                _isRolling = true; // lock rolling
                timerCountdown.ResetTimer();
                // playerTurn.p2Score = 0;
                _nextTurnTriggered = false;
                foreach (var die in _spawnedDice)
                {
                    Destroy(die);
                }

                //await Task.Delay(1000);
                RollDice();
            }
        }
        else if (timerCountdown.timeRemaining == 0)
        {
            p1Cheat = false;
            p2Cheat = false;
            Debug.Log("Cheat codes reset.");
        }

        // Handle Nudge
        if (timerCountdown != null && timerCountdown.timeRemaining > 0)
        {
            HandleNudge();
        }
    }

    private async void RollDice()
    {
        if (dice == null) return;

        for (int i = 0; i < amountOfDice; i++)
        {
            _isRolling = true; // lock

            var newDice = Instantiate(dice, transform.position, transform.rotation);
            _spawnedDice.Add(newDice.gameObject);

            newDice.RollDice(throwForce, rollForce, i);

            await Task.Yield();
        }
        // PlayerTurn.Instance.NextTurn();
    }

    private void OnEnable()
    {
        DiceRollScript.OnDiceResult += OnDiceFinished;
    }

    private void OnDisable()
    {
        DiceRollScript.OnDiceResult -= OnDiceFinished;
    }

    private void OnDiceFinished(int diceIndex, int result)
    {
        _finishedDiceCount++;
        // Debug.Log($"Dice {diceIndex} finished with result: {result}. Total finished: {_finishedDiceCount}");
        if (_finishedDiceCount >= amountOfDice)
        {
            _isRolling = false; // 
            Debug.Log("All dice finished rolling!");
        }
    }

    /* private void HandleNudge()
        {
            if (dice == null) return;
            // Get joystick vector from Tilt Five wand (returns Vector2)
            Vector2 stickInput = TiltFive.Input.GetStickTilt();

            // Optional keyboard fallback for testing
            float x = stickInput.x + UnityEngine.Input.GetAxis("Horizontal");
            float y = stickInput.y + UnityEngine.Input.GetAxis("Vertical");

            // OLD: nudgeDirection was just world-based
            // Vector3 nudgeDirection = new Vector3(x, 0, y);

            // NEW: make it relative to wand’s forward direction
            Vector3 nudgeDirection = (wandLocation.forward * y) + (wandLocation.right * x);

            if (nudgeDirection.magnitude > 0.1f && Time.time - _lastNudgeTime > nudgeCooldown)
            {
                foreach (var die in _spawnedDice)
                {
                    Rigidbody rb = die.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddForce(nudgeDirection.normalized * nudgeForce, ForceMode.Impulse);
                    }
                }

                _lastNudgeTime = Time.time;
            }
        }
        */
    private void HandleNudge()
    {
        if (dice == null) return;

        // Pick correct wand based on player turn
        Transform wandLocation = null;
        // if (playerTurn != null && playerTurn.CurrentPlayerTurn == 1)
        // wandLocation = P1WandLocation;
        // else if (playerTurn != null && playerTurn.CurrentPlayerTurn == 2)
        // wandLocation = P2WandLocation;

        // Get joystick vector from Tilt Five wand
        Vector2 stickInput = TiltFive.Input.GetStickTilt();

        if (playerTurn != null && timerCountdown.timeRemaining >= 0)
        {
            if (playerTurn.CurrentPlayerTurn == 1 && _finishedDiceCount == 0 && p1RolledDice)
            {
                wandLocation = P1WandLocation;
                stickInput = TiltFive.Input.GetStickTilt(ControllerIndex.Right, PlayerIndex.One);
            }
            else if (playerTurn.CurrentPlayerTurn == 2 && _finishedDiceCount == 2 && p2RolledDice)
            {
                wandLocation = P2WandLocation;
                stickInput = TiltFive.Input.GetStickTilt(ControllerIndex.Right, PlayerIndex.Two);
            }
            else
            {
                wandLocation = null;
                stickInput = Vector2.zero;
            }
        }

        if (wandLocation == null) return;

        // Optional keyboard fallback
        float x = stickInput.x + UnityEngine.Input.GetAxis("Horizontal");
        float y = stickInput.y + UnityEngine.Input.GetAxis("Vertical");
        // Make it relative to wand’s forward direction
        Vector3 nudgeDirection = (wandLocation.forward * y) + (wandLocation.right * x);

        if (nudgeDirection.magnitude > 0.1f && Time.time - _lastNudgeTime > nudgeCooldown)
        {
            if (playerTurn != null && playerTurn.CurrentPlayerTurn == 1 && _finishedDiceCount == 0 && p1RolledDice && timerCountdown.timeRemaining >= 0)
            {
                p1Cheat = true; // Player 1 nudged
                // AddCheat(1, true); // Player 1 nudged
            }
            else if (playerTurn != null && playerTurn.CurrentPlayerTurn == 2 && _finishedDiceCount == 2 && p2RolledDice && timerCountdown.timeRemaining >= 0)
            {
                p2Cheat = true; // Player 2 nudged
                // AddCheat(2, true); // Player 2 nudged
            }

            foreach (var die in _spawnedDice)
            {
                if (die == null) continue;

                Rigidbody rb = die.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(nudgeDirection.normalized * nudgeForce, ForceMode.Impulse);
                }
            }

            _lastNudgeTime = Time.time;
        }
        else if (nudgeDirection.magnitude < 0.1f)
        {
            if (playerTurn != null && playerTurn.CurrentPlayerTurn == 1)
            {
                p1Cheat = false; // Player 1 stopped nudging
            }
            else if (playerTurn != null && playerTurn.CurrentPlayerTurn == 2)
            {
                p2Cheat = false; // Player 2 stopped nudging
            }
            // AddCheat(playerTurn.CurrentPlayerTurn, false);
        }
    }

    #region Cheating Logs (unused)
    /*
    public void AddCheat(int playerId, bool cheated)
    {
        var tempList = new List<(int playerId, bool cheated)>(cheatLog);

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].playerId == playerId)
            {
                // Already exists
                if (cheated && tempList[i].cheated == false)
                {
                    // Upgrade false -> true
                    tempList[i] = (playerId, true);
                    cheatLog = new Queue<(int, bool)>(tempList);
                    Debug.Log($"Player {playerId} upgraded from false to true");
                }
                else
                {
                    // Otherwise ignore
                    Debug.Log($"Player {playerId} already logged as {tempList[i].cheated}, no change");
                }
                return;
            }
        }

        // If not logged yet, add new
        cheatLog.Enqueue((playerId, cheated));
        if (cheatLog.Count > playerTurn.PlayerCount)
            cheatLog.Dequeue();

        playersLogged.Add(playerId);
        Debug.Log($"Player {playerId} logged new entry: {cheated}");
        DebugLogCheatLog();
    }

    private void DebugLogCheatLog()
    {
        string log = "Current CheatLog: ";
        foreach (var entry in cheatLog)
        {
            log += $"[P{entry.playerId}, cheat={entry.cheated}] ";
        }
        Debug.Log(log);
    }

    public void ResetRound() // Called from PlayerTurn.cs (ResetScores())
    {
        playersLogged.Clear();
        p1RolledDice = false;
        p2RolledDice = false;
        cheatLog.Clear();
        Debug.Log("New round started — all players can log again!");
    }
    public bool? GetLatestCheat(int playerId)
    {
    foreach (var entry in cheatLog)
    {
        if (entry.playerId == playerId)
            return entry.cheated;
    }
    return null; // not found
    }
    */
    #endregion
}
