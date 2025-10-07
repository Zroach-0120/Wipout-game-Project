using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
