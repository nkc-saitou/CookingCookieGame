using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadNameTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // 接続されているコントローラの名前を調べる
        var controllerNames = Input.GetJoystickNames();

        Debug.Log(controllerNames[0]);

        // 一台もコントローラが接続されていなければエラー
        if (controllerNames[0] == "") Debug.Log("Error");
    }
}