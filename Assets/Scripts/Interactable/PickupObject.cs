using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : InteractableObject
{
    public bool isPickedUp = false;
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        // Nothing for now I guess
    }

    public virtual void Drop(HoboInteractionController hoboInteractionController)
    {
        
    }
}
