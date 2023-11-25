using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


using System.Threading;

public class RuleController
{
    
    public RuleController()
    {
       
       //set up thread to check sm rules could be replaced by an event checker. rules ms broadcasts when a rule changes state(good,bad,updating)
    }

    
}



public class Rule{
    
    public String ruleKey{get;set;}
    
    public Rule(String key){
        this.ruleKey = key;
    }
    public bool EvaluateRule(){
        //will have to be able to go out to systems to get needed data to check if rule is good.
        return true;
    }

}

