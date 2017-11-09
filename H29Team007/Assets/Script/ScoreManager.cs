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

    public static void AddCount(int num)
    {
        switch (num)
        {
            case 1: ningenCount++; break;
            case 2: doubutuCount++; break;
        }
    }

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

    public static int EnemyPrice(int num)
    {
        int result = 0;
        switch (stageNumber)
        {
            case 1: result = Stage1Score(num); break;
        }
        return result;
    }

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
