using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public static MouseController controller;
	private void Awake() { controller = this; }

    public GameObject theX;

    void Update(){

		if ( Input.GetMouseButtonUp(0)){
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 posConvert = Camera.main.ScreenToWorldPoint(pos);
            posConvert.z = 10;
            GameObject x = Instantiate(theX);
            x.transform.position = posConvert;
            StartCoroutine(RemoverX(x));

            NaveController.nave.IrPara(posConvert);
		}

    }

	IEnumerator RemoverX(GameObject g){
        yield return new WaitForSeconds(3.0f);
        Destroy(g.gameObject);
	}

}
