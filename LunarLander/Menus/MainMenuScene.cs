using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunarLander.Lander;

namespace LunarLander.Menus
{
    internal class MainMenuScene: Game
    {
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;
        private bool m_inGame;
        public MainMenuScene()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Input Parsing
            Keys[] keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Contains(Keys.Enter))
            {
                GameScene game = new GameScene();
                m_inGame = true;
                game.Run();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            m_spriteBatch.Begin();
            base.Draw(gameTime);

            m_spriteBatch.End();
        }
    }
}
