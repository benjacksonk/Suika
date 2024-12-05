using UnityEngine;
using Random = UnityEngine.Random;

public class Dropper : MonoBehaviour
{
    Food foodObjHeld;

    // Start is called before the first frame update
    void Start()
    {
        foodObjHeld = Food.Spawn(Random.Range(0, 3), transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x, 0, 0);
        foodObjHeld.transform.position = transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            foodObjHeld.physicsEnabled = true;
            foodObjHeld = Food.Spawn(Random.Range(0, 3), transform.position);
        }
    }
}
