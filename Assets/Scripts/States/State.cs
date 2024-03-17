using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected State nextState;
    public States currState;

    public virtual bool isCondition(){
        return true;
    }
    public virtual void doAction(){
        return;
    }
    public virtual State NextState(){
        return nextState;
    }
    public virtual void ResetVariable(int a){
        return;
    }
    public virtual void SetNextState(State s){
        nextState = s;
    }
}
