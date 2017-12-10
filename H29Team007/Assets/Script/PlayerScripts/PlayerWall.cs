using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour {

    private Player player;
    private Collider myCollider;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        myCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player.GetState() > Player.PlayerState.Takeover)
        {
            myCollider.enabled = false;
        }
        else
        {
            myCollider.enabled = true;
        }
	}
}
