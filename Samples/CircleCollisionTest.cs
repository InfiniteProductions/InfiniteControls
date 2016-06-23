using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using InfiniteControls;

namespace InfiniteControlsSamples
{
	public class CircleCollisionTest : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont osdfont;

		MouseState MouseInput, PreviousMouseInput;
		KeyboardState keyboardState, prevkeyboardState;

		Texture2D circleDebug;

		Circle circle1, circle2, circle3, cirm;
		Rectangle rect1, rect2, rect3, rectm;


		public CircleCollisionTest()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
		}

		protected override void Initialize()
		{
			graphics.ApplyChanges();

			IsMouseVisible = true;


			circle1 = new Circle(20, new Vector2(300, 300));
			circle2 = new Circle(15, new Vector2(290, 300));
			circle3 = new Circle(32, new Vector2(500, 500));

			cirm = new Circle(3, Vector2.Zero);


			rect1 = new Rectangle(300, 300, 40, 40);
			rect2 = new Rectangle(290, 300, 30, 30);
			rect3 = new Rectangle(500, 500, 64, 64);

			rectm = new Rectangle();

			base.Initialize();
		}


		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			osdfont = Content.Load<SpriteFont>("osd");

			circleDebug = Content.Load<Texture2D>("misc/debugcircle");
		}


		protected override void UnloadContent()
		{
		}


		protected override void Update(GameTime gameTime)
		{
			PreviousMouseInput = MouseInput;

			MouseInput = Mouse.GetState();
			keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

			if (keyboardState.IsKeyDown(Keys.Up))
			{
				circle2.Centre.Y -= 2;
				rect2.Y -= 2;
			}
			if (keyboardState.IsKeyDown(Keys.Down))
			{
				circle2.Centre.Y += 2;
				rect2.Y += 2;
			}
			if (keyboardState.IsKeyDown(Keys.Left))
			{
				circle2.Centre.X -= 2;
				rect2.X -= 2;
			}
			if (keyboardState.IsKeyDown(Keys.Right))
			{
				circle2.Centre.X += 2;
				rect2.X += 2;
			}

			prevkeyboardState = keyboardState;
			PreviousMouseInput = MouseInput;

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// not a very good thing to do here, but it's just a mere sample
			string text = string.Format("c1/c2 int={0} c2/c1 int={1} c3/c2 int={2} c2/c3 int={3} mouse={4}", circle1.Intersects(circle2), circle2.Intersects(circle1), circle3.Intersects(circle2), circle2.Intersects(circle3), 0);

			//c2 c3 intersects even when they're not ! TC
			spriteBatch.Begin();

			spriteBatch.DrawString(osdfont, text, new Vector2(10, 10), Color.White);

			spriteBatch.Draw(circleDebug, rect1, Color.White);
			spriteBatch.Draw(circleDebug, rect2, Color.White);
			spriteBatch.Draw(circleDebug, rect3, Color.White);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}