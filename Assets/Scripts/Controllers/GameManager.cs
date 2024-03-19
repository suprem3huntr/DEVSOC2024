using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DEVSOC2024.Utilities;
using Unity.Netcode;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

namespace DEVSOC2024
{
    public class GameManager : NetworkBehaviour
    {
        PlayerDataHolder playerDataHolder;
        UIManager ui;
        GameObject dataHolder;
        List<List<Card>> allHolders = new List<List<Card>>();
        public int numberOfHolders = 4;// 0)Backline 1)Frontline
        int playerNumber;
        List<List<Card>> decks = new List<List<Card>>();
        int turn = 0;
        NetworkList<int> playerResources = new NetworkList<int>();
        List<int> playerScores = new List<int>();
        
        public State currstate;
        IdleState idle = new IdleState();
        GameState game = new GameState();
        PlayState play = new PlayState();
        TargetState target = new TargetState();
        public Card playCard;
        CurrentAction currentAction = CurrentAction.None;
        bool free;
        int abilityValue = 0;
        
        void Awake()
        {
            ui = gameObject.GetComponent<UIManager>();
            ui.setPlayerNumber(playerNumber);   
            SetIdle();
            
            
        }
        void Start()
        {
            
        }

        public override void OnNetworkSpawn()
        {

            for (int i=0; i<numberOfHolders;i++)
            {
                allHolders.Add(new List<Card>());
                
                for(int j = 0;j<5;j++)
                {
                    allHolders[i].Add(new Card());
                }
                
            }
            
            decks.Add(new List<Card>());
            decks.Add(new List<Card>());
;

            playerNumber = (int) NetworkManager.Singleton.LocalClientId;
            
            dataHolder = GameObject.FindGameObjectWithTag("Data");
            playerDataHolder = dataHolder.GetComponent<PlayerDataHolder>();


            formDeck();

            if(!IsServer) return;
            playerResources.Add(10);
            playerResources.Add(10);

            playerScores.Add(0);
            playerScores.Add(0);
            int starter = 0;

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{(ulong)starter}
                }
            };
            EndTurnClientRpc(clientRpcParams);
            
        }



        #region Public Functions
        public void PlaceCard(GameObject location)
        {

            TableCard target = location.GetComponent<TableCard>();
            Abilities curr = playCard.template.abilities;
            PlayCardServerRpc(playCard,target.row,location.transform.GetSiblingIndex());
            if(curr == Abilities.Summon || curr == Abilities.Destroy || curr == Abilities.RedPower)
            {

            }
            else
            {
                SetGame();
            }
            
            
        }

        public void setPlayCard(Card card)
        {
            playCard = new Card(card.template.id);

            
        }

        public int getResource()
        {
            return playerResources[playerNumber];
        }

        public void EndTurn()
        {
            EndTurnServerRpc();
        }

        public void completeAction(GameObject location)
        {
            TableCard target = location.GetComponent<TableCard>();
            SetGame();
            if(currentAction == CurrentAction.RedPower)
            {
                RedPowerServerRpc(abilityValue, target.row, location.transform.GetSiblingIndex());
            }
            else if(currentAction == CurrentAction.Destroy)
            {
                DestroyServerRpc(target.row, location.transform.GetSiblingIndex());
            }
        }

        public void checkAbility(Card card, int clientId, int row = -1, int column = -1)
        {
            if(card.template.ability){
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[]{(ulong)clientId}
                    }
                };

                if(card.template.abilities == Abilities.Summon){
                    free = true;
                    SummonAbilityClientRpc(card, clientRpcParams);
                }
                else if(card.template.abilities == Abilities.Destroy){
                    DestroyAbilityClientRpc(card, clientRpcParams);
                }
                else if(card.template.abilities == Abilities.RedPower){
                    abilityValue = card.template.abilityValue;
                    RedPowerAbilityClientRpc(card, clientRpcParams);                    
                }
                else if(card.template.abilities == Abilities.IncPower){
                    if(column>0)
                    {

                        if(allHolders[row][column-1].template != null)
                        {
                            
                            allHolders[row][column-1].power += card.template.abilityValue;
                            UpdateCardClientRpc(allHolders[row][column-1],row,column-1);
                        }
                    }
                    if(column<4)
                    {
                        Debug.Log("Row: "+row+"  "+column);
                        if(allHolders[row][column+1].template != null)
                        {
                            
                            allHolders[row][column+1].power += card.template.abilityValue;
                            UpdateCardClientRpc(allHolders[row][column+1],row,column+1);
                        }
                    }
                    UpdateScoresValue();
                    UpdateScoresClientRpc(playerScores[1],playerScores[0]);
                }
                else if(card.template.abilities == Abilities.RedResource){
                    
                    playerResources[(clientId==1)?0:1] -= card.template.abilityValue;
                    clientRpcParams.Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[]{(ulong)(1-clientId)}
                    };
                    UpdateResourcesClientRpc(playerResources[1-clientId],clientRpcParams);
                }
                else if(card.template.abilities == Abilities.IncResource){
                    playerResources[(clientId==1)?0:1] += card.template.abilityValue;
                    clientRpcParams.Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[]{(ulong)(1-clientId)}
                    };
                    UpdateResourcesClientRpc(playerResources[1-clientId],clientRpcParams);
                }
                else if(card.template.abilities == Abilities.SummonOpp){
                    if (column>0)
                    {

                        if(allHolders[row][column-1].template != null)
                        {
                            allHolders[row][column-1].power -= card.template.abilityValue;
                            UpdateCardClientRpc(allHolders[row][column-1],row,column-1);
                            if(allHolders[row][column-1].power < 0)
                            {
                                DestroyClientRpc(row,column-1);
                            }
                        }
                    }
                    if(column<4)
                    {

                        if(allHolders[row][column+1].template != null && column<5)
                        {
                            allHolders[row][column+1].power -= card.template.abilityValue;
                            UpdateCardClientRpc(allHolders[row][column+1],row,column+1);
                            if(allHolders[row][column+1].power < 0)
                            {
                                DestroyClientRpc(row,column+1);
                            }
                        }
                    }
                    UpdateScoresValue();
                    UpdateScoresClientRpc(playerScores[1],playerScores[0]);
                    
                }
            }
        }
        #endregion

        

        #region State Switchers
        public void SetIdle()
        {
            currstate = idle;
            ui.removeEnd();
        }

        public void SetGame()
        {
            currstate = game;
            playCard = null;
            free = false;
            abilityValue = 0;
            ui.removeCancel();
            ui.setEnd();
        }

        public void SetTarget()
        {
            Debug.Log("Target");
            currstate = target;
            ui.removeEnd();
            ui.SetPlay();
        }

        public void SetPlay()
        {
            currstate = play;
            ui.removeEnd();
            ui.SetPlay();
        }

        
        

        #endregion



        #region Self Functions
        private void formDeck()
        {
            List<int> deck = playerDataHolder.deck;
            foreach (int cardId in deck)
            {
                Card card = new Card(cardId);
                AddToDeckServerRpc(card);
            }
        }

        private void UpdateScoresValue()
        {
            
            playerScores[0] = allHolders.Take(2).Sum(holder => holder.Sum(card => card.power));
            playerScores[1] = allHolders.Skip(2).Sum(holder => holder.Sum(card => card.power));
        }

        private void endGame()
        {
            endGameClientRPC(playerScores[0],playerScores[1]);
            NetworkManager.Singleton.Shutdown();
        }

        #endregion



        #region ServerRpcs
        [ServerRpc(RequireOwnership = false)]
        void AddToDeckServerRpc(Card card,ServerRpcParams serverRpcParams = default)
        {
            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            decks[clientId].Add(card);
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{(ulong)clientId}
                }
            };
            UIAddtoShopClientRpc(card,clientRpcParams);
            
        }

        [ServerRpc(RequireOwnership = false)]
        void PlayCardServerRpc(Card card,int row,int column,ServerRpcParams serverRpcParams = default)
        {

            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            int newRow = (row + clientId * 2)%4;
            allHolders[newRow][column] = card;
            PlayCardUIClientRpc(card,row,column,clientId);
            if(free)
            {
                free = false;
            }
            else
            {
                playerResources[clientId] -= card.template.cost;
            }
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{(ulong)clientId}
                }
            };
            UpdateResourcesClientRpc(playerResources[clientId],clientRpcParams);
            UpdateScoresValue();
            UpdateScoresClientRpc(playerScores[1],playerScores[0]);
            checkAbility(card, clientId, newRow, column);

        }

        [ServerRpc(RequireOwnership = false)]
        void RedPowerServerRpc(int abilityValue, int row, int column, ServerRpcParams serverRpcParams = default)
        {
            Debug.Log("Target row: "+row);
            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            int newRow = (row + clientId * 2)%4;
            allHolders[newRow][column].power -= abilityValue;
            UpdateCardClientRpc(allHolders[newRow][column],newRow,column);
            if(allHolders[newRow][column].power < 0)
            {
                DestroyClientRpc(newRow,column);
            }
            UpdateScoresValue();
            UpdateScoresClientRpc(playerScores[1],playerScores[0]);
        }
        
        [ServerRpc(RequireOwnership = false)]
        void DestroyServerRpc(int row, int column, ServerRpcParams serverRpcParams = default)
        {
            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            int newRow = (row + clientId * 2)%4;
            allHolders[newRow][column] = new Card();
            DestroyClientRpc(newRow,column);
            UpdateScoresValue();
            UpdateScoresClientRpc(playerScores[1],playerScores[0]);
        }

        [ServerRpc(RequireOwnership = false)]
        void EndTurnServerRpc()
        {
            turn++;
            if(turn == 10)
            {
                endGame();
                return;
            }
            
            int increase = 0;
            for(int i = 0;i<2;i++)
            {
                for(int j = 0; j< 5;j++)
                {
                    Card card = allHolders[i+(2*(turn%2))][j];
                    
                    if(card.template == null)
                    {
                        continue;
                    }
                    if(card.template.abilities == Abilities.IncResource)
                    {
                        increase += card.template.abilityValue;
                    }
                }
            }
            
            playerResources[turn%2] += increase;

            EndTurnClientRpc();
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{(ulong)(turn%2)}
                }
            };
            UpdateResourcesClientRpc(playerResources[turn%2],clientRpcParams);
        }

        

        

        #endregion



        #region ClientRpcs
        [ClientRpc]
        void PlayCardUIClientRpc(Card card,int row,int column,int player)
        {
            if(player != playerNumber)
            {
                row = (row+2)%4;
                
            }
            ui.PlayCard(card,row,column);
        }

        [ClientRpc]
        void UIAddtoShopClientRpc(Card card,ClientRpcParams clientRpcParams = default)
        {
            ui.addToShop(card);
        }

        [ClientRpc]
        void EndTurnClientRpc(ClientRpcParams clientRpcParams = default)
        {
            Debug.Log("Starting");
            if(currstate.currState != States.IdleState)
            {
                SetIdle();
                ui.SetTurn(false);
                
            }
            else
            {
                
                SetGame();
                ui.SetTurn(true);
                
            }
        }

        [ClientRpc]
        void UpdateScoresClientRpc(int enemyScore,int playerScore)
        {
            if(playerNumber==0)
            {
                ui.UpdateScores(playerScore,enemyScore);
            }
            else
            {
                ui.UpdateScores(enemyScore,playerScore);
            }
        }

        [ClientRpc]
        void UpdateResourcesClientRpc(int resource,ClientRpcParams clientRpcParams = default)
        {
            ui.UpdateResources(resource);
        }

        [ClientRpc]
        void endGameClientRPC(int player0,int player1)
        {
            SetIdle();
            if(player0==player1)
            {
                ui.Tie();
            }
            else if(playerNumber == 0)
            {
                if(player0 > player1)
                {
                    ui.Win();
                }
                else
                {
                    ui.Lose();
                }
            }
            else
            {
                if(player0 < player1)
                {
                    ui.Win();
                }
                else
                {
                    ui.Lose();
                }
            }
        }

        [ClientRpc]
        void SummonAbilityClientRpc(Card card, ClientRpcParams clientRpcParams = default)
        {

            setPlayCard(new Card(card.template.abilityValue));
            SetPlay();
        }

        [ClientRpc]
        void DestroyAbilityClientRpc(Card card, ClientRpcParams clientRpcParams = default)
        {
            currentAction = CurrentAction.Destroy;
            SetTarget();
        }

        [ClientRpc]
        void RedPowerAbilityClientRpc(Card card, ClientRpcParams clientRpcParams = default)
        {
            abilityValue = card.template.abilityValue;
            currentAction = CurrentAction.RedPower;
            SetTarget();
        }

        [ClientRpc]
        void UpdateCardClientRpc(Card card,int row,int column)
        {
            if(playerNumber == 1)
            {
                row = (row+2)%4;
            }
            ui.UpdateCard(card,row,column);
            
        }

        [ClientRpc]
        void DestroyClientRpc(int row,int column)
        {
            if(playerNumber == 1)
            {
                row = (row+2)%4;
            }
            ui.DestroyCard(row,column);
        }


        #endregion
    }
}
