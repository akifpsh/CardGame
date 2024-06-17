using UnityEngine;

public class RandomRotations : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float maxAngle = 10f; // Maksimum açý deðiþikliði

    private Quaternion targetRotation;
    private float timeSinceLastChange = 0f;
    private float changeInterval = 1f; // Deðiþiklik aralýðý

    void Start()
    {
        SetRandomRotation();
    }

    void Update()
    {
        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange > changeInterval)
        {
            SetRandomRotation();
            timeSinceLastChange = 0f;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void SetRandomRotation()
    {
        // Her eksende rastgele küçük bir açý deðiþikliði belirleyin
        float xRotation = Random.Range(-maxAngle, maxAngle);
        float yRotation = Random.Range(-maxAngle, maxAngle);
        float zRotation = Random.Range(-maxAngle, maxAngle);
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(xRotation, yRotation, zRotation));
    }
}
