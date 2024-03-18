using System.Collections;
using System.Collections.Generic;
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
        
        NetworkList<int> playerResources = new NetworkList<int>();
        public State currstate;
        IdleState idle = new IdleState();
        GameState game = new GameState();
        PlayState play = new PlayState();
        TargetState target = new TargetState();
        public Card playCard;
        
        void Awake()
        {
            
            currstate = idle;
            
            
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
            ui = gameObject.GetComponent<UIManager>();
            ui.setPlayerNumber(playerNumber);
            dataHolder = GameObject.FindGameObjectWithTag("Data");
            playerDataHolder = dataHolder.GetComponent<PlayerDataHolder>();


            formDeck();

            if(!IsServer) return;
            playerResources.Add(10);
            playerResources.Add(10);

            int starter = Random.Range(0,2);
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
            ui.setEnd();
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
        }

        [ServerRpc(RequireOwnership = false)]
        void EndTurnServerRpc()
        {
            EndTurnClientRpc();
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
                currstate = idle;
                
            }
            else
            {
                currstate = game;
                
            }
        }

        #endregion
    }
}
