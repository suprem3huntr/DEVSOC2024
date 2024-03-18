using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DEVSOC2024
{
    public class PlayerDataHolder : MonoBehaviour
    {
        public List<int> deck = new List<int>();
        public List<int> unlockedCards = new List<int>();

        void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Data");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
        public void AddCard(CardSO card)
        {
            if (deck.Count == 60)
            {
                throw new DeckFullException();
            }
            else
            {
                int count = 0;
                for(int j = 0; j<deck.Count;j++)
                {
                    if(deck[j] == card.id)
                    {
                        count++;
                    }
                    if (count == 4)
                    {
                        throw new OverStockException();
                    }
                }
                deck.Add(card.id);
            }
            
                
            

        }

        public void SetDefaultCards()
        {
            for(int i = 0; i < 10; i++)
            {
                this.deck.Add(i);
                this.unlockedCards.Add(i);
            }
        }

        public string ConvertCardDataToString()
        {
            string str = "UnlockedCards: ";
            for(int i = 0; i < unlockedCards.Count; i++)
            {
                str += unlockedCards.ElementAt(i).ToString() + " ";
                
            }
            str += "Deck: ";
            for(int i = 0; i < deck.Count; i++)
            {
                str += deck.ElementAt(i).ToString() + " ";
            }

            return str;
        }
        
    }
}
