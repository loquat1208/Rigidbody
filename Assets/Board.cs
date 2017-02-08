using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

    private Vector3 normal_vec;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ( ) {
        transform.position = new Vector3( transform.position.x, transform.position.y + 0.5f, transform.position.z );
        normal_vec = Vector3.Cross( transform.TransformDirection(Vector3.forward), transform.TransformDirection(Vector3.right) ).normalized;
	    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
	    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 10, Color.red);
	    Debug.DrawRay(transform.position, normal_vec * 10, Color.blue);
        transform.position = new Vector3( transform.position.x, transform.position.y - 0.5f, transform.position.z );
	}

    public Vector3 getNormalVector( ) {
        return normal_vec;
    }
}
