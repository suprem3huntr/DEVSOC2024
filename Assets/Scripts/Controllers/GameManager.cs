using System.Collections;
using System.Collections.Generic;
using DEVSOC2024.Utilities;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024
{
    public class GameManager : NetworkBehaviour
    {
        PlayerDataHolder playerDataHolder;
        UIManager ui;
        GameObject dataHolder;
        List<CardHolder> allHolders;
        public int numberOfHolders = 6; //0)Resourceline 1)Backline 2)Frontline
        int playerNumber;
        List<List<Card>> decks = new List<List<Card>>();
        
        void Awake()
        {
            if(!IsServer) return;
            for (int i=0; i<numberOfHolders;i++)
            {
                allHolders.Add(new CardHolder());
            }
            decks.Add(new List<Card>());
            decks.Add(new List<Card>());
        }
        void Start()
        {
            playerNumber = (int) NetworkManager.Singleton.LocalClientId;
            ui = gameObject.GetComponent<UIManager>();
            ui.setPlayerNumber(playerNumber);
            dataHolder = GameObject.FindGameObjectWithTag("Data");
            playerDataHolder = dataHolder.GetComponent<PlayerDataHolder>();

            

            formDeck();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

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

        [ServerRpc]
        void AddToDeckServerRpc(Card card,ServerRpcParams serverRpcParams = default)
        {
            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            decks[clientId].Add(card);
        }

        #endregion
    }
}
