using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GamepadInput;

public class TableController : MonoBehaviour {

    //----------------------------------------------------
    // private
    //----------------------------------------------------

    GameObject childObj; //現在持っている子オブジェクト
    PlayerSetting playerSetting;
    GameController gameController;

    bool cookingStartFlg = false; //焼き始めを指定するフラグ

    float waitCookingTime = 1.0f; //調理にかかる時間
    float frame;

    //----------------------------------------------------
    //　子オブジェクトがあるかどうかを調べる
    //  あったらtrue,なかったらfalseを返す
    //----------------------------------------------------
    bool HaveChildObj(GameObject obj)
    {
        if (obj.transform.childCount < 1) return false;
        else return true;
    }
    
    void Start ()
    {
        gameController = new GameController();
        playerSetting = GetComponent<PlayerSetting>();

        playerSetting.cookingBowl.sprite = playerSetting.bowlSp[0];
        playerSetting.cookingOven.sprite = playerSetting.ovenSp[0];
	}
	
	void Update ()
    {
        if (cookingStartFlg)
        {
            GameController.Instance.WaitCookingTime(waitCookingTime);
        }

        HaveCookieManager();
        //StartCoroutine(FloorPut());
    }

    //----------------------------------------------------
    // 対応した仮想入力キーが押されていたらtureを返す
    //----------------------------------------------------
    string OnButtonManager()
    {
        if (playerSetting.playerNum == 1 && playerSetting.playIsGamePad) return "JoyStick_1_Action1";
        else if (playerSetting.playerNum == 2 && playerSetting.playIsGamePad) return "JoyStick_2_Action1";
        else if (playerSetting.playerNum == 1 && playerSetting.playIsGamePad == false) return "Fire2";
        else return "Fire1";
    }

