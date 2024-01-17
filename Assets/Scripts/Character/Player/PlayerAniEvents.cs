using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniEvents : MonoBehaviour
{
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player=GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AnimationTrigger()
    {
        player.AttackOver();
    }
}
