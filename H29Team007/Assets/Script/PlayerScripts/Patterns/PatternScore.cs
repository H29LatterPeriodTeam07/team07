using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternScore {

	static public int PatternPoint(int number)
    {
        int result = 0;
        switch (number)
        {
            case 1: result = 1001;break;
            case 2: result = 1002;break;
        }
        return result;
    }

    static public string PatternText(int number)
    {
        string result = "";
        switch (number)
        {
            case 1: result = "三匹の子豚"; break;
            case 2: result = "最初の三匹"; break;
        }
        return result;
    }

}
