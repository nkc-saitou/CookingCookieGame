using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GamepadInput;

public class TableController : MonoBehaviour
{
    //----------------------------------------------------
    // private
    //----------------------------------------------------

    GameObject childObj; //現在持っている子オブジェクト

    PlayerSetting playerSetting;
    GameController gameController;
    TableManager tableManager;

    Vector3 childPos; //子オブジェクトの位置調整

    bool cookingStartFlg = false; //焼き始めを指定するフラグ

    float waitCookingTime = 1.0f; //調理にかかる時間

    int childCount = 0;

    //string[] cookieType = new string[3];

    ////クッキーの判別　{0(elem),0(jum),0(chocolate)} 
    //int[] cookieType = new int[3];

    //----------------------------------------------------
    //　子オブジェクトがあるかどうかを調べる
    //  あったらtrue, なかったらfalseを返す
    //----------------------------------------------------
    bool HaveChildObj(GameObject obj)
    {
        if (obj.transform.childCount < childCount + 1) return false;
        else return true;
    }

    void Start()
    {
        gameController = new GameController();

        playerSetting = GetComponent<PlayerSetting>();
        tableManager = transform.parent.GetComponent<TableManager>();

        childCount = transform.childCount;
    }

    void Update()
    {
        if (cookingStartFlg) GameController.Instance.WaitCookingTime(waitCookingTime);

        HaveCookieManager();
        ChildObjPosCorrection();

        FloorPut();
    }

    //----------------------------------------------------
    // 子オブジェクトの位置がずれていってしまうのを修正
    //----------------------------------------------------
    void ChildObjPosCorrection()
    {
        if (childObj != null)
        {
            childPos = gameObject.transform.position;
            childPos.y = gameObject.transform.position.y + 1;
            childObj.transform.position = childPos;
        }
    }

    //----------------------------------------------------
    // 机などとの衝突判定処理
    //----------------------------------------------------
    void OnCollisionStay(Collision col)
    {
        //テーブル以外のオブジェクトと衝突したら終了
        if (col.gameObject.tag != "Table" &&
            col.gameObject.tag != "ElemTable" &&
            col.gameObject.tag != "KneadTable" &&
            col.gameObject.tag != "BakingTable" &&
            col.gameObject.tag != "ExitTable" &&
            col.gameObject.tag != "ChocolateTable" &&
            col.gameObject.tag != "JamTable") return;

        if (Input.GetButtonDown(playerSetting.keyAction))
        {
            //クッキーの素を出す
            ElemTablerOut(col.gameObject);

            //机にクッキーを置く
            KneadTablePut(col.gameObject);
            BakingTablePut(col.gameObject);
            TablePut(col.gameObject);

            //作り終えたクッキーを取る
            ElemCookEnd(col.gameObject);
            BakingCookEnd(col.gameObject);
        }
    }

    //----------------------------------------------------
    // クッキー☆との衝突判定処理
    //----------------------------------------------------
    void OnTriggerStay(Collider col)
    {
        if (transform.childCount >= childCount + 1) return;

        if (col.tag != "CookieElem" &&
            col.tag != "CookieJam" &&
            col.tag != "CookieChocolate" &&
            col.tag != "CookieKnead" &&
            col.tag != "CookieBaking") return;

        if (Input.GetButtonDown(playerSetting.keyAction))
        {
            col.transform.parent = transform;
        }
    }

    //----------------------------------------------------
    // 持っている子オブジェクトの管理と初期化
    //----------------------------------------------------
    void HaveCookieManager()
    {
        //子オブジェクトの特定
        if (transform.childCount >= childCount + 1)
        {
            childObj = transform.GetChild(childCount).gameObject;
        }
        //子オブジェクトを持っていなかったら初期化
        else if (transform.childCount < childCount)
        {
            childObj = null;
        }
    }

    //----------------------------------------------------
    // ElemTableからオブジェクトを生成する処理
    //----------------------------------------------------
    void ElemTablerOut(GameObject colObj)
    {
        //プレイヤーに子オブジェクトがあったら終了
        if (HaveChildObj(gameObject)) return;

        switch (colObj.gameObject.tag)
        {
            case "ElemTable":
                //クッキーの素を生成し、プレイヤーの子オブジェクトにする
                Instantiate(tableManager.elemPre, transform);
                break;

            case "ChocolateTable":
                Instantiate(tableManager.chocolatePre, transform);
                break;

            case "JamTable":
                Instantiate(tableManager.jamPre, transform);
                break;

                //case "KneadTable":
                //    Instantiate(tableManager.kneadPre, transform);
                //    break;
        }
    }

