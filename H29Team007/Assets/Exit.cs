using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject m_PreBBA;
    public Transform m_EntrancePoint;
    private GameObject m_GameBBA;
    private float m_time=0.0f;
    float x, z;

    // Use this for initialization
    void Start()
    {
        m_GameBBA = GameObject.FindGameObjectWithTag("BBA");
      //  Instantiate(m_PreBBA, new Vector3(transform.position.x + 3, 0, transform.position.z + 3), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
