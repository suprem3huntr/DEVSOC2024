public class DrawState : State{
    private int draws;
    public DrawState(){
        currState = States.DrawState;
    }

    public override void doAction(){
        draws--;
    }

    public override bool isCondition(){
        return draws==0;
    }

    public override void ResetVariable(int d){
        draws = d;
    }
}