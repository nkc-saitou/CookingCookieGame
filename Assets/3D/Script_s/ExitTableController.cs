using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================================================
// 出口オブジェクトクラス
//===================================================
[System.Serializable]
public class DirectionName
{
    [SerializeField]
    string name = "東";
    
    [Header("工場内のオブジェクト")]
    public GameObject destroyObj;

    [Header("外のオブジェクト")]
    public GameObject createObj;

    public enum DirName
    {
        North = 0, //北
        South,     //南
        East,      //東
        West       //西
    }

    [Header("どの方向に置かれているのか")]
    public DirName dirName;
}

//===================================================

public class ExitTableController : MonoBehaviour
{
    public GameObject cookieSoldierPre; //作ったクッキーのPrefab
    public DirectionName[] directionName;

    void Start()
    {

    }

    void Update()
    {
        foreach(DirectionName n in directionName)
        {
            //子オブジェクトがある状態だったら
            if (n.destroyObj.transform.childCount >= 1)
            {
                //中で作ったクッキーを削除
                Destroy(n.destroyObj.transform.GetChild(0).gameObject);

                //外にクッキーを出す
                Instantiate(
                    cookieSoldierPre, 
                    directionName[(int)n.dirName].createObj.transform.localPosition, 
                    Quaternion.identity,directionName[(int)n.dirName].createObj.transform);
            }
        }
    }
}