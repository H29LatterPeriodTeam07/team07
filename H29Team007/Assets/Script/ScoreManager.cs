using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{

    private static int stageNumber = 1;
    private static int ningenCount = 0;//後で
    private static int doubutuCount = 0;//帰るはず

    public static void StageChenge(int stageNum)
    {
        stageNumber = stageNum;
        Reset();
    }

    public static void Reset()
    {

        ningenCount = 0;
        doubutuCount = 0;
    }

    /// <summary>捕まえた敵の数を増やす</summary>
    /// <param name="num">増やす敵の番号</param>
    public static void AddCount(int num)
    {
        switch (num)
        {
            case 1: ningenCount++; break;
            case 2: doubutuCount++; break;
        }
    }

    /// <summary>番号ごとの捕まえた敵の数</summary>
    /// <param name="num">敵の番号</param>
    /// <returns>何人（体）いるかを返す</returns>
    public static int GetCount(int num)
    {
        int result = 0;
        switch (num)
        {
            case 1: result = ningenCount; break;
            case 2: result = doubutuCount; break; 
        }
        return result;
    }

    /// <summary>捕まえた敵の数を増やす（袋版）</summary>
    /// <param name="num">増やす敵の番号</param>
    public static void AddCount(int[] num)
    {
        for (int i = 0; i < num.Length; i++)
        {
            switch (num[i])
            {
                case 1: ningenCount++; break;
                case 2: doubutuCount++; break;
            }
        }
    }

    /// <summary>ステージごとの敵のスコア</summary>
    /// <param name="num">敵の番号</param>
    /// <returns>敵の番号によるスコアを返す</returns>
    public static int EnemyPrice(int num)
    {
        int result = 0;
        switch (stageNumber)
        {
            case 1: result = Stage1Score(num); break;
        }
        return result;
    }

    /// <summary>ステージ1の各スコア</summary>
    /// <param name="num">敵の番号</param>
    /// <returns>敵の番号によるスコアを返す</returns>
    private static int Stage1Score(int num)
    {
        int result = 0;
        switch (num)
        {
            case 1: result = 100; break;
            case 2: result = 200; break;
        }
        return result;
    }

    /// <summary>各ステージでの敵の種類は何種類かを返す</summary>
    /// <returns></returns>
    public static int EnemyTypeCount()
    {
        int result = 0;
        switch (stageNumber)
        {
            case 1: result = 2; break;
        }
        return result;
    }
}
