using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreditsUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private Button _mainMenuButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("MainMenuButton");
        _mainMenuButton.RegisterCallback<ClickEvent>(MainMenuButtonClicked);
    }

    private void MainMenuButtonClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenu");
    }
}
