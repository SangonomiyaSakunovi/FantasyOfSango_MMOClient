using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//Developer : SangonomiyaSakunovi
//Discription: A Grid Generator.

public class AStarGraphGenerator : MonoBehaviour
{
    //Attention! In this Graph, all the grid is 10*float to use int for avoid the float.epsilon effect.
    //The list should load into Dict, that can give you O(1) speed to use AStarPathFinder.
    //This two method must have same gridRegulation. So you also need give 10*float change the enemy and player position.

    //Attention! Int gridLenth = 1, means the real map grid is 0.1f.
    private int GridLength = 1;
    //Attention! This is use for realY, only to exame if center point is walkable;
    private float WalkableHeight = 0.2f;
    //The ray will shoot from this yPoint, and you can traverse the x and z.
    private int SkyHeight = 1000;

    //Attention! These params can set in inspector, so you need attention the int = 10*float.
    public int startScanIntX;
    public int startScanIntZ;
    public int endScanIntX;
    public int endScanIntZ;

    private Ray ray;
    private RaycastHit hit;

    //private List<AStarGridPoint> aStarGridPointList;

    FileStream fileStream;
    StreamWriter writer;

    void Start()
    {
        //aStarGridPointList = new List<AStarGridPoint>();

    }

    private void Update()
    {
        //Just for test.
        if (Input.GetMouseButtonDown(0))
        {
            fileStream = new FileStream(@"D:\FantasyOfSango\Client\IsLandAStarGraphJson.txt", FileMode.OpenOrCreate, FileAccess.Write);
            writer = new StreamWriter(fileStream);
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            ray = new Ray();
            hit = new RaycastHit();
            ScanGridGraph(startScanIntX, startScanIntZ, endScanIntX, endScanIntZ);
            Debug.Log("“—æ≠ºÏ≤‚ÕÍ±œ");
            writer.Close();
            fileStream.Close();
        }
    }

    private void ScanGridGraph(int startX, int startZ, int endX, int endZ)
    {
        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                ScanGridPoint(x, z);
            }
        }
    }

    private void ScanGridPoint(int xInt, int zInt)
    {
        float x = xInt / 10f;
        float z = zInt / 10f;
        ray.origin = new Vector3(x, SkyHeight, z);
        ray.direction = Vector3.down;
        if (Physics.Raycast(ray, out hit))
        {
            int yInt = (int)((hit.point.y + float.Epsilon) * 10);
            //if (hit.collider.tag == "obstacles")
            //{
            //    //AStarGridPoint aStarGridPoint = new AStarGridPoint(xInt, yInt, zInt, true);

            //    //aStarGridPointList.Add(aStarGridPoint);

            //    writer.Write(xInt+","+yInt+","+zInt+","+"1"+"\n");
            //}
            //else
            //{
            //    //AStarGridPoint aStarGridPoint = new AStarGridPoint(xInt, yInt, zInt, CheckSurround(xInt, yInt, zInt));

            //    if (CheckSurround(xInt, yInt, zInt))
            //    {
            //        writer.Write(xInt + "," + yInt + "," + zInt + "," + "1" + "\n");
            //    }
            //    else
            //    {
            //        writer.Write(xInt + "," + yInt + "," + zInt + "," + "0" + "\n");
            //    }

            //    //aStarGridPointList.Add(aStarGridPoint);
            //}

            if (CheckSurround(xInt, yInt, zInt))
            {
                writer.Write(xInt + "," + yInt + "," + zInt + "," + "1" + "\n");
            }
            else
            {
                writer.Write(xInt + "," + yInt + "," + zInt + "," + "0" + "\n");
            }
        }

    }

    private bool CheckSurround(int xInt, int centerYInt, int zInt)
    {
        bool isObstacle = false;
        float x = xInt / 10f;
        float centerY = centerYInt / 10f;
        float z = zInt / 10f;
        float gridFloat = GridLength / 10f;
        List<Vector3> vector3List = new List<Vector3>();

        Vector3 Up = new Vector3(x, SkyHeight, z + gridFloat);
        Vector3 Down = new Vector3(x, SkyHeight, z - gridFloat);
        Vector3 Left = new Vector3(x - gridFloat, SkyHeight, z);
        Vector3 Right = new Vector3(x + gridFloat, SkyHeight, z);
        Vector3 LeftUp = new Vector3(x - gridFloat, SkyHeight, z + gridFloat);
        Vector3 LeftDown = new Vector3(x - gridFloat, SkyHeight, z - gridFloat);
        Vector3 RightUp = new Vector3(x + gridFloat, SkyHeight, z + gridFloat);
        Vector3 RightDown = new Vector3(x + gridFloat, SkyHeight, z - gridFloat);
        vector3List.Add(Up);
        vector3List.Add(Down);
        vector3List.Add(Left);
        vector3List.Add(Right);
        vector3List.Add(LeftUp);
        vector3List.Add(LeftDown);
        vector3List.Add(RightUp);
        vector3List.Add(RightDown);

        for (int i = 0; i < 8; i++)
        {

            if (!CompareTwoPointY(centerY, vector3List[i]))
            {

                isObstacle = true; break;
            }
        }
        return isObstacle;
    }

    private bool CompareTwoPointY(float centerY, Vector3 pos)
    {
        bool isWalkable = true;
        ray.origin = pos;
        ray.direction = Vector3.down;
        if (Physics.Raycast(ray, out hit))
        {
            float surroundY = hit.point.y;
            if (MathF.Abs(surroundY - centerY) > WalkableHeight)
            {
                isWalkable = false;
            }
        }
        return isWalkable;
    }
}
