using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CookieSoldier : MonoBehaviour {

    int HP = 3;

    void Start ()
    {

    }
	
	void Update ()
    {
		
	}

    GameObject nearTagObj(GameObject dirObs)
    {
        float tmpDis = 0;
        float nearDis = 0;
        GameObject targetObj = null;

        foreach(GameObject obs in dirObs.transform)
        {
            tmpDis = Vector2.Distance(obs.transform.position, transform.position);

            if(tmpDis == 0 || tmpDis < nearDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }
        }

        return targetObj;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "CookieEnemy")
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
