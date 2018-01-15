using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour {

    public CharaFall[] falls;
    private int index = 0;
    private List<CharaFall> fallList = new List<CharaFall>(); 

	// Use this for initialization
	void Start () {
        fallList.Clear();
        for (int i = 0; i < ScoreManager.EnemyTypeCount(); i++)
        {
            int enemycount = ScoreManager.GetCount(i);
            if (enemycount == 0) continue;
            string enemyname = ScoreManager.enemysname[i];
            switch (enemyname)
            {
                case "human": fallList.Add(falls[0]); break;
                case "Pig": fallList.Add(falls[2]); break;
                case "Cow": fallList.Add(falls[3]); break;
                case "Fish": fallList.Add(falls[4]); break;
                case "Lamborghini": fallList.Add(falls[1]); break;
            }

        }
        if (fallList.Count == 0) return;
        fallList[index].FallStart(ScoreManager.GetCount(index));
	}
	
	// Update is called once per frame
	void Update () {
        if (index == fallList.Count - 1 || fallList.Count == 0) return;
        if (fallList[index].IsEnd())
        {
            index++;
            fallList[index].FallStart(ScoreManager.GetCount(index));
        }
    }
}
