using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeAnimationScript : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        this.GetComponentInParent<WeaponScript>().DisableTrigger();
    }
}
