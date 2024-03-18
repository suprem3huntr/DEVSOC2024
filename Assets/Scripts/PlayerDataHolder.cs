using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024
{
    public class PlayerDataHolder : MonoBehaviour
    {
        public List<int> deck = new List<int>();

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
        
    }
}
