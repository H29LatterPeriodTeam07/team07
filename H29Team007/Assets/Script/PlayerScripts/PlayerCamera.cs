using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    enum CameraState
    {
        Normal,   //通常時
        Oidashi,  //追い出された時
        Enter,    //入るとき
        Exit      //出るとき
    }

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
    private float m_PitchAngle = 0f;
    
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

    // カメラリセットに要する時間(秒)
    private float m_ResetTime =1.0f;
    // カメラリセットを開始した時刻
    private float m_ResetStartTime = float.MinValue;

    private bool isCameraReset = false;

    private CameraState myState;

    private Transform exitCameraPoint;
    private Transform entryCameraPoint;
    private bool isEntry;

    //private Vector3 targetPos;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main.transform;
        m_Target = player.transform.Find("CameraLookPoint");
        playerSC = player.GetComponent<ShoppingCount>();
        cameraLeaveVec = CameraLeaveVec();
        myState = CameraState.Enter;
        exitCameraPoint = GameObject.Find("ExitCameraPoint").transform;
        entryCameraPoint = GameObject.Find("EntryCameraPoint").transform;
        isEntry = false;
        //targetPos = mainCamera.transform.position;
        //transform.position = targetPos;
        transform.position = mainCamera.transform.position; 
        mainCamera.position = transform.position + cameraLeaveVec * m_DefaultDistance;
    }

    void LateUpdate()
    {
        switch (myState)
        {
            case CameraState.Normal:NormalCamera();break;
            case CameraState.Oidashi:OidashiCamera();break;
            case CameraState.Enter: EntryCamera();break;
            case CameraState.Exit: ExitCamera(); break;
        }

        //// Rボタンで視点リセット（背後へ回り込む）
        //if (Input.GetKey(KeyCode.R))
        //{
        //    CameraReset();
        //}

    }

    public void ChangeState(int stateNum)
    {
        switch (stateNum)
        {
            case 0: myState = CameraState.Normal;break;
            case 1: myState = CameraState.Oidashi; break;
            case 2: myState = CameraState.Enter; break;
            case 3: myState = CameraState.Exit;break;
        }
    }

    private void NormalCamera()
    {

        // カメラリセット処理
        if (isCameraReset)
        {
            float distance = Vector3.Distance(m_Target.position, mainCamera.position);

            // 現在、プレイヤーから見てカメラがある方角
            Vector3 currentDirection = (transform.position - m_Target.position).normalized;
            // 希望する方向（プレイヤーの背後の方向）
            Vector3 desiredDirection = -m_Target.forward;
            // m_ResetTimeの時間かけて背後に行くようにLerpの強さを調整
            float strength = 4.5f / m_ResetTime;

            // Slerpを使って、目標の方向へ少しずつ向ける
            Vector3 lerpedDirection = Vector3.Slerp(currentDirection, cameraLeaveVec, strength * Time.deltaTime);

            // 場所を確定する
            transform.position = m_Target.position + lerpedDirection * m_DefaultDistance;
            if (Time.time > m_ResetStartTime + m_ResetTime) isCameraReset = false;
            // ターゲットの方を向く
            transform.LookAt(new Vector3(m_Target.position.x, m_Target.position.y + (cameraLeaveVec.y * CameraToPlayerDistance()), m_Target.position.z));
            transform.Rotate(new Vector3(0, 180, 0)); //もともと180度のy軸回転
        }
        else
        {
            if (transform.position == mainCamera.position)
            {
                transform.position = m_Target.position;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
                //transform.Rotate(new Vector3(0, -180, 0));
                cameraLeaveVec = CameraLeaveVec();
                mainCamera.position = transform.position + cameraLeaveVec * CameraToPlayerDistance();
                m_PitchAngle = 0f;

            }
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
            else if (Input.GetAxisRaw("XboxRightHorizontal") != 0 || Input.GetAxisRaw("XboxRightVertical") != 0)
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
    }

    private void Wait()
    {

    }

    private void OidashiCamera()
    {
        // Rボタンで視点リセット（背後へ回り込む）
        //if (Input.GetKey(KeyCode.R))
        //{
        //    CameraReset();
        //}
    }

    private void EntryCamera()
    {
        Vector3 targetPos = m_Target.position;

        // 追跡対象に位置を合わせる
        transform.position = targetPos;

        if (isEntry)
        {
            cameraLeaveVec = CameraLeaveVec();
            targetPos = transform.position + cameraLeaveVec * CameraToPlayerDistance();
            mainCamera.position = Vector3.Lerp(mainCamera.position, targetPos, m_Strength * Time.deltaTime);
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, m_Target.eulerAngles, m_Strength * Time.deltaTime);
            //transform.eulerAngles = m_Target.eulerAngles;
            //transform.Rotate(new Vector3(0, 180, 0)); //もともと180度のy軸回転

            if (Vector3.Distance(targetPos,mainCamera.position)< m_DefaultDistance + 1)
            {
                mainCamera.position = targetPos;
                CameraReset();
                ChangeState(0);
            }
        }
        else
        {
            mainCamera.position = entryCameraPoint.position;
            transform.eulerAngles = entryCameraPoint.eulerAngles;
            transform.Rotate(new Vector3(0, 180, 0)); //もともと180度のy軸回転
        }

        
    }

    private void ExitCamera()
    {
        Vector3 targetPos = m_Target.position;

        // 追跡対象に位置を合わせる
        transform.position = targetPos;
               
        mainCamera.position = exitCameraPoint.position;
        transform.eulerAngles = exitCameraPoint.eulerAngles;
        transform.Rotate(new Vector3(0, 180, 0)); //もともと180度のy軸回転
                
    }

    private Vector3 CameraLeaveVec()
    {
        return Vector3.Normalize(transform.forward * leaveX + transform.up * leaveY);
    }

    private float CameraToPlayerDistance()
    {
        return m_DefaultDistance + addDistance * playerSC.GetBaggageCount();
    }

    public void CameraReset()
    {
        isCameraReset = true;
        m_ResetStartTime = Time.time;
        transform.position = mainCamera.transform.position;
        mainCamera.localPosition = Vector3.zero;
        cameraLeaveVec = Vector3.Normalize(-m_Target.forward * leaveX + m_Target.up * leaveY);
        ChangeState(0);
    }

    public void Oidashi(Transform exitPoint)
    {
        transform.position = m_Target.position; 
        //Vector3 newPos = exitPoint.position  + Vector3.up * 10f;
        //mainCamera.position = exitPoint.position +exitPoint.forward * 1.0f + exitPoint.right * 0.5f + Vector3.up * 1.2f;//exitPoint.forward * 1.0f + exitPoint.right * 0.5f;
        //transform.LookAt(m_Target.up);
        transform.eulerAngles = new Vector3(-70, exitPoint.eulerAngles.y + 20, 0);
        //transform.Rotate(new Vector3(0, 180, 0)); //もともと180度のy軸回転
        //mainCamera.position = exitPoint.position + exitPoint.forward * 1.0f + exitPoint.right * 0.5f + Vector3.up * 1.2f;//exitPoint.forward * 1.0f + exitPoint.right * 0.5f;
        ChangeState(1);
    }

    public void Entry()
    {
        isEntry = true;
    }
}
