using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    //enums for the state of the slingshot, the 
    //state of the game and the state of the bird
    public enum CannonState
    {
        Idle,
        UserAiming,
        WolfFlying
    }

    public enum GameState
    {
        Start,
        WolfMovingToCannon,
        Playing,
        Won,
        Lost
    }


    public enum WolfState
    {
        BeforeShot,
        Shot
    }
    
}
