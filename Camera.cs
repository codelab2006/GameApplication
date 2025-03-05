using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class Camera(ViewportAdapter viewportAdapter)
    {
        private readonly ViewportAdapter _viewportAdapter = viewportAdapter;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; } = 0;
        private float _zoom = 1;
        public float Zoom
        {
            get => _zoom; set => _zoom = value > 0 ? value : 0;
        }
        private readonly Vector2 _origin = new(viewportAdapter.VirtualWidth / 2, viewportAdapter.VirtualHeight / 2);

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0.0f))
            * Matrix.CreateTranslation(new Vector3(-_origin, 0.0f))
            * Matrix.CreateRotationZ(Rotation)
            * Matrix.CreateScale(Zoom, Zoom, 1)
            * Matrix.CreateTranslation(new Vector3(_origin, 0.0f))
            * _viewportAdapter.GetScaleMatrix();
        }

        public void Move(Vector2 direction)
        {
            Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-Rotation));
        }

        public void Rotate(float deltaRadians)
        {
            Rotation += deltaRadians;
        }

        public void ZoomIn(float deltaZoom)
        {
            Zoom += deltaZoom;
        }

        public void ZoomOut(float deltaZoom)
        {
            Zoom -= deltaZoom;
        }

        public void LookAt(Vector2 position)
        {
            Position = position - new Vector2(_viewportAdapter.VirtualWidth / 2, _viewportAdapter.VirtualHeight / 2);
        }

        public Vector2 WorldToScreen(float x, float y)
        {
            return WorldToScreen(new Vector2(x, y));
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            var viewport = _viewportAdapter.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        public Vector2 ScreenToWorld(float x, float y)
        {
            return ScreenToWorld(new Vector2(x, y));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            var viewport = _viewportAdapter.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }
    }
}