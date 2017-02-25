using UnityEngine;
using System.Collections;

public class GamePlayerController : MonoBehaviour {

	private float _deltaMouseX = 0;
	private float _deltaMouseY = 0;
	private float _lastMousePosX = 0;
	private float _lastMousePosY = 0;
	private Vector3 world;  
	private Transform _parent;
	private Camera _cam;
	private Rigidbody _rg;
	private float _originalY;

	// Use this for initialization
	void Awake () {
		_lastMousePosX = Input.mousePosition.x;
		_lastMousePosY = Input.mousePosition.y;
		_originalY = transform.localPosition.y;
		_parent = transform.parent;
		_cam = transform.FindChild ("PlayerCam").gameObject.GetComponent<Camera> ();
		_rg = transform.FindChild ("mauler").gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		_deltaMouseX = Mathf.Lerp(_lastMousePosX , Input.mousePosition.x , Time.deltaTime);
		_deltaMouseY = -Mathf.Lerp(_lastMousePosY , Input.mousePosition.y , Time.deltaTime)*0.2f;
		_lastMousePosX = Input.mousePosition.x;
		_lastMousePosY = Input.mousePosition.y;

		float rotationX = 0;
		if (_parent.localRotation.x + _deltaMouseY > -60 && _parent.localRotation.x + _deltaMouseY < 60) {
			rotationX = _parent.localRotation.x + _deltaMouseY;
		} else {
			if (_parent.localRotation.x + _deltaMouseY < -60) {
				rotationX = -60;
			} else if (_parent.localRotation.x + _deltaMouseY > 60) {
				rotationX = 60;
			}

		}

		_parent.localRotation = Quaternion.Euler (0, transform.localRotation.y + _deltaMouseX, 0);
		transform.localRotation = Quaternion.Euler (rotationX, transform.localRotation.y + _deltaMouseX, 0);

		if (Input.GetKeyUp (KeyCode.W)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			return;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			return;
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			return;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			return;
		}


		Vector3 viewVec;
		Input.GetAxis ("Mouse X");
		if (Input.GetKey (KeyCode.W)) {
			viewVec = _cam.transform.forward;
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = viewVec * Time.deltaTime * 70;
		}
		if (Input.GetKey (KeyCode.S)) {
			viewVec = -_cam.transform.forward;
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = viewVec * Time.deltaTime * 50;
			Debug.Log (viewVec);
		}
		if (Input.GetKey (KeyCode.A)) {
			viewVec = -_cam.transform.right;
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = viewVec * Time.deltaTime * 50;
			Debug.Log (viewVec);
		}
		if (Input.GetKey (KeyCode.D)) {
			viewVec = _cam.transform.right;
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = viewVec * Time.deltaTime * 50;
			Debug.Log (viewVec);


		}
		if (Input.GetKey (KeyCode.Space)) {
			viewVec = _parent.up;
			_parent.gameObject.GetComponent<Rigidbody> ().AddForce (viewVec*200);
			Debug.Log (viewVec);

		}
		//transform.localPosition = new Vector3 (transform.localPosition.x, _originalY, transform.localPosition.z);
	}
}
