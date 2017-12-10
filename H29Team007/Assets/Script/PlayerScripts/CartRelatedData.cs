using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartRelatedData {

    public static float cartLocalPosZ = 1.35f;   //プレイヤーに持たれているカートのz座標

    public static float cartInBagLocalPosY = 0.5f;  //カゴがカートに入っているときのy座標
    public static float cartInBagLocalPosZ = 1.25f;  //カゴがカートに入っているときのz座標
    public static Vector3 cartInBagLocalPos = new Vector3(0, cartInBagLocalPosY, cartInBagLocalPosZ); //カゴがカートに入っているときの座標

    public static float cartOutBagLocalPosX = -0.09f;  //カゴがカートに入っていないときのx座標
    public static float cartOutBagLocalPosY = 0.85f;  //カゴがカートに入っていないときのy座標
    public static float cartOutBagLocalPosZ = 0.45f;  //カゴがカートに入っていないときのz座標
    public static Vector3 cartOutBagLocalPos = new Vector3(cartOutBagLocalPosX, cartOutBagLocalPosY, cartOutBagLocalPosZ);  //カゴがカートに入っていないときの座標

    public static float cartFlyStartPosY = 1.6f;  //カゴを投げ始めるときのy座標
    public static float cartNavPoint = -1.5f;  //カートをジャックするときの目標のz座標

    public static Vector3 cartRotatePointBack = new Vector3(0, 0, 0.4f); //カートを手前に傾けるときの回す軸の位置
    public static Vector3 cartRotatePointFront = new Vector3(0, 0, 1.8f); //カートを奥に傾けるときの回す軸の位置


    /*調整するときのメモ置き場 
   
    y1.4f z0.65f

     
    */

}
