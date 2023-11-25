using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


using System.Threading;

public class StateMachineController
{
    private List<StateMachine> stateMachines = new List<StateMachine>();

    public StateMachineController()
    {
       
       //set up thread to check sm rules could be replaced by an event checker. rules ms broadcasts when a rule changes state(good,bad,updating)
    }

    public IResult AddStateMachine(String stateMachineName){
        stateMachines.Add(new StateMachine(stateMachineName));
        return Results.Ok();
    }

    public IResult SetInitialState(String stateMachineName,String stateName ){
        
        StateMachine stateMachine = stateMachines.FirstOrDefault(obj => obj.stateMachineID == stateMachineName);
        if (stateMachine == null){
            return Results.NoContent();
        }

        Console.WriteLine("it found" + stateMachine.stateMachineID);
        String errormsg = stateMachine.SetInitialState(stateName);
        return Results.Ok(errormsg);
    }
    public IResult AddState(String stateMachineName,String stateName ){
        
        StateMachine stateMachine = stateMachines.FirstOrDefault(obj => obj.stateMachineID == stateMachineName);
        if (stateMachine == null){
            return Results.NoContent();
        }

        Console.WriteLine("it found" + stateMachine.stateMachineID);
        String errormsg = stateMachine.AddState(stateName);
        return Results.Ok(errormsg);
    }
    public IResult GetStates(String stateMachineName){
        StateMachine stateMachine = stateMachines.FirstOrDefault(obj => obj.stateMachineID == stateMachineName);
        if (stateMachine == null){
            return Results.NoContent();
        }
        
        String returnString = JsonSerializer.Serialize(stateMachine.states);
        return Results.Ok(returnString);

    }

    public IResult RemoveState(String stateMachineName,String stateName ){
        return Results.Ok("State Removed");
    }
    public IResult AddTransition(String stateMachineName,String ruleName ,String startState,String endState){
        StateMachine stateMachine = stateMachines.FirstOrDefault(obj => obj.stateMachineID == stateMachineName);
        if (stateMachine == null){
            return Results.NoContent();
        }
        stateMachine.AddTransition(startState,endState,ruleName);
        
        return Results.Ok("Transition Added");
    }

    public IResult GetTransition(String stateMachineName){
        StateMachine stateMachine = stateMachines.FirstOrDefault(obj => obj.stateMachineID == stateMachineName);
        if (stateMachine == null){
            return Results.NoContent();
        }
        String returnString = "[";
        foreach (Transition t in stateMachine.transitions){
            returnString += ","+t.startNode.stateName + "->" + t.destinationNode.stateName + ": " + t.rule.ruleKey;
        }
        returnString += "]";
    
        return Results.Ok(returnString);
        
    }

    public IResult RemoveTransition(String stateMachineName,String startState,String endState){

        return Results.Ok("Transition removed");
    }

    public IResult StepStateMachine(String stateMachineName,String transitionKey){
         StateMachine stateMachine = stateMachines.FirstOrDefault(obj => obj.stateMachineID == stateMachineName);
        if (stateMachine == null){
            return Results.NoContent();
        }
        
        stateMachine.RunTransition(transitionKey);
        return Results.Ok("State Machine Updated");
    }
    
}


public class StateMachine{
    public String stateMachineID{get;private set;}
    public List<State> states{get;private set;}
    public List<Transition> transitions{get;private set;}
    public State initialState{get;private set;}
    public State currentState{get;private set;}

    //public List<State> finalStates{get;private set;}
    public StateMachine(String name){
        states = new List<State>();
        transitions = new List<Transition>();
        stateMachineID = name;
    }

    public String SetInitialState(String stateName){
        State state = states.FirstOrDefault(obj => obj.stateName == stateName);
        if(state!= null){
            Console.WriteLine( "state already exists");
        }else{
            AddState(stateName);
        }
        
        State addedState = states.FirstOrDefault(obj => obj.stateName == stateName);
        
        if (addedState == null){
            return "error: code brokey";
        }
        this.initialState = addedState;
        this.currentState = this.initialState;
        return "added";
    }

    public String AddState(String stateName){
        this.states.Add(new State(stateName));
        return "added";
    }

    public bool RemoveState(String stateName){
        //this.states.Remove(stateName);//might not work as how will it know to remove the one with string name state? idk
        return true;
    }

    public bool AddTransition(String startStateName,String endStateName,String ruleKey){
        State startState = states.FirstOrDefault(obj => obj.stateName == startStateName);
        State endState = states.FirstOrDefault(obj => obj.stateName == endStateName);
        if (startState == null && endState == null){
            return false;
        }
        //instead of adding a new rule. look up rules with that name then add it
        this.transitions.Add(new Transition(ref startState,ref endState,new Rule(ruleKey)));
        return true;
    }

    //check current state, check all valid rules, transition if one is true
    public (bool,String) RunTransition(String triggeredRule){
        if (this.currentState == null){
            Console.WriteLine("no current state");
            return (false,"no current state");
        }

        foreach( Transition t in transitions){
            if(t.startNode == this.currentState){
                if(t.rule.ruleKey == triggeredRule){
                    Console.WriteLine("transitioned to next node:",t.destinationNode.stateName);
                    this.currentState = t.destinationNode;
                    return (true,"moved to:"+t.destinationNode.stateName);
                }
            }
        }
        return (true,"no valid tranision");
    }

}
 
public class State
{
    public String stateName{get;set;}

    public List<Transition> transitions{get;private set;}

    public State(string name){
        stateName = name;
    }

    public bool AddTransitionRef(ref Transition t){
        transitions.Add(t);
        return true;
    }


}

public class Transition
{

    public State startNode{get;private set;}
    public State destinationNode{get;private set;}
    public Rule rule{get;set;}

    public Transition(ref State start, ref State end,Rule rule){
        this.startNode = start;
        this.destinationNode = end;
        this.rule = rule;
    }

    public bool EvaulateTransition(){
        return rule.EvaluateRule();
    }

}



