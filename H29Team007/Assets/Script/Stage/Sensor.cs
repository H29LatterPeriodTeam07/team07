using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {
    bool m_IsStayObject;
    // Use this for initialization
    void Start () {
        m_IsStayObject = false;
    }
	
	// Update is called once per frame
	void Update () {
        //m_IsStayObject = false;
    }
    public bool IsStay()
    {
        return m_IsStayObject;
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Road" || other.name == "Plane") return;
        m_IsStayObject = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Road" || other.name == "Plane") return;
        m_IsStayObject = false;
        
    }
}
