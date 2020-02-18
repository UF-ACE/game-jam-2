using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {
  public GameObject[] terrain;
  public float speed;

  List<GameObject> current = new List<GameObject>();
  void Start() {
    for(int i = 0; i < 10; i++) {
      NewTerrain();
    }
  }

  void Update() {
    foreach(GameObject g in current) {
      g.transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    if(current[0].transform.position.x < 12 * -5 * 5.12f) {
      NewTerrain();
    }
  }

  void NewTerrain() {
    if(current.Count >= 10) {
      GameObject.Destroy(current[0]);
      current.RemoveAt(0);
    }
    if(current.Count > 0) {
      GameObject g = terrain[Random.Range(0, terrain.Length)];
      current.Add(GameObject.Instantiate(g, current[current.Count - 1].transform.position + new Vector3(12 * 5.12f, 0, 0), Quaternion.identity, GetComponent<Transform>()));
    } else {
      current.Add(GameObject.Instantiate(terrain[0], GetComponent<Transform>()));
    } 
  }
}
