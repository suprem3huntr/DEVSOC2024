using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DEVSOC2024.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Analytics;

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
            Debug.Log(decks.Count);

            playerNumber = (int) NetworkManager.Singleton.LocalClientId;
            
            dataHolder = GameObject.FindGameObjectWithTag("Data");
            playerDataHolder = dataHolder.GetComponent<PlayerDataHolder>();


            formDeck();

            if(!IsServer) return;
            playerResources.Add(10);
            playerResources.Add(10);

            playerScores.Add(0);
            playerScores.Add(0);
            int starter = UnityEngine.Random.Range(0,2);
            Debug.Log("Starter "+starter);
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{(ulong)starter}
                }
            };
            EndTurnClientRpc(clientRpcParams);
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        #region Public Functions

        public void PlaceCard(GameObject location)
        {
            TableCard target = location.GetComponent<TableCard>();
            PlayCardServerRpc(playCard,target.row,location.transform.GetSiblingIndex());
            SetGame();
        }

        public void setPlayCard(Card card)
        {
            playCard = card;
            
        }

        public int getResource()
        {
            return playerResources[playerNumber];
        }

        public void EndTurn()
        {
            EndTurnServerRpc();
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
            ui.removeCancel();
            ui.setEnd();
        }

        public void SetTarget()
        {
            currstate = target;
            ui.removeEnd();
        }

        public void SetPlay()
        {
            currstate = play;
            ui.removeEnd();
            ui.SetPlay();
        }

        public void AddResource()
        {
            AddResourceServerRpc();
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
            allHolders[(row + clientId * 2)%4][column] = card;
            PlayCardUIClientRpc(card,row,column,clientId);
            playerResources[clientId] -= card.template.cost;
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

        [ServerRpc(RequireOwnership = false)]
        void AddResourceServerRpc(ServerRpcParams serverRpcParams = default)
        {
            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            
            
            playerResources[clientId] -= 2;
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{(ulong)clientId}
                }
            };
            UpdateResourcesClientRpc(playerResources[clientId],clientRpcParams);

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
            if(currstate.currState != States.IdleState)
            {
                SetIdle();
                
            }
            else
            {
                SetGame();
                
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
        void UpdateResourcesClientRpc(int resource,ClientRpcParams clientRpcParams)
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



        #endregion
    }
}
