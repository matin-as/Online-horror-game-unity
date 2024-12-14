using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class createandjoin : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject more_info;
    [SerializeField] InputField Creat_Input_Room;
    [SerializeField] InputField Join_Input_Room;
    [SerializeField] InputField NikName_Input;
    [SerializeField] GameObject space;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] GameObject back_btn;
    [SerializeField] GameObject creat_Ga;
    [SerializeField] GameObject join_Ga;
    [SerializeField] GameObject c_btn;
    [SerializeField] GameObject j_btn;
    public GameObject[] AllRoom;
    private string maxplayer = "0";
    private byte f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("hidebannedad", "No");
        dropdown.onValueChanged.AddListener(select);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Create_Room_btn()
    {
        if(GameObject.Find("ads manager").GetComponent<ads>().is_readey())
        {
            GameObject.Find("ads manager").GetComponent<ads>().show_rewarded_ad();
            if (NikName_Input.text.Length != 0)
            {
                bool is_vailed;
                Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
                if (toggle.name == "Toggle public")
                {
                    is_vailed = true;
                }
                else
                {
                    is_vailed = false;
                }
                if (maxplayer == "0")
                {
                    maxplayer = "4";
                }
                byte.TryParse(maxplayer, out f);
                PhotonNetwork.NickName = NikName_Input.text;
                PhotonNetwork.CreateRoom(Creat_Input_Room.text, new RoomOptions() { IsVisible = is_vailed, IsOpen = true, MaxPlayers = f });
            }
            else
            {
                _ShowAndroidToastMessage("set Name !");
            }
        }
        else
        {
            _ShowAndroidToastMessage("wait ...");
        }
    }
    public void Join_roon_btn()
    {
        if (NikName_Input.text.Length != 0)
        {
            PhotonNetwork.NickName = NikName_Input.text;
            PhotonNetwork.JoinRoom(Join_Input_Room.text);
        }
        else
        {
            _ShowAndroidToastMessage("set Name !");
        }
    }
    public override void OnJoinedRoom()
    {
        PlayerPrefs.SetString("hidebannedad", "yes");
        PhotonNetwork.LoadLevel(2);
        base.OnJoinedRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int y = -1;
        for(int i = 0;i<AllRoom.Length;i++)
        {
            if(AllRoom[i]!=null)
            {
                Destroy(AllRoom[i]);
            }
        }
        AllRoom = new GameObject[roomList.Count];
        foreach(RoomInfo roomInfo in roomList)
        {
            y++;
            if(roomInfo.IsOpen&&roomInfo.IsVisible&&roomInfo.PlayerCount>=1)
            {
                GameObject content = GameObject.Find("content");
                GameObject g = Instantiate(space, content.transform.transform);
                g.transform.GetChild(0).GetComponent<Text>().text = "room:" + roomInfo.Name;
                g.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate () { j_room(roomInfo.Name); });
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "players:" + roomInfo.PlayerCount.ToString();
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Max:" + roomInfo.MaxPlayers;
                AllRoom[y] = g;
            }
            else
            {

            }
        }
    }
    private void select(int index)
    {
     maxplayer = dropdown.options[index].text;
    }
    public void j_room(string room_name)
    {
        if (NikName_Input.text.Length != 0)
        {
            PhotonNetwork.NickName = NikName_Input.text;
            PhotonNetwork.JoinRoom(room_name);
        }
        else
        {
            _ShowAndroidToastMessage("set Name !");
        }
    }
    public void random_join()
    {
        if(NikName_Input.text.Length != 0)
        {
            PhotonNetwork.NickName = NikName_Input.text;
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        else
        {
            _ShowAndroidToastMessage("set Name !");
        }
    }
    public void creat()
    {
        back_btn.SetActive(true);
        creat_Ga.SetActive(true);
        join_Ga.SetActive(false);
        c_btn.SetActive(false);
        j_btn.SetActive(false);
    }
    public void join()
    {
        back_btn.SetActive(true);
        join_Ga.SetActive(true);
        creat_Ga.SetActive(false);
        c_btn.SetActive(false);
        j_btn.SetActive(false);
    }
    public void back()
    {
        c_btn.SetActive(true);
        j_btn.SetActive(true);
        back_btn.SetActive(false);
        creat_Ga.SetActive(false);
        join_Ga.SetActive(false);
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
    public void on_more_click()
    {
        if(more_info.activeInHierarchy)
        {
            more_info.SetActive(false);
        }
        else
        {
            more_info.SetActive(true);
        }
    }
    public void On_rating_btn()
    {
        Application.OpenURL("myket://comment?id=com.we.multygame");
    }
    public void On_applicationpage_btn()
    {
        Application.OpenURL("myket://details?id=com.we.multygame");
    }
    public void On_List_btn()
    {
        Application.OpenURL("myket://developer/com.we.multygame");

    }

}
