using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReflectWall : MonoBehaviour
{
    [Header("ボールのタグ")]
    [SerializeField] private string[] validTags = { "CorrectBall", "Ball" };

    private void OnCollisionEnter(Collision collision)
    {
        // タグが一致するかチェック
        if (!IsBallTag(collision.gameObject.tag)) return;

        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            // 反射計算
            Vector3 incoming = rb.velocity;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 reflect = Vector3.Reflect(incoming, normal);
            rb.velocity = reflect;
        }
    }

    private bool IsBallTag(string tag)
    {
        foreach (string t in validTags)
        {
            if (tag == t) return true;
        }
        return false;
    }
}
