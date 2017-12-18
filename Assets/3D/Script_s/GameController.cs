﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : SingletonMonoBehaviour<GameController>
{
    //----------------------------------------------------
    // public
    //----------------------------------------------------

    public TableManager tableManager;

    //----------------------------------------------------
    // static
    //----------------------------------------------------

    public static bool kneadCookFlg = false;
    public static bool bakingCookFlg = false;
    public static bool cookingTimeFlg = false; //料理時間を満たしたかどうかをチェックする
    public static int kneadNecessary = 0; //粉クッキーを入れたかず
    
    //クッキーの判別　{0(elem),0(jum),0(chocolate)} 
    public static int[] cookieType = new int[3];
    //----------------------------------------------------
    // private
    //----------------------------------------------------
    GameObject childObj;

    int childCount = 0;

    //public Text testText;
    //public Text flgText;

    float cookTimeCount = 0; //クッキーを調理する時間

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
    }

    void Start()
    {
       
    }

    void Update()
    {
        KneadTableControl();
        //testText.text = cookTimeCount.ToString();
        //flgText.text = cookingTimeFlg.ToString();
    }

    //---------------------------------------------------
    // こねるのに必要な数
    //---------------------------------------------------
    void KneadTableControl()
    {
        if (kneadNecessary >= 3)
        {
            kneadCookFlg = true;
            kneadNecessary = 0;
        }
    }

    //---------------------------------------------------
    // 料理時間
    //---------------------------------------------------
    public void WaitCookingTime(float time)
    {
        if (cookingTimeFlg == true) return;

        if (cookTimeCount <= time)
        {
            cookTimeCount += 1 * Time.deltaTime;
        }
        else
        {
            cookTimeCount = 0;
            cookingTimeFlg = true;
            return;
        }
    }

   public GameObject KneadCreateType()
    {
        if (cookieType[0] == 3) return tableManager.bakingPre_normal;
        else if (cookieType[0] == 2 && cookieType[1] == 1) return tableManager.bakingPre_jam;
        else if (cookieType[0] == 2 && cookieType[2] == 1) return tableManager.bakingPre_chocolate;
        return tableManager.bakingPre_darkMatter;
    }
}