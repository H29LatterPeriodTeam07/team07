using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject m_time;
    [SerializeField, Header("BBAのプレハブ")]
    private GameObject m_PreBBA;
    [SerializeField, Header("闘牛のプレハブ")]
    private GameObject m_prBull;
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
    BullCount m_bc;

    // Use this for initialization
    void Start()
    {
        m_GameBBA = GameObject.FindGameObjectWithTag("BBA");
        m_timer = m_time.GetComponent<Timer>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        pScript = m_player.GetComponent<Player>();
        m_bc = m_prBull.transform.root.GetComponent<BullCount>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bullApperTime.Length > m_CurentApperTimeIndex)
        {
            if (m_timer.timer > m_bullApperTime[m_CurentApperTimeIndex])
            {
                m_CurentApperTimeIndex++;
                Appear();
            }
        }
    }

    public void Appear()
    {
        //m_bullApper = true;
        //if (m_Num < 1)
        //{
        //    Instantiate(m_prBull, m_EntrancePoint.transform.position, transform.rotation);
        //    m_Num++;
        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BBA")
        {
            Destroy(other.gameObject);
            Instantiate(m_PreBBA, m_EntrancePoint.transform.position, transform.rotation);
        }

        if (other.tag == "Bull")
        {
            
            m_bullApper = false;
            //Destroy(other.gameObject);
            m_Num--;
        }
    }

    public bool BullApper()
    {
        return m_bullApper;
    }
}