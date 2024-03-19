using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024
{

    public class Card : INetworkSerializable
    {
        public CardSO template = null;
        public int power;
        int cardId;
        public Card(int cardIdinp)
        {
            cardId = cardIdinp;
            template = CardDatabase.Singleton.GetCard(cardId);
            power = template.power;
            
        }
        public Card()
        {
            template = null;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            
            
            
            serializer.SerializeValue(ref cardId);
            serializer.SerializeValue(ref power);
            
            

            if(serializer.IsReader)
            {
                template = CardDatabase.Singleton.GetCard(cardId);
            }
        }
    }
}
