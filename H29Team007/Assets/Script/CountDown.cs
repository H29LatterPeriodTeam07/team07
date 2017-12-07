using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {


    [SerializeField, Header("数字たち(1～3)")]
    private Sprite[] numberImages;
    private float count = 3.0f;

    private Image countNumber;

    [SerializeField, Header("スタート文字")]
    private GameObject startUI;
    [SerializeField, Header("エンド文字")]
    private GameObject endUI;

    private Player playerScript;

    // Use this for initialization
    void Start () {
        countNumber = GetComponent<Image>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerScript.GetState() == Player.PlayerState.Entry) {
            countNumber.color = new Color(1, 1, 1, 0);
            return;
        }
        else
        {
            countNumber.color = new Color(1, 1, 1, 1);
        }
        if(count <= 0.0f)
        {
            countNumber.color = new Color(1, 1, 1,0);
            MainGameDate.ChangeStartFlag();
            if (MainGameDate.IsStart())
            {
                startUI.SetActive(true);
            }
            else
            {
                endUI.SetActive(true);
                playerScript.ChangeState(6);
            }
            //countNumber.sprite = numberImages[2];
            enabled = false;
        }
        else if(count <= 1.0f)
        {
            countNumber.sprite = numberImages[0];
        }
		else if(count <= 2.0f)
        {
            countNumber.sprite = numberImages[1];
        }
        count -= Time.deltaTime;
	}

    public void SetCount(float time)
    {
        count = time;
        countNumber.sprite = numberImages[2];
        countNumber.color = new Color(1, 1, 1, 1);
    }
}
