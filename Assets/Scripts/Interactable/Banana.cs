using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : InteractableObject
{
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        hoboInteractionController.GetComponent<HoboCharacterController>().SetAbleToJump(true);
        Destroy(gameObject);
    }
}
