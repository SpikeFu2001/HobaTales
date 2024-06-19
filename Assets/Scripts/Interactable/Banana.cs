using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : InteractableObject
{
    [SerializeField] private float _sfxVolume = 0.25f;
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        AudioManager.instance.PlayGlobalAudio("[08] Eating Banana", _sfxVolume);
        
        hoboInteractionController.GetComponent<HoboCharacterController>().SetAbleToJump(true);
        Destroy(gameObject);
    }
}
