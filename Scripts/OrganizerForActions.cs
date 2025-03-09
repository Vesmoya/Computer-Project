using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Main
{
    public class OrganizerForActions : MonoBehaviour
    {
        InterfaceCancel currentAct;
        
        public void NewAct(InterfaceCancel act)
        {
            if (currentAct == act) return;
            if(currentAct != null)
            {
                currentAct.moveStop();
            }
            
            currentAct = act;
        }
        public void CancelAct()
        {
            NewAct(null);
        }
    }
}
