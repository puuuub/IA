using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFollowerWait : MonoBehaviour
{
    public JistPathFollower MyPathFollower;
    public float WaitTime;

    // World Position
    [SerializeField]
    Vector3 CheckPosition = new Vector3(1.401839256286621f, 2.353832960128784f, 24.507183074951173f);
    public float CheckPointTime;
    public float CheckPointSeconds;
    public float CurrentTrabledTime;

    protected abstract void InitAction();

    public void InitPathFollower(float speed)
    {
        MyPathFollower = GetComponent<JistPathFollower>();
        if (MyPathFollower != null)
        {
            MyPathFollower.enabled = (false);
            MyPathFollower.speed = speed;
        }

    }

    public void SetActiveChildren(bool isOn)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform childTrans = transform.GetChild(i);
            childTrans.gameObject.SetActive(isOn);
        }
    }

    public void StartMyCoroutine()
    {
        StartCoroutine("WaitAndPathActivate");
    }

    virtual public IEnumerator WaitAndPathActivate()
    {
        yield return new WaitForSeconds(WaitTime);
        MyPathFollower.enabled = (true);
        print("WaitAndPathActivate " + Time.time);
    }


    virtual protected bool CheckArrivedEnd()
    {
        float time = MyPathFollower.pathCreator.path.GetClosestTimeOnPath(MyPathFollower.gameObject.transform.position);
        if (time < 1.0f)
        {
            return false;
        }
        DestroyImmediate(MyPathFollower.gameObject);
        //BallotPaperManager.Instance.CreateBallotPaper();
        return true;
    }

    virtual protected void Awake()
    {
        InitAction();
    }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        CheckPointTime = MyPathFollower.pathCreator.path.GetClosestTimeOnPath(CheckPosition);
        CheckPointSeconds = MyPathFollower.GetSecondsByPositon(CheckPosition);
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        CheckArrivedEnd();
    }
}
