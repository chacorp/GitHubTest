using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀에 있는 Enemy Object를 활성화 시켜서 씬에 위치시키겠다.
public class EnemyManager : MonoBehaviour
{
    // 에너미 체력을 제어하는 코드를 싱글톤 디자인과 프로퍼티를 이용해서 관리
    public static EnemyManager Instance;

    [SerializeField] ObjectManager obMgr;
    [SerializeField] Transform[] spawnPoints;

    private bool isSpawnEnemy = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 에너미를 생성할 때 에너미끼리 중복 생성되지 않도록 하는 메소드 작성
        StartCoroutine(CreateEnemy());
    }

    void Update()
    {
        if (obMgr.enemyObjectPools[0].Count == 3 && obMgr.enemyObjectPools[1].Count == 3)
        {
            isSpawnEnemy = false;

            if (!QuestManager.Instance.quests[3].goal.IsReached())
            {
                StartCoroutine(CreateEnemy());
                Debug.Log("Recreate Enemy!!");
            }
            else return;
        }


    }

    IEnumerator CreateEnemy()
    {
        if (!isSpawnEnemy)
        {

            List<Transform> spawnPosition = new List<Transform>();
            foreach (Transform point in spawnPoints)
            {
                spawnPosition.Add(point);
            }

            yield return new WaitForFixedUpdate();

            for (int i = 0; i < obMgr.poolSize; i++)
            {
                GameObject enemyType1 = obMgr.GetObject(obMgr.enemyObjectPools[0]);

                int randNum = Random.Range(0, spawnPosition.Count);
                enemyType1.transform.position = spawnPosition[randNum].position;

                spawnPosition.RemoveAt(randNum);
            }

            for (int i = 0; i < obMgr.poolSize; i++)
            {
                GameObject enemyType2 = obMgr.GetObject(obMgr.enemyObjectPools[1]);

                int randNum = Random.Range(0, spawnPosition.Count);
                enemyType2.transform.position = spawnPosition[randNum].position;

                spawnPosition.RemoveAt(randNum);
            }

            isSpawnEnemy = true;
            yield return new WaitForFixedUpdate();
        }
    }

    //IEnumerator CreateEnemy2()
    //{
    //    int randNum = Random.Range(0, obMgr.enemies.Length);

    //    for (int i = 0; i < obMgr.enemies.Length * obMgr.poolSize; i++)
    //    {
    //        enemies[i] = obMgr.GetObject(obMgr.enemyObjectPools[randNum]);
    //        enemies[i].transform.localPosition = new Vector3(0, 0, 0);
    //    }

    //    yield return new WaitForFixedUpdate();

    //    for (int i = 0; i < obMgr.enemies.Length * obMgr.poolSize; i++)
    //    {
    //        int temp = Random.Range(0, spawnPoints.Length);
    //        Vector3 myPos = spawnPoints[temp].position;

    //        int checkLayer = LayerMask.NameToLayer("Enemy");

    //        Collider[] cols = Physics.OverlapSphere(myPos, 0.3f, 1 << checkLayer);

    //        if (cols.Length > 0)
    //        {
    //            i--;
    //            yield return 0;
    //        }
    //        else
    //        {
    //            enemies[i].transform.localPosition = myPos;
    //            yield return new WaitForFixedUpdate();
    //        }
    //    }
    //}
}
