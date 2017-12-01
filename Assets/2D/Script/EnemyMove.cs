using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnemyMove : MonoBehaviour {

    //行動パターン
    public enum MovePattern {_Stalking,_WallAtack,_NormalMove,_CookieAtack}
    MovePattern movepattern=MovePattern._NormalMove;

    //工場の入口前の座標
    public Vector2 pos;
    //工場内の座標
    public Vector2 FactryGate;
    // 現在位置
    private Vector2 Position;
    //速さ
    public Vector2 speed=new Vector2(0.01f,0.01f);

    // ラジアン
    private float rad;
    //一番近いクッキー
    public GameObject nearestCookie=null;

    //クッキーに隣接しているか
    private bool nearflg = false;
    //工場入口についたかどうか
    private bool Gateflag = false;
    bool wall = false;

    void Start () {
    }
	
	void Update () {
        switch (movepattern)
        {
            case MovePattern._Stalking:
                Stalking();
                break;

            case MovePattern._NormalMove:
                NormalMove();
                break;

            case MovePattern._WallAtack:
                WallAtack();
                break;

            case MovePattern._CookieAtack:
                CookieAtack();
                break;
        }
    }
    
    void Stalking()
    {
        if (nearestCookie == null && Gateflag == false)
        {
            movepattern = MovePattern._NormalMove;
            return;
        }
        if (nearflg)
        {
            movepattern = MovePattern._CookieAtack;
            return;
        }

            rad = Mathf.Atan2(
                nearestCookie.transform.position.y - transform.position.y,
                nearestCookie.transform.position.x - transform.position.x);
        Position = transform.position;
        Position.x += speed.x * Mathf.Cos(rad);
        Position.y += speed.y * Mathf.Sin(rad);
        transform.position = Position;
    }

    void NormalMove()
    {
        if (wall)
        {
            movepattern = MovePattern._WallAtack;
        }
        if (nearestCookie!=null&& Gateflag == false)
        {
            movepattern = MovePattern._Stalking;
        }

            rad = Mathf.Atan2(
                0 - transform.position.y,
                0 - transform.position.x);
                //pos.x - transform.position.y,
                //pos.y - transform.position.x);
        Position = transform.position;
        Position.x += speed.x * Mathf.Cos(rad);
        Position.y += speed.y * Mathf.Sin(rad);
        transform.position = Position;
    }

    void WallAtack()
    {
        if (nearestCookie!=null  && Gateflag == false)
        {
            movepattern = MovePattern._Stalking;
        }
    }

    void CookieAtack()
    {
        if (nearflg == false)
        {
            movepattern = MovePattern._NormalMove;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        /*if (col.name == "Gate")
        {
            pos.x = FactryGate.x;
            pos.y = FactryGate.y;
            Gateflag = true;
        }*/
        if (col.tag == "Player")
        {
            nearflg = true;
        }
        else if (col.name == "Wall")
        {
            wall = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            nearflg = false;
        }

        else if (col.name == "Wall")
        {
            wall = false;
        }
    }
}
