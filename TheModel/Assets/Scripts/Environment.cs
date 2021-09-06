using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
public class Environment : MonoBehaviour
{

    public GameObject particleSystem;
    public int N_persons;
    public int N_companies;

    // MonoBehaviours
    public List<Person> persons;
    public List<Company> companies;

    //Scriptable
    public List<Relationship> relationships;
    public List<Job> jobs;

    public List<Node> nodes;

    public List<Relation> relations;
    GameObject[] nodes_obj;
    GameObject[] relations_obj;

    public List<Node> actions;
    public List<Node> goods;


    public Node utility;
    public Node time;

    StreamWriter writer; 

    public List<Node> planned_trajectory;

    public float dt;

    public GameObject energy_hist;

    public float repr_speed;


    Node current_action;
    // Start is called before the first frame update
    void Start()
    {
        nodes_obj = GameObject.FindGameObjectsWithTag("Node");
        relations_obj = GameObject.FindGameObjectsWithTag("Relation");

        foreach (var node in nodes_obj)
        {
            Node n = node.GetComponent<Node>();
            nodes.Add(n);
            if (n.type == Node.myEnum.Good){
                goods.Add(n);
            }
            if (n.type == Node.myEnum.Action){
                actions.Add(n);
            }

        }
        foreach (var relation in relations_obj)
        {
            relations.Add(relation.GetComponent<Relation>());
        }


        foreach (var relation in relations)
        {
            relation.input.outputs.Add(relation);
            relation.output.inputs.Add(relation);
        }

        
        Step(true);

        writer =  new StreamWriter("Assets/Resources/log_histt.csv");
        // Write header
        string header = "";
        foreach (var good in goods)
        {
            header += good.gameObject.name + ',';
        }
         foreach (var action_try in actions)
            {
             
                    header += action_try.gameObject.name + ',';
                
            }
        writer.WriteLine(header);

        
        
    }



    public Node MPD_eval(){
        float max_utility = -Mathf.Infinity;
        List<Node> max_trajectory = new List<Node>();
        List<float> offsets = new List<float>();
        for (int i = 0; i < goods.Count; i++)
        {
            offsets.Add(goods[i].offset);
        }
        for (int i = 0; i < 100; i++)
        {

            List<Node> trajectory = new List<Node>();
            float cum_utility = 0;
            float t = 0;

            
            while(t < 24){
                t += dt;
                List<Node> permitted_actions = PermittedActions();
                Node action;
                if (permitted_actions.Count > 0){
                    action = permitted_actions[Random.Range(0, permitted_actions.Count)];
                    action.Act(false);
                    trajectory.Add(action);
                }
                Step(false);
          
      
            
                cum_utility += utility.val;
            }
            if (cum_utility > max_utility){
                max_utility = cum_utility;
                max_trajectory = trajectory;
            }

            // Reset state
            for (int j = 0; j < goods.Count; j++)
            {
                goods[j].offset = offsets[j];

            }

            UpdateVals(false);
        }



        max_trajectory[0].Act(true);

        for (int j = 0; j < 5; j++)
        {
            planned_trajectory[j].gameObject.SetActive(false);
        }
        for (int j = 0; j < Mathf.Min(5, max_trajectory.Count); j++)
        {
            planned_trajectory[j].gameObject.SetActive(true);

            planned_trajectory[j].gameObject.transform.Find("Name").GetComponent<TextMesh>().text = max_trajectory[j].gameObject.name;
        }
        UpdateVals(true);

        // particleSystem.transform.position = max_trajectory[0].gameObject.transform.position;
        return max_trajectory[0];
    }

    void Update(){
        print("fdsfs");
        for (int i = 0; i < 10; i++)
        {
            
        Step(true);
        string row = "";

        foreach (var good in goods)
        {
            row += good.val.ToString() + ',';
        }
        if (time.val > 0){
            current_action = MPD_eval();

            
        

        }
        foreach (var action_try in actions)
            {
                if (current_action == action_try){
                    row += "1,";
                }else{
                    row += "0,";
                }
            }
       
        writer.WriteLine(row);
        }        
        
        // energy_hist.gameObject.transform.position = new Vector3(energy_hist.transform.position.x + dt/100, goods[5].val/10);
    }

    List<Node> PermittedActions(){

        List<Node> permitted = new List<Node>();
        foreach (var action in actions)
        {
            if (action.hasPermission(false)){
                permitted.Add(action);
            }
        }
        return permitted;
    }

    void Step(bool inReal){
        foreach (var good in goods)
        {
            if (inReal){
            // good.Step(Time.deltaTime/10);
                        good.Step(dt);


            }else{
            good.Step(dt);

            }
          
        }
 
        UpdateVals(inReal);
    }

    void UpdateVals(bool inReal){
        for (int i = 0; i < 5; i++)
        {
             foreach (var good in goods)
        {
       
                good.UpdateVals(inReal);

            
        }
        }

    }

    void  OnApplicationQuit(){
        writer.Close();
    }


}
