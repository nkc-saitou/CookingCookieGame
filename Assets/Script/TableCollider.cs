using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCollider : MonoBehaviour {

    public enum TableType
    {
        Table = 0,
        ElemTable,
        KneadTable,
        BakingTable,
        ExitTable
    }

    public TableType tableType;

	void Start ()
    {
		
	}
	
	void Update () {
		
	}

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player_1" && col.gameObject.tag != "Player_2") return;
        
    }
}
