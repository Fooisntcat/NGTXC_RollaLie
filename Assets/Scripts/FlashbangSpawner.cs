using UnityEngine;

public class SpawnOnEmpty : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // prefab
    [SerializeField] private Transform spawnPoint;     // where to spawn
    [SerializeField] private string targetTag = "Flashbang"; // tag of object to detect
    [SerializeField] private float waitTime = 3f;

    private bool isInside = false;
    private float timer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isInside = true;
            timer = 0f; // reset timer
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            isInside = false;
            timer = 0f; // start counting when it leaves
        }
    }

    private void Update()
    {
        if (!isInside)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
                timer = 0f; // reset so it won’t keep spawning repeatedly
            }
        }
    }
}
