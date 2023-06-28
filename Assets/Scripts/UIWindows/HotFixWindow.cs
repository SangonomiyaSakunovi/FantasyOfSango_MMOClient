using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.EventSystems;

//Developer: SangonomiyaSakunovi

public class HotFixWindow : MonoBehaviour
 {
     //获取需要点击的按钮
    public Button UpdateWindowButton;

    public void UpdateWindowButtonClick()
     {

        //代码实现按钮的点击
        ExecuteEvents.Execute(UpdateWindowButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
     }
 }
