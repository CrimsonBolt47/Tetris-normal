using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;

    float previoustime;
    public float falltime = 0.8f;

    public static int height = 20,width=10;
    static Transform[,] grid = new Transform[width, height];
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
           
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

        }

        if (Time.time - previoustime > (Input.GetKey(KeyCode.DownArrow) ? falltime / 10 : falltime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                Addtogrid();
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().NewTetromino();
            }
            previoustime = Time.time;
        }




    }

    void Addtogrid()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;

        }
    }

    void CheckForLines()
    {
        for(int i=height;i>=0;i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j=0;j<width;j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for(int j=0;j<width;j++)
        {
            Destroy(grid[j, i].gameObject);
        }
    }

    void RowDown(int i)
    {
        for(int y=i;y<height;y++)
        {
            for(int j=0;j<width;j++)
            {
                grid[j, y - 1] = grid[j, y];
                grid[j, y]=null;
                grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);

            }
        }
    }

    bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX<0 ||roundedX >= width || roundedY<0 ||roundedY>=height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }
        return true;

    }
}
