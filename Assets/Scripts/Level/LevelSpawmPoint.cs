using UnityEngine;
public class LevelSpawmPoint : MonoBehaviour
{
    [Header("Target Point")]
    public GameObject player;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.transform.position = transform.position; 
    }
}
