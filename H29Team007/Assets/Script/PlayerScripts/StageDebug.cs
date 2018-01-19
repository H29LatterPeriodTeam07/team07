using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDebug : MonoBehaviour {

    public int stagenum = 0;
    List<StageSelectManager.PriceData> m_Datas;

	// Use this for initialization
	void Start () {
        m_Datas = new  List<StageSelectManager.PriceData>();
        CreateData();
        ScoreManager.StageChenge(stagenum, m_Datas[stagenum]);
	}
	
	// Update is called once per frame
	void Update () {
		
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
            StageSelectManager.PriceData l_data;
            l_data.prices = new Dictionary<string, int>();
            l_data.secretPrices = new Dictionary<string, int>();

            string l_line = row[i].Replace("\r", "");
            if (l_line == "") continue;
            // [//]がある場合無視
            if (l_line.Substring(0, 2) == "//") continue;
            // [_]で分割
            data = l_line.Split(';');
            // 1列目はステージ番号
            l_data.StageIndex = int.Parse(data[0]);

            // 2,3列目は名前と価格
            for (int j = 1; j <= 2; ++j)
            {
                // 空の場合スキップ
                if (data[j] == "") continue;
                animal = data[j].Split('/');
                for (int k = 0; k < animal.Length; ++k)
                {
                    price = animal[k].Split('_');
                    // 保存する
                    if (j == 1)
                        l_data.prices[price[0]] = int.Parse(price[1]);
                    else
                        l_data.secretPrices[price[0]] = int.Parse(price[1]);
                }
            }

            // 丸付け
            l_data.checkAnimalName = data[3];
            // 情報初期化
            l_data.IsCheck = false;
            m_Datas.Add(l_data);
        }
    }
}
