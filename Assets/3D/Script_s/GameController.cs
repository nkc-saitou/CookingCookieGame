using System.Collections;
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

    //---------------------------------------------------
    // 作るクッキーの取得
    //---------------------------------------------------
    public string KneadCreateType()
    {
        if (cookieType[0] == 3) return "normalCookie";
        else if (cookieType[0] == 2 && cookieType[1] == 1) return "jamCookie";
        else if (cookieType[0] == 2 && cookieType[2] == 1) return "chocolateCookie";
        else return "darkMatter";
    }

    //---------------------------------------------------
    // クッキーの素材を初期化
    //---------------------------------------------------
    public void cookieTypeReset()
    {
        for(int i = 0; i < cookieType.Length; i++)
        {
            cookieType[i] = 0;
        }
    }

    public void CookieDateAdd(GameObject knead,string type)
    {
        CookieStatus status = knead.GetComponent<CookieStatus>();
        string createType = type;
        string path = " ";

        switch (createType)
        {
            case "normalCookie":
                path = "NormalCookieDate";
                break;

            case "jamCookie":
                path = "JamCookieDate";
                break;

            case "chocolateCookie":
                path = "ChocolateCookieDate";
                break;

            case "darkMatter":
                path = "DarkMatterDate";
                break;
        }
        status.cookieDate = (CookieDate)Resources.Load("ScriptableObject/" + path);
    }
}