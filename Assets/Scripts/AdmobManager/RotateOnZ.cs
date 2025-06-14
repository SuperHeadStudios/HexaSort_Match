using UnityEngine;

public class RotateOnZ : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Transform rotator;

    void Update()
    {
        rotator.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
