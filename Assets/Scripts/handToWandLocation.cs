using TiltFive;
using UnityEngine;

public class handToWandLocation : MonoBehaviour
{
    [SerializeField] private Transform wandLocation;
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;
    [SerializeField] private AudioSource slap;
    [SerializeField] private HandFlash p1HandFlash; // Assign Player 1's HandFlash component
    [SerializeField] private HandFlash p2HandFlash; // Assign Player 2's HandFlash component
    private int slapCost = 5;
    public Wand wand; // Assign via Inspector or get from Input
    private PlayerTurn playerTurn;
    private DiceThrowerScript diceThrower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right, playerIndex);
        if (wandLocation != null)
        {
            // transform.position = wandLocation.position + wandLocation.right * 2f; // 0.1 = 10cm;
            // transform.position = wandLocation.position + wandLocation.forward * 0.5f; // 0.1 = 10cm forward
            transform.position = wandLocation.TransformPoint(new Vector3(-0.5f, 0.3f, -1.8f));

            transform.rotation = wandLocation.rotation * Quaternion.Euler(-90f, 0f, 90f);
            // transform.rotation = wandLocation.rotation;
            Animator animator = GetComponent<Animator>();
            if (animator != null || UnityEngine.Input.GetKey("return"))
            {
                animator.SetBool("handClose", trigger > 0.4f);

            }
            else
            {
                animator.SetBool("handClose", false);
            }
        }
    }
    // Slap hand (clap sound)
    private void OnTriggerEnter(Collider other)
    {
        if (playerTurn == null || diceThrower == null) return;
        // Debug.Log("Collision Hand");
        bool p1Cheat = diceThrower.p1Cheat;
        bool p2Cheat = diceThrower.p2Cheat;
        if (other.CompareTag("p1Hand"))
        // if (other.CompareTag("p1Hand") || other.CompareTag("p2Hand"))
        {
            slap.Play();
            Debug.Log("Slap hand detected!");
            // wand.Input(0.5f, 0.2f);
            TiltFive.Wand.TrySendImpulse(1f, 0.1f, PlayerIndex.One, ControllerIndex.Right);
            TiltFive.Wand.TrySendImpulse(1f, 0.1f, PlayerIndex.Two, ControllerIndex.Right);

            if (p1Cheat == true && playerTurn.CurrentPlayerTurn == 1)
            {
                // slap.Play();
                // playerTurn.p1Money -= 10; // Deduct 10 from Player 1's money
                PlayerTurn.Instance.ChangeMoney(1, -slapCost);
                p1HandFlash.TriggerFlash(Color.white, 1f); 
            }
            else if (p2Cheat == true && playerTurn.CurrentPlayerTurn == 2)
            {
                // slap.Play();
                PlayerTurn.Instance.ChangeMoney(2, -slapCost);
                p2HandFlash.TriggerFlash(Color.white, 1f); 
            }
            else if (p1Cheat == false && playerTurn.CurrentPlayerTurn == 1)
            {
                PlayerTurn.Instance.ChangeMoney(2, -slapCost);
                p2HandFlash.TriggerFlash(Color.red, 1f); 
            }
            else if (p2Cheat == false && playerTurn.CurrentPlayerTurn == 2)
            {
                PlayerTurn.Instance.ChangeMoney(1, -slapCost);
                p1HandFlash.TriggerFlash(Color.red, 1f); 
            }
        }
    }
}


/* buggy?
using TiltFive;
using UnityEngine;

public class handToWandLocation : MonoBehaviour
{
    [SerializeField] private Transform wandLocation;
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;
    [SerializeField] private AudioSource slap;
    private int slapCost = 5;
    public Wand wand; // optional
    private PlayerTurn playerTurn;
    private DiceThrowerScript diceThrower;
    private Animator animator;

    void Start()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        diceThrower = FindFirstObjectByType<DiceThrowerScript>();
        animator = GetComponent<Animator>();

        if (playerTurn == null) Debug.LogWarning("PlayerTurn not found in scene.");
        if (diceThrower == null) Debug.LogWarning("DiceThrowerScript not found in scene.");
        if (animator == null) Debug.Log("Animator not found on this GameObject (that's OK if you don't need it).");
    }

    void Update()
    {
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right, playerIndex);

        if (wandLocation != null)
        {
            transform.position = wandLocation.TransformPoint(new Vector3(-0.5f, 0.5f, -1.5f));
            transform.rotation = wandLocation.rotation * Quaternion.Euler(-90f, 0f, 90f);
        }

        // Only use animator if it's not null
        if (animator != null)
        {
            bool closed = trigger > 0.4f || UnityEngine.Input.GetKey("return");
            animator.SetBool("handClose", closed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerTurn == null || diceThrower == null)
        {
            Debug.LogWarning("Missing references in handToWandLocation; aborting trigger handling.");
            return;
        }

        bool p1Cheat = diceThrower.p1Cheat;
        bool p2Cheat = diceThrower.p2Cheat;

        // Helper to play sound (if assigned)
        void PlaySlap()
        {
            if (slap != null)
            {
                slap.pitch = Random.Range(0.8f, 1.2f);
                slap.Play();
            }
        }

        // Handle p1's physical hand being slapped
        if (other.CompareTag("p1Hand"))
        {
            PlaySlap();
            // send haptic only to player 1's wand
            TiltFive.Wand.TrySendImpulse(1f, 0.1f, PlayerIndex.One, ControllerIndex.Right);

            Debug.Log($"p1Hand slapped — turn={playerTurn.CurrentPlayerTurn} p1Cheat={p1Cheat}");
            if (playerTurn.CurrentPlayerTurn == 1)
            {
                // If it's player 1's turn: if p1 cheated -> minus p1; else minus p2
                if (p1Cheat)
                {
                    // playerTurn.p1Money -= 10;
                    PlayerTurn.Instance.ChangeMoney(1, -slapCost);
                    Debug.Log("Deducted 10 from p1Money (p1 cheated on their turn).");
                }
                else
                {
                    // playerTurn.p2Money -= 10;
                    PlayerTurn.Instance.ChangeMoney(2, -slapCost);
                    Debug.Log("Deducted 10 from p2Money (p1 did NOT cheat on their turn).");
                }
            }
            // if it's not p1's turn: currently do nothing (adjust if you want a different behavior)
        }
        // Handle p2's physical hand being slapped
        else if (other.CompareTag("p2Hand"))
        {
            PlaySlap();
            // send haptic only to player 2's wand
            TiltFive.Wand.TrySendImpulse(1f, 0.1f, PlayerIndex.Two, ControllerIndex.Right);

            Debug.Log($"p2Hand slapped — turn={playerTurn.CurrentPlayerTurn} p2Cheat={p2Cheat}");
            if (playerTurn.CurrentPlayerTurn == 2)
            {
                // If it's player 2's turn: if p2 cheated -> minus p2; else minus p1
                if (p2Cheat)
                {
                    // playerTurn.p2Money -= 10;
                    PlayerTurn.Instance.ChangeMoney(2, -slapCost);
                    Debug.Log("Deducted 10 from p2Money (p2 cheated on their turn).");
                }
                else
                {
                    PlayerTurn.Instance.ChangeMoney(1, -slapCost);
                    Debug.Log("Deducted 10 from p1Money (p2 did NOT cheat on their turn).");
                }
            }
            // if it's not p2's turn: currently do nothing
        }
    }
}
*/