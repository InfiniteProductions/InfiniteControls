// Timed button: disabled for n seconds after being clicked

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


// allow addition of a spritesheet to blend with the button
// size, cell size, could be a background "clock" or a small foreground one, or a digital timer

namespace InfiniteControls
{
    public class TButton : SimpleButton
    {
        // need another state: coolingdown !

        public float ReleaseTime { get; private set; }
        public float CooldownTime { get; private set; }

        private float pressedTime = 0f;

        public float coolingDown { get; private set; }

        public TButton(Vector2 position, Vector2 size, Texture2D textureoff = null, Texture2D textureon = null, float releaseTime = 0.5f, float cooldown = 0) : base(position, size, releaseTime, CollisionType.Rectangle, textureoff, textureon)
        {
            ReleaseTime = releaseTime;

            buttonState = buttonState.Released;

            // if cooldown set to something > 0, it act like a bonus that needs to be recharged before used again
            CooldownTime = cooldown;
            coolingDown = cooldown;
        }


        public override void Update(GameTime gameTime, bool click)
        {
            System.Diagnostics.Debug.Print(string.Format("E={0} cd={1}", Enabled, coolingDown));

            
            // while cooling down, control is disabled
            if (coolingDown <= 0)
            {
                System.Diagnostics.Debug.Print("end of cool down time");
                Enabled = true;
                buttonState = buttonState.Released;
                
                coolingDown = CooldownTime;
            }
            else
            {
                if (buttonState == buttonState.Pressed)
                {
                    coolingDown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (Enabled == false)
                return;

            Rectangle mouseclick = new Rectangle(Mouse.GetState().X - 2, Mouse.GetState().Y - 2, 4, 4);

            //pressed time check need to be moved above !
            if (click == true && mouseclick.Intersects(ButtonArea))
            {
                buttonState = buttonState.Pressed;
                pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                coolingDown -= pressedTime;

                currentTexture = ReleasedTexture; // pointless, overriden later
                Enabled = false;
            }

            if (pressedTime >= ReleaseTime)
            {
                buttonState = buttonState.Released;
                pressedTime = 0;
            }

            if (buttonState == buttonState.Released)
            {
                currentTexture = ReleasedTexture;
            }
            else
            {
                currentTexture = PressedTexture;
                pressedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }


        public override void Draw()
        {
            // need to be called INSIDE a spritebatch set !
            // ~ draw a clockstyle filling tex
            spriteBatch.Draw(currentTexture, ButtonArea, Color.White);
        }


        public override bool Pressed()
        {
            return base.Pressed();
        }


        public override bool Released()
        {
            return base.Released();
        }


        // supposed ro be event (async)
        public override void onPressed()
        {

        }


        public override void onReleased()
        {

        }
    }
}
