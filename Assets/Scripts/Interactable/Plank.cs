using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Plank : PickupObject
{
    public bool isPickedUp = false;
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        isPickedUp = true;
    }
    
    public override void Drop(HoboInteractionController hoboInteractionController)
    {
        isPickedUp = false;

        if (hoboInteractionController.bridge != null)
        {
            hoboInteractionController.bridge.AddPlank();
            Destroy(gameObject);
        }
    }
}
