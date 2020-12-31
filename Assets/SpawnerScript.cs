using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] Blocks;
    public ManagerScript managerScript;
    public Transform playfield;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    block.SetPlayfield(playfield);
    //}

    public void SpawnRandomBlock()
    {
        int randomNumber = Random.Range(0, Blocks.Length);
        GameObject temp = Instantiate(Blocks[randomNumber], transform.position, Quaternion.identity);
        temp.GetComponent<block>().SetPlayfield(playfield);
        managerScript.SetCurrentBlock(temp);
    }
}