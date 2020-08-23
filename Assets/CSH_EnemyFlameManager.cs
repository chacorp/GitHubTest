using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSH_EnemyFlameManager : MonoBehaviour
{
    public List<GameObject> enemyFlame = new List<GameObject>();

    void Awake()
    {
        //자식으로 갖고 있는 FlameTracker 를 모두 리스트에 넣기
        for (int i = 0; i < transform.childCount; i++)
        {
            enemyFlame.Add(transform.GetChild(i).gameObject);
            // 넣고 비활성화하기
            enemyFlame[i].SetActive(false);
        }
    }

}
