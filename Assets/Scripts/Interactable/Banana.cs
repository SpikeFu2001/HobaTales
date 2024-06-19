using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : InteractableObject
{
    public override void Interact()
    {
        base.Interact();
        // TO DO: On interact, enable for player to jump
        Destroy(gameObject);
    }
}
