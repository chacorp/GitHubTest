using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
#if VR_MODE
    [SerializeField] OVRInput.Axis2D iThumbstick_L;
    [SerializeField] CharacterController cc;
    [SerializeField] float moveSpeed = 2.5f;

    void Start()
    {
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        DirectMove();

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            Debug.Log("OVRINPUT is GetDowned to change height");
            if (cc.height == 0.5f) cc.height = 1.3f;
            else if (cc.height == 1.3f) cc.height = 0.5f;
        }


    }

    void DirectMove()
    {

        Vector2 contPos = OVRInput.Get(iThumbstick_L);

        float h = contPos.x;
        float v = contPos.y;

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);

        dir.y = 0;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
#endif
}
