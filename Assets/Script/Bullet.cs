using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    float px;
    float py;
    float rot;
    float r;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        px = transform.position.x + Mathf.Cos(rot) * r;
        py = transform.position.y + Mathf.Sin(rot) * r;
	}
}
