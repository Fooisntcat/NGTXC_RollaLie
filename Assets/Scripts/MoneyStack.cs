using UnityEngine;
using System.Collections.Generic;

public class MoneyStack : MonoBehaviour
{
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private float stackHeight = 0.02f;
    private List<GameObject> stack = new List<GameObject>();

    public void UpdateChips(int change)
    {
        if (change > 0)
        {
            // Add chips
            for (int i = 0; i < change; i++)
            {
                Vector3 pos = transform.position + Vector3.up * stack.Count * stackHeight;
                // GameObject chip = Instantiate(chipPrefab, pos, Quaternion.identity, transform);
                GameObject chip = Instantiate(chipPrefab, pos, chipPrefab.transform.rotation, transform);
                stack.Add(chip);
            }
        }
        else if (change < 0)
        {
            // Remove chips
            int removeCount = Mathf.Min(-change, stack.Count);
            for (int i = 0; i < removeCount; i++)
            {
                GameObject chip = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                Destroy(chip);
            }
        }
    }
}


/*
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening; // 👈 Needed for DOTween

public class MoneyStack : MonoBehaviour
{
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private float stackHeight = 0.02f;
    private List<GameObject> stack = new List<GameObject>();

    public void UpdateChips(int change)
    {
        if (change > 0)
        {
            // Add chips with animation
            for (int i = 0; i < change; i++)
            {
                Vector3 targetPos = transform.position + Vector3.up * stack.Count * stackHeight;

                // Spawn off-screen (or to the side) then fly to stack
                Vector3 spawnPos = targetPos + new Vector3(Random.Range(-0.5f, 0.5f), 1f, Random.Range(-0.5f, 0.5f));

                GameObject chip = Instantiate(chipPrefab, spawnPos, Quaternion.Euler(90f, 0, 0), transform);
                stack.Add(chip);

                // Animate to stack position
                chip.transform.DOMove(targetPos, 0.5f)
                    .SetEase(Ease.OutBounce)
                    .SetDelay(i * 0.05f); // staggered effect
            }
        }
        else if (change < 0)
        {
            // Remove chips normally
            int removeCount = Mathf.Min(-change, stack.Count);
            for (int i = 0; i < removeCount; i++)
            {
                GameObject chip = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                // Animate chip flying away before destroying
                chip.transform.DOMove(chip.transform.position + new Vector3(0, 1f, 0), 0.3f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(chip));
            }
        }
    }
}
*/
/*
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class MoneyStack : MonoBehaviour
{
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private float stackHeight = 0.02f;
    private PlayerTurn playerTurn;
    private List<GameObject> stack = new List<GameObject>();

    public void UpdateChips(int change)
    {
        if (change > 0)
        {
            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < change; i++)
            {
                int chipIndex = stack.Count + i;

                seq.AppendCallback(() =>
                {
                    Vector3 targetPos = transform.position + Vector3.up * stack.Count * stackHeight;

                    // Spawn from a random offset above
                    Vector3 spawnPos = targetPos + new Vector3(Random.Range(-0.5f, 0.5f), 1f, Random.Range(-0.5f, 0.5f));

                    GameObject chip = Instantiate(chipPrefab, spawnPos, Quaternion.Euler(90f, 0, 0), transform);
                    stack.Add(chip);

                    // Animate chip into stack
                    chip.transform.DOMove(targetPos, 0.5f)
                        .SetEase(Ease.OutBounce);
                });

                // Wait until the chip finishes before spawning the next
                if (PlayerTurn.Instance.roundsPlayed > 1) seq.AppendInterval(0.55f); 
            }
        }
        else if (change < 0)
        {
            Sequence seq = DOTween.Sequence();

            int removeCount = Mathf.Min(-change, stack.Count);

            for (int i = 0; i < removeCount; i++)
            {
                seq.AppendCallback(() =>
                {
                    GameObject chip = stack[stack.Count - 1];
                    stack.RemoveAt(stack.Count - 1);

                    chip.transform.DOMove(chip.transform.position + new Vector3(0, 1f, 0), 0.3f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => Destroy(chip));
                });

                if (PlayerTurn.Instance.roundsPlayed > 1) seq.AppendInterval(0.35f);
            }
        }
    }
}

*/

