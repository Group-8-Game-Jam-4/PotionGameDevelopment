using UnityEngine;

public class furnacePump : MonoBehaviour
{
    public Animator pumpAnim;
    public furnaceController furnaceController;
    public void pumpFurnace()
    {
        Debug.Log("Pumping");
        pumpAnim.SetBool("pump", true);
    }

    public void stopPump()
    {
        furnaceController.progressLvl();
        pumpAnim.SetBool("pump", false);
    }




}
