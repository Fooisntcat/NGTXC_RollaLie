using UnityEngine;
using TiltFive;
using System.Runtime.CompilerServices;
using System.Threading;

public class diceReroll : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasRerolled = false;
    // [SerializeField] private PlayerIndex? lastPlayerTouched = null;
    [SerializeField] private PlayerIndex lastPlayerTouched;
    private ControllerIndex lastControllerTouched = ControllerIndex.Right;
    private PlayerTurn playerTurn;
    private WandFlashbang wandFlashbang;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasRerolled = false;
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        wandFlashbang = FindFirstObjectByType<WandFlashbang>();
        wandFlashbang.HasDeducted = false; // Reset the deduction state
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10f) // Dice fell off the table
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Flashbang hits table → trigger explosion
        if (!hasRerolled && collision.gameObject.CompareTag("Table"))
        {
            Debug.Log("Rerolling dice due to collision with table.");
            PlayerTurn.Instance.RerollDice();
            hasRerolled = true;
            Destroy(gameObject, 1f);
        }
    }

        /*
        if (collision.gameObject.CompareTag("Hand"))
        {
            var id = collision.collider.GetComponentInParent<WandIdentity>();
            if (id != null)
            {
                lastPlayerTouched = id.playerIndex;
                lastControllerTouched = id.controllerIndex;
                Debug.Log($"Touched by: {lastPlayerTouched} ({lastControllerTouched})");
            }
            PlayerTurn.Instance.PumpNDump(id.playerIndex == PlayerIndex.One ? 1 : 2);
        }
        */
    private void OnTriggerEnter(Collider other)
    {
        if (wandFlashbang.HasDeducted) return;

        if (other.CompareTag("p1Hand"))
        {
            Debug.Log("Hand collision detected for Player 1.");
            if (TiltFive.Input.GetTrigger(ControllerIndex.Right, PlayerIndex.One) > 0.8f)
            {
                playerTurn.p1Money -= 10; // Deduct 10 from Player 1's money
                wandFlashbang.HasDeducted = true;
            }
        }
        if (other.CompareTag("p2Hand"))
        {
            Debug.Log("Hand collision detected for Player 2.");
            if (TiltFive.Input.GetTrigger(ControllerIndex.Right, PlayerIndex.Two) > 0.8f)
            {
                playerTurn.p2Money -= 10; // Deduct 10 from Player 2's money
                wandFlashbang.HasDeducted = true;
            }
        }
    }
}



/*
using UnityEngine;
using TiltFive;

public class diceReroll : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasRerolled = false;

    [SerializeField] private PlayerIndex lastPlayerTouched;
    private ControllerIndex lastControllerTouched = ControllerIndex.Right;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasRerolled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        Debug.Log($"[{name}] OnCollisionEnter with {other.name} (tag={other.tag})");

        // Table collision -> reroll
        if (!hasRerolled && other.CompareTag("Table"))
        {
            Debug.Log("Rerolling dice due to collision with table.");
            PlayerTurn.Instance?.RerollDice();
            hasRerolled = true;
            Destroy(gameObject, 1f);
            return;
        }

        // Hand collision -> detect which wand/player touched it
        if (other.CompareTag("Hand"))
        {
            // Try to find WandIdentity. Try the specific collider first, then the whole gameObject.
            WandIdentity id = collision.collider.GetComponentInParent<WandIdentity>();
            if (id == null) id = other.GetComponentInParent<WandIdentity>();

            if (id == null)
            {
                Debug.LogWarning("Hand collided but no WandIdentity found on collider or parents.");
                return;
            }

            lastPlayerTouched = id.playerIndex;
            lastControllerTouched = id.controllerIndex;
            Debug.Log($"Touched by: {lastPlayerTouched} ({lastControllerTouched})");

            // Map PlayerIndex safely to player number
            int playerNumber = -1;
            if (id.playerIndex == PlayerIndex.One) playerNumber = 1;
            else if (id.playerIndex == PlayerIndex.Two) playerNumber = 2;

            if (playerNumber <= 0)
            {
                Debug.LogWarning($"Unhandled playerIndex: {id.playerIndex}");
                return;
            }

            if (PlayerTurn.Instance == null)
            {
                Debug.LogError("PlayerTurn.Instance is null. Cannot call PumpNDump.");
                return;
            }

            PlayerTurn.Instance.PumpNDump(playerNumber);
        }
    }
}
*/