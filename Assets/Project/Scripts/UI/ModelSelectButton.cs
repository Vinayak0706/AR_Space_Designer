using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModelSelectButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlaceableObjectData objectData;

    [Header("References")]
    [SerializeField] private ARPlacementController placementController;
    [SerializeField] private TextMeshProUGUI label;

    private void Awake()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (label != null && objectData != null)
            label.text = objectData.displayName;
    }

    public void SelectModel()
    {
        if (placementController == null || objectData == null)
        {
            Debug.LogWarning("ModelSelectButton is not configured properly.");
            return;
        }

        placementController.SetSelectedObject(objectData);
        gameObject.GetComponent<Button>().interactable = false;
    }
}
