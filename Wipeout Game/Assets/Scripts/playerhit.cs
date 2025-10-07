using UnityEngine;

public class CapsuleHitReaction : MonoBehaviour
{
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;

    private Rigidbody rb;
    private bool isRagdoll = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // controlled by script normally
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            if (!isRagdoll)
            {
                SwitchToThirdPerson();
                TriggerKnockback(collision);
            }
        }
    }

    void SwitchToThirdPerson()
    {
        firstPersonCam.SetActive(false);
        thirdPersonCam.SetActive(true);
    }

    void TriggerKnockback(Collision col)
    {
        isRagdoll = true;
        rb.isKinematic = false; // physics takes over

        // Apply knockback force away from the hit
        Vector3 forceDir = (transform.position - col.transform.position).normalized;
        rb.AddForce(forceDir * 15f, ForceMode.Impulse);
    }
}
