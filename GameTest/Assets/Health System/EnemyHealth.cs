using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;


    public GameObject healthBarUI;
    public Slider slider;
    public Camera camera;
    public GameObject enemy;

    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    void Update()
    {
        slider.value = CalculateHealth();
        transform.rotation = camera.transform.rotation;

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (health <= 0)
        {
            Destroy(enemy);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(10); // Assuming the player deals 10 damage upon collision
        }

    }
}