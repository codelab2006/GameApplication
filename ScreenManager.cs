using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace GameApplication
{
    public class ScreenManager(Game game, Dictionary<string, Screen> screens)
    {
        private readonly Game _game = game;
        private readonly Dictionary<string, Screen> _screens = screens;
        private Screen? _screen;

        private void SetScreen(Screen? screen)
        {
            if (_screen is not null)
                _game.Components.Remove(_screen);
            _screen?.Exit();
            _screen = screen;
            _screen?.Enter();
            if (_screen is not null)
                _game.Components.Add(_screen);
        }

        public void GoTo(string screenName)
        {
            if (_screen is null || screenName != _screen.GetType().Name)
                SetScreen(_screens[screenName]);
        }
    }
}