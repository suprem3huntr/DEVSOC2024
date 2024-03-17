using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024
{
    public class CardMana : MonoBehaviour
    {
        [SerializeField]
        GameObject manaImage;
        int current = 0;
        public void UpdateMana(int newCost)
        {
            newCost = current - newCost;
            if (newCost>0)
            {
                for(int i=0;i<newCost;i++)
                {
                    Instantiate(manaImage,gameObject.transform);
                }
            }
            else
            {
                for(int i=0;i<-1*newCost;i++)
                {
                    Destroy(gameObject.transform.GetChild(0));
                }
            }
        }
    }
}
