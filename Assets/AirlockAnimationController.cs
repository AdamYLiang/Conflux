using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AirlockAnimationController : MonoBehaviour {
    
    
    Animator animator;
    AnimatorStateInfo stateInfo;
    public bool isAnimating = false;

    public enum DoorState { Closed, Open };
    public DoorState currentState = DoorState.Closed;

    public bool use = false;
    private bool locked;
    private bool eventInvoked = true;
    private bool startAnimate = false;
    public UnityEvent animationFinished = new UnityEvent();

    private bool eventIgnore = true;
    

    void Start()
    {
        locked = transform.parent.GetComponent<DoorMaster>().locked;
        animator = GetComponent<Animator>();
        if(currentState == DoorState.Open)
        {
            OpenDoor();
            eventInvoked = false;
        }
    }

    void Update()
    {
        //locked = transform.parent.GetComponent<DoorMaster>().locked;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //The normalized time represents how many complete loops of this animation has occured. Above 1 means it has completed.
        if(stateInfo.normalizedTime > 1)
        {
            isAnimating = false;
            if (!eventInvoked && !eventIgnore)
            {
                animationFinished.Invoke();
                eventInvoked = true;
            }
            else if (eventIgnore)
            {
                eventInvoked = true;
                eventIgnore = false;
            }
        }
        else
        {
            isAnimating = true;
            eventInvoked = false;
        }
    }


    public void Use()
    {
        
        if (currentState == DoorState.Closed)
        {
            OpenDoor();
        }
        else if (currentState == DoorState.Open)
        {
            CloseDoor();
        }
    }

    
    public void OpenDoorIgnoreEvent()
    {
        if (!isAnimating)
        {
            animator.Play("OpenDoor");
            currentState = DoorState.Open;
            eventIgnore = true;
        }
    }

    //Tries to open the door. If the door is currently opening or closing don't start another animation.
    //Return false if we can't play, true if either we are opening the door or it is already open.
    public void OpenDoor()
    {
        if(!isAnimating&& !locked )
        {
            animator.Play("OpenDoor");
            currentState = DoorState.Open;
        }
    }

    //Tries to closethe door. If the door is currently opening or closing don't start another animation.
    //Return false if we can't play, true if either we are closi0ng the door or it is already open.
    public void CloseDoor()
    {
        if (!isAnimating)
        {
            animator.Play("CloseDoor");
            currentState = DoorState.Closed;
        }
    }


    public void CloseDoorIgnoreEvent()
    {
        if (!isAnimating)
        {
            animator.Play("CloseDoor");
            currentState = DoorState.Closed;
            eventIgnore = true;
        }
    }
}
