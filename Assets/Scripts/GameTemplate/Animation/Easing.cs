using UnityEngine;

public static class Easing
{
	/// <summary>
	/// Constant Pi.
	/// </summary>
	private const float Pi = Mathf.PI; 
	
	/// <summary>
	/// Constant Pi / 2.
	/// </summary>
	private const float Halfpi = Mathf.PI / 2.0f; 
	
	/// <summary>
	/// Easing Functions enumeration
	/// </summary>
	public enum Functions
	{
		Linear,
		QuadraticEaseIn,
		QuadraticEaseOut,
		QuadraticEaseInOut,
		CubicEaseIn,
		CubicEaseOut,
		CubicEaseInOut,
		QuarticEaseIn,
		QuarticEaseOut,
		QuarticEaseInOut,
		QuinticEaseIn,
		QuinticEaseOut,
		QuinticEaseInOut,
		SineEaseIn,
		SineEaseOut,
		SineEaseInOut,
		CircularEaseIn,
		CircularEaseOut,
		CircularEaseInOut,
		ExponentialEaseIn,
		ExponentialEaseOut,
		ExponentialEaseInOut,
		ElasticEaseIn,
		ElasticEaseOut,
		ElasticEaseInOut,
		BackEaseIn,
		BackEaseOut,
		BackEaseInOut,
		BounceEaseIn,
		BounceEaseOut,
		BounceEaseInOut
	}
	
	/// <summary>
	/// Interpolate using the specified function.
	/// </summary>
	public static float Interpolate(float p, Functions function)
	{
		switch(function)
		{
			default:                                return Linear(p);
			case Functions.QuadraticEaseOut:		return QuadraticEaseOut(p);
			case Functions.QuadraticEaseIn:			return QuadraticEaseIn(p);
			case Functions.QuadraticEaseInOut:		return QuadraticEaseInOut(p);
			case Functions.CubicEaseIn:				return CubicEaseIn(p);
			case Functions.CubicEaseOut:			return CubicEaseOut(p);
			case Functions.CubicEaseInOut:			return CubicEaseInOut(p);
			case Functions.QuarticEaseIn:			return QuarticEaseIn(p);
			case Functions.QuarticEaseOut:			return QuarticEaseOut(p);
			case Functions.QuarticEaseInOut:		return QuarticEaseInOut(p);
			case Functions.QuinticEaseIn:			return QuinticEaseIn(p);
			case Functions.QuinticEaseOut:			return QuinticEaseOut(p);
			case Functions.QuinticEaseInOut:		return QuinticEaseInOut(p);
			case Functions.SineEaseIn:				return SineEaseIn(p);
			case Functions.SineEaseOut:				return SineEaseOut(p);
			case Functions.SineEaseInOut:			return SineEaseInOut(p);
			case Functions.CircularEaseIn:			return CircularEaseIn(p);
			case Functions.CircularEaseOut:			return CircularEaseOut(p);
			case Functions.CircularEaseInOut:		return CircularEaseInOut(p);
			case Functions.ExponentialEaseIn:		return ExponentialEaseIn(p);
			case Functions.ExponentialEaseOut:		return ExponentialEaseOut(p);
			case Functions.ExponentialEaseInOut:	return ExponentialEaseInOut(p);
			case Functions.ElasticEaseIn:			return ElasticEaseIn(p);
			case Functions.ElasticEaseOut:			return ElasticEaseOut(p);
			case Functions.ElasticEaseInOut:		return ElasticEaseInOut(p);
			case Functions.BackEaseIn:				return BackEaseIn(p);
			case Functions.BackEaseOut:				return BackEaseOut(p);
			case Functions.BackEaseInOut:			return BackEaseInOut(p);
			case Functions.BounceEaseIn:			return BounceEaseIn(p);
			case Functions.BounceEaseOut:			return BounceEaseOut(p);
			case Functions.BounceEaseInOut:			return BounceEaseInOut(p);
		}
	}

