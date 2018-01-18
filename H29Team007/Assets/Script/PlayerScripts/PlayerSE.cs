using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : MonoBehaviour {

    public AudioClip[] playerSEs;

    private AudioSource[] audioSource;
    
    private int nowNumber = 6;

	// Use this for initialization
	void Start () {
        audioSource = GetComponents<AudioSource>();
        audioSource[0].loop = true;
        
    }
	
	// Update is called once per frame
	void Update () {

	}

    /// <summary>一回だけ再生</summary>
    /// <param name="number">再生数SE番号</param>
    public void OnePlay(int number)
    {
        audioSource[1].PlayOneShot(playerSEs[number]);
    }

    public void OnePlay2(int number)
    {
        audioSource[2].PlayOneShot(playerSEs[number]);
    }

    public void SEPlay(int number)
    {
        if (nowNumber == number) return;
        audioSource[0].clip = playerSEs[number];
        audioSource[0].Play();
        nowNumber = number;
    }

    public void SEStop()
    {
        audioSource[0].Stop();
        nowNumber = 5;
    }
}
