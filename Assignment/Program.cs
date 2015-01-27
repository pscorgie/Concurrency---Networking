using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Media;

namespace Assignment
{

    public class Planes
    {
        private static Planes instance = new Planes();

        public Image blueU, blueR, blueD, blueL;
        public Image redR, redD, redL;
        public Image yellowR, yellowD, yellowL;
        public Image greenU, greenR, greenD, greenL;
        public Image white;

        // Singleton initialization 
        private Planes()
        {
            blueR = Image.FromFile("BluePlane.png");
            blueD = Image.FromFile("BluePlane.png");
            blueD.RotateFlip(RotateFlipType.Rotate90FlipNone);
            blueL = Image.FromFile("BluePlane.png");
            blueL.RotateFlip(RotateFlipType.Rotate180FlipNone);

            redR = Image.FromFile("RedPlane.png");
            redD = Image.FromFile("RedPlane.png");
            redD.RotateFlip(RotateFlipType.Rotate90FlipNone);
            redL = Image.FromFile("RedPlane.png");
            redL.RotateFlip(RotateFlipType.Rotate180FlipNone);

            yellowR = Image.FromFile("YellowPlane.png");
            yellowD = Image.FromFile("YellowPlane.png");
            yellowD.RotateFlip(RotateFlipType.Rotate90FlipNone);
            yellowL = Image.FromFile("YellowPlane.png");
            yellowL.RotateFlip(RotateFlipType.Rotate180FlipNone);

            greenU = Image.FromFile("greenPlane.png");
            greenU.RotateFlip(RotateFlipType.Rotate270FlipNone);
            greenD = Image.FromFile("greenPlane.png");
            greenD.RotateFlip(RotateFlipType.Rotate90FlipNone);
            greenL = Image.FromFile("greenPlane.png");
            greenL.RotateFlip(RotateFlipType.Rotate180FlipNone);
            greenR = Image.FromFile("greenPlane.png");

            white = Image.FromFile("whitePlane.png");
            white.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public static Planes Instance
        {
            get { return instance; }
        }

    }
    
    public class Form1 : Form
    {

        private Container
        components = null;
        private Label lbl1, lbl2, lbl3;
        private PanelThread strip1, strip2, strip3;
        private HubThread hub1, hub2, hub3;
        private ArrivalThread arrivalHub;
        private RunwayThread runway;
        private VerticleThread leftStrip, rightStrip;
        private Thread arrivalThread, runwayThread, leftThread, rightThread, strip1Thread, strip2Thread, strip3Thread, hub1Thread, hub2Thread, hub3Thread;
        private Semaphore semaphoreArrRun, semaphoreRunLeft, semaphoreJ1, semaphoreJ2, semaphoreJ3, semaphoreS3Right, semaphoreRightRun;
        private Buffer buffer, bufferArrRun, bufferRunLeft, bufferLeftS1, bufferS1S2, bufferS2S3, bufferS3Right, bufferRightRun, bufferS1Hub, bufferS2Hub, bufferS3Hub;
        private JunctionBuffer bufferJ1, bufferJ2, bufferJ3, bufferJ4;
        // strips
        private Panel pnl1, pnl2, pnl3, pnl4, pnl5, pnl6, pnl7, pnl8, pnl9, pnl10, pnl11;
        // lines
        private Panel line1, line2, line3, line4, line5, line6, line7, line8, line9, line10;
        private Button btn1, btn2, btn3, arrivalBtn;
        private RadioButton radioButton0, radioButton1, radioButton2, radioButton3;

