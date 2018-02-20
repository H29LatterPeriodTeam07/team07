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
    //　避ける
    Avoid,
    // 子供
    ChildPatrol,
    //正義
    JusticeMode,
    //店員
    clerkPatrol
}

public class SecurityGuard : MonoBehaviour
{
    //巡回ポイント
    public Transform[] m_PatrolPoints;
    //見える距離
    float m_ViewingDistance;
    //視野角
    float m_ViewingAngle;
    public GameObject m_Enemy;

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
    GameObject[]m_Childlen;
    Transform m_child;
    Transform m_clerk;
    RunOverObject m_run;
    bool m_bool = false;
    float radius = 100f;
    private LayerMask raycastLayer;
    private LayerMask raycastLayer2;
    float minAngle = 0.0F;
    float maxAngle = 90.0F;
    float m_Horizntal = 0;
    float m_ho = -2.0f;
    float m_Hearingtime = 0.0f;
    Rigidbody m_rb;
    SecurityGuard m_scScrpt;
    Child m_cScript;
    Clerk m_clScript;

    // Use this for initialization
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerSC = m_Player.GetComponent<Player>();
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("LookEye");
        m_Animator = GetComponent<Animator>();
        m_scPlayer = m_Player.GetComponent<Player>();
        m_AS = GetComponent<AudioSource>();
        m_rb = GetComponent<Rigidbody>();
        m_scScrpt = GetComponent<SecurityGuard>();
        raycastLayer = 1 << LayerMask.NameToLayer("Child");
        raycastLayer2 = 1 << LayerMask.NameToLayer("Clerk");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PPos = m_Player.transform.position;
        Vector3 EPos = m_Enemy.transform.position;
        float dis = Vector3.Distance(PPos, EPos);
        if (m_scPlayer.GetState() == Player.PlayerState.Outside)
        {
            m_Agent.speed = 1f;
            m_Animator.SetTrigger("Trigger");
            m_State = EnemyState.Patrolling;
        }
        //巡回中
        if (m_State == EnemyState.Patrolling)
        {
            m_Agent.speed = 1f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            m_bool = false;
            //if (m_Child == null)
            //{
            //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, raycastLayer);
            //    if (hitColliders.Length > 0)
            //    {
            //        int randomInt = Random.Range(0, hitColliders.Length);
            //        m_Child = hitColliders[randomInt].transform;
            //    }
            //}
            if (m_child == null)
            {
                m_Childlen = GameObject.FindGameObjectsWithTag("Child");
                {
                    foreach (GameObject childs in m_Childlen)
                    {
                        m_cScript = childs.GetComponent<Child>();
                        if (m_cScript.Roaring())
                        {
                            m_child = childs.transform;
                            m_Agent.speed = 3.0f;
                            m_Agent.destination = m_child.transform.position;
                            m_State = EnemyState.ChildPatrol;
                        }
                        if (m_scPlayer.GetState() == Player.PlayerState.Outside)
                        {
                            childs.gameObject.SetActive(false);
                            m_child = null;
                            m_Hearingtime = 0;
                        }
                    }
                }
            }
            if (m_clerk == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, raycastLayer2);
                if (hitColliders.Length > 0)
                {
                    int randomInt = Random.Range(0, hitColliders.Length);
                    m_clerk = hitColliders[randomInt].transform;
                }
            }
            else if (m_clerk != null)
            {
                m_clScript = m_clerk.GetComponent<Clerk>();
                if (m_clScript.warning())
                {
                    m_Agent.speed = 3.0f;
                    m_Agent.destination = m_clerk.transform.position;
                    m_State = EnemyState.clerkPatrol;
                }
                if (m_scPlayer.GetState() == Player.PlayerState.Outside)
                {
                    m_clerk.gameObject.SetActive(false);
                    m_clerk = null;
                    m_Hearingtime = 0;
                }
            }
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
        else if (m_State == EnemyState.ChildPatrol)
        {
            if (HasArrived())
            {
                m_Agent.speed = 0;
                m_Hearingtime += Time.deltaTime;
                if (m_Hearingtime > 3)
                {
                    if (m_PlayerSC.IsGetHuman())
                    {
                        m_Agent.speed = 3;
                        m_Agent.destination = m_Player.transform.position;
                        m_State = EnemyState.JusticeMode;
                    }
                    else
                    {
                        m_child.gameObject.SetActive(false);
                        m_child = null;
                        m_Hearingtime = 0;
                        m_Agent.speed = 1f;
                        m_State = EnemyState.Patrolling;
                    }
                }
                if (m_scPlayer.GetState() == Player.PlayerState.Outside)
                {
                    m_child.gameObject.SetActive(false);
                    m_child = null;
                    m_Hearingtime = 0;
                    m_Agent.speed = 1f;
                    m_State = EnemyState.Patrolling;
                }
            }
        }
        else if (m_State == EnemyState.clerkPatrol)
        {
            if (HasArrived())
            {
                m_Agent.speed = 0;
                m_Hearingtime += Time.deltaTime;
                if (m_Hearingtime > 3)
                {
                    m_Agent.speed = 3;
                    m_Agent.destination = m_Player.transform.position;
                    m_State = EnemyState.JusticeMode;
                }
                if (m_scPlayer.GetState() == Player.PlayerState.Outside)
                {
                    m_clerk = null;
                    m_Hearingtime = 0;
                    m_Agent.speed = 1f;
                    m_State = EnemyState.Patrolling;
                }
            }
        }
        else if (m_State == EnemyState.JusticeMode)
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
        return (m_State == EnemyState.Chasing || m_State == EnemyState.JusticeMode);
    }

    public bool Guard()
    {
        return m_bool;
    }
    /// <summary>エネミーのプレイヤーが見えてるかのパクリのパクリ</summary>
    private bool CanGetEnemy(Transform cart)
    {
        if (transform.tag == "Enemy")
        {
            SecurityGuard sg = gameObject.GetComponent<SecurityGuard>();
            if (sg.Guard()) return false;
        }
        //カートからエネミーへの方向ベクトル(ワールド座標系)
        Vector3 directionToEnemy = transform.position - cart.position;
        // エネミーの正面向きベクトルとエネミーへの方向ベクトルの差分角度
        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

        // 引ける角度の範囲内にエネミーがいるかどうかを返却する
        return (Mathf.Abs(angleToEnemy) <= 90);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "FrontHitArea")//プレイヤーババア用　敵ババアが特売品を轢く処理は頑張って
        {
            if (transform.tag == "Enemy" && !CanGetEnemy(other.transform))
            {
                return;
            }
            m_ViewingAngle = 0.0f;
            m_ViewingDistance = 0.0f;
        }
        if (other.name == "BullHitArea")//闘牛用
        {
            m_ViewingAngle = 0.0f;
            m_ViewingDistance = 0.0f;
        }
    }
}