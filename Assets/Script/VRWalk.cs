using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWalk : MonoBehaviour
{
    public Transform vrCamera;
    public float angle = 30.0f;
    public float speed = 3.0f;
    public bool move;

    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vrCamera.eulerAngles.x >= angle && vrCamera.eulerAngles.x <60.0f)
        {
            move = true;
        }
        else
        {
            move = false;
        }

        if (move)
        {
            Vector3 direction = vrCamera.TransformDirection(Vector3.forward);
            cc.SimpleMove(direction*speed);
        }

    }
}
