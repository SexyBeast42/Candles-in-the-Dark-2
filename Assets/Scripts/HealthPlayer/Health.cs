using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth {get; private set;}
    public int numOfHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Scene scene;

    private void Awake(){
        currentHealth = startingHealth;

        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (currentHealth > numOfHealth)
        {
            currentHealth = numOfHealth;
        }
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            
            if (i < numOfHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    
    public void TakeDamage(float _damage){
        if (currentHealth > 0)
        {
            //playerhurt
            Debug.Log(gameObject.name + " got damaged.");
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0 ,startingHealth);
        }
        else
        {
            //player dead
            SceneManager.LoadScene(scene.name);
        }
    }

    public void AddHealth(float _value){
        currentHealth = Mathf.Clamp(currentHealth + _value, 0 , startingHealth);
    }
}
