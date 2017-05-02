using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PudimController : MonoBehaviour
{

    public float ForcaDoPulo = 10f;
    public AudioClip somPulo;
    public AudioClip somMorte;

    private Animator anim;
    private Rigidbody rb;
    private bool pulando = false;
    private AudioSource audiosource;

    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;

    void Start()
    {

        posicaoInicial = transform.localPosition;
        rotacaoInicial = transform.localRotation;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();

    }

    public void recomecar()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.detectCollisions = true;
        transform.localPosition = posicaoInicial;
        transform.localRotation = rotacaoInicial;
    }


    void Update()
    {
        if (GameController.instancia.estado == Estado.Jogando || GameController.instancia.estado == Estado.AguardandoComecar)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("pulando");
                audiosource.PlayOneShot(somPulo);
                rb.useGravity = true;
                pulando = true;

                if (GameController.instancia.estado == Estado.AguardandoComecar)
                {
                    GameController.instancia.PlayerComecou();
                }
            }
        }
    }


    void FixedUpdate()
    {
        if (pulando)
        {
            pulando = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * ForcaDoPulo, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision outro)
    {
        if (GameController.instancia.estado == Estado.Jogando)
        {
            if (outro.gameObject.tag == "Obstaculo")
            {
                rb.AddForce(new Vector3(-50f, 20f, 0), ForceMode.Impulse);
                rb.detectCollisions = false;
                anim.Play("morrendo");
                audiosource.PlayOneShot(somMorte);
                GameController.instancia.PlayerMorreu();
            }
        }
    }
}

