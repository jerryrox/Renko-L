// from: http://www.robertpenner.com/easing/

// Removed animation stuff - not needed for Unity.
// Fixed documentation erroneously calling param "c" Final value. If c == 0 then fuck all happens. Therefore:
// c is not the final value and the person who wrote that it is, happens to be an asshole that wasted me an hour
// figuring out why.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Renko.Utility
{
	/// <summary>
	/// Animates the change in value of a float property using 
	/// Robert Penner's easing equations for interpolation over a specified duration.
	/// </summary>
	public static class Easing {

		/// <summary>
		/// Dictionary of ease functions mapped to their corresponding EaseType values.
		/// You'll have to call Easing.Initialize() first though.
		/// </summary>
		public static Dictionary<EaseType, EaseHandler> Handlers;

		/// <summary>
		/// Delegate for handling ease events.
		/// </summary>
		public delegate float EaseHandler(float t, float b, float c, float d);


		/// <summary>
		/// Initializes easing handler dictionary.
		/// </summary>
		public static void Initialize() {
			if(Handlers != null)
				return;

			Handlers = new Dictionary<EaseType, EaseHandler>();
			Handlers.Add(EaseType.BackEaseIn, BackEaseIn);
			Handlers.Add(EaseType.BackEaseInOut, BackEaseInOut);
			Handlers.Add(EaseType.BackEaseOut, BackEaseOut);
			Handlers.Add(EaseType.BackEaseOutIn, BackEaseOutIn);
			Handlers.Add(EaseType.BounceEaseIn, BounceEaseIn);
			Handlers.Add(EaseType.BounceEaseInOut, BounceEaseInOut);
			Handlers.Add(EaseType.BounceEaseOut, BounceEaseOut);
			Handlers.Add(EaseType.BounceEaseOutIn, BounceEaseOutIn);
			Handlers.Add(EaseType.CircEaseIn, CircEaseIn);
			Handlers.Add(EaseType.CircEaseInOut, CircEaseInOut);
			Handlers.Add(EaseType.CircEaseOut, CircEaseOut);
			Handlers.Add(EaseType.CircEaseOutIn, CircEaseOutIn);
			Handlers.Add(EaseType.CubicEaseIn, CubicEaseIn);
			Handlers.Add(EaseType.CubicEaseInOut, CubicEaseInOut);
			Handlers.Add(EaseType.CubicEaseOut, CubicEaseOut);
			Handlers.Add(EaseType.CubicEaseOutIn, CubicEaseOutIn);
			Handlers.Add(EaseType.EaseIn, EaseIn);
			Handlers.Add(EaseType.EaseOut, EaseOut);
			Handlers.Add(EaseType.ElasticEaseIn, ElasticEaseIn);
			Handlers.Add(EaseType.ElasticEaseInOut, ElasticEaseInOut);
			Handlers.Add(EaseType.ElasticEaseOut, ElasticEaseOut);
			Handlers.Add(EaseType.ElasticEaseOutIn, ElasticEaseOutIn);
			Handlers.Add(EaseType.ExpoEaseIn, ExpoEaseIn);
			Handlers.Add(EaseType.ExpoEaseInOut, ExpoEaseInOut);
			Handlers.Add(EaseType.ExpoEaseOut, ExpoEaseOut);
			Handlers.Add(EaseType.ExpoEaseOutIn, ExpoEaseOutIn);
			Handlers.Add(EaseType.Linear, Linear);
			Handlers.Add(EaseType.QuadEaseIn, QuadEaseIn);
			Handlers.Add(EaseType.QuadEaseInOut, QuadEaseInOut);
			Handlers.Add(EaseType.QuadEaseOut, QuadEaseOut);
			Handlers.Add(EaseType.QuadEaseOutIn, QuadEaseOutIn);
			Handlers.Add(EaseType.QuartEaseIn, QuartEaseIn);
			Handlers.Add(EaseType.QuartEaseInOut, QuartEaseInOut);
			Handlers.Add(EaseType.QuartEaseOut, QuartEaseOut);
			Handlers.Add(EaseType.QuartEaseOutIn, QuartEaseOutIn);
			Handlers.Add(EaseType.QuintEaseIn, QuintEaseIn);
			Handlers.Add(EaseType.QuintEaseInOut, QuintEaseInOut);
			Handlers.Add(EaseType.QuintEaseOut, QuintEaseOut);
			Handlers.Add(EaseType.QuintEaseOutIn, QuintEaseOutIn);
			Handlers.Add(EaseType.SineEaseIn, SineEaseIn);
			Handlers.Add(EaseType.SineEaseInOut, SineEaseInOut);
			Handlers.Add(EaseType.SineEaseOut, SineEaseOut);
			Handlers.Add(EaseType.SineEaseOutIn, SineEaseOutIn);
		}

		/// <summary>
		/// Linear change in value.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <returns>The correct value.</returns>
		public static float Linear( float t, float b, float c, float d ) {
			return c * t + b;
		}

		/// <summary>
		/// Same easing effect as "QuadEaseOut".
		/// </summary>
		/// <returns>The out.</returns>
		/// <param name="t">T.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="c">C.</param>
		/// <param name="d">D.</param>
		public static float EaseOut( float t, float b, float c, float d ) {
			return QuadEaseOut(t, b, c, d);
		}

		/// <summary>
		/// Same easing effect as "QuadEaseIn".
		/// </summary>
		/// <returns>The in.</returns>
		/// <param name="t">T.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="c">C.</param>
		/// <param name="d">D.</param>
		public static float EaseIn( float t, float b, float c, float d ) {
			return QuadEaseIn(t, b, c, d);
		}

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <returns>The correct value.</returns>
		public static float ExpoEaseOut( float t, float b, float c, float d ) {
			return ( t == 1 ) ? b + c : c * ( -Mathf.Pow( 2, -10 * t ) + 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ExpoEaseIn( float t, float b, float c, float d ) {
			return ( t == 0 ) ? b : c * Mathf.Pow( 2, 10 * ( t - 1 ) ) + b;
		}

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ExpoEaseInOut( float t, float b, float c, float d ) {
			if ( t == 0 )
				return b;

			if ( t == 1 )
				return b + c;

			if ( (t *= 2) < 1 )
				return c  * 0.5f * Mathf.Pow( 2, 10 * ( t - 1 ) ) + b;

			return c  * 0.5f * ( -Mathf.Pow( 2, -10 * --t ) + 2 ) + b;
		}

		/// <summary>
		/// Easing equation function for an exponential (2^t) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ExpoEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return ExpoEaseOut( t * 2, b, c  * 0.5f, d );

			return ExpoEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CircEaseOut( float t, float b, float c, float d ) {
			return c * Mathf.Sqrt( 1 - ( t = t - 1 ) * t ) + b;
		}

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CircEaseIn( float t, float b, float c, float d ) {
			return -c * ( Mathf.Sqrt( 1 - t * t ) - 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CircEaseInOut( float t, float b, float c, float d ) {
			if ( (t *= 2f) < 1 )
				return -c * 0.5f * ( Mathf.Sqrt( 1 - t * t ) - 1 ) + b;

			return c * 0.5f * ( Mathf.Sqrt( 1 - ( t -= 2 ) * t ) + 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CircEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return CircEaseOut( t * 2, b, c * 0.5f, d );

			return CircEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuadEaseOut( float t, float b, float c, float d ) {
			return -c * t * ( t - 2 ) + b;
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuadEaseIn( float t, float b, float c, float d ) {
			return c * t * t + b;
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuadEaseInOut( float t, float b, float c, float d ) {
			if ( (t *= 2f) < 1 )
				return c * 0.5f * t * t + b;

			return -c * 0.5f * ( ( --t ) * ( t - 2 ) - 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuadEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return QuadEaseOut( t * 2, b, c * 0.5f, d );

			return QuadEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float SineEaseOut( float t, float b, float c, float d ) {
			return c * Mathf.Sin( t * ( Mathf.PI * 0.5f ) ) + b;
		}

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float SineEaseIn( float t, float b, float c, float d ) {
			return -c * Mathf.Cos( t * ( Mathf.PI * 0.5f ) ) + c + b;
		}

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float SineEaseInOut( float t, float b, float c, float d ) {
			if ((t *= 2f) < 1)
				return SineEaseIn(t, b, c*0.5f, d);
			return SineEaseOut(t-1, b + c*0.5f, c*0.5f, d);
		}

		/// <summary>
		/// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float SineEaseOutIn( float t, float b, float c, float d ) {
			if (t < 0.5f)
				return SineEaseOut(t * 2, b, c * 0.5f, d);

			return SineEaseIn((t * 2) - 1f, b + c * 0.5f, c * 0.5f, d);
		}

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CubicEaseOut( float t, float b, float c, float d ) {
			return c * ( ( t = t - 1 ) * t * t + 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CubicEaseIn( float t, float b, float c, float d ) {
			return c * t * t * t + b;
		}

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CubicEaseInOut( float t, float b, float c, float d ) {
			if ( (t *= 2f) < 1 )
				return c * 0.5f * t * t * t + b;

			return c * 0.5f * ( ( t -= 2 ) * t * t + 2 ) + b;
		}

		/// <summary>
		/// Easing equation function for a cubic (t^3) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float CubicEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return CubicEaseOut( t * 2, b, c * 0.5f, d );

			return CubicEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuartEaseOut( float t, float b, float c, float d ) {
			return -c * ( ( t = t - 1 ) * t * t * t - 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuartEaseIn( float t, float b, float c, float d ) {
			return c * t * t * t * t + b;
		}

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuartEaseInOut( float t, float b, float c, float d ) {
			if ( (t *= 2f) < 1 )
				return c * 0.5f * t * t * t * t + b;

			return -c * 0.5f * ( ( t -= 2 ) * t * t * t - 2 ) + b;
		}

		/// <summary>
		/// Easing equation function for a quartic (t^4) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuartEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return QuartEaseOut( t * 2, b, c * 0.5f, d );

			return QuartEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuintEaseOut( float t, float b, float c, float d ) {
			return c * ( ( t = t - 1 ) * t * t * t * t + 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuintEaseIn( float t, float b, float c, float d ) {
			return c * t * t * t * t * t + b;
		}

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuintEaseInOut( float t, float b, float c, float d ) {
			if ( (t *= 2f) < 1 )
				return c * 0.5f * t * t * t * t * t + b;
			return c * 0.5f * ( ( t -= 2 ) * t * t * t * t + 2 ) + b;
		}

		/// <summary>
		/// Easing equation function for a quintic (t^5) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuintEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return QuintEaseOut( t * 2, b, c * 0.5f, d );
			return QuintEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ElasticEaseOut( float t, float b, float c, float d ) {
			if ( t == 1 )
				return b + c;

			float p = d * 0.3f;
			float s = p * 0.25f;

			return ( c * Mathf.Pow( 2, -10 * t ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) + c + b );
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ElasticEaseIn( float t, float b, float c, float d ) {
			if ( t == 1 )
				return b + c;

			float p = d * 0.3f;
			float s = p * 0.25f;

			return -( c * Mathf.Pow( 2, 10 * ( t -= 1 ) ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) ) + b;
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ElasticEaseInOut( float t, float b, float c, float d ) {
			if ( (t *= 2f) == 2 )
				return b + c;

			float p = d * 0.45f;//( 0.3f * 1.5f );
			float s = p * 0.25f;

			if ( t < 1 )
				return -0.5f * ( c * Mathf.Pow( 2, 10 * ( t -= 1 ) ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) ) + b;
			return c * Mathf.Pow( 2, -10 * ( t -= 1 ) ) * Mathf.Sin( ( t * d - s ) * ( 2 * Mathf.PI ) / p ) * 0.5f + c + b;
		}

		/// <summary>
		/// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float ElasticEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return ElasticEaseOut( t * 2, b, c * 0.5f, d );
			return ElasticEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BounceEaseOut( float t, float b, float c, float d ) {
			if ( t < 0.36363f )//1 * 0.36363f )
				return c * ( 7.5625f * t * t ) + b;
			else if ( t < 0.72726f )//2f * 0.36363f )
				return c * ( 7.5625f * ( t -= 0.545445f ) * t + 0.75f ) + b;
			else if ( t < 0.909075f )//2.5f * 0.36363f )
				return c * ( 7.5625f * ( t -= 0.8181675f ) * t + 0.9375f ) + b;
			else
				return c * ( 7.5625f * ( t -= 0.95452875f ) * t + 0.984375f ) + b;
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BounceEaseIn( float t, float b, float c, float d ) {
			return c - BounceEaseOut( 1f - t, 0, c, d ) + b;
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BounceEaseInOut( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return BounceEaseIn( t * 2, 0, c, d ) * 0.5f + b;
			else
				return BounceEaseOut( t * 2 - 1f, 0, c, d ) * 0.5f + c * 0.5f + b;
		}

		/// <summary>
		/// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BounceEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return BounceEaseOut( t * 2, b, c * 0.5f, d );
			return BounceEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
		/// decelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BackEaseOut( float t, float b, float c, float d ) {
			return c * ( ( t = t - 1 ) * t * ( 2.70158f * t + 1.70158f ) + 1 ) + b;
		}

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BackEaseIn( float t, float b, float c, float d ) {
			return c * t * t * ( 2.70158f * t - 1.70158f ) + b;
		}

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
		/// acceleration until halfway, then deceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BackEaseInOut( float t, float b, float c, float d ) {
			float s = 1.70158f;
			if ( (t *= 2f) < 1 )
				return c * 0.5f * ( t * t * ( ( ( s *= 1.525f ) + 1 ) * t - s ) ) + b;
			return c * 0.5f * ( ( t -= 2 ) * t * ( ( ( s *= 1.525f ) + 1 ) * t + s ) + 2 ) + b;
		}

		/// <summary>
		/// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
		/// deceleration until halfway, then acceleration.
		/// </summary>
		/// <param name="t">Lerp time (0~1).</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Change in value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float BackEaseOutIn( float t, float b, float c, float d ) {
			if ( t < 0.5f )
				return BackEaseOut( t * 2, b, c * 0.5f, d );
			return BackEaseIn( ( t * 2 ) - 1f, b + c * 0.5f, c * 0.5f, d );
		}
	}

	/// <summary>
	/// Types of ease methods available.
	/// </summary>
	public enum EaseType {
		Linear = 0,
		EaseOut,
		EaseIn,
		ExpoEaseOut,
		ExpoEaseIn,
		ExpoEaseInOut,
		ExpoEaseOutIn,
		CircEaseOut,
		CircEaseIn,
		CircEaseInOut,
		CircEaseOutIn,
		QuadEaseOut,
		QuadEaseIn,
		QuadEaseInOut,
		QuadEaseOutIn,
		SineEaseOut,
		SineEaseIn,
		SineEaseInOut,
		SineEaseOutIn,
		CubicEaseOut,
		CubicEaseIn,
		CubicEaseInOut,
		CubicEaseOutIn,
		QuartEaseOut,
		QuartEaseIn,
		QuartEaseInOut,
		QuartEaseOutIn,
		QuintEaseOut,
		QuintEaseIn,
		QuintEaseInOut,
		QuintEaseOutIn,
		ElasticEaseOut,
		ElasticEaseIn,
		ElasticEaseInOut,
		ElasticEaseOutIn,
		BounceEaseOut,
		BounceEaseIn,
		BounceEaseInOut,
		BounceEaseOutIn,
		BackEaseOut,
		BackEaseIn,
		BackEaseInOut,
		BackEaseOutIn,

		END
	}
}