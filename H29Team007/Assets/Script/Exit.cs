using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject m_SaleMaterial;
    public GameObject m_time;
    [SerializeField, Header("BBAのプレハブ")]
    private GameObject m_PreBBA;
    private GameObject m_prBull;
    [SerializeField, Tooltip("危険生物の数字(0:闘牛,1:ヘラジカ,2:鮫)")]
    private int m_DangerNum;
    [SerializeField, Header("入口ポイント")]
    private Transform m_EntrancePoint;
    [SerializeField, Header("闘牛の出現時間")]
    public float[] m_bullApperTime;
    //BBAオブジェクトを検索して保持
    private GameObject m_GameBBA;
    Timer m_timer;
    int m_CurentApperTimeIndex = 0;
    int m_Num = 0;
    bool m_bullApper = false;
    Player pScript;
    GameObject m_player;
   // BullCount m_bc;

     int m_CurrentApperTimeIndex = 0;
    float m_SaleMode = 0;
    SaleMaterial m_scSale;


    // Use this for initialization
    void Start()
    {
        m_GameBBA = GameObject.FindGameObjectWithTag("BBA");
        m_timer = m_time.GetComponent<Timer>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        pScript = m_player.GetComponent<Player>();
       // m_bc = m_prBull.transform.root.GetComponent<BullCount>();
        m_scSale = m_SaleMaterial.GetComponent<SaleMaterial>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bullApperTime.Length > m_CurentApperTimeIndex)
        {
            if (m_timer.NowTime() > m_bullApperTime[m_CurentApperTimeIndex])
            {
                //if (m_prBull.gameObject.name == "Lamborghini")
                //{
                //    m_scSale.ApperBull();
                //}
                if (m_DangerNum == 0)
                {
                    m_scSale.ApperBull();
                }
                else if (m_DangerNum == 1)
                {
                    m_scSale.ApperHera();
                }
                else
                {
                    m_scSale.ApperShark();
                }
            }           
            
        }
    }

    public void Appear()
    {
        m_bullApper = true;
        if (m_Num < 1)
        {
            if (m_DangerNum == 0)
            {
                m_prBull = (GameObject)Resources.Load("Prefab/Lamborghini");
                Instantiate(m_prBull, m_EntrancePoint.transform.position, transform.rotation);
            }
            else if (m_DangerNum == 1)
            {
                m_prBull = (GameObject)Resources.Load("Prefab/Herazika");
                Instantiate(m_prBull, m_EntrancePoint.transform.position, transform.rotation);
            }
            else
            {
                m_prBull = (GameObject)Resources.Load("Prefab/Shark");
                Instantiate(m_prBull, m_EntrancePoint.transform.position, transform.rotation);
            }
            m_CurentApperTimeIndex++;
            m_Num++;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bull")
        {
            m_prBull = null;
            m_bullApper = false;
            //m_Num--;
        }
    }

    public bool BullApper()
    {
        return m_bullApper;
    }

}