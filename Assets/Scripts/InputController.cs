using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputController : MonoBehaviour {
  public float reboundSpeed;

  bool grounded = false;
  bool dead = false;
  float gravity;

  bool WalkSanity() {
    if(GetComponent<Rigidbody2D>().gravityScale > 0) {
      return transform.position.y < 0;
    } else {
      return transform.position.y > 0;
    }
  }

  void Start() {
    gravity = GetComponent<Rigidbody2D>().gravityScale;
  }

  void Update() {
    if(dead) {
      GetComponent<Rigidbody2D>().gravityScale = 0;
      GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
      return;
    }

    if(Input.GetKeyDown(KeyCode.Space)) {
      GetComponent<Rigidbody2D>().gravityScale *= -1;
      GetComponent<SpriteRenderer>().flipY = !GetComponent<SpriteRenderer>().flipY;
    }
    GetComponent<Animator>().SetBool("isWalking", grounded && WalkSanity());
    
    if(grounded && transform.position.x < -0.001f) {
      float amountToMove = reboundSpeed * Time.deltaTime;

      if(Physics2D.BoxCast(GetComponent<Transform>().position, new Vector2(5.12f * 0.8f, 5.12f * 0.8f), 0.0f, Vector2.right, 5.12f * 0.8f).collider == null) {
        transform.Translate(amountToMove, 0, 0);
      }
    }

    Collider2D gravityCollider = Physics2D.Raycast(GetComponent<Transform>().position, Vector2.down * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale), 0.1f).collider;
    if(gravityCollider != null && gravityCollider.gameObject.layer == LayerMask.NameToLayer("GravityDown")) {
      GetComponent<Rigidbody2D>().gravityScale = Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale) * gravity + 3.0f;
    } else if(gravityCollider != null && gravityCollider.gameObject.layer == LayerMask.NameToLayer("GravityUp")) {
      GetComponent<Rigidbody2D>().gravityScale = Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale) * gravity - 3.0f;
    } else {
      GetComponent<Rigidbody2D>().gravityScale = Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale) * gravity;
    }
  }

  void LateUpdate() {
    if(dead) {
      return;
    }

    Collider2D wall = Physics2D.BoxCast(GetComponent<Transform>().position, new Vector2(5.12f * 0.8f, 5.12f * 0.8f), 0.0f, Vector2.right, 1.0f, LayerMask.GetMask("Ground")).collider;
    if(wall != null) {
      transform.parent = wall.transform;
    } else {
      transform.parent = null;
    }
  }

  void OnCollisionStay2D(Collision2D c) {
    if(dead) {
      GetComponent<Rigidbody2D>().AddForce(c.transform.position - transform.position, ForceMode2D.Impulse);
    }
    if(Physics2D.Raycast(GetComponent<Transform>().position, Vector2.down * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale), 2.58f * 0.8f).collider != null) {
      grounded = true;
    }
  }

  void OnCollisionExit2D(Collision2D c) {
    grounded = false;
  }

  void OnCollisionEnter2D(Collision2D c) {
    for(int i = 0; i < c.contactCount; i++) {
      if(c.GetContact(i).collider.gameObject.layer == LayerMask.NameToLayer("Spikes")) {
        dead = true;
      }
    }
  }
}
