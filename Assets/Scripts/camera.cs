using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class camera : MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView _photonView;
    private FixedJoystick joystick;
    public enum rotationAxies { Mouse_x, Mouse_y }
    public rotationAxies axes = rotationAxies.Mouse_y;
    private float corrent_sensivity_x =3f;
    private float corrent_sensivity_y = 3f;
    private float sensivity_x;
    private float sensivity_y;
    private float rotation_x;
    private float rotation_y;
    private float minimum_x = -360f;
    private float maximum_x = 360f;
    private float minimum_y = -60f;
    private float maximum_y = 60f;
    private float mouse_sensivity = 7f;//4
    private Quaternion orginalrotation;
    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        if(_photonView.IsMine)
        {
            orginalrotation = transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate()
    {
        if(_photonView.IsMine)
        {
            handleCamera();
            handelrotation();
        }
    }
    float ClampAngel(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }
    private void handelrotation()
    {
        if (corrent_sensivity_x != mouse_sensivity || corrent_sensivity_y != mouse_sensivity)
        {
            corrent_sensivity_x = corrent_sensivity_y = mouse_sensivity;
        }
        sensivity_x = corrent_sensivity_x;
        sensivity_y = corrent_sensivity_y;
        if (axes == rotationAxies.Mouse_x)
        {
            rotation_x += joystick.Horizontal * sensivity_x;
           // rotation_x += Input.GetAxis("Mouse X") * sensivity_x;
            rotation_x = ClampAngel(rotation_x, minimum_x, maximum_x);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotation_x, Vector3.up);
            transform.localRotation = orginalrotation * xQuaternion;
        }
        if (axes == rotationAxies.Mouse_y)
        {
            rotation_y += joystick.Vertical * sensivity_y;
          //  rotation_y += Input.GetAxis("Mouse Y") * sensivity_y;
            rotation_y = ClampAngel(rotation_y, minimum_y, maximum_y);
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotation_y, Vector3.right);
            transform.localRotation = orginalrotation * yQuaternion;
        }

    }
    private void handleCamera()
    {
        joystick = GetComponentInParent<player>().Joystick_Camera;
    }
}
