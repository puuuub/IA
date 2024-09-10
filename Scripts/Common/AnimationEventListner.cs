using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyIntEvent : UnityEvent<float>
{
}

public class AnimationEventListner : MonoBehaviour
{
    [SerializeField] UnityEvent[] Events;
    [SerializeField] MyIntEvent myEvent;

    [SerializeField] Animator animator;

    public void EventCall(int index)
    {
        if(index >= 0 && index < Events.Length)
        {
            Events[index].Invoke();
        }
        else
        {
            Debug.LogError("Index out of range");
        }
    }

    public void MyEventCall(float val)
    {
        myEvent.Invoke(val);
    }

    public void SetSpeed(float speed)
    {
        if(animator != null)
            animator.speed = speed;
    }

    public void SetRandomSpeed()
    {
        animator.speed = Random.Range(0.05f, 0.2f);
    }

    public void StopAnimation()
    {
        animator.enabled = false;
        //animator.StopPlayback();
        print("stop");
    }

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
                // 아이콘등 애니메이션 속도를 관리 하지 말아야하는 오브젝트
                if (animator.transform.parent.name == "NoAniControl")
                    animator = null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
