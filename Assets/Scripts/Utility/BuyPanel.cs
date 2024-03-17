using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEVSOC2024
{
    public class BuyPanel : MonoBehaviour
    {

        public GameObject buyPanel;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void setPanelActive()
        {
            if(!buyPanel.activeSelf){
                buyPanel.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
