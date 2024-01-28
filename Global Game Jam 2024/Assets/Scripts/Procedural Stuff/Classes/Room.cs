using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Room
{
    string[,] roomData;

    int top;
    int right;
    int left;
    int bottom;

    Vector2Int topCenter;
    Vector2Int rightCenter;
    Vector2Int leftCenter;
    Vector2Int bottomCenter;

    public Room(int collumns, int rows)
    {
        roomData = new string[collumns,rows];

        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                SetTile(x, y, "wall");
                //boardData[x,y] = "wall";
            }
        }

        top = 0;
        right = 0;
        left = 100;
        bottom = 100;
    }

    public void SetTile(int x, int y, string tileName)
    {
        roomData[x, y] = tileName;
    }

    public string GetTile(int x, int y)
    {
        return roomData[x,y];
    }

    public void CarveRoom (int minSplats, int maxSplats, int minSplatSize, int maxSplatSize, int rows, int collumns)
    {
        int numSplats = Random.Range(minSplats, maxSplats);
        for (int i = 0; i < numSplats; i++)
        {
            int range = Random.Range(minSplatSize, maxSplatSize);
            int y = Random.Range(range, rows - range);
            int x = Random.Range(range, collumns - range);

            Carve(x, y, range);
        }

        GetDimensions();

    }

    void Carve(int x, int y, int range)
    {
        for (int dx = -range; dx < range - 1; dx++)
        {
            for (int dy = -range; dy < range - 1; dy++)
            {
                if ((dx - range) * (dx - range) + (dy - range) * (dy - range) <= range * range)
                {
                    SetTile(x + dx, y + dy, "floor");
                }
            }
        }
    }

    void GetDimensions()
    {
        for (int x = 0; x < roomData.GetLength(1); x++)
        {
            for (int y = 0; y < roomData.GetLength(0); y++)
            {
                if (GetTile(x,y) == "floor")
                {
                    if (top < y)
                    {
                        top = y;
                    }

                    if (left > x)
                    {
                        left = x;
                    }

                    if (right < x)
                    {
                        right = x;
                    }

                    if (bottom > y)
                    {
                        bottom = y;
                    }
                }
            }
        }

        topCenter = new Vector2Int(left + (right - left) / 2, top);
        bottomCenter = new Vector2Int(left + (right - left) / 2, bottom);
        leftCenter = new Vector2Int(left, bottom + (top - bottom) / 2);
        rightCenter = new Vector2Int(right, bottom + (top - bottom) / 2);

    }

    public void GenerateDoors(int rows, int collumns)
    {
        Vector2Int validTop = topCenter;
        if (GetTile(topCenter.x, topCenter.y) != "floor")
        {
            validTop = GetValidPosition(topCenter);
        }
        SetTile(validTop.x, validTop.y, "upperDoor");


        Vector2Int validLeft = leftCenter;
        if (GetTile(leftCenter.x, leftCenter.y) != "floor")
        {
            validLeft = GetValidPosition(leftCenter);
        }
        SetTile(validLeft.x, validLeft.y, "leftDoor");


        Vector2Int validRight = rightCenter;
        if (GetTile(rightCenter.x, rightCenter.y) != "floor")
        {
            validRight = GetValidPosition(rightCenter);
        }
        SetTile(validRight.x, validRight.y, "rightDoor");


        Vector2Int validBottom = bottomCenter;
        if (GetTile(bottomCenter.x, bottomCenter.y) != "floor")
        {
            validBottom = GetValidPosition(bottomCenter);
        }
        SetTile(validBottom.x, validBottom.y, "bottomDoor");



        //SetTile(leftCenter.x, leftCenter.y, "leftDoor");


        //SetTile(rightCenter.x, rightCenter.y, "rightDoor");
        //SetTile(bottomCenter.x, bottomCenter.y, "bottomDoor");
    }

    Vector2Int GetValidPosition(Vector2Int pos)
    {
        Vector2Int closestMatch = new Vector2Int(0, 0);
        float closestDistance = 100000f;

        for (int x = 0; x < roomData.GetLength(1); x++)
        {
            for (int y = 0; y < roomData.GetLength(0); y++)
            {
                if (GetTile(x, y) == "floor")
                {
                    float newDistance = Vector2Int.Distance(pos, new Vector2Int(x, y));
                    if (closestDistance > newDistance)
                    {
                        closestMatch = new Vector2Int(x, y);
                        closestDistance = newDistance;
                    }
                }
            }
        }

        Debug.Log(closestMatch + " <-- " + pos);

        return closestMatch;
    }
}