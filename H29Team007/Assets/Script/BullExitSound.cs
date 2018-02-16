using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullExitSound : MonoBehaviour {
    [SerializeField]
    AudioClip m_Sound;
    [SerializeField]
    AudioClip m_giriscream;
    [SerializeField]
    AudioClip m_patroalcarSiren;
    [SerializeField]
    AudioClip m_ambulanceSiren;
    AudioSource audioSource;

    GameObject m_exit;
    Exit exit_;

    // Use this for initialization
    void Start () {
        m_exit = GameObject.FindGameObjectWithTag("Exit");
        exit_ = m_exit.GetComponent<Exit>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(m_Sound);
        audioSource.PlayOneShot(m_giriscream);
      //  audioSource.PlayOneShot(m_patroalcarSiren);
        audioSource.PlayOneShot(m_ambulanceSiren);
    }
	
	// Update is called once per frame
	void Update () {
      //  iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(exit_.m_soundEnd.transform.position.x, 0f, exit_.m_soundEnd.transform.position.z), "time", 10));

        if (transform.position == exit_.m_soundEnd.transform.position)
        {
            Destroy(this);
        }
    }
}
