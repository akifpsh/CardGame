using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    // Start butonunun y�klenece�i sahne ad�
    public string startSceneName = "GAME";

    // Start butonuna bas�ld���nda �a�r�lacak method
    public void OnStartButton()
    {
        SceneManager.LoadScene(startSceneName);
    }

    // Exit butonuna bas�ld���nda �a�r�lacak method
    public void OnExitButton()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
