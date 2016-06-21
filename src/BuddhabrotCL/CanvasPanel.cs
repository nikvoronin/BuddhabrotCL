using System.Windows.Forms;

namespace BuddhabrotCL
{
    public class CanvasPanel : Panel
    {
        public CanvasPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
        }
    }
}