/*
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class MoneyStack : MonoBehaviour
{
    [Header("Prefab & Visuals")]
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private float stackHeight = 0.02f;     // vertical spacing between chips
    [SerializeField] private float spawnRadius = 0.5f;     // random spawn radius around stack center
    [SerializeField] private float spawnHeight = 1f;       // spawn height above the table

    [Header("Tween Settings")]
    [SerializeField] private float addDuration = 0.5f;
    [SerializeField] private float transferDuration = 0.6f;
    [SerializeField] private Ease addEase = Ease.OutBounce;
    [SerializeField] private Ease transferEase = Ease.InOutQuad;
    [SerializeField] private float spawnStagger = 0.05f;   // delay between sequential chip animations

    [Header("References")]
    public MoneyStack opponentStack; // set in Inspector
    // [SerializeField] private GameObject opponentStackObject; // optional: assign opponent stack GameObject in Inspector

    // internal stack of chip GameObjects
    [HideInInspector] public List<GameObject> stack = new List<GameObject>();

    /// <summary>
    /// Call this with positive to add chips to THIS stack,
    /// negative to remove chips FROM THIS stack and send them to opponent (if set).
    /// </summary>
    public void UpdateChips(int change)
    {
        if (chipPrefab == null)
        {
            Debug.LogWarning("MoneyStack: chipPrefab is not assigned!");
            return;
        }

        if (change > 0)
            AddChips(change);
        else if (change < 0)
            RemoveAndSendChips(-change);
    }

    private void AddChips(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // compute target slot index and position BEFORE tweening
            int slotIndex = stack.Count; // next free slot
            Vector3 targetPos = transform.position + Vector3.up * slotIndex * stackHeight;

            // random spawn position around this stack
            // Vector3 spawnOffset = new Vector3(
            //     Random.Range(-spawnRadius, spawnRadius),
            //     spawnHeight,
            //     Random.Range(-spawnRadius, spawnRadius)
            // );
            // Vector3 spawnPos = transform.position + spawnOffset;

            Vector3 spawnPos = opponentStack.transform.position;

            // instantiate chip (flat side up: rotate 90 around X; random Y rotation for variation)
            Quaternion spawnRot = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
            GameObject chip = Instantiate(chipPrefab, spawnPos, spawnRot);

            // add immediately so subsequent chips land on top
            stack.Add(chip);

            // capture local var for closure safety
            GameObject localChip = chip;
            int localSlotIndex = slotIndex;

            // animate into place
            localChip.transform.DOMove(targetPos, addDuration)
                .SetEase(addEase)
                .SetDelay(i * spawnStagger)
                .OnComplete(() =>
                {
                    // parent and snap to exact local position based on slot
                    localChip.transform.SetParent(transform);
                    localChip.transform.localPosition = Vector3.up * localSlotIndex * stackHeight;
                    // optionally reset local rotation so chip sits nicely
                    localChip.transform.localRotation = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
                });
        }
    }

    private void RemoveAndSendChips(int amount)
    {
        int removeCount = Mathf.Min(amount, stack.Count);

        if (removeCount == 0)
            return;

        if (opponentStack == null)
        {
            // No opponent: animate chips away and destroy
            for (int i = 0; i < removeCount; i++)
            {
                GameObject chip = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
                GameObject localChip = chip;

                Vector3 awayPos = localChip.transform.position + new Vector3(0, 1f, 0);
                localChip.transform.DOMove(awayPos, 0.3f)
                    .SetEase(Ease.InBack)
                    .SetDelay(i * spawnStagger)
                    .OnComplete(() => Destroy(localChip));
            }

            return;
        }

        // If we have an opponent, compute destination slot indices up-front so chips don't overlap
        int destStartIndex = opponentStack.stack.Count;

        for (int i = 0; i < removeCount; i++)
        {
            // pop last chip from this stack
            GameObject chip = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);

            GameObject localChip = chip;
            int destIndex = destStartIndex + i;

            // compute world target position for opponent slot destIndex
            Vector3 targetPos = opponentStack.transform.position + Vector3.up * destIndex * stackHeight;

            // unparent so the tween can run in world space cleanly
            localChip.transform.SetParent(null);

            // make it jump/arc to opponent target
            localChip.transform
                .DOJump(targetPos, 0.5f, 1, transferDuration)
                .SetEase(transferEase)
                .SetDelay(i * spawnStagger)
                .OnComplete(() =>
                {
                    // when it lands, add to opponent list and reparent
                    opponentStack.stack.Add(localChip);
                    localChip.transform.SetParent(opponentStack.transform);

                    // snap to the calculated destination local position (based on destIndex)
                    localChip.transform.localPosition = Vector3.up * destIndex * stackHeight;

                    // re-orient slightly
                    localChip.transform.localRotation = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
                });
        }
    }

    // Optional helper to instantly set stack visual to match a count (useful for init)
    public void SetChipsVisualCount(int count)
    {
        // clear existing
        foreach (var c in stack) if (c != null) Destroy(c);
        stack.Clear();

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = transform.position + Vector3.up * i * stackHeight;
            Quaternion rot = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
            GameObject chip = Instantiate(chipPrefab, pos, rot, transform);
            chip.transform.localPosition = Vector3.up * i * stackHeight;
            stack.Add(chip);
        }
    }
}
*/

