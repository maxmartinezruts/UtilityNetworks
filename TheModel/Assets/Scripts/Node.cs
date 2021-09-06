using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public enum myEnum // your custom enumeration
     {
    Good, 
    Action
 };
    public myEnum type;

    public enum myEnumDependency // your custom enumeration
    {
    Independent, 
    Dependent
 };
    public myEnumDependency dependency;

    public float doff_dt;
    public float offset;

    public List<Relation> inputs;
    public List<Relation> outputs;

    public float val;

    public List<Node> constraints;


    // Start is called before the first frame 
    
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update(){
        if(tag == "Node"){
        GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color , Color.white, 0.05f);

        }

    }
    public void Step(float dt){
        offset += doff_dt * dt;

    }
    public void UpdateVals(bool inReal)
    {


        val = offset;

        foreach (var relation in inputs)
        {
            if (relation.input.type == Node.myEnum.Good){
                if (relation.type == Relation.myEnum.Linear){
                    val += relation.input.val*relation.a;
                }
                if (relation.type == Relation.myEnum.Logarithmic){
                    val += Mathf.Log(relation.input.val*relation.b)*relation.a;
                }
            }
        }
        if (inReal){
            if (type == myEnum.Good){
                gameObject.transform.Find("ValText").GetComponent<TextMesh>().text = val.ToString();
            }
        }
        
    }

    public void Act(bool inReal){
      if (hasPermission(inReal)){
          if (inReal){
            GetComponent<SpriteRenderer>().color = Color.green;
          }
            foreach (var relation in outputs)
            {
                if (relation.type == Relation.myEnum.Linear){
                    
                    relation.output.offset += relation.a;
                }
            }
        }else{
            GetComponent<SpriteRenderer>().color = Color.red;

        }
    }

    void  OnMouseDown(){
        
        Act(true);
    }

    public bool hasPermission(bool isReal){
        bool permission = true;
        foreach (var constraint in constraints)
        {
    
            if (constraint.val <= 0){
                permission = false;
                if (isReal){
                constraint.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                }

            }
        }   
        return permission;
    }
}
