using LunarLander.Lander;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarLander.Menus
{
    internal class GameScene: Game
    {
        private SpriteBatch m_spriteBatch;
        private GraphicsDeviceManager m_graphics;
        private DateTime m_lastUpdate;
        private Terrain m_terrain;
        private Lander.Lander m_lander;
        private HeadsUpDisplay m_hud;
        private Texture2D m_defaultTexture;
        public GameScene() 
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            m_lastUpdate = DateTime.Now;
            m_terrain = new Terrain(m_graphics);
            m_terrain.GenerateTerrain();
            m_lander = new Lander.Lander();
            m_hud = new HeadsUpDisplay();

            base.Initialize();
        }
        private void LoadDefaultTexture()
        {
            m_defaultTexture = new Texture2D(GraphicsDevice, 1, 1);

            Color[] colorData = new Color[1];
            colorData[0] = Color.White;

            m_defaultTexture.SetData(colorData);
        }
        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            m_lander.LoadContent(this.Content.Load<Texture2D>("lander"));
            m_hud.LoadContent(this.Content.Load<SpriteFont>("DefaultFont"));
            LoadDefaultTexture();
        }

        protected override void Update(GameTime gameTime)
        {
            TimeSpan deltaTime = DateTime.Now - m_lastUpdate;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Utility.DetectWin(m_lander.GetRectangle(), m_terrain.GetLandingZone()))
            {
                m_hud.GameWin();
            }
            else if (Utility.DetectCollision(m_lander.GetRectangle(), m_terrain.GetVerts()))
            {
                Initialize();
            }

            // Input Parsing
            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            // Lander Update
            m_lander.Update(deltaTime, keys);
            // HUD Update
            m_hud.Update(deltaTime, m_lander);

            base.Update(gameTime);

            // Update _lastUpdate
            m_lastUpdate = DateTime.Now;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            m_spriteBatch.Begin();
            m_lander.Draw(m_spriteBatch);
            m_terrain.Draw(m_defaultTexture, m_spriteBatch);
            m_hud.Draw(m_defaultTexture, m_spriteBatch);
            base.Draw(gameTime);

            m_spriteBatch.End();
        }
    }
}
