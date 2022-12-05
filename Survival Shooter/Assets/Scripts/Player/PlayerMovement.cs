using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed;
    public LayerMask layerFloor;//capa donde va a estar el suelo de la escena

    Rigidbody rb;
    Animator anim;
    Vector3 movement;//vamos a guardar la direcci�n de movimiento
    float horizontal;
    float vertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        InputPlayer();
    }
    void FixedUpdate()
    {
        Move();
        Turning();
        Animating();
    }
    void InputPlayer()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
    void Move()
    {
        movement = new Vector3(horizontal, 0, vertical);//direcci�n de movimiento a trav�s de los input
        movement.Normalize();//Normalizar el vector, es decir, su m�dulo(longitud) vale 1
        rb.MovePosition(transform.position + (movement * speed * Time.deltaTime));
    }
    void Turning()
    {
        //Raycast que va desde el cursor del rat�n en coordenadas de pantalla y con direcci�n hacia la escena
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerFloor))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0;

            //Calculamos la rotaci�n
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            //Aplicar la rotaci�n, es decir, rotar el objeto
            rb.MoveRotation(newRotation);
        }

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
    }
    void Animating()
    {
        //si horizontal es distinto de cero o vertical es distinto de cero
        if(horizontal !=0 || vertical  !=0)
        {
            anim.SetBool("IsMoving", true);
        }
        else//si se mete aqu� significa que horizontal y vertical son ambos cero
        {
            anim.SetBool("IsMoving", false);
        }
    }
}
