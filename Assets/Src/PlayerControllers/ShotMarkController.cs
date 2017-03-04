using UnityEngine;
using System.Collections;

public class ShotMarkController : MonoBehaviour {

	private SpriteRenderer _render;
	private float _alphaValue = 1;

	// Use this for initialization
	void Awake () {
		_alphaValue = 1;
		_render = gameObject.GetComponent<SpriteRenderer> ();
		_render.color = Color.white;

		GoTweenConfig config = new GoTweenConfig ();
		config.colorProp ("color", Color.clear);
		config.easeType = GoEaseType.Linear;
		config.onComplete (delegate {
			Destroy(this.gameObject);	
		});
		Go.to (_render, 7, config);
	}
}
