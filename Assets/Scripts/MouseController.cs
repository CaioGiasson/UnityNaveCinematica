using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseController : MonoBehaviour {

	public static MouseController controller;
	private void Awake ( ) { controller = this; }

	public GameObject theX;
	public Text texto;

	public int qualExercicio;

	public void setExercicio ( int qual ) {
		qualExercicio = qual;
	}

	void Update ( ) {

		switch ( qualExercicio ) {
			case 1:
				texto.text = "Exercício de Cinemática\n\nAo clicar em um lugar qualquer da tela,\na nave gira para essa direção em 1 segundo\ne depois se desloca até o local no decorrer de 5 segundos.";
				break;
			case 2:
				texto.text = "Exercício de Quantidade de Movimento\n\nA nave sempre \"olha\" na direção do ponteiro do mouse.\nQuando o usuário clica, os propulsores são ligados.\nIsso gera um impulso I = F * t \nSendo F a força de propulsão e t a duração do impulso.\nComo o impulso é igual à variação na quantidade de movimento:\nI = Qf - Q0\nI = delta Q\nI = m * delta V\nAssim, a variação da velocidade vale I / m\nVariação da velocidade = F * t / m";
				break;
			case 3:
			default:
				texto.text = "Força resultante = Fgravidade + Fmotores\nAceleração = Força resultante / massa\nVariaçào da velocidade = aceleração * intervalo de tempo (deltaTime)\nA cada frame a velocidade recebe um incremento segundo esses parâmetros.\n\nO chão possui restituição total, portanto,\na nave não perde velocidade ao colidir com o chão.\nPor conta disso ela \"rebate\", voltando \na se mover para cima com a mesma velocidade de queda\n imediatamente anterior.";
				break;
		}

		// CÓDIGO ABAIXO É PARA O EXERCÍCIO 1
		if ( qualExercicio == 1 ) {
			if ( Input.GetMouseButtonUp ( 0 ) ) {
				Vector3 pos = new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, 0 );
				Vector3 posConvert = Camera.main.ScreenToWorldPoint ( pos );
				posConvert.z = 10;
				GameObject x = Instantiate ( theX );
				x.transform.position = posConvert;
				StartCoroutine ( RemoverX ( x ) );

				NaveController.nave.IrPara ( posConvert );
			}
		}
		else
		//CÓDIGO ABAIXO É PARA O EXERCÍCIO 2
		if ( qualExercicio == 2 ) {
			Vector3 pos = new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, 0 );
			Vector3 posConvert = Camera.main.ScreenToWorldPoint ( pos );

			NaveController.nave.Apontar ( posConvert );

			if ( Input.GetMouseButton ( 0 ) ) NaveController.nave.LigarJatos ( );
			else NaveController.nave.DesligarJatos ( );
		}
		else
		// CÓDIGO ABAIXO É PARA O EXERCÍCIO 3. ESTÁ IGUAL O DO 2 MAS SEPARADO PARA INDICAR QUE
		// SE NECESSÁRIO PODEM SER IMPLEMENTADOS SEPARADAMENTE.
		if ( qualExercicio == 3 ) {
			Vector3 pos = new Vector3 ( Input.mousePosition.x, Input.mousePosition.y, 0 );
			Vector3 posConvert = Camera.main.ScreenToWorldPoint ( pos );

			NaveController.nave.Apontar ( posConvert );

			if ( Input.GetMouseButton ( 0 ) ) NaveController.nave.LigarJatos ( );
			else NaveController.nave.DesligarJatos ( );
		}

		// CÓDIGO PARA O RELOAD AO TECLAR "R"
		if ( Input.GetKeyUp ( KeyCode.R ) ) {
			SceneManager.LoadScene ( SceneManager.GetActiveScene ( ).buildIndex );
		}
	}

	IEnumerator RemoverX ( GameObject g ) {
		yield return new WaitForSeconds ( 3.0f );
		Destroy ( g.gameObject );
	}

}
