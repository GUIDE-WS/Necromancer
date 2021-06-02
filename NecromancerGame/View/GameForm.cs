using System.Windows.Forms;
using NecromancerGame.Model;

namespace NecromancerGame.View
{
    public partial class GameForm : Form
    {
        private readonly GameControl _gameControl;
        private readonly MenuControl _menu;
        public readonly Game CurrentGame;
        public GameForm(Game currentGame)
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.UserPaint, true);
            
            CurrentGame = currentGame;
            Name = "Necromancer";
            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;


            SuspendLayout();
            _menu = new MenuControl(this) {Enabled = true};
            _menu.Show();
            _menu.Focus();
            
            _gameControl = new GameControl(this, CurrentGame) {Enabled = false};
            _gameControl.Hide();
            
            Controls.Add(_menu);
            Controls.Add(_gameControl);
            ResumeLayout(false);
        }

        public void StartGame(UserControl from)
        {
            Cursor.Hide();
            from.Enabled = false;
            from.Hide();

            _gameControl.Enabled = true;
            _gameControl.Show();
            _gameControl.Focus();
        }

        public void ToMainMenu(UserControl from)
        {
            from.Enabled = false;
            from.Hide();

            _menu.Enabled = true;
            _menu.Show();
            _menu.Focus();
        }
    }
}