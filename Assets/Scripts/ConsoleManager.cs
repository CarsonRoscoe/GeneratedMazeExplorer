using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class ConsoleManager : MonoBehaviour {
        public static ConsoleManager Instance;
        public GameObject ConsoleInputField;
        private InputField m_inputField;
        private bool m_returnPressed;
        private bool m_consoleOpen;


        void Awake() {
            if ( Instance == null ) {
                Instance = this;
            }
            else {
                Destroy( gameObject );
            }
        }

        void Start() {
            m_consoleOpen = false;
            m_returnPressed = false;
            if ( ConsoleInputField != null ) {
                m_inputField = ConsoleInputField.GetComponent<InputField>();
            }
        }

        void Update() {
            if ( Input.GetKeyDown( KeyCode.Return ) && m_inputField != null ) {
                var consoleText = m_inputField.text;
                m_inputField.text = string.Empty;
                ParseConsoleText( consoleText );
            }
        }

        public void ToggleConsoleVisibility() {
            if ( ConsoleInputField != null ) {
                ConsoleInputField.SetActive( !ConsoleInputField.activeSelf );
            }
        }

        void ParseConsoleText( string consoleText ) {
            var tags = consoleText.Split( ' ' );
            if ( tags.Length == 3 ) {
                var name = tags[0];
                var attribute = tags[1];
                var value = tags[2];

                GameObject gameObject;
                bool background = name.ToLower().Equals("background");
                gameObject = GetGameObject( name );

                switch ( attribute ) {
                    case Console.Attribute.Color:
                        var color = ParseTextToColor( value );
                        if ( color.HasValue ) {
                            try {
                                if ( background ) {
                                    var gameObjects = new GameObject[] {
                                        GameObject.Find( Console.Name.LeftWall ),
                                        GameObject.Find( Console.Name.RightWall ),
                                        GameObject.Find( Console.Name.Ceiling ),
                                        GameObject.Find(Console.Name.Floor)
                                    };
                                    foreach(var gameObj in gameObjects) {
                                        if ( gameObj != null ) {
                                            ChangeColorOfObject( gameObj, color.Value );
                                        }
                                    }
                                }
                                else {
                                    ChangeColorOfObject( gameObject, color.Value );
                                }
                            }
                            catch ( Exception e) {
                                print( "Failed to execute command." );
                                print( e );
                            }
                        }
                        break;
                    case Console.Attribute.Speed:
                        if (name == Console.Name.Ball ) {
                            var ball = gameObject.GetComponent<BallBehaviour>();
                            var rigidBody = gameObject.GetComponent<Rigidbody>();
                            Action<int> changeSpeed = ( int speedDif ) =>
                            {
                                ball.MinimumSpeed += speedDif;
                            };
                            switch ( value ) {
                                case Console.Value.Fast:
                                    changeSpeed( 5 );
                                    break;
                                case Console.Value.Slow:
                                    changeSpeed( -5 );
                                    break;
                                default:
                                    changeSpeed( Convert.ToInt32( value ) );
                                    break;
                            }
                        }
                        break;
                    case Console.Attribute.MaxScore:
                        if (name == Console.Name.Rules) {
                            var newMaxScore = Convert.ToInt32( value );
                            if (newMaxScore > 0) {
                                GameDataManager.Instance.MaxScore = newMaxScore;
                            }
                        }
                        break;
                }
            }
        }

        void ChangeColorOfObject( GameObject gameObject, Color color ) {
            var renderer = gameObject.GetComponent<Renderer>();
            renderer.material.color = color;
        }

        GameObject GetGameObject( string name ) {
            switch ( name ) {
                case Console.Name.LeftWall:
                case Console.Name.RightWall:
                case Console.Name.Ceiling:
                case Console.Name.Floor:
                case Console.Name.PlayerOne:
                case Console.Name.PlayerTwo:
                case Console.Name.Ball:
                    return GameObject.Find( name );
                default:
                    return null;
            }
        }

        Color? ParseTextToColor( string value ) {
            switch ( value ) {
                case "Red":
                    return Console.Value.Red;
                case "Blue":
                    return Console.Value.Blue;
                case "Green":
                    return Console.Value.Green;
                case "Black":
                    return Console.Value.Black;
                case "White":
                    return Console.Value.White;
                default:
                    if ( value.Length < 6 ) {
                        return null;
                    }

                    var r = Convert.ToInt32( string.Format( "{0}{1}", value[0], value[1] ), 16 ) / 255;
                    var g = Convert.ToInt32( string.Format( "{0}{1}", value[2], value[3] ), 16 ) / 255;
                    var b = Convert.ToInt32( string.Format( "{0}{1}", value[4], value[5] ), 16 ) / 255;
                    var a = 1f;

                    if ( value.Length == 8 ) {
                        a = Convert.ToInt32( string.Format( "{0}{1}", value[6], value[7] ), 16 ) / 255;
                    }

                    return new Color( r, g, b, a );
            }
        }
    }
}
