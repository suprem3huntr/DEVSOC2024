using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace DEVSOC2024 
{

    [CreateAssetMenu(fileName = "New Character Card",menuName = "Cards/Character Card")]
    public class CardSO : ScriptableObject
    {
        public int id = -1;
        public string cardName;
        public int cost;
        public int power;
        public string description;
        public Sprite cardImage;
        public string faction;
        public string type;
        public bool Ability;
        

        
    }
}
