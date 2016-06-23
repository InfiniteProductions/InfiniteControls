using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// ~ another kind of button, but state didn't auto get back to released

namespace InfiniteControls
{
    public class Checkbox : SimpleButton
    {
        public enum checkState : byte { Checked, Unchecked };

        //private Rectangle ButtonArea;
        //private Texture2D currentTexture;
        private checkState check;

        public Checkbox(Vector2 position, Vector2 size, Texture2D textureUnchecked = null, Texture2D textureChecked = null)
            : base(position, size, 0.3f, CollisionType.Rectangle, textureUnchecked, textureChecked)
        {
            ButtonArea = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)size.Y);
            currentTexture = textureUnchecked;

            check = checkState.Unchecked;
            
        }


        public override void Update(GameTime gameTime, bool click)
        {
			//quite bad !!
			currentTexture = ReleasedTexture;

			if (Enabled == false)
				return;

			Rectangle mouseclick = new Rectangle(Mouse.GetState().X - 2, Mouse.GetState().Y - 2, 4, 4);

            //System.Diagnostics.Debug.Print(string.Format("CB update called, ct={0} check={1} ", currentTexture, check));

            if (click == true && mouseclick.Intersects(ButtonArea))
            {
                //System.Diagnostics.Debug.Print("clicked");

                if (check == checkState.Unchecked)
                {
                    check = checkState.Checked;
                    //System.Diagnostics.Debug.Print(string.Format("check={0}", check));
                }
                else if (check == checkState.Checked)
                {
                    check = checkState.Unchecked;
                    //System.Diagnostics.Debug.Print(string.Format("check={0}", check));
                }
            }

            if (check == checkState.Unchecked)
            {
                currentTexture = ReleasedTexture;
            }
            else
            {
                currentTexture = PressedTexture;
            }

            
        }


        public override void Draw()
        {
            spriteBatch.Draw(currentTexture, ButtonArea, Color.White);
            //base.Draw();
        }


        public bool Checked()
        {
            return check == checkState.Checked;
        }


        public bool Unchecked()
        {
            return check == checkState.Unchecked;
        }
    }
}
