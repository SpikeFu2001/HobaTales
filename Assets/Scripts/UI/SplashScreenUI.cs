using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenUI : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator.Play("LoadSplashScreen");
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
