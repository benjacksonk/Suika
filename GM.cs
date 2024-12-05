using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM Instance { get; private set; }

    [field: SerializeField]
    public Food FoodPrefab { get; private set; }

    [SerializeField]
    List<Color> FoodColors;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public Color GetFoodColor(int index)
    {
        FoodColors ??= new List<Color>();

        while (FoodColors.Count < 1 + index)
        {
            FoodColors.Add(new Color(Random.Range(0f, 1f), Random.Range(0f, 5f), Random.Range(0f, 1f)));
        }

        return FoodColors[index];
    }
}