using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player Instance;

    public GeodeticPoint position;

    public TilePoint tilePoint { get; private set; }
    public TilePos tilePos { get; private set; }

    void Awake ()
    {
        Instance = this;
    }
    
	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {   
        tilePoint = Map.LonLat2PointInTile (position.lon, position.lat, 17);
        tilePos = new TilePos ((int)tilePoint.x, (int)tilePoint.y);
        float pos_x = Map.TILE_WIDTH * ((tilePos.x - Map.Instance.startPos.x) + (float)(tilePoint.x % 1));
        float pos_y = Map.TILE_WIDTH * ((tilePos.y - Map.Instance.startPos.y) + (float)(tilePoint.y % 1));
        transform.position = Vector3.Lerp (transform.position, new Vector3(pos_x, 0.1f, pos_y), Time.deltaTime * 5);
	}
}
