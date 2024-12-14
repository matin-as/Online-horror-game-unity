using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine .SceneManagement;

public class Connecttotserver : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene(1);
        base.OnJoinedLobby();
    }
}
