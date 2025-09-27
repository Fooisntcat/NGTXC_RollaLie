#region Raycast in game
/* Raycast in game (very buggy)
using UnityEngine;
using TiltFive;

[RequireComponent(typeof(LineRenderer))]
public class WandLaser : MonoBehaviour
{
    private Rigidbody grabbedRb;             // current flashbang held
    public Transform wandHoldPoint;          // empty child where object snaps
    public float rayLength = 2000f;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2; // start + end
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = Color.red;
    }

    void Update()
    {
        if (!TiltFive.Wand.IsTracked())
        {
            lr.enabled = false;
            return;
        }

        float trigger = TiltFive.Input.GetTrigger();

        // Always draw the laser
        lr.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Vector3 endPos = transform.position + transform.forward * rayLength;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            endPos = hit.point;

            // Laser turns green if pointing at flashbang
            lr.material.color = hit.collider.CompareTag("Flashbang") ? Color.green : Color.red;

            // Grab object when trigger pressed
            if (trigger > 0.8f)
            {
                if (grabbedRb == null && hit.collider.CompareTag("Flashbang"))
                {
                    grabbedRb = hit.collider.attachedRigidbody;
                    if (grabbedRb != null)
                    {
                        grabbedRb.isKinematic = true;
                        grabbedRb.transform.position = wandHoldPoint.position;
                        grabbedRb.transform.rotation = wandHoldPoint.rotation;
                        grabbedRb.transform.SetParent(wandHoldPoint);
                    }
                }
            }
        }

        // If holding one, keep it stuck
        if (grabbedRb != null && trigger > 0.8f)
        {
            grabbedRb.transform.position = wandHoldPoint.position;
            grabbedRb.transform.rotation = wandHoldPoint.rotation;
        }
        else if (grabbedRb != null && trigger <= 0.8f)
        {
            // Release
            grabbedRb.isKinematic = false;
            grabbedRb.transform.SetParent(null);
            grabbedRb = null;
        }

        // Update line renderer positions
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, endPos);
    }
}
*/
#endregion
#region Raycast only scene preview
/* Raycast Old Method (no ray in game)
using UnityEngine;
using TiltFive;

public class WandRaycast : MonoBehaviour
{
    private Rigidbody grabbedRb; // the flashbang currently held
    public Transform wandHoldPoint; // empty child object where the flashbang attaches

    void Update()
    {
        if (!TiltFive.Wand.IsTracked()) return;

        float trigger = TiltFive.Input.GetTrigger();

        // If holding trigger strongly
        if (trigger > 0.8f)
        {
            if (grabbedRb == null)
            {
                // Raycast forward
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2000f))
                {
                    if (hit.collider.CompareTag("Flashbang"))
                    {
                        // Grab the flashbang
                        grabbedRb = hit.collider.attachedRigidbody;
                        if (grabbedRb != null)
                        {
                            // Disable physics so it follows cleanly
                            grabbedRb.isKinematic = true;

                            // Snap to hold point
                            grabbedRb.transform.position = wandHoldPoint.position;
                            grabbedRb.transform.rotation = wandHoldPoint.rotation;

                            // Parent it to the wand
                            grabbedRb.transform.SetParent(wandHoldPoint);
                        }
                    }
                }
            }
            // If already holding one, keep it stuck to hold point
            else
            {
                grabbedRb.transform.position = wandHoldPoint.position;
                grabbedRb.transform.rotation = wandHoldPoint.rotation;
            }
        }
        else
        {
            // Release if holding one
            if (grabbedRb != null)
            {
                grabbedRb.isKinematic = false; // physics on again
                grabbedRb.transform.SetParent(null);
                grabbedRb = null;
            }
        }
    }
}
*/
#endregion
#region Using collision (press trigger then enter)
/*
using UnityEngine;
using TiltFive; // don’t forget namespace

public class WandCollisionPickup : MonoBehaviour
{
    private Transform objectToDrag;   // current flashbang being held
    public Transform wandHoldPoint;   // empty child object on wand (where flashbang sticks)
    private bool isHolding = false;

    void Update()
    {
        float trigger = TiltFive.Input.GetTrigger();

        // If holding, keep the flashbang stuck to wand
        if (isHolding && objectToDrag != null)
        {
            objectToDrag.position = wandHoldPoint.position;
            objectToDrag.rotation = wandHoldPoint.rotation;

            // Release when trigger is let go
            if (trigger < 0.2f)
            {
                Rigidbody rb = objectToDrag.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;

                objectToDrag = null;
                isHolding = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float trigger = TiltFive.Input.GetTrigger();

        // Grab when trigger pressed while colliding with a flashbang
        if (other.CompareTag("Flashbang") && !isHolding && trigger > 0.8f)
        {
            objectToDrag = other.transform;
            isHolding = true;

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) rb.isKinematic = true;
        }
    }
}
*/
#endregion
#region Using collision (OnTriggerStay)
/*
using UnityEngine;
using TiltFive; // don’t forget namespace

public class WandCollisionPickup : MonoBehaviour
{
    private Transform objectToDrag;   // current flashbang being held
    public Transform wandHoldPoint;   // empty child object on wand (where flashbang sticks)
    private bool isHolding = false;

    void Update()
    {
        float trigger = TiltFive.Input.GetTrigger();

        // If holding, keep the flashbang stuck to wand
        if (isHolding && objectToDrag != null)
        {
            objectToDrag.position = wandHoldPoint.position;
            objectToDrag.rotation = wandHoldPoint.rotation;

            // Release when trigger is let go
            if (trigger < 0.2f)
            {
                Rigidbody rb = objectToDrag.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;

                objectToDrag = null;
                isHolding = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        float trigger = TiltFive.Input.GetTrigger();

        // Grab only when touching AND trigger is pressed
        if (!isHolding && trigger > 0.8f && other.CompareTag("Flashbang"))
        {
            objectToDrag = other.transform;
            isHolding = true;

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) rb.isKinematic = true;
        }
    }
}
*/
#endregion
#region Using collision (rb.movement)
/*
using UnityEngine;
using TiltFive;  // make sure this is included for PlayerIndex

public class FlashbangGrab : MonoBehaviour
{
    private Transform objectToDrag;
    private Rigidbody objectRb;
    private bool isHolding = false;

    [SerializeField] private Transform wandHoldPoint;
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One; // set in inspector for each wand

    void FixedUpdate()
    {
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right, playerIndex);

        if (isHolding && objectToDrag != null && objectRb != null)
        {
            // Follow wand with physics
            objectRb.MovePosition(wandHoldPoint.position);
            objectRb.MoveRotation(wandHoldPoint.rotation);

            // Release when trigger is let go
            if (trigger < 0.2f)
            {
                objectRb.isKinematic = false;

                objectToDrag = null;
                objectRb = null;
                isHolding = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right,playerIndex);

        // Grab only when touching AND trigger is pressed
        if (!isHolding && trigger > 0.8f && other.CompareTag("Flashbang"))
        {
            objectToDrag = other.transform;
            objectRb = other.attachedRigidbody;

            // if (objectRb != null)
                // objectRb.isKinematic = true;

            isHolding = true;
        }
    }
}
*/
#endregion
#region Using collision (rb.movement Fixed?)
using UnityEngine;
using TiltFive;
using Unity.VisualScripting;
using UnityEngine.UI;

