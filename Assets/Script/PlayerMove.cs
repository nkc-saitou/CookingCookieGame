using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    //----------------------------------------------
    // private
    //----------------------------------------------
    Rigidbody2D rg2d;

    PlayerSetting playerSetting;

    void Start ()
    {
        playerSetting = GetComponent<PlayerSetting>();
        rg2d = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        if (playerSetting.playIsGamePad) Move_GamePad();
        else Move();
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //=====================ゲームパッド用の処理========================

    //----------------------------------------------
    // 移動処理メソッド
    //----------------------------------------------
    void Move_GamePad()
    {
        switch (playerSetting.playerNum)
        {
            case 1:
                rg2d.velocity = new Vector2(Input.GetAxis("L_JoyStick1_XAxis") * playerSetting.speed, Input.GetAxis("L_JoyStick1_YAxis") * playerSetting.speed);
                break;

            case 2:
                rg2d.velocity = new Vector2(Input.GetAxis("L_JoyStick2_XAxis") * playerSetting.speed, Input.GetAxis("L_JoyStick2_YAxis") * playerSetting.speed);
                break;
        }
    }
    
    //=====================キー入力用の処理========================

    //----------------------------------------------
    // 移動処理メソッド
    //----------------------------------------------
    void Move()
    {
        switch (playerSetting.playerNum)
        {
            case 1:
                rg2d.velocity = new Vector2(Input.GetAxis("Horizontal_1") * playerSetting.speed, Input.GetAxis("Vertical_1") * playerSetting.speed);
                break;

            case 2:
                rg2d.velocity = new Vector2(Input.GetAxis("Horizontal_2") * playerSetting.speed, Input.GetAxis("Vertical_2") * playerSetting.speed);
                break;
        }
    }
}
