using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTimer : MonoBehaviour {

    //private Image wakuIma;
    private RectTransform wakuTra;
    private Image redWaku;
    public GameObject circletimerlinepre;

    private Timer stageTimer;
    private int alphaPlus = 1;
    private float alphaTime = 0.0f;
    private bool childDeth = false;
    private SaleSpown ss;
    private List<GameObject> lines;
    [SerializeField, Header("ステージの制限時間 あとでタイマーから持ってくる")]
    private float time = 100.0f;


    // Use this for initialization
    void Start () {
        GameObject waku = transform.Find("Circle").gameObject;
        stageTimer = GameObject.Find("Time").GetComponent<Timer>();
        time = stageTimer.StageTime();
        if (waku == null) return;
        //wakuIma = waku.GetComponent<Image>();
        wakuTra = waku.GetComponent<RectTransform>();
        redWaku = waku.transform.Find("redCircle").GetComponent<Image>();
        ss = GameObject.FindGameObjectWithTag("SaleSpown").GetComponent<SaleSpown>();
        if (ss == null) return;
        lines = new List<GameObject>();
        for (int i = 0; i < ss.GetAppearTime().Length; i++)
        {
            GameObject line = Instantiate(circletimerlinepre,transform.position, Quaternion.identity, waku.transform);
            RectTransform lintre = line.GetComponent<RectTransform>();
            lintre.localPosition = wakuTra.localPosition;
            line.transform.SetAsLastSibling(); //兄弟内での一番下（前面）
            lintre.eulerAngles = new Vector3(0,0, (360 / time) * ss.GetAppearTime()[i]);
            lines.Add(line);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!MainGameDate.IsStart()) {
            redWaku.color = new Color(redWaku.color.r, redWaku.color.g, redWaku.color.b, 0);
            return;
        }

        if (!childDeth)
        {
            for(int i = 0; i < lines.Count; i++)
            {
                Destroy(lines[i], ss.GetAppearTime()[i]);
            }
            childDeth = true;
        }

        //                          時間いっぱいかけて360度回転する
        wakuTra.eulerAngles -= new Vector3(0, 0, (360 / time) * Time.deltaTime);
        if(time - stageTimer.NowTime() < time/4)
        {
            if (alphaTime > 1 || alphaTime < 0)
            {
                alphaPlus *= -1;
            }
            alphaTime = Mathf.Clamp(alphaTime, 0, 1);

            redWaku.color = new Color(redWaku.color.r, redWaku.color.g, redWaku.color.b, alphaTime);
            alphaTime += Time.deltaTime * alphaPlus;
            
        }
    }
}
