using System.Collections.Generic;
using System.Threading.Tasks;
// using Unity.VisualScripting;
using UnityEngine;
using TiltFive;
// using UnityEngine.InputSystem;

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
    public Transform P1WandLocation;
    public Transform P2WandLocation;
    private PlayerTurn playerTurn;
    private bool _nextTurnTriggered = false;
    private List<GameObject> _spawnedDice = new List<GameObject>();

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
            if (playerTurn.CurrentPlayerTurn == 1 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One) || UnityEngine.Input.GetKey("space")))
            {
                _nextTurnTriggered = false;
                foreach (var die in _spawnedDice)
                {
                    Destroy(die);
                }

                //await Task.Delay(1000);
                RollDice();
            }

            if (playerTurn.CurrentPlayerTurn == 2 && (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two) || UnityEngine.Input.GetKey("up")))
            {
                _nextTurnTriggered = false;
                foreach (var die in _spawnedDice)
                {
                    Destroy(die);
                }

                //await Task.Delay(1000);
                RollDice();
            }
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
            // if (!_nextTurnTriggered && !_isRolling)
            // {
            //     _nextTurnTriggered = true;
            // PlayerTurn.Instance.NextTurn();
            // }
        }
    }

}
