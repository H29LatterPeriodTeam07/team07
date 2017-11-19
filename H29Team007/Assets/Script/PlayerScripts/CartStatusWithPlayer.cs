using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStatusWithPlayer : MonoBehaviour
{

    private Player playerScript;
    private ShoppingCount scScript;

    private float[] cartStatus;


    [SerializeField, Header("カートのタイヤが壊れた時に毎フレーム下げる本体の耐久度")]
    private float minusCartHP = 0.01f;
    [SerializeField, Header("カートのハンドルが壊れた時に下げる回転速度")]
    private float minusRotateSpeedDefault = 60;

    private PlayerSE seScript;

    // Use this for initialization
    void Start()
    {
        playerScript = GetComponent<Player>();
        scScript = GetComponent<ShoppingCount>();

        cartStatus = new float[4];

        seScript = GetComponent<PlayerSE>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScript.IsCart()) return;
        /*ここに部位の耐久値が０以下になったときの処理を書く*/
         if(cartStatus[1] <= 0)
        {
            cartStatus[0] -= minusCartHP;
        }

    }

    /// <summary>
    /// カートを持った時にカートのデータをもらう
    /// </summary>
    /// <param name="cart"></param>
    public void GetCart(CartStatusWithCart cart)
    {
        cartStatus = cart.PassStatus();
        if (cartStatus[3] <= 0)
        {
            playerScript.SetMinusRotateSpeed(minusRotateSpeedDefault);
        }
    }

    /// <summary>
    /// カートを離した時にカートのデータを渡す
    /// </summary>
    /// <param name="cart"></param>
    public void SetCart(CartStatusWithCart cart)
    {
        cart.SetStatus(cartStatus);
        playerScript.SetMinusRotateSpeed(0);
    }

    public float HPNow()
    {
        return cartStatus[0];
    }

    public void DamageCart(float dm)
    {
        float dame = dm;
        if (cartStatus[2] <= 0)dame = dame * 2;
        cartStatus[0] -= dame;
        /*ここでランダム部位にダメージを与える*/
        int rand = Random.Range(1, 4);
        
        switch (rand)
        {
            case 1:cartStatus[1] -= dame; break;
            case 2:cartStatus[2] -= dame; break;
            case 3:cartStatus[3] -= dame; break;
        }




        if (cartStatus[3] <= 0)
        {
            playerScript.SetMinusRotateSpeed(minusRotateSpeedDefault);
        }
        if (cartStatus[0] <= 0)
        {
            playerScript.BreakCart();
            scScript.BaggegeFall(transform.position);
            playerScript.SetMinusRotateSpeed(0);
            seScript.OnePlay(3);
        }
        else
        {
            seScript.OnePlay(2);
        }
    }

    public void SetBasketParent(Transform parent)
    {
        scScript.SetBasketParent(parent);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Register")
        {
            scScript.PassTheRegister();
        }
    }
}
