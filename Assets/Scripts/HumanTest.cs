using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTest : MonoBehaviour
{
    Animator _anim => GetComponent<Animator>();


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _anim.SetBool("Walk", true);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _anim.SetBool("Walk", false);
            _anim.SetBool("Look", false);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _anim.SetBool("Look", true);
        }
    }
}
