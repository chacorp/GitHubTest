using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardSway : MonoBehaviour
{
    [SerializeField] Animator clipBoard;
    [SerializeField] CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
        {
            clipBoard.SetBool("IsMoving", true);
        }
        else if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") == 0.0f)
        {
            clipBoard.SetBool("IsMoving", false);
        }
    }
}
