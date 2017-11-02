using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchArea : MonoBehaviour {
    EnemyMove EM;
	void Start () {
        EM = GetComponentInParent<EnemyMove>();
	}
	
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                float dist = 3.0f; // 距離3未満の範囲のクッキーを検知
                GameObject[] cookies = null;
                cookies = GameObject.FindGameObjectsWithTag("Player").
                Where(e => Vector2.Distance(transform.position, e.transform.position) < dist).
                OrderBy(e => Vector2.Distance(transform.position, e.transform.position)).ToArray();
                EM.nearestCookie = cookies[0];
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            EM.nearestCookie = null;
        }
    }
}
