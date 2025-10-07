using UnityEngine;
using TMPro;


public class HealthSystem : MonoBehaviour
{
    private Rigidbody rb;           // reference to the Rigidbody
    private float coolDown = 1.5f;
    public int playerHealth = 3;
    public GameObject LoseTextObject;
    public TextMeshProUGUI HealthText;
    void Start()
    {
        SetCountText();
        rb = GetComponent<Rigidbody>();
        LoseTextObject.SetActive(false);
    }

    void Update()
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }
        else
        {
            coolDown = 0;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            if (playerHealth <= 0)
                LoseTextObject.SetActive(true);
            else if (coolDown <= 0)
                playerHealth -= 1;
        }
        SetCountText();
    }
    void SetCountText() 
    {
        HealthText.text = "Health: " + playerHealth.ToString();
    }
}
