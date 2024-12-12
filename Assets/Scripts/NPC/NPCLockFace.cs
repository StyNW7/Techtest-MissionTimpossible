using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{

    public Transform player; // Referensi ke posisi pemain
    public float rotationSpeed = 5f; // Kecepatan rotasi NPC
    public float viewRadius = 10f; // Radius pandangan NPC

    void Start()
    {
        // Mencari referensi pemain jika belum diatur
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found! Please assign a Player object or ensure the Player has the 'Player' tag.");
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // Hitung jarak ke pemain
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Periksa apakah pemain berada dalam radius pandangan
        if (distanceToPlayer <= viewRadius)
        {
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        // Hitung arah ke pemain
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Abaikan komponen Y untuk rotasi horizontal

        if (directionToPlayer.magnitude > 0.1f) // Hindari rotasi jika jaraknya terlalu kecil
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Gambarkan radius pandangan saat objek dipilih di Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

}