    public static Functions Reverse(Functions f)
    {
        switch (f)
        {
            default:                                return Functions.Linear;
            case Functions.QuadraticEaseOut:        return Functions.QuadraticEaseIn;
            case Functions.QuadraticEaseIn:         return Functions.QuadraticEaseOut;
            case Functions.QuadraticEaseInOut:      return Functions.QuadraticEaseInOut;
            case Functions.CubicEaseIn:             return Functions.CubicEaseOut;
            case Functions.CubicEaseOut:            return Functions.CubicEaseIn;
            case Functions.CubicEaseInOut:          return Functions.CubicEaseInOut;
            case Functions.QuarticEaseIn:           return Functions.QuarticEaseOut;
            case Functions.QuarticEaseOut:          return Functions.QuarticEaseIn;
            case Functions.QuarticEaseInOut:        return Functions.QuarticEaseInOut;
            case Functions.QuinticEaseIn:           return Functions.QuinticEaseOut;
            case Functions.QuinticEaseOut:          return Functions.QuinticEaseIn;
            case Functions.QuinticEaseInOut:        return Functions.QuinticEaseInOut;
            case Functions.SineEaseIn:              return Functions.SineEaseOut;
            case Functions.SineEaseOut:             return Functions.SineEaseIn;
            case Functions.SineEaseInOut:           return Functions.SineEaseInOut;
            case Functions.CircularEaseIn:          return Functions.CircularEaseOut;
            case Functions.CircularEaseOut:         return Functions.CircularEaseIn;
            case Functions.CircularEaseInOut:       return Functions.CircularEaseInOut;
            case Functions.ExponentialEaseIn:       return Functions.ExponentialEaseOut;
            case Functions.ExponentialEaseOut:      return Functions.ExponentialEaseIn;
            case Functions.ExponentialEaseInOut:    return Functions.ExponentialEaseInOut;
            case Functions.ElasticEaseIn:           return Functions.ElasticEaseOut;
            case Functions.ElasticEaseOut:          return Functions.ElasticEaseIn;
            case Functions.ElasticEaseInOut:        return Functions.ElasticEaseInOut;
            case Functions.BackEaseIn:              return Functions.BackEaseOut;
            case Functions.BackEaseOut:             return Functions.BackEaseIn;
            case Functions.BackEaseInOut:           return Functions.BackEaseInOut;
            case Functions.BounceEaseIn:            return Functions.BounceEaseOut;
            case Functions.BounceEaseOut:           return Functions.BounceEaseIn;
            case Functions.BounceEaseInOut:         return Functions.BounceEaseInOut;
        }
    }
	
	/// <summary>
	/// Modeled after the line y = x
	/// </summary>
	public static float Linear(float p)
	{
		return p;
	}
	
	/// <summary>
	/// Modeled after the parabola y = x^2
	/// </summary>
	public static float QuadraticEaseIn(float p)
	{
		return p * p;
	}
	
	/// <summary>
	/// Modeled after the parabola y = -x^2 + 2x
	/// </summary>
	public static float QuadraticEaseOut(float p)
	{
		return -(p * (p - 2));
	}
	
	/// <summary>
	/// Modeled after the piecewise quadratic
	/// y = (1/2)((2x)^2)             ; [0, 0.5)
	/// y = -(1/2)((2x-1)*(2x-3) - 1) ; [0.5, 1]
	/// </summary>
	public static float QuadraticEaseInOut(float p)
	{
	    if(p < 0.5f)
		{
			return 2 * p * p;
		}
	    return (-2 * p * p) + (4 * p) - 1;
	}
	
	/// <summary>
	/// Modeled after the cubic y = x^3
	/// </summary>
	public static float CubicEaseIn(float p)
	{
		return p * p * p;
	}
	
	/// <summary>
	/// Modeled after the cubic y = (x - 1)^3 + 1
	/// </summary>
	public static float CubicEaseOut(float p)
	{
		var f = (p - 1);
		return f * f * f + 1;
	}
	
	/// <summary>	
	/// Modeled after the piecewise cubic
	/// y = (1/2)((2x)^3)       ; [0, 0.5)
	/// y = (1/2)((2x-2)^3 + 2) ; [0.5, 1]
	/// </summary>
	public static float CubicEaseInOut(float p)
	{
		if(p < 0.5f)
		{
			return 4 * p * p * p;
		}
	    var f = ((2 * p) - 2);
	    return 0.5f * f * f * f + 1;
	}
	
	/// <summary>
	/// Modeled after the quartic x^4
	/// </summary>
	public static float QuarticEaseIn(float p)
	{
		return p * p * p * p;
	}
	
