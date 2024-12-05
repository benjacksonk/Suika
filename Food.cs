using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Food : MonoBehaviour
{
    public static event Action<int> OnPointsTotalChanged;

    private static void SetPointsTotal(int newVal)
    {
        _pointsTotal = newVal;
        OnPointsTotalChanged?.Invoke(_pointsTotal);
    }

    public static int pointsTotal
    {
        get => _pointsTotal;
        private set
        {
            _pointsTotal = value;
            OnPointsTotalChanged?.Invoke(_pointsTotal);
        }
    }

    private static int _pointsTotal;

    public static Food Spawn(int newTierIndex, Vector3 position)
    {

        var relativeScale = Mathf.Pow(2, newTierIndex / 3f);
        var food = Instantiate(GM.Instance.FoodPrefab, position, Quaternion.identity);
        food.tierIndex = newTierIndex;
        food.transform.localScale = Vector3.Scale(food.transform.localScale, new Vector3(relativeScale, relativeScale, 1));
        food.GetComponent<SpriteRenderer>().color = GM.Instance.GetFoodColor(newTierIndex);
        food.physicsEnabled = false;

        return food;
    }

    int CalculatePoints(int newTierIndex)
    {
        int newWorth = 1;

        for (int i = 0; i < newTierIndex; i++)
        {
            newWorth = Mathf.RoundToInt(newWorth + (1 + Mathf.Sqrt(8 * newWorth + 1)) / 2);
        }

        return newWorth;
    }


    // NONSTATIC
    Collider2D physCollider;
    Rigidbody2D physBody;

    public int tierIndex
    {
        get => _tierIndex;
        private set
        {
            _tierIndex = value;
            points = physicsEnabled ? CalculatePoints(value) : 0;
        }
    }

    public int points
    {
        get => _points;
        private set
        {
            pointsTotal += value - _points;
            _points = value;
        }
    }

    public bool physicsEnabled
    {
        get => _physicsEnabled;
        set
        {
            if (value == true)
            {
                points = CalculatePoints(tierIndex);
                physCollider.enabled = true;
                physBody.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                points = 0;
                physBody.bodyType = RigidbodyType2D.Kinematic;
                physCollider.enabled = false;
            }
        }
    }

    private int _tierIndex = 0;
    private int _points = 0;
    private bool _physicsEnabled = false;


    private void Awake()
    {
        physCollider = GetComponent<Collider2D>();
        physBody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (points == 0)
        { points = 1; }

        if (collision.gameObject.TryGetComponent(out Food otherFood)
            && tierIndex == otherFood.tierIndex
            && GetInstanceID() > otherFood.GetInstanceID())
        {
            var newFood = Spawn(tierIndex + 1, (transform.position + otherFood.transform.position) / 2);
            newFood.physicsEnabled = true;

            Destroy(otherFood.gameObject);
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        points = 0;
        Destroy(this.gameObject);
    }
}
