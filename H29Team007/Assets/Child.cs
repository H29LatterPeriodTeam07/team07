using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ParentState
{
    //ノーマルモード
    NormalMode,
    //警戒モード
    WarningMode
}

public class Child : MonoBehaviour {
    //見える距離
    public float m_ViewingDistance;
    //視野角
    public float m_ViewingAngle;

    private ParentState m_State = ParentState.NormalMode;
    private GameObject m_Parent;
    private Transform m_ParentEyePoint;
    private Transform m_LookEye;
    

	// Use this for initialization
	void Start () {
        m_Parent = GameObject.FindGameObjectWithTag("Parent");
        m_ParentEyePoint = m_Parent.transform.Find("ParentEye");
        m_LookEye = transform.Find("LookEye");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

     //親が見える距離内にいるか？
    bool IsParentInViewingDistance()
    {
        //自身から親までの距離
        float distanceToPlayer = Vector3.Distance(m_ParentEyePoint.position, m_LookEye.position);
        //親が見える距離内にいるかどうかを返却する
        return (distanceToPlayer <= m_ViewingDistance);
    }

    //親が見える視野角内にいるか？
    bool IsParentInViewingAngle()
    {
        //自分から親への方向ベクトル(ワールド座標系)
        Vector3 directionToPlayer = m_ParentEyePoint.position - m_LookEye.position;
        // 自分の正面向きベクトルと親への方向ベクトルの差分角度
        float angleToPlayer = Vector3.Angle(m_LookEye.forward, directionToPlayer);

        // 見える視野角の範囲内に親がいるかどうかを返却する
        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }

    // 親にRayを飛ばしたら当たるか？
    bool CanHitRayToParent()
    {
        // 自分から親への方向ベクトル（ワールド座標系）
        Vector3 directionToPlayer = m_ParentEyePoint.position - m_LookEye.position;
        // 壁の向こう側などにいる場合は見えない
        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_ParentEyePoint.position, directionToPlayer, out hitInfo);
        // 親にRayが当たったかどうかを返却する
        return (hit && hitInfo.collider.tag == "Player");
    }

    // 親が見えるか？
    bool CanSeePlayer()
    {
        // 見える距離の範囲内に親がいない場合→見えない
        if (!IsParentInViewingDistance())
            return false;
        // 見える視野角の範囲内に親がいない場合→見えない
        if (!IsParentInViewingAngle())
            return false;
        // Rayを飛ばして、それが親に当たらない場合→見えない
        if (!CanHitRayToParent())
            return false;
        // ここまで到達したら、それは親が見えるということ
        return true;
    }
}
