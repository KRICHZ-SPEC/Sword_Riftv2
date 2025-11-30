using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "SWORDRIFT"; 
    
    [Header("UI Panels")]
    public GameObject settingsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OpenSettings()
    {
        if(settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if(settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}