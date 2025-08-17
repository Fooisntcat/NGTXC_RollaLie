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
    private int _finishedDiceCount = 0;
    public bool _isRolling = false;
    // Wand vars
    public Transform wandLocation;
    private List<GameObject> _spawnedDice = new List<GameObject>();

    private void Update()
    {
        Debug.Log("Dice is rolling: " + _isRolling);
        if (wandLocation != null)
        {
            transform.position = wandLocation.position;
            transform.rotation = wandLocation.rotation;
        }
        if ((TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A) || UnityEngine.Input.GetKey("space")) && !_isRolling)
        {
            foreach (var die in _spawnedDice)
            {
                Destroy(die);
            }

            //await Task.Delay(1000);
            RollDice();
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

}
