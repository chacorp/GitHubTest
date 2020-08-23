using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지정된 장소에 에너미를 생성하는 오브젝트 풀을 만들고 싶다.
// 필요 속성 : 생성위치, Enemy 프리팹, Object Pool
public class ObjectManager : MonoBehaviour
{
    #region EnemyList
    //[SerializeField] public Transform[] spawnPoints;
    [SerializeField] public GameObject[] enemies;

    #endregion
    [SerializeField] public List<GameObject>[] enemyObjectPools = new List<GameObject>[] { new List<GameObject>(), new List<GameObject>(), new List<GameObject>(), new List<GameObject>() };
    [SerializeField] public int poolSize = 2;


    void Start()
    {
        InitPool(poolSize);
    }

    void InitPool(int size)
    {

        for (int i = 0; i < size; i++)
        {

            // spiderS1을 오브젝트 풀 리스트에 넣어준다
            GameObject spiderS1 = Instantiate(enemies[0]);
            spiderS1.transform.position = Vector3.zero;
            spiderS1.SetActive(false);
            enemyObjectPools[0].Add(spiderS1);
        }

        for (int i = 0; i < size; i++)
        {
            // spiderS2을 오브젝트 풀 리스트에 넣어준다
            GameObject spiderS2 = Instantiate(enemies[1]);
            spiderS2.transform.position = Vector3.zero;
            spiderS2.SetActive(false);
            enemyObjectPools[1].Add(spiderS2);
        }

        //for (int i = 0; i < size; i++)
        //{
        //    // spiderL를 오브젝트 풀 리스트에 넣어준다
        //    GameObject spiderL = Instantiate(enemies[2]);
        //    spiderL.SetActive(false);
        //    enemyObjectPools[2].Add(spiderL);
        //}

        //for (int i = 0; i < size; i++)
        //{
        //    // spiderL를 오브젝트 풀 리스트에 넣어준다
        //    GameObject beetle = Instantiate(enemies[3]);
        //    beetle.SetActive(false);
        //    enemyObjectPools[3].Add(beetle);
        //}
    }

    public GameObject GetObject(List<GameObject> enemyObjectPool)
    {
        GameObject obj = enemyObjectPool[0];
        obj.SetActive(true);
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.localScale = Vector3.one;
        obj.transform.up = Vector3.up;

        enemyObjectPool.RemoveAt(0);
        return obj;

    }

    public void ReturnObject(GameObject enemy, List<GameObject> enemyObjectPool)
    {
        enemy.SetActive(false);
        enemy.transform.position = Vector3.zero;
        enemyObjectPool.Add(enemy);
    }
}
