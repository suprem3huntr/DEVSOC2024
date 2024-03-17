using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024
{
    public class UIManager : MonoBehaviour
    {
        int playerNumber;
        [SerializeField]
        GameObject cardPrefab;
        [SerializeField]
        List<GameObject> holders;
        int numberOfHolders;
        void Awake()
        {
            numberOfHolders = holders.Count;
        }
        public void AddToHolder(int holder, Card card)
        {
            GameObject instance = Instantiate(cardPrefab);
            CardInstance instancecontrol = instance.GetComponent<CardInstance>();
            instancecontrol.UpdateCard(card);
            int destination = holder + (numberOfHolders/2)*playerNumber;
            instancecontrol.UpdateRow(holders[destination]);
        }

        public void setPlayerNumber(int pno)
        {
            playerNumber = pno;
        }
    }

    
}
