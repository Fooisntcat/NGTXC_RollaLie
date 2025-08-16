using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DiceRollScriptWithoutUsingFaceDetect : MonoBehaviour
{
    public Transform[] diceFaces;
    public Rigidbody rb;

    private int _diceIndex = -1;
    public bool _hasStoppedRolling;
    private bool _delayFinished;
    public bool _isRolling = false; // is this even working lmao?


    public static UnityAction<int, int> OnDiceResult;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _delayFinished = false;
        _hasStoppedRolling = false;
    }

    private void Update()
    {
        if (!_delayFinished) return;
        if (!_hasStoppedRolling && rb.angularVelocity == Vector3.zero)
        {
            _isRolling = true; // lock rolling
            _hasStoppedRolling = true;
            GetNumberOnTopFace();
        }
        Debug.Log("dice stop rolling var: " + _hasStoppedRolling);
        Debug.Log("_isRolling" + _isRolling);
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

        Debug.Log($"Dice Result: {topFace + 1}");

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
    }

    private async void DelayResult()
    {
        Debug.Log("Delay Result");
        await Task.Delay(1000);
        _hasStoppedRolling = false;
        _delayFinished = true;
    }
}
