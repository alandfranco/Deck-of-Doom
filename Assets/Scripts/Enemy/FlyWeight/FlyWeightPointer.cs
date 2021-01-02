using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlyWeightPointer
{
    //Creamos una variable estatica de la "estructura" que creamos y le asignamos los valores y le decimos que sea de solo lectura para evitar que sea modificada desde algun lugar
    public static readonly FlyWeight BasicEnemy = new FlyWeight
    {
        config = Resources.Load("ScriptableObjects/BasicEnemy") as ScriptableObject
    };
}