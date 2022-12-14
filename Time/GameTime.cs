using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public int second;
    
    private void Start()
    {
        StartCoroutine(Time());
    }

    private IEnumerator Time()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            second++;
        }
    }
}
