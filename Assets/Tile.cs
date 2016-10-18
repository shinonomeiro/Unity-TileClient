using UnityEngine;
using System;
using System.Collections;

[Serializable]
public struct TilePos
{
    public int x;
    public int y;

    public TilePos (int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[Serializable]
public struct TilePoint
{
    public double x;
    public double y;

    public TilePoint (double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

[Serializable]
public struct GeodeticPoint
{
    public double lon;
    public double lat;

    public GeodeticPoint (double lon, double lat)
    {
        this.lon = lon;
        this.lat = lat;
    }
}

public class Tile : MonoBehaviour 
{
    public TilePos position;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
