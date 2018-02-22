using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameDate {
    
    public static float ROOFSHEIGHT = 3.0f;
    private static bool isStart = false;

    public static bool IsStart()
    {
        return isStart;
    }

    public static void ChangeStartFlag()
    {
        if (isStart)
        {
            isStart = false;
        }
        else
        {
            isStart = true;
        }
    }
}
