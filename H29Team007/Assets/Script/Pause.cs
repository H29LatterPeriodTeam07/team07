using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public GameObject m_PauseObj;
    public GameObject m_SaleMaterial;
    public GameObject m_Danger;

    Flash _flash;
    SaleMaterial _saleMaterial;

    public bool _Switch;

    RectTransform m_RectTransform;

    // Use this for initialization
    void Start()
    {
        _saleMaterial = m_SaleMaterial.GetComponent<SaleMaterial>();
        _flash = m_Danger.GetComponent<Flash>();

        m_RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("XboxStart") || Input.GetKeyDown(KeyCode.M))
        {
            print("A");
            if (_Switch == false)
            {
                _Switch = true;
                m_PauseObj.SetActive(true);
                _flash.FadeStop();
                _saleMaterial.m_speed = 0;
                Time.timeScale = 0;
                return;
            }
            else
            {
                _Switch = false;
                m_PauseObj.SetActive(false);
                Time.timeScale = 1;
                _saleMaterial.m_speed = 30;
                _flash.FadeStart();
                return;
            }
        }

        //GameObject selectObject = EventSystem.current.currentSelectedGameObject;

        //if (selectObject == null)
        //{
        //    return;
        //}

        //m_RectTransform.anchoredPosition = selectObject.GetComponent<RectTransform>().anchoredPosition;

    }

    public void Stop()
    {
        m_PauseObj.SetActive(true);
        _flash.FadeStop();
        _saleMaterial.m_speed = 0;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        _Switch = false;
        m_PauseObj.SetActive(false);
        Time.timeScale = 1;
        _saleMaterial.m_speed = 30;
        _flash.FadeStart();
    }
}
