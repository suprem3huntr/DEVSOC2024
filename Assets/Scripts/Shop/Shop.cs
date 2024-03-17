using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DEVSOC2024
{
    public class Shop : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField]
        GameObject BuyPanel;
        public void OnPointerClick(PointerEventData eventData)
        {
            BuyPanel.SetActive(true);
        }
        

        
    }
}
