using System;
using System.Drawing;
using System.Windows.Forms;

namespace NecromancerGame.View
{
    public sealed partial class ControlReference : UserControl
    {
        private GameForm _form;
        private Action _hide;

        private string[,] Description =
        {
            {
                "F1",
                "Esc",
                "W/A/S/D",
                "E",
                "F",
                "R"
            },
            {
                " открыть/закрыть справку",
                " пауза",
                "перемещение персонажа",
                " атака",
                " взаимодействие с объектами",
                " вызов призрака"
            }
        };

        public ControlReference(GameForm form, Action hide)
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.UserPaint, true);
            _form = form;
            _hide = hide;
            ClientSize = _form.Size;
            Controls.Add(InitializeTable());
            BackColor = Color.Transparent;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                _hide.Invoke();
        }

        private TableLayoutPanel InitializeTable()
        {
            var tableSize = new Size(ClientSize.Width / 2, ClientSize.Height / 2);
            var tableLocation = new Point(ClientSize.Width / 2 - tableSize.Width / 2,
                ClientSize.Height / 2 - tableSize.Height / 2);
            var table = new TableLayoutPanel
            {
                Location = tableLocation,
                Size = tableSize,
                BackColor = Color.LightSlateGray
            };

            var columnCount = Description.GetLength(0);
            var rowCount = Description.GetLength(1);

            for (var i = 0; i < rowCount; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / rowCount));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));

            for (var column = 0; column < columnCount; column++)
                for (var row = 0; row < rowCount; row++)
                    table.Controls.Add(new Label
                    {
                        Text = Description[column, row],
                        BackColor = Color.Transparent,
                        Font = new Font("AlundraText", (float) table.Size.Height / 20),
                        Dock = DockStyle.Fill
                    }, column, row);

            return table;
        }
    }
}