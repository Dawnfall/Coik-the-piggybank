using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneController : MonoBehaviour
{
    public string NextScene;
    [SerializeField] private Transform m_allEnemiesParent;

    private void Update()
    {
        if (m_allEnemiesParent.childCount == 0)
        {
            GameManager.Instance.SavedHealth = FindObjectOfType<PlayerController>().Life.HP;
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
        }
    }

}
