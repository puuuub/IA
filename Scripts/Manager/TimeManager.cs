
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

    // ������ ��û(Request) �Ҷ� SeoulTimeSpan ��ŭ ó��
    public static DateTime GetCurrentDateTimeWithGap()
    {
        DateTime currTime = GetCurrentDateTime();
        // ���������Ϳ� 9�ð�����
        currTime -= SeoulTimeSpan;
        return currTime;
    }

    // ������ ��û�ϱ� ���� �ð����� ������
    public static long GetNowTimestamp()
    {
        DateTime currTime = GetCurrentDateTimeWithGap();
        long cur = (long)((currTime - PastOrigin).TotalMilliseconds);
        DebugScrollView.Instance.Print(" ����ð�  " + currTime.ToString() + "  " + cur);

        //epoch time���� ������� millisecond timestamp�� �����Ѵ�.          
        return cur;
    }

    // ������ ��û�ϱ� ���� �ð����� ������
    /// <summary>
    /// ����κ��� ����, ��ð�, ���, ���� �� or ���� TimeStamp�� �����´�
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

        DebugScrollView.Instance.Print(" ���ýð�  " + currTime.ToString());
        //epoch time���� ������� millisecond timestamp�� �����Ѵ�.          
        return cur;
    }


    // �ð� ������ ���� ���� ���� �и������� ��������
    public static long GetNowTimestampEx()
    {
        //���� �ð������� �����´�.
        DateTime currTime = DateTime.Now;

        long cur = (long)(currTime - PastOrigin).TotalMilliseconds;

        //epoch time���� ������� millisecond timestamp�� �����Ѵ�.          
        return cur;
    }

    // ���� ��� ����ð� ǥ�ÿ�
    public static DateTime GetCurrentDateTime()
    {
        //���� �ð������� �����´�.
        DateTime currTime = DateTime.Now;

        TimeSpan ts = ManipulateTS_SeoulUCT;// new TimeSpan(SpecifyGapSeoulUTC * TimeSpan.TicksPerMillisecond);
        currTime -= ts;
        //DebugScrollView.Instance.Print(" ����ð�  " + currTime.ToString() + "  " + cur);
        //print(" ����ð�  " + currTime.ToString());

        return currTime;
    }

    // ������ ��û�ϱ� ���� �ð����� ������
    public static long GetTimestamp(DateTime dt)
    {
        DateTime currTime = dt;

        DebugScrollView.Instance.Print(" ���ýð�  " + currTime.ToString());
        // ���������Ϳ� 9�ð�����
        currTime -= SeoulTimeSpan;
        //epoch time���� ������� millisecond timestamp�� �����Ѵ�.          
        return (long)(currTime - PastOrigin).TotalMilliseconds;
    }


    // �������� ������� ������ ó��
    public static string GetDate(string timeStemp)
    {
        long tempTime = long.Parse(timeStemp);

        long curStemp = GetNowTimestampEx();

        long gapStemp = curStemp - tempTime;

        //DateTime result = DateTime.Now.AddMilliseconds(-gapStemp);
        TimeSpan ts = new DateTime(0) - PastOrigin;
        DateTime result = new DateTime((tempTime - (long)ts.TotalMilliseconds) * TimeSpan.TicksPerMillisecond);

        // ������ 9�ð�����
        result += SeoulTimeSpan;
        //DebugScrollView.Instance.Print(" ���ýð�  " + result.ToString());
        return result.ToString(@"yyyy-MM-dd HH:mm:ss");
    }

    // �������� ������� ������ ó��
    public static string GetDate(long timeStemp)
    {
        long curStemp = GetNowTimestampEx();

        long gapStemp = curStemp - timeStemp;

        //DateTime result = DateTime.Now.AddMilliseconds(-gapStemp);
        TimeSpan ts = new DateTime(0) - PastOrigin;
        DateTime result = new DateTime((timeStemp - (long)ts.TotalMilliseconds) * TimeSpan.TicksPerMillisecond);

        // ������ 9�ð�����
        //print(RspGap + "RspGap");
        result += SeoulTimeSpan;
        DebugScrollView.Instance.Print(" ���ýð�  " + result.ToString());
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
        // �����ð��� ���۽ð� ���� ���߱�
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
