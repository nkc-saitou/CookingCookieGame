using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class EnemyMove : MonoBehaviour {

    //行動パターン
    public enum MovePattern {_Stalking,_Wall,_NormalMove,_CookieAtack,_Death}
    MovePattern movepattern=MovePattern._NormalMove;

    NavMeshAgent NMA;

    // 現在位置
    private Vector3 Position;
    public int _HP = 1;
    //速さ
    private Vector3 speed=new Vector3(0.1f,0f,0.1f);

    // ラジアン
    private float rad;
    //一番近いクッキー
    private GameObject nearestCookie=null;

    //クッキーに接触しているか
    private bool nearflg = false;
    //クッキーが視界にいるか
    private bool vision;
    //工場入口についたかどうか
    private bool wall = false;

    public GameObject NearestCookie
    {
        get { return nearestCookie; }
        set { nearestCookie = value; }
    }

    void Start () {
        NMA = GetComponent<NavMeshAgent>();
        NMA.stoppingDistance = 0.5f;
    }

    void Update()
    {
        if (_HP <= 0)
        {
            movepattern = MovePattern._Death;
        }

        switch (movepattern)
        {
            case MovePattern._Stalking:
                if (nearestCookie == null)
                {
                    movepattern = MovePattern._NormalMove;
                    break;
                }
                if (nearflg)
                {
                    movepattern = MovePattern._CookieAtack;
                    break;
                }
                Stalking();
                break;

            case MovePattern._NormalMove:
                if (wall)
                {
                    movepattern = MovePattern._Wall;
                    break;
                }
                if (nearestCookie != null)
                {
                    movepattern = MovePattern._Stalking;
                    break;
                }
                NormalMove();
                break;

            case MovePattern._Wall:
                if (nearestCookie != null)
                {
                    movepattern = MovePattern._Stalking;
                    break;
                }
                if (wall == false)
                {
                    movepattern = MovePattern._NormalMove;
                    break;
                }
                Wall();
                break;

            case MovePattern._CookieAtack:
                if (nearflg == false)
                {
                    movepattern = MovePattern._NormalMove;
                    break;
                }
                CookieAtack();
                break;

            case MovePattern._Death:
                Death();
                break;
        }

    }
    
    void Stalking()
    {
        NMA.SetDestination(nearestCookie.transform.position);
        /*    rad = Mathf.Atan2(
                nearestCookie.transform.position.z - transform.position.z,
                nearestCookie.transform.position.x - transform.position.x);
        Position = transform.position;
        Position.x += speed.x * Mathf.Cos(rad);
        Position.z += speed.z * Mathf.Sin(rad);
        transform.position = Position;*/
    }

    void NormalMove()
    {
           rad = Mathf.Atan2(
               0 - transform.position.z,
               0 - transform.position.x);
       Position = transform.position;
       Position.x += speed.x * Mathf.Cos(rad);
       Position.z += speed.z * Mathf.Sin(rad);
       transform.position = Position;
    }

    void Wall()
    {
        
    }

    void CookieAtack()
    {
        
    }

    void Death()
    {
        Destroy(gameObject);
    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Cookie")
        {
            nearflg = true;
        }
        if (col.tag == "Wall")
        {
            wall = true;
        }
        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Cookie")
        {
            nearflg = false;
        }

        if (col.tag == "Wall")
        {
            wall = false;
        }
    }
}
