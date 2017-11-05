using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BBACartCount : MonoBehaviour {

    GameObject m_AnimalTransform;
    int m_AnimalCount = 0;

    // Use this for initialization 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsAnimal()
    {
        if(m_AnimalTransform.tag == "Animal")
        {
            m_AnimalCount=1;
        }
        else
        {
            m_AnimalCount = 0;
        }

        return (m_AnimalCount > 0);
    }
}
