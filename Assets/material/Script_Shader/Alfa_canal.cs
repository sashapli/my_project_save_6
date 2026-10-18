using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alfa_canal : MonoBehaviour
{
    public float tran;
    public float tran2;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tran = Input.GetAxis("Horizontal");
        tran2 = Input.GetAxis("HorizontalShader");
        material.SetFloat("_Tran", tran > 0 ? tran : tran2);

    }
}
