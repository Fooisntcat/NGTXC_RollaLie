using TiltFive;
using UnityEngine;

public class handToWandLocation : MonoBehaviour
{
    [SerializeField] private Transform wandLocation;
    private PlayerTurn playerTurn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTurn = FindFirstObjectByType<PlayerTurn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wandLocation != null)
        {
            transform.position = wandLocation.position;
            transform.rotation = wandLocation.rotation;
        }
    }
}
