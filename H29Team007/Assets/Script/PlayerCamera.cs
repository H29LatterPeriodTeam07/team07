using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    // 追跡対象
    public Transform m_Target;
    // ヨー速度(角度/秒)
    public float m_YawSpeed = 180f;
    // ピッチ速度(角度/秒)
    public float m_PitchSpeed = 90f;
    // 最大仰角
    public float m_MaxPitch = 30;
    // 最小仰角
    public float m_MinPitch = -50f;

    // 現在の仰角
    float m_PitchAngle = 0f;
    
    private Transform mainCamera;


    // 追跡対象とカメラのデフォルトの距離
    public float m_DefaultDistance = 4f;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        // 追跡対象に位置を合わせる
        transform.position = m_Target.position;
        float dis = Vector3.Distance(m_Target.position, mainCamera.position);

        Ray ray = new Ray(m_Target.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, m_DefaultDistance))
        {
            mainCamera.position = hitInfo.point;
        }
        else
        {
            mainCamera.position = transform.position + transform.forward * m_DefaultDistance;
        }


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
}
