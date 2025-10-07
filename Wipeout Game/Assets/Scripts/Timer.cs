using UnityEngine;
using TMPro;

// Attach this script to the player
// ToDo: add some logic to when time runs out
public class Timer : MonoBehaviour
{
    [Tooltip("Sets the timer length")]
    public float timerLength = 0;
    [Tooltip("Connect the Timer text object here")]
    public TextMeshProUGUI timerText;
    private float currentTime = 0;
    private bool timerActive = true;

    void Start()
    {
        currentTime = timerLength;
    }

    void Update()
    {
        if (timerActive)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                timerText.text = "Seconds: " + string.Format("{0:F2}", currentTime);
            }
            else
            {
                currentTime = 0;
                timerActive = false;
                timerText.text = "Out Of Time!";
            }
        }
    }
}
