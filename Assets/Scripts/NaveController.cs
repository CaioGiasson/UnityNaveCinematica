using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveController : MonoBehaviour {

	public static NaveController nave;
	private void Awake ( ) { nave = this; }

	public float tempoRotacao = 10.0f, tempoTranslacao = 5.0f;

	// DADOS DO EXERCICIO 1
	private Vector3 alvo;
	float distanciaAngular, deslocamentoAngular, passoAngular;
	Vector3 distanciaLinear, deslocamentoLinear, passoLinear;

	// DADOS DO EXERCICIO 2
	int e;
	public float massaNave, forcaJato;
	private float Vx, Vy, Fx, Fy;
	private Vector3 direcao;
	public ParticleSystem jato;
	private Rigidbody rb;

	// DADOS DO EXERCICIO 3
	public float gravidade;
	public GameObject chao;

	// DADOS PARA FAZER O WARP DA TELA QUANDO CHEGA NA BORDA
	float zFixo = 10.0f;
	Camera c;

	private void Start ( ) {
		rb = GetComponent<Rigidbody> ( );
		e = MouseController.controller.qualExercicio;
		if ( e == 1 ) {
			rb.useGravity = false;
			rb.isKinematic = false;

			chao.SetActive ( false );

		}
		else if ( e == 2 ) {
			rb.useGravity = false;
			rb.isKinematic = false;

			chao.SetActive ( false );
		}
		else if ( e == 3 ) {
			// DEIXEI A GRAVIDADE NATIVA COMO FALSE PARA PODER USAR A GRAVIDADE ARITMÉTICA, APLICADA DIRETO COMO FORÇA (NA fixedUpdate)
			rb.useGravity = false;
			rb.isKinematic = false;

			chao.SetActive ( true );
		}

	}

	public void Apontar ( Vector3 direcaoApontar ) {
		direcao = direcaoApontar;
	}

	public void LigarJatos ( ) {
		if ( jato.isStopped ) jato.Play ( );
	}

	public void DesligarJatos ( ) {
		if ( jato.isPlaying ) jato.Stop ( );
	}

	public void IrPara ( Vector3 novoAlvo ) {
		alvo = novoAlvo;

		distanciaLinear = Vet.Subtrai ( alvo, transform.position );

		float anguloNovo = Mathf.Atan2 ( distanciaLinear.y, distanciaLinear.x ) * Mathf.Rad2Deg + 90;      // ESSE +90 É POR QUE A ORIGEM FICA PRA CIMA E NÃO PARA A DIREITA COMO NO CÍRCULO TRIGONOMÉTRICO PADRÃO
		float anguloAtual = transform.rotation.eulerAngles.z;
		distanciaAngular = anguloNovo - anguloAtual;
		if ( distanciaAngular > 360.0f ) distanciaAngular -= 360.0f;
		if ( distanciaAngular < -360.0f ) distanciaAngular += 360.0f;
		deslocamentoAngular = Mathf.Abs ( distanciaAngular );

		deslocamentoLinear = distanciaLinear;
	}

	private void FixedUpdate ( ) {
		e = MouseController.controller.qualExercicio;

		if ( e == 1 ) {
			chao.SetActive ( false );

			if ( deslocamentoAngular > 1.0f ) {
				passoAngular = distanciaAngular * Time.deltaTime / tempoRotacao;
				transform.rotation = Quaternion.Euler ( Vet.Soma ( transform.rotation.eulerAngles, new Vector3 ( 0, 0, passoAngular ) ) );
				deslocamentoAngular -= Mathf.Abs ( passoAngular );
			}
			else if ( Vet.Magnitude ( deslocamentoLinear ) > 1.0f ) {
				passoLinear = distanciaLinear * Time.deltaTime / tempoTranslacao;
				transform.position = Vet.Soma ( transform.position, passoLinear );
				deslocamentoLinear = Vet.Subtrai ( deslocamentoLinear, passoLinear );
			}
		}
		else if ( e == 2 ) {

			chao.SetActive ( false );

			if ( direcao != null ) {
				Vector3 diferencaAngular = Vet.Subtrai ( direcao, transform.position );
				float anguloNovo = Mathf.Atan2 ( diferencaAngular.y, diferencaAngular.x ) * Mathf.Rad2Deg + 90;
				transform.rotation = Quaternion.Euler ( new Vector3 ( 0, 0, anguloNovo ) );

				if ( !jato.isStopped ) {
					// ALTERANDO A QUANTIDADE DE MOVIMENTO / MOMENTO LINEAR
					// MASSA É CONSTANTE, PORTANTO, O QUE VARIA É A VELOCIDADE
					float impulso = forcaJato * Time.fixedDeltaTime;
					// IMPULSO É IGUAL À VARIAÇÃO DA QUANTIDADE DE MOVIMENTO
					float deltaQ = impulso;
					float deltaV = deltaQ / massaNave;
					float deltaVx = deltaV * Mathf.Cos ( ( transform.root.eulerAngles.z - 90 ) * Mathf.Deg2Rad );
					float deltaVy = deltaV * Mathf.Sin ( ( transform.root.eulerAngles.z - 90 ) * Mathf.Deg2Rad );
					Vx += deltaVx;
					Vy += deltaVy;
				}

				Vector3 tp = transform.position;
				transform.position = new Vector3 (
					tp.x + Vx * Time.fixedDeltaTime,
					tp.y + Vy * Time.fixedDeltaTime,
					zFixo
				);
			}
		}
		else if ( e == 3 ) {

			chao.SetActive ( true );

			if ( direcao != null ) {
				Vector3 diferencaAngular = Vet.Subtrai ( direcao, transform.position );
				float anguloNovo = Mathf.Atan2 ( diferencaAngular.y, diferencaAngular.x ) * Mathf.Rad2Deg + 90;
				transform.rotation = Quaternion.Euler ( new Vector3 ( 0, 0, anguloNovo ) );

				float forcaX = 0;
				float forcaY = gravidade;

				if ( !jato.isStopped ) {
					// AGORA SEPARANDO JÁ NO IMPULSO OS VALORES EM X E Y
					// POR QUE GRAVIDADE SÓ VAI AFETAR EM Y
					forcaX += forcaJato * Mathf.Cos ( ( transform.root.eulerAngles.z - 90 ) * Mathf.Deg2Rad );
					forcaY += forcaJato * Mathf.Sin ( ( transform.root.eulerAngles.z - 90 ) * Mathf.Deg2Rad );
				}

				float Ax = forcaX / massaNave;
				float Ay = forcaY / massaNave;

				Vx = Vx + Ax * Time.fixedDeltaTime;
				Vy = Vy + Ay * Time.fixedDeltaTime;

				Vector3 tp = transform.position;
				transform.position = new Vector3 (
					tp.x + Vx * Time.fixedDeltaTime,
					tp.y + Vy * Time.fixedDeltaTime,
					zFixo
				);

				if ( transform.position.y < -4.2 ) {
					Vy *= -1;
				}
			}
		}

	}

}
