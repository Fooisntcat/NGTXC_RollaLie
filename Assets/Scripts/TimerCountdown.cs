using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TimerCountdown
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Text timerText; // Reference to the UI Text component to display the timer
        public int timerDuration = 3;
        public float timeRemaining = 3;
        public bool timerIsRunning = false;

        private void Start()
        {
            // Starts the timer automatically
            timerIsRunning = true;
        }

        void Update()
        {
            if (timerIsRunning)
            {
                // timerText.text = Mathf.Round(timeRemaining).ToString() + "s";
                timerText.text = Mathf.Round(timeRemaining).ToString();

                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    // Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                }
            }
        }

        public void ResetTimer()
        {
            timerIsRunning = true;
            timeRemaining = 3;
            // Debug.Log("Timer reset to " + timeRemaining + " seconds.");
        }
    }
}