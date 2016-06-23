using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


// ~ hook for playing sound when pressed/released

// main generic button class (pressed/released) => press with time button, cb classes
// rawButton - SimpleButton (=CB?)
//           - TimedButton
//           - Checkbox
// need mouse input objects (prev, current)

// add cooldown button (for bonus that's can't be use all the time, like in tower defense games)

namespace InfiniteControls
{
    public enum buttonState : byte { Released, Pressed };
	public enum CollisionType : byte { Rectangle, Circle };

    public class SimpleButton
    {
        public Vector2 Position { get; private set; }
        public Vector2 Size { get; private set; }
        public Texture2D ReleasedTexture { get; private set; }
        public Texture2D PressedTexture { get; private set; }

        public bool Enabled { get; set; }
		public bool EnableTint { get; set; }  //OnMouseHovering
		public Color tintColour { get; set; }
		public CollisionType collisionType { get; set; }

		public SpriteBatch spriteBatch { get; set; }

		// for rectangle based "collision" (with mouse pointer) check
		protected Rectangle ButtonArea;
		protected Circle ButtonAreaCircle;

		protected buttonState buttonState;
		protected Texture2D currentTexture;
		protected Texture2D DisabledOverlay;

		private float pressedTime;
        private float ReleaseTime;  //tmp ?


        public SimpleButton(Vector2 position, Vector2 size, float reltime = 0.3f, CollisionType ctype = CollisionType.Rectangle, Texture2D textureoff = null, Texture2D textureon = null)
        {
            Position = position;
            Size = size;
            ReleasedTexture = textureoff;
            PressedTexture = textureon;

			if (ctype == CollisionType.Rectangle)
			{
				ButtonArea = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)size.Y);
				collisionType = CollisionType.Rectangle;
			}
			else
			{
				ButtonAreaCircle = new Circle(Size.X /2.0f, new Vector2(position.X, position.Y));
				collisionType = CollisionType.Circle;
			}

            buttonState = buttonState.Released;
            currentTexture = ReleasedTexture;

            Enabled = true;
			EnableTint = false;
			tintColour = Color.White;

            pressedTime = 0;
            ReleaseTime = reltime;
        }


        public void SetTextures(Texture2D onTex, Texture2D offTex, Texture2D ghost = null)
		{
            ReleasedTexture = offTex;
            PressedTexture = onTex;
			DisabledOverlay = ghost; // should be created here, caller may don't have to bother with it !

			currentTexture = ReleasedTexture;
        }


        public virtual void Update(GameTime gameTime, bool click)
        {
			// NEED TO BE dupe inside update code of the child class to work (method is overriden !!)
			if (Enabled == false)
				return;

			// tint on hovering:
			// if mouse coord inside control area => change tint

			if (click == true)
            {
				if (collisionType == CollisionType.Rectangle)
				{
					Rectangle mouseclick = new Rectangle(Mouse.GetState().X - 2, Mouse.GetState().Y - 2, 4, 4);

					System.Diagnostics.Debug.Print(string.Format("SB upd mc={0}", mouseclick));

					if (mouseclick.Intersects(ButtonArea))
					{
						buttonState = buttonState.Pressed;
						pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

						System.Diagnostics.Debug.Print(string.Format("SB upd click pt={0}", pressedTime));
					}
				}
				else
				{
					Circle mouseclick = new Circle(4.0f, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

					System.Diagnostics.Debug.Print(string.Format("SBcc upd mc={0}", mouseclick));

					if (mouseclick.Intersects(ButtonAreaCircle))
					{
						buttonState = buttonState.Pressed;
						pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

						System.Diagnostics.Debug.Print(string.Format("SBcc upd click pt={0}", pressedTime));
					}
				}
            }

            if (pressedTime >= ReleaseTime)
            {
                buttonState = buttonState.Released;
                pressedTime = 0;

                System.Diagnostics.Debug.Print(string.Format("SB upd rel pt={0}", pressedTime));
            }

            if (buttonState == buttonState.Released)
            {
                currentTexture = ReleasedTexture;
            }
            else
            {
                currentTexture = PressedTexture;
                pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                System.Diagnostics.Debug.Print(string.Format("SB upd click pressed={0}", pressedTime));
            }
        }


        public virtual void Draw()
        {
            // need to be called INSIDE a spritebatch set !
            //~if disabled draw a gray "shadow" on top of it
            spriteBatch.Draw(currentTexture, ButtonArea, tintColour);
        }


        public virtual bool Pressed()
        {
            return buttonState == buttonState.Pressed;
        }


        public virtual bool Released()
        {
            return buttonState == buttonState.Released;
        }


        // supposed to be event (async)
        public virtual void onPressed()
        {

        }


        public virtual void onReleased()
        {

        }
    }
}
