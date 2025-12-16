using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using UnityEngine.EventSystems;

public class ARPlacementController : MonoBehaviour
{
    [Header("AR")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("Placement")]
    public PlaceableObjectData selectedObject;


    [Header("UI")]
    public InstructionText instructionText;
       public GameObject resetButton;

    private GameObject placedObject;
    private static List<ARRaycastHit> hits = new();


    void Update()
    {
        // No touch → do nothing
        if (Input.touchCount == 0)
            return;

        // Now it is SAFE to get touch
        Touch touch = Input.GetTouch(0);

        // Ignore UI touches
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        // Allow placement only once
        if (placedObject != null)
            return;

        // Safety checks
        if (raycastManager == null)
            return;

        if (selectedObject == null || selectedObject.prefab == null)
            return;

        // Place only on first tap
        if (touch.phase != TouchPhase.Began)
            return;

        // AR Plane Raycast
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            placedObject = Instantiate(
                selectedObject.prefab,
                pose.position,
                pose.rotation
            );

            placedObject.transform.localScale = selectedObject.defaultScale;

            var interaction = placedObject.GetComponent<ARObjectInteraction>();
            if (interaction != null)
            {
                interaction.raycastManager = raycastManager;
            }

            instructionText.SetInstruction(
                "Drag to move • Pinch to scale • Double-tap and drag to rotate"
            );
            resetButton.SetActive(true);
            DisablePlaneVisualization();
        }
    }

    public void SetSelectedObject(PlaceableObjectData data)
    {
        selectedObject = data;
        placedObject = null;
    }
    public void ResetPlacement()
    {
        placedObject = null;
    }

    private void DisablePlaneVisualization()
    {
        planeManager.enabled = false;

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}
