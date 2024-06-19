using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject bridgeSilhouette;
    [SerializeField] private TextMeshPro numPlanksText;
    [SerializeField] private GameObject blockCollider;

    [Space] [SerializeField] private int planksNeeded = 3;
    
    private int numPlanks = 0;
    
    public void AddPlank()
    {
        numPlanks++;
        
        UpdateBridgeUI();

        if (numPlanks == planksNeeded)
        {
            bridge.SetActive(true);
            bridgeSilhouette.SetActive(false);
            blockCollider.SetActive(false);
            numPlanksText.text = "";
            
            // Play Puzzle Complete Audio
            AudioManager.instance.PlayGlobalAudio("[03] Puzzle Completion Tone");
        }
    }

    private void UpdateBridgeUI()
    {
        numPlanksText.text = $"{numPlanks} / 3 Planks";
    }
}
