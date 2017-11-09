using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour {

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

    public GameObject[] player;

    public enum TableType
    {
        Table = 0,
        ElemTable,
        KneadTable,
        BakingTable
    }

    public TableType tableType;

	void Start ()
    {

	}
	
	void Update ()
    {
		
	}

    void OnCollisionStay2D(Collision2D col)
    {
        foreach(GameObject p in player)
        {
            if (col.gameObject != p) return;
        }
    }

    //----------------------------------------------------
    // クッキーの素のテーブルに触れた時
    //----------------------------------------------------
    // colObj 衝突したオブジェクト, p どのプレイヤーと衝突したか
    void ElemTableCollision(GameObject colObj,GameObject p)
    {
        if (p.transform.childCount < 1)
        {
            Instantiate(elemPre, p.transform);
        }
    }

    void KneadTablePut(GameObject colObj, GameObject p)
    {


    }
}