using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CookieMove : MonoBehaviour {

    //行動パターン
    public enum MovePattern { _Wait, _Search, _Stalking, _Atack, _Death }
    MovePattern movepattern = MovePattern._Wait;
    NavMeshAgent NMA;
    NavMeshHit hit;
    //視界に入っているか
    private bool vision;

    // 現在位置
    private Vector3 Position;
    public float _HP = 5;
    //体当たり速さ
    private Vector3 speed = new Vector3(0.5f, 0f, 0.5f);

    // ラジアン
    private float rad;
    //一番近いクッキー
    private GameObject nearestEnemy = null;

    //クッキーが攻撃範囲
    private bool atackarea = false;
    //待ち
    private bool wai = true;
    private float time = 0.08f;

    public bool Atackarea
    {
        get { return atackarea; }
        set { atackarea = value; }
    }

    public GameObject NearestEnemy
    {
        get { return nearestEnemy; }
        set { nearestEnemy = value; }
    }

    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        NMA.stoppingDistance = 4f;
    }

    void Update()
    {
        if (_HP <= 0)
        {
            movepattern = MovePattern._Death;
        }
        switch (movepattern)
        {

            case MovePattern._Wait:
                if (wai)
                {
                    Invoke("Wait", time);
                    break;
                }
                break;


            case MovePattern._Search:
                wai = true;
                if (GameObject.FindGameObjectWithTag("Enemy")||GameObject.FindGameObjectWithTag("Prediction"))
                {
                    atackarea = false;
                    movepattern = MovePattern._Stalking;
                    NMA.autoBraking = false;
                    break;
                }
                Search();
                break;


            case MovePattern._Stalking:
                if (atackarea&&vision)
                {
                    movepattern = MovePattern._Atack;
                    NMA.isStopped = true;
                    break;
                }
                if (!GameObject.FindGameObjectWithTag("Enemy")&& !GameObject.FindGameObjectWithTag("Prediction"))
                {
                    movepattern = MovePattern._Wait;
                    break;
                }
                Stalking();
                break;


            case MovePattern._Atack:
                if (!GameObject.FindGameObjectWithTag("Enemy"))
                {
                    movepattern = MovePattern._Wait;
                    break;
                }
                Atack();
                break;


            case MovePattern._Death:
                Death();
                break;
        }
    }

    void Wait()
    {
        wai = false;
        NMA.ResetPath();
        NMA.isStopped = false;
        movepattern = MovePattern._Search;
    }

    void Search()
    {

    }
    void Stalking()
    {
        if (nearestEnemy == null)
        {
            return;
        }
        else if (NMA.Raycast(nearestEnemy.transform.position, out hit))
        {
            vision = false;
        }
        else
        {
            vision = true;
        }

        NMA.speed = 4;
        NMA.SetDestination(nearestEnemy.transform.position);
    }

    void Atack()
    {
        if (nearestEnemy != null)
        {
            /*NMA.speed = 20;
            NMA.SetDestination(nearestEnemy.transform.position);*/
            rad = Mathf.Atan2(
                nearestEnemy.transform.position.z - transform.position.z,
                nearestEnemy.transform.position.x - transform.position.x);
            Position = transform.position;
            Position.x += speed.x * Mathf.Cos(rad);
            Position.z += speed.z * Mathf.Sin(rad);
            transform.position = Position;
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            NMA.isStopped = true;
            movepattern = MovePattern._Wait;
            col.GetComponent<EnemyMove>()._HP--;
            _HP-=0.5f;
        }
    }
}

