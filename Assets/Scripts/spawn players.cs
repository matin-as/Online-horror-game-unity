using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class spawnplayers : MonoBehaviour
{
    [SerializeField] GameObject player;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 random_pos = new Vector2(Random.Range(minX,maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(player.name,random_pos,Quaternion.identity);
     //   player.GetComponent<player>().Joystick = player_Convas.transform.GetChild(0).GetComponent<FixedJoystick>();
      //  player.GetComponent<player>().Joystick_Camera = player_Convas.transform.GetChild(1).GetComponent<FixedJoystick>();

        //  joystick_player.transform.parent = player_Convas.transform;
        //  joystick_player.transform.SetParent(player_Convas.transform);
        //   joystick_Camera.transform.SetParent(player_Convas.transform);
        //  joystick_Camera.transform.parent = player_Convas.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
