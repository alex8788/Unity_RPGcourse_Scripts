using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimeEvents : MonoBehaviour
{
    private Player player;


    void Start()
    {
        player = GetComponentInParent<Player>();
    }


    void Update()
    {
        
    }


    private void AnimationTrigger()
    {
        player.AttackOver();
    }
}
