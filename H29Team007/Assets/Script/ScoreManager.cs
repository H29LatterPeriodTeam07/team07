using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreManager
{
    public struct PatternData
    {
        // パターンを構成する動物名リスト
        public List<string> PatternList;
        // パターンの名前
        public string PatternName;
        // パターンがそろった時の獲得ポイント割合
        // パターン動物それぞれの獲得ポイント＊獲得ポイント割合
        public int pointPercents;
    }
    public struct PatternResultData
    {
        // パターンの名前
        public string PatternName;
        public List<string> nameList;
        public int point;
    }

    static List<PatternData> patternDatas;

    private static int stageNumber = 1;

    //private static string readtxt = "";
    private static List<int> prices = new List<int>();  //ステージごとのスコア(txtからの読み込み)
    private static List<int> pointPercents = new List<int>();  //ステージごとのポイント取得(txtからの読み込み)
    public static List<string> enemysname = new List<string>(); //ステージごとの敵の名前(txtからの読み込み)
    private static List<int> scorescount = new List<int>(); //何番目のやつを何体取ったかを数える                              
    // レシートに書くパターン群
    private static List<PatternResultData> patternresults = new List<PatternResultData>();

    // 名前を変換する
    private static Dictionary<string, string> PriceDataNameToPrefabName = new Dictionary<string, string>();


    public static void StageChenge(int stageNum, StageSelectManager.PriceData priceData)
    {
        patternresults.Clear();
        PriceDataNameToPrefabName.Clear();
        PriceDataNameToPrefabName["arai"] = "Arai";
        PriceDataNameToPrefabName["buta"] = "Pig";
        PriceDataNameToPrefabName["gyo"] = "Fish";
        PriceDataNameToPrefabName["hera"] = "Herazika";
        PriceDataNameToPrefabName["hitsu"] = "Sheep";
        PriceDataNameToPrefabName["kaji"] = "Kaziki";
        PriceDataNameToPrefabName["niwa"] = "Chickin";
        PriceDataNameToPrefabName["tougyu"] = "Lamborghini";
        PriceDataNameToPrefabName["ushi"] = "Cow";
        PriceDataNameToPrefabName["same"] = "Shark";

        stageNumber = stageNum;
        if (stageNumber == 0) stageNumber = 1;
        // ReadEnemyScoreFile();
        // string[] txt = readtxt.Split(',');//,で区切る
        prices.Clear();
        prices.Add(100); //人間のスコア
        enemysname.Clear();
        enemysname.Add("human"); //人間 リストの場所をあわせるため
        pointPercents.Clear();
        pointPercents.Add(0);
        // チラシに表示されているもの
        foreach (var i in priceData.prices)
        {
            enemysname.Add(PriceDataNameToPrefabName[i.Key]);
            prices.Add(i.Value);
        }
        // チラシに表示されていないもの
        foreach (var i in priceData.secretPrices)
        {
            enemysname.Add(PriceDataNameToPrefabName[i.Key]);
            prices.Add(i.Value);
        }

        foreach (var i in priceData.pointPercents)
        {
            pointPercents.Add(i.Value);
        }
        CreateData();
        Reset();
    }

    private static void CreateData()
    {
        patternDatas = new List<PatternData>();
        //resourcesフォルダ内にあるsampleTextファイルをロード
        TextAsset textAsset = Resources.Load("PatternData") as TextAsset;
        //ロードした中身を
        //1行ずつに分割
        string[] row = textAsset.text.Split('\n');
        string[] data;
        for (int i = 0; i < row.Length; i++)
        {
            PatternData l_data;
            l_data.PatternList = new List<string>();
            string l_line = row[i].Replace("\r", "");
            if (l_line == "") continue;
            // [//]がある場合無視
            if (l_line.Substring(0, 2) == "//") continue;
            // [;]で分割
            data = l_line.Split(';');
            // 1列目はパターン名
            l_data.PatternName = data[0];

            // 2列目はパターンを構成する動物名
            string[] patternanimalNames = data[1].Split(',');
            foreach (var j in patternanimalNames)
            {
                l_data.PatternList.Add(PriceDataNameToPrefabName[j]);
            }

            // ３列目は獲得ポイント割合
            string late = data[2];
            l_data.pointPercents = int.Parse(late);

            patternDatas.Add(l_data);
        }
    }

    public static void Reset()
    {
        scorescount.Clear();
        for (int i = 0; i < prices.Count; i++)
        {
            scorescount.Add(0);
            //Debug.Log(scorescount[i]);
        }
    }

    /// <summary>捕まえた敵の数を増やす</summary>
    public static void AddCount(string name)
    {
        scorescount[EnemyNumber(name)]++;
    }

    /// <summary>番号ごとの捕まえた敵の数</summary>
    /// <param name="num">敵の番号</param>
    /// <returns>何人（体）いるかを返す</returns>
    public static int GetCount(int num)
    {
        return scorescount[num];
    }

    /// <summary>捕まえた敵の数を増やす（袋版）</summary>
    public static void AddCount(string[] name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            scorescount[EnemyNumber(name[i])]++;
        }
    }

    /// <summary>ステージごとの敵のスコア</summary>
    public static int EnemyPrice(string name)
    {
        return prices[EnemyNumber(name)];
    }

    public static PatternData GetEnemyPatternData(string[] names)
    {
        PatternData result;
        result.PatternList = new List<string>();
        result.PatternList.Add("");
        result.PatternList.Add("");
        result.PatternList.Add("");
        result.PatternName = "None";
        result.pointPercents = 0;
        // 3つフラグがたったらパターン
        bool[] patternFlags = new bool[3];
        foreach(var i in patternDatas)
        {
            // 初期化
            for(int j = 0; j < patternFlags.Length; ++j)
            {
                patternFlags[j] = false;
            }

            // パターンリストを確認
            for(int j = 0; j < i.PatternList.Count; ++j)
            {
                // カゴの名前を回す
                for(int k = 0; k < names.Length; ++k)
                {
                    // パターンフラグが立っていないとき
                    // かつ、かごの名前と一致する場合
                    if (!patternFlags[k] && names[k].Contains(i.PatternList[j]))
                    {
                        patternFlags[k] = true;
                        result.PatternList[k] = i.PatternList[j];
                        break;
                    }
                }
            }
            bool l_isHitPattern = true;
            foreach(var j in patternFlags)
            {
                if (!j)
                {
                    l_isHitPattern = false;
                }
            }
            if (!l_isHitPattern) continue;
            result.PatternName = i.PatternName;
            result.pointPercents = i.pointPercents;
            return result;
        }
        return result;
    }

    public static int GetPatternPoint(PatternData data)
    {
        int result = 0;
        foreach(var i in data.PatternList)
        {
            result += EnemyPoint(i);
        }

        // 切り上げる
        result = (int)((float)result  / 100.0f * (float)data.pointPercents + 0.9f);
        return result;
    }

    public static int EnemyPoint(string name)
    {
        return ((prices[EnemyNumber(name)] + 99) / 100) * pointPercents[EnemyNumber(name)];
    }

    public static void AddResultPatternData(PatternResultData data)
    {
        patternresults.Add(data);
    }
    public static List<PatternResultData> GetResultPatternDatas()
    {
        return patternresults;
    }

    private static int EnemyNumber(string name)
    {
        int result = 0;
        for (int i = 0; i < enemysname.Count; i++)
        {
            if (name.Contains(enemysname[i]))
            {
                return i;
            }
        }
        return result;
    }

    /// <summary>各ステージでの敵の種類は何種類かを返す</summary>
    /// <returns></returns>
    public static int EnemyTypeCount()
    {
        return prices.Count;
    }

    public static int GetStageNumber()
    {
        return stageNumber;
    }
}

