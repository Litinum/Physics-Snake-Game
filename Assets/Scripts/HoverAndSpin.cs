using UnityEngine;

public class HoverAndSpin : MonoBehaviour
{
    [SerializeField] float bounceSpeed = 2f;
    [SerializeField] float bounceAmplitude = 0.05f;
    [SerializeField] float rotationSpeed = 30f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    
    void Update()
    {
        float newY = originalPosition.y = (Mathf.Sin(Time.time * bounceSpeed) * bounceAmplitude);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
