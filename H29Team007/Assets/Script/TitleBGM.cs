using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBGM : MonoBehaviour {

    public GameObject m_sm;
    private SoundManagerScript m_scScript;

    // Use this for initialization
    void Start () {
        m_scScript = m_sm.transform.GetComponent<SoundManagerScript>();

        m_scScript.PlayBGM(0);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
