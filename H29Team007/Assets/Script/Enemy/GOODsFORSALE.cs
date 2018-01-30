using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SaleAnimalState
{
    //ノーマルモード
    NormalMode,
    //警戒モード
    WarningMode
}

public class GOODsFORSALE : MonoBehaviour
{

    //巡回ポイント
    // public Transform[] m_PatrolPoints;
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;

    private SaleAnimalState m_State = SaleAnimalState.NormalMode;
    private float m_Speed = 1.0f;
    private Vector3 pos;
    NavMeshAgent m_Agent;
    //現在の巡回ポイントのインデックス
    int m_CurrentPatrolPointIndex = 1;
    //プレイヤーへの参照
    GameObject m_Player;
    //ババアへの参照
    GameObject m_BBA;
    //プレイヤーへの注視点
    Transform m_PlayerLookpoint;
    //ババアへの注視点
    Transform m_BBALookpoint;
    //自身の目の位置
    Transform m_EyePoint;
    //巡回ポイントの親
    GameObject m_PatrolPoint;
    GameObject[] m_PatrolPoints;
    GameObject m_ParentBBA;
    GameObject m_exitPont;
    Exit m_eScript;

    // Use this for initialization
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        //目的地を設定する
        // SetNewPatrolPointToDestination();
         DoPatrol();
        //タグでプレイヤーオブジェクトを検索して保持
        m_Player = GameObject.FindGameObjectWithTag("Player");
        //タグでババアオブジェクトを検索して保持
        m_BBA = GameObject.FindGameObjectWithTag("BBA");
        //プレイヤーの注視点を名前で検索して保持
        m_PlayerLookpoint = m_Player.transform.Find("LookPoint");
        //BBAの注視点を名前で検索して保持
        if (m_BBA != null) m_BBALookpoint = m_BBA.transform.Find("BBAEye");
        m_EyePoint = transform.Find("AnimalLookEye");


    }

    // Update is called once per frame
    void Update()
    {
        if (m_State == SaleAnimalState.NormalMode)
        {
            if (HasArrived())
            {
                DoPatrol();
            }
            if (CanSeePlayer() || CanSeeBBA())
            {
                //退避に状態変更
                m_State = SaleAnimalState.WarningMode;
            }
            if (m_eScript.BullApper())
            {
                BullPatrol();
            }

        }
        else if (m_State == SaleAnimalState.WarningMode)
        {
            m_Agent.speed = 3.0f;
            // プレイヤーが見えている場合
            if (CanSeePlayer()|| CanSeeBBA())
            {
                if (!m_Agent.enabled) return;
                m_ViewingDistance = 10;
                m_ViewingAngle = 360;
                m_Agent.speed = 3;
                m_Agent.destination = -m_Player.transform.position;
                Vector3 dir = this.transform.position - m_Player.transform.position;
                Vector3 pos = this.transform.position + dir * 0.5f;
                m_Agent.destination = pos;

            }
            // 見失った場合
            else
            {
                // 追跡中（見失い中）に状態変更
                m_State = SaleAnimalState.NormalMode;
            }
        }
    }

    //エージェントが向かう先をランダムに指定するメソッド
    public void DoPatrol()
    {
        if (m_Agent.enabled == false) return;
        var x = Random.Range(-100.0f, 100.0f);
        var z = Random.Range(-100.0f, 100.0f);
        pos = new Vector3(x, 0, z);
        m_Agent.SetDestination(pos);
    }

    public void BullPatrol()
    {
        if (m_Agent.enabled == false) return;
        var x = Random.Range(-100.0f, 100.0f);
        var z = Random.Range(-100.0f, 100.0f);
        pos = new Vector3(0+x, 0, 0+z);
        m_Agent.SetDestination(pos);
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

    //ババアが見える距離内にいるか？
    bool IsBBAInViewingDistance()
    {
        //自身からプレイヤーまでの距離
        float distanceToPlayer = Vector3.Distance(m_BBALookpoint.position, m_EyePoint.position);
        //プレイヤーが見える距離内にいるかどうかを返却する
        return (distanceToPlayer <= m_ViewingDistance);
    }

    //ババアが見える視野角内にいるか？
    bool IsBBAInViewingAngle()
    {
        //自分からババアへの方向ベクトル(ワールド座標系)
        Vector3 directionToPlayer = m_BBALookpoint.position - m_EyePoint.position;
        // 自分の正面向きベクトルとババアへの方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        // 見える視野角の範囲内にババアがいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    // ババアにRayを飛ばしたら当たるか？
    bool CanHitRayToBBA()
    {
        // 自分からババアへの方向ベクトル（ワールド座標系）
        Vector3 directionToPlayer = m_BBALookpoint.position - m_EyePoint.position;
        // 壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);
        // ババアにRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "BBA");
    }

    // プレイヤーが見えるか？
    bool CanSeeBBA()
    {
        if (m_BBA == null) return false;
        // 見える距離の範囲内にプレイヤーがいない場合→見えない
        if (!IsBBAInViewingDistance())
            return false;
        // 見える視野角の範囲内にプレイヤーがいない場合→見えない
        if (!IsBBAInViewingAngle())
            return false;
        // Rayを飛ばして、それがプレイヤーに当たらない場合→見えない
        if (!CanHitRayToBBA())
            return false;
        // ここまで到達したら、それはプレイヤーが見えるということ
        return true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Road")
        {
            Destroy(gameObject);
        }
    }
}