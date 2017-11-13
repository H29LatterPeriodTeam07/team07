using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //ＢＢＡの巡回ポイント
    public Transform[] m_PatrolPoints;
    //ＢＢＡの巡回ポイント2
    public Transform[] m_Patrolpoints2;
    //レジから出口のポイント
    public Transform[] m_ReziExitpoints;
    public GameObject[] m_SaleAnimalSpowns;
    [System.NonSerialized]
    public SaleSpown m_scSaleSpown;

    //BBAオブジェクト
    private GameObject m_BBA;
    //警備員オブジェクト
    private GameObject m_SG;
    BBA m_BBAScript;

    //サウンドマネージャーオブジェクト
    GameObject SM;
    //サウンドマネージャースクリプト
    SoundManagerScript smScript;
    //ＢＢＡの現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    int m_CurrentPatrolPoint2Index = 1;
    int m_CurrentPatrolPoint3Index = 1;

    // Use this for initialization
    void Start () {
        m_BBA = GameObject.FindGameObjectWithTag("BBA");
        m_BBAScript = m_BBA.GetComponent<BBA>();
        SM = GameObject.Find("SoundManager");
        smScript = SM.transform.GetComponent<SoundManagerScript>();
        smScript.PlayBGM(0);
        //スクリプトSaleSpownへの参照
        for (int i = 0; i < m_SaleAnimalSpowns.Length; i++)
        {
            m_scSaleSpown = m_SaleAnimalSpowns[i].GetComponent<SaleSpown>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void EnterShopBBA()
    {

    }

    //次の巡回ポイントを目的地に設定する
    public void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex
            = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

        m_BBAScript.m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;
    }

    public void SetNewSalePatrolPointToDestination()
    {
        m_CurrentPatrolPoint2Index
            = (m_CurrentPatrolPoint2Index + 1) % m_Patrolpoints2.Length;

        m_BBAScript.m_Agent.destination = m_Patrolpoints2[m_CurrentPatrolPoint2Index].position;
    }

    public void SetNewExitPointToDestination()
    {
        m_CurrentPatrolPoint3Index
            = (m_CurrentPatrolPoint2Index + 1) % m_ReziExitpoints.Length;

      m_BBAScript.m_Agent.destination = m_ReziExitpoints[m_CurrentPatrolPoint3Index].position;
    }

    // 目的地に到着したか
    public bool HasArrived()
    {
        return (Vector3.Distance(m_BBAScript.m_Agent.destination, m_BBA.transform.position) < 0.5f);
    }
}
