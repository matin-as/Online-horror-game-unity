using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    enemy e;
    [SerializeField] GameObject lose_convas;
    public bool lose;
    [SerializeField] GameObject btn_p;
    [SerializeField] GameObject btn_k1;
    // for iteams
    [SerializeField] GameObject Back_pack_Convas;
    [SerializeField] Text tir_pistol_text;
    public int tir_pistol;

    // for wepens
    [SerializeField] GameObject jash;
    [SerializeField] GameObject flash_lith;
    [SerializeField] GameObject Shoot_btn;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject key;
    private float fire_rate = 15f;
    private float next_time_to_fire = 0f;
    // for game in online
    PhotonView _photonView;
    private GameObject _camera;
    private GameObject _camera2;
    private GameObject player_convas;
    private Animator anim;
    Rigidbody rd;
    private Vector3 direction;
    private bool is_run;
    private bool is_gun;
    // for move in mobile
    public  FixedJoystick Joystick;
    public  FixedJoystick Joystick_Camera;
    public  Camera main_cam;
    public GameObject map_convas;
    public bool is_player_hith;
    public bool has_pistol;
    // for move and jump
    private Vector3 firstperson_view_Rotation = Vector3.zero;
    public float jumpSpeed = 8.0f;
    public float gravity = 10f;
    private float speed;
    private bool is_moving;
    private bool is_grounded;
    private float inputx;
    private float inputy;
    private float input_set_x;
    private float input_set_y;
    private float timeRemaining = 2;
    private int c;
    private float inputmodifyfactor;
    private bool limitspeed = true;
    private bool is_lose;
    private float antiBumpfactor;
    public CharacterController char_Contoroler;
    private Vector3 moveDirection = Vector3.zero;
    public bool hid;
    GameObject b;
    GameObject b2;
    bool has_key1;
    bool has_key2;

    // for move and jump
    // animate player



    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
        _photonView = GetComponent<PhotonView>();
        char_Contoroler = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        _camera = transform.GetChild(0).transform.GetChild(5).gameObject;
        _camera2 = transform.GetChild(1).gameObject;
        player_convas = transform.GetChild(0).transform.GetChild(6).gameObject;
        transform.GetChild(0).transform.GetChild(11).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _photonView.Controller.NickName;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(_photonView.IsMine)
        {
            handle_lsoe();
            _camera.SetActive(true);
            player_convas.SetActive(true);
            movment_Control();
            control_wapens();
            control_anim();
            control_Back_pack();
            
        }
        else
        {
            _camera2.SetActive(false);
            _camera.SetActive(false);
            player_convas.SetActive(false);
        }
    }
    private void movment_Control()
    {
        if (is_player_hith == false)
        {
            direction = Joystick.Horizontal * 10 * transform.right + Joystick.Vertical * 10 * transform.forward;
            direction = direction * Mathf.Min(1f, direction.magnitude);


            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    input_set_y = 1f;
                }
                else
                {
                    input_set_y = -1f;
                }

            }
            else
            {
                input_set_y = 0f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    input_set_x = -1f;
                }
                else
                {
                    input_set_x = 1f;
                }

            }
            else
            {
                input_set_x = 0f;
            }
            inputy = Mathf.Lerp(inputy, input_set_y, Time.deltaTime * 19f);
            inputx = Mathf.Lerp(inputx, input_set_x, Time.deltaTime * 19f);
            inputmodifyfactor = Mathf.Lerp(inputmodifyfactor,
                (input_set_x != 0 && input_set_y != 0 && limitspeed) ? 0.75f : 1.0f,
                Time.deltaTime * 19f);

            firstperson_view_Rotation = Vector3.Lerp(firstperson_view_Rotation,
               Vector3.zero, Time.deltaTime * 5f);

            if (is_grounded)
            {

                moveDirection = new Vector3(inputx * inputmodifyfactor, -antiBumpfactor,
                    inputy * inputmodifyfactor);
                moveDirection = transform.TransformDirection(moveDirection) * speed;

            }
            moveDirection.y -= gravity * Time.deltaTime;
            is_grounded = (char_Contoroler.Move(direction * Time.deltaTime) & CollisionFlags.Below) != 0;
           // is_grounded = (char_Contoroler.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
            is_moving = char_Contoroler.velocity.magnitude > 0.15f;

        }
    }
    private void control_wapens()
    {
        if(Input.GetMouseButtonDown(0))
        {
           if(!hid)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitC;
                if (Physics.Raycast(ray, out hitC))
                {
                    if(hitC.transform.gameObject.name=="key1")
                    {
                        has_key1 = true;
                        btn_k1.SetActive(true);
                        is_gun = true;
                        _photonView.RPC("show_key", RpcTarget.All);
                        hitC.transform.GetComponent<door>().call_rpc_mag();
                    }
                    if (hitC.transform.gameObject.name == "key2")
                    {
                        has_key2 = true;
                        btn_k1.SetActive(true);
                        is_gun = true;
                        _photonView.RPC("show_key", RpcTarget.All);
                        hitC.transform.GetComponent<door>().call_rpc_mag();
                    }
                    if (hitC.transform.gameObject.tag== "drawer")
                    {
                        hitC.transform.GetComponent<door>().call_rpc_D();
                    }
                    if(hitC.transform.gameObject.tag == "door")
                    {
                        if(has_key1)
                        {
                            _photonView.RPC("show_key1", RpcTarget.All);
                            is_gun = false;
                            hitC.transform.gameObject.GetComponent<door>().call_rpc();
                        }
                        else
                        {
                            _ShowAndroidToastMessage("Need key !");
                        }
                        print("this is door");
                    }
                    if(hitC.transform.gameObject.tag == "doorend")
                    {
                        if(has_key2)
                        {
                            foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
                            {
                                p.GetComponent<player>().win();
                            }
                            print("Win");
                        }
                        else
                        {
                            _ShowAndroidToastMessage("Need key ! ");
                        }
                    }
                    if (hitC.transform.gameObject.tag == "bed")
                    {
                        if (!hid)
                        {
                            if(_photonView.IsMine)
                            {
                                _camera.SetActive(false);
                                b = hitC.transform.GetChild(3).gameObject;
                                b2 = hitC.transform.GetChild(4).gameObject;
                                b.SetActive(true);
                                b2.SetActive(true);
                                _photonView.RPC("hide", RpcTarget.All, false);
                                hid = true;
                            }
                        }
                    }
                    if (hitC.transform.tag == "pistol")
                    {
                        btn_p.SetActive(true);
                        Destroy(hitC.transform.gameObject);
                        is_gun = true;
                        Shoot_btn.SetActive(true);
                        _photonView.RPC("ShowMesh", RpcTarget.All, true);


                    }
                    if (hitC.transform.gameObject.tag == "tir")
                    {
                        if (_photonView.IsMine)
                        {
                            tir_pistol = tir_pistol + 5;
                            hitC.transform.GetComponent<door>().call_rpc_mag();
                        }

                    }
                }
            }
           else
            {
                if(_photonView.IsMine)
                {
                    // hide the player
                    _camera.SetActive(true);
                    b.SetActive(false);
                    b2.SetActive(false);
                    _photonView.RPC("hide", RpcTarget.All, true);
                    hid = false;
                }
            }
        }
    }
    private void control_anim()
    {
        if (direction != new Vector3(0, 0))
        {
            is_run = true;
        }
        else
        {
            is_run = false;
        }
        if(lose ==false)
        {
            if (is_run && is_gun == false)
            {
                anim.SetBool("run", true);
            }
            if (is_run == false && is_gun == false)
            {
                anim.SetBool("run", false);
            }
            if (is_run && is_gun)
            {
                anim.SetBool("have gun and run", true);
                anim.SetBool("have gun", false);
            }
            if (is_gun && is_run == false)
            {
                anim.SetBool("have gun and run", false);
                anim.SetBool("have gun", true);
            }
        }
    }
    public  void shout()
    {
        if(_photonView.IsMine)
        {
            if (tir_pistol > 0)
            {
                tir_pistol = tir_pistol - 1;
                next_time_to_fire = Time.time + 1f / fire_rate;
                RaycastHit hit;
                if (Physics.Raycast(main_cam.transform.position, main_cam.transform.forward, out hit))
                {
                    if(hit.transform.tag=="enemy")
                    {
                        e = hit.transform.GetComponent<enemy>();
                        _photonView.RPC("take_damage", RpcTarget.All);
                    }
                    print(hit.transform.gameObject.name);
                    PhotonNetwork.Instantiate(jash.name, hit.point, Quaternion.LookRotation(hit.normal));

                }

                StartCoroutine(TurnOn_muzzle_flash());
            }
        }
    }
    IEnumerator TurnOn_muzzle_flash()
    {
        flash_lith.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flash_lith.SetActive(false);
    }
    public void Back_pack_btn()
    {
        if(Back_pack_Convas.activeInHierarchy)
        {
            Back_pack_Convas.SetActive(false);
        }
        else
        {
            Back_pack_Convas.SetActive(true);
        }
    }
    private void control_Back_pack()
    {
        tir_pistol_text.text = "Your pistol tir : " + tir_pistol;

    }
    [PunRPC]
    void ShowMesh(bool c)
    {
        pistol.SetActive(c);
        key.SetActive(false);
        
    }
    [PunRPC]
    void hide(bool c)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(c);

    }
    [PunRPC]
    void show_key()
    {
        pistol.SetActive(false);
        key.SetActive(true);
    }
    [PunRPC]
    void take_damage()
    {
        e.transform.GetComponent<enemy>().hurt = e.transform.GetComponent<enemy>().hurt-1;
    }
    [PunRPC]
    void show_key1()
    {
        key.SetActive(false);
    }

    public void sw()
    {
        if(_photonView.IsMine)
        {
            Shoot_btn.SetActive(false);
            _photonView.RPC("show_key", RpcTarget.All);
        }
    }
    public void sw_2()
    {
        if (_photonView.IsMine)
        {
            Shoot_btn.SetActive(true);
            _photonView.RPC("ShowMesh", RpcTarget.All, true);
        }
    }
    private void _ShowAndroidToastMessage(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        else
        {
            print(message);
        }
    }
    public void handle_lsoe()
    {
        if(lose)
        {
            _photonView.RPC("hide", RpcTarget.All, false);
            hid = true;
            lose_convas.SetActive(true);
            player_convas.SetActive(false);
            _camera2.SetActive(true);
        }
    }
    public void back()
    {
        if(_photonView.IsMine)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(1);
        }
    }
    public void Watch_AD()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if (_photonView.IsMine)
            {
                if (transform.GetChild(3).GetComponent<ads2>().is_readey())
                {
                    transform.GetChild(3).GetComponent<ads2>().show_rewarded_ad();
                }
                else
                {
                    _ShowAndroidToastMessage("wait ...");
                }
            }
        }
        else
        {
            cont();
        }
    }
    public void cont()
    {
        if(_photonView.IsMine)
        {
            lose = false;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            _photonView.RPC("hide", RpcTarget.All,true);
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
            hid = false;
            lose_convas.SetActive(false);
            player_convas.SetActive(true);
            _camera2.SetActive(false);
        }
    }
    public void win()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);
    }

}
