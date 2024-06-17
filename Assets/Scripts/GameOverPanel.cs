using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public GameObject panel;
    public Button mainMenuButton;
    public Button newGameButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenu);
        newGameButton.onClick.AddListener(NewGame);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void MainMenu()
    {
        // Ana menüye dönmek için baþka bir sahne yüklenir
        SceneManager.LoadScene("MENU");
    }

    public void NewGame()
    {
        panel.SetActive(false);
        // Yeni oyuna baþlamak için GameManager'daki RestartGame metodunu çaðýrarak oyun sýfýrlanýr
        GameManager.Instance.RestartGame();
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == null) // DontDestroyOnLoad objeleri sahnesi null'dýr
            {
                Destroy(obj);
            }
        }
    }
}
