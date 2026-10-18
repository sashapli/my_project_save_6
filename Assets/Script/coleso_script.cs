using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coleso_script : MonoBehaviour
{
    public float test;
    public Joystick _joy;
    public Transform _tran;
    public WBGL_derect wBGL_Derect;
    public Vector2 input;
    public Playercontroller _playercontrollerl;
    float angler;
    public float moveRotationSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (wBGL_Derect.IsMobileDevice)
        {
            input = new Vector2(_joy.Horizontal, _joy.Vertical);
        }
        else
        {
             input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        if (_playercontrollerl.isYpr)
        {
            float anglera = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, anglera);
        }

       if(input.magnitude >= 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _tran.rotation, 2 * Time.deltaTime);
        }
      // Quaternion q = Quaternion.Euler(0, 0, a);
      //  rd.MoveRotation(anglera);
    }
}
