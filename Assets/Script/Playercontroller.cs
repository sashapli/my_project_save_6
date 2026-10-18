using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    [Header("controll")]
    public float moveSpeed = 0;
    public float moveRotation = 0;
    public float moveRotationSpeed = 0;
    public float targetRotation = 1;
    public float maxSpeed = 5;
    public float Drift = 0.95f;
    public bool isRotation = false;
    public bool isdrift = false;
    public bool isYpr;
    float angler = 0;

    [Header("Joystic")]
    public Joystick _joy;

    public Rigidbody2D _rd;
    public Vector2 MoveInput;
    public WBGL_derect wBGL_Derect;

    private void Start()
    {
        _rd = GetComponent<Rigidbody2D>();
        switch (SaveMenager.Instance.GetCar())
        {
            case 1: maxSpeed = 10; break;
            case 2: maxSpeed = 10; break;
            case 3:maxSpeed = 20; break;
        }
    }

    void Update()
    {
        if (wBGL_Derect.IsMobileDevice)
        {
            MoveInput = new Vector2(_joy.Horizontal, _joy.Vertical);
        }
        else
        {
                MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        if (Input.GetKey(KeyCode.LeftShift) || isdrift == true)
        {
            Drift = 1;
            _rd.drag = 1;
        }
        else
        {
            Drift = 0.97f;
            _rd.drag = 0;
        }
   }

    void FixedUpdate()
    {
        if (MoveInput.magnitude > 0.1f)
        {
            move();
            rotation();
        }
        Drift0();
        target();
    }

    void move()
    {
        if (isYpr)
        {
            Vector2 Moveforse = MoveInput.normalized * moveSpeed;
            _rd.AddForce(Moveforse, ForceMode2D.Force);


            if (_rd.velocity.magnitude > maxSpeed)
            {
                _rd.velocity = _rd.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            Vector2 lookMowe = Quaternion.Euler(0, 0, angler) * new Vector2(0, MoveInput.y) * moveSpeed;
            _rd.AddForce(lookMowe, ForceMode2D.Force);
            if (_rd.velocity.magnitude > maxSpeed)
            {
                _rd.velocity = _rd.velocity.normalized * maxSpeed;
            }
        }
    }
    
    void rotation()
    {
        if (isYpr)
        {
            float angler = Mathf.Atan2(MoveInput.y, MoveInput.x) * Mathf.Rad2Deg - 90;
            float target = Mathf.LerpAngle(_rd.rotation, angler, moveRotation * Time.fixedDeltaTime);
            _rd.MoveRotation(target);
        }
        else
        {
            angler += -MoveInput.x * moveRotationSpeed;
            float target = Mathf.LerpAngle(_rd.rotation, angler, moveRotation * Time.fixedDeltaTime);
            _rd.MoveRotation(target);
        }
    }

    void Drift0()
    {
        Vector2 up = transform.up * Vector2.Dot(_rd.velocity, transform.up);
        Vector2 right = transform.right* Vector2.Dot(_rd.velocity, transform.right);
        _rd.velocity = up + right * Drift;
    }

    void target()
    {
        if (isRotation == true)
        {
            if (MoveInput.magnitude >= 0.1f)
            {
                if (MoveInput.magnitude <= 0.3f)
                {
                    moveRotation = targetRotation / MoveInput.magnitude;
                }
                else
                {
                    moveRotation = 3;
                }
            }
            else
            {
                moveRotation = 0;
            }
        }
        else
        {
            moveRotation = 0;
        }
    }

    public void play2()
    {
        moveSpeed = 7;
        isRotation = true;
    }

    public void DriftClickT()
    {
        isdrift = true;
    }
    public void DriftClickF()
    {
        isdrift = false;
    }







}
