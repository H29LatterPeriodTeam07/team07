using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public Vector2 m_InitPosition;

    private float t;
    private RectTransform rectTransform;
    private Vector2 m_ScorePosition;
    private Vector2 m_TargetPosition;
    private Vector2 m_MiddlePosition;
    private bool m_IsScattered;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        m_IsScattered = false;
        m_ScorePosition = new Vector2(-580.0f, 300.0f);
        t = 0.0f;
    }

    void Update()
    {
        if (!m_IsScattered && t < 1.0f)
        {
            rectTransform.localPosition = QuadriaticBezierCurves(m_InitPosition, m_MiddlePosition, m_TargetPosition, t);
            t += Time.deltaTime;
            if (t >= 1.0f)
            {
                m_IsScattered = true;
                CalcMiddlePosition();
                t = 0.0f;
            }
        }
        else
        {
            if (t >= 1.0f)
            {
                t = 1.0f;
                rectTransform.localPosition = QuadriaticBezierCurves(m_TargetPosition, m_MiddlePosition, m_ScorePosition, t);
                Destroy(gameObject);
            }
            else
            {
                rectTransform.localPosition = QuadriaticBezierCurves(m_TargetPosition, m_MiddlePosition, m_ScorePosition, t);
            }


            t += Time.deltaTime;
        }
    }

    public void SetInitPosition(Vector2 initPosition)
    {
        m_InitPosition = initPosition;
    }
    public void SetTarget(Vector2 targetPosition)
    {
        m_TargetPosition = targetPosition;
        CalcMiddlePosition();
    }

    public void CalcMiddlePosition()
    {
        Vector2 MiddlePosition;
        if (!m_IsScattered)
        {
            MiddlePosition = (m_TargetPosition - m_InitPosition) / 2.0f;
            float Length = MiddlePosition.magnitude;
            MiddlePosition += m_InitPosition;
            MiddlePosition += Vector2.up * Length * 2.0f;
        }
        else
        {
            // 画面左端を通る
            MiddlePosition = new Vector2(-640.0f , -360.0f);
        }
        m_MiddlePosition = MiddlePosition;
    }
    // 2次ベジェ曲線
    private Vector2 QuadriaticBezierCurves(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        Vector2 l_result;
        Vector2 l_0 = Vector2.Lerp(p0, p1, t);
        Vector2 l_1 = Vector2.Lerp(p1, p2, t);
        l_result = Vector2.Lerp(l_0, l_1, t);

        return l_result;
    }

}
