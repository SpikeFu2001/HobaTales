using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoboSwingController : MonoBehaviour
{
    [SerializeField] private float _sfxVolume = 0.5f;
    
    // Controls
    private int swingMouseButton = 1;

    private HashSet<Bird> nearbyBirds;
    
    private bool canSwing = true;

    private void Awake()
    {
        nearbyBirds = new HashSet<Bird>();
    }

    private void Update()
    {
        OnUpdateInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        Bird bird = other.GetComponent<Bird>();
        if (bird != null)
        {
            nearbyBirds.Add(bird);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Bird bird = other.GetComponent<Bird>();
        if (bird != null)
        {
            nearbyBirds.Remove(bird);
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateInput()
    {
        if (Input.GetMouseButtonDown(swingMouseButton) && canSwing)
        {
            OnStartSwingCane();
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnStartSwingCane()
    {
        canSwing = false;

        AudioManager.instance.PlayGlobalAudio("[05] Cane", _sfxVolume);
        
        // Detect if birds are nearby to swing at
        foreach (Bird bird in nearbyBirds)
        {
            bird.Fly();
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    /// Is called from HoboAnimationEvents
    /// 
    public void OnStopSwingCane()
    {
        canSwing = true;
    }
}
