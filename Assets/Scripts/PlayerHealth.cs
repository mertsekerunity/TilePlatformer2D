using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float enemyDamage = 20f;
    [SerializeField] float spikeDamage = 50f;
    public Slider healthbar;

    float maxHealth = 100f;
    float health;
    float currentHealth;
    float damageDelay = 0.8f;

    CapsuleCollider2D capsuleCollider;
    PlayerController player;


    bool isVulnerable = true;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        player = GetComponent<PlayerController>();
        healthbar.value = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage();
        UpdateHealth();
    }

    void TakeDamage()
    {
        if (!isVulnerable) { return; }

        float damage = CalculateDamage();

        if (damage > 0)
        {
            health -= damage;

            if (health <= 0)
            {
                health = 0;
                player.Die();
            }

            StartCoroutine(ApplyDamageDelay());
        }
    }

    IEnumerator ApplyDamageDelay()
    {
        isVulnerable = false;
        yield return new WaitForSecondsRealtime(damageDelay);
        isVulnerable = true;
    }

    float CalculateDamage()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        { return enemyDamage; }

        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        { return spikeDamage; }

        return 0;
    }

    void UpdateHealth()
    {
        healthbar.value = health;
    }
}
