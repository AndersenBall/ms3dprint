public class msStateMachine : IEndpoints
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapPost("setInitialState",SetInitialState);
        app.MapPost("addState",AddState);
        app.MapGet("getStates",GetStates);
        app.MapPost("addStateMachine",AddStateMachine);
        app.MapPost("stepStateMachine",StepStateMachine);
        app.MapPost("addtransition",AddTransition);
        app.MapPost("removetransition",RemoveTransition);
        app.MapPost("getStateMachineTransitions",GetStateMachineTransition);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<StateMachineController>((ServiceProvider) => {
            return new StateMachineController();
        });
        
    }

    private IResult AddState(StateMachineController mainController,String stateMachine,String stateName){
        return mainController.AddState(stateMachine,stateName);
    }
    private IResult GetStates(StateMachineController mainController,String stateMachineID){
        return mainController.GetStates(stateMachineID);
    }
    
    private IResult AddStateMachine(StateMachineController mainController,String stateMachine){
        return mainController.AddStateMachine(stateMachine);
    }

    private IResult AddTransition(StateMachineController mainController,String stateMachine,String startState,String endState, String ruleKey){
        return mainController.AddTransition(stateMachine,ruleKey,startState,endState);
    }
    private IResult RemoveTransition(StateMachineController mainController,String stateMachine,String startState,String endState){
        return mainController.RemoveTransition(stateMachine,startState,endState);
    }
    private IResult StepStateMachine(StateMachineController mainController,String stateMachine,String ruleTrigger){
        return mainController.StepStateMachine(stateMachine,ruleTrigger);
    }
    private IResult GetStateMachineTransition(StateMachineController mainController,String stateMachine){
        return mainController.GetTransition(stateMachine);
    }
    private IResult SetInitialState(StateMachineController mainController,String stateMachine,String stateName){
        return mainController.SetInitialState(stateMachine,stateName);
    }

    

}

