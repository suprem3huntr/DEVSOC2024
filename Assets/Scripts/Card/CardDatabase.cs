using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data.Common;
using Unity.VisualScripting;

namespace DEVSOC2024
{

    [CreateAssetMenu(fileName = "Card Database",menuName ="Cards/Database")]
    public class CardDatabase : SingletonSriptableObject<CardDatabase>
    {
        [SerializeField] private List<CardSO> allCards;

        [ContextMenu(itemName:"Set IDs")]
        private void SetItemIDs()
        {
            allCards = new List<CardSO>();

            var foundCards = Resources.LoadAll<CardSO>("Cards").OrderBy(i => i.id).ToList();
            var hasIDinRange = foundCards.Where(i => i.id != -1 && i.id <foundCards.Count).OrderBy(i => i.id).ToList();
            var hasIDNotinRange = foundCards.Where(i => i.id != -1 && i.id >= foundCards.Count).OrderBy(i => i.id).ToList();
            var noID = foundCards.Where(i => i.id <= -1).OrderBy(i => i.id).ToList();

            var index = 0;
            for (int i=0; i<foundCards.Count;i++)
            {
                CardSO cardToAdd;
                cardToAdd = hasIDinRange.Find(d => d.id == i);

                if (cardToAdd != null)
                {
                    allCards.Add(cardToAdd);
                }
                else if (index <= noID.Count)
                {
                    noID[index].id = i;
                    cardToAdd = noID[index];
                    index++;
                    allCards.Add(cardToAdd);
                }
            }
            foreach(var item in hasIDNotinRange)
            {
                allCards.Add(item);
            }
        }
        public CardSO GetCard(int index)
        {
            return allCards[index];
        }
        
        
    }
}
