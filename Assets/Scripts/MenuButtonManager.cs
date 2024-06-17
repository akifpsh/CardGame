using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    // Start butonunun yükleneceği sahne adı
    public string startSceneName = "GAME";

    // Start butonuna basıldığında çağrılacak method
    public void OnStartButton()
    {
        SceneManager.LoadScene(startSceneName);
    }

    // Exit butonuna basıldığında çağrılacak method
    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
