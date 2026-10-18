using UnityEngine;

public class Camera_Script : MonoBehaviour
{
    public Transform _tansPLAYER;

    void Update() {
        transform.position = new Vector3(_tansPLAYER.position.x, _tansPLAYER.position.y, transform.position.z);
    }
}
