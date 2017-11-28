﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartRelatedData {

    public static float cartLocalPosZ = 1.35f;   //プレイヤーに持たれているカートのz座標

    public static float cartInBagLocalPosY = 0.5f;  //カゴがカートに入っているときのy座標
    public static float cartInBagLocalPosZ = 1.25f;  //カゴがカートに入っているときのz座標
    public static Vector3 cartInBagLocalPos = new Vector3(0, cartInBagLocalPosY, cartInBagLocalPosZ); //カゴがカートに入っているときの座標

    public static float cartOutBagLocalPosX = -0.09f;  //カゴがカートに入っていないのx座標
    public static float cartOutBagLocalPosY = 1.4f;  //カゴがカートに入っていないのy座標
    public static float cartOutBagLocalPosZ = 0.65f;  //カゴがカートに入っていないのz座標
    public static Vector3 cartOutBagLocalPos = new Vector3(cartOutBagLocalPosX, cartOutBagLocalPosY, cartOutBagLocalPosZ);  //カゴがカートに入っていないの座標

    public static float cartFlyStartPosY = 1.6f;  //カゴを投げ始めるときのy座標
    public static float cartNavPoint = -1.5f;  //カートをジャックするときの目標のz座標

}
