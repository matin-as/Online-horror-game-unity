using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyc : MonoBehaviour
{
    private  float timer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
