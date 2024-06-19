using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : PickupObject
{
    private Vector3 position;

    private void Awake()
    {
        position = transform.position;
    }
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        isPickedUp = true;
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
