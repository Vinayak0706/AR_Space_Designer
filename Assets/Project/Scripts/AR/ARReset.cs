using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;

public class ARReset : MonoBehaviour
{
    [Header("AR")]
    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;

    [Header("Placement")]
    public ARPlacementController placementController;

    [Header("Instruction")]
    public InstructionText instructionText;

    public void ResetAR()
    {
        // Remove all placed objects
        foreach (var obj in FindObjectsOfType<ARObjectInteraction>())
        {
            Destroy(obj.gameObject);
        }

        // Clear active selection
        ARObjectInteraction.ActiveObject = null;

        // Reset placement controller
        placementController.ResetPlacement();

        // Re-enable plane detection
        planeManager.enabled = true;

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(true);
        }

        //  Reset instruction
        instructionText.SetInstruction("Tap to place object.");
    }
}
