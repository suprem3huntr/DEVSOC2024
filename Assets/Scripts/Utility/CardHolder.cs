using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024.Utilities
{
    public class CardHolder
    {
        List<Card> cards = new List<Card>();

        public void moveTo(CardHolder reciever,int i)
        {
            reciever.Add(cards[i]);
            cards.RemoveAt(i);
        }
        public void Add(Card i)
        {
            cards.Add(i);
        }
        public void Remove(int i)
        {
            cards.RemoveAt(i);
        }
    }
}
