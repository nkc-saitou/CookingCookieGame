using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : SingletonMonoBehaviour<GameController>
{
    //----------------------------------------------------
    // public
    //----------------------------------------------------

    public static bool kneadCookFlg = false;
    public static bool bakingCookFlg = false;
    public static bool cookingTimeFlg = false; //料理時間を満たしたかどうかをチェックする
    public static int kneadNecessary = 0; //粉クッキーを入れたかず

    //----------------------------------------------------
    // private
    //----------------------------------------------------
    GameObject childObj;

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
            Debug.Log(cookTimeCount);
        }
        else
        {
            cookTimeCount = 0;
            cookingTimeFlg = true;
            return;
        }
    }
}