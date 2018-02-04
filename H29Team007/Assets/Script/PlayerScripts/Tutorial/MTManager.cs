using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class MTManager : MonoBehaviour {

    /*
     0:スコア
     1:タイマー
     2:ミニマップ
     3:カート持つまで
     4:カート持っての移動(滑走の説明)
     5:人轢く
     6:豚轢く
     7:傾き
     8:レジ行き
     9:袋説明
     10:警備員
     11:敵説明
     12:カートジャック
     13:ダブルカート
     14:闘牛説明
     15:まとめ
     16:シーン移動
         */

    private int tutorialIndex = 0;
    private MTPlayer player;
    private TutorialShopping shopping;
    public GameObject pigPrefab;
    public GameObject hagePrefab;
    public GameObject cartRigit;
    public GameObject bbaPrefab;
    public GameObject securityPrefab;
    public GameObject[] points;
    public GameObject reji;
    public Fade fade;
    private bool isFadeNow = false;

    private Vector3 startPoint;
    private GameObject p;
    private TutorialCamera tcamera;

    private bool rpush = false;
    private bool lpush = false;
    
    private GameObject bba;
    private GameObject security;
    private GameObject kagoirihage;

    public GameObject scoreG;
    public GameObject timerG;
    public GameObject mapG;

    private float time;

    // Use this for initialization
    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<MTPlayer>();
        shopping = p.GetComponent<TutorialShopping>();
        tcamera = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<TutorialCamera>();
        startPoint = p.transform.position;
        reji.SetActive(false);
        //scoreG.SetActive(false);
        timerG.SetActive(false);
        mapG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeNow)
        {
            if (fade.IsFadeEnd())
            {
                switch (tutorialIndex)//フェード中にやりたいこと
                {
                    case 3: Index3Start(); break;
                    case 4: Index4Start(); break;
                    case 5: Index5Start(); break;
                    case 6: Index6Start(); break;
                    case 7: Index7Start(); break;
                    case 8: Index8Start(); break;
                    case 10: Index10Start(); break;
                    case 12: Index12Start(); break;
                    case 13: Index13Start(); break;
                    case 15: Index15Start(); break;
                    case 16: Index16Start(); break;
                    case 77: shopping.BasketActive(true); player.CatchCart(); Reset10();  break;
                }
                p.transform.position = startPoint;
                p.transform.eulerAngles = Vector3.zero;
                tcamera.CameraReset();
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
                case 14: Index14Update(); break;
                case 15: Index15Update(); break;
            }

        }
    }

    private void Index0Update()
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
        if (Input.anyKeyDown)
        {
            IndexNext();
        }

    }

    private void Index1Update()
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
        if (Input.anyKeyDown)
        {
            IndexNext();
        }
    }

    private void Index2Update()
    {
        //ミニマップ
        time += Time.deltaTime;
        if ((int)time % 2 == 0)
        {
            mapG.SetActive(true);
        }
        else
        {
            mapG.SetActive(false);
        }
        if (Input.anyKeyDown)
        {
            IndexNext();
        }
    }

    private void Index3Update()
    {
        //カート持つ
        if (player.IsCart())
        {
            IndexNext();
        }
    }

    private void Index4Update()
    {
        //特定の場所まで移動(滑走可)
        if (Vector3.Distance(p.transform.position, points[5].transform.position) < 2.0f)
        {
            IndexNext();
        }

    }

    private void Index5Update()
    {
        //人二人引く
        if (shopping.GetAllCount() == 2)
        {
            IndexNext();
        }
    }

    private void Index6Update()
    {
        //豚引く
        if (shopping.GetAllCount() == 5)
        {
            IndexNext();
        }

    }

    private void Index7Update()
    {
        //傾き
        if (lpush && rpush)
        {
            IndexNext();
        }
    }

    private void Index8Update()
    {
        //全部捕まえて袋
        if (shopping.IsPlasticBag())
        {
            IndexNext();
        }

    }

    private void Index9Update()
    {
        //袋説明、ボタン押されたら次
        if (Input.anyKeyDown)
        {
            IndexNext();
        }
    }

    private void Index10Update()
    {
        //特定の場所まで移動(警備員説明)
        if (Vector3.Distance(p.transform.position, points[5].transform.position) < 2.0f)
        {
            IndexNext();
        }
    }

    private void Index11Update()
    {
        //敵説明、ボタン押されたら次
        if (Input.anyKeyDown)
        {
            IndexNext();
        }
    }

    private void Index12Update()
    {

        //カートジャック終了
        if (player.IsCart())
        {
            IndexNext();
        }
    }

    private void Index13Update()
    {
        //ダブルカート
        if (player.IsCart2())
        {
            IndexNext();
        }
    }


    private void Index14Update()
    {
        //闘牛説明、ボタン押されたら次
        if (Input.anyKeyDown)
        {
            IndexNext();
        }
    }

    private void Index15Update()
    {

        if (shopping.GetAllCount() >= 5)
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
        switch (tutorialIndex) //フェードせずにやりたいこと
        {
            case 1: Index1Start(); break;
            case 2: Index2Start(); break;
            case 9: Index9Start(); break;
            case 11: Index11Start(); break;
            case 14: Index14Start(); break;
        }
        if (tutorialIndex == 1 || tutorialIndex == 2
            || tutorialIndex == 9 || tutorialIndex == 11
            || tutorialIndex == 14) return;
        fade.FadeOut(1.0f);
        isFadeNow = true;
    }

    public void Index10Reset()
    {
        tutorialIndex = 77;
        fade.FadeOut(1.0f);
        isFadeNow = true;

    }

    private void Index1Start()
    {
        scoreG.SetActive(true);
        //目指す場所にエフェクト配置
    }

    private void Index2Start()
    {

        timerG.SetActive(true);

    }

    private void Index3Start()
    {

        mapG.SetActive(true);

    }

    private void Index4Start()
    {
        //目指す場所にエフェクト配置
    }

    private void Index5Start()
    {
        GameObject hage1 = Instantiate(hagePrefab);
        GameObject hage2 = Instantiate(hagePrefab);
        hage1.transform.position = points[0].transform.position;
        hage2.transform.position = points[2].transform.position;
    }

    private void Index6Start()
    {
        GameObject pig1 = Instantiate(pigPrefab);
        GameObject pig2 = Instantiate(pigPrefab);
        GameObject pig3 = Instantiate(pigPrefab);

        pig1.transform.position = points[0].transform.position;
        pig2.transform.position = points[2].transform.position;
        pig3.transform.position = points[3].transform.position;
    }

    private void Index7Start()
    {

    }

    private void Index8Start()
    {
        reji.SetActive(true);
    }

    private void Index9Start()
    {
        reji.SetActive(false);


    }

    private void Index10Start()
    {
        security = Instantiate(securityPrefab);

        kagoirihage = Instantiate(hagePrefab);
        shopping.DeleteBaggege();
        Reset10();
    }

    private void Index11Start()
    {
        if(security.transform.parent == null)Destroy(security);
        //敵説明
    }

    private void Index12Start()
    {
        
        bba = Instantiate(bbaPrefab);

        bba.transform.position = points[1].transform.position;
        shopping.DeleteBaggege();
        player.BreakCart();
    }

    private void Index13Start()
    {

        Destroy(bba);
        //ダブルカート用カート出現
        GameObject cart = Instantiate(cartRigit);

        cart.transform.position = points[1].transform.position;
        
    }

    private void Index14Start()
    {
        //闘牛の説明のui
    }

    private void Index15Start()
    {
        //説明uiの削除
        //まとめ
        GameObject hage11 = Instantiate(hagePrefab);
        GameObject hage12 = Instantiate(hagePrefab);
        GameObject hage13 = Instantiate(hagePrefab);
        GameObject pig11 = Instantiate(pigPrefab);
        GameObject pig12 = Instantiate(pigPrefab);

        hage11.transform.position = points[0].transform.position;
        hage12.transform.position = points[1].transform.position;
        hage13.transform.position = points[2].transform.position;
        pig11.transform.position = points[4].transform.position;
        pig12.transform.position = points[5].transform.position;
    }


    private void Index16Start()
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

    private void Reset10()
    {
        security.transform.position = points[3].transform.position;
        kagoirihage.GetComponent<Animator>().SetTrigger("Kago");
        kagoirihage.GetComponent<NavMeshAgent>().enabled = false;
        kagoirihage.GetComponent<Collider>().enabled = false;
        shopping.AddBaggege(kagoirihage.transform);
        player.GetComponent<Animator>().Play("OnCart");
        tutorialIndex = 10;
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