	/// <summary>
	/// Modeled after the quartic y = 1 - (x - 1)^4
	/// </summary>
	public static float QuarticEaseOut(float p)
	{
		var f = (p - 1);
		return f * f * f * (1 - p) + 1;
	}
	
	/// <summary>
	// Modeled after the piecewise quartic
	// y = (1/2)((2x)^4)        ; [0, 0.5)
	// y = -(1/2)((2x-2)^4 - 2) ; [0.5, 1]
	/// </summary>
	public static float QuarticEaseInOut(float p)
	{
		if(p < 0.5f)
		{
			return 8 * p * p * p * p;
		}
	    var f = (p - 1);
	    return -8 * f * f * f * f + 1;
	}
	
	/// <summary>
	/// Modeled after the quintic y = x^5
	/// </summary>
	public static float QuinticEaseIn(float p)
	{
		return p * p * p * p * p;
	}
	
	/// <summary>
	/// Modeled after the quintic y = (x - 1)^5 + 1
	/// </summary>
	public static float QuinticEaseOut(float p)
	{
		var f = (p - 1);
		return f * f * f * f * f + 1;
	}
	
	/// <summary>
	/// Modeled after the piecewise quintic
	/// y = (1/2)((2x)^5)       ; [0, 0.5)
	/// y = (1/2)((2x-2)^5 + 2) ; [0.5, 1]
	/// </summary>
	public static float QuinticEaseInOut(float p)
	{
		if(p < 0.5f)
		{
			return 16 * p * p * p * p * p;
		}
	    var f = ((2 * p) - 2);
	    return 0.5f * f * f * f * f * f + 1;
	}
	
	/// <summary>
	/// Modeled after quarter-cycle of sine wave
	/// </summary>
	public static float SineEaseIn(float p)
	{
		return Mathf.Sin((p - 1) * Halfpi) + 1;
	}
	
	/// <summary>
	/// Modeled after quarter-cycle of sine wave (different phase)
	/// </summary>
	public static float SineEaseOut(float p)
	{
		return Mathf.Sin(p * Halfpi);
	}
	
	/// <summary>
	/// Modeled after half sine wave
	/// </summary>
	public static float SineEaseInOut(float p)
	{
		return 0.5f * (1 - Mathf.Cos(p * Pi));
	}
	
	/// <summary>
	/// Modeled after shifted quadrant IV of unit circle
	/// </summary>
	public static float CircularEaseIn(float p)
	{
		return 1 - Mathf.Sqrt(1 - (p * p));
	}
	
	/// <summary>
	/// Modeled after shifted quadrant II of unit circle
	/// </summary>
	public static float CircularEaseOut(float p)
	{
		return Mathf.Sqrt((2 - p) * p);
	}
	
	/// <summary>	
	/// Modeled after the piecewise circular function
	/// y = (1/2)(1 - Mathf.Sqrt(1 - 4x^2))           ; [0, 0.5)
	/// y = (1/2)(Mathf.Sqrt(-(2x - 3)*(2x - 1)) + 1) ; [0.5, 1]
	/// </summary>
	public static float CircularEaseInOut(float p)
	{
	    if(p < 0.5f)
		{
			return 0.5f * (1 - Mathf.Sqrt(1 - 4 * (p * p)));
		}
	    return 0.5f * (Mathf.Sqrt(-((2 * p) - 3) * ((2 * p) - 1)) + 1);
	}
	
	/// <summary>
	/// Modeled after the exponential function y = 2^(10(x - 1))
	/// </summary>
	public static float ExponentialEaseIn(float p)
	{
		return (p == 0.0f) ? p : Mathf.Pow(2, 10 * (p - 1));
	}
	
	/// <summary>
	/// Modeled after the exponential function y = -2^(-10x) + 1
	/// </summary>
	public static float ExponentialEaseOut(float p)
	{
		return (p == 1.0f) ? p : 1 - Mathf.Pow(2, -10 * p);
	}
	
	/// <summary>
	/// Modeled after the piecewise exponential
	/// y = (1/2)2^(10(2x - 1))         ; [0,0.5)
	/// y = -(1/2)*2^(-10(2x - 1))) + 1 ; [0.5,1]
	/// </summary>
	public static float ExponentialEaseInOut(float p)
	{
		if(p == 0.0 || p == 1.0) return p;
		
		if(p < 0.5f)
		{
			return 0.5f * Mathf.Pow(2, (20 * p) - 10);
		}
	    return -0.5f * Mathf.Pow(2, (-20 * p) + 10) + 1;
	}
	
