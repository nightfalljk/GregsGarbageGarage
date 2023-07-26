using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    
    public void ChangeToSceneByID(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void ChangeToSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
