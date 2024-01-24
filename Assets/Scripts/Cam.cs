using UnityEngine;

public class Cam : MonoBehaviour {
    private Player playerScript;
    private Vector3 follow;

    public Transform player;
    [SerializeField]
    private float distanceOffset, camMovementSmoothness;
    private float posInitY;

    private void Start() {
        playerScript = player.gameObject.GetComponent<Player>();
        posInitY = transform.position.y;
    }

    void FixedUpdate() {
        if (player != null) {
            //Fazendo a movimentação da câmera
            if (playerScript.horizontal > 0)
                distanceOffset = Mathf.Abs(distanceOffset);
            else if (playerScript.horizontal < 0)
                distanceOffset = (-1) * Mathf.Abs(distanceOffset);

            if (player.position.y < -6)
                follow = new Vector3(player.position.x + distanceOffset, posInitY, transform.position.z);
            else if (player.position.y > -6 && player.position.y < -4)
                follow = new Vector3(player.position.x + distanceOffset, posInitY + 0.1f, transform.position.z);
            else if (player.position.y > -4 && player.position.y < -2)
                follow = new Vector3(player.position.x + distanceOffset, posInitY + 0.2f, transform.position.z);
            else if (player.position.y > -2 && player.position.y < 0)
                follow = new Vector3(player.position.x + distanceOffset, posInitY + 0.3f, transform.position.z);
            else
                follow = new Vector3(player.position.x + distanceOffset, posInitY + 0.4f, transform.position.z);

            //transform.position = follow;
            transform.position = Vector3.Lerp(transform.position, follow, camMovementSmoothness * Time.deltaTime);
        }
    }
}