using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

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
        SceneManager.LoadScene("Alpha");
    }
    public void StageSelectLoad()
    {
        SceneManager.LoadScene("StageSelectTest");
    }

}
