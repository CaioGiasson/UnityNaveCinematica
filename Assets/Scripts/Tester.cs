using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 a = new Vector3(3, 4, 8);
        Vector3 b = new Vector3(3, 4, 8);

        Vector3 c = Vet.Soma(a, b);

        print( c );
        print( Vet.MultiplicarEscalar(c, 3.0f) );
        print( Vet.Magnitude(c) );
        print( Vet.Normalizar(c) );
        print( Vet.Magnitude( Vet.Normalizar(c) ) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
