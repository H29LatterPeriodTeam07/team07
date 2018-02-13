using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTitle : MonoBehaviour
{

    private float m_Time = 0.0f;
    private float returnTime = 30.0f;

    public static ReturnTitle Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        // 重複防止措置。
        // 既にある場合は自身を削除する
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        returnTime /= Time.deltaTime;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.anyKeyDown
            || Input.GetAxis("XboxLeftHorizontal") != 0 || Input.GetAxis("XboxLeftVertical") != 0
            || Input.GetAxis("XboxRightHorizontal") != 0 || Input.GetAxis("XboxRightVertical") != 0) m_Time = 0.0f;


        if (SceneManager.GetActiveScene().name == "Title") return;

        m_Time += 1.0f;

        if (m_Time > returnTime)
        {
            SceneManager.LoadScene("Title");
            m_Time = 0.0f;
        }
    }
}
