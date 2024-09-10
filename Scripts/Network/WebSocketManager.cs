using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WebSocketManager : SingletonMonoBehaviour<WebSocketManager>
{
    [SerializeField]
    WebSocketController wsController;

    public static Queue<Action> ActionQueue = new Queue<Action>();
    Action CurrentAction;
    // Start is called before the first frame update

    //string alianUrl = "ws://211.217.241.196:50440/dtsys";
    string alianUrl = "wss://211.217.241.196:50442/dtsys";
    string[] subs = { "/topic/event/rule" };

    void Start()
    {

    }
    public void EnqueueAction(Action act)
    {
        ActionQueue.Enqueue(act);
        DebugScrollView.Instance.Print("ActionQueue.Count : " + ActionQueue.Count);
    }


    IEnumerator UpdateQueue()
    {
        //BusyWating.ins.ShowWithCount();
        while (true)
        {
            // WebRequestUtil에서 Coroutine이 끝나고 로그인 하면
            if (ActionQueue.Count > 0)
            {
                //큐에서 가장 위에 있는 항목을 반환한다
                Action act = CurrentAction = ActionQueue.Peek();
                act.Invoke();
                DebugScrollView.Instance.Print("WebSocketActionQueue Invoke : " + (ActionQueue.Count - 1));
                //yield return new WaitForEndOfFrame();
                // 실행후 제거
                ActionQueue.Dequeue();
            }
            yield return null; // new WaitForSeconds(1.0f);
        }
        //BusyWating.ins.HideWithCount();
    }

    public void WebsocketConnect()
    {

        wsController.SetUrlSubs(alianUrl, subs);
        wsController.Connect();
        
        StartCoroutine(UpdateQueue());
    }
    


    public void SetDisconnect()
    {
        wsController.Disconnect();
    }
}
