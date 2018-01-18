using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTimer : MonoBehaviour {

    //private Image wakuIma;
    private RectTransform wakuTra;
    public GameObject circletimerlinepre;

    [SerializeField, Header("ステージの制限時間 あとでタイマーから持ってくる")]
    private float time = 100.0f;


    // Use this for initialization
    void Start () {
        GameObject waku = transform.Find("Circle").gameObject;
        time = GameObject.Find("Time").GetComponent<Timer>().StageTime();
        if (waku == null) return;
        //wakuIma = waku.GetComponent<Image>();
        wakuTra = waku.GetComponent<RectTransform>();
        SaleSpown ss = GameObject.FindGameObjectWithTag("SaleSpown").GetComponent<SaleSpown>();
        if (ss == null) return;
        for (int i = 0; i < ss.GetAppearTime().Length; i++)
        {
            GameObject line = Instantiate(circletimerlinepre,transform.position, Quaternion.identity, waku.transform);
            RectTransform lintre = line.GetComponent<RectTransform>();
            lintre.localPosition = wakuTra.localPosition;
            line.transform.SetAsLastSibling(); //兄弟内での一番下（前面）
            lintre.eulerAngles = new Vector3(0,0, (360 / time) * ss.GetAppearTime()[i]);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!MainGameDate.IsStart()) return;
        //                          時間いっぱいかけて360度回転する
        wakuTra.eulerAngles -= new Vector3(0, 0, (360 / time) * Time.deltaTime);
	}
}
