using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InfiniteControls
{
// time progress 'strategy game, like building construction or someone doing something)
// ressource gathering 
// upd must be "feed" by caller
// 1st version no "tip" on the bar

// label: on the bar itself, above, below, left, right (justify parameter)
// display: % or count (for resource) or something else (time ? value matching %), time ~for games like Millionaire city

// vertical: beware: y goes the opposite way as x, so 90° rot won't work
// reverse progress bar: for time completion or time needed before something

    //base class => child: progress with % test on/off
    // ressource bar, time bar
    // no tex provided: allocate simple tex 1px
    public class ProgressBar
    {
        private enum VSSizes : byte { Body = 0, Gap = 1, Step = 2, Width = 3, Height = 4 };
		private enum Justify : byte { None = 0, In, Above, Below, Left, Right };

        public Vector2 Position { get; private set; }
        public UInt16[] Sizes { get; private set; }
        public bool DisplayText { get; private set; }

        public Texture2D BodyTexture { get; private set; }
        public Texture2D StepTexture { get; private set; }

        // hold current progress from 0 to 100
        public Byte currentProgress { get; private set; }

        public Byte StepSize { get; private set; }

        public SpriteBatch spriteBatch { get; set; }

        private Rectangle ControlArea;
        private Rectangle StepRect;


        public ProgressBar(Vector2 position, UInt16[] sizes, bool text = false, Byte defaultprogress = 0, Byte step = 1)
        {
            Position = position;
            Sizes = sizes;
            DisplayText = text;
            currentProgress = defaultprogress;
            StepSize = step;

            ControlArea = new Rectangle((int)position.X + sizes[(int)VSSizes.Gap], (int)position.Y, sizes[(int)VSSizes.Width], sizes[(int)VSSizes.Height]);
            StepRect = new Rectangle();
        }


        public void SetTextures(Texture2D body, Texture2D step)
        {
            BodyTexture = body;
            StepTexture = step;
        }


        public void Update(GameTime gameTime, Byte progress)
        {
            Byte actualProgress = (Byte)(progress / StepSize);

            //tmp, ~ need to fill it * dt
            currentProgress += actualProgress;

            currentProgress = (Byte)MathHelper.Clamp(currentProgress, 0, 100);
            // fill the bar to progress count (0-100)
            // upd steprect size accordingly
        }


        public Byte GetProgress()
        {
            return currentProgress;
        }


        public void Draw()
        {
			// draw core/body of the bar
			//   then progress itself: small sprite increment which will have to be drawn in line: progress=20% = 2 items to display (step = 10%)
            spriteBatch.Draw(StepTexture, StepRect, Color.White);
            spriteBatch.Draw(BodyTexture, ControlArea, Color.White);
        }
    }
}
