using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionText;

    void Awake()
    {
        // Safety check
        if (instructionText == null)
            instructionText = GetComponent<TextMeshProUGUI>();
    }

    public void SetInstruction(string message)
    {
        instructionText.text = message;
        instructionText.gameObject.SetActive(true);
    }

    public void HideInstruction()
    {
        instructionText.gameObject.SetActive(false);
    }
}
