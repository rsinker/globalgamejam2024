using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Room
{
    string[,] roomData;

    Vector2 tL;
    Vector2 tR;
    Vector2 bL;
    Vector2 bR;

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

        tL = new Vector2(0, 0);
        tR = new Vector2(0, 0);
        bL = new Vector2(0, 0);
        bR = new Vector2(0, 0);
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
                    /*if (tL.y < y + dy)
                    {
                        tL.y = y + dy;
                        tL.x = x + dx;
                    }

                    if (bL.y > y + dy)
                    {
                        bL.y = y + dy;
                        bL.x = x + dx;
                    }

                    if (tR.y < y + dy || tR.x < x + dx)
                    {
                        tR.y = y + dy;
                        tR.x = x + dx;
                    }*/
                }
            }
        }
    }

    public void GenerateDoors(int rows, int collumns)
    {
        //Find top left door
        int maxRowTL = 0;
        int validXTL = 0;
        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (y > maxRowTL && GetTile(x, y) == "floor")
                {
                    maxRowTL = y;
                    validXTL = x;
                }
            }
        }

        SetTile(validXTL, maxRowTL, "upperDoor");

        //boardData[validXTL, maxRowTL] = "upperDoor";

        //Find bottom left door
        int minRow = 100;
        int validXBL = 0;
        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (y < minRow && GetTile(x, y) == "floor")
                {
                    minRow = y;
                    validXBL = x;
                }
            }
        }

        SetTile(validXBL, minRow, "leftDoor");

        //Find top right door
        int maxRowTR = 0;
        int validXTR = 0;
        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if ((y > maxRowTR || x > validXTR) && GetTile(x, y) == "floor")
                {
                    maxRowTR = y;
                    validXTR = x;
                }
            }
        }

        SetTile(validXTR, maxRowTR, "rightDoor");
        //boardData[validXTR, maxRowTR] = "rightDoor";
    }
}
