using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using random = UnityEngine.Random;

public class drawer : MonoBehaviour
{
   
    private void Start()
    {
        if(random.Range(0,2)!=1)
        {
            transform.GetChild(19).transform.GetChild(0).GetComponent<door>().call_rpc_mag();//gun
            // delete mag 
        }
        else
        {
            if(random.Range(0,1)==0)
            {
                transform.GetChild(12).transform.GetChild(0).GetComponent<door>().call_rpc_mag();//mag
            }
        }
    }
    private void Update()
    {
        
    }
}
