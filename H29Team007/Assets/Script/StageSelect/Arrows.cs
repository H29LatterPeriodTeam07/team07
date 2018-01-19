using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrows : MonoBehaviour {
    GameObject m_Flyers;
    GameObject m_Direction;
    float m_theta;

	// Use this for initialization
	void Start () {
        m_Flyers = transform.parent.gameObject;
        m_theta = 0.0f;
        m_Direction = null;
    }
	
	// Update is called once per frame
	void Update () {
        float Speed = 5.0f;
        if (!m_Flyers.GetComponent<Flyers>().IsReachTargetPositionX())
        {
            m_theta = 90.0f;
        }
        else
        {
            m_theta += Time.deltaTime * Speed;
        }
        if(m_theta >= 180.0f)
        {
            m_theta = 0.0f;
        }
        Color color;
        for (int i = 0; i< transform.childCount; ++i)
        {
            color = transform.GetChild(i).GetComponent<Image>().color;
            transform.GetChild(i).GetComponent<Image>().color = new Color(color.r, color.g, color.b, Mathf.Sin(m_theta));
        }
    }

    public void SetTargetLocalPositionX(float positionX)
    {
        bool l_IsRight = positionX - GetComponent<RectTransform>().localPosition.x < 0.0f;
        string name;
        if (l_IsRight)
        {
            name = "Left";
        }
        else
        {
            name = "Right";
        }
        
        m_Direction = Instantiate(transform.Find(name).gameObject);
        m_Direction.transform.parent = transform.parent;
        m_Direction.GetComponent<RectTransform>().localScale = Vector3.one * 2.0f;
        m_Direction.GetComponent<RectTransform>().position = transform.Find(name).gameObject.transform.position;
        m_Direction.GetComponent<Image>().color = Color.yellow;
        Destroy(m_Direction, 1.0f);
        GetComponent<RectTransform>().localPosition = Vector3.right * positionX;
    }

}
