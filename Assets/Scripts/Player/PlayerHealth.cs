using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float lerpTimer;    
    public float chipSpeed = 2f;
    public string currHP;
    public Image frontHealthBar;
    public Image backHealthBar;
    public TextMeshProUGUI numCurrHealth;
    public GameObject gs;
    private GameState gameState;
    
    // Start is called before the first frame update
    void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameState");
        gameState = gs.GetComponent<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        //health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(5);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            RestoreHealth(5);
        }
    }
    public void UpdateHealthUI()
    {
        gameState.currentHp = Mathf.Clamp(gameState.currentHp, 0, gameState.maxHp);
        //Debug.Log(gameState.currentHp);
        UpdateNumHPText();
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float hFraction = gameState.currentHp/gameState.maxHp; //true health value

        if(fillBack > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer/chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
        }

        if(fillFront < hFraction)   //if new health value is greater than what is shown 
        {
            
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer/chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }
    public void TakeDamage(float damage)
    {
        gameState.currentHp -= damage;
        lerpTimer = 0f;
    }
    public void RestoreHealth(float healAmt)
    {
        gameState.currentHp += healAmt;
        lerpTimer = 0f;
    }
    public void UpdateNumHPText()
    {
        numCurrHealth.text = Convert.ToString(gameState.currentHp) + "/" + gameState.maxHp;
    }
}
