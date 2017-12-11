using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    /*
     0:カート持つまで
     1:カート持っての移動
     2:滑走
     3:滑走終了
     4:人轢く
     5:豚轢く
     6:傾き
     7:カートジャック
     8:カートジャック終了
     9:スコア
     10:タイマー
     11:ミニマップ
     12:ダブルカート
     13:まとめ
     14:シーン移動
         */


    private int tutorialIndex = 0;
    private TutorialPlayer player;
    private TutorialShopping shopping;
    public GameObject pigPrefab;
    public GameObject hagePrefab;
    public GameObject cartRigit;
    public GameObject bbaPrehab;
    public GameObject[] points;
    public GameObject reji;
    public Fade fade;
    private bool isFadeNow = false;

    private Vector3 startPoint;
    private GameObject p;
    private TutorialCamera camera;

    private bool rpush = false;
    private bool lpush = false;

    private GameObject hage1;
    private GameObject hage2;
    private GameObject pig1;
    private GameObject bba;

    public GameObject scoreG;
    public GameObject timerG;
    public GameObject mapG;

    private float time;

    // Use this for initialization
    void Start () {
        p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<TutorialPlayer>();
        shopping = p.GetComponent<TutorialShopping>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<TutorialCamera>();
        startPoint = p.transform.position;
        reji.SetActive(false);
        scoreG.SetActive(false);
        timerG.SetActive(false);
        mapG.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isFadeNow)
        {
            if (fade.IsFadeEnd())
            {
                switch (tutorialIndex)
                {
                    case 4: Index4Start(); break;
                    case 5: Index5Start(); break;
                    case 7: Index7Start(); break;
                    case 9: Index9Start(); break;
                    case 10: Index10Start(); break;
                    case 11: Index11Start(); break;
                    case 12: Index12Start(); break;
                    case 13: Index13Start(); break;
                    case 14: Index14Start(); break;
                }
                p.transform.position = startPoint;
                p.transform.eulerAngles = Vector3.zero;
                camera.CameraReset();
                fade.FadeIn(1.0f);
                isFadeNow = false;
            }
        }
        else if (fade.IsFadeEnd())
        {
            switch (tutorialIndex)
            {
                case 0: Index0Update(); break;
                case 1: Index1Update(); break;
                case 2: Index2Update(); break;
                case 3: Index3Update(); break;
                case 4: Index4Update(); break;
                case 5: Index5Update(); break;
                case 6: Index6Update(); break;
                case 7: Index7Update(); break;
                case 8: Index8Update(); break;
                case 9: Index9Update(); break;
                case 10: Index10Update(); break;
                case 11: Index11Update(); break;
                case 12: Index12Update(); break;
                case 13: Index13Update(); break;
            }

        }
    }

    private void Index0Update()
    {
        //籠持つ
        if (player.IsCart())
        {
            IndexNext();
        }
    }

    private void Index1Update()
    {
        //特定の場所まで移動
        if(Vector3.Distance(p.transform.position,points[5].transform.position) < 2.0f)
        {
            IndexNext();
        }
    }

    private void Index2Update()
    {
        //滑走
        if (player.GetState() == TutorialPlayer.PlayerState.Gliding)
        {
            IndexNext();
        }
    }

    private void Index3Update()
    {
        //滑走終了
        if (player.GetState() == TutorialPlayer.PlayerState.OnCart)
        {
            IndexNext();
        }
    }

    private void Index4Update()
    {
        //人二人引く
        if (shopping.GetAllCount() == 2)
        {
            IndexNext();
        }
    }

    private void Index5Update()
    {
        //豚引く
        if (shopping.GetAllCount() == 3)
        {
            IndexNext();
        }

    }

    private void Index6Update()
    {
        //傾き
        if (lpush && rpush)
        {
            IndexNext();
        }

    }

    private void Index7Update()
    {
        //カートジャック
        if (player.GetState() == TutorialPlayer.PlayerState.Takeover)
        {
            IndexNext();
        }
    }

    private void Index8Update()
    {
        //カートジャック終了
        if (player.IsCart())
        {
            IndexNext();
        }
    }

    private void Index9Update()
    {
        //スコア
        time += Time.deltaTime;
        if ((int)time % 2 == 0)
        {
            scoreG.SetActive(true);
        }
        else
        {
            scoreG.SetActive(false);
        }
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            IndexNext();
        }
    }

    private void Index10Update()
    {
        //タイマー
        time += Time.deltaTime;
        if ((int)time % 2 == 0)
        {
            timerG.SetActive(true);
        }
        else
        {
            timerG.SetActive(false);
        }
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            IndexNext();
        }
    }

    private void Index11Update()
    {
        //ミニマップ
        time += Time.deltaTime;
        if((int)time % 2 == 0)
        {
            mapG.SetActive(true);
        }
        else
        {
            mapG.SetActive(false);
        }
        if (Input.GetButtonDown("XboxB") || Input.GetKeyDown(KeyCode.O))
        {
            IndexNext();
        }
    }

    private void Index12Update()
    {
        //ダブルカート
        if (player.IsCart2())
        {
            IndexNext();
        }
    }

    private void Index13Update()
    {

        if (shopping.GetAllCount() >= 6)
        {
            reji.SetActive(true);
        }
        else
        {
            reji.SetActive(false);
        }

        //全部捕まえて袋
        if (shopping.IsPlasticBag())
        {
            IndexNext();
        }
    }

    private void IndexNext()
    {
        tutorialIndex++;
        if (tutorialIndex == 3 || tutorialIndex == 8) return;
        fade.FadeOut(1.0f);
        isFadeNow = true;
    }

    private void Index4Start()
    {
        hage1 = Instantiate(hagePrefab);
        hage2 = Instantiate(hagePrefab);
        hage1.transform.position = points[0].transform.position;
        hage2.transform.position = points[2].transform.position;
    }

    private void Index5Start()
    {
        pig1 = Instantiate(pigPrefab);

        pig1.transform.position = points[0].transform.position;
    }

    private void Index7Start()
    {
        shopping.BaggegeFall(p.transform.position);
        Destroy(hage1);
        Destroy(hage2);
        Destroy(pig1);
        player.BreakCart();
        bba = Instantiate(bbaPrehab);

        bba.transform.position = points[1].transform.position;

    }


    private void Index9Start()
    {
        Destroy(bba);
    }

    private void Index10Start()
    {
        scoreG.SetActive(true);
    }

    private void Index11Start()
    {
        timerG.SetActive(true);
    }

    private void Index12Start()
    {
        mapG.SetActive(true);
        GameObject cart = Instantiate(cartRigit);

        cart.transform.position = points[1].transform.position;
    }

    private void Index13Start()
    {
        GameObject hage11 = Instantiate(hagePrefab);
        GameObject hage12 = Instantiate(hagePrefab);
        GameObject hage13 = Instantiate(hagePrefab);
        GameObject hage14 = Instantiate(hagePrefab);
        GameObject pig11 = Instantiate(pigPrefab);
        GameObject pig12 = Instantiate(pigPrefab);

        hage11.transform.position = points[0].transform.position;
        hage12.transform.position = points[1].transform.position;
        hage13.transform.position = points[2].transform.position;
        hage14.transform.position = points[3].transform.position;
        pig11.transform.position = points[4].transform.position;
        pig12.transform.position = points[5].transform.position;
    }

    private void Index14Start()
    {
        SceneManager.LoadScene("Title");
    }

    public void LPush()
    {
        lpush = true;
    }

    public void RPush()
    {
        rpush = true;
    }

    public int TutorialIndex()
    {
        return tutorialIndex;
    }

    public bool FadeEnd()
    {
        return fade.IsFadeEnd();
    }
}