/*
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class MoneyStack : MonoBehaviour
{
    [Header("Prefab & Visuals")]
    [SerializeField] private GameObject chipPrefab;
    [SerializeField] private float stackHeight = 0.02f;   // vertical spacing between chips

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 0.5f;
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private float spawnStagger = 0.05f;

    [Header("Tween Settings")]
    [SerializeField] private float addDuration = 0.5f;
    [SerializeField] private float transferDuration = 0.6f;
    [SerializeField] private float transferJumpPower = 0.5f;
    [SerializeField] private Ease addEase = Ease.OutBounce;
    [SerializeField] private Ease transferEase = Ease.InOutQuad;

    [Header("References")]
    public MoneyStack opponentStack; // set in Inspector

    // public so other scripts can inspect for debugging if needed
    [HideInInspector] public List<GameObject> stack = new List<GameObject>();

    public void UpdateChips(int change)
    {
        if (chipPrefab == null)
        {
            Debug.LogWarning("MoneyStack: chipPrefab is not assigned.");
            return;
        }

        if (change > 0) AddChips(change);
        else if (change < 0) TransferChipsToOpponent(-change);
    }

    private void AddChips(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int slotIndex = stack.Count; // next free slot on THIS stack
            Vector3 targetPos = transform.position + Vector3.up * slotIndex * stackHeight;

            Vector3 spawnOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                spawnHeight,
                Random.Range(-spawnRadius, spawnRadius)
            );
            Vector3 spawnPos = transform.position + spawnOffset;

            Quaternion spawnRot = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
            GameObject chip = Instantiate(chipPrefab, spawnPos, spawnRot);

            // add immediately so later chips calc correct slots
            stack.Add(chip);

            // local copies for closure safety
            GameObject localChip = chip;
            int localSlot = slotIndex;

            // animate into place, then parent & snap local position
            localChip.transform.DOMove(targetPos, addDuration)
                .SetEase(addEase)
                .SetDelay(i * spawnStagger)
                .OnComplete(() =>
                {
                    localChip.transform.SetParent(transform);
                    localChip.transform.localPosition = Vector3.up * localSlot * stackHeight;
                    localChip.transform.localRotation = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
                });
        }
    }

    private void TransferChipsToOpponent(int amount)
    {
        if (opponentStack == null)
        {
            // no opponent: fallback to removing & destroy animation
            RemoveAndDestroy(amount);
            return;
        }

        int transferCount = Mathf.Min(amount, stack.Count);
        if (transferCount == 0) return;

        // destination start index on opponent (pre-calc so slots are reserved)
        int destStartIndex = opponentStack.stack.Count;

        for (int i = 0; i < transferCount; i++)
        {
            // take last chip from THIS stack
            GameObject chip = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);

            GameObject localChip = chip;
            int destIndex = destStartIndex + i;
            Vector3 destPos = opponentStack.transform.position + Vector3.up * destIndex * stackHeight;

            // optionally disable collider/rigidbody so tween is clean
            var col = localChip.GetComponent<Collider>();
            if (col) col.enabled = false;
            var rb = localChip.GetComponent<Rigidbody>();
            if (rb) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; }

            // unparent so we animate in world space
            localChip.transform.SetParent(null);

            // Tween: DOJump to create an arc. add small random offset so chips don't perfectly overlap
            Vector3 destWithVariance = destPos + new Vector3(Random.Range(-0.01f, 0.01f), 0f, Random.Range(-0.01f, 0.01f));

            localChip.transform
                .DOJump(destWithVariance, transferJumpPower, 1, transferDuration)
                .SetEase(transferEase)
                .SetDelay(i * spawnStagger)
                .OnComplete(() =>
                {
                    // Add to opponent stack list and reparent
                    opponentStack.stack.Add(localChip);
                    localChip.transform.SetParent(opponentStack.transform);

                    // Snap exactly into its local stack slot (important if slight variance was used)
                    localChip.transform.localPosition = Vector3.up * destIndex * stackHeight;
                    localChip.transform.localRotation = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);

                    // re-enable physics/collider if needed
                    if (rb) { rb.isKinematic = false; }
                    if (col) col.enabled = true;
                });
        }
    }

    private void RemoveAndDestroy(int amount)
    {
        int removeCount = Mathf.Min(amount, stack.Count);
        for (int i = 0; i < removeCount; i++)
        {
            GameObject chip = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);

            GameObject localChip = chip;

            localChip.transform.DOMove(localChip.transform.position + Vector3.up * 1f, 0.3f)
                .SetEase(Ease.InBack)
                .SetDelay(i * spawnStagger)
                .OnComplete(() => Destroy(localChip));
        }
    }

    // helper: instantly set visuals to X chips (for initialization)
    public void SetChipsVisualCount(int count)
    {
        // destroy existing visually
        for (int i = stack.Count - 1; i >= 0; i--) { if (stack[i] != null) Destroy(stack[i]); }
        stack.Clear();

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = transform.position + Vector3.up * i * stackHeight;
            Quaternion rot = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
            GameObject chip = Instantiate(chipPrefab, pos, rot, transform);
            chip.transform.localPosition = Vector3.up * i * stackHeight;
            stack.Add(chip);
        }
    }
}
*/