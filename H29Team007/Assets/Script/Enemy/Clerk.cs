using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ClerkState
{
    //ノーマルモード
    NormalMode,
    //警戒モード
    WarningMode
}

public class Clerk : MonoBehaviour
{
    //巡回ポイント
   // public Transform[] m_PatrolPoints;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public AudioClip m_se;

    private ClerkState m_State = ClerkState.NormalMode;
    NavMeshAgent m_Agent;
    private Animator m_Animator;
    private AudioSource m_AS;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 0;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    int m_rand;
    GameObject m_PatrolPoint;
    GameObject[] m_PatrolPoints;
    Player m_pScript;

    // Use this for initialization
    void Start()
    {
        //タグでパトロールポイントの親を検索して保持
        m_PatrolPoint = GameObject.FindGameObjectWithTag("PatrolPoint");
        m_PatrolPoints = new GameObject[m_PatrolPoint.transform.childCount];
        //パトロールポイントの子を取得
        for (int i = 0; m_PatrolPoint.transform.childCount > i; i++)
        {
            m_PatrolPoints[i] = m_PatrolPoint.transform.GetChild(i).gameObject;
        }
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("LookEye");
        m_Animator = GetComponent<Animator>();
        m_rand = Random.Range(0, m_PatrolPoints.Length);
        m_AS = GetComponent<AudioSource>();
        m_pScript = m_Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        print(m_State);
        //巡回中
        if (m_State == ClerkState.NormalMode)
        {
            m_Agent.speed = 1.0f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            //プレイイヤーが見えた場合
            if (CanSeePlayer() && m_pScript.IsGetHuman())
            {
                m_Agent.speed = 1.0f;
                //追跡中に状態変更
                m_State = ClerkState.WarningMode;
            }
            //プレイヤーが見えなくて、目的地に到着した場合
            else if (HasArrived())
            {
                m_rand = Random.Range(0, m_PatrolPoints.Length);
                //目的地を次の巡回ポイントに切り替える
                SetNewPatrolPointToDestination();
            }
        }
        // 警備員を呼ぶ
        else if (m_State == ClerkState.WarningMode)
        {
            m_Agent.speed = 0.0f;
            if (m_pScript.GetState() == Player.PlayerState.Outside)
            {
                m_State = ClerkState.NormalMode;
            }
        }
        m_Animator.SetFloat("Speed", m_Agent.speed);
    }

    //次の巡回ポイントを目的地に設定する
    void SetNewPatrolPointToDestination()
    {

        m_Agent.destination = m_PatrolPoints[m_rand].transform.position;
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

    public bool warning()
    {
        return m_State == ClerkState.WarningMode;
    }
}