using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //ＢＢＡの巡回ポイント
    [SerializeField, Header("BBAのノーマル巡回ルート")]
    public Transform[] m_PatrolPoints;
    //ＢＢＡの巡回ポイント2
    [SerializeField, Header("BBAの特売品モードの巡回ルート")]
    public Transform[] m_Patrolpoints2;
    [SerializeField, Header("闘牛の特売品モードの巡回ルート")]
    public Transform[] m_BullPatrolPoints;

    //レジから出口のポイント
    [SerializeField, Header("レジポイントと出口を入れる")]
    public Transform[] m_ReziExitpoints;
    [SerializeField, Header("特売品出現場所をすべて入れる")]
    public GameObject[] m_SaleAnimalSpowns;
    [System.NonSerialized]
    public SaleSpown m_scSaleSpown;
    //ＢＢＡの現在の巡回ポイントのインデックス
    [System.NonSerialized]
    public int m_CurrentPatrolPointIndex = 1;
    [System.NonSerialized]
    public int m_CurrentPatrolPoint2Index = 1;
    [System.NonSerialized]
    public int m_CurrentPatrolPoint3Index = 1;
    //闘牛の現在の巡回ポイントのインデックス
    [System.NonSerialized]
    public int m_CurentBullPatrolPointIndex = 1;

    //BBAオブジェクト
    private GameObject m_BBA;
    //警備員オブジェクト
    private GameObject m_SG;
    BBA m_BBAScript;
    //サウンドマネージャーオブジェクト
    public GameObject SM;
    //サウンドマネージャースクリプト
    SoundManagerScript smScript;

    // Use this for initialization
    void Start () {
        m_BBA = GameObject.FindGameObjectWithTag("BBA");
        m_BBAScript = m_BBA.GetComponent<BBA>();
        smScript = SM.transform.GetComponent<SoundManagerScript>();
        smScript.PlayBGM(2);
        //スクリプトSaleSpownへの参照
        for (int i = 0; i < m_SaleAnimalSpowns.Length; i++)
        {
            m_scSaleSpown = m_SaleAnimalSpowns[i].GetComponent<SaleSpown>();
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
