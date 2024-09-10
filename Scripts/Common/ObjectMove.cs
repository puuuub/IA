using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectMove : MonoBehaviour
{
    public GameObject WayPointRoot;                 //이동 포인트 루트
    public LoopType LoopType = LoopType.Restart;    //루프 타입
    public PathType PathType = PathType.Linear;     
    public float Speed = 10.0f;                     //이동 속도    

    void Start()
    {
        if (WayPointRoot != null)
        {
            List<Vector3> wayPoints = new List<Vector3>();
            for (int i = 0; i < WayPointRoot.transform.childCount; ++i)
            {
                wayPoints.Add(WayPointRoot.transform.GetChild(i).position);
            }

            bool yoyo = LoopType == LoopType.Yoyo ? true : false;
            Tween t = transform.DOPath(wayPoints.ToArray(), Speed, PathType, PathMode.Full3D)
                .SetOptions(yoyo)
                .SetLookAt(0.001f);
            t.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
        else
        {
            Debug.LogError("이동할 수 있는 포인트가 없습니다. : " + this.gameObject.name);
        }
    }
}
