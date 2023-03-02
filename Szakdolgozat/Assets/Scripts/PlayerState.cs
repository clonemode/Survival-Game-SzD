using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{


    public static PlayerState Instance { get; set;}

    //---PlayerHealth---//
    public float currentHealth;
    public float maxHealth;

    //---PlayerCalories---//
    public float currentCalories;
    public float maxCalories;
    float distanceTravelled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;
    //---PlayerHydration---//
    public float currentHydration;
    public float maxHydration;
    public bool isHydrationActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydration = maxHydration;
        isHydrationActive = true;
        StartCoroutine(DecreaseHydration());
    }
    public IEnumerator DecreaseHydration()
    {
        while (isHydrationActive)
        {
            currentHydration -= 1;
            yield return new WaitForSeconds(1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >=5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

    }
}
