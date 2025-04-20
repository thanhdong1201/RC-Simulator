using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] private SceneSO sceneSO;
    [SerializeField] private GameObject starsUI;
    [SerializeField] private Button selectBtn;

    [SerializeField] private List<Image> starImages;

    private LoadSceneManager loadSceneManager;
    private void Start()
    {
        for (int i = 0; i < starImages.Count; i++)
        {
            if (i <= sceneSO.questSO.StarPoints)
            {
                starImages[i].gameObject.SetActive(true);
            }
            else
            {
                starImages[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetUp(LoadSceneManager loadSceneManager)
    {
        this.loadSceneManager = loadSceneManager;
    }
    public void SelectScene()
    {
        this.loadSceneManager.SetTargetScene(sceneSO);
    }
}