public class FlashbangGrab : MonoBehaviour
{
    private Rigidbody objectRb;
    private bool isHolding = false;
    private PlayerTurn playerTurn;

    [SerializeField] private Transform wandHoldPoint;
    [SerializeField] private PlayerIndex playerIndex = PlayerIndex.One;
    [SerializeField] private int price_flashbang = 5;
    [SerializeField] private int price_dicereroll = 10;
    [SerializeField] private int price_pumpndump = 20;

    [SerializeField] private TextController textController;
    // [SerializeField] private TextController diceRerollDescription;
    // [SerializeField] private TextController flashbangDescription;
    private TextController TextController;
    private SpawnerScript spawnScript;

    void Awake()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
        spawnScript = FindFirstObjectByType<SpawnerScript>();
        textController = FindFirstObjectByType<TextController>();
    }
    void FixedUpdate()
    {
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right, playerIndex);

        // objectRb.isKinematic = false; // allow physics again

        if (isHolding && objectRb != null)
        {
            // Move with physics
            objectRb.MovePosition(wandHoldPoint.position);
            objectRb.MoveRotation(wandHoldPoint.rotation);

            // Release when trigger is let go
            if (trigger < 0.2f)
            {
                objectRb.useGravity = true;   // re-enable gravity
                objectRb.isKinematic = false; // allow physics again
                objectRb = null;
                isHolding = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right, playerIndex);

        // Grab only when touching AND trigger is pressed
        // if (!isHolding && (trigger > 0.8f || UnityEngine.Input.GetKey(KeyCode.R)) && other.CompareTag("Flashbang"))
        if (other.CompareTag("Flashbang"))
        {
            // flashbangDescription.ShowText(); // Show description when hovering over flashbang
            textController.ShowText("flashbang", playerIndex == PlayerIndex.One ? 1 : 2);
            if (!isHolding && (trigger > 0.8f || UnityEngine.Input.GetKey(KeyCode.R)))
            {
                if (playerIndex == PlayerIndex.One) // Only allow player 1 to grab
                {
                    // playerTurn.p1Money -= price_flashbang; // Deduct money for player 1
                    PlayerTurn.Instance.ChangeMoney(1, -price_flashbang);
                    // Debug.Log($"Player 1 grabbed flashbang, remaining money: {playerTurn.p1Money}");
                    // textController.ShowText("flashbang", 1);
                }
                else if (playerIndex == PlayerIndex.Two) // Only allow player 2 to grab
                {
                    PlayerTurn.Instance.ChangeMoney(2, -price_flashbang);
                    // Debug.Log($"Player 2 grabbed flashbang, remaining money: {playerTurn.p2Money}");
                    // textController.ShowText("flashbang", 2);
                }

                objectRb = other.attachedRigidbody;

                if (objectRb != null)
                {
                    objectRb.useGravity = false;   // prevent falling while held
                }

                isHolding = true;
            }
        }

        if (other.CompareTag("diceReroll"))
        {
            // diceRerollDescription.ShowText(); // Show description when hovering over dice reroll
            textController.ShowText("diceReroll", playerIndex == PlayerIndex.One ? 1 : 2);
            if (!isHolding && (trigger > 0.8f || UnityEngine.Input.GetKey(KeyCode.T)))
            {
                if (playerIndex == PlayerIndex.One) // Only allow player 1 to grab
                {
                    PlayerTurn.Instance.ChangeMoney(1, -price_dicereroll);
                    // Debug.Log($"Player 1 grabbed dice reroll, remaining money: {playerTurn.p1Money}");
                    // textController.ShowText("diceReroll", 1);
                }
                else if (playerIndex == PlayerIndex.Two) // Only allow player 2 to grab
                {
                    PlayerTurn.Instance.ChangeMoney(2, -price_dicereroll);
                    // Debug.Log($"Player 2 grabbed dice reroll, remaining money: {playerTurn.p2Money}");
                    // textController.ShowText("diceReroll", 2);
                }

                objectRb = other.attachedRigidbody;

                if (objectRb != null)
                {
                    objectRb.useGravity = false;   // prevent falling while held
                }

                isHolding = true;
            }
        }
        if (other.CompareTag("PumpNDump"))
        {
            textController.ShowText("PumpNDump", playerIndex == PlayerIndex.One ? 1 : 2); // Show description when hovering over PumpNDump
            if (!isHolding && (trigger > 0.8f || UnityEngine.Input.GetKey(KeyCode.T)))
            {
                if (playerIndex == PlayerIndex.One) // Only allow player 1 to grab
                {
                    // playerTurn.p1Money -= price_pumpndump; // Deduct money for player 1
                    PlayerTurn.Instance.ChangeMoney(1, -price_pumpndump);
                    PlayerTurn.Instance.PumpNDump(1);
                    // Debug.Log($"Player 1 grabbed PumpNDump, remaining money: {playerTurn.p1Money}");
                    // textController.ShowText("PumpNDump", 1);
                }
                else if (playerIndex == PlayerIndex.Two) // Only allow player 2 to grab
                {
                    PlayerTurn.Instance.ChangeMoney(2, -price_pumpndump);
                    PlayerTurn.Instance.PumpNDump(2);
                    // Debug.Log($"Player 2 grabbed PumpNDump, remaining money: {playerTurn.p2Money}");
                    // textController.ShowText("PumpNDump", 2);
                }

                objectRb = other.attachedRigidbody;

                if (objectRb != null)
                {
                    objectRb.useGravity = false;   // prevent falling while held
                }

                isHolding = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // if (textController != null) textController.HideText(); // Hide description when not hovering over PumpNDump
        // if (flashbangDescription != null) flashbangDescription.HideText(); // Hide description when not hovering over flashbang
        // if (diceRerollDescription != null) diceRerollDescription.HideText(); // Hide description when not hovering over dice reroll
        float trigger = TiltFive.Input.GetTrigger(ControllerIndex.Right, playerIndex);
        // if ((other.CompareTag("Flashbang") || other.CompareTag("diceReroll") || other.CompareTag("PumpNDump")) && trigger < 0.2f && isHolding)
        if (other.CompareTag("Flashbang") || other.CompareTag("diceReroll") || other.CompareTag("PumpNDump"))
        {
            Debug.Log("Exited trigger");
            textController.ShowText("", playerIndex == PlayerIndex.One ? 1 : 2);
            // textController.HideText();

            // spawnScript = FindFirstObjectByType<SpawnerScript>();
            // spawnScript.itemBought = false; // Set item bought to false when exiting trigger
            // textController.HideText();
            // spawnScript = FindFirstObjectByType<SpawnerScript>();
            if (spawnScript != null)
            {
                spawnScript.itemBought = false;
            }

            isHolding = false;
            objectRb = null;

        }
    }
}
#endregion