using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/*
Carson's extension methods for C#/Unity
*/

public static class NumberExtensionMethods {
    public static double MinMax( this double number, double min, double max ) {
        return Math.Max( Math.Min( number, max ), min );
    }

    public static float MinMax( this float number, float min, float max ) {
        return (float)MinMax( (double)number, (double)max, (double)min );
    }

    public static int MinMax( this int number, int min, int max ) {
        return (int)MinMax( (double)number, (double)max, (double)min );
    }
}

public static class GameObjectExtensionMethods {
    public static bool HasComponent<T>( this GameObject gameObject ) where T : Component {
        return gameObject.GetComponent<T>() != null;
    }
}

public static class TransformExtensionMethods {
    public static void SetZRotation( this Transform transform, float zDegrees ) {
        transform.eulerAngles = new Vector3( transform.eulerAngles.x, transform.eulerAngles.z, zDegrees % 360 );
    }

    public static void SetYRotation( this Transform transform, float yDegrees ) {
        transform.eulerAngles = new Vector3( transform.eulerAngles.x, yDegrees % 360, transform.eulerAngles.z );
    }

    public static void SetXRotation( this Transform transform, float xDegrees ) {
        transform.eulerAngles = new Vector3( xDegrees % 360, transform.eulerAngles.y, transform.eulerAngles.z );
    }

    public static void SetRotation( this Transform transform, float xDegrees, float yDegrees, float zDegrees ) {
        transform.eulerAngles = new Vector3( xDegrees % 360, yDegrees % 360, zDegrees % 360 );
    }

    public static void SetRelativeRotation( this Transform transform, float xDegrees, float yDegrees, float zDegrees) {
        var oldX = transform.eulerAngles.x;
        var oldY = transform.eulerAngles.y;
        var oldZ = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3((oldX + xDegrees)%360, (oldY + yDegrees)%360, (oldZ + zDegrees)%360);
    }

    public static void SetRelativeXRotation( this Transform transform, float xDegrees ) {
        var oldX = transform.eulerAngles.x;
        transform.eulerAngles = new Vector3( (oldX + xDegrees) % 360, transform.eulerAngles.y, transform.eulerAngles.z );
    }

    public static void SetRelativeYRotation( this Transform transform, float yDegrees ) {
        var oldY = transform.eulerAngles.y;
        transform.eulerAngles = new Vector3( transform.eulerAngles.x, (oldY + yDegrees) % 360, transform.eulerAngles.z );
    }

    public static void SetRelativeZRotation( this Transform transform, float zDegrees ) {
        var oldZ = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3( transform.eulerAngles.x, transform.eulerAngles.y, (oldZ + zDegrees) % 360 );
    }
}

public static class VectorExtensionMethods {
    public static Vector2 xy( this Vector3 v ) {
        return new Vector2( v.x, v.y );
    }

    public static Vector3 WithX( this Vector3 v, float x ) {
        return new Vector3( x, v.y, v.z );
    }

    public static Vector3 WithY( this Vector3 v, float y ) {
        return new Vector3( v.x, y, v.z );
    }

    public static Vector3 WithZ( this Vector3 v, float z ) {
        return new Vector3( v.x, v.y, z );
    }

    public static Vector2 WithX( this Vector2 v, float x ) {
        return new Vector2( x, v.y );
    }

    public static Vector2 WithY( this Vector2 v, float y ) {
        return new Vector2( v.x, y );
    }

    public static Vector3 WithZ( this Vector2 v, float z ) {
        return new Vector3( v.x, v.y, z );
    }
}

public static class ListExtensionMethods {
    public static void Shuffle<T>( this IList<T> list ) {
        System.Random rng = new System.Random();
        int n = list.Count;
        while ( n > 1 ) {
            n--;
            int k = rng.Next( n + 1 );
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T RandomItem<T>( this IList<T> list ) {
        if ( list.Count == 0 )
            throw new System.IndexOutOfRangeException( "Cannot select a random item from an empty list" );
        return list[UnityEngine.Random.Range( 0, list.Count )];
    }

    public static T RemoveRandom<T>( this IList<T> list ) {
        if ( list.Count == 0 )
            throw new System.IndexOutOfRangeException( "Cannot remove a random item from an empty list" );
        int index = UnityEngine.Random.Range( 0, list.Count );
        T item = list[index];
        list.RemoveAt( index );
        return item;
    }
}

public static class StringExtensionMethods {
    public static string Truncate( this string value, int maxLength ) {
        if ( string.IsNullOrEmpty( value ) )
            return value;
        return value.Length <= maxLength ? value : value.Substring( 0, maxLength );
    }

    public static bool IsNullOrEmpty( this string value ) {
        return string.IsNullOrEmpty( value );
    }
}