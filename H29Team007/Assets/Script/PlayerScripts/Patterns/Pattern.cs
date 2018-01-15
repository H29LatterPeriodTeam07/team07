using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern {

    static public int PatternNumber(Transform under, Transform center, Transform top)
    {
        int pigCount = PigCount(under, center, top);
        int cowCount = CowCount(under, center, top);
        int fishCount = FishCount(under, center, top);
        int lambrCount = LamborghiniCount(under, center, top);
        int chikinCount = ChickinCount(under, center, top);
        int araiCount = AraiCount(under, center, top);
        int herazikaCount = HerazikaCount(under, center, top);
        int kazikiCount = KazikiCount(under, center, top);
        int goatCount = GoatCount(under, center, top);
        int sharkCount = SharkCount(under, center, top);
        int sheepCount = SheepCount(under, center, top);


        return PatternCheck(pigCount, cowCount, fishCount,lambrCount,chikinCount,araiCount,herazikaCount,kazikiCount,goatCount,sharkCount,sheepCount);
    }

    static private int PatternCheck(int pig, int cow, int fish, int lambr, int chikin, int arai, int herazika,int kaziki,int goat,int shark,int sheep)
    {
        int result = 0;
        if (pig == 3)
        {
            result = 1;
        }
        if (pig == 1)
        {
            if (cow == 1)
            {
                if (fish == 1)
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

    static private int LamborghiniCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Lamborghini")) result++;
        if (center.name.Contains("Lamborghini")) result++;
        if (top.name.Contains("Lamborghini")) result++;

        return result;
    }

    static private int ChickinCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Chickin")) result++;
        if (center.name.Contains("Chickin")) result++;
        if (top.name.Contains("Chickin")) result++;

        return result;
    }

    static private int AraiCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Arai")) result++;
        if (center.name.Contains("Arai")) result++;
        if (top.name.Contains("Arai")) result++;

        return result;
    }

    static private int GoatCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Goat")) result++;
        if (center.name.Contains("Goat")) result++;
        if (top.name.Contains("Goat")) result++;

        return result;
    }

    static private int HerazikaCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Herazika")) result++;
        if (center.name.Contains("Herazika")) result++;
        if (top.name.Contains("Herazika")) result++;

        return result;
    }

    static private int KazikiCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Kaziki")) result++;
        if (center.name.Contains("Kaziki")) result++;
        if (top.name.Contains("Kaziki")) result++;

        return result;
    }

    static private int SharkCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Shark")) result++;
        if (center.name.Contains("Shark")) result++;
        if (top.name.Contains("Shark")) result++;

        return result;
    }

    static private int SheepCount(Transform under, Transform center, Transform top)
    {
        int result = 0;

        if (under.name.Contains("Sheep")) result++;
        if (center.name.Contains("Sheep")) result++;
        if (top.name.Contains("Sheep")) result++;

        return result;
    }
}
