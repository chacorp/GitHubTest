using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoPool : MonoBehaviour
{
    [SerializeField] GameObject echo;
    [SerializeField] int poolSize = 15;
    private Enemy enemy;
    public Queue<GameObject> echoPool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {

            GameObject effects = Instantiate(echo);
            effects.SetActive(false);
            echoPool.Enqueue(effects);
        }
    }


    public GameObject GetEchoPool()
    {
        if (echoPool.Count > 0)
        {
            GameObject echoEffect = echoPool.Dequeue();
            echoEffect.SetActive(true);
            return echoEffect;
        }
        else return null;

    }


    public IEnumerator ReturnEchoPool(GameObject obj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        echoPool.Enqueue(obj);
    }

}
