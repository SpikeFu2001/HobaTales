using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///-////////////////////////////////////////////////////////////////////////////////
/// 
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private Dictionary<string, AudioClip> _audioClips;

    private HashSet<GameObject> _activeAudioObjects;

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        // Initiate Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
        // Load Audio
        LoadAudio();

        _activeAudioObjects = new HashSet<GameObject>();
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Start()
    {
        // FOR TESTING
        
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void LoadAudio()
    {
        // Create dictionary that will contain all audio clips
        _audioClips = new Dictionary<string, AudioClip>();
        
        // Get all audio clip files
        AudioClip[] audioClipFiles = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip audioClip in audioClipFiles)
        {
            _audioClips.Add(audioClip.name, audioClip);
        }
    }
    
    ///-////////////////////////////////////////////////////////////////////////////////
    ///
    public GameObject PlayGlobalAudio(string audioName, float volume = 1f, bool loop = false, GameObject audioObj = null)
    {
        GameObject audioObject = audioObj;
        if (audioObject == null)
        {
            audioObject = new GameObject($"AudioObject_{audioName}");
        }
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        AudioClip audioClip = _audioClips[audioName];

        // Set AudioSource Properties
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = loop;

        // Set audio to global
        audioSource.spatialBlend = 0;
        
        audioSource.Play();
        _activeAudioObjects.Add(audioObject);
        
        if (loop == false)
        {
            Destroy(audioObject, audioClip.length);
        }

        return audioObject;
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    ///
    public void StopAllAudio()
    {
        foreach (GameObject audioObject in _activeAudioObjects)
        {
            _activeAudioObjects.Remove(audioObject);
            Destroy(audioObject);
        }
    }
    
}