        public Form1()
        {
            InitializeComponent();

            semaphoreArrRun = new Semaphore(); semaphoreRunLeft = new Semaphore(); semaphoreJ1 = new Semaphore();
            semaphoreJ2 = new Semaphore(); semaphoreJ3 = new Semaphore(); semaphoreS3Right = new Semaphore(); semaphoreRightRun = new Semaphore();

            buffer = new Buffer(); bufferArrRun = new Buffer(); bufferRunLeft = new Buffer(); bufferLeftS1 = new Buffer();
            bufferS1S2 = new Buffer(); bufferS2S3 = new Buffer(); bufferS3Right = new Buffer(); bufferRightRun = new Buffer();
            bufferS1Hub = new Buffer(); bufferS2Hub = new Buffer(); bufferS3Hub = new Buffer();

            bufferJ1 = new JunctionBuffer(); bufferJ2 = new JunctionBuffer(); bufferJ3 = new JunctionBuffer(); bufferJ4 = new JunctionBuffer();
            bufferJ1.write(true); bufferJ2.write(true); bufferJ3.write(true); bufferJ4.write(true);


            hub1 = new HubThread(new Point(-10, -10),
                                 100, pnl1,
                                 Color.Blue,
                                 semaphoreJ1, bufferLeftS1, bufferS1Hub, bufferJ1, btn1, 1
                                 );

            hub2 = new HubThread(new Point(-10, -10),
                                 100, pnl2,
                                 Color.Red,
                                 semaphoreJ2, bufferS1S2, bufferS2Hub, bufferJ2, btn2, 2
                                 );

            hub3 = new HubThread(new Point(-10, -10),
                                 100, pnl3,
                                 Color.Yellow,
                                 semaphoreJ3, bufferS2S3, bufferS3Hub, bufferJ3, btn3, 3
                                 );

            strip1 = new PanelThread(new Point(-10, -10),
                                 80, pnl4,
                                 Color.Gray,
                                 semaphoreJ1, bufferLeftS1, semaphoreJ2, bufferS1S2, bufferS1Hub, bufferJ1, bufferJ2, 1
                                 );


            strip2 = new PanelThread(new Point(-10, -10),
                                 80, pnl5,
                                 Color.Gray,
                                 semaphoreJ2, bufferS1S2, semaphoreJ3, bufferS2S3, bufferS2Hub, bufferJ2, bufferJ3, 2
                                 );

            strip3 = new PanelThread(new Point(-10, -10),
                                 80, pnl6,
                                 Color.Gray,
                                 semaphoreJ3, bufferS2S3, semaphoreS3Right, bufferS3Right, bufferS3Hub, bufferJ3, bufferJ4, 3
                                 );

            leftStrip = new VerticleThread(new Point(-10, 140),
                                 100, pnl10,
                                 Color.Gray,
                                 semaphoreRunLeft, bufferRunLeft, semaphoreJ1, bufferLeftS1, bufferJ1, true
                                 );

            rightStrip = new VerticleThread(new Point(-10, -10),
                                 100, pnl7,
                                 Color.Gray,
                                 semaphoreS3Right, bufferS3Right, semaphoreArrRun, bufferArrRun, bufferJ4, false
                                 );

            runway = new RunwayThread(new Point(550, -10),
                                 50, pnl8,
                                 Color.Gray,
                                 semaphoreArrRun, bufferArrRun, semaphoreRunLeft, bufferRunLeft
                                 );

            arrivalHub = new ArrivalThread(new Point(30, -10),
                                 100, pnl9,
                                 Color.Green,
                                 semaphoreArrRun, bufferArrRun, arrivalBtn, pnl11
                                 );

            arrivalThread = new Thread(new ThreadStart(arrivalHub.Start));
            runwayThread = new Thread(new ThreadStart(runway.Start));
            leftThread = new Thread(new ThreadStart(leftStrip.Start));
            strip1Thread = new Thread(new ThreadStart(strip1.Start));
            strip2Thread = new Thread(new ThreadStart(strip2.Start));
            strip3Thread = new Thread(new ThreadStart(strip3.Start));
            rightThread = new Thread(new ThreadStart(rightStrip.Start));
            hub1Thread = new Thread(new ThreadStart(hub1.Start));
            hub2Thread = new Thread(new ThreadStart(hub2.Start));
            hub3Thread = new Thread(new ThreadStart(hub3.Start));

            this.Closing += new CancelEventHandler(this.Form1_Closing);

            arrivalThread.Start();
            runwayThread.Start();
            leftThread.Start();
            strip1Thread.Start();
            strip2Thread.Start();
            strip3Thread.Start();
            rightThread.Start();
            hub1Thread.Start();
            hub2Thread.Start();
            hub3Thread.Start();

            BufferData data;
            data.destination = 0;
            data.direction = true;
            data.brushColour = Color.Blue;
            data.panel = 1;
            data.name = "X955";
            bufferS1Hub.write(data);
            data.brushColour = Color.Red;
            data.name = "X133";
            bufferS2Hub.write(data);
            data.brushColour = Color.Yellow;
            data.name = "X090";
            bufferS3Hub.write(data);

        }

        


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Bermuda Triangle Airways";
            this.Size = new System.Drawing.Size(700, 500);
            this.BackColor = Color.LightSeaGreen;
            Image grass = Image.FromFile("grass.png");
            this.BackgroundImage = grass;
            Image tar = Image.FromFile("tar.png");

            // Hub 1
            this.pnl1 = new Panel();
            this.pnl1.Location = new Point(100, 50);
            this.pnl1.Size = new Size(30, 150);
            this.pnl1.BackgroundImage = tar;

            // Hub 2
            this.pnl2 = new Panel();
            this.pnl2.Location = new Point(250, 50);
            this.pnl2.Size = new Size(30, 150);
            this.pnl2.BackgroundImage = tar;

            // Hub 3
            this.pnl3 = new Panel();
            this.pnl3.Location = new Point(400, 50);
            this.pnl3.Size = new Size(30, 150);
            this.pnl3.BackgroundImage = tar;
            
