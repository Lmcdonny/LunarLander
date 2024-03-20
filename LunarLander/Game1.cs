using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LunarLander.Lander;
using System;
using System.Collections.Generic;
using LunarLander.Menus;

namespace LunarLander
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Lander.Lander _lander;
        private Texture2D _texture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            MainMenuScene main = new MainMenuScene();
            main.Run();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
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