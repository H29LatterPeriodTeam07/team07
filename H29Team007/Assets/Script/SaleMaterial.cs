﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleMaterial : MonoBehaviour {
    public AudioClip m_se;
    public GameObject m_SoundManager;
    private SoundManagerScript m_smScript;

    public GameObject m_Cow, m_fish, m_Pig,m_CowCharacter, m_FishCharacter, m_pigCharacter;
    
    GameObject m_SaleSpown;
    float m_time=0.0f;
    SaleSpown m_scSaleSpwn;
    AudioSource m_AS;
    bool m_fse;
	// Use this for initialization
	void Start () {
        m_Cow.SetActive(false);
        m_fish.SetActive(false);
        m_Pig.SetActive(false);
        m_CowCharacter.SetActive(false);
        m_FishCharacter.SetActive(false);
        m_pigCharacter.SetActive(false);
        m_SaleSpown = GameObject.FindGameObjectWithTag("SaleSpown");
        m_scSaleSpwn = m_SaleSpown.GetComponent<SaleSpown>();
        m_smScript = m_SoundManager.GetComponent<SoundManagerScript>();
        m_AS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ApperCow()
    {
        //    m_smScript.PlayBGM(0);
        m_time += Time.deltaTime;
        m_Cow.SetActive(true);
        m_CowCharacter.SetActive(true);
        m_AS.clip = m_se;
        m_AS.Play();
        if (m_time > 3.0f)
        {
            //   m_smScript.StopSE();
            m_Cow.SetActive(false);
            m_CowCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
            m_scSaleSpwn.m_Num++;
            m_time = 0;
        }
    }

    public void ApperFish()
    {
        m_time += Time.deltaTime;
      //  m_smScript.PlayBGM(0);
        m_fish.SetActive(true);
        m_FishCharacter.SetActive(true);
        m_AS.clip = m_se;
        m_AS.Play();
        if (m_time > 3.0f)
        {
            //    m_smScript.StopSE();
            m_fish.SetActive(false);
            m_FishCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
            m_scSaleSpwn.m_Num++;
            m_time = 0;
        }
    }

    public void ApperPig()
    {
        m_time += Time.deltaTime;
      //  m_smScript.PlayBGM(0);
        m_Pig.SetActive(true);
        m_pigCharacter.SetActive(true);
        if (m_time > 3.0f)
        {
            // m_smScript.StopBGM();
            m_Pig.SetActive(false);
            m_pigCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
            m_scSaleSpwn.m_Num++;
            m_time = 0;
        }
    }
}
