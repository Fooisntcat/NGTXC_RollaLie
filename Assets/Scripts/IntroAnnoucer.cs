using UnityEngine;
using TiltFive;

public class IntroAnnouncer : MonoBehaviour
{
    [SerializeField] private AudioSource introAudio;

    public bool IntroAnnouncerhasPlayed = false;

    void Start()
    {
        IntroAnnouncerhasPlayed = false;
    }
    void Update()
    {
        if (!IntroAnnouncerhasPlayed)
        {
            Pose pose;

            // Check Player One's glasses
            bool isTracking = Glasses.TryGetPose(PlayerIndex.One, out pose);

            if (isTracking)
            {
                introAudio.Play();
                IntroAnnouncerhasPlayed = true;
            }
        }
    }
}
