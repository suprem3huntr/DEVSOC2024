public class PlayState : State{
    private bool isSelected;
    public PlayState(){
        isSelected = false;
        currState = States.PlayState;
    }

    public override void doAction(){
        isSelected = !isSelected;
    }

    public override bool isCondition(){
        return isSelected;
    }
    
}