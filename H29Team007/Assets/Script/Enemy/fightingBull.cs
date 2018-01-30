using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fightingBull : MonoBehaviour {

    [SerializeField, Header("元のモデル")]
    public GameObject m_model;
    [SerializeField, Header("ぐてモデル")]
    public GameObject m_gutemodel;


    GameObject m_ExitPoint;
    GameObject m_GameManager;
    GameManager m_gmScript;
    NavMeshAgent m_Agent;
    BoxCollider m_Box;
    BullCount bcScript;

    int m_curent;

    // Use this for initialization
    void Start() {
        m_ExitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        m_Agent = GetComponent<NavMeshAgent>();
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_gmScript = m_GameManager.GetComponent<GameManager>();
        m_curent = m_gmScript.m_CurentBullPatrolPointIndex;
        m_Agent.destination = m_gmScript.m_BullPatrolPoints[m_curent].position;
        bcScript = transform.GetComponent<BullCount>();
    }

    // Update is called once per frame
    void Update() {
        m_Agent.speed = 8.0f;
        if (BUllHasArrived())
        {
            BullSetNewPatrolPoint();
        }
    }
    void BullSetNewPatrolPoint()
    {
        m_curent
             = (m_curent + 1) % m_gmScript.m_BullPatrolPoints.Length;

        m_Agent.destination = m_gmScript.m_BullPatrolPoints[m_curent].position;
    }

    bool BUllHasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")//プレイヤーババア用　敵ババアが特売品を轢く処理は頑張って
        {
            if (m_model != null)
            {
                m_model.SetActive(false);
                m_gutemodel.SetActive(true);
            }
        }

            if (other.tag == "ExitPoint")
        {
            bcScript.BaggegeFall(transform.position);
            Destroy(gameObject);

        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Plane")
        {
            if (m_model != null && m_gutemodel != null)
            {
                m_model.SetActive(true);
                m_gutemodel.SetActive(false);
            }
        }
    }
}



