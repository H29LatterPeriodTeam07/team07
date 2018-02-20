using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaleSpown : MonoBehaviour {
    public AudioClip m_se;
    private AudioSource m_AS;

    private GameObject m_SaleAnimal;
    [SerializeField,Header("アライグマと鶏を一度に何匹/何羽出すか")]
    private int m_ARAIorChikinsNum;
    [SerializeField, Tooltip("上から順に数字を入れる(0:豚,1:牛,2:鯉,\n3:カジキ,4:アライグマ,5:羊,6:チキン)")]
    private int[] m_SaleAnimals;
    //出現時間
    [SerializeField, Header("指定した特売品が出現する時間の指定(float型)")]
    private float[] m_ApperTime;
    [SerializeField, Header("指定した特売品が出現する場所")]
    Transform[] m_AnimalApeers;
    public float m_SaleModeTime;
    public GameObject m_Time;
    public GameObject m_SaleMaterial;
    public int m_Num = 0;

    private int m_ChikinNum = 0;
    Timer m_timer;
    int m_CurrentSaleAnimeIndex=0;
    public int m_CurrentApperTimeIndex=0;
    public int m_CurrentChikinApperTimeIndex = 0;
    float m_SaleMode=0;
    SaleMaterial m_scSale;
    string m_Pigname, FishName, CowName;
    int m_CurrentPatrolPointIndex = 1;
    int m_CurrentApperindex = 0;

    private float myTime = 0.0f;
    int _SaleNum;
    int _ACnum=0;
    // Use this for initialization
    void Start()
    {
        m_timer = m_Time.GetComponent<Timer>();
        m_scSale = m_SaleMaterial.GetComponent<SaleMaterial>();
        m_AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ApperTime.Length > m_CurrentApperTimeIndex)
        {
            m_SaleMode = m_ApperTime[m_CurrentApperTimeIndex] - m_SaleModeTime;
            if (myTime > m_ApperTime[m_CurrentApperTimeIndex])
            {
                //豚
                if (m_SaleAnimals[m_CurrentSaleAnimeIndex] == 0)
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/SaleAnimalPig");
                    _SaleNum = 0;
                    m_scSale.SaleAnimalApper();
                }
                //牛
                else if(m_SaleAnimals[m_CurrentSaleAnimeIndex] == 1)
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/SaleAnimalCow");
                    _SaleNum = 1;
                    m_scSale.SaleAnimalApper();
                }
                //鯉
                else if (m_SaleAnimals[m_CurrentSaleAnimeIndex] == 2)
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/SaleAnimalFish");
                    _SaleNum = 2;
                    m_scSale.SaleAnimalApper();
                }
                //カジキ
                else if (m_SaleAnimals[m_CurrentSaleAnimeIndex] == 3)
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/SaleAnimalKaziki");
                    _SaleNum = 3;
                    m_scSale.SaleAnimalApper();
                }
                //アライグマ
                else if (m_SaleAnimals[m_CurrentSaleAnimeIndex] == 4)
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/SaleAnimalArai");
                    _SaleNum = 4;
                    m_scSale.SaleAnimalApper();
                }
                //羊
                else if (m_SaleAnimals[m_CurrentSaleAnimeIndex] == 5)
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/SaleAnimalSheep");
                    _SaleNum = 5;
                    m_scSale.SaleAnimalApper();
                }
                else
                {
                    m_SaleAnimal = (GameObject)Resources.Load("Prefab/Chickin");
                    _SaleNum = 6;
                    m_scSale.SaleAnimalApper();
                }
            }
        }
    }

   public void Appear()
    {
        if (m_Num < m_SaleAnimals.Length || m_Num<m_AnimalApeers.Length)
        {
                Instantiate(m_SaleAnimal, m_AnimalApeers[m_CurrentApperindex].position, transform.rotation);
            m_SaleAnimal = null;
                m_CurrentSaleAnimeIndex++;
            m_CurrentApperindex++;
        }
    }

   public  void ChikinApper()
    {
        if (m_Num < m_SaleAnimals.Length || m_Num < m_AnimalApeers.Length)
        {
            for (int i = 0; i < m_ARAIorChikinsNum; i++)
            {
                Instantiate(m_SaleAnimal, m_AnimalApeers[m_CurrentApperindex].position, transform.rotation);
            }
            m_SaleAnimal = null;
            m_CurrentSaleAnimeIndex++;
            m_CurrentApperindex++;
        }
    }

    public bool SaleMode()
    {
        return myTime > m_SaleMode;
    }

    public void SetTime(float time)
    {
        myTime = time;
    }

    public float[] GetAppearTime()
    {
        return m_ApperTime;
    }

    public int SaleNumber()
    {
        return _SaleNum;
    }
}
