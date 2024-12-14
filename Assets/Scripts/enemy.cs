using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using random = UnityEngine.Random;
using Photon.Pun;
public class enemy : MonoBehaviour
{
    public int hurt = 3;
    private float timer = 20;
    private NavMeshAgent navMeshAgent;
    private GameObject target;
    private GameObject[] players;
    private PhotonView photonView;
    bool is_donbal;
    string f;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name=="e")
        {
            f = "b";
        }
        else
        {
            f = "a";
        }
        photonView = GetComponent<PhotonView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectsWithTag(f)[random.Range(0, GameObject.FindGameObjectsWithTag(f).Length)];
    }

    // Update is called once per frame
    void Update()
    {
        call_rpc_handle();
    }
    void call_rpc_handle()
    {
        photonView.RPC("handle",RpcTarget.All);
    }
    [PunRPC]
    void handle()
    {
        if(hurt<=0)
        {
            if(timer>=0)
            {
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                timer -= Time.deltaTime;
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().isTrigger = false;
                hurt = 5;
                timer = 20;
            }
        }
        else
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                float fasele = Vector3.Distance(p.transform.position, transform.position);
                if (fasele <= 7 && p.GetComponent<player>().hid == false)
                {
                    if (fasele < 0.9)
                    {
                        p.GetComponent<player>().lose = true;
                    }
                    else
                    {
                        is_donbal = true;
                        navMeshAgent.SetDestination(p.transform.position);
                    }
                }
                else
                {
                    is_donbal = false;
                }

            }
            if (is_donbal == false)
            {
                if (transform.position.x == target.transform.position.x && transform.position.z == target.transform.position.z)
                {
                    target = GameObject.FindGameObjectsWithTag(f)[random.Range(0, GameObject.FindGameObjectsWithTag(f).Length)];
                }
                navMeshAgent.SetDestination(target.transform.position);
            }
        }
    }
}
