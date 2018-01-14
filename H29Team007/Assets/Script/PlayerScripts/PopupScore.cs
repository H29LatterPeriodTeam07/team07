using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupScore : MonoBehaviour
{

    private Text myText;
    private RectTransform myTransform;
    private float a = 1;
    public Color orengeColor;

    public void Awake()
    {
        myText = GetComponent<Text>();
        myTransform = GetComponent<RectTransform>();
    }


    // Use this for initialization
    void Start()
    {
        myText = GetComponent<Text>();
        myTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, a);
        myTransform.position += Vector3.up * Time.deltaTime;
        a -= Time.deltaTime;
        if (a < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        myText.text = text;
    }

    public void SetOutColorOrange()
    {
        Outline outcolor = GetComponent<Outline>();
        outcolor.effectColor = orengeColor;
    }

    public void SetPositionAndRotation(Vector3 pos, float angle)
    {
        myTransform.position = pos;
        myTransform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
