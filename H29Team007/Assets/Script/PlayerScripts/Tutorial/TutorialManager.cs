using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    private int tutorialIndex = 0;
    private TutorialPlayer player;
    private TutorialShopping shopping;
    public GameObject pigPrefab;
    public GameObject hagePrefab;
    public GameObject cartRigit;
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

    // Use this for initialization
    void Start () {
        p = GameObject.FindGameObjectWithTag("Player");
        player = p.GetComponent<TutorialPlayer>();
        shopping = p.GetComponent<TutorialShopping>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<TutorialCamera>();
        startPoint = p.transform.position;
        reji.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (isFadeNow)
        {
            if (fade.IsFadeEnd())
            {
                switch (tutorialIndex)
                {
                    case 3: Index3Start(); break;
                    case 4: Index4Start(); break;
                    case 6: Index6Start(); break;
                    case 8: Index8Start(); break;
                    case 9: Index9Start(); break;
                    case 10: Index10Start(); break;
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
            }

        }
    }

    private void Index0Update()
    {
        if (player.IsCart())
        {
            IndexNext();
        }
    }

    private void Index1Update()
    {
        if(Vector3.Distance(p.transform.position,points[5].transform.position) < 0.5f)
        {
            IndexNext();
        }
    }

    private void Index2Update()
    {
        if (player.GetState() == TutorialPlayer.PlayerState.Gliding)
        {
            IndexNext();
        }
    }

    private void Index3Update()
    {
        if (shopping.GetAllCount() == 2)
        {
            IndexNext();
        }
    }

    private void Index4Update()
    {

        if (shopping.GetAllCount() == 3)
        {
            IndexNext();
        }
    }

    private void Index5Update()
    {
        if (lpush && rpush)
        {
            IndexNext();
        }

    }

    private void Index6Update()
    {
        if (player.GetState() == TutorialPlayer.PlayerState.Takeover)
        {
            IndexNext();
        }

    }

    private void Index7Update()
    {
        if (player.IsCart())
        {
            IndexNext();
        }
    }

    private void Index8Update()
    {
        if (player.IsCart2())
        {
            IndexNext();
        }
    }

    private void Index9Update()
    {
        if (shopping.GetAllCount() >= 6)
        {
            reji.SetActive(true);
        }
        else
        {
            reji.SetActive(false);
        }

        if (shopping.IsPlasticBag())
        {
            IndexNext();
        }
    }

    private void IndexNext()
    {
        tutorialIndex++;
        if (tutorialIndex == 7) return;
        fade.FadeOut(1.0f);
        isFadeNow = true;
    }

    private void Index3Start()
    {
        hage1 = Instantiate(hagePrefab);
        hage2 = Instantiate(hagePrefab);
        hage1.transform.position = points[0].transform.position;
        hage2.transform.position = points[2].transform.position;
    }

    private void Index4Start()
    {
        pig1 = Instantiate(pigPrefab);

        pig1.transform.position = points[0].transform.position;
    }

    private void Index6Start()
    {
        player.BreakCart();
        GameObject cart = Instantiate(cartRigit);

        cart.transform.position = points[1].transform.position;
    }

    private void Index8Start()
    {
        shopping.BaggegeFall(p.transform.position);
        Destroy(hage1);
        Destroy(hage2);
        Destroy(pig1);
        GameObject cart = Instantiate(cartRigit);

        cart.transform.position = points[1].transform.position;
    }

    private void Index9Start()
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

    private void Index10Start()
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
}
