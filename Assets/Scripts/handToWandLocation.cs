using TiltFive;
using UnityEngine;

public class handToWandLocation : MonoBehaviour
{
    [SerializeField] private Transform wandLocation;
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;
    [SerializeField] private AudioSource slap;
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
            transform.position = wandLocation.TransformPoint(new Vector3(-0.5f, 0.5f, -1.5f));

            transform.rotation = wandLocation.rotation * Quaternion.Euler(-90f, 0f, 90f);
            // transform.rotation = wandLocation.rotation;
            Animator animator = GetComponent<Animator>();
            if (animator != null || UnityEngine.Input.GetKey("return"))
            {
                animator.SetBool("handClose", trigger > 0.8f);

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
        // Debug.Log("Collision Hand");
        bool p1Cheat = diceThrower.p1Cheat;
        bool p2Cheat = diceThrower.p2Cheat;
        if (other.CompareTag("Hand"))
        // if (other.CompareTag("p1Hand") || other.CompareTag("p2Hand"))
        {
            slap.Play();
            Debug.Log("Slap hand detected!");
            // wand.Input(0.5f, 0.2f);
            TiltFive.Wand.TrySendImpulse(0.5f, 1f, PlayerIndex.One, ControllerIndex.Right);
            TiltFive.Wand.TrySendImpulse(0.5f, 1f, PlayerIndex.Two, ControllerIndex.Right);
            if (p1Cheat == true)
            {
                // slap.Play();
                playerTurn.p1Money -= 10; // Deduct 10 from Player 1's money

            }
            else if (p2Cheat == true)
            {
                // slap.Play();
                playerTurn.p2Money -= 10; // Deduct 10 from Player 2's money
            }
        }
    }
}
