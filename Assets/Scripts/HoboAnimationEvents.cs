using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoboAnimationEvents : MonoBehaviour
{
    private HoboSwingController _hoboSwingController;

    private void Awake()
    {
        _hoboSwingController = GetComponentInParent<HoboSwingController>();
    }
    
    public void StopAttackAnimation()
    {
        Debug.Log("ANIMTION EVENT");
        _hoboSwingController.OnStopSwingCane();
    }
}

