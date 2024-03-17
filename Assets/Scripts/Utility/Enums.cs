using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CostType
{
    None,
    Mana,
    Troop,
    Hand,
    Graveyard,
    Damage,
}
public enum EffectType
{
    None,
    Draw,
    Summon,
    AtkBoost,
    Heal,
    Damage,
    TwinStrike,
    Destroy

}
public enum TargetType
{
    Player,
    Troop,
    Row,
    Position,
    All,
    
}
public enum TriggerType
{
    OnPlace,
    OnAttack,
    OnHit,
    OnDeath,
    OnTrigger,
    Condition,
    onMove
}

public enum ConditionType
{
    Troop,
    Mana,
}

public enum SummonLocation
{
    Graveyard,
    OpponentGraveyard,
    Hand,
    Deck,
    Instant,
    OpponentHand,
    OpponentDeck,
}

public enum States{
    IdleState,
    TargetState,
    AttackState,
    DrawState,
    PlayState,
    GuardState,
    AttackTargetState,
}

public enum Holders{
    Deck,
    Hand,
    BackRow,
    FrontRow,
    Graveyard,
    Discard
}