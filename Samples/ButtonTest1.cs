using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using InfiniteControls;

namespace InfiniteControlsSamples
{
	public class ButtonsTest1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont osdfont;

		public MouseState MouseInput;
		public MouseState PreviousMouseInput;

		Texture2D background;

		SimpleButton testButton1, circleButton1;
		Button cooldown1;
		Checkbox cbtest1;
		VolumeSelector vstest1;
		ProgressBar pbar1;

		TButton tbtest;
		bool varButton1 = false;
		bool varCB1 = false;


		public ButtonsTest1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
		}


		protected override void Initialize()
		{
			graphics.ApplyChanges();

			
			testButton1 = new SimpleButton(new Vector2(100, 80), new Vector2(64, 64));

			cooldown1 = new Button(new Vector2(100, 180), new Vector2(64, 64), null, null, 0.4f, 5f);
			//cooldown1.Enabled
			cbtest1 = new Checkbox(new Vector2(180, 80), new Vector2(64, 64));
			//cbtest1.Enabled = false;

			//circle button: 64x64 base rec
			// 445,232 (TL corner), R=32  centre=477,264   //445, 232
			circleButton1 = new SimpleButton(new Vector2(477, 264), new Vector2(64, 64), 0.8f, CollisionType.Circle);

			//								 dec button, inc button, body, cursor (width), gap and height
			// body size = 11 * cursorsize
			UInt16[] vsSizes = new UInt16[6] { 20, 20, 110, 10, 1, 64 };
			vstest1 = new VolumeSelector(new Vector2(300, 80), vsSizes);

			tbtest = new TButton(new Vector2(180, 180), new Vector2(64, 64), null, null, 0.2f, 8);

			//pbar1 = new ProgressBar(new Vector2(300, 180), new UInt16[5] { 110, 2, 5, 100, 64 }, false, 0, 5);

			IsMouseVisible = true;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			osdfont = Content.Load<SpriteFont>("osd");

			background = Content.Load<Texture2D>("backgrounds/test1-background");

			Texture2D buttonON = Content.Load<Texture2D>("buttons/ON");
			Texture2D buttonOFF = Content.Load<Texture2D>("buttons/OFF");
			Texture2D cbChecked = Content.Load<Texture2D>("buttons/CBc");
			Texture2D cbUnchecked = Content.Load<Texture2D>("buttons/CBu");

			Texture2D CircleButtonOff = Content.Load<Texture2D>("buttons/CirBu");
			Texture2D CircleButtonOn = Content.Load<Texture2D>("buttons/CirBu-Pressed");

			testButton1.SetTextures(buttonON, buttonOFF);
			testButton1.spriteBatch = spriteBatch;

			tbtest.SetTextures(buttonON, buttonOFF);
			tbtest.spriteBatch = spriteBatch;

			cooldown1.SetTextures(buttonON, buttonOFF);
			cooldown1.spriteBatch = spriteBatch;

			cbtest1.SetTextures(cbChecked, cbUnchecked);
			cbtest1.spriteBatch = spriteBatch;


			circleButton1.SetTextures(CircleButtonOn, CircleButtonOff);
			circleButton1.spriteBatch = spriteBatch;


			Texture2D vsbody = Content.Load<Texture2D>("buttons/vsBody");
			Texture2D vscursor = Content.Load<Texture2D>("buttons/vsCursor");
			Texture2D vsdecr = Content.Load<Texture2D>("buttons/vsDEC");
			Texture2D vsincr = Content.Load<Texture2D>("buttons/vsINC");
			Texture2D vsdecp = Content.Load<Texture2D>("buttons/vsDEC");
			Texture2D vsincp = Content.Load<Texture2D>("buttons/vsINC");

			vstest1.spriteBatch = spriteBatch;
			vstest1.SetTextures(vsbody, vscursor, vsdecr, vsdecp, vsincr, vsincp);

			//radio buttons
			// 830, 140
			// 830, 224
			// 830, 300


		}


		protected override void UnloadContent()
		{
			Content.Unload();
			GC.Collect(0);
		}


		protected override void Update(GameTime gameTime)
		{
			PreviousMouseInput = MouseInput;
			MouseInput = Mouse.GetState();

			bool clicked = false;

			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

			if (MouseInput.LeftButton == ButtonState.Pressed && PreviousMouseInput.LeftButton == ButtonState.Released)
			{
				clicked = true;
				//Vector2 clickLocation = new Vector2(MouseInput.X, MouseInput.Y);
			}

			testButton1.Update(gameTime, clicked);
			cooldown1.Update(gameTime, clicked);
			tbtest.Update(gameTime, clicked);
			circleButton1.Update(gameTime, clicked);

			cbtest1.Update(gameTime, clicked);
			vstest1.Update(gameTime, clicked);

			varButton1 = testButton1.Pressed();
			varCB1 = cbtest1.Checked();

			//pbar1.Update(gameTime, 5);
			//System.Diagnostics.Debug.Print(string.Format("PBar prog={0}", pbar1.GetProgress()));

			PreviousMouseInput = MouseInput;

			base.Update(gameTime);
		}


		// VS cursor not drawn => SB sortmode/alphablend
		// circle button not drawn ! => butonarea (drawrect) 0000 !

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			string text = string.Format("Button1={0} CB1={1} Vol={2} cooldowntime={3} SDclicked={4}", varButton1, varCB1, vstest1.currentVolume, cooldown1.CooldownTime, circleButton1.Pressed());

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend); // SpriteSortMode.BackToFront, BlendState.AlphaBlend);

			spriteBatch.Draw(background, Vector2.Zero, Color.White);

			spriteBatch.DrawString(osdfont, text, new Vector2(10, 10), Color.White);

			testButton1.Draw();
			cooldown1.Draw();
			tbtest.Draw();

			circleButton1.Draw();

			cbtest1.Draw();

			vstest1.Draw();

			//pbar1.Draw();

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}