using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// issue: vol=100 cursor hide/drawn below by right arrow (+) sprite
// body and spaces before inc button are short 1 incr

// vertical ! y goes opp way as x !

namespace InfiniteControls
{
    public class VolumeSelector
    {
        private enum VSSizes : byte { IncButton = 0, DecButton = 1, Body = 2, Cursor = 3, Gap = 4, Height = 5 };
		private enum Way : byte { Horiz, Vert }; // or bool: false = H, true = V (isVertical)

        public Vector2 Position { get; private set; }
        public UInt16[] Sizes { get; private set; }
        // buttons sizes + body/cursor sizes

        public Texture2D DecreaseTextureReleased { get; private set; }
        public Texture2D IncreaseTextureReleased { get; private set; }
        public Texture2D DecreaseTexturePressed { get; private set; }
        public Texture2D IncreaseTexturePressed { get; private set; }
        public Texture2D BodyTexture { get; private set; }
        public Texture2D CursorTexture { get; private set; }

        private SimpleButton DecreaseVolume, IncreaseVolume;
        private Rectangle Cursor;   // or a sprite
        private Rectangle Body; //~

        // hold current volume fro 0 to 100
        public Byte currentVolume { get; private set; }

        // increment size when clicking on buttons, = precision of settings
        public Byte StepSize { get; private set; }

        public SpriteBatch spriteBatch { get; set; }

        //private Texture2D baseTexture;  // ~tmp
        private Rectangle ControlArea;
        private Rectangle CursorRect;

		// <----O---->
		// 4 tex: -,+, cursor, area
		// pos: top/left corner, ~size of button, size of area, siez of cursor

		// sizes: 5 items: dec button, inc button, body, cursor (width), gap and height (common for all parts)
		public VolumeSelector(Vector2 position, UInt16[] sizes, Byte defaultVolume = 50, Byte stepsize = 10)
        {
            Position = position;
            Sizes = sizes;
            currentVolume = defaultVolume;
            StepSize = stepsize;

			// check: body = cursor * 11

			int width = sizes[(int)VSSizes.Body] + 2 * sizes[(int)VSSizes.Gap]; //+ sizes[(int)VSSizes.DecButton] + sizes[(int)VSSizes.IncButton]
			System.Diagnostics.Debug.Print(string.Format("w={0} si-body={1} incpx={2}", width, sizes[(int)VSSizes.Body], (int)position.X + sizes[(int)VSSizes.Body] + sizes[(int)VSSizes.DecButton] + sizes[(int)VSSizes.Gap]));

			// ~ two cursor sizes are missing 0-10 and 90-100 => upd 19/06/16: only 1: 100
			// issue comes from: input data (validation/adjustement needed) or size calc here (not done ~body size may be min size)
			ControlArea = new Rectangle((int)position.X + sizes[(int)VSSizes.DecButton] + sizes[(int)VSSizes.Gap], (int)position.Y, width, sizes[(int)VSSizes.Height]);
			CursorRect = new Rectangle((int)position.X + sizes[(int)VSSizes.DecButton] + sizes[(int)VSSizes.Gap] + (currentVolume / stepsize) * sizes[(int)VSSizes.Cursor], (int)position.Y, sizes[(int)VSSizes.Cursor], sizes[(int)VSSizes.Height]);
			System.Diagnostics.Debug.Print(string.Format("contArea={0}", ControlArea));

			//pos, size, ISSUE/FIXED ==> inc pos is wrong, covered by/over body tex
			DecreaseVolume = new SimpleButton(new Vector2((int)position.X, (int)position.Y), new Vector2(sizes[(int)VSSizes.DecButton], sizes[(int)VSSizes.Height]), 0.02f);
			IncreaseVolume = new SimpleButton(new Vector2((int)position.X + sizes[(int)VSSizes.Body] + sizes[(int)VSSizes.DecButton] + sizes[(int)VSSizes.Gap], (int)position.Y), new Vector2(sizes[(int)VSSizes.IncButton], sizes[(int)VSSizes.Height]), 0.02f);
		}


        public void SetTextures(Texture2D body, Texture2D cursor, Texture2D decButtonR, Texture2D decButtonP, Texture2D incButtonR, Texture2D incButtonP)
        {
            DecreaseTextureReleased = decButtonR;
            IncreaseTextureReleased = incButtonR;
            DecreaseTexturePressed = decButtonP;
            IncreaseTexturePressed = incButtonP;

            BodyTexture = body;
            CursorTexture = cursor;

            DecreaseVolume.SetTextures(DecreaseTextureReleased, DecreaseTexturePressed);
            DecreaseVolume.spriteBatch = spriteBatch;

            IncreaseVolume.SetTextures(IncreaseTextureReleased, IncreaseTexturePressed);
            IncreaseVolume.spriteBatch = spriteBatch;
        }


        public void Update(GameTime gameTime, bool click)
        {
			// to be able to get cursor dragging:
			//  if mouse clicked on cursor => and button still pressed
			//  if mouse.X (or Y for vertical VS) + or -, incr/decr vol
            if (click == true) //wont work => click = true = mouse release
            {
                Rectangle mouseclick = new Rectangle(Mouse.GetState().X - 2, Mouse.GetState().Y - 2, 4, 4);

                if (mouseclick.Intersects(CursorRect))
                {
                    System.Diagnostics.Debug.Print("cursor");

                    //mouse button need to be kept pressed
                }
            }

            DecreaseVolume.Update(gameTime, click);
            IncreaseVolume.Update(gameTime, click);

            // 1 click = 0->100 or 100->0
            // pb button think its pressed for the time it is pressed before being effectively released !
            // cause pb here ! need to have p/r only ONCE
            if (DecreaseVolume.Pressed())
            {
                if (currentVolume > 0)
                    CursorRect.X -= 1 * Sizes[(int)VSSizes.Cursor]; // *StepSize;

                System.Diagnostics.Debug.Print("dec");
                currentVolume = (byte)MathHelper.Clamp(currentVolume - StepSize, 0, 100);

                
            }

            if (IncreaseVolume.Pressed())
            {
                if (currentVolume < 100)
                    CursorRect.X += 1 * Sizes[(int)VSSizes.Cursor];

                System.Diagnostics.Debug.Print("inc");
                currentVolume = (byte)MathHelper.Clamp(currentVolume + StepSize, 0, 100);

                
            }
        }


        public void Draw()
        {
			spriteBatch.Draw(BodyTexture, ControlArea, Color.White);
			spriteBatch.Draw(CursorTexture, CursorRect, Color.White);

			IncreaseVolume.Draw();
			DecreaseVolume.Draw();
		}


        public void VolumeIncreased()
        {
        }


        public void VolumeDecreased()
        {
        }


        public void MoveCursor(sbyte targetVolume)  // relative move, -5 or +3 or absolute: target=50%, 10%,...
        {
        }
    }
}
