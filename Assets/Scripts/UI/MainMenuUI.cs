using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string _gameSceneName;

    private UIDocument _uiDocument;
    private VisualElement _startVisualElement;
    private Button _creditsButton;
    

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        InitializeUI();

        AudioManager.instance.StopAllAudio();
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    ///
    private void InitializeUI()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _startVisualElement = _uiDocument.rootVisualElement.Q<VisualElement>("Start");
        _startVisualElement.RegisterCallback<ClickEvent>(OnStartButtonPressed);
        
        _creditsButton = _uiDocument.rootVisualElement.Q<Button>("CreditsButton");
        _creditsButton.RegisterCallback<ClickEvent>(OnCreditsButtonPressed);
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnStartButtonPressed(ClickEvent evt)
    {
        // Start Game
        SceneManager.LoadScene(_gameSceneName);
    }
    
    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnCreditsButtonPressed(ClickEvent evt)
    {
        Debug.Log("TO DO: IMPLEMENT CREDITS BUTTON");
    }
}
