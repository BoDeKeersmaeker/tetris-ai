using UnityEngine;

public class block : MonoBehaviour
{
    private int bottomLeftX = 0;
    private int bottomLeftY = 0;
    private int height = 20;
    private int width = 10;
    private float elapsedFallSec = 0;
    private Transform[,] grid;
    private float control1 = 0;
    private float control2 = 0;
    private ManagerScript managerScript = null;
    private SpawnerScript spawnerScript = null;
    private float fallSpeed = 1f;
    private bool useManualControl = true;
    private float elapsedControlSec = 0;

    public float controlTime = 0.1f;
    public Vector3 pivot = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    //void Start()
    //{
    // 
    //}

    // Update is called once per frame
    void Update()
    {
        float tempFallSpeed = fallSpeed;

        if (Time.time - elapsedControlSec > controlTime)
        {
            bool keyWasPressed = false;

            if ((!useManualControl && control1 == 1) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SafeMove(new Vector3(-1, 0, 0));
                keyWasPressed = true;
            }
            else if ((!useManualControl && control1 == 2) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SafeMove(new Vector3(1, 0, 0));
                keyWasPressed = true;
            }
            else if ((!useManualControl && control1 == 3) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                SafeRotate(90);
                keyWasPressed = true;
            }

            if ((!useManualControl && control2 == 1) || Input.GetKey(KeyCode.DownArrow))
                tempFallSpeed /= 10;

            if(keyWasPressed)
                elapsedControlSec = Time.time;
        }

        if (Time.time - elapsedFallSec > tempFallSpeed)
        {
            if (!SafeMove(new Vector3(0, -1, 0)))
            {
                AddToGrid();
                CheckForLines();
                this.enabled = false;
            }
            elapsedFallSec = Time.time;
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
        if (managerScript == null && spawnerScript == null)
            return;

        float moveScore = 0;
        int biggestY = 0;

        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x - bottomLeftX);
            int roundedY = Mathf.RoundToInt(children.transform.position.y - bottomLeftY);

            grid[roundedX, roundedY] = children;

            if (roundedY > height - 2)
            {
                managerScript.GameOver();
                return;
            }

            if (roundedY > biggestY)
                biggestY = roundedY;
        }

        if (biggestY <= managerScript.GetTotalHeight())
            moveScore += 10;
        else
            managerScript.SetTotalHeight(biggestY);


        int newAmountOfHoles = managerScript.calculateAmountOfHoles();
        if (newAmountOfHoles <= managerScript.GetAmountOfHoles())
            moveScore += 10;
        managerScript.SetAmountOfHoles(newAmountOfHoles);

        if (moveScore > 0)
        {
            managerScript.AddReward(moveScore);
            managerScript.AddTotalReward((int)moveScore);
        }

        //Debug.Log(managerScript.GetTotalReward());

        managerScript.SetGrid(grid);
        spawnerScript.SpawnRandomBlock();
    }

    void CheckForLines()
    {
        for(int i = height - 1; i >= 0 ; i--)
            if(HasLine(i))
            {
                if (managerScript != null)
                    managerScript.AddScore();
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
                    grid[j, i] = null;
                }
            }
        }
    }

    public void SetControl(float tempControl1, float tempControl2)
    {
        control1 = tempControl1;
        control2 = tempControl2;
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

    public void Initialise(Transform playfield, ManagerScript manager, SpawnerScript spawner)
    {
        width = Mathf.RoundToInt(playfield.localScale.x);
        height = Mathf.RoundToInt(playfield.localScale.y);
        bottomLeftX = Mathf.RoundToInt(playfield.position.x - (width / 2));
        bottomLeftY = Mathf.RoundToInt(playfield.position.y - (height / 2));

        managerScript = manager;
        spawnerScript = spawner;

        grid = managerScript.GetGrid();
        fallSpeed = managerScript.GetFallSpeed();
        useManualControl = managerScript.IsUsingManualControl();
    }
}