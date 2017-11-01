using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTable : MonoBehaviour {

    ExitTableController exitTableController = new ExitTableController();

    public enum ExitType
    {
        North = 0, //北
        South,     //南
        East,      //東
        West       //西
    }

    public ExitType exitType;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        CookieSetting();
    }

    void CookieSetting()
    {
        //クッキーの台になにもおいてなかったら終了
        if (gameObject.transform.childCount == 0) return;

        Destroy(gameObject.transform.GetChild(0).gameObject);
        
    }
}
