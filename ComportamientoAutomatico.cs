﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoAutomatico : MonoBehaviour {


    //Enum para los estados
    public enum State {
        MAPEO,
        DFS
    }

    private State currentState;
    private Sensores sensor;
	private Actuadores actuador;
	private Mapa mapa;
    private Vertice verticeActual, verticeDestino;
    public bool fp = true, look;


    void Start(){
        SetState(State.DFS);
        sensor = GetComponent<Sensores>();
		actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        mapa.ColocarNodo(0);
        mapa.popStack(out verticeActual);
    }


    void FixedUpdate() {
        switch (currentState) {
            case State.MAPEO:
            UpdateMAPEO();
            break;
            case State.DFS:
            UpdateDFS();
            break;
        }
    }

    // Funciones de actualizacion especificas para cada estado
    /**
     * PASOS PARA EL DFS
     * 1.- Colocar un vértice (meterlo a la pila 'ColocarNodo' ya lo mete a la pila
     * 2.- Sacar de la pila, e intentar poner mas vértices
     * 3.- Hacer backtrack al siguiente vértice en la pila
     * 4.- Repetir hasta vaciar la pila
     */
    void UpdateMAPEO() {
        if (fp) {
            mapa.popStack(out verticeActual);
            mapa.setPreV(verticeActual);   //Asignar a mapa el vértice nuevo al que nos vamos a mover, para crear las adyacencias necesarias.
            fp = false;
        }
        if (Vector3.Distance(sensor.Ubicacion(), verticeActual.posicion) >= 0.04f) {
            if (!look) {
                transform.LookAt(verticeActual.posicion);
                look = true;
            }
            actuador.Adelante();
        } else {
            look = false;
            fp = true;
            SetState(State.DFS);
        }
        // TODO: TransformlookAt() buscar la función, se realiza solo una vez antes de mover el objeto.

        // if(sensor.FrenteLibre()) {
        // } else {
        //     actuador.GirarIzquierda();
        // }
        // if(sensor.TocandoBasura()) {
        //     actuador.Limpiar(sensor.GetBasura());
        // }
    }

    void UpdateDFS() {
        if(sensor.IzquierdaLibre()) {
            mapa.ColocarNodo(1);
        }
        if(sensor.FrenteLibre()) {
            mapa.ColocarNodo(3);
        }
        if(sensor.DerechaLibre()) {
            mapa.ColocarNodo(2);
        }
        SetState(State.MAPEO);
    }

    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }

}

