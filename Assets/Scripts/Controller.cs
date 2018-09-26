using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Controller : MonoBehaviour {
    [SerializeField]
    private float _speed = 5.0f;
    public float Speed {
        get { return _speed; }
        set { _speed = value; }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
            this.transform.position += this.transform.up * Time.deltaTime * Speed;
        if (Input.GetKey(KeyCode.RightArrow))
            this.transform.position += this.transform.right * Time.deltaTime * Speed;
        if (Input.GetKey(KeyCode.DownArrow))
            this.transform.position -= this.transform.up * Time.deltaTime * Speed;
        if (Input.GetKey(KeyCode.LeftArrow))
            this.transform.position -= this.transform.right * Time.deltaTime * Speed;
    }
}
