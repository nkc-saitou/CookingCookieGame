using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CookieSearch : MonoBehaviour {
    EnemyMove EM;

	void Start () {
        EM = GetComponentInParent<EnemyMove>();
	}
	void Update () {
        
    }
    void OnTriggerStay2D(Collider2D col)
    {
        //if(col.tag=="クッキー☆")
        if (col.name == "cK")
        {
            
            
        }
    }
}
