using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024
{
    public class CardInstance : MonoBehaviour
    {
        Card card;
        public int row = -1;
        [SerializeField]
        private CardDisplay display;
        private GameManager gameManager;

        void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        public void UpdateCard(Card cardInp)
        {
            card = cardInp;
            display.UpdateDisplay(card,row);
        }

        public void UpdateRow(GameObject destination)
        {
            gameObject.transform.SetParent(destination.transform);
            row = destination.GetComponent<CardHolderUI>().holderID;
            display.UpdateDisplay(card,row);
        }

        
    }
}
