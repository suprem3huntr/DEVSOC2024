using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DEVSOC2024
{
    
    public class TableCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        [SerializeField]
        Card card = null;
        CardDisplay display = null;
        [SerializeField]
        GameObject cardPrefab;
        GameManager gameManager;
        public int row;

        void Start()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {

            if(gameManager.currstate.currState == States.PlayState && transform.childCount == 0)
            {
                gameManager.PlaceCard(gameObject);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }

        public void setCard(Card card)
        {
            GameObject cardDisplay = Instantiate(cardPrefab,gameObject.transform);
            display = cardDisplay.GetComponent<CardDisplay>();
            display.UpdateDisplay(card);
        }

        public void UpdateCard(Card cardInp)
        {
            card = cardInp;
            display.UpdateDisplay(card);
        }

        public void RedPower(int p)
        {
            card.power-=p;
        }

        public void IncPower(int p)
        {
            card.power+=p;
        }
        public void DestroyCard()
        {
            Destroy(this);
        }
        
    }
}
