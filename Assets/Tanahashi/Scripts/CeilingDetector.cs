using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() == null) return;
        if (other.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f) return;

        // 木箱に頭が当たったときの処理
        if (other.gameObject.tag == "GimmicWoodBox")
        {
            other.GetComponent<GimmicWoodBox>().BreakBox();
        }

        // 鉄箱に頭が当たったときの処理
        if (other.gameObject.tag == "GimmicIronBox")
        {
            transform.parent.gameObject.GetComponent<PlayerControl>().Killed();
        }

        // スライダーに頭が当たったときの処理
        if (other.gameObject.tag == "GimmicSlider")
        {
            transform.parent.gameObject.GetComponent<PlayerControl>().Killed();
        }

        // つららに頭が当たったときの処理
        if (other.gameObject.tag == "GimmicTurara")
        {
            transform.parent.gameObject.GetComponent<PlayerControl>().Killed();
        }
    }
}
