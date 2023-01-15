using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerMenuCloseScript : MonoBehaviour,IPointerClickHandler
{
    public GameObject inventory;
    public void OnPointerClick(PointerEventData eventData)
    {
        //closed
        inventory.GetComponent<TowerMenuState>().state = false;
    }
}
