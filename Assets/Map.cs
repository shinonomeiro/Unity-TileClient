using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour 
{
    public const int TILE_WIDTH = 256;

    public static Map Instance;

    public TilePos startPos { get; private set; }
    public List<TilePos> tileList { get; private set; }

    public Transform tilePrefab;

    void Awake ()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        tileList = new List<TilePos>();
        startPos = LonLat2Tile (Player.Instance.position.lon, Player.Instance.position.lat, 17);
        StartCoroutine (IELoadTile (new TilePos (startPos.x, startPos.y)));
	}

    IEnumerator IELoadTile (TilePos pos)
    {
        if (tileList.Contains (pos))
        {
            yield break;
        }

        tileList.Add (pos);

        TilePos playerTile;

        do
        {
            playerTile = Player.Instance.tilePos;
            yield return null;
        }
        while (Mathf.Abs(pos.x - playerTile.x) > 1 || Mathf.Abs(pos.y - playerTile.y) > 1);

        Transform tileObj = Instantiate (tilePrefab, Vector3.zero, transform.rotation) as Transform;
        TilePos relativePos = new TilePos (pos.x - startPos.x, pos.y - startPos.y);
        tileObj.position = new Vector3 (relativePos.x * TILE_WIDTH, 0, relativePos.y * TILE_WIDTH);
        tileObj.transform.rotation = Quaternion.Euler (90, 0, 0);

        StartCoroutine (IELoadTex (pos, tileObj.GetComponent<SpriteRenderer>()));

        StartCoroutine (IELoadTile (new TilePos (pos.x + 1, pos.y)));
        StartCoroutine (IELoadTile (new TilePos (pos.x - 1, pos.y)));
        StartCoroutine (IELoadTile (new TilePos (pos.x, pos.y + 1)));
        StartCoroutine (IELoadTile (new TilePos (pos.x, pos.y - 1)));
    }

    IEnumerator IELoadTex (TilePos pos, SpriteRenderer tile)
    {
        string uri = string.Format("http://localhost:3000/location/{0}/{1}/{2}.png", 17, pos.x, pos.y);
        WWW www = new WWW (uri);
        yield return www;
        Debug.Log(www.bytesDownloaded + " bytes downloaded");

        Texture2D tex = www.texture;
        Sprite sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), Vector2.zero, 1);
        tile.sprite = sprite;
    }

    // Mathematics

    public static TilePoint LonLat2PointInTile (double lon, double lat, int zoom)
    {
        double tx = ((180 + lon) / 360) * (1 << zoom);
        double lat_rad = lat * Math.PI / 180;
        double ty = (1 << zoom) - (1 - (Math.Log(Math.Tan(lat_rad) + 1.0 / Math.Cos(lat_rad)) / Math.PI)) * (1 << zoom) / 2.0;

        return new TilePoint (tx, ty);
    }

    public static TilePos LonLat2Tile (double lon, double lat, int zoom)
    {
        TilePoint res = LonLat2PointInTile (lon, lat, zoom);
        return new TilePos ((int)res.x, (int)res.y);
    }
}
