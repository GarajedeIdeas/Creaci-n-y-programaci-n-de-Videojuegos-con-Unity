using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;//m�xima salud del enemigo
    public int currentHealth;
    public float sinkSpeed;//velocidad de hundimiento del enemigo
    public int scoreValue;//los puntos que va a dar al player una vez el enemigo haya sido destruido
    public bool isDead;

    public AudioClip deathClip;

    public ParticleSystem hitParticles;

    AudioSource audioS;
    Animator anim;
    bool isSinking;//para saber si el enemigo "se est� hundiendo"

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(isSinking == true)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }
    //Funci�n p�blico porque voy a llamar a esta funci�n desde el script de disparo del player
    public void TakeDamage(int amount, Vector3 point)
    {
        //si el valor de la booleana isDead es true, me salgo de la funci�n
        if (isDead) return;

        currentHealth -= amount;
        audioS.Play();

        //situo el sistema de part�culas en el punto de impacto del raycast con el enemigo
        hitParticles.transform.position = point;
        hitParticles.Play();
        //currentHealth = currentHealth - amount;

        if (currentHealth <= 0) Death();
    }
    void Death()
    {
        audioS.clip = deathClip;
        audioS.Play();

        isDead = true;
        anim.SetTrigger("Death");
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ScoreEnemy(scoreValue);
    }

    //m�todo p�blico que voy a llamar desde la animaci�n de Death
    public void StartSinking()
    {
        isSinking = true;
        //Deshabilitando el componente NavMeshAgent
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        Destroy(gameObject, 2);
    }
 
}
