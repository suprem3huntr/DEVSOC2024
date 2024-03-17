using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024
{
    public class Card : INetworkSerializable
    {
        public CardSO card;
        public int power;
        public int health;
        public int cost;
      
        
        public Card(int cardId, int Power = -1,int Health = -1, int Cost = -1,int powerMod = 0, int healthMod = 0,int costMod = 0)
        {
            card = CardDatabase.Singleton.GetCard(cardId);
            power = (Power == -1? card.atk:Power) + powerMod;
            health = (Health == -1? card.def : Health) + healthMod;
            cost = (Cost == -1? card.cost : Cost) + costMod;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            int cardId = -1;
            if(!serializer.IsReader)
            {
                cardId = card.id;
            }
            serializer.SerializeValue(ref cardId);
            serializer.SerializeValue(ref power);
            serializer.SerializeValue(ref health);
            serializer.SerializeValue(ref cost);

            if(serializer.IsReader)
            {
                card = CardDatabase.Singleton.GetCard(cardId);
            }
        }
    }
}
