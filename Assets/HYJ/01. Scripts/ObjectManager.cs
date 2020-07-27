using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지정된 장소에 에너미를 생성하는 오브젝트 풀을 만들고 싶다.
// 필요 속성 : 생성위치, Enemy 프리팹, Object Pool
public class ObjectManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject spiderL;

    public List<GameObject> enemyObjectPool = new List<GameObject>();
    [SerializeField] int poolSize = 10;
    
    
    void Start()
    {
        InitPool(poolSize);
    }

    void InitPool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            // createNewObject 함수에서 생성된 프리팹을 오브젝트 풀에 넣는다.
            enemyObjectPool.Add(createNewObject());
        }
    }

    private GameObject createNewObject()
    {
        GameObject newObj = Instantiate(spiderL);
        newObj.SetActive(false);
        return newObj;
    }

    public GameObject GetObject()
    {
        if (enemyObjectPool.Count > 0)
        {
            GameObject obj = enemyObjectPool[0];
            obj.SetActive(true);

            int rand = Random.Range(0, spawnPoints.Length);
            obj.transform.position = spawnPoints[rand].position;

            enemyObjectPool.RemoveAt(0);
            return obj;
        }
        else return null;
        //else
        //{
        //    GameObject newObj = createNewObject();
        //    newObj.SetActive(true);
        //    return newObj;
        //}

    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        enemyObjectPool.Add(obj);
    }
}
