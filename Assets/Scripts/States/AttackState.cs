public class AttackState : State{
    bool turnOne = false;
    public AttackState(){
        currState = States.AttackState;
    }
    public override bool isCondition()
    {
        return turnOne;
    }
    public override void doAction()
    {
        turnOne = !turnOne;
    }
}