using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TimeDateChanger
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetSystemTime(ref SYSTEMTIME lpSystemTime);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Electron Experts Time and Date Changer";
            this.Size = new System.Drawing.Size(400, 300);

            dateTimePicker1 = new DateTimePicker
            {
                Location = new System.Drawing.Point(50, 50),
                Size = new System.Drawing.Size(300, 30),
                Format = DateTimePickerFormat.Long
            };
            this.Controls.Add(dateTimePicker1);

            dateTimePicker2 = new DateTimePicker
            {
                Location = new System.Drawing.Point(50, 100),
                Size = new System.Drawing.Size(300, 30),
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };
            this.Controls.Add(dateTimePicker2);

            Button setButton = new Button
            {
                Text = "Set System Time",
                Location = new System.Drawing.Point(50, 150),
                Size = new System.Drawing.Size(300, 30)
            };
            setButton.Click += SetSystemTimeButton_Click;
            this.Controls.Add(setButton);
        }

        private void SetSystemTimeButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Combine selected date and time
                DateTime localDateTime = new DateTime(
                    dateTimePicker1.Value.Year,
                    dateTimePicker1.Value.Month,
                    dateTimePicker1.Value.Day,
                    dateTimePicker2.Value.Hour,
                    dateTimePicker2.Value.Minute,
                    dateTimePicker2.Value.Second
                );

                // Convert to UTC
                DateTime utcTime = localDateTime.ToUniversalTime();

                SYSTEMTIME systime = new SYSTEMTIME
                {
                    wYear = (short)utcTime.Year,
                    wMonth = (short)utcTime.Month,
                    wDay = (short)utcTime.Day,
                    wHour = (short)utcTime.Hour,
                    wMinute = (short)utcTime.Minute,
                    wSecond = (short)utcTime.Second,
                    wMilliseconds = (short)utcTime.Millisecond
                };

                if (SetSystemTime(ref systime))
                {
                    MessageBox.Show("System time successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update system time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
