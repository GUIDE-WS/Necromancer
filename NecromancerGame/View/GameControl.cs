using System;
using System.Drawing;
using System.Windows.Forms;
using NecromancerGame.Model;

namespace NecromancerGame.View
{
    public sealed partial class GameControl : UserControl
    {
        private readonly MyForm _form;
        private readonly Game _currentGame;
        private readonly Control _pauseMenu;
        private PlayerBar _playerBar;

        public GameControl(MyForm form, Game game)
        {
            _currentGame = game;
            _form = form;

            ClientSize = form.Size;
            DoubleBuffered = true;
            BackgroundImage = Resources.GameBackground;
            BackgroundImageLayout = ImageLayout.Stretch;

            _pauseMenu = InitializePause();
            _pauseMenu.Hide();
            Controls.Add(_pauseMenu);

            Controls.Add(InitialiseMap());
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _playerBar = InitialisePlayerBar();
            _playerBar.Enabled = false;
            Controls.Add(_playerBar);
        }

        private void Pause()
        {
            Cursor.Show();
            _pauseMenu.Enabled = true;
            _pauseMenu.Show();
            _pauseMenu.Focus();
        }

        private void Resume()
        {
            Cursor.Hide();
            _pauseMenu.Enabled = false;
            _pauseMenu.Hide();
            Focus();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Pause();
                    return;
                case Keys.W:
                    Controller.Controller.SetPosition(_currentGame, Direction.Up);
                    break;
                case Keys.S:
                    Controller.Controller.SetPosition(_currentGame, Direction.Down);
                    break;
                case Keys.A:
                    Controller.Controller.SetPosition(_currentGame, Direction.Left);
                    break;
                case Keys.D:
                    Controller.Controller.SetPosition(_currentGame, Direction.Right);
                    break;
                case Keys.Space:
                    Controller.Controller.SetPosition(_currentGame, _currentGame.CurrentPlayer.CurrentDirection);
                    Controller.Controller.SetPosition(_currentGame, _currentGame.CurrentPlayer.CurrentDirection);
                    break;
                case Keys.E:
                    Controller.Controller.Battle(_currentGame.CurrentPlayer, _currentGame.CurrentLocation);
                    break;
                case Keys.F:
                    Controller.Controller.Interaction(_currentGame);
                    break;
                case Keys.R:
                    Controller.Controller.SetPlayer(_currentGame);
                    break;
            }

            UpdateGame();
        }

        private void UpdateGame()
        {
            _currentGame.Update();
            _playerBar.SetValue(_currentGame.CurrentLocation.CurrentNecromancer.Health);
            Invalidate();
        }


        private TableLayoutPanel InitializePause()
        {
            var tableSize = new Size(ClientSize.Width / 4, ClientSize.Height / 3);
            var tableLocation = new Point(ClientSize.Width / 2 - tableSize.Width / 2,
                ClientSize.Height / 2 - tableSize.Height / 2);

            var pauseTable = new TableLayoutPanel
            {
                Location = tableLocation,
                Size = tableSize,
                Enabled = false,
                BackColor = Color.DarkSlateGray
            };
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            pauseTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pauseTable.Controls.Add(MenuControl.MenuBatton(@"Продолжить", pauseTable.Size.Height / 15, Resume), 0,
                0);
            pauseTable.Controls.Add(
                MenuControl.MenuBatton(@"В главное меню", pauseTable.Size.Height / 15, () => _form.ToMainMenu(this)),
                0, 1);
            pauseTable.Controls.Add(
                MenuControl.MenuBatton(@"Выйти на рабочий стол", pauseTable.Size.Height / 15, Application.Exit),
                0, 2);
            pauseTable.KeyDown += (_, args) =>
            {
                if (args.KeyCode == Keys.Escape)
                    Resume();
            };
            return pauseTable;
        }

        private ScaledViewPanel InitialiseMap()
        {
            var painter = new ScenePainter(_form);
            var mapSize = new Size(_form.Size.Width / 5 * 4, _form.Size.Height / 5 * 4);
            var mapLocation = new Point(_form.Size.Width/2 - mapSize.Width/2, _form.Size.Height/2 - mapSize.Height/2);

            var map = new ScaledViewPanel(painter)
            {
                Location = mapLocation,
                Size = mapSize,
                BackColor = Color.Transparent,
            };

            return map;
        }

        private PlayerBar InitialisePlayerBar()
        {
            var barLocation = new Point(_form.Size.Width / 150, _form.Size.Height / 15);
            var barSize = new Size(_form.Size.Width / 5, _form.Size.Height / 100);
            var maxHealth = _currentGame.CurrentLocation.CurrentNecromancer.MaxHealth;
            var health = _currentGame.CurrentLocation.CurrentNecromancer.Health;

            var bar = new PlayerBar(health, maxHealth, 0)
            {
                Location = barLocation,
                Size = barSize,
                BaseColor = Color.Red,
                ProgressColor = Color.Green,
                BackColor = Color.Transparent
            };

            return bar;
        }
    }
}