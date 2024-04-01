using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launch : MonoBehaviour {

	public Vector3 init_speed = new Vector3(-1.5f, 0f,0f);
	private float fallTime;
	private float shotTime;
	private Rigidbody rbGO;
	private GameObject arrow;
	private bool started;
	public ForceInput force;
	public Vector3 init_pos;
	private bool triggered;
	private bool reset;
    protected bool fire_button = false;
    protected JoyButton joybutton;
    protected Joystick joystick;
    private GameObject Can;
	bool disable_throw = false;

	// Use this for initialization
	void Start () {
		//You get the Rigidbody component you attach to the GameObject
		fallTime = 0.0f;
		shotTime = 0.0f;
		started = false;
		triggered = false;
		disable_throw = false;

		reset = false;
		rbGO = GetComponent<Rigidbody>();
		arrow = GameObject.FindGameObjectsWithTag("Arrow")[0];
		init_pos = transform.localPosition;
        rbGO.useGravity = false;
		
        joybutton = FindObjectOfType<JoyButton>();

		AllReset();
	}

	public void AllReset() {

		// Debug.Log("Reset!");
		// all Cans back on line
		for (int i = 1; i <= 6; i++) {
			 Can = GameObject.Find ("Can" + i.ToString ());
			Rigidbody rbb = Can.GetComponent<Rigidbody>();

			if (i==1)
				Can.transform.position = new Vector3 (-0.5f, 0.3f, 0f);
			if (i==2)
				Can.transform.position = new Vector3 (0f, 0.3f, 0f);
			if (i==3)
				Can.transform.position = new Vector3 (0.5f, 0.3f, 0f);
			if (i==4)
				Can.transform.position = new Vector3 (-0.25f, 0.72f, 0f);
			if (i==5)
				Can.transform.position = new Vector3 (0.25f, 0.72f, 0f);
			if (i==6)
				Can.transform.position = new Vector3 (0f, 1.15f, 0f);
			

			Can.transform.rotation = Quaternion.identity;    
			rbb.velocity = Vector3.zero;
			rbb.angularVelocity = Vector3.zero;
		}
		// score back to zero and reset can state
		force.score = 0;
		for (int i = 1; i <= 6; i++)
		{
			force.scored[i] = false;
		}
	}

	// Update is called once per frame
	void Update () {

		
		started = Input.GetMouseButtonDown (1);
        if (joybutton)
        {
            if (!fire_button && joybutton.pressed && !disable_throw)
            {
                fire_button = true;
				disable_throw = false;
                started = true;
            }

            // stop "fire" action
            if (fire_button && !joybutton.pressed)
                fire_button = false;
        }

        reset = Input.GetMouseButtonDown (2);

		// check for ball speed, if above threshold, start timer
		if (Vector3.Magnitude(rbGO.velocity) > 0.01 && triggered == false) {
			// Debug.Log ("triggered");
			triggered = true;
		}
		
		
		if (reset) {
			AllReset ();
		}
		// if time has started you can start playing
		if (fallTime > 0.0f)
		{
			if (started && !disable_throw)
			{
				// if "fire" pressed (should be called once)
				shotTime = 0.0f;
				triggered = false;
				rbGO.WakeUp();
				rbGO.useGravity = true;
				// compute 3d speed vector as unit vector oriented like parent arrow
				// multiplied by ballspeed intensity
				Vector3 Xaxis = new Vector3(0,1,0);
				// compute direction of arrow
				Vector3 RotatedX = arrow.transform.rotation * Xaxis;
				// multiply vector by speed magnitude to get speed vector
				RotatedX *= -force.ballspeed;
				// Debug.Log ("speed=" + RotatedX);
				rbGO.AddForce(RotatedX, ForceMode.Impulse);
				started = false;
				// disable actions on ball during throw
				disable_throw = true;
				// Debug.Log("throw disabled");
			}
		}
		else
		{
			rbGO.Sleep();
			//Debug.Log("sleep");
		}

		fallTime += Time.deltaTime;

		if (triggered) {
			shotTime += Time.deltaTime;
			// Debug.Log("Time =" + shotTime);
			if (shotTime > 5.0f) {
				transform.localPosition = init_pos;
				rbGO.Sleep ();
				rbGO.useGravity = false;
				triggered = false;
				shotTime = 0.0f;
				disable_throw = false;
			}
		}
	}

	public void QuitApp()
    {
		Debug.Log("Quitting the app !");
        Application.Quit();
    }
}
