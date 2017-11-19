using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    //[SerializeField, Header("追跡対象")]
    private Transform m_Target;
    [SerializeField, Header("ヨー速度(角度/秒)")]
    private float m_YawSpeed = 180f;
    [SerializeField, Header("ピッチ速度(角度/秒)")]
    private float m_PitchSpeed = 90f;
    [SerializeField, Header("最大仰角")]
    private float m_MaxPitch = 30;
    [SerializeField, Header("最小仰角")]
    private float m_MinPitch = -50f;

    // 現在の仰角
    float m_PitchAngle = 0f;
    
    private Transform mainCamera;

    private ShoppingCount playerSC;

    //カメラを離す角度、ベクトル
    private Vector3 cameraLeaveVec;
    [Header("どのくらい離れるかの横と縦の比率、直角三角形の直角を作っている辺の比率")]
    [SerializeField, Header("横")]
    private float leaveX = 3;
    [SerializeField, Header("縦")]
    private float leaveY = 1;


    [SerializeField, Header("追跡対象とカメラのデフォルトの距離")]
    private float m_DefaultDistance = 4f;
    [SerializeField, Header("荷物が増えるたびに離れる距離、加算")]
    private float addDistance = 0;

    [SerializeField, Header("カメラの移動・値が大きいほどキビキビ、小さいほどネットリ動く")]
    public float m_Strength = 5f;

    private Vector3 targetPos;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.transform;
        m_Target = player.transform.Find("CameraLookPoint");
        playerSC = player.GetComponent<ShoppingCount>();
        cameraLeaveVec = CameraLeaveVec();
        targetPos = mainCamera.transform.position;
        transform.position = targetPos;
        mainCamera.position = transform.position + cameraLeaveVec * m_DefaultDistance;
    }

    void LateUpdate()
    {
        Vector3 targetPos = m_Target.position;
        //targetPos.y += playerSC.GetCameraLookPoiintPluseY();
        cameraLeaveVec = CameraLeaveVec();

        // 追跡対象に位置を合わせる
        transform.position = targetPos;
        //float dis = Vector3.Distance(targetPos, mainCamera.position);

        Ray ray = new Ray(targetPos, cameraLeaveVec);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, CameraToPlayerDistance()))
        {
            targetPos = hitInfo.point;
        }
        else
        {
            targetPos = transform.position + cameraLeaveVec * CameraToPlayerDistance();
        }

        // 現在の場所から目標の場所まで少しずつ近づく
        mainCamera.position = Vector3.Lerp(mainCamera.position, targetPos, m_Strength * Time.deltaTime);

        float inputHorizontal = (Input.GetAxisRaw("XboxRightHorizontal") != 0) ? Input.GetAxisRaw("XboxRightHorizontal") : Input.GetAxisRaw("Mouse X");
        float inputVertical = (Input.GetAxisRaw("XboxRightVertical") != 0) ? Input.GetAxisRaw("XboxRightVertical") : Input.GetAxisRaw("Mouse Y");
        
        if (Input.GetKey("mouse 0"))//作業の邪魔だからクリックしてる間にしてる、いらないif
        {
            // 横回転（ヨー）
            transform.Rotate(Vector3.up, inputHorizontal * m_YawSpeed * Time.deltaTime, Space.World);

            // 縦回転（ピッチ）
            m_PitchAngle += inputVertical * m_PitchSpeed * Time.deltaTime;
            // 角度制限
            m_PitchAngle = Mathf.Clamp(m_PitchAngle, m_MinPitch, m_MaxPitch);
            // 現在の角度をVector3で取得する
            Vector3 rotation = transform.rotation.eulerAngles;
            // 変更した値を仰角に設定する
            rotation.x = m_PitchAngle;
            // Quaternionに変換してtransform.rotationに設定し直す
            transform.rotation = Quaternion.Euler(rotation);
        }
        else if(Input.GetAxisRaw("XboxRightHorizontal") != 0 || Input.GetAxisRaw("XboxRightVertical") != 0)
        {
            // 横回転（ヨー）
            transform.Rotate(Vector3.up, inputHorizontal * m_YawSpeed * Time.deltaTime, Space.World);

            // 縦回転（ピッチ）
            m_PitchAngle += inputVertical * m_PitchSpeed * Time.deltaTime;
            // 角度制限
            m_PitchAngle = Mathf.Clamp(m_PitchAngle, m_MinPitch, m_MaxPitch);
            // 現在の角度をVector3で取得する
            Vector3 rotation = transform.rotation.eulerAngles;
            // 変更した値を仰角に設定する
            rotation.x = m_PitchAngle;
            // Quaternionに変換してtransform.rotationに設定し直す
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    private Vector3 CameraLeaveVec()
    {
        return Vector3.Normalize(transform.forward * leaveX + transform.up * leaveY);
    }

    private float CameraToPlayerDistance()
    {
        return m_DefaultDistance + addDistance * playerSC.GetBaggageCount();
    }
}
