using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    private int bottomLeftX = 0;
    private int bottomLeftY = 0;
    private int height = 20;
    private int width = 10;
    private float elapsedSec;
    private Transform[,] grid = new Transform[10, 20];
    private float control = 0;
    private ManagerScript managerScript = null;
    private SpawnerScript spawnerScript = null;
    
    public float fallSpeed = 1f;
    public Vector3 pivot = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<ManagerScript>().GetGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || control == 0)
            SafeMove(new Vector3(-1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.RightArrow) || control == 1)
            SafeMove(new Vector3(1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.UpArrow) || control == 2)
            SafeRotate(90);

        float tempFallSpeed = fallSpeed;
        if (Input.GetKey(KeyCode.DownArrow) || control == 4)
            tempFallSpeed /= 10;

        if (Time.time - elapsedSec > tempFallSpeed)
        {
            if (!SafeMove(new Vector3(0, -1, 0)))
            {
                AddToGrid();
                CheckForLines();
                this.enabled = false;
            }
            elapsedSec = Time.time;
        }
    }

    bool SafeMove(Vector3 vector)
    {
        bool isSafe = true;
        foreach (Transform children in transform)
        { 
            int roundedX = Mathf.RoundToInt(children.transform.position.x + vector.x - bottomLeftX);
            int roundedY = Mathf.RoundToInt(children.transform.position.y + vector.y - bottomLeftY);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                isSafe = false;
                return isSafe;
            }

            if (grid[roundedX, roundedY] != null)
                isSafe = false;
        }

        if (isSafe)
            transform.position += vector;

        return isSafe;
    }

    bool SafeRotate(float angle)
    {
        transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), angle);

        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x - bottomLeftX);
            int roundedY = Mathf.RoundToInt(children.transform.position.y - bottomLeftY);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), -angle);
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                transform.RotateAround(transform.TransformPoint(pivot), new Vector3(0, 0, 1), -angle);
                return false;
            }
        }

        return true;
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x - bottomLeftX);
            int roundedY = Mathf.RoundToInt(children.transform.position.y - bottomLeftY);

            grid[roundedX, roundedY] = children;

            if (roundedY > height - 2)
            {
                FindObjectOfType<ManagerScript>().GameOver();
                return;
            }
        }
        FindObjectOfType<ManagerScript>().SetGrid(grid);
        FindObjectOfType<SpawnerScript>().SpawnRandomBlock();
    }

    void CheckForLines()
    {
        for(int i = height - 1; i >= 0 ; i--)
            if(HasLine(i))
            {
                FindObjectOfType<ManagerScript>().AddScore();
                DeleteLine(i);
                RowDown(i);
            }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
            if (grid[j, i] == null)
                return false;
        return true;
    }

    void DeleteLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
            for(int j = 0; j < width; j++)
                if(grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
    }

    public void ResetGrid()
    {
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if (grid[j, i] != null)
                {
                    Destroy(grid[j, i].gameObject);
                }
            }
        }

        grid = new Transform[width, height];
    }

    public void SetControl(float tempControl)
    {
        control = tempControl;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public Transform[,] GetGrid()
    {
        return grid;
    }

    public void SetPlayfield(Transform playfield)
    {
        width = Mathf.RoundToInt(playfield.localScale.x);
        height = Mathf.RoundToInt(playfield.localScale.y);
        bottomLeftX = Mathf.RoundToInt(playfield.position.x - (width / 2));
        bottomLeftY = Mathf.RoundToInt(playfield.position.y - (height / 2));
    }
}