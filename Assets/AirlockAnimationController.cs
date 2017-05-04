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

    private bool openDoor = false, closeDoor = false;

    public UnityEvent animationFinished = new UnityEvent();

    private bool eventIgnore = true;
    
	//Currentbug:
	//Seems to be firing on rotation over and over
	//Could be related to start button or invokes here

    void Start()
    {
        locked = transform.GetComponent<DoorMaster>().locked;
        animator = GetComponent<Animator>();
        if(currentState == DoorState.Open)
        {
            OpenDoor();
            eventInvoked = false;
        }
        StartCoroutine(HandleAnimations());
    }

    void Update()
    {
        locked = transform.GetComponent<DoorMaster>().locked;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //The normalized time represents how many complete loops of this animation has occured. Above 1 means it has completed.
        /*
        if(stateInfo.normalizedTime > 1)
        {
            isAnimating = false;
            if (!eventInvoked && !eventIgnore)
            {
                Debug.Log("Shouldn't happen but it did. " + transform.parent.name);
                animationFinished.Invoke();
                eventInvoked = true;
            }
            else if (eventIgnore)
            {
                Debug.Log("Ignoring event...");
                eventInvoked = true;
                eventIgnore = false;
            }
        }
        else
        {
            Debug.Log("Resetting...");
            isAnimating = true;
            eventInvoked = false;
        }*/

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
        eventIgnore = true;
        openDoor = true;
        /*
        if (!isAnimating)
        {
            Debug.Log("Open door ignoring event. " + transform.parent.name);
            animator.Play("OpenDoor");
            currentState = DoorState.Open;
            eventIgnore = true;
        }*/
    }

    //Handles animations
    public IEnumerator HandleAnimations() 
    {
        while (true) {
            if (openDoor) {
                animator.Play("OpenDoor");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 2f);
                if (!eventIgnore) {
                    animationFinished.Invoke();
                }
                eventIgnore = false;
                openDoor = false;
            }
            else if (closeDoor) {
                animator.Play("CloseDoor");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 2f);
                if (!eventIgnore) {
                    animationFinished.Invoke();
                }
                eventIgnore = false;
                closeDoor = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    //Tries to open the door. If the door is currently opening or closing don't start another animation.
    //Return false if we can't play, true if either we are opening the door or it is already open.
    public void OpenDoor()
    {
        openDoor = true;
        /*
        if(!isAnimating&& !locked )
        {
            Debug.Log("Opening door with event. " + transform.parent.name);
            animator.Play("OpenDoor");
            currentState = DoorState.Open;
        }*/
    }

    //Tries to closethe door. If the door is currently opening or closing don't start another animation.
    //Return false if we can't play, true if either we are closi0ng the door or it is already open.
    public void CloseDoor()
    {
        closeDoor = true;
        /*
        if (!isAnimating)
        {
            animator.Play("CloseDoor");
            currentState = DoorState.Closed;
        }*/
    }


    public void CloseDoorIgnoreEvent()
    {
        eventIgnore = true;
        closeDoor = true;
        /*
        if (!isAnimating)
        {
            animator.Play("CloseDoor");
            currentState = DoorState.Closed;
            eventIgnore = true;
        }*/
    }
}
