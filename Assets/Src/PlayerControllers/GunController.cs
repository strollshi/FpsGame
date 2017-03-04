using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GunController : MonoBehaviour {

	private GameObject _gunFire;
	private AudioSource _sound;
	private Vector3 _originalGunPos;
	private bool isFire = false;

	private Renderer _renderer;
	private MeshCollider _meshCollider;
	private RaycastHit _hit;
	private GameObject _cam;
	private Transform _aimMark;

	// Use this for initialization
	void Awake () {
		_gunFire = transform.FindChild ("Flare").gameObject;
		_sound = transform.gameObject.GetComponent<AudioSource> ();
		_sound.Stop ();
		_originalGunPos = transform.localPosition;
		_cam = GameObject.Find ("PlayerCam");
		_aimMark = _cam.gameObject.transform.FindChild ("aimMark");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)){
			if (!isFire) {
				Shoot (true);
				PlaySnd ();
				TweenAimMark ();
			}
			isFire = true;
			_gunFire.SetActive (true);
		} else if(isFire && Input.GetMouseButtonUp (0)){
			isFire = false;
			_gunFire.SetActive (false);
			StopSnd ();
		}
	}

	void CheckShootCollision()
	{
		if (UnityEngine.Physics.Raycast(_cam.transform.position , _cam.transform.forward , out _hit, 500)){
			//如果射线碰撞到带有 Collider的模型获取渲染器和被击中模型的 MeshCollider
			_renderer = _hit.collider.gameObject.GetComponent<Renderer>();
			_meshCollider = _hit.collider as MeshCollider;

			if (_renderer == null || _renderer.sharedMaterial == null || _renderer.sharedMaterial.mainTexture == null || _meshCollider == null) {
				return;
			}

			//add shot marks
			Object hole = Instantiate(Resources.Load ("Prefabs/ShotMark"),_hit.point,Quaternion.Euler(_hit.normal));
			(hole as GameObject).name = "hole";
			(hole as GameObject).transform.position = _hit.point + _hit.normal*0.01f;
			(hole as GameObject).transform.forward = _hit.normal;
			Debug.Log("x  " + _hit.point.x + "      y   " + _hit.point.y + "   " + hole.name);
		}
	}

	void TweenAimMark()
	{
		if (!isFire) {
			GoTweenConfig config = new GoTweenConfig ();
			config.scale (new Vector3 (0.1f, 0.1f, 1));
			config.easeType = GoEaseType.ExpoOut;
			Go.to (_aimMark, 0.5f, config);
			return;
		}
		GoTweenConfig config1 = new GoTweenConfig ();
		config1.scale (new Vector3 (0.15f, 0.15f, 1));
		config1.easeType = GoEaseType.ExpoOut;
		config1.onComplete (delegate {
			TweenAimMark();
		});
		Go.from (_aimMark, 0.5f, config1);
	}

	void Shoot(bool isUp)
	{
		GoTweenConfig config = new GoTweenConfig ();
		config.easeType = GoEaseType.BackOut;
		config.localPosition (new Vector3 (_originalGunPos.x, _originalGunPos.y + (isUp?0.01f:-0.01f), _originalGunPos.z - (isUp?0.01f:-0.01f)));
		config.onComplete(delegate(AbstractGoTween obj) {
			if(isFire){
				Shoot(!isUp);
			}
		});
		Go.to (transform, 0.05f, config);
		CheckShootCollision ();
	}

	void PlaySnd()
	{
		_sound.PlayScheduled (0.2f);
		Invoke ("PlaySnd", 6f);
	}
	void StopSnd()
	{
		_sound.Stop ();
		CancelInvoke ();
	}
}
