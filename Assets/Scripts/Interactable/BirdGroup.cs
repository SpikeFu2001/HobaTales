using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGroup : Bird
{
    [SerializeField] private GameObject key;
    [SerializeField] private float _sfxVolume = 0.5f;
    [SerializeField] public GameObject keySpawnedDialogue;

    void Start()
    {
        if (keySpawnedDialogue != null) { keySpawnedDialogue.SetActive(false); }
    }
    public override void Fly()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGlobalAudio("[01] Object Interaction", _sfxVolume);
        }
        
        key.SetActive(true);
        key.transform.parent = null;

        if (keySpawnedDialogue != null)
        {
            keySpawnedDialogue.SetActive(true);
        }
        
        Destroy(this);
    }
}
