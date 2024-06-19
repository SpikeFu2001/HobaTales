using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoboSwingController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string swingAnimationName = "";
    
    // Controls
    private int swingMouseButton = 1;
    
    private bool canSwing = true;
    
    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateInput()
    {
        if (Input.GetMouseButton(swingMouseButton) && canSwing)
        {
            OnStartSwingCane();
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnStartSwingCane()
    {
        canSwing = false;

        PlaySwingAnimation();
        
        // Detect if birds are nearby to swing at
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    /// Is called from HoboAnimationEvents
    /// 
    private void OnStopSwingCane()
    {
        canSwing = true;
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void PlaySwingAnimation()
    {
        _animator.CrossFade(swingAnimationName, 0.15f);
    }
}
