using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DEVSOC2024
{
    public class CardDisplay : MonoBehaviour
    {
        private GameManager gameManager;
        
        [SerializeField]
        GameObject front;
        [SerializeField]
        Image back;
        [SerializeField]
        Image namePlate;
        [SerializeField]
        Image background;
        [SerializeField]
        TMP_Text attack;
        [SerializeField]
        TMP_Text health;
        [SerializeField]
        TMP_Text nameText;
        [SerializeField]
        CardMana costHolder;
        List<int> hidden = new List<int>();
    

        void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            for(int i=0;i<2;i++)
            {
                hidden.Add((gameManager.numberOfHolders/2)*i + (int) Holders.Deck);
                hidden.Add((gameManager.numberOfHolders/2)*i + (int) Holders.Graveyard);
                hidden.Add((gameManager.numberOfHolders/2)*i + (int) Holders.Discard);
                
            }

        }

        public void UpdateDisplay(Card card, int row)
        {
            if(hidden.Contains(row))
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                if (row == gameManager.numberOfHolders/2)
                {
                    back.gameObject.SetActive(true);
                    front.SetActive(false);
                }
                else
                {
                    back.gameObject.SetActive(false);
                    front.SetActive(true);
                    namePlate.sprite = card.card.cardNameplate;
                    background.sprite = card.card.cardBackground;
                    nameText.text = card.card.cardName;
                    attack.text = ""+card.power;
                    health.text = ""+card.health;
                    costHolder.UpdateMana(card.cost);
                }
            }
        }
    }
}
