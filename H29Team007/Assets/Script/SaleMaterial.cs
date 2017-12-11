using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleMaterial : MonoBehaviour {
    [SerializeField, Header("aaaa")]
    private GameObject[] m_sale;

    public AudioClip m_se;
    public GameObject m_SoundManager;
    private SoundManagerScript m_smScript;

    public GameObject m_Cow, m_fish, m_Pig,m_CowCharacter, m_FishCharacter, m_pigCharacter;
    
    GameObject m_SaleSpown;
    float m_time=0.0f;
    SaleSpown m_scSaleSpwn;
    AudioSource m_AS;
    bool m_fse = false;
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
        if (m_time <= 3.0f)
        {
            if (!m_fse)
            {              
                m_smScript.PlaySE(2);
                m_smScript.PlaySE(3);
                m_fse = true;
            }
            m_time += Time.deltaTime;
            m_Cow.SetActive(true);
            m_CowCharacter.SetActive(true);
            m_AS.clip = m_se;
            m_AS.Play();
        }
        else
        {
            m_smScript.StopSE();
            m_fse = false;
            m_Cow.SetActive(false);
            m_CowCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_Num++;
            m_time = 0;
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
        }
    }

    public void ApperFish()
    {
        if (m_time <= 3.0f)
        {
            if (!m_fse)
            {
                m_smScript.PlaySE(2);
                m_smScript.PlaySE(3);
                m_fse = true;
            }
            m_time += Time.deltaTime;
            m_fish.SetActive(true);
            m_FishCharacter.SetActive(true);
            m_AS.clip = m_se;
            m_AS.Play();
        }
        else {
            m_smScript.StopSE();
            m_fse = false;
            m_fish.SetActive(false);
            m_FishCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_Num++;
            m_time = 0;
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
        }
    }

    public void ApperPig()
    {
        if (m_time <= 3.0f)
        {
            if (!m_fse)
            {
                m_smScript.PlaySE(2);
                m_smScript.PlaySE(3);
                m_fse = true;
            }
            m_time += Time.deltaTime;
            m_Pig.SetActive(true);
            m_pigCharacter.SetActive(true);
        }
        else
        {
            m_smScript.StopSE();
            m_fse = false;
            m_Pig.SetActive(false);
            m_pigCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_Num++;
            m_time = 0;
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
        }
    }
}
