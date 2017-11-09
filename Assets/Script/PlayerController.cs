using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //----------------------------------------------------
    // public
    //----------------------------------------------------
    [Header("クッキーの素")]
    public GameObject elemPre; //クッキーPrefab

    [Header("こねたクッキー")]
    public GameObject kneadPre;

    [Header("焼いたクッキー")]
    public GameObject cookiePre;

    public SpriteRenderer cookingBowl;
    public Sprite[] bowlSp;

    //----------------------------------------------------
    // private
    //----------------------------------------------------
    GameObject elemObj; //クッキーの素
    bool elemCookieChangeFlg = false; //クッキーの素がこねられていたらflgをtrueにする
    bool bakingFlg = false; //クッキーが焼ける状態だったらflgをtrueにする

    void Start()
    {
        cookingBowl.sprite = bowlSp[0];
    }

    void Update()
    {
        StartCoroutine(CookieManager());
        
        ChildCookieManager();

        StartCoroutine(ElemCookingTime());
        StartCoroutine(BakingTime());
    }

    //----------------------------------------------------
    // 机などとの衝突判定処理
    //----------------------------------------------------
    void OnCollisionStay2D(Collision2D col)
    {
        //あたったのが机オブジェクト以外だったら終了
        if (col.gameObject.tag != "Table" &&
            col.gameObject.tag != "ElemTable" &&
            col.gameObject.tag != "CookingTable" &&
            col.gameObject.tag != "BakingTable" &&
            col.gameObject.tag != "ExitTable") return;

        //右クリックされていたら
        if (!Input.GetButtonDown("Fire1")) return;

        //プレイヤーが何も持っていないときのみ、クッキーの素を出すことが出来る
        if (col.gameObject.tag == "ElemTable" && transform.childCount < 1)
        {
            Instantiate(elemPre, transform); //クッキーの素を出す
        }
        //机に何も物が置かれていなかったら
        else if (col.transform.childCount < 1 && transform.childCount >= 1)
        {
            CookiePut(col.transform.localPosition, col.transform); //クッキーを机に置く

            //置いた机が調理台だったら、こねてくれる
            if (col.gameObject.tag == "CookingTable")
            {
                cookingBowl.sprite = bowlSp[1];
                Destroy(elemObj);
                //捏ねた後のクッキーを作れるようにする
                elemCookieChangeFlg = true;
            }

            if(col.gameObject.tag == "BakingTable")
            {
                Destroy(elemObj);
                bakingFlg = true;
            }
        }
    }


    //----------------------------------------------------
    // クッキー☆との衝突判定処理
    //----------------------------------------------------
    void OnTriggerStay2D(Collider2D col)
    {
        //クッキーを持っていたら他のクッキーは持てない
        if (transform.childCount >= 1) return;
        if (col.tag != "Cookie") return;

        if (Input.GetButtonDown("Fire1"))
        {
            col.transform.parent = transform;
        }
    }
    //----------------------------------------------------
    // 持っているクッキーを管理する
    //----------------------------------------------------
    void ChildCookieManager()
    {
        //クッキーを持っていたときのみ
        if (transform.childCount >= 1) elemObj = transform.GetChild(0).gameObject;
        else if (transform.childCount < 0) elemObj = null;
    }

    //----------------------------------------------------
    // クッキー☆を机に置く処理
    //----------------------------------------------------
    void CookiePut(Vector2 tablePos,Transform childParent)
    {
        if (elemObj == null) return;

        //持っているオブジェクトを机の真ん中に置く
        elemObj.transform.position = tablePos;

        //置いたオブジェクトを机の子オブジェクトにする
        elemObj.transform.parent = childParent;

        //クッキー☆の描画順を変える
        SpriteRenderer cookieObjRenderer = elemObj.GetComponent<SpriteRenderer>();
        cookieObjRenderer.sortingOrder = 1;

        //プレイヤーとクッキーの親子関係を解除する
        transform.DetachChildren();
    }

    //----------------------------------------------------
    // クッキー☆を床に置くときの処理
    //----------------------------------------------------
    IEnumerator CookieManager()
    {
        //クッキーを持っていなかったら終了
        if (transform.childCount < 1) yield break;

        //持ったらすぐ離してしまうのを防止。持ったあと0.5秒は離すことが出来ないようにする
        yield return new WaitForSeconds(0.5f);

        //0.5秒後に親子関係を解除し、elemObjを初期化する
        if (Input.GetButtonDown("Fire1"))
        {
            transform.DetachChildren();
            elemObj = null;
        }
    }

    //----------------------------------------------------
    // クッキーが捏ね終わるまでの時間
    //----------------------------------------------------
    IEnumerator ElemCookingTime()
    {
        if (elemCookieChangeFlg == false) yield break;

        yield return new WaitForSeconds(1.0f);

        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(kneadPre, transform);
            cookingBowl.sprite = bowlSp[0];
            elemCookieChangeFlg = false;
        }
    }

    IEnumerator BakingTime()
    {
        if (bakingFlg == false) yield break;

        yield return new WaitForSeconds(1.0f);

        if(Input.GetButtonDown("Fire1"))
        {
            Instantiate(cookiePre, transform);
            bakingFlg = false;
        }
    }
}