using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TableController))]
[RequireComponent(typeof(PlayerMove))]
public class PlayerSetting : MonoBehaviour {

    //----------------------------------------------------
    // public
    //----------------------------------------------------

    [Header("クッキーの素")]
    public GameObject elemPre;

    [Header("こねたクッキー")]
    public GameObject kneadPre;

    [Header("焼いたクッキー")]
    public GameObject bakingPre;

    //ボウルのスプライトを変更
    public SpriteRenderer cookingBowl;
    public Sprite[] bowlSp;

    [SerializeField, Header("移動速度"), Range(0, 5)]
    public float speed = 3.0f;

    [SerializeField, Header("ゲームパッドでプレイするか")]
    public bool playIsGamePad = false;

    [System.NonSerialized]
    public int playerNum;

    //----------------------------------------------------
    // 列挙型
    //----------------------------------------------------

    public enum PlayerNumber
    {
        One = 0,
        Two
    }

    public PlayerNumber playerNumber;

    void Start ()
    {
        GamePlayers();
    }
	
	void Update ()
    {
		
	}

    //-----------------------------------
    // プレイ人数の取得
    //-----------------------------------
    void GamePlayers()
    {
        switch (playerNumber)
        {
            case PlayerNumber.One:
                playerNum = 1;
                break;

            case PlayerNumber.Two:
                playerNum = 2;
                break;
        }
    }
}