            // strip 1
            this.pnl4 = new Panel();
            this.pnl4.Location = new Point(100, 200);
            this.pnl4.Size = new Size(150, 30);
            this.pnl4.BackgroundImage = tar;

            // strip 2
            this.pnl5 = new Panel();
            this.pnl5.Location = new Point(250, 200);
            this.pnl5.Size = new Size(150, 30);
            this.pnl5.BackgroundImage = tar;

            // strip 3
            this.pnl6 = new Panel();
            this.pnl6.Location = new Point(400, 200);
            this.pnl6.Size = new Size(150, 30);
            this.pnl6.BackgroundImage = tar;

            // Right Verticle
            this.pnl7 = new Panel();
            this.pnl7.Location = new Point(550, 200);
            this.pnl7.Size = new Size(30, 150);
            this.pnl7.BackgroundImage = tar;

            // Runway
            this.pnl8 = new Panel();
            this.pnl8.Location = new Point(0, 350);
            this.pnl8.Size = new Size(580, 30);
            this.pnl8.BackgroundImage = tar;

            // Arrival strip
            this.pnl9 = new Panel();
            this.pnl9.Location = new Point(580, 350);
            this.pnl9.Size = new Size(70, 30);
            this.pnl9.BackgroundImage = tar;

            // Left Verticle
            this.pnl10 = new Panel();
            this.pnl10.Location = new Point(100, 170);
            this.pnl10.Size = new Size(30, 180);
            this.pnl10.BackgroundImage = tar;

            this.pnl11 = new Panel();
            this.pnl11.Location = new Point(600, 200);
            this.pnl11.Size = new Size(80, 150);
            this.pnl11.BackColor = Color.Teal;

            this.radioButton0 = new RadioButton();
            this.radioButton0.Location = new Point(20, 10);
            this.radioButton0.Text = "0";

            this.radioButton1 = new RadioButton();
            this.radioButton1.Location = new Point(20, 40);
            this.radioButton1.Text = "1";

            this.radioButton2 = new RadioButton();
            this.radioButton2.Location = new Point(20, 70);
            this.radioButton2.Text = "2";

            this.radioButton3 = new RadioButton();
            this.radioButton3.Location = new Point(20, 100);
            this.radioButton3.Text = "3";

            pnl11.Controls.Add(radioButton0);
            pnl11.Controls.Add(radioButton1);
            pnl11.Controls.Add(radioButton2);
            pnl11.Controls.Add(radioButton3);


            // Black lines

            this.line1 = new Panel();
            this.line1.Location = new Point(250, 200);
            this.line1.Size = new Size(2, 30);
            this.line1.BackColor = Color.Black;

            this.line2 = new Panel();
            this.line2.Location = new Point(400, 200);
            this.line2.Size = new Size(2, 30);
            this.line2.BackColor = Color.Black;

            this.line3 = new Panel();
            this.line3.Location = new Point(550, 200);
            this.line3.Size = new Size(2, 30);
            this.line3.BackColor = Color.Black;

            this.line4 = new Panel();
            this.line4.Location = new Point(100, 200);
            this.line4.Size = new Size(30, 2);
            this.line4.BackColor = Color.Black;

            this.line5 = new Panel();
            this.line5.Location = new Point(250, 200);
            this.line5.Size = new Size(30, 2);
            this.line5.BackColor = Color.Black;

            this.line6 = new Panel();
            this.line6.Location = new Point(400, 200);
            this.line6.Size = new Size(30, 2);
            this.line6.BackColor = Color.Black;

            this.line7 = new Panel();
            this.line7.Location = new Point(550, 350);
            this.line7.Size = new Size(30, 2);
            this.line7.BackColor = Color.Black;

            this.line8 = new Panel();
            this.line8.Location = new Point(580, 350);
            this.line8.Size = new Size(2, 30);
            this.line8.BackColor = Color.Black;

            this.line9 = new Panel();
            this.line9.Location = new Point(100, 350);
            this.line9.Size = new Size(30, 2);
            this.line9.BackColor = Color.Black;

            this.line10 = new Panel();
            this.line10.Location = new Point(100, 230);
            this.line10.Size = new Size(30, 2);
            this.line10.BackColor = Color.Black;

            this.btn1 = new Button();
            this.btn1.Size = new Size(30, 30);
            this.btn1.BackColor = Color.Pink;
            this.btn1.Location = new Point(100, 20);

            this.btn2 = new Button();
            this.btn2.Size = new Size(30, 30);
            this.btn2.BackColor = Color.Pink;
            this.btn2.Location = new Point(250, 20);

            this.btn3 = new Button();
            this.btn3.Size = new Size(30, 30);
            this.btn3.BackColor = Color.Pink;
            this.btn3.Location = new Point(400, 20);

