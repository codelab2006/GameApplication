using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameApplication
{
    public class ViewportAdapter
    {
        private readonly GameWindow _window;
        private readonly GraphicsDevice _graphicsDevice;
        public int VirtualWidth { get; }
        public int VirtualHeight { get; }

        public Viewport Viewport => _graphicsDevice.Viewport;

        public ViewportAdapter(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
        {
            _window = window;
            _graphicsDevice = graphicsDevice;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;

            _window.ClientSizeChanged += OnClientSizeChanged;
        }

        private void OnClientSizeChanged(object sender, EventArgs eventArgs)
        {
            var clientBounds = _window.ClientBounds;

            var worldScale = MathHelper.Min((float)clientBounds.Width / VirtualWidth, (float)clientBounds.Height / VirtualHeight);
            var width = (int)(worldScale * VirtualWidth);
            var height = (int)(worldScale * VirtualHeight);

            _graphicsDevice.Viewport = new Viewport(clientBounds.Width / 2 - width / 2, clientBounds.Height / 2 - height / 2, width, height);
        }

        public Point PointToScreen(Point point)
        {
            return PointToScreen(point.X, point.Y);
        }

        public Point PointToScreen(int x, int y)
        {
            var viewport = _graphicsDevice.Viewport;

            var scaleMatrix = GetScaleMatrix();
            var invertedMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(new Vector2(x - viewport.X, y - viewport.Y), invertedMatrix).ToPoint();
        }

        public Matrix GetScaleMatrix()
        {
            return Matrix.CreateScale((float)Viewport.Width / VirtualWidth, (float)Viewport.Height / VirtualHeight, 1.0f);
        }
    }

}