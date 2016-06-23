using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace InfiniteControls	//~ => .Buttons
{
    public class Button
    {
        public enum State : byte { Released, Pressed };

        public Vector2 Position { get; private set; }
        public Vector2 Size { get; private set; }
        public Texture2D ReleasedTexture { get; private set; }
        public Texture2D PressedTexture { get; private set; }
        public float ReleaseTime { get; private set; }
        public float CooldownTime { get; private set; }

        public SpriteBatch spriteBatch { get; set; }
        
        private Rectangle ButtonArea;
        private State buttonState;
        private Texture2D currentTexture;

        private float pressedTime = 0f;
		public float coolingDown { get; private set; }


		public Button(Vector2 position, Vector2 size, Texture2D textureoff = null, Texture2D textureon = null, float releaseTime = 0.5f, float cooldown = 0)
        {
            Position = position;
            Size = size;
            ReleasedTexture = textureoff;
            PressedTexture = textureon;
            ReleaseTime = releaseTime;

            // if cooldown set to something > 0, it act like a bonus that needs to be recharged before used again
            CooldownTime = cooldown;
			coolingDown = cooldown;

			ButtonArea = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)size.Y);
            buttonState = State.Released;
            currentTexture = ReleasedTexture;

            //need something to set simple boolean button (pressed/released)
            // + two way buttons (can be pressed/relesed = cb !)
        }


        public void SetTextures(Texture2D onTex, Texture2D offTex)
        {
            ReleasedTexture = offTex;
            PressedTexture = onTex;
        }


        public void Update(GameTime gameTime, bool click)
        {
			// while cooling down, control is disabled
			if (coolingDown <= 0)
			{
			}

			Rectangle mouseclick = new Rectangle(Mouse.GetState().X - 2, Mouse.GetState().Y - 2, 4, 4);

            if (click == true && mouseclick.Intersects(ButtonArea))
            {
                buttonState = State.Pressed;
                pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
				coolingDown -= pressedTime;
			}

            if (pressedTime >= ReleaseTime)
            {
				// need a property/field to get the remaining cooling time
				// while colling down = button released !
				//if (CooldownTime == 0 || CooldownTime <= pressedTime)
				//{
				buttonState = State.Released;
				pressedTime = 0;
				//}
			}

			if (buttonState == State.Released)
            {
                currentTexture = ReleasedTexture;
            }
            else
            {
                currentTexture = PressedTexture;
                pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }


        public void Draw()
        {
            // need to be called INSIDE a spritebatch set !
            spriteBatch.Draw(currentTexture, ButtonArea, Color.White);
        }


        public void Pressed()
        {

        }


        public void Released()
        {

        }


        // supposed ro be event (async)
        public void onPressed()
        {

        }


        public void onReleased()
        {

        }
    }
}
