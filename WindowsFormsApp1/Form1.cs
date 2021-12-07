using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UDP_server;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            stopReceive = false;
            rec = new Thread(new ThreadStart(Receive));
            rec.Start();

            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }


        public Bitmap drawFillRectangle((int CommandNumber, int[] parameters, int[] color) tupleFillRectangle)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(tupleFillRectangle.color[0], tupleFillRectangle.color[1], tupleFillRectangle.color[2])), tupleFillRectangle.parameters[0], tupleFillRectangle.parameters[1], tupleFillRectangle.parameters[2], tupleFillRectangle.parameters[3]); //рисуем закрашенный прямоугольник
            return bitmap;
        }

        public Bitmap drawFillEllipse((int CommandNumber, int[] parameters, int[] color) tupleFillEllipse)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillEllipse(new SolidBrush(Color.FromArgb(tupleFillEllipse.color[0], tupleFillEllipse.color[1], tupleFillEllipse.color[2])), tupleFillEllipse.parameters[0], tupleFillEllipse.parameters[1], tupleFillEllipse.parameters[2], tupleFillEllipse.parameters[3]);
            return bitmap;
        }

        public Bitmap ClearImage((int CommandNumber, int[] parameters, int[] color) tupleClear)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.FromArgb(tupleClear.color[0], tupleClear.color[1], tupleClear.color[2]));
            return bitmap;
        }


        public Bitmap drawEllipse((int CommandNumber, int[] parameters, int[] color) tupleEllipse)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawEllipse(new Pen(Color.FromArgb(tupleEllipse.color[0], tupleEllipse.color[1], tupleEllipse.color[2]), 2), tupleEllipse.parameters[0], tupleEllipse.parameters[1], tupleEllipse.parameters[2], tupleEllipse.parameters[3]);
            return bitmap;
        }


        public Bitmap drawRectangle((int CommandNumber, int[] parameters, int[] color) tupleRectangle)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawRectangle(new Pen(Color.FromArgb(tupleRectangle.color[0], tupleRectangle.color[1], tupleRectangle.color[2]), 2), tupleRectangle.parameters[0], tupleRectangle.parameters[1], tupleRectangle.parameters[2], tupleRectangle.parameters[3]);
            return bitmap;
        }

        public Bitmap drawLine((int CommandNumber, int[] parameters, int[] color) tupleLine)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawLine(new Pen(Color.FromArgb(tupleLine.color[0], tupleLine.color[1], tupleLine.color[2]), 2), tupleLine.parameters[0], tupleLine.parameters[1], tupleLine.parameters[2], tupleLine.parameters[3]); //рисуем линию
            return bitmap;
        }

        public Bitmap drawPixel((int CommandNumber, int[] parameters, int[] color) tuplePixel)
        {
           
            var graphics = Graphics.FromImage(bitmap);
            bitmap.SetPixel(tuplePixel.parameters[0], tuplePixel.parameters[1], Color.FromArgb(tuplePixel.color[0], tuplePixel.color[1], tuplePixel.color[2])); //рисуем точку
            return bitmap;
        }

        public Bitmap drawString((int CommandNumber, int[] parameters, int[] color) tupleString)
        {
            
            char letter = Convert.ToChar(tupleString.parameters[3]);
            var graphics = Graphics.FromImage(bitmap);
            Font drawFont = new Font("Arial", tupleString.parameters[2]);
            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(tupleString.color[0], tupleString.color[1], tupleString.color[2]));
            if (letter == 'T')
            {
                label1.Visible = true;
                label1.ForeColor = Color.FromArgb(tupleString.color[0], tupleString.color[1], tupleString.color[2]);
            }
            graphics.DrawString(Convert.ToString(letter), drawFont, drawBrush, tupleString.parameters[0], tupleString.parameters[1]);
            return bitmap;
        }



        



        Thread rec = null;
        UdpClient udp = new UdpClient(15000);
        bool stopReceive = false;
        private Bitmap bitmap;

        void Receive()
        {
            try
            {
                while (true)
                {

                    IPEndPoint ipendpoint = null;
                    byte[] message = udp.Receive(ref ipendpoint);
                    ShowMessage(Encoding.Default.GetString(message));

                    if (stopReceive == true) break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopReceive();
            Application.Exit();
        }

        void StopReceive()
        {
            stopReceive = true;
            if (udp != null) udp.Close();
            if (rec != null) rec.Join();
        }



        delegate void ShowMessageCallback(string message);
        void ShowMessage(string message)
        {
            
            if (InvokeRequired)
            {
                ShowMessageCallback dt = new ShowMessageCallback(ShowMessage);
                Invoke(dt, new object[] { message });
            }
            else
            {
                textBox1.Text = message;
                Command CommandObject = new Command();
                var TupleResult = CommandObject.Parser(message);
                
                int NumberCommand = TupleResult.CommandNumber;
                switch(NumberCommand)
                {
                    case 1:
                        pictureBox1.Image = ClearImage( TupleResult);
                        break;
                    case 2:
                        pictureBox1.Image = drawPixel(TupleResult);
                        break;
                    case 3:
                        pictureBox1.Image = drawLine(TupleResult);
                        break;
                    case 4:
                        pictureBox1.Image = drawRectangle(TupleResult);
                        break;
                    case 5:
                        pictureBox1.Image = drawFillRectangle(TupleResult);
                        break;
                    case 6:
                        pictureBox1.Image = drawEllipse(TupleResult);
                        break;
                    case 7:
                        pictureBox1.Image = drawFillEllipse(TupleResult);
                        break;
                    case 8:
                        pictureBox1.Image = drawString(TupleResult);
                        break;
                }
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            int s = DateTime.Now.Second;

            string time = "";

            if (h < 10)
            {
                time += "0" + h;
            }
            else
            {
                time += h;
            }

            time += ":";

            if (m < 10)
            {
                time += "0" + m;
            }
            else
            {
                time += m;
            }

            time += ":";

            if (s < 10)
            {
                time += "0" + s;
            }
            else
            {
                time += s;
            }

            label1.Text = time;
        }
    }
}