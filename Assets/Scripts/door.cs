using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class door : MonoBehaviour
{
    private Animator animator;
    private PhotonView photonView;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(gameObject.tag!= "drawer"||gameObject.tag!="tir")
        {
            animator = GetComponent<Animator>();
        }
        boxCollider = GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void call_rpc()
    {
        photonView.RPC("opendoor", RpcTarget.All);
    }
    public void call_rpc_D()
    {
        photonView.RPC("drawer", RpcTarget.All);
    }
    public void call_rpc_mag()
    {
        photonView.RPC("mag", RpcTarget.All);
    }
    [PunRPC]
    void opendoor()
    {
        animator.SetTrigger("opendoor");
        boxCollider.isTrigger = true;
    }
    [PunRPC]
    void drawer()
    {
        if (gameObject.transform.localPosition.x == 0)
        {
           gameObject.transform.localPosition = new Vector3(-40, 0);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(0, 0);
        }
    }
    [PunRPC]
    void mag()
    {
        Destroy(gameObject);
    }
}
