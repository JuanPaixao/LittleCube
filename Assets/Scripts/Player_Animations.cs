using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void isDefeated()
    {
        _animator.SetBool("onDefeat", true);
    }
    public void isVictorious()
    {
        _animator.SetBool("onWin", true);
    }
}
