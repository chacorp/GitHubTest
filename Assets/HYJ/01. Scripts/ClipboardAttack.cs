using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipboardAttack : MonoBehaviour
{
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float swingTimer = 0.0f;
    [SerializeField] private bool isSwinging = false;
    [SerializeField] private Vector3 startRot;
    [SerializeField] Vector3 targetAngle = new Vector3(50.543f, -21.848f, -44.478f);
    [SerializeField] private Vector3 startPos;
    [SerializeField] Vector3 targetPos1 = new Vector3(0.063f, 0.001f, 0.13f);
    [SerializeField] Vector3 targetPos2 = new Vector3(0.063f, -0.5f, 0.13f);
    [SerializeField] AudioClip swingSound;
    [SerializeField] AudioClip[] collisionSFXs;
    [SerializeField] AudioSource clipboardAudio;
    [SerializeField] GameObject clipboard;
    [SerializeField] GameObject sparkFactory;
    [SerializeField] OVRInput.Button iTrigger_R;
    [SerializeField] Transform rightControllerAnchor;


    #region 충돌체크를 위한 변수
    Ray ray;
    RaycastHit rayhit;
    [SerializeField] float maxDistance = 100.0f;
    #endregion

    float motionRatio = 0;
    float motionRatio2 = 0;
    float motionRatio3 = 0;
    public float effectRange = 1.0f;

    void Start()
    {
        startRot = transform.localEulerAngles;
        startPos = transform.transform.localPosition;
        clipboardAudio = GetComponentInChildren<AudioSource>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
#if EDITOR_MODE
        if (Input.GetMouseButtonDown(0) && !isSwinging && clipboard.activeSelf && !QuestManager.Instance.quests[3].goal.IsReached())
#elif VR_MODE
        if (OVRInput.GetDown(iTrigger_R) && !isSwinging && clipboard.activeSelf)
#endif
        {
            isSwinging = true;

            clipboardAudio = GetComponentInChildren<AudioSource>();// ----- 추가
            clipboardAudio.PlayOneShot(swingSound);
#if EDITOR_MODE
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
#elif VR_MODE
            ray = new Ray(rightControllerAnchor.position, rightControllerAnchor.forward);
#endif
            int layer = LayerMask.NameToLayer("Player");
            layer = 1 << layer;

            ShootRay(ray, layer);
        }

        if (isSwinging)
        {
            SwingClipboard();
        }
    }

    void ShootRay(Ray ray, int layer)
    {
        //Physics.Raycast(ray, out rayhit, maxDistance, ~layer)
        if (Physics.SphereCast(ray, 0.3f, out rayhit, maxDistance, ~layer))
        {
            //Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, 3f);
            Enemy enemy = rayhit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                Debug.Log($"{rayhit.transform.gameObject.name} is attacked!! with Ray");
                enemy.OnDamageProcess();
            }
            CollisionEffect(rayhit);
        }
    }

    void CollisionEffect(RaycastHit hitinfo)
    {
        // 빛과 충돌된 물체가 있다면 그 물체와 클립보드(게임 오브젝트) 사이의 거리를 측정한다
        if (!hitinfo.transform.gameObject.tag.Contains("Enemy"))
        {
            Vector3 distance = gameObject.transform.position - hitinfo.point;
            if (distance.magnitude <= effectRange)
            {
                // 거리가 일정 범위 안으로 들어오면 SFX와 VFX를 나타내라
                int randNum = Random.Range(0, collisionSFXs.Length);

                clipboardAudio.PlayOneShot(collisionSFXs[randNum]);
                GameObject sparkEffect = Instantiate(sparkFactory);
                sparkEffect.transform.transform.up = hitinfo.normal;
                sparkEffect.transform.position = hitinfo.point;
                Debug.Log($"Clipboard is collide with {hitinfo.transform.name}");
            }
        }
    }

    //   Vector3 targetAngle = new Vector3(13.5f, 205.0f, 10.0f);


    void SwingClipboard()
    {
        swingTimer += Time.deltaTime;


        if (swingTimer < swingDuration * 0.33f)
        {
            motionRatio += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos1, motionRatio * 3 * (1.0f / swingDuration));

        }

        else if (swingTimer > swingDuration * 0.33f && swingTimer < swingDuration * 0.66f)
        {
            motionRatio2 += Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRot, targetAngle, motionRatio2 * 3 * (1.0f / swingDuration));
            transform.localPosition = Vector3.Lerp(targetPos1, targetPos2, motionRatio2 * 3 * (1.0f / swingDuration));

        }

        else if (swingTimer > swingDuration * 0.66f && swingTimer < swingDuration)
        {
            motionRatio -= Time.deltaTime;
            motionRatio3 += Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(startRot, targetAngle, motionRatio * 3 * (1.0f / swingDuration));
            transform.localPosition = Vector3.Lerp(targetPos2, startPos, motionRatio3 * 3 * (1.0f / swingDuration));
        }

        else if (swingTimer >= swingDuration)
        {
            swingTimer = 0.0f;
            motionRatio = 0;
            motionRatio2 = 0;
            isSwinging = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Enemy enemy = other.gameObject.GetComponent<Enemy>();
    //    if (enemy)
    //    {
    //        Debug.Log($"{other.gameObject.name} is attacked!");
    //        enemy.OnDamageProcess();
    //    }
    //}

}
