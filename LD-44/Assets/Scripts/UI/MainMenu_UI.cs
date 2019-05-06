using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using helper;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] Button m_playButton;
    [SerializeField] Button m_exitButton;
    [SerializeField] Button m_startButton;

    [SerializeField] Panel_UI m_rulesPanel;

    private void Awake()
    {
        m_startButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SavedHealth = -1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene 1");
        });
        m_exitButton.onClick.AddListener(HelperUnity.ExitGame);
        m_playButton.onClick.AddListener(m_rulesPanel.Activate);
    }
}
