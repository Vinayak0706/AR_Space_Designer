using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectInteraction : MonoBehaviour
{
  [Header("AR")]
    public ARRaycastManager raycastManager;

    [Header("Move")]
    public float moveSmooth = 15f;

    [Header("Scale")]
    public float minScale = 0.05f;
    public float maxScale = 0.5f;

    [Header("Rotate")]
    public float rotationSpeed = 0.2f;

    private static List<ARRaycastHit> hits = new();

    // Rotation tracking
    private bool isRotating;
    private float lastTapTime;
    private const float doubleTapThreshold = 0.3f;
    private Vector2 lastTouchPos;

    // Scale tracking
    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (Input.touchCount == 2)
        {
            HandlePinchScale();
        }
    }

    // ---------------- MOVE & ROTATE ----------------

    void HandleSingleTouch(Touch touch)
    {
        // Double tap detection
        if (touch.phase == TouchPhase.Began)
        {
            if (Time.time - lastTapTime < doubleTapThreshold)
            {
                isRotating = true;
            }

            lastTapTime = Time.time;
            lastTouchPos = touch.position;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            if (isRotating)
            {
                RotateObject(touch);
            }
            else
            {
                MoveObject(touch);
            }

            lastTouchPos = touch.position;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            isRotating = false;
        }
    }

    void MoveObject(Touch touch)
    {
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Vector3 targetPos = hits[0].pose.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSmooth);
        }
    }

    void RotateObject(Touch touch)
    {
        float deltaX = touch.position.x - lastTouchPos.x;
        transform.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.World);
    }

    // ---------------- SCALE ----------------

    void HandlePinchScale()
    {
        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        if (t2.phase == TouchPhase.Began)
        {
            initialDistance = Vector2.Distance(t1.position, t2.position);
            initialScale = transform.localScale;
        }

        float currentDistance = Vector2.Distance(t1.position, t2.position);
        float scaleFactor = currentDistance / initialDistance;

        Vector3 newScale = initialScale * scaleFactor;
        newScale = Vector3.one * Mathf.Clamp(newScale.x, minScale, maxScale);

        transform.localScale = newScale;
    }
}
