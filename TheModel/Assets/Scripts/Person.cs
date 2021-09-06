using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{

    public List<Person> children;
    public List<Job> jobs;
    public (Person, Person) parents;
    public List<Relationship> relationships;
    public float age;
    public float wealth;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float income_wealth = wealth*1.006f;
        wealth += income_wealth;
        foreach (var job in jobs)
        {
            wealth += job.salary;
        }
        // wealth -= 
    }


}
