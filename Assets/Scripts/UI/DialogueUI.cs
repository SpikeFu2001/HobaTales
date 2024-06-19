using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject dialogueCanvas;

    [SerializeField]
    public GameObject nextTriggerArea;

    [SerializeField] 
    public GameObject player;

    [SerializeField]
    public float disableUIdelay = 3f;
    void Start()
    {
        if (dialogueCanvas != null) { dialogueCanvas.SetActive(false); }
        if (nextTriggerArea != null) { nextTriggerArea.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (dialogueCanvas.name == "Dialogue_Banana_Collected")
        {
            if (player != null)
            {
                if (!player.GetComponent<HoboCharacterController>().GetAbleToJump())
                {
                    return;
                }
            }
            return;
        }
        if (other.CompareTag("Player")&& dialogueCanvas!=null)
        {
            dialogueCanvas.SetActive(true);
            StartCoroutine(MoveOnToNextDialogue());
        }
    }
    IEnumerator MoveOnToNextDialogue()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(disableUIdelay);
        Destroy(dialogueCanvas);
        if (nextTriggerArea != null)
        {
            nextTriggerArea.SetActive(true);
        }
    }
}
