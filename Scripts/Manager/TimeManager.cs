
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{

    const int SeoulUTC = 9;
    static TimeSpan SeoulTimeSpan = new TimeSpan(SeoulUTC, 0, 0);
    static DateTime PastOrigin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


    //static TimeSpan ManipulateTS = GetManipulateTimeSpan(new DateTime(2021, 12, 27, 17, 19, 10), 0);
    static TimeSpan ManipulateTS_SeoulUCT = GetManipulateTimeSpan(new DateTime(2021, 12, 27, 17, 20, 10), 0);

    // 서버에 요청(Request) 할때 SeoulTimeSpan 만큼 처리
    public static DateTime GetCurrentDateTimeWithGap()
    {
        DateTime currTime = GetCurrentDateTime();
        // 서버데이터와 9시간차이
        currTime -= SeoulTimeSpan;
        return currTime;
    }

    // 서버에 요청하기 위해 시간정보 구성시
    public static long GetNowTimestamp()
    {
        DateTime currTime = GetCurrentDateTimeWithGap();
        long cur = (long)((currTime - PastOrigin).TotalMilliseconds);
        DebugScrollView.Instance.Print(" 현재시각  " + currTime.ToString() + "  " + cur);

        //epoch time부터 현재까지 millisecond timestamp를 리턴한다.          
        return cur;
    }

    // 서버에 요청하기 위해 시간정보 구성시
    /// <summary>
    /// 현재로부터 몇일, 몇시간, 몇분, 몇초 전 or 후의 TimeStamp를 가져온다
    /// </summary>
    /// <param name="days"></param>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static long GetNowTimestamp(double days, double hours, double minutes, double seconds)
    {
        DateTime currTime = GetCurrentDateTimeWithGap();
        currTime = currTime.AddDays(days).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);

        long cur = (long)((currTime - PastOrigin).TotalMilliseconds);

        DebugScrollView.Instance.Print(" 선택시각  " + currTime.ToString());
        //epoch time부터 현재까지 millisecond timestamp를 리턴한다.          
        return cur;
    }


    // 시간 조정을 위해 실제 현재 밀리세컨드 가져오기
    public static long GetNowTimestampEx()
    {
        //현재 시간정보를 가져온다.
        DateTime currTime = DateTime.Now;

        long cur = (long)(currTime - PastOrigin).TotalMilliseconds;

        //epoch time부터 현재까지 millisecond timestamp를 리턴한다.          
        return cur;
    }

    // 좌측 상단 현재시간 표시용
    public static DateTime GetCurrentDateTime()
    {
        //현재 시간정보를 가져온다.
        DateTime currTime = DateTime.Now;

        TimeSpan ts = ManipulateTS_SeoulUCT;// new TimeSpan(SpecifyGapSeoulUTC * TimeSpan.TicksPerMillisecond);
        currTime -= ts;
        //DebugScrollView.Instance.Print(" 현재시각  " + currTime.ToString() + "  " + cur);
        //print(" 현재시각  " + currTime.ToString());

        return currTime;
    }

    // 서버에 요청하기 위해 시간정보 구성시
    public static long GetTimestamp(DateTime dt)
    {
        DateTime currTime = dt;

        DebugScrollView.Instance.Print(" 선택시각  " + currTime.ToString());
        // 서버데이터와 9시간차이
        currTime -= SeoulTimeSpan;
        //epoch time부터 현재까지 millisecond timestamp를 리턴한다.          
        return (long)(currTime - PastOrigin).TotalMilliseconds;
    }


    // 서버에서 응답받은 데이터 처리
    public static string GetDate(string timeStemp)
    {
        long tempTime = long.Parse(timeStemp);

        long curStemp = GetNowTimestampEx();

        long gapStemp = curStemp - tempTime;

        //DateTime result = DateTime.Now.AddMilliseconds(-gapStemp);
        TimeSpan ts = new DateTime(0) - PastOrigin;
        DateTime result = new DateTime((tempTime - (long)ts.TotalMilliseconds) * TimeSpan.TicksPerMillisecond);

        // 서버와 9시간차이
        result += SeoulTimeSpan;
        //DebugScrollView.Instance.Print(" 선택시각  " + result.ToString());
        return result.ToString(@"yyyy-MM-dd HH:mm:ss");
    }

    // 서버에서 응답받은 데이터 처리
    public static string GetDate(long timeStemp)
    {
        long curStemp = GetNowTimestampEx();

        long gapStemp = curStemp - timeStemp;

        //DateTime result = DateTime.Now.AddMilliseconds(-gapStemp);
        TimeSpan ts = new DateTime(0) - PastOrigin;
        DateTime result = new DateTime((timeStemp - (long)ts.TotalMilliseconds) * TimeSpan.TicksPerMillisecond);

        // 서버와 9시간차이
        //print(RspGap + "RspGap");
        result += SeoulTimeSpan;
        DebugScrollView.Instance.Print(" 선택시각  " + result.ToString());
        return result.ToString(@"yyyy-MM-dd HH:mm:ss");
    }

    public static string GetTimeStemp(string year_, string mon_, string day_, string hour_, string min_, string sec_)
    {
        int year = int.Parse(year_);
        int mon = int.Parse(mon_);
        int day = int.Parse(day_);
        int hour = int.Parse(hour_);
        int min = int.Parse(min_);
        int sec = int.Parse(sec_);
        DateTime date = new DateTime(year, mon, day, hour, min, sec);
        long tagetTime = (long)(date - PastOrigin).TotalMilliseconds;
        tagetTime += (long)SeoulTimeSpan.TotalMilliseconds;
        return tagetTime.ToString();
    }

    public static string GetCurrentTime()
    {
        return GetCurrentDateTime().ToString(@"yyyy-MM-dd HH:mm:ss");
    }

    static TimeSpan GetManipulateTimeSpan(DateTime dTime, int localUTC)
    {
#if USE_START_TIME_CONTROL
        TimeSpan originTs = new DateTime(0) - PastOrigin;
        // 서버시간과 시작시각 기준 맞추기
        TimeSpan tempSpan = new DateTime(0) - dTime.AddMilliseconds(originTs.TotalMilliseconds);
        long cur = GetNowTimestampEx();
        long localUTCMillisec = (long)new TimeSpan(localUTC, 0, 0).TotalMilliseconds;
        long gap = cur + (long)tempSpan.TotalMilliseconds + localUTCMillisec;
        TimeSpan resultSpan = new TimeSpan(gap * TimeSpan.TicksPerMillisecond);
        return resultSpan;
#else
        return new TimeSpan(0);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
