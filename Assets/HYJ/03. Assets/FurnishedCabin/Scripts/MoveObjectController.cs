using UnityEngine;
using System.Collections;

public class MoveObjectController : MonoBehaviour
{
    float reachRange;

    private Animator anim;
    private Camera fpsCam;
    private GameObject player;

    private const string animBoolName = "isOpen_Obj_";

    private bool playerEntered;
    private bool showInteractMsg;
    private GUIStyle guiStyle;
    private string msg;

    private int rayLayerMask;


    void Start()
    {
        // 상호 작용을 할 수 있는 최대 거리
        reachRange = CSH_RayManager.Instance.distanceLimit;

        //Initialize moveDrawController if script is enabled.
        // Xesco 게임엔 딱히 쓸모 없는 moveDrawController.cs를 가져오기 위한 것
        player = CSH_RayManager.Instance.player;

        fpsCam = Camera.main;
        if (fpsCam == null) //a reference to Camera is required for rayasts
        {
            Debug.LogError("A camera tagged 'MainCamera' is missing.");
        }

        //create AnimatorOverrideController to re-use animationController for sliding draws.
        // 애니메이션 컴포넌트 가져오기
        anim = GetComponent<Animator>();
        anim.enabled = true;  //disable animation states by default.  

        //the layer used to mask raycast for interactable objects only
        // 움직이는 애니메이션이 붙어있는 오브젝트만 검출할 수 있는 레이어
        rayLayerMask = 1 << LayerMask.NameToLayer("InteractRaycast");

        //setup GUI style settings for user prompts
        setupGui();

    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Finish"))     //player has collided with trigger
        {
            playerEntered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))     //player has exited trigger
        {
            playerEntered = false;
            //hide interact message as player may not have been looking at object when they left
            showInteractMsg = false;
        }
    }



    void Update()
    {
        if (playerEntered)
        {
            //center point of viewport in World space.
            // 카메라 뷰포트의 중앙 위치 가져오기
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            //if raycast hits a collider on the rayLayerMask
            // 1. 뷰포트 중앙에서 카메라의 정면 방향으로
            // 2. reachRange만큼
            // 3. Ray를 쏴서
            // 4. rayLayerMask 검출하기
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, reachRange, rayLayerMask))
            {
                MoveableObject moveableObject = null;
                //is the object of the collider player is looking at the same as me?
                if (!isEqualToParent(hit.collider, out moveableObject))
                {   //it's not so return;
                    return;
                }

                if (moveableObject != null)     //hit object must have MoveableDraw script attached
                {
                    showInteractMsg = true;
                    string animBoolNameNum = animBoolName + moveableObject.objectNumber.ToString();

                    bool isOpen = anim.GetBool(animBoolNameNum);    //need current state for message.
                    msg = getGuiMsg(isOpen);
#if EDITOR_MODE
                    if (Input.GetKeyUp(KeyCode.E))
#elif VR_MODE
                    if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
#endif
                    {
                        anim.SetBool(animBoolNameNum, !isOpen);
                        msg = getGuiMsg(!isOpen);
                    }

                }
            }
            else
            {
                showInteractMsg = false;
            }
        }

    }

    //is current gameObject equal to the gameObject of other.  check its parents
    private bool isEqualToParent(Collider other, out MoveableObject draw)
    {
        draw = null;
        bool rtnVal = false;
        try
        {
            int maxWalk = 6;
            draw = other.GetComponent<MoveableObject>();

            GameObject currentGO = other.gameObject;
            for (int i = 0; i < maxWalk; i++)
            {
                if (currentGO.Equals(this.gameObject))
                {
                    rtnVal = true;
                    if (draw == null) draw = currentGO.GetComponentInParent<MoveableObject>();
                    break;          //exit loop early.
                }

                //not equal to if reached this far in loop. move to parent if exists.
                if (currentGO.transform.parent != null)     //is there a parent
                {
                    currentGO = currentGO.transform.parent.gameObject;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        return rtnVal;

    }


    #region GUI Config

    //configure the style of the GUI
    private void setupGui()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 30;
        guiStyle.fontStyle = FontStyle.Bold;
        guiStyle.normal.textColor = Color.white;
        msg = "E 버튼 누르기";
    }

    private string getGuiMsg(bool isOpen)
    {
        string rtnVal;
        if (isOpen)
        {
            rtnVal = "E 버튼 눌러서 닫기";
        }
        else
        {
            rtnVal = "E 버튼 눌러서 열기";
        }

        return rtnVal;
    }

    void OnGUI()
    {
        if (showInteractMsg)  //show on-screen prompts to user for guide.
        {
            GUI.Label(new Rect(800, Screen.height - 120, 200, 50), msg, guiStyle);
        }
    }
    //End of GUI Config --------------
    #endregion
}
