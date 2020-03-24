using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour
{
    private static Animator playerTrigger, zombieTrigger;
    public static bool touching = false;
    // Use this for initialization
    void Start()
    {
        playerTrigger = GameObject.FindGameObjectWithTag("player").GetComponent<Animator>();
        zombieTrigger = GameObject.FindGameObjectWithTag("zombie").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is BoxCollider || other is CapsuleCollider) {
            if(other.gameObject.tag == "player")
            {
                playerTrigger.SetTrigger("runAway");
                touching = true;
            }
            if (other.gameObject.tag == "zombie")
            {
                zombieTrigger.SetTrigger("attackPlayer");
                touching = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerTrigger.ResetTrigger("runAway");
        zombieTrigger.ResetTrigger("attackPlayer");
    }
}
