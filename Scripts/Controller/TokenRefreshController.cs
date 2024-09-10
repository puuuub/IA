using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenRefreshController : MonoBehaviour
{
    public bool coroutineOn;

    Coroutine coroutine;

    readonly WaitForSeconds wfs5m = new WaitForSeconds(300f);
    
    public void StartTokenRefresh()
    {
        coroutineOn = true;
        coroutine = StartCoroutine(TokenRefresh());
    }

    public void EndTokenRefresh()
    {
        coroutineOn = false;

        if (coroutine !=null)
            StopCoroutine(coroutine);
    }

    IEnumerator TokenRefresh()
    {
        while (coroutineOn)
        {
            yield return wfs5m;
            WebRequestItemPool.Instance.RequestTokenRefresh();
        }
    }
}
