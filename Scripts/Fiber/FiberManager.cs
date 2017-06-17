using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FiberManager : MonoBehaviour
{
    private static Queue<Fiber> fiberQueue = new Queue<Fiber>();

    public static void AddFiber(IEnumerator coroutine)
    {
        var fiber = new Fiber(coroutine);
        fiberQueue.Enqueue(fiber);
    }

    private void Update()
    {
        lock(fiberQueue)
        {
            if (fiberQueue.Count == 0)
                return;
            var fiber = fiberQueue.Peek();
            if(fiber != null)
            {
                fiber.Update();
                if (!fiber.IsWait)
                    fiberQueue.Dequeue();
            }
        }
    }

}
