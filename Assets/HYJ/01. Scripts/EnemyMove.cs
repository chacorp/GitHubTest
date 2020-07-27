using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 가장 가까운 목적지를 탐색하여 이동하는 스크립트
public class EnemyMove : MonoBehaviour
{
    [SerializeField] int destinationLength;
    private GameObject[] destinations;
    private Vector3[] targetVectors;
    private NavMeshAgent spiderAgent;

    void Start()
    {
        spiderAgent = this.GetComponent<NavMeshAgent>();
        targetVectors = new Vector3[destinationLength];
        destinations = GameObject.FindGameObjectsWithTag("Destination");
        if (spiderAgent == null) Debug.Log("The nav mesh agent component is no attched to " + gameObject.name);
        else SetDestination();
    }

    private void SetDestination()
    {
        if (destinations != null)
        {
            // 목적지와의 거리 값을 나타내는 배열 생성
            for (int i = 0; i < destinations.Length; i++)
            {
                targetVectors[i] = destinations[i].transform.position - transform.position;
                if (targetVectors[i] == null) Debug.Log("Cannot find destination " + destinations[i].name);
            }

            // 목적지와의 거리를 가까운 기준으로 정렬하는 함수 (선택 정렬)
            for (int i = 0; i < targetVectors.Length - 1; i++)
            {
                for (int j = i + 1; j < targetVectors.Length; j++)
                {
                    if (targetVectors[i].magnitude > targetVectors[j].magnitude)
                    {
                        Vector3 temp = targetVectors[i];
                        targetVectors[i] = targetVectors[j];
                        targetVectors[j] = temp;
                    }
                }
            }

            spiderAgent.SetDestination(targetVectors[0] + transform.position);
            Debug.Log(targetVectors[0]);

        }
    }

    //private void GetDestination()
    //{
    //    for (int i = 0; i < destinationLength; i++)
    //    {
    //        destinations[i] = GameObject.Find("Dest" + i + 1).GetComponent<Transform>();
    //    }
    //}
}
