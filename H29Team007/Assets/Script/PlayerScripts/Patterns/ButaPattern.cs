using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButaPattern {

    static public int PatternNumber(Transform under,Transform center,Transform top)
    {
        int pigCount = PigCount(under, center, top);
        int cowCount = CowCount(under, center, top);
        int fishCount = FishCount(under, center, top);
        

        return PatternCheck(pigCount,cowCount,fishCount);
    }

    static private int PatternCheck(int pc,int cc,int fc)
    {
        int result = 0;
        if(pc == 3)
        {
            result = 1;
        }
        if(pc == 1)
        {
            if(cc == 1)
            {
                if(fc == 1)
                {
                    result = 2;
                }
            }
        }

        return result;
    }

    static private int PigCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Pig")) result++;
        if (center.name.Contains("Pig")) result++;
        if (top.name.Contains("Pig")) result++;

        return result;
    }

    static private int CowCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Cow")) result++;
        if (center.name.Contains("Cow")) result++;
        if (top.name.Contains("Cow")) result++;

        return result;
    }

    static private int FishCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Fish")) result++;
        if (center.name.Contains("Fish")) result++;
        if (top.name.Contains("Fish")) result++;

        return result;
    }

}
