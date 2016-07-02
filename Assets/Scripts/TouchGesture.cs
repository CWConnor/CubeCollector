using UnityEngine;
using System.Collections;
using System;

public class TouchGesture : MonoBehaviour
{
    public float minSwipeDist = 100f;
    public float maxSwipeTime = 10f;
    private float swipeStartTime;
    private bool couldBeSwipe;
    private Vector2 startPos;
    private int stationaryForFrames;
    private TouchPhase lastPhase;

    public IEnumerator CheckHorizontalSwipes(Action onLeftSwipe, Action onRightSwipe)
    {
        while (true)
        { //Loop. Otherwise we wouldnt check continuously ;-)
            foreach (Touch touch in Input.touches)
            { //For every touch in the Input.touches - array...
                switch (touch.phase)
                {
                    case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
                        couldBeSwipe = true;
                        startPos = touch.position;  //Position where the touch started
                        swipeStartTime = Time.time; //The time it started
                        stationaryForFrames = 0;
                        break;
                    case TouchPhase.Stationary: //Is the touch stationary? --> No swipe then!
                        if (isContinouslyStationary(frames: 6))
                            couldBeSwipe = false;
                        break;
                    case TouchPhase.Ended:
                        if (IsAHorizontalSwipe(touch))
                        {
                            couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.
                            if (Mathf.Sign(touch.position.x - startPos.x) == 1f) //Swipe-direction, either 1 or -1.   
                                onRightSwipe(); //Right-swipe
                            else
                                onLeftSwipe(); //Left-swipe
                        }
                        break;
                }
                lastPhase = touch.phase;
            }
            yield return null;
        }
    }

    public IEnumerator CheckVerticalSwipes(Action onUpSwipe, Action onDownSwipe)
    {
        while (true)
        { //Loop. Otherwise we wouldnt check continuously ;-)
            foreach (Touch touch in Input.touches)
            { //For every touch in the Input.touches - array...
                switch (touch.phase)
                {
                    case TouchPhase.Began: //The finger first touched the screen --> It could be(come) a swipe
                        couldBeSwipe = true;
                        startPos = touch.position;  //Position where the touch started
                        swipeStartTime = Time.time; //The time it started
                        stationaryForFrames = 0;
                        break;
                    case TouchPhase.Stationary: //Is the touch stationary? --> No swipe then!
                        if (isContinouslyStationary(frames: 6))
                            couldBeSwipe = false;
                        break;
                    case TouchPhase.Ended:
                        if (IsAVerticalSwipe(touch))
                        {
                            couldBeSwipe = false; //<-- Otherwise this part would be called over and over again.
                            if (Mathf.Sign(touch.position.y - startPos.y) == 1f)
                                onUpSwipe(); //Swipe up.
                            else
                                onDownSwipe(); //Swipe down.
                        }
                        break;
                }
                lastPhase = touch.phase;
            }
            yield return null;
        }
    }

    private bool isContinouslyStationary(int frames)
    {
        if (lastPhase == TouchPhase.Stationary)
            stationaryForFrames++;
        else // reset back to 1
            stationaryForFrames = 1;
        return stationaryForFrames > frames;
    }

    private bool IsAHorizontalSwipe(Touch touch)
    {
        float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
        float swipeDist = Mathf.Abs(touch.position.x - startPos.x); //Swipe distance
        return couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist;
    }

    private bool IsAVerticalSwipe(Touch touch)
    {
        float swipeTime = Time.time - swipeStartTime; //Time the touch stayed at the screen till now.
        float swipeDist = Mathf.Abs(touch.position.y - startPos.y); //Swipe distance
        return couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}