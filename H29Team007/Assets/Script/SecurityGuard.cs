using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 敵の状態種別
public enum EnemyState
{
    // 巡回中
    Patrolling,
    // 追跡中
    Chasing,
    // 追跡中（見失っている）
    ChasingButLosed,
}

public class SecurityGuard : MonoBehaviour {
    //巡回ポイント
    public Transform[] m_PatrolPoints;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;

    private EnemyState m_State = EnemyState.Patrolling;
    private float m_Speed = 1.0f;
    NavMeshAgent m_Agent;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;


	// Use this for initialization
	void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("LookEye");
	}

    // Update is called once per frame
    void Update()
    {
        //巡回中
        if (m_State == EnemyState.Patrolling)
        {
            m_Agent.speed = 3.5f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            //プレイイヤーが見えた場合
            if (CanSeePlayer())
            {
                //追跡中に状態変更
                m_State = EnemyState.Chasing;
                m_Agent.destination = m_Player.transform.position;
            }
            //プレイヤーが見えなくて、目的地に到着した場合
            else if (HasArrived())
            {
                //目的地を次の巡回ポイントに切り替える
                SetNewPatrolPointToDestination();
            }
        }
        // プレイヤーを追跡中
        else if (m_State == EnemyState.Chasing)
        {
            // プレイヤーが見えている場合
            if (CanSeePlayer())
            {
                m_Agent.speed = 10.0f;
                // プレイヤーの場所へ向かう
                m_Agent.destination = m_Player.transform.position;
                m_ViewingDistance = 1000;
                m_ViewingAngle = 360;
            }
            // 見失った場合
            else
            {
                // 追跡中（見失い中）に状態変更
                m_State = EnemyState.ChasingButLosed;
            }
        }
        // 追跡中（見失い中）の場合
        else if (m_State == EnemyState.ChasingButLosed)
        {
            if (CanSeePlayer())
            {
                m_Agent.speed = 3.5f;
                // 追跡中に状態変更
                m_State = EnemyState.Chasing;
                m_Agent.destination = m_Player.transform.position;
                m_ViewingDistance = 1000;
                m_ViewingAngle = 360;
            }
            // プレイヤーを見つけられないまま目的地に到着
            else if (HasArrived())
            {
                    // 巡回中に状態遷移
                    m_State = EnemyState.Patrolling;
            }

        }
    }

        //次の巡回ポイントを目的地に設定する
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

    //プレイヤーが見える距離内にいるか？
    bool IsPlayerInViewingDistance()
    {
        //自身からプレイヤーまでの距離
        float distanceToPlayer = Vector3.Distance(m_PlayerLookpoint.position, m_EyePoint.position);
        //プレイヤーが見える距離内にいるかどうかを返却する
        return (distanceToPlayer <= m_ViewingDistance);
    }

    //プレイヤーが見える視野角内にいるか？
    bool IsPlayerInViewingAngle()
    {
        //自分からプレイヤーへの方向ベクトル(ワールド座標系)
        Vector3 directionToPlayer = m_PlayerLookpoint.position - m_EyePoint.position;
        // 自分の正面向きベクトルとプレイヤーへの方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        // 見える視野角の範囲内にプレイヤーがいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    // プレイヤーにRayを飛ばしたら当たるか？
    bool CanHitRayToPlayer()
    {
        // 自分からプレイヤーへの方向ベクトル（ワールド座標系）
        Vector3 directionToPlayer = m_PlayerLookpoint.position - m_EyePoint.position;
        // 壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);
        // プレイヤーにRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "Player");
    }

    // プレイヤーが見えるか？
    bool CanSeePlayer()
    {
        // 見える距離の範囲内にプレイヤーがいない場合→見えない
        if (!IsPlayerInViewingDistance())
            return false;
        // 見える視野角の範囲内にプレイヤーがいない場合→見えない
        if (!IsPlayerInViewingAngle())
            return false;
        // Rayを飛ばして、それがプレイヤーに当たらない場合→見えない
        if (!CanHitRayToPlayer())
            return false;
        // ここまで到達したら、それはプレイヤーが見えるということ
        return true;
    }
}
