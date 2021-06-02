namespace NecromancerGame.Model
{
    public class Dialog
    {
        public int CurrentLine;
        public int CurrentBigLine;
        public string[] Text;
        public readonly int[] LinesCapacity;
        
        public Dialog(string[] text, int[] capacity)
        {
            Text = text;
            CurrentLine = 0;
            CurrentBigLine = 0;
            LinesCapacity = capacity;
        }

        public void StartDialog(Game game)
        {
        }

        public void Next(Game game)
        {
            ++CurrentLine;
            var num = 0;
            for (var index = 0; index < CurrentBigLine; ++index)
                num += LinesCapacity[index];
            if (CurrentLine < num + LinesCapacity[CurrentBigLine])
                return;
            ++CurrentBigLine;
            if (CurrentBigLine >= LinesCapacity.Length)
            {
                CurrentBigLine = 0;
                CurrentLine = 0;
            }
        }
    }
}