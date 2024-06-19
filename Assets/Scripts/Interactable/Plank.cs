using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Plank : PickupObject
{
    [SerializeField] private float _sfxVolume = 0.5f;
    public bool isPickedUp = false;
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        AudioManager.instance.PlayGlobalAudio("[01] Object Interaction", _sfxVolume);
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
