using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States{
    IdleState,
    TargetState,
    GameState,
    PlayState,
}


public enum Holders{
    Resourceline,
    Frontline,
    Backline,
    
}

public enum Abilities{
    Summon,
    Destroy,
    RedPower,
    IncPower,
    RedResource,
    IncResource,
    SummonOpp,
    GenerateResource,

}

public enum CurrentAction{
    None,
    RedPower,
    Destroy,
}