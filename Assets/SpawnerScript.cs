using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] Blocks;

    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnRandomBlock()
    {
        int randomNumber = Random.Range(0, Blocks.Length);
        Instantiate(Blocks[randomNumber], transform.position, Quaternion.identity);
    }
}