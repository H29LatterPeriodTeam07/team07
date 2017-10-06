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
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;
    //特売品ゲットだぜ
    public bool m_GetSaleAnimal = false;

    private BBAState m_State = BBAState.NormalMode;
    private float m_Speed = 1.0f;
    private bool m_sale=false;

    NavMeshAgent m_Agent;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
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
        m_EyePoint = transform.Find("LookEye");
        //タグで特売品オブジェぅとを検索して保持
        m_SaleAnimals = GameObject.FindGameObjectWithTag("SaleAnimal");
        //特売品の注視点を名前で検索して保持
        m_SaleAnimalsLookPoint = transform.Find("AnimalLookEye");
    }

    // Update is called once per frame
    void Update()
    {
        //特売品出現時間ON・OFF
        if (m_sale==false && Input.GetKeyDown(KeyCode.K))
        {
            m_sale = true;
        }
        if (m_sale == true && Input.GetKeyDown(KeyCode.K))
        {
            m_sale = false;
        }
        //巡回中
        if (m_State == BBAState.NormalMode)
        {
            //特売品が出てくる時間になったら特売品モードに
            if (m_sale)
            {
                //退避に状態変更
                m_State = BBAState.SaleMode;
                m_Agent.destination = m_Player.transform.position;
            }
            else if(HasArrived())
            {
                SetNewPatrolPointToDestination();
            }
        }
        //特売品モード
        else if (m_State == BBAState.SaleMode)
        {
            if (m_GetSaleAnimal==false)
            {
                //特売品が見えた場合
                if (CanSeeSaleAnimal())
                {
                    m_ViewingDistance = 50;
                    m_ViewingAngle = 45;
                    m_Agent.destination = m_SaleAnimals.transform.position;
                }
            }
            else
            {

            }
        }
        //攻撃モード
        else if (m_State == BBAState.attackMode)
        {
            //特売品が見えている場合
            if (CanSeeSaleAnimal())
            {

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

    //特売品が見える距離内にいるか？
    bool IsSaleAnimalInViewingDistance()
    {
        //自身から特売品までの距離
        float distanceToAnimal = Vector3.Distance(m_SaleAnimalsLookPoint.position, m_EyePoint.position);
        //特売品が見える距離内にいるかどうかを返却する
        return (distanceToAnimal <= m_ViewingDistance);
    }

    //特売品が見える視野角内にいるか？
    bool IsSaleAnimalInViewingAngle()
    {
        //自身から特売品への方向ベクトル(ワールド座標系)
        Vector3 directionToSaleAnimal = m_SaleAnimalsLookPoint.position - m_EyePoint.position;
        //自身のそうめん向きベクトルと特売品への方向ベクトルの差分角度
        float angleToSaleAnimal = Vector3.Angle(m_EyePoint.forward, directionToSaleAnimal);
        //見える視野角内の範囲内に特売品がいるかどうかを返却する
        return (Mathf.Abs(angleToSaleAnimal) <= m_ViewingAngle);
    }

    //特売品にRayを飛ばしたら当たるか？
    bool CanHitToSaleAnimal()
    {
        //自身から特売品への方向ベクトル(ワールド座標系)
        Vector3 directionToSaleAnimal = m_SaleAnimalsLookPoint.position - m_EyePoint.position;
        //壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(m_EyePoint.position, directionToSaleAnimal, out hitInfo);
        //特売品にRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "SaleAnimal");
    }

    //特売品が見えるか？
    bool CanSeeSaleAnimal()
    {
        //見える距離の範囲内に特売品がいない場合→見えない
        if (!IsSaleAnimalInViewingDistance())
            return false;
        //見える視野角の範囲内に特売品がいない場合→見えない
        if (!IsSaleAnimalInViewingAngle())
            return false;
        //Rayを飛ばして、それが特売品の当たらない場合→見えない
        if (!CanSeeSaleAnimal())
            return false;
        //ここまで到達したら、それは特売品が見える
        return true;
    }
}
