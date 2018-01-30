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
    static List<PatternData> patternDatas;

    private static int stageNumber = 1;
    //private static int ningenCount = 0;//後で
    //private static int doubutuCount = 0;//帰るはず
    //private static int pigCount = 0;
    //private static int cowCount = 0;
    //private static int fishCount = 0;

    //private static string readtxt = "";
    private static List<int> prices = new List<int>();  //ステージごとのスコア(txtからの読み込み)
    private static List<int> pointPercents = new List<int>();  //ステージごとのポイント取得(txtからの読み込み)
    public static List<string> enemysname = new List<string>(); //ステージごとの敵の名前(txtからの読み込み)
    private static List<int> scorescount = new List<int>(); //何番目のやつを何体取ったかを数える                                                            // 名前を変換する
    private static Dictionary<string, string> PriceDataNameToPrefabName = new Dictionary<string, string>();


    public static void StageChenge(int stageNum, StageSelectManager.PriceData priceData)
    {
        PriceDataNameToPrefabName["arai"] = "Arai";
        PriceDataNameToPrefabName["buta"] = "Pig";
        PriceDataNameToPrefabName["gyo"] = "Fish";
        PriceDataNameToPrefabName["hera"] = "Herazika";
        PriceDataNameToPrefabName["hitsu"] = "Sheep";
        PriceDataNameToPrefabName["inoshi"] = "Boar";
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
        // for (int i = 0; i < txt.Length; i++)
        // {
        //     scores.Add(int.Parse(txt[i]));//リストの中に入れる
        //     //Debug.Log(test[i]);
        // }
        // ReadEnemyNameFile();
        // txt = readtxt.Split(',');//,で区切る
        // for (int i = 0; i < txt.Length; i++)
        // {
        //     enemysname.Add(txt[i]);//リストの中に入れる
        //     //Debug.Log(txt[i]);
        // }

        // switch (stageNum)
        // {
        //     case 1: Stage1Pattern(); break;
        // }
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
        //ningenCount = 0;
        //doubutuCount = 0;
        //pigCount = 0;
        //cowCount = 0;
        //fishCount = 0;
    }

    /// <summary>捕まえた敵の数を増やす</summary>
    public static void AddCount(string name)
    {
        scorescount[EnemyNumber(name)]++;
        //switch (num)
        //{
        //    case 1: ningenCount++; break;
        //    case 2: doubutuCount++; break;
        //    case 3: pigCount++; break;
        //    case 4: cowCount++; break;
        //    case 5: fishCount++; break;
        //}
    }

    /// <summary>番号ごとの捕まえた敵の数</summary>
    /// <param name="num">敵の番号</param>
    /// <returns>何人（体）いるかを返す</returns>
    public static int GetCount(int num)
    {
        //int result = 0;
        //switch (num)
        //{
        //    case 1: result = ningenCount; break;
        //    case 2: result = doubutuCount; break;
        //    case 3: result = pigCount; break;
        //    case 4: result = cowCount; break;
        //    case 5: result = fishCount; break;
        //}
        //return result;
        return scorescount[num];
    }

    /// <summary>捕まえた敵の数を増やす（袋版）</summary>
    public static void AddCount(string[] name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            //switch (num[i])
            //{
            //    case 1: ningenCount++; break;
            //    case 2: doubutuCount++; break;
            //    case 3: pigCount++; break;
            //    case 4: cowCount++; break;
            //    case 5: fishCount++; break;
            //}
            scorescount[EnemyNumber(name[i])]++;
        }
    }

    /// <summary>ステージごとの敵のスコア</summary>
    public static int EnemyPrice(string name)
    {
        //int result = 0;
        //switch (stageNumber)
        //{
        //    case 1: result = Stage1Score(num); break;
        //    case 2: result = Stage2Score(num); break;
        //    case 3: result = Stage3Score(num); break;
        //}
        //return result;
        return prices[EnemyNumber(name)];
    }

    public static PatternData GetEnemyPatternData(string[] names)
    {
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
                        break;
                    }
                }
            }
            foreach(var j in patternFlags)
            {
                if (!j)
                {
                    continue;
                }
                // 計算&離脱
            }
            // データ名からprefab名に変換
            return i;
        }
        PatternData patternData;
        patternData.PatternList = new List<string>();
        patternData.PatternName = "None";
        patternData.pointPercents = 0;
        return patternData;
    }

    public static int GetPatternPoint(PatternData data)
    {
        int result = 0;
        foreach(var i in data.PatternList)
        {
            result += EnemyPoint(i);
        }

        result = (int)((float)result / 100.0f * (float)data.pointPercents);
        return result;
    }

    public static int EnemyPoint(string name)
    {
        return (prices[EnemyNumber(name)] / 100) * pointPercents[EnemyNumber(name)];
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
        //switch (stageNumber)
        //{
        //    case 1: result = Stage1EnemyNumber(name);break;
        //    case 2: result = Stage2EnemyNumber(name);break;
        //    case 3: result = Stage3EnemyNumber(name);break;
        //}
        return result;
    }
    /*
    /// <summary>ステージ1の敵の番号</summary>
    /// <param name="name">敵の名前</param>
    private static int Stage1EnemyNumber(string name)
    {
        int result = 0;
        if (name.Contains("Pig"))
        {
            result = 1;
        }
        else if(name.Contains("Bull"))
        {
            result = scores.Count - 1;
        }
        return result;
    }

    /// <summary>ステージ2の敵の番号</summary>
    /// <param name="name">敵の名前</param>
    private static int Stage2EnemyNumber(string name)
    {
        int result = 0;
        if (name.Contains("Pig"))
        {
            result = 1;
        }
        else if (name.Contains("Cow"))
        {
            result = 2;
        }
        else if (name.Contains("Bull"))
        {
            result = scores.Count - 1;
        }
        return result;
    }

    /// <summary>ステージ3の敵の番号</summary>
    /// <param name="name">敵の名前</param>
    private static int Stage3EnemyNumber(string name)
    {
        int result = 0;
        if (name.Contains("Pig"))
        {
            result = 1;
        }
        else if (name.Contains("Cow"))
        {
            result = 2;
        }
        else if (name.Contains("Fish"))
        {
            result = 3;
        }
        else if (name.Contains("Bull"))
        {
            result = scores.Count - 1;
        }
        return result;
    }
    */

    //private static void Stage1Pattern() //txtからの漢字の読み込みがわからんからScriptに直打ち
    //{
    //    prices.Add(1500);  //下のスコア
    //    enemysname.Add("三匹の子豚"); //Patternの名前
    //}

    /// <summary>各ステージでの敵の種類は何種類かを返す</summary>
    /// <returns></returns>
    public static int EnemyTypeCount()
    {
        //int result = 0;
        //switch (stageNumber)
        //{
        //    case 1: result = 3; break;
        //    case 2: result = 4; break;
        //    case 3: result = 5; break;
        //}
        //result = 5; //もう全部でいいんじゃね
        //return result;
        return prices.Count;
    }

    public static int GetStageNumber()
    {
        return stageNumber;
    }

    // private static void ReadEnemyScoreFile()
    // {
    //     StreamReader reder;
    //     reder = new StreamReader(
    //        Application.streamingAssetsPath + "//enemyscore0" + stageNumber.ToString() + ".txt",
    //         System.Text.Encoding.GetEncoding("utf-8"));
    // 
    //     readtxt = reder.ReadLine();
    //     reder.Close();
    // }

    // private static void ReadEnemyNameFile()
    // {
    //     StreamReader reder;
    //     reder = new StreamReader(
    //        Application.streamingAssetsPath + "//enemyname0" + stageNumber.ToString() + ".txt",
    //         System.Text.Encoding.GetEncoding("utf-8"));
    // 
    //     readtxt = reder.ReadLine();
    //     reder.Close();
    // }
}

