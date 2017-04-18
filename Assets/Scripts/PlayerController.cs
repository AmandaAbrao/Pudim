using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float ForcaDoPulo = 10f;
    public AudioClip somPulo;
    public AudioClip somMorte;

    private Animator anim;
    private Rigidbody rb;
    private bool pulando = false;
    private AudioSource audiosource;

	void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (GameController.instancia.estado == Estado.Jogando || GameController.instancia.estado == Estado.AguardandoComecar)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("Pulando");
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


    void FixedUpdate() {
        if (pulando) {
            pulando = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * ForcaDoPulo, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision outro) {
        if (GameController.instancia.estado == Estado.Jogando) {
            if (outro.gameObject.tag == "Obstaculo") {
                rb.AddForce(new Vector3(-50f, 20f, 0), ForceMode.Impulse);
                rb.detectCollisions = false;
                anim.Play("Morrendo");
                audiosource.PlayOneShot(somMorte);
                GameController.instancia.PlayerMorreu();
            }
        }
    }
}
