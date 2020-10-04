using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player_Movement))]
[RequireComponent(typeof(Controller2D))]
public class Game_Mech : MonoBehaviour
{
    private Player_Movement player_movement;
    private Controller2D controller;
    void Start()
    {
        player_movement = GetComponent<Player_Movement>();
        controller = GetComponent<Controller2D>();
    }
    void SecondJumpPower()
    {
    }

}
