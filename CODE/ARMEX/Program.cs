namespace _ARMEX
{
    internal static class Program
    {
        /// <summary>
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Main mainForm = new Main();  
            mainForm.Hide();            

            Login loginForm = new Login(mainForm);  
            Application.Run(loginForm);             

        }
    }
}