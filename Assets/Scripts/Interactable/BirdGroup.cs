using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGroup : Bird
{
    [SerializeField] private GameObject key;
    [SerializeField] private float _sfxVolume = 0.5f;
    
    public override void Fly()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGlobalAudio("[01] Object Interaction", _sfxVolume);
        }
        
        key.SetActive(true);
        key.transform.parent = null;
        
        Destroy(this);
    }
}
