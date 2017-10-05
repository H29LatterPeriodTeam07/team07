using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStatusWithPlayer : MonoBehaviour {
    
    private Player playerScript;

    private float[] cartStatus;

	// Use this for initialization
	void Start () {
        playerScript = GetComponent<Player>();

        cartStatus = new float[4];
    }
	
	// Update is called once per frame
	void Update () {
		/*ここに部位の耐久値が０以下になったときに処理を書く
         
         */
	}

    /// <summary>
    /// カートを持った時にカートのデータをもらう
    /// </summary>
    /// <param name="cart"></param>
    public void GetCart(CartStatusWithCart cart)
    {
        cartStatus = cart.PassStatus();
    }

    /// <summary>
    /// カートを離した時にカートのデータを渡す
    /// </summary>
    /// <param name="cart"></param>
    public void SetCart(CartStatusWithCart cart)
    {
        cart.SetStatus(cartStatus);
    }

    public float HPNow()
    {
        return cartStatus[0];
    }
    
    public void DamageCart(float dm)
    {
        cartStatus[0] -= dm;
        /*ここでランダム部位にダメージを与える
         
         
         
         */

        if(cartStatus[0] <= 0)
        {
            playerScript.BreakCart();
            GetComponent<ShoppingCount>().BaggegeFall();
        }
    }

}
