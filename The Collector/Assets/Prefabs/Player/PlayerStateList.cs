using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateList : MonoBehaviour
{
    // script source: https://www.reddit.com/r/Unity2D/comments/arb0tp/hollowknight_style_movement/
    // https://pastebin.com/JYnRcPZ6

    public bool walking;
    public bool interact;
    public bool interacting;
    public bool lookingRight;
    public bool jumping;
    public bool recoilingX;
    public bool recoilingY;
    public bool casting;
    public bool castReleased;
    public bool atNPC;
    public bool usingNPC;
    public bool jumpedOnSpikes;
    public bool dashing;
    public bool canJumpAgain;
}