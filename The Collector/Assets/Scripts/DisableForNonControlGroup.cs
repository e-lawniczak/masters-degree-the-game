using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableForNonControlGroup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       if(!RuntimeVariables.IsControlGroup)
        {
            Destroy(gameObject, 0.0f);
        } 
    }

  
}
