using Checkers;

namespace CheckersMVC.ViewModels
{
    public class PieceVM
    {
        public bool IsDefined { get; set; }
        public Color Color { get; set; }

        public static PieceVM From(Piece p)
        {
            var vm = new PieceVM()
            {
                IsDefined = p != null,
            };
            if (p != null)
                vm.Color = (ViewModels.Color) p.pieceColor;
            return vm;
        }
    }

    public enum Color
    {
        White = 1,
        Black = 2,
    }
}