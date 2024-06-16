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
    private Button _startButton;
    private Button _creditsButton;

    ///-////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Awake()
    {
        InitializeUI();
    }

    ///-////////////////////////////////////////////////////////////////////////////////
    ///
    private void InitializeUI()
    {
        _uiDocument = GetComponent<UIDocument>();

        _startButton = _uiDocument.rootVisualElement.Q<Button>("StartButton");
        _startButton.RegisterCallback<ClickEvent>(OnStartButtonPressed);
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
