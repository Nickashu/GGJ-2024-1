using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public ParticleSystem particlesDeath;

    [SerializeField]
    private float movementSpeed, jumpPower, movementSmoothness, jumpSmoothness;
    private float smoothMove=0, originalJumpPower;
    private bool lookingRight=true, tookDamage=false;
    private List<GameObject> powerUpsCollected = new List<GameObject>();

    [HideInInspector]
    public float horizontal=0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        originalJumpPower = jumpPower;
    }

    private void FixedUpdate() {
        if (!GameController.GetInstance().gameIsPaused) {   //Se o jogo não estiver pausado
            //Fazendo a movimentação do player:
            if (horizontal > 0) {
                smoothMove = Mathf.Lerp(smoothMove, 1, movementSmoothness);
                rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
                if (!lookingRight)
                    flipPlayer();
            }
            else if (horizontal < 0) {
                smoothMove = Mathf.Lerp(smoothMove, -1, movementSmoothness);
                rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
                if (lookingRight)
                    flipPlayer();
            }
            else {
                smoothMove = Mathf.Lerp(smoothMove, 0, movementSmoothness);
                rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
            }
        }
    }

    private void Update() {
        if ((transform.position.y < GameController.GetInstance().limitMinYMap || transform.position.y > GameController.GetInstance().limitMaxYMap)) {   //Se o jogador sair dos limites do mapa
            damage();
        }
    }

    //Estes métodos detectam os eventos de inputs:
    public void SetMovement(InputAction.CallbackContext value) {
        if (!GameController.GetInstance().gameIsPaused) {
            horizontal = value.ReadValue<Vector2>().x;
            if (value.canceled)    //Se o evento for cancelado
                horizontal = 0;
        }
    }
    public void SetJump(InputAction.CallbackContext value) {
        if (!GameController.GetInstance().gameIsPaused) {
            if (value.performed) {   //Quando o evento acontecer
                if (isOnGround())
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
            if (value.canceled) {    //Se o evento for cancelado
                if (rb.velocity.y > 0)
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpSmoothness);
            }
        }
    }


    private bool isOnGround() {   //Método para verificar se o jogador está encostado no chão
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private void flipPlayer() {   //Método para fazer o sprite do jogador virar
        lookingRight = !lookingRight;
        Vector3 locScale = transform.localScale;
        locScale.x *= -1;
        transform.localScale = locScale;
    }

    private void damage() {  //Este método será chamado se o player levar um dano
        if (!tookDamage) {
            gameObject.SetActive(false);
            resetPowerUps();
            Vector3 particlesPosition = new Vector3(transform.position.x, transform.position.y + 0.3f);
            particlesDeath.transform.position = particlesPosition;
            particlesDeath.Play();
            GameController.GetInstance().playerIsRespawning = true;
            tookDamage = true;
        }
    }

    private void resetPowerUps() {
        jumpPower = originalJumpPower;
        foreach(GameObject powerUpCollected in powerUpsCollected) {  //Fazendo todos os pwer-ups coletados reaparecerem no mapa
            powerUpCollected.SetActive(true);
        }
        powerUpsCollected.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "obstacle_damage") {    //O player morre ao tocar em algo que dá dano
            damage();
        }
        if (collision.gameObject.tag == "power_up_jump") {
            powerUpsCollected.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
            jumpPower *= 20;
        }
    }


    private void animationRespawnEnd() {   //Quando o player terminar a animação de respawn, o jogo despausa
        GameController.GetInstance().gameIsPaused = false;
        tookDamage = false;
    }
}
