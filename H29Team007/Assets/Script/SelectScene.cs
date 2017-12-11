using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    public GameObject m_sm;
    private SoundManagerScript m_scScript;
    string currentScene;
    // Use this for initialization
    void Start()
    {
        m_scScript = m_sm.transform.GetComponent<SoundManagerScript>();
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Title")
        {
            m_scScript.PlayBGM(0);
        }
        if (currentScene == "StageSelect")
        {
            m_scScript.PlayBGM(0);
        }
        if (currentScene == "Result")
        {
            m_scScript.PlaySE(0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TitleLoad()
    {
        SceneManager.LoadScene("Title");
    }
    public void StageLoad()
    {
        SceneManager.LoadScene("Beforeβ");
    }
    public void StageSelectLoad()
    {
        SceneManager.LoadScene("StageSelect");
    }

}
