using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //----------------------------------------------------
    // public
    //----------------------------------------------------
    public GameObject cookiePre; //クッキーPrefab

    //----------------------------------------------------
    // private
    //----------------------------------------------------
    GameObject cookieObj; //作ったオブジェクトを保存する
    bool cookieHaveFlg = false;

    List<GameObject> cookieLis = new List<GameObject>(); //作ったオブジェクトをリストで管理

	void Start()
    {
		
	}

    void Update()
    {

        StartCoroutine(CookieManager());
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //あたったのが机オブジェクト以外だったら終了
            if (col.gameObject.tag != "Table" && col.gameObject.tag != "CookingTable" && col.gameObject.tag != "ExitTable") return;

            //調理台に当たった状態で、クッキーを持っていなかったら
            if (col.gameObject.tag == "CookingTable" && gameObject.transform.childCount < 1)
            {
                cookieObj = Instantiate(cookiePre, gameObject.transform);
            }

            else if (col.gameObject.transform.childCount < 1) //既に机に物が置いてあったらおけない
            {
                if (cookieObj == null) return;

                //持っているオブジェクトをを机の真ん中に置く
                cookieObj.transform.position = col.gameObject.transform.position;

                //置いたオブジェクトを机の子オブジェクトにする
                cookieObj.transform.parent = col.gameObject.transform;

                //クッキー☆の描画順を変える
                SpriteRenderer cookieObjRenderer = cookieObj.GetComponent<SpriteRenderer>();
                cookieObjRenderer.sortingOrder = 1;

                //プレイヤーとクッキーの親子関係を解除する
                gameObject.transform.DetachChildren();
            }
        }
    }

    IEnumerator CookieManager()
    {
        //クッキーを持っていなかったら終了
        if (gameObject.transform.childCount < 1) yield break;

        yield return new WaitForSeconds(0.5f);

        if (Input.GetButtonDown("Fire1"))
        {
            gameObject.transform.DetachChildren();
            cookieObj = null;
        }
    }
}