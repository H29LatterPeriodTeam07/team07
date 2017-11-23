using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fightingBull : MonoBehaviour {

    GameObject m_GameManager;
    GameManager m_gmScript;
    NavMeshAgent m_Agent;
    BoxCollider m_Box;
    BullCount bcScript;

    // Use this for initialization
    void Start() {
        m_Agent = GetComponent<NavMeshAgent>();
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_gmScript = m_GameManager.GetComponent<GameManager>();
        BullSetNewPatrolPoint();
        bcScript = transform.GetComponent<BullCount>();

    }
	
	// Update is called once per frame
	void Update () {
        m_Agent.speed = 8.0f;
        if (BUllHasArrived())
        {
            BullSetNewPatrolPoint();
        }
	}
    void BullSetNewPatrolPoint()
    {
        m_gmScript.m_CurentBullPatrolPointIndex
             = (m_gmScript.m_CurentBullPatrolPointIndex + 1) % m_gmScript.m_BullPatrolPoints.Length;

        m_Agent.destination = m_gmScript.m_BullPatrolPoints[m_gmScript.m_CurentBullPatrolPointIndex].position;
    }

    bool BUllHasArrived()
    {
        
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

}
