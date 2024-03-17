using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DEVSOC2024
{
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField]
        TMP_Text cost;
        TMP_Text power;
        TMP_Text description;
        Image character;
        TMP_Text troopType;
        TMP_Text troopFaction;
        public void UpdateDisplay(Card card)
        {
            cost.text = ""+card.template.cost;
            power.text = ""+card.power;
            character.sprite = card.template.cardImage;
            troopType.text = card.template.type;
            troopFaction.text = card.template.faction;
        }
    }
}
