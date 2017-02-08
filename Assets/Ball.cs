using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    public float Stiffness = 1.0f; //1(完全吸収）２(完全反射）
    private Vector3 accel;
    private Vector3 speed;
    private Vector3 a_speed;
    private Vector3 pos;
    private Vector3 gravity;
    private GameObject[] _board = new GameObject[ 2 ];
    private GameObject[] _ball = new GameObject[ 2 ];
    private string board;
    private string ball;

	// Use this for initialization
	void Start () {
        gravity = new Vector3( 0.0f, -0.002f, 0.0f );
	    accel = gravity;
        pos = transform.position;
        a_speed = Vector3.zero;

        for ( int i = 0; i < 2; i++ ) {
            board = "Board";
            ball = "Ball";
            board += i.ToString( );
            ball += i.ToString( );
            _board[ i ] = GameObject.Find( board );
            _ball[ i ] = GameObject.Find( ball );
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {

	    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 3, Color.blue);
	    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 3, Color.red);
	    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 3, Color.green);

        for ( int i = 0; i < 2; i++ ) {
            if ( onBoard( i ) ) {
                Vector3 n_v = _board[ i ].GetComponent<Board>().getNormalVector( );
                Vector3 board_pos = _board[ i ].GetComponent<Transform>().position;
                Vector3 board_to_ball_pos = pos - board_pos;
                float diff = Vector3.Dot( board_to_ball_pos, n_v );
                float length = gameObject.GetComponent<SphereCollider>().radius + _board[ i ].GetComponent<Transform>().localScale.y / 2;
                if ( diff <= length ) {
                    pos += ( length - diff ) * n_v;
                    speed = speed + Vector3.Dot( speed, -n_v ) * n_v * Stiffness;
                    a_speed = -speed / gameObject.GetComponent<SphereCollider>().radius;
                    accel = gravity + Vector3.Dot( accel, -n_v ) * n_v;
                }
            } else {
                accel = gravity;
            }
        
            if ( _ball[ i ].name == name ) {
                continue;
            }
            float diff_b = Vector3.Distance( _ball[ i ].GetComponent<Transform>().position, transform.position );
            float length_b = gameObject.GetComponent<SphereCollider>().radius + _ball[ i ].GetComponent<SphereCollider>().radius;
            Vector3 c_v = ( transform.position - _ball[ i ].GetComponent<Transform>().position ).normalized;
            if ( diff_b <= length_b ) {
                pos += ( length_b - diff_b ) * c_v;
                Vector3 add_speed = Vector3.Dot( speed, c_v ) * c_v * Stiffness;
                speed = speed - add_speed;
                a_speed = -speed / gameObject.GetComponent<SphereCollider>().radius;
                _ball[ i ].GetComponent<Rigidbody>().velocity = add_speed * 20.0f;
            }
        }

        transform.Rotate( Vector3.Cross( a_speed, Vector3.up ), Vector3.Dot( a_speed, a_speed.normalized ) * 180 / Mathf.PI );
        pos += speed;
	    speed += accel;
        transform.position = pos;
	}

    bool onBoard( int idx ) {
        Vector3 b_p = _board[ idx ].GetComponent<Transform>().position;
        Vector3 b_s = _board[ idx ].GetComponent<Transform>().localScale / 2;
        if ( pos.x < b_p.x - b_s.x ) {
            return false;
        }
        if ( pos.x > b_p.x + b_s.x ) {
            return false;
        }
        if ( pos.z < b_p.z - b_s.z ) {
            return false;
        }
        if ( pos.z > b_p.z + b_s.z ) {
            return false;
        }
        if ( pos.y > b_p.y + b_s.y + gameObject.GetComponent<SphereCollider>().radius ) {
            return false;
        }
        if ( pos.y < b_p.y - b_s.y - gameObject.GetComponent<SphereCollider>().radius ) {
            return false;
        }
        return true;
    }
}
