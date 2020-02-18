using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
  public float reboundSpeed;
  public float terrainSpeed;

  bool grounded = false;

  bool WalkSanity() {
    if(GetComponent<Rigidbody2D>().gravityScale > 0) {
      return transform.position.y < 0;
    } else {
      return transform.position.y > 0;
    }
  }

  void Update() {
    if(Physics2D.BoxCast(GetComponent<Transform>().position, new Vector2(5.12f * 0.8f, 5.12f * 0.8f), 0.0f, Vector2.right, 2.6f * 0.8f).collider != null) {
      transform.Translate(Vector2.left * terrainSpeed * Time.deltaTime);
    }

    if(Input.GetKeyDown(KeyCode.Space)) {
      GetComponent<Rigidbody2D>().gravityScale *= -1;
      GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
    }
    GetComponent<Animator>().SetBool("isWalking", grounded && WalkSanity());
    
    if(transform.position.x < -0.001f) {
      float amountToMove = reboundSpeed * Time.deltaTime;

      if(Physics2D.BoxCast(GetComponent<Transform>().position, new Vector2(5.12f * 0.8f, 5.12f * 0.8f), 0.0f, Vector2.right, 5.12f * 0.8f).collider == null) {
        transform.Translate(amountToMove, 0, 0);
        Debug.Log("test");
      }
    }
  }

  void OnCollisionStay2D(Collision2D c) {
    if(Physics2D.Raycast(GetComponent<Transform>().position, Vector2.down, 2.57f * 0.8f).collider != null) {
      grounded = true;
    }
  }

  void OnCollisionExit2D(Collision2D c) {
    grounded = false;
  }
}
