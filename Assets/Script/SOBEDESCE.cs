using UnityEngine;

public class    SOBEDESCE : MonoBehaviour
{
    public float speed = 1.0f;  // Speed of the movement
    public float distance = 1.0f;  // Distance to move in the Z-axis

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float z = Mathf.Sin(Time.time * speed) * distance;
        transform.position = startPos + new Vector3(0, 0, z);
    }
}
