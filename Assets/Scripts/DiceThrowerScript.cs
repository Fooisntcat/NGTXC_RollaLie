using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TiltFive;

public class DiceThrowerScript : MonoBehaviour
{
    public DiceRollScript dice;

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

    // Nudge Vars
    [Header("Nudge Settings")]
    public float nudgeForce = 2f;    // strength of joystick push
    public float nudgeCooldown = 0.1f; // prevent spamming force
    private float _lastNudgeTime = 0f;

    void Awake()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
    }

    private void Update()
    {
        // Debug.Log("Dice is rolling: " + _isRolling);

        // if (P1WandLocation != null)
        // {
        //     transform.position = P1WandLocation.position;
        //     transform.rotation = P1WandLocation.rotation;
        // }

        // Roll Dice
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

        if (!_isRolling && playerTurn != null)
        {
            if (playerTurn.CurrentPlayerTurn == 1 && _finishedDiceCount == 0 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One) || (UnityEngine.Input.GetKey(KeyCode.Q))))
            {
                _isRolling = true; // lock rolling
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

            if (playerTurn.CurrentPlayerTurn == 2 && _finishedDiceCount == 2 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two) || (UnityEngine.Input.GetKey(KeyCode.W))))
            {
                _isRolling = true; // lock rolling
                // playerTurn.p2Score = 0;
                _nextTurnTriggered = false;
                foreach (var die in _spawnedDice)
                {
                    Destroy(die);
                }

                //await Task.Delay(1000);
                RollDice();
            }

            // Handle Nudging via Joystick
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
            _isRolling = false; // unlock
            Debug.Log("All dice finished rolling!");
        }
    }

/*
    private void HandleNudge()
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
            wandLocation = P1WandLocation;
        // else if (playerTurn != null && playerTurn.CurrentPlayerTurn == 2)
            // wandLocation = P2WandLocation;

        if (wandLocation == null) return;

        // Get joystick vector from Tilt Five wand
        Vector2 stickInput = TiltFive.Input.GetStickTilt();

        // Optional keyboard fallback
        float x = stickInput.x + UnityEngine.Input.GetAxis("Horizontal");
        float y = stickInput.y + UnityEngine.Input.GetAxis("Vertical");

        // Make it relative to wand’s forward direction
        Vector3 nudgeDirection = (wandLocation.forward * y) + (wandLocation.right * x);

        if (nudgeDirection.magnitude > 0.1f && Time.time - _lastNudgeTime > nudgeCooldown)
        {
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
    }
    
}
