using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ManagerScript : Agent
{
    private int score = 0;
    private Transform[,] grid = null;
    private GameObject currentBlock = null;
    private int width = 10;
    private int height = 20;

    public SpawnerScript spawnerScript = null;

    public void AddScore()
    {
        score += 10;
        Debug.Log(score);
        AddReward(10);
    }

    public void SetGrid(Transform[,] tempGrid)
    {
        grid = tempGrid;
    }

    public Transform[,] GetGrid()
    {
        return grid;
    }

    public void SetCurrentBlock(GameObject tempBlock)
    {
        currentBlock = tempBlock;
        height = currentBlock.GetComponent<block>().GetHeight();
        width = currentBlock.GetComponent<block>().GetWidth();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        if(currentBlock != null)
            currentBlock.GetComponent<block>().ResetGrid();
        if (currentBlock != null)
            Destroy(currentBlock);
        score = 0;
        spawnerScript.SpawnRandomBlock();
        grid = currentBlock.GetComponent<block>().GetGrid();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (Transform children in currentBlock.transform)
        {
            sensor.AddObservation(children);
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, i] != null)
                    sensor.AddObservation(new Vector2(grid[j, i].position.x, grid[j, i].position.y));
                else
                    sensor.AddObservation(new Vector2(-1, -1));

            }
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        Debug.Log(vectorAction[0]);
        currentBlock.GetComponent<block>().SetControl(vectorAction[0]);
    }
}
