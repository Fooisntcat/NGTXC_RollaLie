/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TiltFive;

public class gameWinner : MonoBehaviour
{
    [SerializeField] private Text P1Stats;
    [SerializeField] private Text P2Stats;
    [SerializeField] private AudioSource player1Won;
    [SerializeField] private AudioSource player2Won;
    [SerializeField] private AudioSource gameTied;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerTurn.GameWinner == 1)
        {
            P1Stats.text = "<color=lime> Player 1 Wins! </color>";
            P2Stats.text = "<color=red> Player 2 Loses! </color>";
            player1Won.Play();
            TiltFive.Wand.TrySendImpulse(1f, 1f, PlayerIndex.One, ControllerIndex.Right);
        }
        else if (PlayerTurn.GameWinner == 2)
        {
            P1Stats.text = "<color=red> Player 1 Loses! </color>";
            P2Stats.text = "<color=lime> Player 2 Wins! </color>";
            player2Won.Play();
            TiltFive.Wand.TrySendImpulse(1f, 1f, PlayerIndex.Two, ControllerIndex.Right);
        }
        else if (PlayerTurn.GameWinner == 0)
        {
            P1Stats.text = "<color=yellow> Player 1 Tied! </color>";
            P2Stats.text = "<color=yellow> Player 2 Tied! </color>";
            gameTied.Play();
        }
        else
        {
            P1Stats.text = "<color=gray> Player 1 No Result! </color>";
            P2Stats.text = "<color=gray> Player 2 No Result! </color>";
        }
    }

    
}
*/

/* using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TiltFive;

public class GameWinner : MonoBehaviour
{
    [SerializeField] private Text P1Stats;
    [SerializeField] private Text P2Stats;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource player1Won;
    [SerializeField] private AudioSource player2Won;
    [SerializeField] private AudioSource gameTied;

    // [Header("Settings")]
    // [SerializeField] private string mainMenuSceneName = "MainMenu";

    private AudioSource activeAudio;

    void Start()
    {
        // Check winner from your PlayerTurn system
        if (PlayerTurn.GameWinner == 1)
        {
            P1Stats.text = "<color=lime> Player 1 Wins! </color>";
            P2Stats.text = "<color=red> Player 2 Loses! </color>";
            activeAudio = player1Won;
            activeAudio.Play();

            // Haptic feedback for Player 1
            Wand.TrySendImpulse(1f, 1f, PlayerIndex.One, ControllerIndex.Right);
        }
        else if (PlayerTurn.GameWinner == 2)
        {
            P1Stats.text = "<color=red> Player 1 Loses! </color>";
            P2Stats.text = "<color=lime> Player 2 Wins! </color>";
            activeAudio = player2Won;
            activeAudio.Play();

            // Haptic feedback for Player 2
            Wand.TrySendImpulse(1f, 1f, PlayerIndex.Two, ControllerIndex.Right);
        }
        else if (PlayerTurn.GameWinner == 0)
        {
            P1Stats.text = "<color=yellow> Player 1 Tied! </color>";
            P2Stats.text = "<color=yellow> Player 2 Tied! </color>";
            activeAudio = gameTied;
            activeAudio.Play();
        }
        else
        {
            P1Stats.text = "<color=gray> Player 1 No Result! </color>";
            P2Stats.text = "<color=gray> Player 2 No Result! </color>";
        }

        // Start waiting for player input to go back to menu
        StartCoroutine(WaitForInput());
    }

    private IEnumerator WaitForInput()
    {
        // If an outro clip is playing, wait until it finishes
        if (activeAudio != null)
        {
            yield return new WaitWhile(() => activeAudio.isPlaying);
        }

        // Now wait for any button or key press
        // while (!TiltFive.Input.anyKeyDown)
        while (!UnityEngine.Input.GetKeyDown(KeyCode.Space) ||
            UnityEngine.Input.GetButtonDown("Submit") ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.X, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.X, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.Two) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Two, ControllerIndex.Right, PlayerIndex.One) ||
            TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Two, ControllerIndex.Right, PlayerIndex.Two))
        {
            yield return null;
        }

        Destroy(GameObject.Find("BG Music"));
        SceneManager.LoadScene("MainMenu");
        // SceneManager.LoadScene(mainMenuSceneName);
    }
}
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TiltFive;

public class GameWinner : MonoBehaviour
{
    [SerializeField] private Text P1Stats;
    [SerializeField] private Text P2Stats;
    [SerializeField] private Text roundsPlayedText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;   // One AudioSource
    [SerializeField] private AudioClip player1WinClip;  // Clips instead of separate sources
    [SerializeField] private AudioClip player2WinClip;
    [SerializeField] private AudioClip tieClip;

    private AudioClip activeClip;

    void Start()
    {
        // if (roundsPlayedText != null) return;
        roundsPlayedText.text = $"Rounds Played: {PlayerTurn.Instance.roundsPlayed}";
        if (PlayerTurn.GameWinner == 1)
        {
            P1Stats.text = "<color=lime> Player 1 Wins! </color>";
            P2Stats.text = "<color=red> Player 2 Loses! </color>";
            activeClip = player1WinClip;
            audioSource.PlayOneShot(activeClip);

            // Haptic feedback for Player 1
            Wand.TrySendImpulse(1f, 1f, PlayerIndex.One, ControllerIndex.Right);
        }
        else if (PlayerTurn.GameWinner == 2)
        {
            P1Stats.text = "<color=red> Player 1 Loses! </color>";
            P2Stats.text = "<color=lime> Player 2 Wins! </color>";
            activeClip = player2WinClip;
            audioSource.PlayOneShot(activeClip);

            // Haptic feedback for Player 2
            Wand.TrySendImpulse(1f, 1f, PlayerIndex.Two, ControllerIndex.Right);
        }
        else if (PlayerTurn.GameWinner == 0)
        {
            P1Stats.text = "<color=yellow> Player 1 Tied! </color>";
            P2Stats.text = "<color=yellow> Player 2 Tied! </color>";
            activeClip = tieClip;
            audioSource.PlayOneShot(activeClip);
        }
        else
        {
            P1Stats.text = "<color=gray> Player 1 No Result! </color>";
            P2Stats.text = "<color=gray> Player 2 No Result! </color>";
        }

        StartCoroutine(WaitForInput());
    }

    private IEnumerator WaitForInput()
    {
        // Wait for clip to finish (if any)
        if (activeClip != null)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        yield return new WaitForSeconds(5f);

        // Wait until any button or key press
        while (!UnityEngine.Input.anyKeyDown &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.One) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.A, ControllerIndex.Right, PlayerIndex.Two) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.One) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.B, ControllerIndex.Right, PlayerIndex.Two) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.X, ControllerIndex.Right, PlayerIndex.One) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.X, ControllerIndex.Right, PlayerIndex.Two) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.One) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Y, ControllerIndex.Right, PlayerIndex.Two) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.One) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.One, ControllerIndex.Right, PlayerIndex.Two) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Two, ControllerIndex.Right, PlayerIndex.One) &&
               !TiltFive.Input.GetButtonDown(TiltFive.Input.WandButton.Two, ControllerIndex.Right, PlayerIndex.Two))
        {
            yield return null;
        }

        Destroy(GameObject.Find("BG Music"));
        SceneManager.LoadScene("MainMenu");
    }
}

