using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public static TurnManager Instance;

    private Dictionary<int, Action> m_turns;

    void Awake() {
        if (Instance == null ) {
            Instance = this;
        } else {
            Destroy( this );
        }
        m_turns = new Dictionary<int, Action>();
    }

    public void Subscribe( int ID, Action callback) {
        m_turns.Add( ID, callback );
    }

    public void Unsubscribe( int ID) {
        m_turns.Remove( ID );
    }

    public void TakeTurn() {
        var keys = m_turns.Keys.ToList();
        foreach (var key in keys ) {
            try {
                m_turns[key]();
            } catch (Exception) {
                m_turns.Remove( key );
            }
        }
    }
}
