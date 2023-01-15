using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TowerMenuItemScript : MonoBehaviour,IPointerClickHandler
{
    //what TowerData should that item have
    public TowerData towerdata;
    //get UITileMap
    public MapManager mm;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData){
        //set the Mapmanager Select Tower
        mm.setCurrentSelectedData(towerdata);
    }

    void Start(){

    }
    void Update(){

    }
}
