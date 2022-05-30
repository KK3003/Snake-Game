using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCtrl : MonoBehaviour
{

    public GameObject shield,score;   // to spawn
    public GameObject top, bottom, left, right;
    public float minTime = 10, maxTime = 15; // for shield
    float time, spawnTime, timeScore, spawnTimeScore;
    public float minTimeScore = 10, maxTimeScore = 15; // for double score



    // Start is called before the first frame update
    void Start()
    {
        SetRandomTime();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
        spawnTimeScore = Random.Range(minTimeScore, maxTimeScore);
        Debug.Log("Next object spawn in " + spawnTime + " seconds.");
    }

    void FixedUpdate()
    {
        //Counts up
        time += Time.deltaTime;
        //Check if its the right time to spawn the object
        if (time >= spawnTime)
        {
            Spawn();
            SetRandomTime();
            time = 0;
        }

        timeScore += Time.deltaTime;
        if (timeScore >= spawnTimeScore)
        {
            SpawnScore();
            SetRandomTime();
            timeScore = 0;
        }
    }

    void Spawn()
    {
       shield.transform.position = new Vector3(Random.Range(left.transform.position.x, right.transform.position.x),
       Random.Range(top.transform.position.y, bottom.transform.position.y), 0);
       Instantiate(shield, shield.transform.position, Quaternion.identity);
    }

    void SpawnScore()
    {
        score.transform.position = new Vector3(Random.Range(left.transform.position.x, right.transform.position.x),
       Random.Range(top.transform.position.y, bottom.transform.position.y), 0);
        Instantiate(score, shield.transform.position, Quaternion.identity);
    }
}
