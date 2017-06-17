using UnityEngine;
using System.Collections;

public class FiberTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for(int i = 1; i < 5; i++)
        {
            FiberManager.AddFiber(test1(i, i));
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private IEnumerator test1(int output, int waitTime)
    {
        Debug.Log("wait " + waitTime + " secend...");
        yield return new WaitSecondsForFiber(waitTime);
        Debug.Log("output: " + output);
    }
}
