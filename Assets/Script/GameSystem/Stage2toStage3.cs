using UnityEngine;

public class Stage2toStage3 : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform creature2;
    [SerializeField] private Transform ceiling;
    [SerializeField] private Transform floor;

    void Start()
    {
        InvokeRepeating("ceilandfloorcomeout", 0f,0.1f);
    }
    void ceilandfloorcomeout()
    {
        Debug.Log(ceiling.position.x);
        if(ceiling.position.x <0.5f && floor.position.x <0.5f) 
        {
            CancelInvoke("ceilandfloorcomeout");
            InvokeRepeating("bosscomeout", 0f, 0.1f);

        }
            ceiling.position = new Vector3(ceiling.position.x - 0.5f, ceiling.position.y, ceiling.position.z);
            floor.position = new Vector3(floor.position.x -0.5f, floor.position.y, floor.position.z);
    }
    void bosscomeout()
    {
        if (creature2.position.x <= 7f)
        {
            CancelInvoke("bosscomeout");
            ball.SetActive(true);
        }
        creature2.position = new Vector3(creature2.position.x - 0.3f, creature2.position.y, creature2.position.z);
    }
}
