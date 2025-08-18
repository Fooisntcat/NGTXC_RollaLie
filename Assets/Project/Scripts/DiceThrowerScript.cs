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
    private int _finishedDiceCount = 0;
    public bool _isRolling = false;

    // Wand vars
    public Transform wandLocation;
    private List<GameObject> _spawnedDice = new List<GameObject>();

    // Nudge Vars
    [Header("Nudge Settings")]
    public float nudgeForce = 2f;    // strength of joystick push
    public float nudgeCooldown = 0.1f; // prevent spamming force
    private float _lastNudgeTime = 0f;

    private void Update()
    {
        Debug.Log("Dice is rolling: " + _isRolling);

        if (wandLocation != null)
        {
            transform.position = wandLocation.position;
            transform.rotation = wandLocation.rotation;
        }

        // Roll Dice
        if ((TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A) || UnityEngine.Input.GetKey("space")) && !_isRolling)
{
    foreach (var die in _spawnedDice)
    {
        Destroy(die);
    }
    _spawnedDice.Clear(); // ✅ clear list so no dead dice stay around

    RollDice();
}

        // Handle Nudging via Joystick
        HandleNudge();
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

        PlayerTurn.Instance.NextTurn();
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

        if (_finishedDiceCount >= amountOfDice)
        {
            _isRolling = false; // unlock
            _finishedDiceCount = 0;
            Debug.Log("All dice finished rolling!");
        }
    }

private void HandleNudge()
{
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
}
