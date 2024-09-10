using WebSocketSharp;
using WebSocketSharp.Net;
using System;
using System.Security.Authentication;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.Collections;
using UnityEngine;

public class WebSocketController : MonoBehaviour
{
    
    private enum SslProtocolsHack
    {
        Tls = 192,
        Tls11 = 768,
        Tls12 = 3072
    }
    Action CurrentAction;
    public static Queue<Action> ActionQueue;
    Coroutine updateRoutine;

    WebSocket ws;

    int tryCnt = 3;
    string url;
    string[] subs;

    string _certificatePath = "\\cert\\public_privatekey.pfx";

    IEnumerator UpdateQueue()
    {
        //****************
        //웹소캣 리시브 받아서 다음 동작하면 오브젝트 기능이 안됨
        //큐로 액션 받아서 여기서 실행
        //****************

        //BusyWating.ins.ShowWithCount();
        while (true)
        {
            // WebRequestUtil에서 Coroutine이 끝나고 로그인 하면
            if (ActionQueue != null && ActionQueue.Count > 0)
            {
                //큐에서 가장 위에 있는 항목을 반환한다
                Action act = CurrentAction = ActionQueue.Peek();
                act.Invoke();
                print("WebSocketActionQueue Invoke : " + (ActionQueue.Count - 1));
                //yield return new WaitForEndOfFrame();
                // 실행후 제거
                ActionQueue.Dequeue();
            }
            yield return null; // new WaitForSeconds(1.0f);
        }
        //BusyWating.ins.HideWithCount();
    }


    public void SetUrlSubs(string url_, string[] subs_)
    {
        url = url_;
        subs = subs_;
    }
    public void Connect()
    {
        
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }

        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        
        ws = new WebSocket(url);

        var sslProtocolHack = (System.Security.Authentication.SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
        ws.SslConfiguration.EnabledSslProtocols = sslProtocolHack;

        //ssl 인증서 무시하는 코드입니다
        ws.SslConfiguration.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        DebugScrollView.Instance.Print("ReadyState = " + ws.ReadyState.ToString()); // ReadyState=Connecting 상태

        ws.OnOpen += (sender, e) => //ws.Send("Hi, there!");
        {
            DebugScrollView.Instance.Print("OnOpen");
            DebugScrollView.Instance.Print("ReadyState = " + ws.ReadyState.ToString()); // ws.Connect 가 성공적이면 ReadyState 는 Open 이며 이 상태에서 서버와 통신가능. 
            ConnectStomp();

            //Subscribe("/sub/weatherWarning");
            for(int i = 0; i < subs.Length; i++)
            {
                Subscribe(subs[i], i);
            }
            updateRoutine = StartCoroutine(UpdateQueue());
            tryCnt = 3;
        };

        ws.OnError += (sender, e) =>
        {
            DebugScrollView.Instance.Print("OnError e :" + e.Message);

        };

        ws.OnClose += (sender, e) =>
        {
            //if (e.Code == 1015 && ws.SslConfiguration.EnabledSslProtocols != sslProtocolHack)
            //{
            //    ws.SslConfiguration.EnabledSslProtocols = sslProtocolHack;
            //    DebugScrollView.Instance.Print("handshake");
            //}
            DebugScrollView.Instance.Print("OnClose");
            DebugScrollView.Instance.Print("e :" + e.Code);

            //if (tryCnt > 0)
            //{
            //    DebugScrollView.Instance.Print("ReConnect : " + tryCnt);

            //    tryCnt--;
            //    Connect();
            //}
            //else
            {
                DebugScrollView.Instance.Print("Connect Fail");
                if (updateRoutine != null)
                {
                    StopCoroutine(updateRoutine);
                }
            }
        };
        ws.OnMessage += (sender, e) =>
        {
            DebugScrollView.Instance.Print("OnMessage : " + e.Data);
            AddAction(e.Data);
        };

        ws.Connect(); // Connect to the server.
    }

    public void Disconnect()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    void AddAction(string eData)
    {

        AddWebsocketAction(delegate {
            if (DataManager.Instance.CheckReceiveEvent(eData))
            {
                MainManager.Instance.ReceiveEvent();
            }
        });
    }
    public void AddWebsocketAction(Action act)
    {
        if (ActionQueue == null)
        {
            ActionQueue = new Queue<Action>();
        }
        ActionQueue.Enqueue(act);
    }

    private void ConnectStomp()
    {
        string stompConnect = "CONNECT\naccept-version:1.0,1.1,2.0,1.2\nheart-beat:60000,60000\n\n\0";
        ws.Send(stompConnect);
        //Debug.Log("cons");
    }
    private void Subscribe(string destination, int id)
    {
        string stompSubscribe = $"SUBSCRIBE\nid:sub-{id}\ndestination:{destination}\nack:auto\n\n\0";
        ws.Send(stompSubscribe);
        //Debug.Log("subs");
    }
     bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }


}
