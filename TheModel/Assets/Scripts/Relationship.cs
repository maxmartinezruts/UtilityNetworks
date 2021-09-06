using UnityEngine;
using System.Collections;

[System.Serializable]                         //    Our Representation of an InventoryItem
public class Relationship 
{
    public float age;

    // Types:
    // - Friendship
    // - Date
    // - Marriage
    public bool isMarriage;
    public bool isDate;
    public bool isEx; 

    public (Person, Person) persons;

    public Relationship(Person p1, Person p2, bool isMarriage_, bool isDate_, bool isEx_){
        persons = (p1,p2);
        isMarriage = isMarriage_;
        isDate = isDate_;
        isEx = isEx_;
        age = 0;
    }

    public void Step(){
        // Probability breakup
        int N_children = persons.Item1.children.Count + persons.Item2.children.Count;
        float p_children =  1/(10*(N_children + 5));
        float p_breakup = 1/(2*age*(N_children + 1));
        float p_marry = 0.02f*age;

        // Probability children

        // Probability divorce

        // Probability marry

        // 
    }

}