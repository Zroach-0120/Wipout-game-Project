using UnityEngine;
using TMPro;

// Attach this script to the Player and tag collectables with "Collect" (& make them triggers)
// ToDo: probably add some logic when you win
public class Score : MonoBehaviour
{
    [Tooltip("Connect the SCORE text object here")]
    public TextMeshProUGUI scoreText;
    [Tooltip("Connect the WIN text object here")]
    public GameObject winText;
    [Tooltip("How many points it takes to show the win text")]
    public int winCondition = 12;
    private int score = 0;

    void Start()
    {
        winText?.SetActive(false);
        scoreText.text = "Points: " + score;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collect"))
            Destroy(other.gameObject);
            score++;
            scoreText.text = "Points: " + score;
            if (score >= winCondition)
            {
                winText?.SetActive(true);
            }
    }
}