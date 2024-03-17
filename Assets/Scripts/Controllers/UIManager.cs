using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024
{
    public class UIManager : MonoBehaviour
    {
        int playerNumber;
        [SerializeField]
        GameObject cancelButton;
        [SerializeField]
        List<Transform> rows;
        [SerializeField]
        Transform BuyPanel;
        [SerializeField]
        GameObject buySlot;
        public void setPlayerNumber(int pno)
        {
            playerNumber = pno;
        }
        public void SetPlay()
        {
            cancelButton.SetActive(true);
        }
        public void removeCancel()
        {
            cancelButton.SetActive(false);
        }
        public void PlayCard(Card card,int row,int column)
        {
            rows[row].transform.GetChild(column).GetComponent<TableCard>().setCard(card);
        }
        public void addToShop(Card card)
        {
            
            GameObject newCard = Instantiate(buySlot,BuyPanel);
            newCard.GetComponent<BuySlot>().setCard(card);
        }
    }
}
