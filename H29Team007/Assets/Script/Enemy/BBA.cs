using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum BBAState
{
    //ノーマルモード
    NormalMode,
    //特売品モード
    SaleMode,
    //攻撃モード
    attackMode,
    //レジへ向かうモード
    CashMode,
    //ノーカートモード
    NoCart,
    CratIn
}

public class BBA : MonoBehaviour
{
    [SerializeField,Header("クソババアのカートを入れて")]
    private GameObject myCart;
    [SerializeField, Header("GameManagerのm_gdのElementと同じ数字を入れてケロ")]
    private int m_int;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public BBACartCount m_scBBAcount;
    [System.NonSerialized]
    public NavMeshAgent m_Agent;

    public GameObject m_BBABasket;

    private BBAState m_State = BBAState.NormalMode;
    private float m_Speed = 1.0f;
    private Rigidbody rb;
    private GameObject m_GameManager;
    private GameManager m_gmScript;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    //特売品への参照
    GameObject m_SaleAnimals;
    //特売品への注視点
    Transform m_SaleAnimalsLookPoint;
    Transform m_Animal;
    Transform m_basket;
    int m_SaleSpownIndex = 0;
    BBACartCount bcScript;
    private Animator m_Animator;
    bool m_bo = true;
    float radius = 5f;
    private LayerMask raycastLayer;
    GameObject m_PatrolPoint;
    GameObject[] m_PatrolPoints;
    int m_rand;
    private GameObject m_Cart;
    Animator m_Anime;

    // Use this for initialization
    void Start()
    {
        m_Anime = GetComponent<Animator>();
        m_Cart = GameObject.FindGameObjectWithTag("EnemyCart");
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_gmScript = m_GameManager.GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        bcScript = GetComponent<BBACartCount>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        m_EyePoint = transform.Find("BBAEye");
        raycastLayer = 1 << LayerMask.NameToLayer("Animal");
        m_basket = transform.Find("EnemyBasket");
        //タグでパトロールポイントの親を検索して保持
        m_PatrolPoint = GameObject.FindGameObjectWithTag("PatrolPoint");
        m_PatrolPoints = new GameObject[m_PatrolPoint.transform.childCount];
        //パトロールポイントの子を取得
        for (int i = 0; m_PatrolPoint.transform.childCount > i; i++)
        {
            m_PatrolPoints[i] = m_PatrolPoint.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null) return;
        //巡回中
        if (m_State == BBAState.NormalMode)
        {
            if (myCart == null)
            {
                SetNewRPatrolPointToDestination();
                m_Anime.SetTrigger("DDK");
                m_State = BBAState.NoCart;
            }
            m_Agent.speed = 1.0f;
            m_Animator.SetFloat("Speed", m_Agent.speed);
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            //特売品が出てくる時間になったら特売品モードに
            if (m_gmScript.m_scSaleSpown.SaleMode())
            {
                SetNewSalePatrolPointToDestination();
                //特売品モードに状態変更
                m_State = BBAState.SaleMode;
            }
            else
            {
                if (BBAHasArrived())
                {
                    SetNewPatrolPointToDestination();
                }
            }
            if (transform.root.tag == "Player")
            {
                m_State = BBAState.CratIn;
            }
        }
        //特売品モード
        else if (m_State == BBAState.SaleMode)
        {
            m_Agent.speed = 4.0f;
            m_Animator.SetFloat("Speed", m_Agent.speed);
            // SetNewSalePatrolPointToDestination();
            m_ViewingDistance = 100;
            m_ViewingAngle = 180;

            if (myCart == null)
            {
                SetNewRPatrolPointToDestination();
                m_Anime.SetTrigger("DDK");
                m_State = BBAState.NoCart;
            }

            if (BBAHasArrived())
            {
                SetNewSalePatrolPointToDestination();
            }

            if (m_Animal == null)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, raycastLayer);
                foreach(Collider hit in hitColliders)
                {
                    if (m_Animal != null || hit.transform.parent != null) continue;
                    m_Animal = hit.transform;
                }
            }

            else if (m_Animal != null)
            {
                m_Agent.destination = m_Animal.transform.position;

                if(m_Animal.parent != null)
                {
                    m_Animal = null;
                    //m_State = BBAState.NormalMode;
                }
            }

