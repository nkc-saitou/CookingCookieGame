using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    //----------------------------------------------
    // private
    //----------------------------------------------
    Rigidbody2D rg2d;

    //----------------------------------------------
    // public
    //----------------------------------------------
    [SerializeField,Header("移動速度"),Range(0,5)]
    public float speed = 3.0f;

    [SerializeField, Header("ゲームパッドでプレイするか")]
    public bool PlayIsGamePad = false;

    //================================================
    void Start ()
    {
        rg2d = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        if (PlayIsGamePad) Move_GamePad();
        else Move();
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //=====================ゲームパッド用の処理========================

    //----------------------------------------------
    // 移動処理メソッド
    //----------------------------------------------
    void Move_GamePad()
    {
        rg2d.velocity = new Vector2(Input.GetAxis("L_JoyStick1_XAxis") * speed, Input.GetAxis("L_JoyStick1_YAxis") * speed);
    }

    //=====================キー入力用の処理========================

    //----------------------------------------------
    // 移動処理メソッド
    //----------------------------------------------
    void Move()
    {
        rg2d.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
    }
}
