using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {
    [System.Serializable]
    public class GameData
    {
        public GameObject m_BBA;
        //ＢＢＡの巡回ポイント
        [SerializeField, Header("BBAのノーマル巡回ルート")]
        public Transform[] m_PatrolPoints;
        //ＢＢＡの巡回ポイント2
        [SerializeField, Header("BBAの特売品モードの巡回ルート")]
        public Transform[] m_Patrolpoints2;
        //ＢＢＡの現在の巡回ポイントのインデックス
        [System.NonSerialized]
        public int m_CurrentPatrolPointIndex = 0;
        [System.NonSerialized]
        public int m_CurrentPatrolPoint2Index = 0;
        [System.NonSerialized]
        public int m_CurrentPatrolPoint3Index = 0;
    }
    public GameData[] m_gd;

    [SerializeField, Header("闘牛の特売品モードの巡回ルート")]
    public Transform[] m_BullPatrolPoints;

    //レジから出口のポイント
    [SerializeField, Header("レジポイントと出口を入れる")]
    public Transform[] m_ReziExitpoints;
    [System.NonSerialized]
    public SaleSpown m_scSaleSpown;
    [System.NonSerialized]
    public int m_CurrentPatrolPoint3Index = 0;
    //闘牛の現在の巡回ポイントのインデックス
    [System.NonSerialized]
    public int m_CurentBullPatrolPointIndex = 0;

    [SerializeField,Header("太陽")]
    private GameObject m_Sun;
    [SerializeField, Header("タイマー")]
    private GameObject m_Timer;
    Timer m_tmScript;
    //BBAオブジェクト
    private GameObject m_BBA;
    //警備員オブジェクト
    private GameObject m_SG;
    BBA m_BBAScript;
    //サウンドマネージャーオブジェクト
    public GameObject SM;

    GameObject[] m_enemys;

    // Use this for initialization
    void Start () {
        m_BBA = GameObject.FindGameObjectWithTag("BBA");
        m_BBAScript = m_BBA.GetComponent<BBA>();
        //smScript = SM.transform.GetComponent<SoundManagerScript>();
        //smScript.PlayBGM(2);
        m_tmScript = m_Timer.GetComponent<Timer>();
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        m_Sun.transform.eulerAngles = new Vector3(210 / m_tmScript.StageTime() * m_tmScript.NowTime(), 0, 0);
    }

    public void EnemyDelete() {
        foreach (GameObject enemy in m_enemys)
        {

                Destroy(enemy);
            
        }
    }


}
