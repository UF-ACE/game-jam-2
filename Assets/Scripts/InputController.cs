using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InputController : MonoBehaviour {
  public float reboundSpeed;

  bool grounded = false;
  bool dead = false;

  bool WalkSanity() {
    if(GetComponent<Rigidbody2D>().gravityScale > 0) {
      return transform.position.y < 0;
    } else {
      return transform.position.y > 0;
    }
  }

  void Update() {
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

    if(dead) {
      Debug.Log("YOU ARE DEAD");
    }
  }

  void LateUpdate() {
    Collider2D wall = Physics2D.BoxCast(GetComponent<Transform>().position, new Vector2(5.12f * 0.8f, 5.12f * 0.8f), 0.0f, Vector2.right, 1.0f).collider;
    if(wall != null) {
      transform.parent = wall.transform;
    } else {
      transform.parent = null;
    }
  }

  void OnCollisionStay2D(Collision2D c) {
    if(Physics2D.Raycast(GetComponent<Transform>().position, Vector2.down * Mathf.Sign(GetComponent<Rigidbody2D>().gravityScale), 2.58f * 0.8f).collider != null) {
      grounded = true;
    }
  }

  void OnCollisionExit2D(Collision2D c) {
    grounded = false;
  }

  void OnCollisionEnter2D(Collision2D c) {
    Tilemap t = c.gameObject.GetComponent<Tilemap>();
    if(t != null) {
      for(int i = 0; i < c.contactCount; i++) {
        Vector3 pos = (new Vector3(c.GetContact(i).point.x, c.GetContact(i).point.y, 0) - transform.position) * 1.1f + transform.position;
        Vector3Int v = t.WorldToCell(pos);
        Sprite s = t.GetSprite(v);
        if(s != null && s.name.StartsWith("die")) {
          dead = true;
        }
      }
    }
  }
}
