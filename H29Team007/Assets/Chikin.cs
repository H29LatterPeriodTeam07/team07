using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chikin : MonoBehaviour {

    //巡回ポイント
    public Transform[] m_PatrolPoints;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    NavMeshAgent m_Agent;
    float m_speed;
    private Vector3 pos;

    // Use this for initialization
    void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
        //SetNewPatrolPointToDestination();
       // DoPatrol();
      //  DoSpeed();
    }
	
	// Update is called once per frame
	void Update () {
        m_Agent.speed = 3;
        //SetNewPatrolPointToDestination();
        DoPatrol();
        //if (HasArrived()) SetNewPatrolPointToDestination();
    }

    void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex
            = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

        m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;
    }

    // 目的地に到着したか
    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

    void DoSpeed()
    {
        var speed = Random.Range(0, 3.0f);

        m_speed = speed;
    }

    public void DoPatrol()
    {
        if (m_Agent.enabled == false) return;
        var x = Random.Range(-100.0f, 100.0f);
        var z = Random.Range(-100.0f, 100.0f);
        pos = new Vector3(x, 0, z);
        m_Agent.SetDestination(pos);
    }
}
