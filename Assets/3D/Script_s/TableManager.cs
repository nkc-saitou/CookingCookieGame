using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour {

    //-----------------------------------------
    // public
    //-----------------------------------------
    [Header("クッキーの素")]
    public GameObject elemPre;

    [Header("こねたクッキー")]
    public GameObject kneadPre;

    [Header("焼いたクッキー(ノーマル)")]
    public GameObject bakingPre_normal;

    [Header("焼いたクッキー(チョコレート)")]
    public GameObject bakingPre_chocolate;

    [Header("焼いたクッキー(ジャム)")]
    public GameObject bakingPre_jam;

    [Header("焼いたクッキー(ダークマター)")]
    public GameObject bakingPre_darkMatter;

    [Header("チョコレート")]
    public GameObject chocolatePre;

    [Header("ジャム")]
    public GameObject jamPre;
}
