using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridChangeTest : MonoBehaviour
{
    private Tilemap world;
    public Tilemap overlay;
    public TileBase previewTile;
    public static Vector3Int tilePos;

    void Awake() {
        world = gameObject.GetComponent<Tilemap>();
    }

    void Update() {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(tilePos != world.WorldToCell(pos)) {
            overlay.SetTile(tilePos, null);
            tilePos = world.WorldToCell(pos);
            overlay.SetTile(tilePos, previewTile); 
        }
        
        Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}]", tilePos.x, tilePos.y));

    }
}
