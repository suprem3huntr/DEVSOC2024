using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField]
        GameObject endTurn;
        [SerializeField]
        TMP_Text playerScore;
        [SerializeField]
        TMP_Text enemyScore;
        [SerializeField]
        TMP_Text resources;
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
        public void removeEnd()
        {
            endTurn.SetActive(false);
        }
        public void setEnd()
        {
            endTurn.SetActive(true);
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

        public void UpdateScores(int player,int enemy)
        {
            playerScore.text = "" + player;;
            enemyScore.text = "" + enemy;
        }
        public void UpdateResources(int resourceAmt)
        {
            resources.text = ""+resourceAmt;
        }

        public void UpdateCard(Card card,int row,int column)
        {
            Debug.Log(row+" "+column);
            rows[row].GetChild(column).GetComponent<TableCard>().UpdateCard(card);
        }

        public void Win()
        {
            Debug.Log("win");
        }

        public void Tie()
        {
            Debug.Log("tie");
        }

        public void Lose()
        {
            Debug.Log("lose");
        }


    }
}
