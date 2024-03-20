using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace LunarLander.Lander
{
	internal class HeadsUpDisplay
	{
		private Rectangle SafeLandingIndicator;
		private bool Safe = false;
		private const int MAX_LANDING_SPEED = 2;
		private const float MAX_LANDING_ORIENTATION = (float)(Math.PI / 36); // 5 degree margin of error
		private Color WarningLight = Color.Red;
		private SpriteFont m_font;
		private float m_win = 0;

		public HeadsUpDisplay()
		{
			int width = GraphicsDeviceManager.DefaultBackBufferWidth;
			SafeLandingIndicator = new Rectangle(width - 60, 30, 30, 30);
		}

		public void LoadContent(SpriteFont font)
		{
			m_font = font;
		}

		private bool IsSafe(Lander lander)
		{
			if (!(lander.GetOrientation() < MAX_LANDING_ORIENTATION ||
				lander.GetOrientation() > (2 * Math.PI) - MAX_LANDING_ORIENTATION)) return false;
			if (lander.GetVelocity().Length() > MAX_LANDING_SPEED) return false;

			return true;
		}
		public void GameWin()
		{
			m_win = 5f;
		}
		private void UpdateIndicators(Lander lander)
		{
			Safe = IsSafe(lander);
            WarningLight = Safe == true ?  Color.Green : Color.Red;
		}

		public void Update(TimeSpan deltaTime, Lander lander)
		{
			if (m_win > 0)
			{
				m_win -= (float)deltaTime.TotalSeconds;
			}
			UpdateIndicators(lander);
		}

		public void Draw(Texture2D texture, SpriteBatch sb)
		{
			sb.Draw(texture, SafeLandingIndicator, WarningLight);
            if (m_win > 0)
            {
                sb.DrawString(
					m_font,
					"Congrats!",
					new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth - 350, 40),
					Color.Black);
            }
        }
	}
}

