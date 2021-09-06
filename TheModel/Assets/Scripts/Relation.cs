using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relation : MonoBehaviour
{

    public Node input;
    public Node output;
    public float a;
    public float b;

    public enum myEnum // your custom enumeration
     {
    Linear, 
    Logarithmic
 };
    public myEnum type;
    // Start is called before the first frame update
    void Start()
    {

         GetComponent<LineRenderer>().SetPosition(0, input.gameObject.transform.position+Vector3.forward*5);
        GetComponent<LineRenderer>().SetPosition(1, output.gameObject.transform.position+Vector3.forward*5);

    }

    // Update is called once per frame
    void Update()
    {
        Color green = Color.green;
        Color red = Color.red;
        green.a = 0.4f;
        red.a = 0.4f;
        if (Mathf.Sign(a) > 0){
            GetComponent<LineRenderer>().startColor = green;
            GetComponent<LineRenderer>().endColor =green;

        }else{
            GetComponent<LineRenderer>().startColor = red;
            GetComponent<LineRenderer>().endColor = red;


        }

        if (type == myEnum.Linear){
            GetComponent<LineRenderer>().startWidth = 0.3f*Mathf.Sqrt(Mathf.Abs(a));
        }else{
            GetComponent<LineRenderer>().startWidth = 1f*Mathf.Sqrt(Mathf.Abs(a/input.val));
        }



    }
}
