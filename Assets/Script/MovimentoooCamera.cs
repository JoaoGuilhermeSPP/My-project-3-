using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoooCamera : MonoBehaviour
{
    public float scrollSpeed = 10f;
    public float panSpeed = 1f;
    public Vector2 panLimitX = new Vector2(-50f, 50f); // X-axis movement limits
    public Vector2 panLimitY = new Vector2(-50f, 50f); // Y-axis movement limits
    public float zoomSpeed = 20f;
    public float zoomMin = -40f;
    public float zoomMax = 10f;
    public float smoothTime = 0.2f; // Time to smooth the movement
    public float doubleClickZoomSpeed = 5f;
    public float doubleClickZoomAmount = 15f;
    private bool isPanning;
    private Vector2 initialMousePos;
    private Vector3 initialCameraPos;
    private Vector3 targetCameraPos;
    private Vector3 velocity = Vector3.zero;
    private float lastClickTime = 0f;
    private float doubleClickTime = 0.3f;

    void Start()
    {
        targetCameraPos = transform.position;
    }

    void Update()
    {
        HandleZoom();
        if (Input.GetMouseButtonDown(2))
            StartPanning();
        if (isPanning)
            UpdateCameraPosition();
        else
            SmoothCameraPosition();
        if (Input.GetMouseButtonUp(2))
            StopPanning();

        HandleDoubleClickZoom();
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        Vector3 newPos = transform.position;
        newPos.z = Mathf.Clamp(newPos.z + scroll, zoomMin, zoomMax);
        transform.position = newPos;
    }

    void StartPanning()
    {
        initialMousePos = Input.mousePosition;
        initialCameraPos = transform.position;
        isPanning = true;
    }

    void UpdateCameraPosition()
    {
        Vector2 mouseDelta = (Vector2)Input.mousePosition - initialMousePos;
        
        float zoomScale = Mathf.Abs(transform.position.z); 
        Vector3 adjustedDelta = (Vector3)mouseDelta * panSpeed * zoomScale * -1f;
        targetCameraPos = initialCameraPos + adjustedDelta;

        targetCameraPos.x = Mathf.Clamp(targetCameraPos.x, panLimitX.x, panLimitX.y);
        targetCameraPos.y = Mathf.Clamp(targetCameraPos.y, panLimitY.x, panLimitY.y);
        transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smoothTime);
    }

    void SmoothCameraPosition()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetCameraPos, ref velocity, smoothTime);
    }

    void StopPanning()
    {
        isPanning = false;
    }

    void HandleDoubleClickZoom()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < doubleClickTime)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = (mouseWorldPos - transform.position).normalized;
                targetCameraPos = transform.position + direction * doubleClickZoomAmount;
                targetCameraPos.z = Mathf.Clamp(targetCameraPos.z, zoomMin, zoomMax);
            }
            lastClickTime = Time.time;
        }
    }
}
