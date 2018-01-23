using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour {
    public GameObject m_CheckUI;
    GameObject Flayers;
    GameObject Arrows;
    public int m_FlyerCount;
    bool m_IsChange;
    // データ
    public struct PriceData{
        public int StageIndex;
        public Dictionary<string, int> prices;
        public Dictionary<string, int> pointPercents;
        public Dictionary<string, int> secretPrices;
        public bool IsCheck;
        public string checkAnimalName;
    }

    private List<PriceData> m_Datas;
    private List<string> m_SceneNames;
    
    private int currentSelectStageIndex;


    public SoundManagerScript sm;
    public GameObject m_NowLoad;
    AsyncOperation async;

    // Use this for initialization
    void Start () {
        m_Datas = new List<PriceData>();
        currentSelectStageIndex = 0;
        Flayers = transform.Find("Screen").Find("Flyers").gameObject;
        Arrows = Flayers.transform.Find("Arrows").gameObject;
        m_FlyerCount = 0;
        for (int i = 0; i <Flayers.transform.childCount; ++i){
            if(Flayers.transform.GetChild(i).name.Split('_')[0] == "Flyer")
            {
                ++m_FlyerCount;
            }
        }
        CreateData();
        SetFlyerGoodsPrice();

        m_NowLoad.SetActive(false);
        m_SceneNames = new List<string>();
        m_SceneNames.Add("Tutorial");
        m_SceneNames.Add("Stage1");
        m_SceneNames.Add("Stage2.1");
        m_SceneNames.Add("Stage3");
        m_SceneNames.Add("Stage4");
        m_SceneNames.Add("Stage5");
        m_SceneNames.Add("Stage6");
        m_SceneNames.Add("Stage7");
        m_SceneNames.Add("Stage8");
    }
	
	// Update is called once per frame
	void Update () {
        UpdateSelect();
        UpdateSceneChange();
    }

    void UpdateSceneChange()
    {
        if (!Flayers.GetComponent<Flyers>().IsReachTargetPositionX()) return;
        if(Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O)){
            sm.PlaySE(0);
            async = SceneManager.LoadSceneAsync(m_SceneNames[currentSelectStageIndex]);
            StartCoroutine("LoadScene");
            ScoreManager.StageChenge(currentSelectStageIndex, m_Datas[currentSelectStageIndex]);
            m_NowLoad.SetActive(true);
        }

        if (Input.GetButton("XboxA") || Input.GetKeyDown(KeyCode.F))
        {
            sm.PlaySE(1);
            SceneManager.LoadScene("Title");
        }
    }

    void UpdateSelect()
    {
        const float Margin = 0.5f;
        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(inputHorizontal) > Margin && !m_IsChange)
        {
            if (inputHorizontal > 0.0f)
            {
                currentSelectStageIndex += 1;
            }
            else
            {
                currentSelectStageIndex += (m_FlyerCount - 1);
            }
            currentSelectStageIndex = currentSelectStageIndex % m_FlyerCount;
            float l_positionX = currentSelectStageIndex * 1280;
            Arrows.GetComponent<Arrows>().SetTargetLocalPositionX(l_positionX);
            Flayers.GetComponent<Flyers>().MoveTargetPositionX(-l_positionX);
            m_IsChange = true;
        }
        else if(Mathf.Abs(inputHorizontal) <= Margin)
        {
            m_IsChange = false;
        }

        if (Flayers.GetComponent<Flyers>().IsReachTargetPositionX())
        {
            PriceData l_data = m_Datas[currentSelectStageIndex];
            if (!l_data.IsCheck)
            {
                GameObject l_checkUI = Instantiate(m_CheckUI);
                GameObject l_flyer = Flayers.transform.GetChild(currentSelectStageIndex).gameObject;
                l_flyer.GetComponent<Flyer>().SetImageGoodslocalPosition(l_checkUI, l_data.checkAnimalName);
                bool isFirst = Random.Range(0, 2) == 0;
                l_checkUI.GetComponent<Animator>().SetBool("IsFirst", !isFirst);
                l_checkUI.GetComponent<Animator>().enabled = true;
                l_checkUI.GetComponent<Image>().color = Color.white;
                Vector3 l_scale;
                if(currentSelectStageIndex < 2)
                {
                    l_scale = Vector3.one * 1.25f;
                }
                else if(currentSelectStageIndex < 5)
                {
                    l_scale = Vector3.one;
                }
                else
                {
                    l_scale = Vector3.one * 0.5f;
                }
                l_checkUI.GetComponent<RectTransform>().localScale = l_scale;
                l_data.IsCheck = true;
                m_Datas[currentSelectStageIndex] = l_data;
            }
        }
    }

    void CreateData()
    {
        //resourcesフォルダ内にあるsampleTextファイルをロード
        TextAsset textAsset = Resources.Load("PriceData") as TextAsset;
        //ロードした中身を
        //1行ずつに分割
        string[] row = textAsset.text.Split('\n');
        string[] data;
        string[] animal;
        string[] price;
        for (int i = 0; i < row.Length; i++)
        {
            PriceData l_data;
            l_data.prices = new Dictionary<string, int>();
            l_data.secretPrices = new Dictionary<string, int>();
            l_data.pointPercents = new Dictionary<string, int>();
            string l_line = row[i].Replace("\r", "");
            if (l_line == "") continue;
            // [//]がある場合無視
            if (l_line.Substring(0, 2) == "//") continue;
            // [_]で分割
            data = l_line.Split(';');
            // 1列目はステージ番号
            string[] stagedata = data[0].Split('_');
            l_data.StageIndex = int.Parse(stagedata[0]);


            // 2,3列目は名前と価格
            for(int j = 1; j <= 2; ++j)
            {
                // 空の場合スキップ
                if (data[j] == "") continue;
                animal = data[j].Split('/');
                for (int k = 0; k < animal.Length; ++k)
                {
                    price = animal[k].Split('_');
                    // 保存する
                    if(j == 1)
                    l_data.prices[price[0]] = int.Parse(price[1]);
                    else
                    l_data.secretPrices[price[0]] = int.Parse(price[1]);
                    l_data.pointPercents[price[0]] = int.Parse(stagedata[1]);
                }
            }

            // 丸付け
            string[] checkAnimalData = data[3].Split('_');
            l_data.checkAnimalName = checkAnimalData[0];
            l_data.pointPercents[l_data.checkAnimalName] = int.Parse(checkAnimalData[1]);
            // 情報初期化
            l_data.IsCheck = false;
            m_Datas.Add(l_data);
        }
    }

    void SetFlyerGoodsPrice()
    {
        int StartBigFlyerCount = 3;
        GameObject l_flyer;
        for(int i = 0; i < m_FlyerCount; ++i)
        {
            l_flyer = Flayers.transform.GetChild(i).gameObject;
            // 価格設定
            foreach (var j in m_Datas[i].prices) {
                l_flyer.GetComponent<Flyer>().SetPrice(j.Key, j.Value);
            }
            // 回転
            float angle;
            if (i < StartBigFlyerCount)
            {
                angle = Random.Range(-10, 10);
            }
            else
            {
                angle = Random.Range(-2, 2);
            }
            l_flyer.GetComponent<RectTransform>().Rotate(0.0f, 0.0f, angle);
        }
    }

    IEnumerator LoadScene()
    {
        async.allowSceneActivation = false;    // シーン遷移をしない

        while (async.progress < 0.9f)
        {
            Debug.Log(async.progress);
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Scene Loaded");


        yield return new WaitForSeconds(1);

        async.allowSceneActivation = true;    // シーン遷移許可

    }
}
