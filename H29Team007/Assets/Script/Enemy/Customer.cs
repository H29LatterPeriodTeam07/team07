using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    //ノーマルモード
    NormalMode,
    RoaringMode
}

public class Customer : MonoBehaviour {

    [SerializeField,Header("Trueなら子供を生成させる")]
    private bool _Child = false;

    //巡回ポイント
   // public Transform[] m_PatrolPoints;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;

    private CustomerState m_State = CustomerState.NormalMode;
    private Animator m_Animator;
    NavMeshAgent m_Agent;
    Player m_pScript;
    //プレイヤーへの参照
    GameObject m_Player;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    int m_rand;
    GameObject m_PatrolPoint;
    GameObject[] m_PatrolPoints;
    GameObject m_Child;


    // Use this for initialization
    void Start()
    {
        int _rand = Random.Range(0, 2);
        print(_rand);
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
        if(m_Agent.enabled)SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("LookEye");
        m_Animator = GetComponent<Animator>();
        m_pScript = m_Player.GetComponent<Player>();
        if (_rand == 0)
        {
            this.tag = "Custmoer";
            _Child = false;
        }
        else {
            this.tag = "Parent";
            _Child = true;
        }
        if (_Child)
        {
            m_Child = (GameObject)Resources.Load("Prefab/Giri");
            Instantiate(m_Child, new Vector3(transform.position.x,transform.position.y,transform.position.z-2), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        m_Agent.speed = 1.0f;
        //巡回中
        if (m_State == CustomerState.NormalMode)
        {
           // m_Agent.speed = 1.0f;
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            if (HasArrived())
            {
                SetNewPatrolPointToDestination();
            }
            
        }
        m_Animator.SetFloat("Speed", m_Agent.speed);
    }

    //次の巡回ポイントを目的地に設定する
    void SetNewPatrolPointToDestination()
    {
        m_rand = Random.Range(0, m_PatrolPoints.Length);
        m_Agent.destination = m_PatrolPoints[m_rand].transform.position;
    }

    // 目的地に到着したか
    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }
}
