using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ScriptableObjects/SceneSO")]
public class SceneSO : ScriptableObject
{
    public int level;
    public bool isUnlocked = false;

    public void UnlockLevel()
    {
        isUnlocked = true;
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(this.name);
    }
}
