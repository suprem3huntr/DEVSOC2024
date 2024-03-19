using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField]
        GameObject gameOver;
        [SerializeField]
        GameObject selfTurn;
        [SerializeField]
        GameObject enemyTurn;
        [SerializeField]
        TMP_Text resultText;
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
            Debug.Log(card.power);
            rows[row].GetChild(column).GetComponent<TableCard>().UpdateCard(card);
        }

        public void Win()
        {
            gameOver.SetActive(true);
            resultText.text = "YOU WIN";
        }

        public void Tie()
        {
            gameOver.SetActive(true);
            resultText.text = "DRAW";
        }

        public void Lose()
        {
            gameOver.SetActive(true);
            resultText.text = "YOU LOSE";
        }

        public void DestroyCard(int row,int column)
        {
            GameObject target = rows[row].GetChild(column).gameObject;
            target.GetComponent<TableCard>().Kill();
        }
        public void SetTurn(bool turn)
        {
            enemyTurn.SetActive(!turn);
            selfTurn.SetActive(turn);
        }

        public void ReturntoLobby()
        {
            SceneManager.LoadScene(1);
        }


    }
}