            this.arrivalBtn = new Button();
            this.arrivalBtn.Size = new Size(30, 30);
            this.arrivalBtn.BackColor = Color.Pink;
            this.arrivalBtn.Location = new Point(650, 350);

            // 
            // lbl1
            // 
            lbl1 = new Label();
            lbl1.BackColor = Color.Blue;
            this.lbl1.Font = new System.Drawing.Font("Times New Roman", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1.Location = new System.Drawing.Point(140, 20);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(30, 30);
            this.lbl1.TabIndex = 25;
            this.lbl1.Text = "1";
            // 
            // lbl2
            // 
            lbl2 = new Label();
            this.lbl2.BackColor = Color.Red;
            this.lbl2.Font = new System.Drawing.Font("Times New Roman", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(290, 20);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(30, 30);
            this.lbl2.TabIndex = 26;
            this.lbl2.Text = "2";
            // 
            // lbl3
            // 
            lbl3 = new Label();
            this.lbl3.BackColor = Color.Yellow;
            this.lbl3.Font = new System.Drawing.Font("Times New Roman", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.Location = new System.Drawing.Point(440, 20);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(30, 30);
            this.lbl3.TabIndex = 27;
            this.lbl3.Text = "3";

            this.Controls.Add(line1);
            this.Controls.Add(line2);
            this.Controls.Add(line3);
            this.Controls.Add(line4);
            this.Controls.Add(line5);
            this.Controls.Add(line6);
            this.Controls.Add(line7);
            this.Controls.Add(line8);
            this.Controls.Add(line9);
            this.Controls.Add(line10);
            this.Controls.Add(pnl1);
            this.Controls.Add(pnl2);
            this.Controls.Add(pnl3);
            this.Controls.Add(pnl4);
            this.Controls.Add(pnl5);
            this.Controls.Add(pnl6);
            this.Controls.Add(pnl7);
            this.Controls.Add(pnl8);
            this.Controls.Add(pnl9);
            this.Controls.Add(pnl10);
            this.Controls.Add(pnl11);
            this.Controls.Add(btn1);
            this.Controls.Add(btn2);
            this.Controls.Add(btn3);
            this.Controls.Add(arrivalBtn);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl3);

            // Wire Closing event.      
            this.Closing += new CancelEventHandler(this.Form1_Closing);
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            // Environment is a System class.
            // Kill off all threads on exit.
            Environment.Exit(Environment.ExitCode);
        }


    }// end class form1


    public class Client
    {
        private NetworkStream output;
        private BinaryWriter writer;
        private BinaryReader reader;
        private TcpClient client;

        public Client()
        {
            
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 5000);
                output = client.GetStream();
                writer = new BinaryWriter(output);
                reader = new BinaryReader(output);

            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                System.Environment.Exit(System.Environment.ExitCode);
            }
            
        }
        public void HandleCommunication(string message)
        {

            writer.Write(DateTime.Now.ToString("h:mm:ss tt") + ">>> " + message);

        }
    }

    public class Semaphore
    {
        private int count = 1; // set semaphore to available

        public void Wait()
        {
            lock (this)
            {
                // wait if semaphore unavailable
                while (count == 0)
                    Monitor.Wait(this);
                count = 0;
            }
            //Console.WriteLine("Sem: " + count);
        }

        public void Signal()
        {
            lock (this)
            {
                count = 1;
                Monitor.Pulse(this);
            }
            //Console.WriteLine("Sem: " + count);
        }

        public void Start()
        {
        }

    }// end class Semaphore
    public struct BufferData
    {
        public Color brushColour;
        public int destination;
        public bool direction;
        public int panel;
        public string name;
    }
    public class Buffer
    {

        private BufferData data;

        private bool empty = true;

        public void read(ref BufferData data)
        {
            lock (this)
            {
                if (empty)
                    Monitor.Wait(this);
                empty = true;
                data.brushColour = this.data.brushColour;
                data.destination = this.data.destination;
                data.direction = this.data.direction;
                data.panel = this.data.panel;
                data.name = this.data.name;
                Monitor.Pulse(this);
                //Console.WriteLine("Buffer Read complete");

            }
        }

        public void write(BufferData data)
        {
            lock (this)
            {
                if (!empty)
                    Monitor.Wait(this);
                empty = false;
                this.data.brushColour = data.brushColour;
                this.data.destination = data.destination;
                this.data.direction = data.direction;
                this.data.panel = data.panel;
                this.data.name = data.name;
                Monitor.Pulse(this);
                //Console.WriteLine("Buffer write complete");
            }
        }

        public void Start()
        { }
    }

    public class JunctionBuffer
    {
        //private Color brushColour;
        //private int destination;
        private bool hub;

        private bool empty = true;

