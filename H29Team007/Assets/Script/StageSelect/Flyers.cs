using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyers : MonoBehaviour {
    bool m_IsSetTarget;
    float m_Value;
    float m_TargetPositionX;
	// Use this for initialization
	void Start () {
        m_IsSetTarget = false;
        m_Value = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_IsSetTarget) return;
        GetComponent<RectTransform>().localPosition = Vector3.right * Mathf.Lerp(GetComponent<RectTransform>().localPosition.x, m_TargetPositionX, m_Value);
        if(m_Value >= 1.0f)
        {
            m_IsSetTarget = false;
            m_Value = 1.0f;
            return;
        }
        m_Value += Time.deltaTime;
	}

    public bool IsSetTarget()
    {
        return m_IsSetTarget;
    }
    public bool IsReachTargetPositionX()
    {
        return m_Value >= 1.0f;
    }

    public void MoveTargetPositionX(float targetPositionX)
    {
        m_TargetPositionX = targetPositionX;
        m_IsSetTarget = true;
        m_Value = 0.0f;
    }
}
