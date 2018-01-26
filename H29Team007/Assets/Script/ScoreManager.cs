using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreManager
{

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
    private static List<int> scorescount = new List<int>(); //何番目のやつを何体取ったかを数える

    public static void StageChenge(int stageNum, StageSelectManager.PriceData priceData)
    {
        // 名前を変換する
        Dictionary<string, string> PriceDataNameToPrefabName = new Dictionary<string, string>();
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

        switch (stageNum)
        {
            case 1: Stage1Pattern(); break;
        }

        Reset();
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

    private static void Stage1Pattern() //txtからの漢字の読み込みがわからんからScriptに直打ち
    {
        prices.Add(1500);  //下のスコア
        enemysname.Add("三匹の子豚"); //Patternの名前
    }

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

