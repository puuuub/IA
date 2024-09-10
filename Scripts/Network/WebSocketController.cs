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
        //����Ĺ ���ú� �޾Ƽ� ���� �����ϸ� ������Ʈ ����� �ȵ�
        //ť�� �׼� �޾Ƽ� ���⼭ ����
        //****************

        //BusyWating.ins.ShowWithCount();
        while (true)
        {
            // WebRequestUtil���� Coroutine�� ������ �α��� �ϸ�
            if (ActionQueue != null && ActionQueue.Count > 0)
            {
                //ť���� ���� ���� �ִ� �׸��� ��ȯ�Ѵ�
                Action act = CurrentAction = ActionQueue.Peek();
                act.Invoke();
                print("WebSocketActionQueue Invoke : " + (ActionQueue.Count - 1));
                //yield return new WaitForEndOfFrame();
                // ������ ����
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

        //ssl ������ �����ϴ� �ڵ��Դϴ�
        ws.SslConfiguration.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        DebugScrollView.Instance.Print("ReadyState = " + ws.ReadyState.ToString()); // ReadyState=Connecting ����

        ws.OnOpen += (sender, e) => //ws.Send("Hi, there!");
        {
            DebugScrollView.Instance.Print("OnOpen");
            DebugScrollView.Instance.Print("ReadyState = " + ws.ReadyState.ToString()); // ws.Connect �� �������̸� ReadyState �� Open �̸� �� ���¿��� ������ ��Ű���. 
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
