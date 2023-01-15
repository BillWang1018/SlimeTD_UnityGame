using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerMenuOpenScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject inventory;
    public void OnPointerClick(PointerEventData eventData)
    {
        //if it's opened
        if(inventory.GetComponent<TowerMenuState>().state){
            //open inventory;
            inventory.GetComponent<TowerMenuState>().state = false;
            inventory.SetActive(false);
            return;
        }
        //if it's not opened
        inventory.GetComponent<TowerMenuState>().state = true;
        inventory.SetActive(true);
        return;
    }
}
