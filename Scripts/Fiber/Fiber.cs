using UnityEngine;
using System.Collections;

public class Fiber : IFiberWait
{
    private readonly IEnumerator fiber;
    public bool IsWait { get; private set; }

    public Fiber(IEnumerator coroutine)
    {
        this.fiber = coroutine;
        this.IsWait = true;
    }

    public void Update()
    {
        //if use wait func, should be hold on
        var wait = this.fiber.Current as IFiberWait;
        if(wait != null && wait.IsWait)
        {
            this.IsWait = true;
            return;
        }
        try
        {
            this.IsWait = this.fiber.MoveNext();
        }
        catch(System.Exception e)
        {
            Debug.Log("Fiber exception:" + e.Message);
            this.IsWait = false;
        }
    }
}

public interface IFiberWait
{
    bool IsWait { get; }
}

//Use this instead unity's WaitForSeconds, beacuse WaitForSeconds belong to unity's coroutine
public class WaitSecondsForFiber : IFiberWait
{
    private float endTime;
    public WaitSecondsForFiber(float waitTime)
    {
        this.endTime = waitTime + Time.time;
    }
    
    public bool IsWait
    {
        get
        {
            return (Time.time < this.endTime);
        }
    }
}
