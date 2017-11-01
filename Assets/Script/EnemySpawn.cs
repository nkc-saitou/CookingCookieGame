using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    //沸くクッキー
    public GameObject Cookie1;
    //クッキーの沸きパターン
    private int Pattern;
    //正負ランダム
    private int a;
	void Start () {
        StartCoroutine("ESpawn");
	}
	

	void Update () {
	}

    private void RandomSpawnECookie()
    {
        RandomPM();
        switch (Pattern = Random.Range(1, 3))
        {
            case 1://上下のクッキーの沸き
                GameObject Cookie = Instantiate(Cookie1, new Vector3(Random.Range(-5.5f, 5.5f), 5 * a, 0), Quaternion.identity);
                EnemyMove EM = Cookie.GetComponent<EnemyMove>();
                EM.pos = transform.position;
                EM.pos.x = 3.5f * a; EM.pos.y = 0;
                EM.FactryGate.x = 2 * a; EM.FactryGate.y = 0;
                Debug.Log(EM.pos);
                break;

            case 2://左右のクッキーの沸き
                GameObject cooKie = Instantiate(Cookie1, new Vector3(9 * a, Random.Range(-4f, 4f), 0), Quaternion.identity);
                EnemyMove _EM = cooKie.GetComponent<EnemyMove>();
                _EM.pos = transform.position;
                _EM.pos.x = 0; _EM.pos.y = 4.5f * a;
                _EM.FactryGate.x = 0; _EM.FactryGate.y = 3 * a;
                Debug.Log(_EM.pos);
                break;
        }
    }

    private IEnumerator ESpawn()//敵の沸き間隔
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            RandomSpawnECookie();
        }
    }

    private void RandomPM()//正負のランダム
    {
        a = Random.Range(0, 2);
        a *= 2;
        a--;
    }
}
