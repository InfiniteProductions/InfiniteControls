using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfiniteControls //.Helpers
{
	public class Circle
	{
		public float Radius;
		public Vector2 Centre;


		public Circle(float radius, Vector2 centre)
		{
			Radius = radius;
			Centre = centre;
		}

		// warning order may matters !!
		// mouse/circle 30^2 + 32^2 > 36 ^2
		public bool Intersects(Circle circleB)
		{
			bool collide = false;

			int sqrRadius = (int)System.Math.Pow((double)(Radius + circleB.Radius), 2);
			int xdiff = (int)(Centre.X - circleB.Centre.X);
			int ydiff = (int)(Centre.Y - circleB.Centre.Y);

			if (xdiff * xdiff + ydiff * ydiff < sqrRadius)
			{
				collide = true;
			}

			return collide;
		}
	}
}
