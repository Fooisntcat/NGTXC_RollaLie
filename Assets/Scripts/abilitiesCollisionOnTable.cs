using UnityEngine;
using TiltFive;

public class diceReroll : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasRerolled = false;
    [SerializeField] private PlayerIndex? lastPlayerTouched = null;
    private ControllerIndex lastControllerTouched = ControllerIndex.Right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasRerolled = false;
    }

    // Update is called once per frame
    void Update()
    {

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
        if (collision.gameObject.CompareTag("PumpNDump"))
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
    }
}