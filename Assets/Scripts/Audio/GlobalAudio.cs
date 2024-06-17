using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-////////////////////////////////////////////////////////////////////////////////
/// 
public class GlobalAudio : MonoBehaviour
{
    [Header("Audio Clip Properties")]
    [SerializeField] private string _audioClipName = "";
    [Range(0f, 1f)] 
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool loop = false;
    
    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Start()
    {
        AudioManager.instance.PlayGlobalAudio(_audioClipName, volume, loop, gameObject);
    }
}
