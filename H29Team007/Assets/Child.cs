using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ChildState
{
    //ノーマルモード
    NormalMode,
    //親を追いかけるモード
    WarningMode,
    //泣きわめくモード
    CryMode
}

public class Child : MonoBehaviour {
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;

    private ChildState m_State = ChildState.NormalMode;
    private GameObject m_Parent;
    private Transform m_ParentEyePoint;
    private Transform m_LookEye;
    private Vector3 pos;
    NavMeshAgent m_Agent;


    // Use this for initialization
    void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Parent = GameObject.FindGameObjectWithTag("Parent");
        m_ParentEyePoint = m_Parent.transform.Find("ParentEye");
        m_LookEye = transform.Find("LookEye");
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 PPos = m_Parent.transform.position;
        Vector3 CPos = transform.position;
        float dis = Vector3.Distance(PPos, CPos);
        if (m_State == ChildState.NormalMode)
        {
            m_Agent.speed = 1.0f;
            m_Agent.destination = PPos;
            if(dis >= 3.0f)
            {
                m_State = ChildState.WarningMode;
            }
        }
        else if(m_State == ChildState.WarningMode)
        {
            m_ViewingDistance = 1000;
            m_ViewingAngle = 360;
            if (CanSeePlayer())
            {
                m_Agent.speed = 3.0f;
                m_Agent.destination = PPos;
            }
            else
            {
                m_State = ChildState.CryMode;
            }
        }
        else if(m_State == ChildState.CryMode)
        {
            DoPatrol();
        }
    }

     //親が見える距離内にいるか？
    bool IsParentInViewingDistance()
    {
        //自身から親までの距離
        float distanceToPlayer = Vector3.Distance(m_ParentEyePoint.position, m_LookEye.position);
        //親が見える距離内にいるかどうかを返却する
        return (distanceToPlayer <= m_ViewingDistance);
    }

    //親が見える視野角内にいるか？
    bool IsParentInViewingAngle()
    {
        //自分から親への方向ベクトル(ワールド座標系)
        Vector3 directionToPlayer = m_ParentEyePoint.position - m_LookEye.position;
        // 自分の正面向きベクトルと親への方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_LookEye.forward, directionToPlayer);

        // 見える視野角の範囲内に親がいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    // 親にRayを飛ばしたら当たるか？
    bool CanHitRayToParent()
    {
        // 自分から親への方向ベクトル（ワールド座標系）
        Vector3 directionToPlayer = m_ParentEyePoint.position - m_LookEye.position;
        // 壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_ParentEyePoint.position, directionToPlayer, out hitInfo);
        // 親にRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "Parent");
    }

    // 親が見えるか？
    bool CanSeePlayer()
    {
        // 見える距離の範囲内に親がいない場合→見えない
        if (!IsParentInViewingDistance())
            return false;
        // 見える視野角の範囲内に親がいない場合→見えない
        if (!IsParentInViewingAngle())
            return false;
        // Rayを飛ばして、それが親に当たらない場合→見えない
        if (!CanHitRayToParent())
            return false;
        // ここまで到達したら、それは親が見えるということ
        return true;
    }

    public void DoPatrol()
    {
        if (m_Agent.enabled == false) return;
        var x = Random.Range(-10.0f, 10.0f);
        var z = Random.Range(-10.0f, 10.0f);
        pos = new Vector3(x, 0, z);
        m_Agent.SetDestination(pos);
    }
}
