public class TargetState : State{
    private int targets;
    public TargetState(){
        currState = States.TargetState;
    }

    public override void doAction(){
        targets--;
    }

    public override bool isCondition(){
        return targets==0;
    }

    public override void ResetVariable(int t){
        targets = t;
    }
}