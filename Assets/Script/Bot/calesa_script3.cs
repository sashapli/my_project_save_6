using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calesa_script3 : MonoBehaviour
{
    public BotController3 Ai;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Ai.targetAngle);
    }
}