	/// <summary>
	/// Modeled after the damped sine wave y = sin(13pi/2*x)*Mathf.Pow(2, 10 * (x - 1))
	/// </summary>
	public static float ElasticEaseIn(float p)
	{
		return Mathf.Sin(13 * Halfpi * p) * Mathf.Pow(2, 10 * (p - 1));
	}
	
	/// <summary>
	/// Modeled after the damped sine wave y = sin(-13pi/2*(x + 1))*Mathf.Pow(2, -10x) + 1
	/// </summary>
	public static float ElasticEaseOut(float p)
	{
		return Mathf.Sin(-13 * Halfpi * (p + 1)) * Mathf.Pow(2, -10 * p) + 1;
	}
	
	/// <summary>
	/// Modeled after the piecewise exponentially-damped sine wave:
	/// y = (1/2)*sin(13pi/2*(2*x))*Mathf.Pow(2, 10 * ((2*x) - 1))      ; [0,0.5)
	/// y = (1/2)*(sin(-13pi/2*((2x-1)+1))*Mathf.Pow(2,-10(2*x-1)) + 2) ; [0.5, 1]
	/// </summary>
	public static float ElasticEaseInOut(float p)
	{
	    if(p < 0.5f)
		{
			return 0.5f * Mathf.Sin(13 * Halfpi * (2 * p)) * Mathf.Pow(2, 10 * ((2 * p) - 1));
		}
	    return 0.5f * (Mathf.Sin(-13 * Halfpi * ((2 * p - 1) + 1)) * Mathf.Pow(2, -10 * (2 * p - 1)) + 2);
	}
	
	/// <summary>
	/// Modeled after the overshooting cubic y = x^3-x*sin(x*pi)
	/// </summary>
	public static float BackEaseIn(float p)
	{
		return p * p * p - p * Mathf.Sin(p * Pi);
	}
	
	/// <summary>
	/// Modeled after overshooting cubic y = 1-((1-x)^3-(1-x)*sin((1-x)*pi))
	/// </summary>	
	public static float BackEaseOut(float p)
	{
		var f = (1 - p);
		return 1 - (f * f * f - f * Mathf.Sin(f * Pi));
	}
	
	/// <summary>
	/// Modeled after the piecewise overshooting cubic function:
	/// y = (1/2)*((2x)^3-(2x)*sin(2*x*pi))           ; [0, 0.5)
	/// y = (1/2)*(1-((1-x)^3-(1-x)*sin((1-x)*pi))+1) ; [0.5, 1]
	/// </summary>
	public static float BackEaseInOut(float p)
	{
		if(p < 0.5f)
		{
			var f = 2 * p;
			return 0.5f * (f * f * f - f * Mathf.Sin(f * Pi));
		}
		else
		{
			var f = (1 - (2*p - 1));
			return 0.5f * (1 - (f * f * f - f * Mathf.Sin(f * Pi))) + 0.5f;
		}
	}
	
	/// <summary>
	/// </summary>
	public static float BounceEaseIn(float p)
	{
		return 1 - BounceEaseOut(1 - p);
	}
	
	/// <summary>
	/// </summary>
	public static float BounceEaseOut(float p)
	{
	    if(p < 4/11.0f)
		{
			return (121 * p * p)/16.0f;
		}
	    if(p < 8/11.0f)
	    {
	        return (363/40.0f * p * p) - (99/10.0f * p) + 17/5.0f;
	    }
	    if(p < 9/10.0f)
	    {
	        return (4356/361.0f * p * p) - (35442/1805.0f * p) + 16061/1805.0f;
	    }
	    return (54/5.0f * p * p) - (513/25.0f * p) + 268/25.0f;
	}
	
	/// <summary>
	/// </summary>
	public static float BounceEaseInOut(float p)
	{
	    if(p < 0.5f)
		{
			return 0.5f * BounceEaseIn(p*2);
		}
	    return 0.5f * BounceEaseOut(p * 2 - 1) + 0.5f;
	}
}
 	
