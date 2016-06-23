using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GenericMenu;

// new menu class test "game"
namespace InfiniteControls
{
	public class mtest : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		SpriteFont menuFont;
		Rectangle mainFrame;
		Texture2D menuBackground;

		static string[] mainmenu_items = new string[] { "New game", "Players", "Options", "About", "Quit" };
		Actions[] mainmenu_actions = new Actions[] { GenericMenu.Actions.Action, GenericMenu.Actions.Menu, GenericMenu.Actions.Menu, GenericMenu.Actions.Page, GenericMenu.Actions.Action };

		static string[] playersmenu_items = new string[] { "New Profile", "Use Profile", "Delete profile", "Back" };
		static string[] settingsmenu_items = new string[] { "Video", "Full screen", "Cursor", "Back" };
		static string[] about_items = new string[] { "Back" };

		GenericMenu.Menu main_menu;
		GenericMenu.Menu settings_menu;
		GenericMenu.Menu about_menu;


		public mtest()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}


		protected override void Initialize()
		{
			graphics.PreferredBackBufferHeight = 768;
			graphics.PreferredBackBufferWidth = 1024;
			graphics.ApplyChanges();

			this.IsMouseVisible = true;

			mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			//main_menu = new Menu(mainmenu_items);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			menuBackground = Content.Load<Texture2D>("backgrounds\\nat_menu");
			menuFont = Content.Load<SpriteFont>("menuFont");

			Texture2D popup_test = Content.Load<Texture2D>("backgrounds\\popup");

			//main_menu.background = menuBackground;
			//main_menu.MenuSetBackground(menuBackground);
			main_menu = new Menu(mainmenu_items, menuBackground);
			main_menu.mouseEnabled = false;
			main_menu.keyboardEnabled = true;

			//ISSUE: adding new instance => index out of range EXE in draw !!!
			// WTF !? ==> static item_click in class, the 2nd one for about overwrite prev one for main menu !!!
			// about menu is a new instance... but static property seems to mess things up badely
			//settings_menu = new Menu(settingsmenu_items);
			about_menu = new Menu(about_items);


			//tmp test
			//main_menu.MenuSetWinBackground(popup_test);
		}


		protected override void UnloadContent()
		{

		}


		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			main_menu.Update();
			if (false) //main_menu.exit == true)
			{
				this.Exit();
			}

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			main_menu.Draw(graphics, spriteBatch, menuFont, mainFrame);
			base.Draw(gameTime);
		}
	}
}