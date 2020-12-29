using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    public static int height = 20;
    public static int width = 10;
    public float fallSpeed = 1f;
    private float elapsedSec;
    public Vector3 pivot = new Vector3(0, 0, 0);
    private static Transform[,] grid = new Transform[width, height];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SafeMove(new Vector3(-1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SafeMove(new Vector3(1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            SafeRotate(90);

        float tempFallSpeed = fallSpeed;
        if (Input.GetKey(KeyCode.DownArrow))
            tempFallSpeed /= 10;

        if (Time.time - elapsedSec > tempFallSpeed)
        {
            if (!SafeMove(new Vector3(0, -1, 0)))
            {
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<SpawnerScript>().SpawnRandomBlock();
            }
            elapsedSec = Time.time;
        }
    }

    bool SafeMove(Vector3 vector)
    {
        bool isSafe = true;
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x + vector.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y + vector.y);

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
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

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
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedy = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedy] = children;
        }
    }

    void CheckForLines()
    {
        for(int i = height - 1; i >= 0 ; i--)
            if(HasLine(i))
            {
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
}
