using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DiceRollScript : MonoBehaviour
{
    public Transform[] diceFaces;
    public Rigidbody rb;

    private int _diceIndex = -1;
    public bool _hasStoppedRolling;
    private bool _delayFinished;
    // public bool diceThrower._isRolling = false; // is this even working lmao? (its not haha)
    // public GameObject diceThrower;
    // public diceThrower diceThrower;
    private DiceThrowerScript diceThrower;
    // public static int amountOfDice;


    public static UnityAction<int, int> OnDiceResult;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _delayFinished = false;
        _hasStoppedRolling = false;
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
    }

    private void Update()
    {
        if (!_delayFinished) return;
        if (!_hasStoppedRolling && rb.angularVelocity == Vector3.zero)
        {
            diceThrower._isRolling = false; // lock rolling
            _hasStoppedRolling = true;
            GetNumberOnTopFace();
            Debug.Log("Current dice index: " + _diceIndex);
        }
        // if (transform.position.y < -10f && diceThrower._isRolling) // Dice fell off the table
        if (transform.position.y < -10f) // Dice fell off the table
        {
            Debug.Log("dice < -10f diceThrower._isRolling? " + diceThrower._isRolling);
            diceThrower._isRolling = false;
            Debug.Log("Dice fell off the table");
            Destroy(gameObject);
        }
    }

    [ContextMenu(itemName: "Get Top Face")]
    private void GetNumberOnTopFace()
    {
        if (diceFaces == null) return;

        var topFace = 0;
        var lastYPosition = diceFaces[0].position.y;

        for (int i = 0; i < diceFaces.Length; i++)
        {
            if (diceFaces[i].position.y > lastYPosition)
            {
                lastYPosition = diceFaces[i].position.y;
                topFace = i;
            }
        }

        OnDiceResult?.Invoke(_diceIndex, topFace + 1);

        // Debug.Log($"Dice Result: {topFace + 1}");
        PlayerTurn.Instance.AddPoint(topFace + 1);

    }

    public void RollDice(float throwForce, float rollForce, int i)
    {
        //_delayFinished = false;      // Reset delay flag
        //_hasStoppedRolling = false;  // Reset stopped rolling flag

        _diceIndex = i;
        var RandomVariance = Random.Range(-1f, 1f);
        rb.AddForce(transform.forward * (throwForce + RandomVariance), ForceMode.Impulse); //foward = blue axis, forcemode.impulse only adds force then physics engine takes over it.

        var randX = Random.Range(0f, 1f);
        var randY = Random.Range(0f, 1f);
        var randZ = Random.Range(0f, 1f);

        rb.AddTorque(new Vector3(randX, randY, randZ) * (rollForce + RandomVariance), ForceMode.Impulse);

        DelayResult();
        //  _hasStoppedRolling = false;
        // _delayFinished = true;
    }

    // MUST HAVE DELAYRESULT (if not the CurrentPlayerTurn will turns to 3)
    private async void DelayResult()
    {
        await Task.Delay(1000);
        _hasStoppedRolling = false;
        _delayFinished = true;
    }
}
