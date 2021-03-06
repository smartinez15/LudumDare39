﻿using UnityEngine;
using UnityEngine.UI;

public class Source : MonoBehaviour
{
    public GameObject pulsePrefab;
    public GameObject sourceDestroyedPrefab;

    int[] types;

    int startingPower;
    int uses;

    int powerLeft;
    int powerRate;

    Vector2 coords;

    [Header("UI")]
    public Image healthBar;
    public Text healthText;
    public Text name;

    void Start()
    {
        types = new int[3]
        {
            100,
            200,
            300
        };

        startingPower = types[Random.Range(0, types.Length)];
        uses = Random.Range(5, 21);
        powerLeft = startingPower;
        powerRate = startingPower / uses;
        coords = GetComponentInParent<HexUnit>().coords;

        healthBar.fillAmount = 1;
        healthText.text = powerLeft + " / " + startingPower;
        if (startingPower == 100)
        {
            name.text = "Small Power Crystal";
        }
        else if (startingPower == 200)
        {
            name.text = "Medium Power Crystal";
        }
        else
        {
            name.text = "Big Power Crystal";
        }
    }

    public void StartExtract()
    {
        FindObjectOfType<HexGrid>().Pulse += OnPulse;
    }

    void OnPulse()
    {
        GameObject newPulse = Instantiate(pulsePrefab);

        newPulse.GetComponent<Pulse>().Generate(coords, powerRate);

        powerLeft -= powerRate;
        healthBar.fillAmount = (float)powerLeft / (float)startingPower;
        healthText.text = powerLeft + " / " + startingPower;
        if (powerLeft <= 0)
        {
            DestroySource();
        }
    }

    void DestroySource()
    {
        FindObjectOfType<HexGrid>().Pulse -= OnPulse;
        GameObject sourceDestroyed = Instantiate(sourceDestroyedPrefab);
        sourceDestroyed.transform.SetParent(transform.parent);
        sourceDestroyed.transform.localPosition = Vector3.zero;
        Destroy(gameObject);
    }
}
