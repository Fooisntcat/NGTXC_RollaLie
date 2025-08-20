using UnityEngine;

public class SpawnOnEmpty : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // prefab
    [SerializeField] private Transform spawnPoint;     // where to spawn
    [SerializeField] private string targetTag; // tag of object to detect
    [SerializeField] private float waitTime = 3f;

    private bool itemBought = false;
    private bool isInside = false;
    private float timer = 0f;
    [SerializeField]private AudioSource kaching;

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
            if (!itemBought)
            {
                kaching.Play();
            }
            itemBought = true;
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
                itemBought = false;
                timer = 0f; // reset so it won’t keep spawning repeatedly
            }
        }
    }
}
