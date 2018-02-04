using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MTSecurity : MonoBehaviour {

    // 敵の状態種別
    public enum EnemyState
    {
        // 巡回中
        Patrolling,
        // 追跡中
        Chasing,
        // 追跡中（見失っている）
        ChasingButLosed,
        //　避ける
        Avoid
    }

    //巡回ポイント
    private Transform[] m_PatrolPoints;
    //見える距離
    float m_ViewingDistance;
    //視野角
    float m_ViewingAngle;

    private MTPlayer m_scPlayer;
    private EnemyState m_State = EnemyState.Patrolling;
    private Animator m_Animator;
    private AudioSource m_AS;
    NavMeshAgent m_Agent;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    Transform m_Child;
    Transform m_clerk;
    MTRunOver m_run;
    bool m_bool = false;
    float radius = 100f;
    float minAngle = 0.0F;
    float maxAngle = 90.0F;
    float m_Horizntal = 0;
    float m_ho = -2.0f;
    float m_Hearingtime = 0.0f;
    Rigidbody m_rb;

    // Use this for initialization
    void Start()
    {
        //タグでパトロールポイントの親を検索して保持
        GameObject m_PatrolPoint = GameObject.FindGameObjectWithTag("PatrolPoint");
        m_PatrolPoints = new Transform[m_PatrolPoint.transform.childCount];
        //パトロールポイントの子を取得
        for (int i = 0; m_PatrolPoint.transform.childCount > i; i++)
        {
            m_PatrolPoints[i] = m_PatrolPoint.transform.GetChild(i);
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
        m_scPlayer = m_Player.GetComponent<MTPlayer>();
        m_AS = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PPos = m_Player.transform.position;
        Vector3 EPos = transform.position;
        float dis = Vector3.Distance(PPos, EPos);
        //if (m_scPlayer.GetState() == MTPlayer.PlayerState.Outside)
        //{
        //    m_Agent.speed = 1f;
        //    m_Animator.SetTrigger("Trigger");
        //    m_State = EnemyState.Patrolling;
        //}
        //巡回中
        if (m_State == EnemyState.Patrolling)
        {
            m_Agent.speed = 1f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            m_bool = false;
            if (CanSeePlayer() && m_bool == false && dis <= 5 && m_scPlayer.GetState() == MTPlayer.PlayerState.Gliding)
            {
                m_Agent.enabled = false;
                m_bool = true;
                m_Animator.SetTrigger("Jump2");

                iTween.MoveTo(gameObject, iTween.Hash(
                    "x", transform.position.x - 2,
                    "z", transform.position.z - 2,
                    "easeType", iTween.EaseType.linear,
                    "time", 2.5f,
                    "oncomplete", "OnCompleteHandler",
                    "oncompletetarget", this.gameObject));
            }
            //カゴに人を乗せているプレイヤーが見えた場合
            if (CanSeePlayer() && m_scPlayer.IsGetHuman())
            {
                //  m_smScript.PlaySE(1);
                m_AS.Play();
                //追跡中に状態変更
                m_State = EnemyState.Chasing;
                m_Agent.destination = m_Player.transform.position;
            }
            //プレイヤーが見えなくて、目的地に到着した場合
            if (HasArrived())
            {
                //目的地を次の巡回ポイントに切り替える
                SetNewPatrolPointToDestination();
            }
        }
        // プレイヤーを追跡中
        else if (m_State == EnemyState.Chasing)
        {
            m_ViewingDistance = 1000;
            m_ViewingAngle = 360;

            if (CanSeePlayer() && m_bool == false && dis <= 5 && m_scPlayer.GetState() == MTPlayer.PlayerState.Gliding)
            {
                m_Agent.enabled = false;
                m_bool = true;
                m_Animator.SetTrigger("Jump2");

                iTween.MoveTo(gameObject, iTween.Hash(
                    "x", transform.position.x - 2,
                    "z", transform.position.z - 2,
                    "easeType", iTween.EaseType.linear,
                    "time", 2.0f,
                    "oncomplete", "OnCompleteHandler",
                    "oncompletetarget", this.gameObject));
            }

            // プレイヤーが見えている場合
            if (CanSeePlayer() && m_scPlayer.IsGetHuman())
            {
                m_Agent.speed = 3.0f;
                // プレイヤーの場所へ向かう
                m_Agent.destination = m_Player.transform.position;
                if (dis <= 3 && m_bool == false)
                {
                    m_Animator.SetTrigger("Jump");
                    m_bool = true;
                }
                else if (dis > 3 && m_bool == true)
                {
                    m_bool = false;
                    m_State = EnemyState.Patrolling;
                    m_Animator.SetTrigger("Trigger");
                }
            }
            // 見失った場合
            else
            {
                // 追跡中（見失い中）に状態変更
                m_State = EnemyState.ChasingButLosed;
                m_Agent.speed = 1f;
            }
        }
        // 追跡中（見失い中）の場合
        else if (m_State == EnemyState.ChasingButLosed)
        {
            m_ViewingDistance = 3;
            m_ViewingAngle = 140;
            if (CanSeePlayer() && m_scPlayer.IsGetHuman())
            {
                m_Agent.speed = 3.0f;
                // 追跡中に状態変更
                m_State = EnemyState.Chasing;
                m_Agent.destination = m_Player.transform.position;
            }
            // プレイヤーを見つけられないまま目的地に到着
            else if (HasArrived())
            {
                // 巡回中に状態遷移
                m_State = EnemyState.Patrolling;
            }
        }
        //  Debug.Log(dis);
        m_Animator.SetFloat("Speed", m_Agent.speed);

    }

    void OnCompleteHandler()
    {
        m_Animator.SetTrigger("Trigger");
        m_Agent.enabled = true;
        m_Agent.speed = 0;
        m_bool = false;
    }

    //次の巡回ポイントを目的地に設定する
    void SetNewPatrolPointToDestination()
    {

        int m_rand = Random.Range(0, m_PatrolPoints.Length);
        m_Agent.destination = m_PatrolPoints[m_rand].position;
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

    //プレイヤーを追いかける   
    public bool StateChasing()
    {
        return (m_State == EnemyState.Chasing);
    }

    public bool Guard()
    {
        return m_bool;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")//プレイヤーババア用　敵ババアが特売品を轢く処理は頑張って
        {
            
            m_ViewingAngle = 0.0f;
            m_ViewingDistance = 0.0f;
        }
    }
}
