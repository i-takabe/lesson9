using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furifuri : MonoBehaviour
{
	public float speed = 3.0f;
	GameObject sword;
	bool f1 = false;
	float r = 0;

	Vector3 pos0;

    // Start is called before the first frame update
    void Start()
    {
		sword = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		if (f1 == true)
		{
			r+=20;
			sword.transform.position = new Vector3(2.2f * Mathf.Cos(Mathf.Deg2Rad * r) + this.transform.position.x,
												   2.2f * Mathf.Sin(Mathf.Deg2Rad * r) + this.transform.position.y+0.2f,
													0+this.transform.position.z);

			sword.transform.Rotate(new Vector3(0,0,20));		
		}
		if (r > 180)
		{
			f1 = false;

			Invoke("Reset", 0.5f);
		}

		Swaip();
    }

	void Swaip()
	{
		if (Input.GetMouseButton(0) && f1 != true)
		{
			pos0 = sword.transform.position;
			
			//r = 0;
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Vector3 target_pos = ray.GetPoint(5.0f);

			f1 = true;
				
		}
	}

	private void Reset()
	{
		sword.transform.position = pos0;
		sword.transform.Rotate(0, 0, -r);
		r = 0;
	}
}