        public void read(ref bool hub)
        {
            lock (this)
            {
                if (empty)
                    Monitor.Wait(this);
                empty = true;
                hub = this.hub;
                Monitor.Pulse(this);
                //Console.WriteLine("J Read complete");
            }
        }

        public void write(bool hub)
        {
            lock (this)
            {
                if (!empty)
                    Monitor.Wait(this);
                empty = false;
                this.hub = hub;
                Monitor.Pulse(this);
                //Console.WriteLine("J write complete");
            }
        }

        public void Start()
        { }
    }

    public class HubThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphore;
        private Buffer bufferIn, bufferOut;
        private JunctionBuffer jBuffer;
        private Button btn;
        private bool locked = true;
        private BufferData data;
        private int hub;
        private bool bufread;
        private Client client;
        private Image stop, go;

        public HubThread(Point origin, int delay, Panel panel, Color colour, Semaphore semaphore, Buffer bufferOut, Buffer bufferIn, JunctionBuffer jBuffer, Button btn, int hub)
        {
            this.origin = origin;
            this.delay = delay;
            this.panel = panel;
            this.data.brushColour = colour;
            this.data.destination = 0;
            this.data.direction = false;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = 0;
            this.yDelta = 5;
            this.semaphore = semaphore;
            this.bufferIn = bufferIn;
            this.bufferOut = bufferOut;
            this.jBuffer = jBuffer;
            this.btn = btn;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            this.hub = hub;
            client = new Client();
            this.stop = Image.FromFile("stop.png");
            this.go = Image.FromFile("go.png");
            this.btn.BackgroundImage = stop;
        }

        private void btn_Click(object sender, System.EventArgs e)
        {
            locked = !locked;
            this.btn.BackColor = locked ? Color.Pink : Color.LightGreen;
            if (locked)
            {
                btn.BackgroundImage = stop;
            }
            else
                btn.BackgroundImage = go;
            lock (this)
            {
                if (!locked)
                {
                    this.data.direction = false;
                    this.data.destination = 0;
                    Monitor.Pulse(this);

                }
               
            }
        }


        public void Start()
        {
            Color signal = Color.Red;
            Thread.Sleep(delay);
            while (true)
            {
 
                this.zeroPlane();
                panel.Invalidate();
                bufferIn.read(ref this.data);
                if (hub == 1)
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Hub " + hub);
                if (hub == 2)
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Hub " + hub);
                if (hub == 3)
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Hub " + hub);
                
                if (!locked/*btn.BackColor == Color.LightGreen*/)
                {
                    if(data.direction)// going in
                    {
                        this.zeroPlane();  
                        panel.Invalidate();
                        for (int i = 1; i < 26; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                            panel.Invalidate();
                        }                   
                        changeDirection(); // going out
                    }
                    if(!data.direction)
                    {
                        client.HandleCommunication("ArrtxtFlight " + data.name + " Released from Hub " + hub);
                        for (int i = 1; i < 22; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                            panel.Invalidate();
                        }
                        data.destination = 0;
                        semaphore.Wait();
                        bufferOut.write(this.data);
                        jBuffer.read(ref bufread);
                        Thread.Sleep(delay);
                        this.data.brushColour = Color.Gray;
                        panel.Invalidate();
                    }
                }
                if (locked/*btn.BackColor == Color.Pink*/)
                {
                    if (data.direction)// going in
                    {
                        this.zeroPlane();
                        panel.Invalidate();
                        for (int i = 1; i < 26; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                            panel.Invalidate();
                        }
                        client.HandleCommunication("ArrtxtFlight " + data.name + " Held in Hub " + hub);
                        changeDirection(); // going out

                        lock (this)
                        {
                            while (locked)
                                Monitor.Wait(this);
                        }
                    }

                    if (!locked/*btn.BackColor == Color.LightGreen*/)
                    {
                        client.HandleCommunication("ArrtxtFlight " + data.name + " Released from Hub " + hub);
                        for (int i = 1; i < 22; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                            panel.Invalidate();
                        }
                        data.destination = 0;
                        semaphore.Wait();
                        bufferOut.write(this.data);
                        jBuffer.read(ref bufread);
                        Thread.Sleep(delay);
                        this.data.brushColour = Color.Gray;
                        panel.Invalidate();
                    }
                }

                            
            }

        }

        private void changeDirection()
        {
            data.direction = false;
            switch (hub)
            {
                case 1 :
                    data.brushColour = Color.Blue;
                    break;
                case 2:
                    data.brushColour = Color.Red;
                    break;
                case 3:
                    data.brushColour = Color.Yellow;
                    break;
            }
            
        }

        private void zeroPlane()
        {
            if (!data.direction)
            {
                plane.X = origin.X;
                plane.Y = origin.Y;

            }
            else
            {
                plane.X = origin.X;
                plane.Y = 130;

            }
        }

