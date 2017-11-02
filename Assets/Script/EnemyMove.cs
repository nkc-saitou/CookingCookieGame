using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnemyMove : MonoBehaviour {
    //工場の入口前の座標
    public Vector2 pos;
    //工場内の座標
    public Vector2 FactryGate;
    //速さ
    public Vector2 speed=new Vector2(0.01f,0.01f);
    // ラジアン
    private float rad;
    // 現在位置
    private Vector2 Position;
    //一番近いクッキー
    public GameObject nearestCookie=null;
    //工場入口についたかどうか
    private bool Gateflag = false;
    void Start () {
    }
	
	void Update () {
        if (nearestCookie != null && Gateflag == false)//クッキーがいるときの動き
        {
            rad = Mathf.Atan2(
                nearestCookie.transform.position.y - transform.position.y,
                nearestCookie.transform.position.x - transform.position.x);
            Position = transform.position;
            Position.x += speed.x * Mathf.Cos(rad);
            Position.y += speed.y * Mathf.Sin(rad);
            transform.position = Position;
        }
        else
        {//クッキーがいないときの動き

            rad = Mathf.Atan2(
                pos.x - transform.position.y,
                pos.y - transform.position.x);
            Position = transform.position;
            Position.x += speed.x * Mathf.Cos(rad);
            Position.y += speed.y * Mathf.Sin(rad);
            transform.position = Position;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Gate")
        {
            pos.x = FactryGate.x;
            pos.y = FactryGate.y;
            Gateflag = true;
        }
    }
}
