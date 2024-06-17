using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    // Start butonunun yükleneceði sahne adý
    public string startSceneName = "GAME";

    // Start butonuna basýldýðýnda çaðrýlacak method
    public void OnStartButton()
    {
        SceneManager.LoadScene(startSceneName);
    }

    // Exit butonuna basýldýðýnda çaðrýlacak method
    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
