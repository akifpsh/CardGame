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
        // Ana men�ye d�nmek i�in ba�ka bir sahne y�klenir
        SceneManager.LoadScene("MENU");
    }

    public void NewGame()
    {
        panel.SetActive(false);
        // Yeni oyuna ba�lamak i�in GameManager'daki RestartGame metodunu �a��rarak oyun s�f�rlan�r
        GameManager.Instance.RestartGame();
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.name == null) // DontDestroyOnLoad objeleri sahnesi null'd�r
            {
                Destroy(obj);
            }
        }
    }
}
