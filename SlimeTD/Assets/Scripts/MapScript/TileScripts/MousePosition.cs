using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MousePosition : MonoBehaviour
{
    private Tilemap world;
    public Tilemap overlay;
    private TileBase previewTile;
    public static Vector3Int tilePos;
    private BuildManager buildManager;

    void Start() {
        buildManager = BuildManager.instance;
        world = gameObject.GetComponent<Tilemap>();
    }

    void Update() {
        previewTile = buildManager.selectedBuilding;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(buildManager.checkValid()) {
            overlay.color = new Color(225,225,225,0.7f);
        } else {
            overlay.color = new Color(225,0,0,0.8f);
        }
        if(tilePos != world.WorldToCell(pos)) {
            overlay.SetTile(tilePos, null);
            tilePos = world.WorldToCell(pos);
            overlay.SetTile(tilePos, previewTile); 

        }
        
    
        // Debug.Log(string.Format("Co-ords of mouse is [X: {0} Y: {1}]", tilePos.x, tilePos.y));
    }
}
