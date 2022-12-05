using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public Slider slider;
    public Image damageImage;
    public Image healthImage;//el coraz�n que tenemos en la interfaz

    public float flashSpeed;//La velocidad a la que va a desaparecer la imagen
    public Color flashColour;

    public GameManager gameManager;

    public AudioClip deathClip;

    AudioSource audioS;
    Animator anim;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    
    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        healthImage.fillAmount = 1;

        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
    }

    void Update()
    {
        if(damaged == true)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }
    //funci�n p�blica que voy a llamar desde el script del enemigo
    public void TakeDamage(int amount)
    {
        if (isDead == true) return;//si el player ha muerto se sale de la funci�n

        audioS.Play();
        damaged = true;
        currentHealth -= amount;
        slider.value = currentHealth;
        healthImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0) Death();
    }
    void Death()
    {
        audioS.clip = deathClip;
        audioS.Play();

        isDead = true;
        anim.SetTrigger("Death");
        //Deshabilitamos los componentes para que el player no pueda moverse ni disparar
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    //Funci�n p�blica que va como evento en la animaci�n de Death
    public void RestartLevel()
    {
        gameManager.GameOver();
    }
}
