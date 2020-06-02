using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveController : MonoBehaviour
{

	public static NaveController nave;
	private void Awake() { nave = this; }

	public float tempoRotacao = 10.0f, tempoTranslacao = 5.0f;

	private Vector3 alvo;
	float distanciaAngular, deslocamentoAngular, passoAngular;
	Vector3 distanciaLinear, deslocamentoLinear, passoLinear;

	private void Update(){
		
	}

	public void IrPara(Vector3 novoAlvo) {
		alvo = novoAlvo;

		distanciaLinear = Vet.Subtrai(alvo, transform.position);

		float anguloNovo = Mathf.Atan2(distanciaLinear.y, distanciaLinear.x) * Mathf.Rad2Deg + 90;      // ESSE +90 É POR QUE A ORIGEM FICA PRA CIMA E NÃO PARA A DIREITA COMO NO CÍRCULO TRIGONOMÉTRICO PADRÃO
		float anguloAtual = transform.rotation.eulerAngles.z;
		distanciaAngular = anguloNovo - anguloAtual;
		if (distanciaAngular > 360.0f) distanciaAngular -= 360.0f;
		if (distanciaAngular < -360.0f) distanciaAngular += 360.0f;
		deslocamentoAngular = Mathf.Abs(distanciaAngular);

		deslocamentoLinear = distanciaLinear;
	}

	private void FixedUpdate()
	{
		if ( deslocamentoAngular>1.0f )
		{
			passoAngular = distanciaAngular * Time.deltaTime / tempoRotacao;
			transform.rotation = Quaternion.Euler( Vet.Soma(transform.rotation.eulerAngles, new Vector3(0, 0, passoAngular )));
			deslocamentoAngular -= Mathf.Abs(passoAngular);
		}
		else if ( Vet.Magnitude(deslocamentoLinear)>1.0f )
		{
			passoLinear = distanciaLinear * Time.deltaTime / tempoTranslacao;
			transform.position = Vet.Soma(transform.position, passoLinear);
			deslocamentoLinear = Vet.Subtrai(deslocamentoLinear, passoLinear);
		}

	}

}
