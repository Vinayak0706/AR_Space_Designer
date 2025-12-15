using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScanningUIController : MonoBehaviour
{
    [Header("References")]
    public ARPlaneManager planeManager;
    public InstructionText instructionText;
    public GameObject modelSelectorUI;

    bool hasFoundPlane = false;

    void Update()
    {
        if (hasFoundPlane)
            return;

        if (planeManager.trackables.count > 0)
        {
            hasFoundPlane = true;
            instructionText.SetInstruction("Tap to place object.");
            modelSelectorUI.SetActive(true);

        }
    }

 

}
