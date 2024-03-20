using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LunarLander.Lander;
using System;
using System.Collections.Generic;

namespace LunarLander
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Lander.Lander _lander = new Lander.Lander();
        private Texture2D _texture;
        private DateTime _lastUpdate;
        private Terrain _terrain;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _lastUpdate = DateTime.Now;

            _terrain = new Terrain(_graphics);
            _terrain.GenerateTerrain();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _texture = new Texture2D(GraphicsDevice, 40, 30);
            Color[] colorData = new Color[40 * 30];
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = Color.Red;
            }
            _texture.SetData(colorData);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            TimeSpan deltaTime = DateTime.Now - _lastUpdate;

            // Input Parsing
            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            // Lander Update
            _lander.Update(deltaTime, keys);
            base.Update(gameTime);

            // Update _lastUpdate
            _lastUpdate = DateTime.Now;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _lander.Draw(_texture, _spriteBatch);
            _terrain.Draw(_texture, _spriteBatch);
            base.Draw(gameTime);

            _spriteBatch.End();
        }
    }
}