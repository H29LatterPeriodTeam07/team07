using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//　　　　　　　　　　　　　　　　 /
//　　　　　　　　　　 　 　 　 　 |　オ さ
//　　　　　　　　　　　　　　　　」　メ て
//＿_ ＞ ´￣￣￣｀ ＜⌒Y＼      ｀}   | は
//_／　　　　　　　ﾒ､　　＼　ヽ          ア
//　　/{ '⌒､＿  / ＼} ヽ 　∨∧.　　    ン
//　　{　ｒ云     芹云ﾐｉ   ｉ |         チ
//　　|　んハ     ヒハ}　ｉ  |＿ﾄｊ}　   だ
//　　|　ヒ ツ  , ゞ ツ　{ 　 }　∧　　  な
//　　|　　 　ｰｰ'　　　　　ﾌ /　　∧
//　　|　　　　　　＿＿ イ ∧　　　 ＼＿／
//＼ 厂￣￣ﾏ У / }/　 ＼


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

public class SecurityGuard : MonoBehaviour
{
    //巡回ポイント
    public Transform[] m_PatrolPoints;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public GameObject m_Enemy;
    public AudioClip m_se;

    private Player m_scPlayer;
    private EnemyState m_State = EnemyState.Patrolling;
    private Animator m_Animator;
    private AudioSource m_AS;
    NavMeshAgent m_Agent;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    //プレイヤーへの参照
    GameObject m_Player;
    Player m_PlayerSC;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    public GameObject m_SoundManager;
    SoundManagerScript m_smScript;
    RunOverObject m_run;
    bool m_bool = false;

    float minAngle = 0.0F;
    float maxAngle = 90.0F;
    float m_Horizntal = 0;
    float m_ho = -2.0f;
    Rigidbody m_rb;
    SecurityGuard m_scScrpt;

    // Use this for initialization
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerSC = GetComponent<Player>();
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("LookEye");
        m_Animator = GetComponent<Animator>();
        m_scPlayer = m_Player.GetComponent<Player>();
        m_smScript = m_SoundManager.GetComponent<SoundManagerScript>();
        m_AS = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
        m_scScrpt = GetComponent<SecurityGuard>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 PPos = m_Player.transform.position;
        Vector3 EPos = m_Enemy.transform.position;
        float dis = Vector3.Distance(PPos, EPos);
        //巡回中
        if (m_State == EnemyState.Patrolling)
        {
            m_Agent.speed = 1f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            if (CanSeePlayer() && m_bool == false && dis <= 5 && m_scPlayer.GetState() == Player.PlayerState.Gliding)
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
            //プレイヤーが見えた場合
            if (CanSeePlayer() && m_scPlayer.IsGetHuman())
            {
                //  m_smScript.PlaySE(1);
                m_AS.Play();
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
            m_ViewingDistance = 1000;
            m_ViewingAngle = 360;

            if (CanSeePlayer() && m_bool == false && dis <= 5 && m_scPlayer.GetState() == Player.PlayerState.Gliding)
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
                else if (dis > 5 && m_bool == true)
                {
                    m_bool = false;
                    m_Animator.SetTrigger("Trigger");
                }
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
            m_bool = false;
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

    //プレイヤーを追いかける   
    public bool StateChasing()
    {
        return m_State == EnemyState.Chasing;
    }

    public bool Guard()
    {
        return m_bool;
    }
}