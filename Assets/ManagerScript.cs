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
    private int totalReward = 0;
    private int totalHeight = 0;
    private int amountOfHoles = 0;

    public float fallSpeed = 1f;
    public SpawnerScript spawnerScript = null;
    public bool useManualControl = false;

    public void AddScore()
    {
        score += 1000;
        if (fallSpeed > 0.1f)
            fallSpeed -= 0.025f;
        AddReward(1000);
        totalReward += 1000;
        //Debug.Log(totalReward);
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

    public float GetFallSpeed()
    {
        return fallSpeed;
    }

    public bool IsUsingManualControl()
    {
        return useManualControl;
    }

    public void AddTotalReward(int reward)
    {
        totalReward += reward;
    }

    public int GetTotalReward()
    {
        return totalReward;
    }

    public int GetTotalHeight()
    {
        return totalHeight;
    }

    public void SetTotalHeight(int height)
    {
        totalHeight = height;
    }

    public int GetAmountOfHoles()
    {
        return amountOfHoles;
    }
    public void SetAmountOfHoles(int amount)
    {
        amountOfHoles = amount;
    }

    public int calculateAmountOfHoles()
    {
        int amountOfHoles = 0;
        for (int i = 0; i < width; i++)
        {
            bool hasRoof = false;
            for (int j = height - 1; j >= 0; j--)
            {
                if (hasRoof && grid[i, j] == null)
                    amountOfHoles++;
                if (!hasRoof && grid[i, j] != null)
                    hasRoof = true;
            }
        }
        return amountOfHoles;
    }

    public void GameOver()
    {
        EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        if(currentBlock != null)
            currentBlock.GetComponent<block>().ResetGrid();
        if (currentBlock != null)
            Destroy(currentBlock);
        score = 0;
        totalReward = 0;
        totalHeight = 0;
        //grid = null;
        spawnerScript.SpawnRandomBlock();
        grid = currentBlock.GetComponent<block>().GetGrid();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (currentBlock == null)
            return;

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
        //Debug.Log(vectorAction[1]);
        currentBlock.GetComponent<block>().SetControl(vectorAction[0], vectorAction[1]);
    }

    public override void Heuristic(float[] actionsOut)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            actionsOut[0] = 1;
        else if(Input.GetKeyDown(KeyCode.RightArrow))
            actionsOut[0] = 2;
        else if(Input.GetKeyDown(KeyCode.UpArrow))
            actionsOut[0] = 3;
        else
            actionsOut[0] = 0;

        if (Input.GetKey(KeyCode.DownArrow))
            actionsOut[1] = 1;
        else
            actionsOut[1] = 0;
    }
}
