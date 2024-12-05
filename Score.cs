using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Score : MonoBehaviour
{
    private TextMeshProUGUI textUI;



    private void Awake()
    {
        if (TryGetComponent(out textUI))
        { SetScore(0); }
    }

    private void Start()
    { Food.OnPointsTotalChanged += SetScore; }

    private void OnDestroy()
    { Food.OnPointsTotalChanged -= SetScore; }



    private void SetScore(int newScore)
    { textUI.text = $"Score: {newScore}"; }
}
