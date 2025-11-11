using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void PlayIdle()
    {
        if (anim)
            anim.Play("Flag_Idle");
    }

    public void PlayOut()
    {
        if (anim)
            anim.Play("Flag_Out");
    }
}
