using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class resourceTest : MonoBehaviour,IPointerClickHandler
{
    // Start is called before the first frame update
    bool isHolding = false;
    Vector3 mousePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHolding){
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.position = new Vector3(mousePos.x,mousePos.y,0.0f);
            this.gameObject.GetComponent<Image>().color = new Color(1.0f,1.0f,1.0f,0.5f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    void OnMouseDown(){
        if(Input.GetMouseButtonDown(0)){
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            isHolding = true;
        }
    }
    void OnMouseUp(){
        isHolding = false;
    }
}
