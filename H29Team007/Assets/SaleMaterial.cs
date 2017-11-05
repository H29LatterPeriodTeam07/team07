using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleMaterial : MonoBehaviour {

    public GameObject m_Cow, m_fish, m_Pig,m_CowCharacter, m_FishCharacter, m_pigCharacter;

    GameObject m_SaleSpown;
    float m_time=0.0f;
    SaleSpown m_scSaleSpwn;
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
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void ApperCow()
    {
        m_time += Time.deltaTime;
        m_Cow.SetActive(true);
        m_CowCharacter.SetActive(true);
        if (m_time > 2.0f)
        {
            m_Cow.SetActive(false);
            m_CowCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
            m_scSaleSpwn.m_Num++;
        }
    }

    public void ApperFish()
    {
        m_time += Time.deltaTime;
        m_fish.SetActive(true);
        m_FishCharacter.SetActive(true);
        if (m_time > 2.0f)
        {
            m_fish.SetActive(false);
            m_FishCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
            m_scSaleSpwn.m_Num++;
        }
    }

    public void ApperPig()
    {
        m_time += Time.deltaTime;
        m_Pig.SetActive(true);
        m_pigCharacter.SetActive(true);
        if (m_time > 2.0f)
        {
            m_Pig.SetActive(false);
            m_pigCharacter.SetActive(false);
            m_scSaleSpwn.Appear();
            m_scSaleSpwn.m_CurrentApperTimeIndex++;
            m_scSaleSpwn.m_Num++;
        }
    }
}
