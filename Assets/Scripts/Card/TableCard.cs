using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DEVSOC2024
{
    
    public class TableCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        [SerializeField]
        Card card = null;
        [SerializeField]
        CardDisplay display;
        [SerializeField]
        GameObject cardPrefab;
        GameManager gameManager;
        public int row;
        public Card target;

        void Start()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            

            if(gameManager.currstate.currState == States.PlayState && transform.childCount == 0)
            {
                if(row<2 || gameManager.playCard.template.abilities == Abilities.SummonOpp)
                {
                    gameManager.PlaceCard(gameObject);
                }
            }
            if(gameManager.currstate.currState == States.TargetState && transform.childCount != 0)
            {
                gameManager.completeAction(gameObject);
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
            Debug.Log(card);
            card = cardInp;
            
            display.UpdateDisplay(card);
        }

        public void Kill()
        {
            GameObject.Destroy(display.gameObject);
            display = null;
            card = null;
            
        }
    }
}
