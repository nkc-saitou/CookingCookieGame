using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GamepadInput;

public class TableController : MonoBehaviour {

    //----------------------------------------------------
    // public
    //----------------------------------------------------

    //[Header("クッキーの素")]
    //public GameObject elemPre;

    //[Header("こねたクッキー")]
    //public GameObject kneadPre;

    //[Header("焼いたクッキー")]
    //public GameObject bakingPre;

    ////ボウルのスプライトを変更
    //public SpriteRenderer cookingBowl;
    //public Sprite[] bowlSp;

    
    //----------------------------------------------------
    // private
    //----------------------------------------------------

    GameObject childObj; //現在持っている子オブジェクト
    PlayerSetting playerSetting;

    bool elemCookFlg = false;
    bool kneadCookFlg = false;

    bool cookingTimeFlg = false;

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
        playerSetting = GetComponent<PlayerSetting>();
        playerSetting.cookingBowl.sprite = playerSetting.bowlSp[0];
	}
	
	void Update ()
    {
        HaveCookieManager();
        StartCoroutine(FloorPut());
    }

    bool OnButtonManager()
    {
        if (playerSetting.playerNum == 1 && playerSetting.playIsGamePad) return Input.GetButtonDown("JoyStick_1_Action1");
        else if (playerSetting.playerNum == 2 && playerSetting.playIsGamePad) return Input.GetButtonDown("JoyStick_2_Action1");
        else if (playerSetting.playerNum == 1 && playerSetting.playIsGamePad == false) return Input.GetButtonDown("Fire2");
        else return Input.GetButtonDown("Fire1");
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

        if(OnButtonManager())
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

        if(OnButtonManager())
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
        if (HaveChildObj(gameObject) == false && childObj == null) return;

        //try/catch ブロック　で
        try
        {
            // 衝突した机がKneadTableであり、クッキーの素を持っていたら
            if (col_KneadPut.gameObject.tag == "KneadTable" &&
                childObj.tag == "CookieElem")
            {
                playerSetting.cookingBowl.sprite = playerSetting.bowlSp[1];
                Destroy(childObj);
                elemCookFlg = true;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Object reference not set to an instance of an object");
        }
    }

    //----------------------------------------------------
    // KneadTableにクッキーの素を置いた時の処理
    //----------------------------------------------------
    void BakingTablePut(GameObject col_BakingPut)
    {
        //プレイヤーが子オブジェクトを持っていなければ終了
        if (HaveChildObj(gameObject) == false) return;

        try
        {
            //衝突した机がBakingTableであり、こねたクッキーを持っていたら
            if (col_BakingPut.gameObject.tag == "BakingTable" &&
                childObj.tag == "CookieKnead")
            {
                Destroy(childObj);
                kneadCookFlg = true;
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
                childObj.tag == "CookieBaking")
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
    // クッキー☆を床に置くときの処理
    //----------------------------------------------------
    IEnumerator FloorPut()
    {
        // クッキーを持っていなかったら終了
        if (HaveChildObj(gameObject) == false) yield break;

        // オブジェクトを持った時にすぐに離してしまうのを防止
        yield return new WaitForSeconds(0.5f);

        if(OnButtonManager())
        {
            // 親子関係を解除
            transform.DetachChildren();
            // 子オブジェクトを初期化
            childObj = null;
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
            elemCookFlg == false) return;

        //クッキーの調理にかかる時間
        StartCoroutine(WaitCookingTime(1.0f));

        if (col_KneadOut.gameObject.tag == "KneadTable" && cookingTimeFlg)
        {
            Instantiate(playerSetting.kneadPre, transform);
            playerSetting.cookingBowl.sprite = playerSetting.bowlSp[0];

            //ボウルに何も入っていない状態にする
            elemCookFlg = false;

            cookingTimeFlg = false;
        }
    }

    //----------------------------------------------------
    // クッキーを焼く時間、焼いた後の処理
    //----------------------------------------------------
    void BakingCookEnd(GameObject col_CookieOut)
    {
        if (HaveChildObj(gameObject) ||
            kneadCookFlg == false) return;

        StartCoroutine(WaitCookingTime(1.0f));

        if(col_CookieOut.gameObject.tag == "BakingTable" && cookingTimeFlg)
        {
            Instantiate(playerSetting.bakingPre, transform);

            kneadCookFlg = false;

            cookingTimeFlg = false;
        }
    }

    //----------------------------------------------------
    // クッキーの調理時間
    //----------------------------------------------------
    IEnumerator WaitCookingTime(float time)
    {
        yield return new WaitForSeconds(time);

        cookingTimeFlg = true;
    }
}