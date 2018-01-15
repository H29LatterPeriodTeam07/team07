using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receipt : MonoBehaviour {

    private RectTransform myTransform;
    private Vector2 defPos;
    private float moveYmin = 0.0f;
    private float moveYmax = 0.0f;
    private float movespeed = 5.0f;

    private bool automove = false;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<RectTransform>();
        defPos = myTransform.anchoredPosition;
        myTransform.anchoredPosition += new Vector2(-500, 0);
        moveYmin = defPos.y;

    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 newPos = myTransform.anchoredPosition;
        if (automove)
        {
            newPos.y -= movespeed;
            if (newPos.y < moveYmin)
            {
                newPos.y = moveYmin;
                automove = false;
            }
        }
        else
        { 
            //レシートの移動
            float input = (Input.GetAxisRaw("XboxLeftVertical") != 0) ? Input.GetAxisRaw("XboxLeftVertical") : Input.GetAxisRaw("Vertical");
            newPos.y += input * movespeed;
            newPos.y = Mathf.Clamp(newPos.y, moveYmin, moveYmax);
        }
        myTransform.anchoredPosition = newPos;
    }

    public void SetParameter(float height)
    {
        float newY = height - 700.0f; //画面サイズが720から、上下の隙間10ずつ引いたやつ
        if(newY <= 0)
        {
            moveYmax = defPos.y;
        }
        else
        {
            moveYmax = newY - defPos.y;
        }
        myTransform.anchoredPosition = new Vector2(defPos.x, defPos.y + height);
        automove = true;
    }
}
