using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndMenu_UI: MonoBehaviour
{
    [SerializeField] Button m_returnButton;

    private void Awake()
    {
        m_returnButton.onClick.AddListener(() => { UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene"); });
    }
}
