using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GroundDetector : MonoBehaviour
{
    public bool onGround;

    string[] groundList = {
        "GimmicWoodBox",
        "GimmicIronBox",
        "GimmicSlider",
        "GimmicTurara",
        "Wall"
    };


    private void Start()
    {
        onGround = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "GimmicTurara")
        {
            if (other.gameObject.GetComponent<GimmicTurara>().CheckDamageToPlayer(this.transform))
            {
                transform.parent.gameObject.GetComponent<PlayerControl>().Killed();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (groundList.Contains(other.gameObject.tag))
        {
            onGround = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (groundList.Contains(other.gameObject.tag))
        {
            onGround = false;
        }
    }
}
