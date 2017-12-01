using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CookieSoldier : MonoBehaviour {

    int HP = 1;
    float rad;//ラジアン計算用変数
    Vector2 soldierPos; //クッキーの現在のポジション
    GameObject enemyParent;
    GameObject targetObj;

    public float speed = 10.0f;

    void Start ()
    {
        enemyParent = GameObject.FindGameObjectWithTag("EnemyObj");
        nearTagObj(enemyParent);
    }
	
	void Update ()
    {
        EnemyMove();
        Debug.Log(targetObj);
    }

    void nearTagObj(GameObject dirObs)
    {
        float tmpDis = 0;
        float nearDis = 0;

        foreach(Transform obs in dirObs.transform)
        {
            tmpDis = Vector2.Distance(obs.transform.position, transform.position);

            if(tmpDis == 0 || tmpDis < nearDis)
            {
                nearDis = tmpDis;
            }
            targetObj = obs.gameObject;
        }
    }

    //---------------------------------------------
    // クッキーの移動処理
    //---------------------------------------------
    void EnemyMove()
    {
        //nearTagObj(enemyParent);

        Debug.Log(targetObj);
        //ラジアン計算
        //atan2(目標方向のy座標 - 初期位置のy座標、目標方向のｘ座標 - 初期位置のy座標)
        rad = Mathf.Atan2(
            targetObj.transform.localPosition.y - gameObject.transform.position.y,
            targetObj.transform.localPosition.x - gameObject.transform.position.x
            );

        //現在のオブジェクトの位置を代入
        soldierPos = gameObject.transform.localPosition;

        //x += SPEED * cos(ラジアン)
        //y += SPEED * sin(ラジアン)
        soldierPos.x += speed * Time.deltaTime * Mathf.Cos(rad);
        soldierPos.y += speed * Time.deltaTime * Mathf.Sin(rad);

        //現在の位置へ計算した値を代入する
        gameObject.transform.localPosition = soldierPos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            Destroy(col.gameObject);
            HP--;
        }

        if(HP< 0)
        {
            Destroy(gameObject);
        }
    }
}