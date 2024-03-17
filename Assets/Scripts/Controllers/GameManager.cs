using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;



//0)Deck,1)Hand,2)BackRow,3)FrontRow,4)Graveyard,5)Discard
namespace DEVSOC2024
{
    public class GameManager : NetworkBehaviour
    {
        List<Utilities.CardHolder> allHolders = new List<Utilities.CardHolder>();
        public int numberOfHolders = 12;
        private int playerNumber;
        GameObject dataHolder;
        PlayerDataHolder playerDataHolder;
        UIManager ui;

        #region  MonoBehaviour Functions

        void Awake()
        {

            if(!IsServer) return;
            for (int i=0; i<numberOfHolders;i++)
            {
                allHolders.Add(new Utilities.CardHolder());
            }
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

        #endregion

        #region Holder Functions

        public void addToHolder(int holder,Card card)
        {
            allHolders[holder].Add(card);
        }

        #endregion

        #region Self Functions

        void formDeck()
        {
            List<int> deck = playerDataHolder.deck;
            foreach (int cardId in deck)
            {
                Card card = new Card(cardId);
                AddToHolderServerRpc(Holders.Deck,card);
            }
        }

        #endregion


        #region  ServerRpcs

        [ServerRpc]
        void AddToHolderServerRpc(Holders holder,Card card,bool toEnemy = false,ServerRpcParams serverRpcParams = default)
        {
            int boolFix = toEnemy?1:0;
            int clientId = (int)serverRpcParams.Receive.SenderClientId;
            int reciever = boolFix ^ clientId;
            int holderIndex = (numberOfHolders/2)*reciever + (int)holder;
            addToHolder(holderIndex,card);
            AddToHolderUIClientRpc(holderIndex,card);
        }

        #endregion

        #region ClientRpcs

        [ClientRpc]
        void AddToHolderUIClientRpc(int holder,Card card)
        {
            ui.AddToHolder(holder,card);
        }

        #endregion


    }
}