            if (IsGetAnimal())
            {
                SetNewExitPointToDestination();
                m_State = BBAState.CashMode;
            }
            if (transform.root.tag == "Player")
            {
                m_State = BBAState.CratIn;
            }
        }

        //レジ～出入り口へGOモード
        else if (m_State == BBAState.CashMode)
        {
        
            m_Speed = 4.0f;
            if (BBAHasArrived())
            {
                SetNewExitPointToDestination();
            }
            if (myCart == null)
            {
                SetNewRPatrolPointToDestination();
                m_Anime.SetTrigger("DDK");
                m_State = BBAState.NoCart;
            }

            if (transform.root.tag == "Player")
            {
                m_State = BBAState.CratIn;
            }

            if (!IsGetAnimal()) m_State = BBAState.NormalMode;
        }

        else if (m_State == BBAState.NoCart)
        {
            if (!m_Agent.enabled) m_Agent.enabled = true;
            m_Agent.speed = 1.0f;
            if (BBAHasArrived())
            {
                SetNewRPatrolPointToDestination();
            }
            if (transform.root.tag == "Player")
            {
                m_State = BBAState.CratIn;
            }
        }

        else if(m_State == BBAState.CratIn)
        {
            m_Agent.speed = 0.0f;

            if (transform.parent == null)
            {
                m_State = BBAState.NoCart;
            }
        }
    }

    void SetNewRPatrolPointToDestination()
    {
        m_rand = Random.Range(0, m_PatrolPoints.Length);
        m_Agent.destination = m_PatrolPoints[m_rand].transform.position;
    }

    //次の巡回ポイントを目的地に設定する
    public void SetNewPatrolPointToDestination()
    {
        for (int i = 0; i < m_gmScript.m_gd.Length; i++)
        {
            if (i == 0 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPointIndex
                    = (m_gmScript.m_gd[i].m_CurrentPatrolPointIndex + 1) % m_gmScript.m_gd[i].m_PatrolPoints.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_PatrolPoints[m_gmScript.m_gd[i].m_CurrentPatrolPointIndex].position;
            }
            else if (i == 1 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPointIndex
                    = (m_gmScript.m_gd[i].m_CurrentPatrolPointIndex + 1) % m_gmScript.m_gd[i].m_PatrolPoints.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_PatrolPoints[m_gmScript.m_gd[i].m_CurrentPatrolPointIndex].position;
            }
            else if (i == 2 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPointIndex
                    = (m_gmScript.m_gd[i].m_CurrentPatrolPointIndex + 1) % m_gmScript.m_gd[i].m_PatrolPoints.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_PatrolPoints[m_gmScript.m_gd[i].m_CurrentPatrolPointIndex].position;
            }
        }
    }

    public void SetNewSalePatrolPointToDestination()
    {
        for (int i = 0; i < m_gmScript.m_gd.Length; i++)
        {
            if (i == 0 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_gd[i].m_Patrolpoints2.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_Patrolpoints2[m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index].position;
            }
            else if (i == 1 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_gd[i].m_Patrolpoints2.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_Patrolpoints2[m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index].position;
            }
            else if (i == 2 && m_int == i)
            {
                m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index + 1) % m_gmScript.m_gd[i].m_Patrolpoints2.Length;

                m_Agent.destination = m_gmScript.m_gd[i].m_Patrolpoints2[m_gmScript.m_gd[i].m_CurrentPatrolPoint2Index].position;
            }
        }             
    }

    public void SetNewExitPointToDestination()
    {
        for (int i = 0; i < m_gmScript.m_gd.Length; i++)
        {
            if (i == 0 && m_int == i)
            {
                m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint3Index + 1) % m_gmScript.m_ReziExitpoints.Length;

                m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
            }
            else if (i == 1 && m_int == i)
            {
                m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint3Index + 1) % m_gmScript.m_ReziExitpoints.Length;

                m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
            }
            else if (i == 2 && m_int == i)
            {
                m_gmScript.m_CurrentPatrolPoint3Index
            = (m_gmScript.m_gd[i].m_CurrentPatrolPoint3Index + 1) % m_gmScript.m_ReziExitpoints.Length;

                m_Agent.destination = m_gmScript.m_ReziExitpoints[m_gmScript.m_CurrentPatrolPoint3Index].position;
            }
        }
    }

    // 目的地に到着したか
    public bool BBAHasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

    public bool IsGetAnimal()
    {
        return m_scBBAcount.IsBaggegeinHuman();
    }
    public bool NoCart()
    {
        m_Agent.enabled = false;
        myCart = null;
        return m_State == BBAState.NoCart;
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
        if (other.name == "FrontHitArea")
        {
            if (transform.tag == "BBA" && !CanGetEnemy(other.transform)) { return; }
            if (other.transform.root.GetComponent<Player>().GetFowardSpeed() <= 0.1f * 0.1f) return;
            bcScript.BaggegeFall(transform.position);
            Destroy(myCart.gameObject);
            m_Animator.SetTrigger("Kago");
        }

        if (other.tag == "ExitPoint")
        {
            if (IsGetAnimal())
            {
                gameObject.SetActive(false);
                bcScript.BaggegeFall(transform.position);
            }
            Invoke(("BBAReset"),3);
        }

    }

    void BBAReset()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        m_Animal = null;
        gameObject.SetActive(true);
        m_State = BBAState.NormalMode;
        SetNewPatrolPointToDestination();
    }
}