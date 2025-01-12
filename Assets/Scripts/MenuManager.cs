using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Quit();
        }
    }

    private void SceneLoader(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
