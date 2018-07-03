using System.Collections;


using System.Collections.Generic;


using UnityEngine;

[System.Serializable]


public class Part
{


    // These three fields need to be defined in the Inspector pane


    public string name;         // The name of this part


    public float health;       // The amount of health this part has


    public string[] protectedBy;  // The other parts that protect this




    // These two fields are set automatically in Start().


    // Caching like this makes it faster and easier to find these later


    [HideInInspector]  // Makes field on the next line not appear in the Inspector


    public GameObject go;           // The GameObject of this part


    [HideInInspector]


    public Material mat;         // The Material to show damage


}





/// <summary>


/// Enemy_4 will start offscreen and then pick a random point on screen to


///   move to. Once it has arrived, it will pick another random point and


///   continue until the player has shot it down.


/// </summary>


public class Enemy_4 : Enemy
{



    [Header("Set in Inspector: Enemy_4")]                                    // a


    public Part[] parts;   // The array of ship Parts


    private Vector3 p0, p1;        // The two points to interpolate


    private float timeStart;     // Birth time for this Enemy_4


    private float duration = 4;  // Duration of movement





    void Start()
    {


        // There is already an initial position chosen by Main.SpawnEnemy()


        //   so add it to points as the initial p0 & p1


        p0 = p1 = pos;                                                        // a





        InitMovement();

        Transform t;


        foreach (Part prt in parts)
        {


            t = transform.Find(prt.name);


            if (t != null)
            {


                prt.go = t.gameObject;


                prt.mat = prt.go.GetComponent<Renderer>().material;


            }


        }

    }




    void InitMovement()
    {                                                     // b


        p0 = p1;   // Set p0 to the old p1


        // Assign a new on-screen location to p1


        float widMinRad = bndCheck.camWidth - bndCheck.radius;


        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;


        p1.x = Random.Range(-widMinRad, widMinRad);


        p1.y = Random.Range(-hgtMinRad, hgtMinRad);





        // Reset the time


        timeStart = Time.time;


    }





    public override void Move()
    {                                            // c


        // This completely overrides Enemy.Move() with a linear interpolation


        float u = (Time.time - timeStart) / duration;





        if (u >= 1)
        {


            InitMovement();


            u = 0;


        }





        u = 1 - Mathf.Pow(1 - u, 2);    // Apply Ease Out easing to u         // d


        pos = (1 - u) * p0 + u * p1;          // Simple linear interpolation        // e


    }


}