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

	private AudioSource _footstepSnd;
	private bool _isWalking = false;
	private bool _softStep = false;

	// Use this for initialization
	void Awake () {
		_lastMousePosX = Input.mousePosition.x;
		_lastMousePosY = Input.mousePosition.y;
		_originalY = transform.localPosition.y;
		_parent = transform.parent;
		_cam = transform.FindChild ("PlayerCam").gameObject.GetComponent<Camera> ();
		_rg = transform.FindChild ("mauler").gameObject.GetComponent<Rigidbody> ();
		_footstepSnd = _parent.gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		_deltaMouseX = Mathf.Lerp(_lastMousePosX , Input.mousePosition.x , Time.deltaTime)*0.5f;
		_deltaMouseY = -Mathf.Lerp(_lastMousePosY , Input.mousePosition.y , Time.deltaTime)*0.2f;
		_lastMousePosX = Input.mousePosition.x;
		_lastMousePosY = Input.mousePosition.y;

		float rotationX = 0;
		rotationX = _parent.localRotation.x + _deltaMouseY;
		rotationX = Mathf.Clamp (rotationX, -60, 60);

		_parent.localRotation = Quaternion.Euler (0, transform.localRotation.y + _deltaMouseX, 0);
		transform.localRotation = Quaternion.Euler (rotationX, transform.localRotation.y + _deltaMouseX, 0);

		if (Input.GetKeyUp (KeyCode.W)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			StopWalk ();
			return;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			StopWalk ();
			return;
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			StopWalk ();
			return;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			_parent.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			StopWalk ();
			return;
		}


		Vector3 viewVec;
		if (Input.GetKey (KeyCode.W)) {
			viewVec = _cam.transform.forward;
			_parent.localPosition += viewVec * Time.deltaTime * 2;
			Walk ();
		}
		if (Input.GetKey (KeyCode.S)) {
			viewVec = -_cam.transform.forward;
			_parent.localPosition += viewVec * Time.deltaTime * 2;
			Debug.Log (viewVec);
			Walk ();
		}
		if (Input.GetKey (KeyCode.A)) {
			viewVec = -_cam.transform.right;
			_parent.localPosition += viewVec * Time.deltaTime * 2;
			Debug.Log (viewVec);
			Walk ();
		}
		if (Input.GetKey (KeyCode.D)) {
			viewVec = _cam.transform.right;
			_parent.localPosition += viewVec * Time.deltaTime * 2;
			Debug.Log (viewVec);
			Walk ();

		}
		if (Input.GetKey (KeyCode.Space)) {
			viewVec = _parent.up;
			_parent.gameObject.GetComponent<Rigidbody> ().AddForce (viewVec*200);
			Debug.Log (viewVec);

		}
		//transform.localPosition = new Vector3 (transform.localPosition.x, _originalY, transform.localPosition.z);
	}

	void Walk()
	{
		if (!_isWalking) {
			_isWalking = true;
			_footstepSnd.Play ();

		} else {
			_softStep = !_softStep;
			_footstepSnd.volume = _softStep ? 0.1f : 0.3f;
		}
	}

	void StopWalk()
	{
		if (_isWalking) {
			_isWalking = false;
			_footstepSnd.Stop ();
		}

	}
}
