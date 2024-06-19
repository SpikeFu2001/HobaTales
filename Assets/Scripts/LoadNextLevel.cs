using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Load Next Level");
            //Load();
        }
    }
    public void Load()
    {

        // Get the current active scene
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 4)
        {
            Debug.Log("Level 2 trying to load level 3");
            return;
        }

        // Calculate the next scene index
        int nextSceneIndex = currentScene.buildIndex + 1;

        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene
            SceneManager.LoadSceneAsync(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
        }
    }

}