        private void movePlane(int xDelta, int yDelta)
        {
            if (!data.direction)
            {
                plane.X += xDelta; plane.Y += yDelta;
            }
            else
            {
                plane.X += xDelta; plane.Y -= yDelta;
            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(data.brushColour);


            if (data.brushColour == Color.Green)
            {
                g.DrawImage(Planes.Instance.greenU, plane.X, plane.Y, 50.0f, 50.0f);
            }
            else
            {
                if (data.brushColour == Color.Blue)
                {
                    g.DrawImage(Planes.Instance.blueD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Red)
                {
                    g.DrawImage(Planes.Instance.redD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Yellow)
                {
                    g.DrawImage(Planes.Instance.yellowD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Gray)
                {
                    g.DrawImage(Planes.Instance.white, plane.X, plane.Y, 50.0f, 50.0f);
                }
            }
            

            
            brush.Dispose();     //  Dispose of graphic
            g.Dispose();        //  resources  

        }
    }// end class HubThread

    public class PanelThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphoreEntry, semaphoreExit;
        private Buffer bufferEntry, bufferExit, bufferHub;
        private JunctionBuffer jBuffer1, jBuffer2;
        private int strip;
        private BufferData data;
        private bool hub = false;
        private Client client;

        public PanelThread(Point origin, int delay, Panel panel, Color colour, Semaphore semaphoreEntry, Buffer bufferEntry, Semaphore semaphoreExit,
            Buffer bufferExit, Buffer bufferHub, JunctionBuffer jBuffer1, JunctionBuffer jBuffer2, int strip)
        {
            this.origin = origin;
            this.delay = delay;
            this.panel = panel;
            this.data.brushColour = colour;
            this.data.destination = 0;
            this.data.direction = true;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = 5;
            this.yDelta = 0;
            this.semaphoreEntry = semaphoreEntry;
            this.bufferEntry = bufferEntry;
            this.semaphoreExit = semaphoreExit;
            this.bufferExit = bufferExit;
            this.bufferHub = bufferHub;
            this.jBuffer1 = jBuffer1;
            this.jBuffer2 = jBuffer2;
            this.strip = strip;
            client = new Client();

        }

        public void Start()
        {

            Color signal = Color.Red;
            Thread.Sleep(delay);

            while (true)
            {

                this.zeroPlane();
                bufferEntry.read(ref this.data);
                if(strip == 1)
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Section 2");
                else if (strip == 2)
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Section 3");
                else if (strip == 3)
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Section 4");
                panel.Invalidate();

                if (data.destination == strip)
                {
                    panel.Invalidate();
                    Thread.Sleep(delay);
                    bufferHub.write(this.data);
                    semaphoreEntry.Signal();
                }
                else
                {

                    for (int i = 1; i < 25; i++)
                    {
                        this.movePlane(xDelta, yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }

                    if (data.destination == (strip + 1))
                    {
                        hub = true;
                        jBuffer2.write(this.hub);
                        hub = false;
                    }
                    
                    semaphoreExit.Wait();
                    bufferExit.write(this.data);
                    semaphoreEntry.Signal();
                }
                this.data.brushColour = Color.Gray;
                panel.Invalidate();
            }
        }


        private void zeroPlane()
        {
            plane.X = origin.X;
            plane.Y = origin.Y;
        }

        private void movePlane(int xDelta, int yDelta)
        {
            plane.X += xDelta; plane.Y += yDelta;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(data.brushColour);

            if (data.brushColour == Color.Blue)
            {
                g.DrawImage(Planes.Instance.blueR, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Green)
            {
                g.DrawImage(Planes.Instance.greenR, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Red)
            {
                g.DrawImage(Planes.Instance.redR, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Yellow)
            {
                g.DrawImage(Planes.Instance.yellowR, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Gray)
            {
                g.DrawImage(Planes.Instance.white, plane.X, plane.Y, 50.0f, 50.0f);
            }


            brush.Dispose();     //  Dispose of graphic
            g.Dispose();        //  resources  

        }
    }// end class PanelThread

    public class VerticleThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphoreEntry, semaphoreExit;
        private Buffer bufferEntry, bufferExit;
        private JunctionBuffer jBuffer;
        private bool leftSide;
        private BufferData data;
        private bool hub = false;
        private Client client;


        public VerticleThread(Point origin, int delay, Panel panel, Color colour, Semaphore semaphoreEntry, Buffer bufferEntry, Semaphore semaphoreExit, Buffer bufferExit, JunctionBuffer jBuffer, bool leftSide)
        {
            this.origin = origin;
            this.delay = delay;
            this.panel = panel;
            this.data.brushColour = colour;
            this.data.destination = 0;
            this.data.direction = true;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = 0;
            this.yDelta = leftSide ? -5 : +5;
            this.semaphoreEntry = semaphoreEntry;
            this.bufferEntry = bufferEntry;
            this.semaphoreExit = semaphoreExit;
            this.bufferExit = bufferExit;
            this.jBuffer = jBuffer;
            this.leftSide = leftSide;
            client = new Client();
        }

        public void Start()
        {
            Color signal = Color.Red;
            Thread.Sleep(delay);

            while (true)
            {
                if (leftSide)
                {
                    this.zeroPlane();
                    panel.Invalidate();
                    bufferEntry.read(ref this.data);
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Section 1");
                    for (int i = 1; i < 19; i++)
                    {
                        this.movePlane(xDelta, yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }
                    if (data.destination == 1)
                    {
                        hub = true;
                        jBuffer.write(this.hub);
                        hub = false;
                    }
                    semaphoreExit.Wait();
                    bufferExit.write(this.data);
                    semaphoreEntry.Signal();
                    this.data.brushColour = Color.Gray;
                    panel.Invalidate();                   
                }
                else 
                {
                    this.zeroPlane();
                    panel.Invalidate();
                    bufferEntry.read(ref this.data);
                    client.HandleCommunication("ArrtxtFlight " + data.name + " Entered Section 5");
                    for (int i = 1; i < 25; i++)
                    {
                        this.movePlane(xDelta, yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }
                    semaphoreExit.Wait();
                    bufferExit.write(this.data);
                    semaphoreEntry.Signal();
                    this.data.brushColour = Color.Gray;
                    panel.Invalidate();
                }

            }
        }


        private void zeroPlane()
        {
            plane.X = origin.X;
            plane.Y = origin.Y;
        }

        private void movePlane(int xDelta, int yDelta)
        {
            plane.X += xDelta; plane.Y += yDelta;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(data.brushColour);
            if(leftSide)
            { 

                if (data.brushColour == Color.Green)
                {
                    g.DrawImage(Planes.Instance.greenU, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Gray)
                {
                    g.DrawImage(Planes.Instance.white, plane.X, plane.Y, 50.0f, 50.0f);
                }
            }
            else 
            {
                if (data.brushColour == Color.Blue)
                {
                    g.DrawImage(Planes.Instance.blueD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Green)
                {
                    g.DrawImage(Planes.Instance.greenD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Red)
                {
                    g.DrawImage(Planes.Instance.redD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Yellow)
                {
                    g.DrawImage(Planes.Instance.yellowD, plane.X, plane.Y, 50.0f, 50.0f);
                }
                if (data.brushColour == Color.Gray)
                {
                    g.DrawImage(Planes.Instance.white, plane.X, plane.Y, 50.0f, 50.0f);
                }
            }

            brush.Dispose();     //  Dispose of graphic
            g.Dispose();        //  resources  

        }
    }// end class VerticleThread

    public class RunwayThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphore, semaphoreStrip;
        private Buffer buffer, bufferStrip;
        private BufferData data;
        private Client client;
        private int takeOff;
        private SoundPlayer soundPlayer;

        public RunwayThread(Point origin, int delay, Panel panel, Color colour, Semaphore semaphore, Buffer buffer, Semaphore semaphoreStrip, Buffer bufferStrip)
        {
            this.origin = origin;
            this.delay = delay;
            this.panel = panel;
            this.data.brushColour = colour;
            this.data.destination = 0;
            this.data.direction = true;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = -5;
            this.yDelta = 0;
            this.semaphore = semaphore;
            this.buffer = buffer;
            this.semaphoreStrip = semaphoreStrip;
            this.bufferStrip = bufferStrip;
            this.client = new Client();
            takeOff = 0;
            soundPlayer = new SoundPlayer("planeSound.wav");

        }

        public void Start()
        {
            Color signal = Color.Red;
            Thread.Sleep(delay);

                while (true)
                {
                    this.zeroPlane();
                    buffer.read(ref this.data);

                    client.HandleCommunication("ArrtxtFlight " + data.name + " On Runway");
                    
                    if (data.destination != 0)
                    {
                        client.HandleCommunication("ArrtxtFlight " + data.name + " Destination Hub: " + data.destination);
                        delay = 15;
                        panel.Invalidate();

                        for (int i = 1; i < 93; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                            panel.Invalidate();
                            if (delay < 50)
                                delay++;
                        }
            
                        this.semaphoreStrip.Wait();
                        this.bufferStrip.write(this.data);
                        semaphore.Signal();
                        this.data.brushColour = Color.Gray;
                        panel.Invalidate();
                    }
                    else
                    {
                        soundPlayer.Play();
                        delay = 50;
                        panel.Invalidate();

                        for (int i = 1; i < 114; i++)
                        {
                            this.movePlane(xDelta, yDelta);
                            Thread.Sleep(delay);
                            panel.Invalidate();
                            if (delay > 15)
                                delay--;
                        }
                        semaphore.Signal();
                        takeOff++;
                        client.HandleCommunication("deptxtFlight " + data.name + " Take-off");
                        client.HandleCommunication("Total" + takeOff);
                        client.HandleCommunication("Count" + "down");
                        this.data.brushColour = Color.Gray;
                        panel.Invalidate();
                    }
                }
        }


        private void zeroPlane()
        {
            plane.X = origin.X;
            plane.Y = origin.Y;
        }

        private void movePlane(int xDelta, int yDelta)
        {
            plane.X += xDelta; plane.Y += yDelta;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(data.brushColour);
            if (data.brushColour == Color.Blue)
            {
                g.DrawImage(Planes.Instance.blueL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Green)
            {
                g.DrawImage(Planes.Instance.greenL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Red)
            {
                g.DrawImage(Planes.Instance.redL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Yellow)
            {
                g.DrawImage(Planes.Instance.yellowL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Gray)
            {
                g.DrawImage(Planes.Instance.white, plane.X, plane.Y, 50.0f, 50.0f);
            }


            brush.Dispose();     //  Dispose of graphic
            g.Dispose();        //  resources  

        }
    }// end class RunwayThread

    public class ArrivalThread
    {
        private Point origin;
        private int delay;
        private Panel panel, dest;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphore;
        private Buffer buffer;
        private Button btn;
        private bool locked = true;
        private BufferData data;
        //private bool hub = true;
        private Client client;

        public ArrivalThread(Point origin, int delay, Panel panel, Color colour, Semaphore semaphore, Buffer buffer, Button btn, Panel dest)
        {
            this.origin = origin;
            this.delay = delay;
            this.panel = panel;
            this.data.brushColour = colour;
            this.data.destination = 0;
            this.data.direction = true;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = -5;
            this.yDelta = 0;
            this.semaphore = semaphore;
            this.buffer = buffer;
            this.btn = btn;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            this.dest = dest;
            this.client = new Client();
        }

        private void btn_Click(object sender,
                            System.EventArgs e)
        {
            locked = !locked;
            this.btn.BackColor = locked ? Color.Pink : Color.LightGreen;
            lock (this)
            {
                if (!locked)
                {
                    this.zeroPlane();
                    this.data.brushColour = Color.Green;
                    Random ran = new Random();
                    this.data.name = "X" + ran.Next(100, 999);
                    this.client.HandleCommunication("ArrtxtFlight " + data.name + " Incoming");
                    client.HandleCommunication("Count");
                    Monitor.Pulse(this);
                    foreach (RadioButton radioButton in dest.Controls)
                    {
                        if (radioButton.Checked)
                        {
                            int.TryParse(radioButton.Text, out data.destination);
                        }
                    }
                }
               
            }
        }


        public void Start()
        {
            Color signal = Color.Red;
            Thread.Sleep(delay);

            while(true)
            {
                
                this.zeroPlane();
                panel.Invalidate();
                lock(this)
                {
                    while(locked)
                    {
                        Monitor.Wait(this);
                    }
                }

                for (int i = 1; i < 10; i++)
                {
                    this.movePlane(xDelta, yDelta);
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }
                semaphore.Wait();
                client.HandleCommunication("ArrtxtFlight " + data.name + " Landed");
                this.buffer.write(this.data);
                this.data.brushColour = Color.White;
                panel.Invalidate();
                locked = true;
                this.btn.BackColor = locked ? Color.Pink : Color.LightGreen;
                this.data.brushColour = Color.Gray;
                panel.Invalidate();

            }


        }


        private void zeroPlane()
        {
            plane.X = origin.X;
            plane.Y = origin.Y;
        }

        private void movePlane(int xDelta, int yDelta)
        {
            plane.X += xDelta; plane.Y += yDelta;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush brush = new SolidBrush(data.brushColour);
            if (data.brushColour == Color.Blue)
            {
                g.DrawImage(Planes.Instance.blueL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Green)
            {
                g.DrawImage(Planes.Instance.greenL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Red)
            {
                g.DrawImage(Planes.Instance.redL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Yellow)
            {
                g.DrawImage(Planes.Instance.yellowL, plane.X, plane.Y, 50.0f, 50.0f);
            }
            if (data.brushColour == Color.Gray)
            {
                g.DrawImage(Planes.Instance.white, plane.X, plane.Y, 50.0f, 50.0f);
            }

            brush.Dispose();     //  Dispose of graphic
            g.Dispose();        //  resources  

        }
    }// end class ArrivalThread

    public class TheOne
    {
        public static void Main()//
        {
            Application.Run(new Form1());
        }
    }// end class TheOne

}
