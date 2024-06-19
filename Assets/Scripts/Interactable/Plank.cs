using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Plank : PickupObject
{
    [SerializeField] private float _sfxVolume = 0.5f;

    private Vector3 position;

    private void Awake()
    {
        position = transform.position;
    }
    
    public override void Interact(HoboInteractionController hoboInteractionController)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGlobalAudio("[01] Object Interaction", _sfxVolume);
        }
        ;
        isPickedUp = true;
    }
    
    public override void Drop(HoboInteractionController hoboInteractionController)
    {
        transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
        
        isPickedUp = false;

        if (hoboInteractionController.bridge != null)
        {
            hoboInteractionController.bridge.AddPlank();
            Destroy(gameObject);
        }
    }
}
