using System;
using System.Windows.Forms;

namespace ungdungthoitiet
{
    static class Program
    {
        /// <summary>
        /// Điểm bắt đầu của ứng dụng.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new txt()); // Tạo và hiển thị form chính
        }
    }
}
