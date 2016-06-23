//#REGION ABOUT
// Generic menu class for your game
//
// mouse item selection comes from http://www.spikie.be/blog/page/Building-a-main-menu-and-loading-screens-in-XNA.aspx
// Thanks to Nico for the tips
//
// 26/05/2015
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GenericMenu
{
    public enum justify
    {
        none = 0,
        vcenter = 1,
        top = 2,
        bottom = 3
    };

    public enum Actions
    {
        Menu = 0,
        Action = 1,
        Page = 2
    };


    public class Menu
    {
        public bool mouseEnabled { get; set; }
        public bool keyboardEnabled { get; set; }
        public justify justify { get; set; }

        public string[] items { get; set; }
        public UInt16[] actions { get; set; }
        public UInt16 selected_item { get; set; }
        //public UInt16 chosen_item { get; set; }

        public bool askForExit { get; set; }

        public Texture2D background { get; set; }
        public Texture2D additionnal_background { get; set; }

        public Color textColor { get; set; }
        public Color highlightColor { get; set; }

        private static KeyboardState prevKeyState;
        private static KeyboardState keyState;
        private static MouseState prevMouseState;
        private static MouseState mouseState;

        private Rectangle[] items_click;
        private UInt16 item_picked;


        // ~add selectable/unselectable items

        public Menu(string[] _items, Texture2D _background = null, Texture2D _add_background = null) //actions = link from items to others, ~add default selected item
        {
            mouseEnabled = true;
            keyboardEnabled = false;

            items = _items;
            selected_item = 0;
            background = _background;
            additionnal_background = _add_background;
            askForExit = false;

            items_click = new Rectangle[_items.Length];
        }


        public void MenuSetBackground(Texture2D newBackground)
        {
            if (newBackground != null)
            {
                background = newBackground;
            }
        }


        public void MenuSetWinBackground(Texture2D AdditionalBackground)
        {
            if (AdditionalBackground != null)
            {
                Debug.Print("Add background set");
                additionnal_background = AdditionalBackground;
            }
        }


        public UInt16 HandleInput()
        {
            UInt16 item_picked = 0;
            UInt16 item_number = 0;

            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyboardEnabled == false && mouseEnabled == false)
            {
                //WARNING: no input enabled!
                // Raise exception
                Debug.Print("No input enabled, menu can't work !");
            }

            // key input, no mouse move: pointer disabled/discared

            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                Rectangle mouseClickRect = new Rectangle(mouseState.X, mouseState.Y, 10, 10);

                Debug.Print("mouse click, get location");

                for (item_number = 0; item_number < items.Length; item_number++)
                {
                    Debug.Print(string.Format("check item [{0}]", item_number));

                    if (mouseClickRect.Intersects(items_click[item_number]))
                    {
                        Debug.Print(string.Format("item {0} clicked", items[item_number]));
                        item_picked = item_number;
                    }
                }

            }

            if (keyboardEnabled == true)
            {
                if (keyState.IsKeyDown(Keys.Down) && prevKeyState.IsKeyUp(Keys.Down))
                {
                    Debug.Print(string.Format("D/ Selected item[{0}/{1}]", selected_item, items.Length));
                    selected_item++;
                    if (selected_item >= items.Length - 1)
                    {
                        selected_item = (UInt16)(items.Length - 1);
                    }
                    Debug.Print(string.Format("D/b Selected item[{0}/{1}]", selected_item, items.Length));
                }

                if (keyState.IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up))
                {
                    Debug.Print(string.Format("U/ Selected item[{0}/{1}]", selected_item, items.Length));
                    if (selected_item > 0)
                    {
                        selected_item--;
                    }

                    Debug.Print(string.Format("U/b Selected item[{0}/{1}]", selected_item, items.Length));
                }

                if (keyState.IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter))
                {
                    item_picked = selected_item;
                    Debug.Print(string.Format("Selected item[{0}/{1}] = {2}", selected_item, items.Length, items[selected_item]));
                }
                else if (keyState.IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space))
                {
                    Debug.Print("Selected item = " + items[selected_item]);
                }

                // WARNING: ONLY if the menu have actually an EXIT item !!
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) || item_picked == items.Length - 1)
                {
                    // if menu have exit: it's exit
                    // else back
                    // eventually exit_action: exit or back

                    Debug.Print("Exit selected");
                    askForExit = true;

                    //item_picked = UInt16.MaxValue;
                    // ~do it later, show a confirm popup window + do some cleaning
                    //exit = true;
                }

            }
            else
            {
                Debug.Print("keyboard disabled");
            }

            prevKeyState = keyState;
            prevMouseState = mouseState;

            return item_picked;
        }


        public void Update()
        {
            item_picked = this.HandleInput();

            // handle actions
            if (askForExit)  //item_picked == UInt16.MaxValue)
            {
                Debug.Print("Exiting...");
                //todo
            }
            else if (item_picked == 0)
            {
            }
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont menuFont, Rectangle menu_area)
        {
            Rectangle mousePointer = new Rectangle(mouseState.X, mouseState.Y, 10, 10);
            bool mouse_highlight = false;

            //tmp debug
            Texture2D debugtex = new Texture2D(graphics.GraphicsDevice, 1, 1);
            debugtex.SetData(new Color[] { Color.Blue });


            UInt16 item_number = 0;

            // number of pixels between each menu item lines
            UInt16 menu_spacing = 20;

            // size of choosen font/size
            UInt16 menu_item_height = (UInt16)menuFont.MeasureString(items[0]).Y;

            //if (this.justify == justify.vcenter)
            Int32 MenuStartHeightUI = (graphics.GraphicsDevice.Viewport.Height - (items.Length * (menu_item_height + menu_spacing - 1))) / 2;
            Vector2 MenuItemHeight = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, (float)MenuStartHeightUI);


            // clear background/viewport
            //GraphicsDevice.Clear(Color.Black);

            // only deferred/immediate works for pop-up background, back2front don't
            // or... draw bacground at the end
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque);

            if (additionnal_background != null)
            {
                //Debug.Print("add background drawing...");

                Rectangle background_area = new Rectangle((int)(graphics.GraphicsDevice.Viewport.Width - additionnal_background.Width) / 2,
                                                          (int)(graphics.GraphicsDevice.Viewport.Height - additionnal_background.Height) / 2,
                                                          additionnal_background.Width, additionnal_background.Height);

                //draw pop-up window background
                spriteBatch.Draw(additionnal_background, background_area, Color.White);

            }

            if (background != null)
            {
                spriteBatch.Draw(background, menu_area, Color.White);
            }

            spriteBatch.End();

            // draw menu items

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            Vector2 item_coordinates = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, (float)MenuStartHeightUI);
            Vector2 item_horiz_position = new Vector2(menuFont.MeasureString(items[0]).X / 2, 0.0f);

            foreach (string menu_item in items)
            {
                item_horiz_position.X = menuFont.MeasureString(menu_item).X / 2;

                Debug.Print(string.Format("in={0} it={1}", item_number, menu_item));
                // mouse click support = create rectangle at the same coord, same size as texts
                items_click[item_number] = new Rectangle((int)(item_coordinates.X - menuFont.MeasureString(items[item_number]).X / 2), (int)item_coordinates.Y, (int)menuFont.MeasureString(items[item_number]).X, (int)menuFont.MeasureString(items[item_number]).Y);

                //tmp debug
                //spriteBatch.Draw(debugtex, items_click[item_number], Color.White);

                // + get mouse cursor coord: highlight as item selected
                // see handle input above !

                // if keyboard use: "disable" mouse highlight
                if (mousePointer.Intersects(items_click[item_number]))
                {
                    mouse_highlight = true;
                    selected_item = 10;
                }


                //outline, choose color below (change all four !)
                // both are mutually exclusive ! if mouse highlight => keybaord sel item cancelled, and vice versa
                // no mouse move + key = discard/dont care about mouse highlight
                if (mouse_highlight == true || (mouse_highlight == false && selected_item == Array.IndexOf(items, menu_item)))
                {
                    // 2 = outline offset
                    spriteBatch.DrawString(menuFont, menu_item, item_coordinates, Color.White, 0, new Vector2(item_horiz_position.X - 2, item_horiz_position.Y), 1.0f, SpriteEffects.None, 0.6f);
                    spriteBatch.DrawString(menuFont, menu_item, item_coordinates, Color.White, 0, new Vector2(item_horiz_position.X + 2, item_horiz_position.Y), 1.0f, SpriteEffects.None, 0.6f);
                    spriteBatch.DrawString(menuFont, menu_item, item_coordinates, Color.White, 0, new Vector2(item_horiz_position.X, item_horiz_position.Y - 2), 1.0f, SpriteEffects.None, 0.6f);
                    spriteBatch.DrawString(menuFont, menu_item, item_coordinates, Color.White, 0, new Vector2(item_horiz_position.X, item_horiz_position.Y + 2), 1.0f, SpriteEffects.None, 0.6f);
                }

                // here color have to be set well with background used !          White
                spriteBatch.DrawString(menuFont, menu_item, item_coordinates, Color.Black, 0, item_horiz_position, 1.0f, SpriteEffects.None, 0.5f);

                item_coordinates.Y += (float)(menu_item_height + menu_spacing);

                item_number++;
                mouse_highlight = false;
            }

            spriteBatch.End();
        }
    }
}