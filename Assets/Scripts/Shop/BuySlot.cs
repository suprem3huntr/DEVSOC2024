using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DEVSOC2024
{
    
    public class BuySlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        Card card;
        [SerializeField]
        CardDisplay display;
        void Start()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            
            if(gameManager.currstate.currState == States.GameState && gameManager.getResource() >= card.template.cost)
            {
                
                transform.parent.gameObject.SetActive(false);
                gameManager.setPlayCard(card);
                gameManager.SetPlay();

            }
            

        }
        public void setCard(Card cardinp)
        {
            card = cardinp;
            display.UpdateDisplay(card);
        }
    }
}
