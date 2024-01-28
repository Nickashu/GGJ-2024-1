using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public Transform groundCheck;
    public Transform WallCheck, WallCheck2;
    public LayerMask groundLayer, wallLayer;
    public ParticleSystem particlesDeath;

    [SerializeField]
    private float movementSpeed, jumpPower, movementSmoothness, jumpSmoothness;
    private float smoothMove=0, originalJumpPower, originalGravity, originalScaleX, originalScaleY;
    private float lastWall=-1;
    //-1 inicial, 1 esquerda, 2 direita
    private bool lookingRight=true, tookDamage=false, hasJumped=false;
    private List<GameObject> powerUpsCollected = new List<GameObject>();

    [HideInInspector]
    public float horizontal=0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalJumpPower = jumpPower;
        originalGravity = rb.gravityScale;
        originalScaleX = spriteRenderer.transform.localScale.x;
        originalScaleY = spriteRenderer.transform.localScale.y;
    }

    private void FixedUpdate() {
        if (!GameController.GetInstance().gamePaused()) {   //Se o jogo n�o estiver pausado
            rb.gravityScale = originalGravity;
            //Fazendo a movimenta��o do player:
            if (horizontal > 0) {
                smoothMove = Mathf.Lerp(smoothMove, 1, movementSmoothness);
                rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
                if (!lookingRight)
                    flipPlayer();
                isOnWall();
            }
            else if (horizontal < 0) {
                smoothMove = Mathf.Lerp(smoothMove, -1, movementSmoothness);
                rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
                if (lookingRight)
                    flipPlayer();
                isOnWall();
            }
            else {
                smoothMove = Mathf.Lerp(smoothMove, 0, movementSmoothness);
                rb.velocity = new Vector2(smoothMove * movementSpeed, rb.velocity.y);
            }
        }
        else {
            horizontal = 0;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 10;
        }
        anim.SetFloat("velocity", Mathf.Abs(horizontal));
    }

    private void Update() {
        if ((transform.position.y < GameController.GetInstance().limitMinYMap || transform.position.y > GameController.GetInstance().limitMaxYMap)) {   //Se o jogador sair dos limites do mapa
            damage("deathOffLimits");
        }

        anim.SetBool("onGround", isOnGround());
        anim.SetBool("onWall", isOnWall());
    }

    //Estes m�todos detectam os eventos de inputs:
    public void SetMovement(InputAction.CallbackContext value) {
        if (!GameController.GetInstance().gamePaused()) {
            horizontal = value.ReadValue<Vector2>().x;
            if (value.canceled)    //Se o evento for cancelado
                horizontal = 0;
        }
    }
    public void SetJump(InputAction.CallbackContext value) {
        if (!GameController.GetInstance().gamePaused()) {
            if (value.performed) {   //Quando o evento acontecer
                anim.SetTrigger("jump");
                bool OnWall = isOnWall();
                if (isOnGround() || (OnWall && !hasJumped))
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    hasJumped=true;
            }
            if (value.canceled) {    //Se o evento for cancelado
                if (rb.velocity.y > 0)
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpSmoothness);
            }
        }
    }


    private bool isOnGround() {   //M�todo para verificar se o jogador est� encostado no ch�o
        bool onGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if(onGround){
            hasJumped = false;
            lastWall = -1;
        }
        return onGround;
    }

    private bool isOnWall()
    {
        bool onWall = (Physics2D.OverlapCircle(WallCheck.position, 0.1f, wallLayer) || Physics2D.OverlapCircle(WallCheck2.position, 0.1f, wallLayer));
        if (onWall && lookingRight && lastWall != 2)
        {
            hasJumped = false;
            lastWall = 2;
        }
        if (onWall && !lookingRight && lastWall != 1)
        {
            hasJumped = false;
            lastWall = 1;
        }
        return onWall;
    }


    private void flipPlayer() {   //M�todo para fazer o sprite do jogador virar
        lookingRight = !lookingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void damage(string details) {  //Este m�todo ser� chamado se o player levar um dano
        if (!tookDamage) {
            gameObject.SetActive(false);
            Vector3 originalScale = new Vector3(originalScaleX, originalScaleY);
            gameObject.transform.localScale = originalScale;
            resetPowerUps();
            Vector3 particlesPosition = new Vector3(transform.position.x, transform.position.y + 0.3f);
            particlesDeath.transform.position = particlesPosition;
            particlesDeath.Play();
            tookDamage = true;
            GameController.GetInstance().gameStartDialogue((int)GameController.DialogueTypes.Death, details);   //details define como o jogador morreu
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
        if(collision.gameObject.tag == "obstacle_damage") {    //O player morre ao tocar em algo que d� dano
            damage(collision.gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "power_up") {
            if (collision.gameObject.layer == LayerMask.NameToLayer("power_up_jump")) {
                jumpPower *= 10;
            }
            powerUpsCollected.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "lore_object") {
            GameController.GetInstance().gameStartDialogue((int)GameController.DialogueTypes.Lore, collision.gameObject.name);
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "joke_object") {
            GameController.GetInstance().gameStartDialogue((int)GameController.DialogueTypes.Joke, collision.gameObject.name);
        }


        if (collision.gameObject.tag == "end_game") {   //Se o jogadro chegar ao fim do jogo
            Destroy(collision.gameObject);
            StartCoroutine(endGame());
        }
    }


    private IEnumerator endGame() {
        yield return new WaitForSeconds(2);
        TransitionsController.GetInstance().LoadNextScene();
    }


    private void animationRespawnEnd() {   //Quando o player terminar a anima��o de respawn, o jogo despausa
        GameController.GetInstance().gameIsPaused = false;
        tookDamage = false;
    }
}
