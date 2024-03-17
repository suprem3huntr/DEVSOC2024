public class AttackTargetState : State{
    private int targets;
    public AttackTargetState(){
        currState = States.AttackTargetState;
    }

    public override void doAction(){
        targets--;
    }

    public override void ResetVariable(int t){
        targets = t;
    }
}