    //----------------------------------------------------
    // KneadTableにクッキーの素を置いた時の処理
    //----------------------------------------------------
    void KneadTablePut(GameObject colObj)
    {
        //プレイヤーが子オブジェクトを持っていなければ終了
        if (HaveChildObj(gameObject) == false) return;

        //try/catch ブロック
        try
        {
            //衝突した机がKneadTableであり、クッキーの素を持っていたら
            if (childObj == null ||
                colObj.gameObject.tag != "KneadTable" ||
                GameController.kneadCookFlg != false) return;

            if (childObj.tag == "CookieElem" ||
               childObj.tag == "CookieJam" ||
               childObj.tag == "CookieChocolate")
            {
                ElemCookieCheck();
                GameController.kneadNecessary += 1;
                Destroy(childObj);
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // どの種類の素材が入れられたかをチェック
    //----------------------------------------------------
    void ElemCookieCheck()
    {
        switch(childObj.tag)
        {
            case "CookieElem":
                GameController.cookieType[0] += 1;
                break;

            case "CookieJam":
                GameController.cookieType[1] += 1;
                break;

            case "CookieChocolate":
                GameController.cookieType[2] += 1;
                break;
        }
    }

    //----------------------------------------------------
    // BakingTableにクッキーの素を置いた時の処理
    //----------------------------------------------------
    void BakingTablePut(GameObject colObj)
    {
        //プレイヤーが子オブジェクトを持っていなければ終了
        if (HaveChildObj(gameObject) == false) return;

        try
        {
            //衝突した机がBakingTableであり、こねたクッキーを持っていたら
            if (childObj != null &&
                colObj.gameObject.tag == "BakingTable" &&
                childObj.tag == "CookieKnead" &&
                GameController.bakingCookFlg == false)
            {
                GameController.bakingCookFlg = true;

                waitCookingTime = 2.0f;
                cookingStartFlg = true;

                Destroy(childObj);
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // 通常のTableに物を置いた時の処理
    //----------------------------------------------------
    void TablePut(GameObject colObj)
    {
        //子オブジェクトを持っていたら終了
        if (HaveChildObj(colObj)) return;

        try
        {
            //衝突した机がTableであったら
            if (colObj.gameObject.tag == "Table" ||
            colObj.gameObject.tag == "ExitTable" &&
            childObj.tag == "CookieBaking")
            {
                //持っているオブジェクトを机の真ん中に置く
                childObj.transform.position = colObj.transform.position;

                //置いたオブジェクトを机の子オブジェクトにする
                childObj.transform.parent = colObj.transform;

                ////プレイヤーとクッキーの親子関係を解除する
                //childObj.transform.parent = null;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // クッキーの素を捏ね終わった後の処理
    //----------------------------------------------------
    void ElemCookEnd(GameObject colObj)
    {
        //子オブジェクトを持っている または
        //素が入れられてない場合は終了
        if (HaveChildObj(gameObject) ||
            GameController.kneadCookFlg == false) return;

        if (colObj.gameObject.tag == "KneadTable")
        {
            Instantiate(tableManager.kneadPre, transform);

            //ボウルに何も入っていない状態にする
            GameController.kneadCookFlg = false;
        }
    }

    //----------------------------------------------------
    //クッキーを焼く時間、焼いた後の処理
    //----------------------------------------------------
    void BakingCookEnd(GameObject colObj)
    {
        if (HaveChildObj(gameObject) ||
            GameController.bakingCookFlg == false) return;

        GameObject bakingCookieType = GameController.Instance.KneadCreateType();

        if (colObj.gameObject.tag == "BakingTable" && GameController.cookingTimeFlg)
        {
            Instantiate(bakingCookieType, transform);

            cookingStartFlg = false;
            GameController.cookingTimeFlg = false;
            GameController.bakingCookFlg = false;
        }
    }

    //----------------------------------------------------
    //クッキー☆を床に置くときの処理
    //----------------------------------------------------
    void FloorPut()
    {
        // クッキーを持っていなかったら終了
        if (HaveChildObj(gameObject) == false) return;

        if (Input.GetButtonUp(playerSetting.keyAction_2) && childObj != null)
        {
            childObj.transform.position = gameObject.transform.position;

            // 親子関係を解除
            childObj.transform.parent = null;

            // 子オブジェクトを初期化
            childObj = null;
        }
    }
}