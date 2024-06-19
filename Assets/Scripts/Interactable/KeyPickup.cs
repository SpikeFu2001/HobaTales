using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : PickupObject
{
    private Vector3 position;
    [SerializeField] public GameObject keyPickedUpDialogue;
    [SerializeField] public GameObject openHouseDialogue;
    void Start()
    {
        if (keyPickedUpDialogue != null) { keyPickedUpDialogue.SetActive(false); }
        if (openHouseDialogue != null) { openHouseDialogue.SetActive(false); }
    }

    private void Awake()
    {
        position = transform.position;
    }
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        isPickedUp = true;
        if (keyPickedUpDialogue != null)
        {
            keyPickedUpDialogue.SetActive(true);
        }
        if(openHouseDialogue != null)
        {
            openHouseDialogue.SetActive(true);
        }
    }

    public override void Drop(HoboInteractionController hoboInteractionController)
    {
        transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
        
        isPickedUp = false;

        if (hoboInteractionController.unlockableDoor != null)
        {
            hoboInteractionController.unlockableDoor.Unlock();
        }
    }
}
