using TiltFive;
using UnityEngine;

public class handToWandLocation : MonoBehaviour
{
    [SerializeField] private Transform wandLocation;
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;
    private PlayerTurn playerTurn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
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
}
