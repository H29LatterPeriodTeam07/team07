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

    void Update()
    {
        // 追跡対象に位置を合わせる
        transform.position = m_Target.position;

        if (Input.GetKey("mouse 0"))
        {
            // 横回転（ヨー）
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * m_YawSpeed * Time.deltaTime, Space.World);

            // 縦回転（ピッチ）
            m_PitchAngle += Input.GetAxis("Mouse Y") * m_PitchSpeed * Time.deltaTime;
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
