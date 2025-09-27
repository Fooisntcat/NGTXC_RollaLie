using UnityEngine;
using TiltFive;

public class BGMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    void Update()
    {
        if (TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.One) || 
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.Two) || 
            UnityEngine.Input.GetKeyDown(KeyCode.B)) // Added keyboard support for testing in editor
        {
            audioSource.mute = !audioSource.mute;
        }
    }
}

