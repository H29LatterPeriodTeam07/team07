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
    //親を追いかけるモード(見失い中)
    ChasingWarningMode,
    //泣きわめくモード
    CryMode
}

public class Child : MonoBehaviour {
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public bool m_GaurdCoal = false;

    private GameObject m_ExitPoition;
    private ChildState m_State = ChildState.NormalMode;
   // private GameObject m_Parent;
    private Transform m_ParentEyePoint;
    private Transform m_LookEye;
    private Vector3 pos;
    private Animator m_Animator;
    NavMeshAgent m_Agent;
    Collider m_collider;

    private GameObject m_Player;
    private Transform m_PlayerEyePoint;
    private ShoppingCount scScript;

    private bool isParentinBaggege = false;

    float radius = 5f;
    private LayerMask raycastLayer;
    Transform m_Parent;


    // Use this for initialization
    void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
        m_LookEye = transform.Find("LookEye");
        m_Animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider>();
        m_collider.enabled = false;
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerEyePoint = m_Player.transform.Find("LookPoint");
        scScript = m_Player.GetComponent<ShoppingCount>();
        m_ExitPoition = GameObject.FindGameObjectWithTag("ExitPoint");
        raycastLayer = 1 << LayerMask.NameToLayer("Parent");

        if (m_Parent == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, raycastLayer);
            if (hitColliders.Length > 0)
            {
                int randomInt = Random.Range(0, hitColliders.Length);
                m_Parent = hitColliders[randomInt].transform;
            }
        }
        m_ParentEyePoint = m_Parent.transform.Find("ParentEye");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Parent == null)
        {
            m_Agent.speed = 3;
            m_Agent.destination = m_ExitPoition.transform.position;
            m_collider.enabled = true;
            if (HasArrived())
            {
                m_Agent.speed = 0;
                Destroy(gameObject);
            }
        }
        else {
            if (isParentinBaggege)
            {
                if (m_Parent.transform.parent == null)
                {
                    scScript.MinusChild();
                    isParentinBaggege = false;
                }
            }
            else
            {
                if (m_Parent.transform.root.tag == "Player")
                {
                    
                    scScript.PlusChild();
                    isParentinBaggege = true;
                }
            }

            Vector3 PPos = m_ParentEyePoint.position;
            PPos.y = 0;
            Vector3 CPos = m_LookEye.position;
            CPos.y = 0;
            float dis = Vector3.Distance(PPos, CPos);
            // print(dis);
            m_Animator.SetFloat("Speed", m_Agent.speed);
            if (m_State == ChildState.NormalMode)
            {
                m_Agent.speed = 1f;
                m_Agent.destination = PPos;
                if (dis > 5.0f)
                {
                    m_ViewingDistance = 100;
                    m_ViewingAngle = 200;
                    if (CanSeeParent2())
                    {
                 
                        m_Agent.speed = 2.0f;
                        m_Agent.destination = PPos;
                    }
                    else
                    {
                        m_State = ChildState.ChasingWarningMode;
                    }
                }
            }

            else if (m_State == ChildState.ChasingWarningMode)
            {
                m_ViewingDistance = 300;
                m_ViewingAngle = 280;
                if (CanSeeParent2())
                {
                 
                    m_Agent.speed = 2.0f;
                    m_Agent.destination = PPos;
                }
                else if (HasArrived())
                {
                    if (CanSeeParent2())
                    {
                        m_State = ChildState.NormalMode;
                    }
                    else {
                       // m_collider.enabled = true;
                        m_State = ChildState.CryMode;
                        m_Animator.SetTrigger("Cry");
                    }
                }
            }
            else if (m_State == ChildState.CryMode)
            {
                m_Agent.speed = 0;
                print("おーいおいおいおいおい、おいおい");
                m_GaurdCoal = true;
                if (CanSeeParent2())
                {
                    m_collider.enabled = false;
                    m_State = ChildState.NormalMode;
                    m_Animator.SetTrigger("Trigger");
                }
            }
        }
    }

    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }

    public void DoPatrol()
    {
        if (m_Agent.enabled == false) return;
        var x = Random.Range(-15.0f, 15.0f);
        var z = Random.Range(-15.0f, 15.0f);
        pos = new Vector3(x, 0, z);
        m_Agent.SetDestination(pos);
    }

    bool IsParentInViewingDistance(Transform target)
    {
        //自身から親までの距離
        float distanceToPlayer = Vector3.Distance(target.position, m_LookEye.position);
        //親が見える距離内にいるかどうかを返却する
        return (distanceToPlayer <= m_ViewingDistance);
    }

    //親が見える視野角内にいるか？
    bool IsParentInViewingAngle(Transform target)
    {
        //自分から親への方向ベクトル(ワールド座標系)
        Vector3 directionToPlayer = target.position - m_LookEye.position;
        // 自分の正面向きベクトルと親への方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_LookEye.forward, directionToPlayer);

        // 見える視野角の範囲内に親がいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    // 親にRayを飛ばしたら当たるか？
    bool CanHitRayToParent(Transform target)
    {
        // 自分から親への方向ベクトル（ワールド座標系）
        Vector3 directionToPlayer = target.position - m_LookEye.position;
        // 壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_LookEye.position, directionToPlayer, out hitInfo);
        // 親にRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "Parent"|| hit && hitInfo.collider.tag == "Player");
    }

    // 親が見えるか？
    bool CanSeeParent2()
    {
        Transform target = (isParentinBaggege) ? m_Player.transform : m_ParentEyePoint;
      //  Debug.Log(target);
        // 見える距離の範囲内に親がいない場合→見えない
        if (!IsParentInViewingDistance(target))
            return false;
        // 見える視野角の範囲内に親がいない場合→見えない
        if (!IsParentInViewingAngle(target))
            return false;
        // Rayを飛ばして、それが親に当たらない場合→見えない
        if (!CanHitRayToParent(target))
            return false;
        // ここまで到達したら、それは親が見えるということ
        return true;
    }

    public bool Roaring()
    {
        return m_State == ChildState.CryMode;
    }
}
