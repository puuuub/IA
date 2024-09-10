/*

new WebSocket("ws://110.45.218.79:8085/ws")에 웹소켓 주소 입력
 Subscribe("/sub/weatherStatus", 0);로 세부 주소 입력
 OnMessage {}에서 데이터 받아옴(3줄이상 안읽혀지므로 받아서 변수에 저장만 하고 업데이트에서 처리할것)

WeatherStatus와WeatherWarnings은 데이터 저장용 클래스 이므로 본인 데이터에 맞게 클래스를 만들던지 JObject활용

*/

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class Stomp : MonoBehaviour
{

    private WebSocket webSocket;
    
    void Start()
    {
    //portStandatData = GetComponent<PortStandatData>();
    // 웹소켓 연결
    //https://211.217.241.196:50443
        webSocket = new WebSocket("wss://211.217.241.196:50442/dtsys");
        webSocket.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Opened" + e.ToString());
            // STOMP 프로토콜 연결
            ConnectStomp();

            //Subscribe("/sub/weatherWarning");

            Subscribe("/topic/event/rule", 0);
            //Subscribe("/sub/weatherWarning", 1);

        };
        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Received message: " + e.Data);

            // 메시지 처리 로직 추가

            if (e.Data.Contains("sub-0"))
            {
                //status = JsonUtil.JsonToObject<WeatherStatus>(Deserialize(e.Data));
                SetStatus(Deserialize(e.Data));
            }
            else
            {
                //warning = JsonUtil.JsonToObject<WeatherWarnings>(Deserialize(e.Data));
                SetWarning(Deserialize(e.Data));
            }


        };
        webSocket.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Closed");
        };


        // 웹소켓 연결 시작
        webSocket.Connect();


    }

    // STOMP 프로토콜 연결
    private void ConnectStomp()
    {
        string stompConnect = "CONNECT\naccept-version:1.0,1.1,2.0,1.2\nheart-beat:60000,60000\n\n\0";
        webSocket.Send(stompConnect);
        //Debug.Log("cons");
    }

    // 메시지 전송
    public void SendMessage(string destination, string message)
    {
        string stompMessage = $"SEND\ndestination:{destination}\n\n{message}\0";
        webSocket.Send(stompMessage);
    }
    // Subscribe
    private void Subscribe(string destination, int id)
    {
        string stompSubscribe = $"SUBSCRIBE\nid:sub-{id}\ndestination:{destination}\nack:auto\n\n\0";
        webSocket.Send(stompSubscribe);
        //Debug.Log("subs");
    }
    void OnDestroy()
    {
        // 웹소켓 연결 해제
        if (webSocket != null && webSocket.ReadyState == WebSocketState.Open)
        {
            webSocket.Close();
        }
    }
    string Deserialize(string input)
    {
        // 정규식을 사용하여 JSON 부분 추출
        string pattern = @"\{""success"".*\}";
        Match match = Regex.Match(input, pattern);

        if (match.Success)
        {
            return match.Value;
        }

        return null;
    }
    bool IsStatusChanged=false;
    void SetStatus(string statu)
    {
        IsStatusChanged = true;
        //portStandatData.SetStatus(status);
    }
    bool IsWarningChanged = false;
    void SetWarning(string warnings)
    {
        IsWarningChanged = true;
        //portStandatData.SetWarning(warning);
    }
    private void Update()
    {
        if (IsStatusChanged)
        {
            IsStatusChanged =false;
            Debug.Log("StatusChange");
        }
        if (IsWarningChanged)
        {
            IsWarningChanged=false;
            Debug.Log("WarningChange");
        }
    }

}