    //----------------------------------------------------
    // 机などとの衝突判定処理
    //----------------------------------------------------
    void OnCollisionStay2D(Collision2D col)
    {
        //テーブル以外のオブジェクトと衝突したら終了
        if (col.gameObject.tag != "Table" &&
            col.gameObject.tag != "ElemTable" &&
            col.gameObject.tag != "KneadTable" &&
            col.gameObject.tag != "BakingTable" &&
            col.gameObject.tag != "ExitTable") return;

        if(Input.GetButtonDown(OnButtonManager()))
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
    void OnTriggerStay2D(Collider2D col)
    {
        if (transform.childCount >= 1) return;

        if (col.tag != "CookieElem" &&
            col.tag != "CookieKnead" &&
            col.tag != "CookieBaking") return;

        if (Input.GetButtonDown(OnButtonManager()))
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
        if (transform.childCount >= 1) childObj = transform.GetChild(0).gameObject;
        //子オブジェクトを持っていなかったら初期化
        else if (transform.childCount < 0) childObj = null;
    }

    //----------------------------------------------------
    // ElemTableからオブジェクトを生成する処理
    //----------------------------------------------------
    void ElemTablerOut(GameObject colObj_elem)
    {
        // プレイヤーに子オブジェクトがあったら終了
        if (HaveChildObj(gameObject)) return;

        // 衝突した机がElemTable
        if (colObj_elem.gameObject.tag == "ElemTable")
        {
            //クッキーの素を生成し、プレイヤーの子オブジェクトにする
            Instantiate(playerSetting.elemPre, transform);
        }
    }

    //----------------------------------------------------
    // KneadTableにクッキーの素を置いた時の処理
    //----------------------------------------------------
    void KneadTablePut(GameObject col_KneadPut)
    {
        //プレイヤーが子オブジェクトを持っていなければ終了
        if (HaveChildObj(gameObject) == false) return;

        //try/catch ブロック
        try
        {
            // 衝突した机がKneadTableであり、クッキーの素を持っていたら
            if (col_KneadPut.gameObject.tag == "KneadTable" &&
                childObj.tag == "CookieElem" &&
                GameController.kneadCookFlg == false)
            {
                playerSetting.cookingBowl.sprite = playerSetting.bowlSp[1];
                Destroy(childObj);
                GameController.kneadNecessary += 1;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // BakingTableにクッキーの素を置いた時の処理
    //----------------------------------------------------
    void BakingTablePut(GameObject col_BakingPut)
    {
        //プレイヤーが子オブジェクトを持っていなければ終了
        if (HaveChildObj(gameObject) == false) return;

        try
        {
            //衝突した机がBakingTableであり、こねたクッキーを持っていたら
            if (col_BakingPut.gameObject.tag == "BakingTable" &&
                childObj.tag == "CookieKnead" &&
                GameController.bakingCookFlg == false)
            {
                playerSetting.cookingOven.sprite = playerSetting.ovenSp[1];

                Destroy(childObj);
                GameController.bakingCookFlg = true;

                waitCookingTime = 2.0f;
                cookingStartFlg = true;
            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // 通常のTableに物を置いた時の処理
    //----------------------------------------------------
    void TablePut(GameObject col_TablePut)
    {
        //子オブジェクトを持っていたら終了
        if (HaveChildObj(col_TablePut)) return;

        try
        {
            //衝突した机がTableであったら
            if (col_TablePut.gameObject.tag == "Table" ||
                col_TablePut.gameObject.tag == "ExitTable" &&
                childObj.tag == "CookieBaking" &&
                childObj != null)
            {
                //持っているオブジェクトを机の真ん中に置く
                childObj.transform.position = col_TablePut.transform.position;

                //置いたオブジェクトを机の子オブジェクトにする
                childObj.transform.parent = col_TablePut.transform;

                //クッキーの描画順を変える
                SpriteRenderer cookieObjRenderer = childObj.GetComponent<SpriteRenderer>();
                cookieObjRenderer.sortingOrder = 1;

                //プレイヤーとクッキーの親子関係を解除する
                transform.DetachChildren();
            }
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // クッキーの素を捏ね終わるまでの時間、捏ね終わった後の処理
    //----------------------------------------------------
    void ElemCookEnd(GameObject col_KneadOut)
    {
        //子オブジェクトを持っている　または
        //素が入れられてない場合は終了
        if (HaveChildObj(gameObject) ||
            GameController.kneadCookFlg == false) return;

        if (col_KneadOut.gameObject.tag == "KneadTable")
        {
            Instantiate(playerSetting.kneadPre, transform);
            playerSetting.cookingBowl.sprite = playerSetting.bowlSp[0];

            //ボウルに何も入っていない状態にする
            GameController.kneadCookFlg = false;
        }
    }

    //----------------------------------------------------
    // クッキーを焼く時間、焼いた後の処理
    //----------------------------------------------------
    void BakingCookEnd(GameObject col_CookieOut)
    {
        if (HaveChildObj(gameObject) ||
            GameController.bakingCookFlg == false) return;

        if (col_CookieOut.gameObject.tag == "BakingTable" && GameController.cookingTimeFlg)
        {
            Instantiate(playerSetting.bakingPre, transform);

            cookingStartFlg = false;
            GameController.cookingTimeFlg = false;
            GameController.bakingCookFlg = false;

            playerSetting.cookingOven.sprite = playerSetting.ovenSp[0];
        }
    }

    //void TableCookieOut(GameObject col)
    //{
    //    //テーブルになにもおいていない、または
    //    if (HaveChildObj(col.gameObject) == false ||
    //        HaveChildObj(gameObject)) return;
    //}

    //----------------------------------------------------
    // クッキー☆を床に置くときの処理
    //----------------------------------------------------
    //IEnumerator FloorPut()
    //{
    //    // クッキーを持っていなかったら終了
    //    if (HaveChildObj(gameObject) == false) yield break;

    //    frame += Time.deltaTime;
    //    yield return new WaitUntil(() => frame > 0.5f);

    //    //// オブジェクトを持った時にすぐに離してしまうのを防止
    //    //yield return new WaitForSeconds(0.5f);

    //    if (Input.GetButtonUp(OnButtonManager()))
    //    {
    //        // 親子関係を解除
    //        transform.DetachChildren();
    //        // 子オブジェクトを初期化
    //        childObj = null;
    //        frame = 0;
    //    }
    //}
}