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
        private Lander.Lander _lander;
        private Texture2D _texture;
        private DateTime _lastUpdate;
        private Terrain _terrain;
        private HeadsUpDisplay _hud;
        private SpriteFont _font;

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
            _lander = new Lander.Lander();
            _hud = new HeadsUpDisplay();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _lander.LoadContent(this.Content.Load<Texture2D>("lander"));
            _hud.LoadContent(this.Content.Load<SpriteFont>("DefaultFont"));
            _texture = new Texture2D(GraphicsDevice, 40, 30);

            Color[] colorData = new Color[40 * 30];
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = Color.White;
            }
            _texture.SetData(colorData);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Utility.DetectWin(_lander.GetRectangle(), _terrain.GetLandingZone()))
            {
                _hud.GameWin();
            }
            else if (Utility.DetectCollision(_lander.GetRectangle(), _terrain.GetVerts()))
            {
                Initialize();
            }
            TimeSpan deltaTime = DateTime.Now - _lastUpdate;

            // Input Parsing
            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            // Lander Update
            _lander.Update(deltaTime, keys);
            // HUD Update
            _hud.Update(deltaTime, _lander);

            base.Update(gameTime);

            // Update _lastUpdate
            _lastUpdate = DateTime.Now;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _lander.Draw(_spriteBatch);
            _terrain.Draw(_texture, _spriteBatch);
            _hud.Draw(_texture, _spriteBatch);
            base.Draw(gameTime);

            _spriteBatch.End();
        }
    }

    class Utility
    {
        public static bool DetectCollision(Rectangle lander, List<Vector3> ground)
        {
            for (int i = 0; i < ground.Count - 1; i++)
            {
                // Create a line segment from the two points
                LineSegment line = new LineSegment(ground[i], ground[i + 1]);

                // Check for intersection between the line segment and the rectangle
                if (lander.Intersects(line.ToRectangle())) return true;
            }
            return false;
        }

        public static bool DetectWin(Rectangle lander, Rectangle landingZone)
        {
            if (lander.X >= landingZone.X && lander.X + lander.Width < landingZone.X + landingZone.Width)
            {
                if (lander.Y + lander.Height >= landingZone.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }

    // Custom LineSegment class to represent a line segment
    class LineSegment
    {
        public Vector3 Start { get; }
        public Vector3 End { get; }

        public LineSegment(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        // Convert the line segment to a rectangle for intersection check
        public Rectangle ToRectangle()
        {
            int x = (int)Math.Min(Start.X, End.X);
            int y = (int)Math.Min(Start.Y, End.Y);
            int width = (int)Math.Abs(Start.X - End.X);
            int height = (int)Math.Abs(Start.Y - End.Y);

            return new Rectangle(x, y, width, height);
        }
    }
}