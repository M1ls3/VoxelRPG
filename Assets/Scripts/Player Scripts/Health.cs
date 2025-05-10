using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public int CurrentHealth { get { return currentHealth; } }
    public int MaxHealth { get { return maxHealth; } }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Debug.Log(transform.name);
            //Destroy(gameObject);
            if (Regex.IsMatch(transform.name, "Player"))
                Debug.Log("Player is dead!!!");
            else Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Regex.IsMatch(transform.name, "Player"))
        {
            PlayerStats.Instance.Initialize(maxHealth, maxHealth);
            PlayerStats.Instance.onHealthChangedCallback?.Invoke();
        }
    }
}
