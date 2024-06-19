using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockableDoor : MonoBehaviour
{
    [SerializeField] private float _sfxVolume = 0.5f;
    
    public void Unlock()
    {
        if (AudioManager.instance != null)
        {
            GameObject audioObject = AudioManager.instance.PlayGlobalAudio("[03] Puzzle Completion Tone", _sfxVolume);
            DontDestroyOnLoad(audioObject);
        }
        
        SceneManager.LoadScene("Credits");
    }
}
