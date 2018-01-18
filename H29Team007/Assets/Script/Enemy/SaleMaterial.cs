using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleMaterial : MonoBehaviour {
   
    [SerializeField, Header("ポジション")]
    private GameObject m_position;
    [SerializeField, Tooltip("出す動物の数字を入れる(0:豚,1:牛,2:鯉,\n3:カジキ,4:アライグマ,5:ヤギ,6:羊,7:猪)")]
    private int[] m_SaleAnimals;
    public float m_speed = 3.0f;
    public AudioClip m_se;
    public GameObject m_SoundManager;
    private SoundManagerScript m_smScript;

    public GameObject m_Cow, m_fish, m_Pig,m_Bull,m_CowCharacter, m_FishCharacter, m_pigCharacter,m_BullCharacter,
        m_Aray,m_ArayCharacter,m_Kaziki,m_KazikiCharacter,m_Chikin,m_ChikinCharacter,m_Goat,m_GoatCharacter,
        m_Boar,m_BoarCharacter,m_Sheep,m_SheepCharacter,m_Shark,m_SharkCharacter,m_Herazika,m_HerazikaCharacter;
    
    GameObject m_SaleSpown;
    GameObject m_ExitPoint;
    float m_time=0.0f;
    float m_timebul = 0.0f;
    SaleSpown m_scSaleSpwn;
    Exit m_exScript;
    AudioSource m_AS;
    bool m_fse = false;
    bool m_fsee = false;
    int _num = 0;
    int _numIndex=0;
    Transform m_UI,m_UI2;
	// Use this for initialization
	void Start () {
        m_Cow.SetActive(false);
        m_fish.SetActive(false);
        m_Pig.SetActive(false);
        m_Bull.SetActive(false);
        //m_CowCharacter.SetActive(false);
        //m_FishCharacter.SetActive(false);
        //m_pigCharacter.SetActive(false);
        //m_BullCharacter.SetActive(false);
        m_SaleSpown = GameObject.FindGameObjectWithTag("SaleSpown");
        m_ExitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        m_scSaleSpwn = m_SaleSpown.GetComponent<SaleSpown>();
        m_exScript = m_ExitPoint.GetComponent<Exit>();
        m_smScript = m_SoundManager.GetComponent<SoundManagerScript>();
        m_AS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ApperBull()
    {
        if (m_timebul <= 3.5f)
        {
            if (!m_fsee)
            {
                m_smScript.PlaySE(2);
                m_smScript.PlaySE(3);
                m_fsee = true;
            }
            m_timebul += Time.deltaTime;
            m_time += Time.deltaTime;
            m_Bull.SetActive(true);
            m_BullCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
        }
        else
        {
            m_smScript.StopSE();
            m_fsee = false;
            m_Bull.SetActive(false);
            m_BullCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
            m_timebul = 0;
            m_exScript.Appear();
        }
    }
    public void ApperHera()
    {
        if (m_timebul <= 3.5f)
        {
            if (!m_fsee)
            {
                m_smScript.PlaySE(2);
                m_smScript.PlaySE(3);
                m_fsee = true;
            }
            m_timebul += Time.deltaTime;
            m_time += Time.deltaTime;
            m_Herazika.SetActive(true);
            m_HerazikaCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
        }
        else
        {
            m_smScript.StopSE();
            m_fsee = false;
            m_Herazika.SetActive(false);
            m_HerazikaCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
            m_timebul = 0;
            m_exScript.Appear();
        }
    }

    public void ApperShark()
    {
        if (m_timebul <= 3.5f)
        {
            if (!m_fsee)
            {
                m_smScript.PlaySE(2);
                m_smScript.PlaySE(3);
                m_fsee = true;
            }
            m_timebul += Time.deltaTime;
            m_time += Time.deltaTime;
            m_Shark.SetActive(true);
            m_SharkCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
        }
        else
        {
            m_smScript.StopSE();
            m_fsee = false;
            m_Shark.SetActive(false);
            m_SharkCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
            m_timebul = 0;
            m_exScript.Appear();
        }
    }

    public void SaleAnimalApper()
    {
            if (  m_scSaleSpwn.SaleNumber()==0)
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
                    m_pigCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_Pig.SetActive(true);
            }
                else
                {
                    m_smScript.StopSE();
                    m_fse = false;
                    m_Pig.SetActive(false);
                    m_pigCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                    m_scSaleSpwn.Appear();
                    m_scSaleSpwn.m_Num++;
                    m_time = 0;
                    m_scSaleSpwn.m_CurrentApperTimeIndex++;
                }
            }
          　else if ( m_scSaleSpwn.SaleNumber() == 1)
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
                    m_CowCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                    m_Cow.SetActive(true);
                    //m_CowCharacter.SetActive(true);
                }
                else
                {
                    m_smScript.StopSE();
                    m_fse = false;
                    m_Cow.SetActive(false);
                    m_CowCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                    //m_CowCharacter.SetActive(false);
                    m_scSaleSpwn.Appear();
                    m_scSaleSpwn.m_Num++;
                    m_time = 0;
                    m_scSaleSpwn.m_CurrentApperTimeIndex++;
                }
            }
        else if (m_scSaleSpwn.SaleNumber() == 2)
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
                m_FishCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_fish.SetActive(true);
                //m_CowCharacter.SetActive(true);
            }
            else
            {
                m_smScript.StopSE();
                m_fse = false;
                m_fish.SetActive(false);
                m_FishCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                //m_CowCharacter.SetActive(false);
                m_scSaleSpwn.Appear();
                m_scSaleSpwn.m_Num++;
                m_time = 0;
                m_scSaleSpwn.m_CurrentApperTimeIndex++;
            }
        }
        else if (m_scSaleSpwn.SaleNumber() == 3)
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
                m_KazikiCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_Kaziki.SetActive(true);
                //m_CowCharacter.SetActive(true);
            }
            else
            {
                m_smScript.StopSE();
                m_fse = false;
                m_Kaziki.SetActive(false);
                m_KazikiCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                //m_CowCharacter.SetActive(false);
                m_scSaleSpwn.Appear();
                m_scSaleSpwn.m_Num++;
                m_time = 0;
                m_scSaleSpwn.m_CurrentApperTimeIndex++;
            }
        }
        else if (m_scSaleSpwn.SaleNumber() == 4)
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
                m_ArayCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_Aray.SetActive(true);
                //m_CowCharacter.SetActive(true);
            }
            else
            {
                m_smScript.StopSE();
                m_fse = false;
                m_Aray.SetActive(false);
                m_ArayCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                //m_CowCharacter.SetActive(false);
                m_scSaleSpwn.Appear();
                m_scSaleSpwn.m_Num++;
                m_time = 0;
                m_scSaleSpwn.m_CurrentApperTimeIndex++;
            }
        }
        else if (m_scSaleSpwn.SaleNumber() == 5)
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
                m_GoatCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_Goat.SetActive(true);
                //m_CowCharacter.SetActive(true);
            }
            else
            {
                m_smScript.StopSE();
                m_fse = false;
                m_Goat.SetActive(false);
                m_GoatCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                //m_CowCharacter.SetActive(false);
                m_scSaleSpwn.Appear();
                m_scSaleSpwn.m_Num++;
                m_time = 0;
                m_scSaleSpwn.m_CurrentApperTimeIndex++;
            }
        }
        else if (m_scSaleSpwn.SaleNumber() == 6)
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
                m_SheepCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_Sheep.SetActive(true);
                //m_CowCharacter.SetActive(true);
            }
            else
            {
                m_smScript.StopSE();
                m_fse = false;
                m_Sheep.SetActive(false);
                m_SheepCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                //m_CowCharacter.SetActive(false);
                m_scSaleSpwn.Appear();
                m_scSaleSpwn.m_Num++;
                m_time = 0;
                m_scSaleSpwn.m_CurrentApperTimeIndex++;
            }
        }
        else 
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
                m_BoarCharacter.GetComponent<RectTransform>().localPosition -= Vector3.right * m_speed;
                m_Boar.SetActive(true);
                //m_CowCharacter.SetActive(true);
            }
            else
            {
                m_smScript.StopSE();
                m_fse = false;
                m_Boar.SetActive(false);
                m_BoarCharacter.GetComponent<RectTransform>().localPosition = m_position.GetComponent<RectTransform>().localPosition;
                //m_CowCharacter.SetActive(false);
                m_scSaleSpwn.Appear();
                m_scSaleSpwn.m_Num++;
                m_time = 0;
                m_scSaleSpwn.m_CurrentApperTimeIndex++;
            }
        }
    }
}
