using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyToClipboard : MonoBehaviour
{
    public void Copy(string textToCopy)
    {
        GUIUtility.systemCopyBuffer = textToCopy;
    }
}
