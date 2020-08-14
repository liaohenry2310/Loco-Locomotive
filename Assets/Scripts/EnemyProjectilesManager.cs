using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilesManager : MonoBehaviour
{
    public int maxCount;

    List<GameObject> mBasicEP;

    public GameObject ProjectilePrefab;




    // Start is called before the first frame update
    void Start()
    {


        mBasicEP = new List<GameObject>();
        for (int i = 0; i < maxCount; i++)
        {
            //Set ProjectilePrefab to gO then add to list
            GameObject gO = Instantiate(ProjectilePrefab);
            //
            gO.transform.SetParent(transform);
            gO.SetActive(false);
            mBasicEP.Add(gO);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetActiveProjectiles()
    {
        foreach (GameObject gO in mBasicEP)
        {
            if (gO.activeInHierarchy)
            {
                continue;
            }
            else
            {
                return gO;
            }
        }
        return null;
    }
}
