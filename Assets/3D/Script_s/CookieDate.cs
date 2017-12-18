using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CookieDate : MonoBehaviour {

    public enum CookieType
    {
        normalCookie = 0,
        jamCookie,
        chocolateCookie
    }

    public string cookieKing;

    public CookieType cookieType;

    void Start()
    {
        switch(cookieType)
        {
            case CookieType.normalCookie:
                break;

            case CookieType.jamCookie:
                break;

            case CookieType.chocolateCookie:
                break;
        }
    }
}
