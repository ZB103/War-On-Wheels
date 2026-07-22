//Author: Small Hedge Games
//Date: 05/04/2024

namespace SHG.AnimatorCoder
{
    /// <summary> Complete list of all animation state names </summary>
    public enum Animations
    {
        //Change the list below to your animation state names
        Idle,
        Forward,
        Backward,
        Jump,
        Std_Attack,
        Spc_Attack,
        Def_Move,
        Win,
        Lose,
        RESET  //Keep Reset
    }

    /// <summary> Complete list of all animator parameters </summary>
    public enum Parameters
    {
        //Change the list below to your animator parameters
        GROUNDED,
        FALLING
    }
}


