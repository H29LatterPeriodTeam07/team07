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
    CashMode
}

public class BBA : MonoBehaviour {

    //巡回ポイント
    public Transform[] m_PatrolPoints;
    //巡回ポイント2
    public Transform[] m_Patrolpoints2;
    //レジのポイント
    public Transform m_ReziPoint;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    public bool m_sale = false;
    //特売品ゲットだぜ
    public bool m_GetSaleAnimal = false;

    [SerializeField, Header("特売品の出現場所を入れるところ")]
    private GameObject[] m_SaleAnimalSpowns;
    private BBAState m_State = BBAState.NormalMode;
    private float m_Speed = 1.0f;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    int m_CurrentPatrolPoint2Index = 1;

    NavMeshAgent m_Agent;
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
    SaleSpown m_scSaleSpown;


    // Use this for initialization
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        SetNewPatrolPointToDestination();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("BBAEye");
        //スクリプトSaleSpownへの参照
        for (int i = 0; i < m_SaleAnimalSpowns.Length; i++) {
            m_scSaleSpown = m_SaleAnimalSpowns[i].GetComponent<SaleSpown>();
        }
        //タグで特売品オブジェぅとを検索して保持
         m_SaleAnimals = GameObject.FindGameObjectWithTag("Animal");
        //特売品の注視点を名前で検索して保持
         m_SaleAnimalsLookPoint = m_SaleAnimals.transform.Find("AnimalLookEye");
    }

    // Update is called once per frame
    void Update()
    {
        //巡回中
        if (m_State == BBAState.NormalMode)
        {
            m_ViewingDistance = 100;
            m_ViewingAngle = 45;
            //特売品が出てくる時間になったら特売品モードに
            if (m_scSaleSpown.SaleMode())
            {
                //特売品モードに状態変更
                m_State = BBAState.SaleMode;
             //   m_Agent.destination = m_Player.transform.position;
            }
            else
            {
                if (HasArrived())
                {
                    SetNewPatrolPointToDestination();
                }
            }
        }
        //特売品モード
        else if (m_State == BBAState.SaleMode)
        {
            m_ViewingDistance = 100;
            m_ViewingAngle = 180;
                //特売品が見えた場合
                if (CanSeePlayer())
                {
                    m_Agent.speed = 3;
                    m_Agent.destination = m_SaleAnimals.transform.position;
                }
                else if (HasArrived())
                {
                    SetNewSalePatrolPointToDestination();
                }
                if (m_sale == false)
                {
                    m_State = BBAState.NormalMode;
                }
            
            if(m_GetSaleAnimal)
            {
              m_Agent.destination = m_ReziPoint.transform.position;
            }
        }
        //攻撃モード
        else if (m_State == BBAState.attackMode)
        {
        }
    }

    //次の巡回ポイントを目的地に設定する
    void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex
            = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

        m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;
    }

    void SetNewSalePatrolPointToDestination()
    {
        m_CurrentPatrolPoint2Index
            = (m_CurrentPatrolPoint2Index + 1) % m_Patrolpoints2.Length;

        m_Agent.destination = m_Patrolpoints2[m_CurrentPatrolPoint2Index].position;
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
        float distanceToPlayer = Vector3.Distance(m_SaleAnimalsLookPoint.position, m_EyePoint.position);
        //プレイヤーが見える距離内にいるかどうかを返却する
        return (distanceToPlayer <= m_ViewingDistance);
    }

    //プレイヤーが見える視野角内にいるか？
    bool IsPlayerInViewingAngle()
    {
        //自分からプレイヤーへの方向ベクトル(ワールド座標系)
        Vector3 directionToPlayer = m_SaleAnimalsLookPoint.position - m_EyePoint.position;
        // 自分の正面向きベクトルとプレイヤーへの方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        // 見える視野角の範囲内にプレイヤーがいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    // プレイヤーにRayを飛ばしたら当たるか？
    bool CanHitRayToPlayer()
    {
        // 自分からプレイヤーへの方向ベクトル（ワールド座標系）
        Vector3 directionToPlayer = m_SaleAnimalsLookPoint.position - m_EyePoint.position;
        // 壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);
        // プレイヤーにRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "Animal");
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
