using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    //工場の入口前
    public Vector2 pos;
    public Vector2 FactryGate;
    //速さ
    public Vector2 speed=new Vector2(0.05f,0.05f);
    // ラジアン
    private float rad;
    // 現在位置
    private Vector2 Position;
    void Start () {
    }
	
	void Update () {
        if (pos.x-0.5f<=gameObject.transform.position.x&&gameObject.transform.position.x<= pos.x+0.5f
            || pos.y-0.5f<=transform.position.y&&transform.position.y<=pos.y+0.5f)
        {
            
            Debug.Log(pos);
        }
        rad = Mathf.Atan2(
            pos.x - transform.position.y,
            pos.y - transform.position.x);
        Position = transform.position;
        Position.x += speed.x * Mathf.Cos(rad);
        Position.y += speed.y * Mathf.Sin(rad);
        transform.position = Position;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Gate")
        {
            pos.x = FactryGate.x;
            pos.y = FactryGate.y;
        }
    }
}
