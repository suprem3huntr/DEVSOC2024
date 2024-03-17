using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024
{
    public class Card : INetworkSerializable
    {
        CardSO template;
        int power;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            int cardId = -1;
            if(!serializer.IsReader)
            {
                cardId = template.id;
            }
            serializer.SerializeValue(ref cardId);
            serializer.SerializeValue(ref power);
            
            

            if(serializer.IsReader)
            {
                template = CardDatabase.Singleton.GetCard(cardId);
            }
        }
    }
}
