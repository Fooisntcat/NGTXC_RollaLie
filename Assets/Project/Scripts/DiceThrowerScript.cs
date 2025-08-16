using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using TiltFive;
using UnityEngine.InputSystem;

public class DiceThrowerScript : MonoBehaviour
{
    public DiceRollScript dice;
    public int amountOfDice = 2;
    public float throwForce = 5f;
    public float rollForce = 10f;
    private int _finishedDiceCount = 0;
    public bool _isRolling;
    public bool _hasStoppedRolling;
    public Transform wandLocation;
    private List<GameObject> _spawnedDice = new List<GameObject>();

    private void Update()
    {
        if (wandLocation != null)
        {
            transform.position = wandLocation.position;
        }
        if (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A) && !_hasStoppedRolling)
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
            var newDice = Instantiate(dice, transform.position, transform.rotation);
            _spawnedDice.Add(newDice.gameObject);
            newDice.RollDice(throwForce, rollForce, i);
            await Task.Yield();
        }
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
