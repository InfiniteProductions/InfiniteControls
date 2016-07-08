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
		public struct _Sizes
		{
			UInt16 Body;
			UInt16 Gap;
			UInt16 Step;
			UInt16 Width;
			UInt16 Height;
		}

		//body ?? not used
		// size: |AA[---------------------]AA|
		//        body texture thickness from left to inner spce where bar lies
        private enum VSSizes : byte { Body = 0, BodyThickness = 1, Gap = 2, Step = 3, Width = 4, Height = 5 };
		private enum Justify : byte { None = 0, In, Above, Below, Left, Right };

        public Vector2 Position { get; private set; }
        public UInt16[] Sizes { get; private set; }
        public bool DisplayText { get; private set; }

        public Texture2D BodyTexture { get; private set; }
		public Texture2D StepTexture { get; private set; }

        // hold current progress from 0 to 100
        public Byte currentProgress { get; private set; }

		// in which direction progress goes: true = forward, false = backward
		public bool progressWay { get; set; }

        public Byte StepSize { get; private set; }
		public float StepDelay { get; private set; }

		public string Text { get; set; }
		// add an array of strings for text associated to a given progress %
		// 10 generating this
		// 20 customizing that
		// 30 beautifulling foo

		public SpriteBatch spriteBatch { get; set; }

        private Rectangle ControlArea;
        private Rectangle StepRect;

		private float elapsedTime;
		private int stepwidth;
		private float goal;


		// + starting and total values
		public ProgressBar(Vector2 position, UInt16[] sizes, bool text = false, Byte defaultprogress = 0, Byte wantedResolution = 1, float stepdelay = 0f, bool way = true)
        {
            Position = position;
            Sizes = sizes;
            DisplayText = text;
			// ~add base text format (+precision for string format arg)
			// ex: "ETA: ", add in front of progress
			// + std format: number, time, %
			// number: raw print data, time: calc time from a given origin (1st call ?), % progress

			progressWay = way;
            currentProgress = defaultprogress;
			goal = 100.0f;

			if ((progressWay == true && currentProgress == 100) || (progressWay == false && currentProgress == 0))
			{
				//exception: invalid parameters
			}

            //StepSize = step;    // use to set width of progress texture on the bar (allow to use various step size with a single texture, which should be not too much width-sensitive to avoid stretching issues, in both ways)
			StepDelay = stepdelay;
			StepSize = wantedResolution;

			elapsedTime = 0f;

			// here: calculate step width/height (V) once and for all, then use it on update when a step is filled
			// (x   + sizes[(int)VSSizes.Gap]  ==> ??
			float accurateSW = (sizes[(int)VSSizes.Width] - sizes[(int)VSSizes.Gap] * 2) * wantedResolution / 100.0f;
			

			stepwidth = (int)accurateSW;
			System.Diagnostics.Debug.Print(string.Format("sw={0:F3} swi={1}", accurateSW, stepwidth));


			ControlArea = new Rectangle((int)position.X, (int)position.Y, sizes[(int)VSSizes.Width], sizes[(int)VSSizes.Height]);
            StepRect = new Rectangle((int)position.X + sizes[(int)VSSizes.Gap], (int)position.Y, 0, sizes[(int)VSSizes.Height]);
			// initial pos: fill or empty bar (countdown/reverse)

        }


		// ~ optional parameters: left/right part of the progress bar, so it wouldn't be art dependent/screen res dependent
        public void SetTextures(Texture2D body, Texture2D step)
        {
            BodyTexture = body;
            StepTexture = step;
        }


		// float for progress ?
        public void Update(GameTime gameTime, Byte progress = 1, string text = null)
        {
			// not 100: goal !
			if (currentProgress >= goal)
			{
				return;
			}

			// if wantedres=1 => 100 not in the end of the bar ! => progress here need to match WR (remove arg ?)
			// progress here is to fill bar "faster" for big progress made (like loading/generating level steps)
			// ISSUE: need a way to set progress to a bunch of steps not only 1 by 1

			float time = (float)gameTime.TotalGameTime.TotalSeconds;

			elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

			
			if (elapsedTime >= StepDelay)
			{
				elapsedTime = 0f;

				// ~ need to use dt somewhere to match speed with framerate ! => no need, update is time controlled, so this is not an issue

				// issue: internal progress AND visual progress didn't match at all, see below:
				// steprect rect width and actualprogress increase at different paces !

				// check for progressway 

				// add internal progress
				currentProgress += progress;
				currentProgress = (Byte)MathHelper.Clamp(currentProgress, 0, 100);

				// "convert" int progress into visual progress
				if (currentProgress % StepSize == 0)
				{
					StepRect.Width += stepwidth;
				}


				// progress increase for EACH single update call, maybe TOO FAST
				// also calculte delay between steps
				//Byte actualProgress = (Byte)(progress * stepwidth);  //(Byte)(progress / StepSize);
				//System.Diagnostics.Debug.Print(string.Format("==> aP={0}", actualProgress));

				//tmp, ~ need to fill it * dt (TC)
				//currentProgress += progress; //actualProgress;

				
				// fill the bar to progress count (0-100)
				// upd steprect size accordingly

				// if progress > stepsize: add another step ==> increase steprec width (height for V bar) by barwidth/stepwith (*progress)
				// to do ONLY when step reach wanted resolution !
				// TC: not so good
				//if (currentProgress % StepSize == 0)
				//{
				//	StepRect.Width += stepwidth;
				//	System.Diagnostics.Debug.Print(string.Format("==> srw={0}", StepRect.Width));
				//}

				// +display something "on" bar: resources, progress, etc, text here is more for big steps: rendering x, creating y, ...
				if (text != null)
					Text = text;
			}

			System.Diagnostics.Debug.Print(string.Format("TGTts={0:F3} eT={1:F3} srw={2}", time, elapsedTime, StepRect));
        }


        public Byte GetProgress()
        {
            return currentProgress;
        }


		public void SwapWays()
		{
			// need to check what current progress are: 100% => F, 0% => T, otherwise: do nothing (log message maybe)
			progressWay = !progressWay;
		}


        public void Draw()
        {
			spriteBatch.Draw(StepTexture, StepRect, Color.White);
			// draw core/body of the bar
			//   then progress itself: small sprite increment which will have to be drawn in line: progress=20% = 2 items to display (step = 10%)
			spriteBatch.Draw(BodyTexture, ControlArea, Color.White);
			
			//draw text over
			// spritefont or
			// sprites as font
		}
    }
}
