using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Room
{
    string[,] roomData;

    [HideInInspector] public int top;
    [HideInInspector] public int right;
    [HideInInspector] public int left;
    [HideInInspector] public int bottom;

    Vector2Int topCenter;
    Vector2Int rightCenter;
    Vector2Int leftCenter;
    Vector2Int bottomCenter;

    [HideInInspector] public Vector2Int topDoorPos;
    [HideInInspector] public Vector2Int bottomDoorPos;
    [HideInInspector] public Vector2Int leftDoorPos;
    [HideInInspector] public Vector2Int rightDoorPos;

    [HideInInspector] public bool isShop;
    [HideInInspector] public bool isBoss;
    [HideInInspector] public bool isHub;


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

        if (collumns == ProceduralConstants.FAKE_SHOP_SIZE && rows == ProceduralConstants.FAKE_SHOP_SIZE)
        {
            isShop = true;

            topDoorPos = new Vector2Int(24,24);
            leftDoorPos = new Vector2Int(12,21);
            bottomDoorPos = new Vector2Int(20, 18);
            rightDoorPos = new Vector2Int(28, 21);
        }
        else if (collumns == ProceduralConstants.FAKE_BOSS_SIZE && rows == ProceduralConstants.FAKE_BOSS_SIZE)
        {
            isBoss = true;

            topDoorPos = new Vector2Int(24, 24);
            leftDoorPos = new Vector2Int(12, 21);
            bottomDoorPos = new Vector2Int(20, 18);
            rightDoorPos = new Vector2Int(28, 21);
        }
        else if (collumns == ProceduralConstants.FAKE_HUB_SIZE && rows == ProceduralConstants.FAKE_HUB_SIZE)
        {
            isHub = true;

            topDoorPos = new Vector2Int(20, 24);
        }
    }

    public void SetTile(int x, int y, string tileName)
    {
        roomData[x, y] = tileName;
    }

    public string GetTile(int x, int y)
    {
        return roomData[x,y];
    }

    public string GetTile(Vector2Int pos)
    {
        return roomData[pos.x, pos.y];
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
        // Debug.Log("Generating rooms");
        //Debug.Log("Generating doors for: " + BoardManager.mapCoordinates);

        if (BoardManager.mapCoordinates.y < ProceduralConstants.MAX_MAP_VERT - 1)
        {
            Vector2Int validTop = topCenter;
            if (GetTile(topCenter.x, topCenter.y) != "floor")
            {
                validTop = GetValidPosition(topCenter);
            }
            topDoorPos = validTop;
            SetTile(validTop.x, validTop.y, "upperDoor");
        }

        if (BoardManager.mapCoordinates.y > 0)
        {
            Vector2Int validBottom = bottomCenter;
            if (GetTile(bottomCenter.x, bottomCenter.y) != "floor")
            {
                validBottom = GetValidPosition(bottomCenter);
            }
            bottomDoorPos = validBottom;
            SetTile(validBottom.x, validBottom.y, "bottomDoor");
        }
        
        if (BoardManager.mapCoordinates.x < ProceduralConstants.MAX_MAP_HORIZ - 1)
        {
            Vector2Int validRight = rightCenter;
            if (GetTile(rightCenter.x, rightCenter.y) != "floor")
            {
                validRight = GetValidPosition(rightCenter);
            }
            rightDoorPos = validRight;
            SetTile(validRight.x, validRight.y, "rightDoor");
        }
        
        if (BoardManager.mapCoordinates.x > 0)
        {
            Vector2Int validLeft = leftCenter;
            if (GetTile(leftCenter.x, leftCenter.y) != "floor")
            {
                validLeft = GetValidPosition(leftCenter);
            }
            leftDoorPos = validLeft;
            SetTile(validLeft.x, validLeft.y, "leftDoor");
        }

        //SetTile(leftCenter.x, leftCenter.y, "leftDoor");


        //SetTile(rightCenter.x, rightCenter.y, "rightDoor");
        //SetTile(bottomCenter.x, bottomCenter.y, "bottomDoor");
    }

    public void GenerateCookware()
    {
        Vector2Int blenderLoc = new Vector2Int(Random.Range(left, right), Random.Range(bottom, top));
        
        if (GetTile(blenderLoc.x, blenderLoc.y) != "floor")
        {
            blenderLoc = GetValidPosition(blenderLoc);
        }

        roomData[blenderLoc.x, blenderLoc.y] = "blender";



        Vector2Int ovenLoc = new Vector2Int(Random.Range(left, right), Random.Range(bottom, top));

        if (GetTile(ovenLoc.x, ovenLoc.y) != "floor")
        {
            ovenLoc = GetValidPosition(ovenLoc);
        }

        roomData[ovenLoc.x, ovenLoc.y] = "oven";



        Vector2Int fryerLoc = new Vector2Int(Random.Range(left, right), Random.Range(bottom, top));

        if (GetTile(fryerLoc.x, fryerLoc.y) != "floor")
        {
            fryerLoc = GetValidPosition(fryerLoc);
        }

        roomData[fryerLoc.x, fryerLoc.y] = "fryer";



        Vector2Int plateLoc = new Vector2Int(Random.Range(left, right), Random.Range(bottom, top));

        if (GetTile(plateLoc.x, plateLoc.y) != "floor")
        {
            plateLoc = GetValidPosition(plateLoc);
        }

        roomData[plateLoc.x, plateLoc.y] = "plate";

    }

    public Vector2Int GetValidPosition(Vector2Int pos)
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

        //Debug.Log(closestMatch + " <-- " + pos);

        return closestMatch;
    }

    public Vector2 GetValidFloatPosition(Vector2 pos)
    {
        Vector2 closestMatch = new Vector2(0, 0);
        float closestDistance = 100000f;

        for (int x = 0; x < roomData.GetLength(1); x++)
        {
            for (int y = 0; y < roomData.GetLength(0); y++)
            {
                if (GetTile(x, y) == "floor")
                {
                    float newDistance = Vector2.Distance(pos, new Vector2(x, y));
                    if (closestDistance > newDistance)
                    {
                        closestMatch = new Vector2(x, y);
                        closestDistance = newDistance;
                    }
                }
            }
        }

        //Debug.Log(closestMatch + " <-- " + pos);

        return closestMatch;
    }
}