using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===========================================================================
// 1.  1,2,3,4,5 번으로 활성화 무기 바꾸기 ----------------------------------[O] <<<<<<<<<<<<<<    CSH_ItemGrab.cs 
// ===========================================================================

public class CSH_ItemSwitch : MonoBehaviour
{
    // 무기 잡고 있는 곳
    public Transform Holder;

    public int select_current = 0;
    int select_before = 0;

    // 갖고 있는 아이템 갯수
    public int HolderCount = 1;

    void Switch_item()
    {
        if (HolderCount <= 0) return;
        // 갖고 있는 아이템들 중에서
        // 선택한 번호의 아이템을 활성화하고 & 나머지는 비활성화하기
        for (int i = 0; i < HolderCount; i++)
        {
            // i번째의 아이템이 null이 아닐때,
            // => i번째 아이템이 있을때!
            // 리스트의 길이는 0보다 커야한다

            if (CSH_ItemGrab.Instance.activeItems.Count > 0)
            {
                // 리스트에 있는 아이템과 선택한 단축키 번호가 같을때
                if (i == select_current)
                {
                    CSH_ItemGrab.Instance.activeItems[i].SetActive(true);
                }
                // 리스트에 있는 아이템과 선택한 단축키 번호가 다를때
                else
                {
                    CSH_ItemGrab.Instance.activeItems[i].SetActive(false);
                }
            }
        }
    }

    private void Compare()
    {
        //만약 스크롤한 범위가
        //    아이템 리스트의 범위보다 크다면,
        // = 아이템 선택하면 안됨
        if (select_current >= CSH_ItemGrab.Instance.activeItems.Count)
        {
            select_current = select_before;
        }

    }

    void Update()
    {
        // -1 0 1
        //float a = Input.GetAxis("Mouse ScrollWheel");

        // 1번 누를때
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            select_current = 0;
        }
        // 2번 누를때
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            select_current = 1;
        }
        // 3번 누를때
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            select_current = 2;
        }
        // 4번 누를때
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            select_current = 3;
        }
        // 5번 누를때
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            select_current = 4;
        }

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0)
        {
            select_current--;
        }
        else if (wheel < 0)
        {
            select_current++;
        }
        select_current = Mathf.Clamp(select_current, 0, 4);


        Compare();

        // 만약 현재 선택한 값이 이전에 선택한 값과 다르면, 함수 발동!
        if (select_current != select_before)
        {
            Switch_item();

            // 이전에 선택한 값과 동기화
            select_before = select_current;
        }
    }
}
