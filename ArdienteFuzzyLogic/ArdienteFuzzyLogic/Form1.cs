using DotFuzzy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ArdienteFuzzyLogic
{
    public partial class Form1 : Form
    {
        FuzzyEngine fe;
        MembershipFunctionCollection speed, angle, throttle;
        LinguisticVariable myspeed, myangle, mythrottle;
        bool canExit = false;

       

        FuzzyRuleCollection myrules;
        public Form1()
        {
            InitializeComponent();
            InitializeCharts();
            setMembers();
            setRules();
            setFuzzyEngine();
        }
        

        public void setMembers()
        {
            textBox3.Text = "0";
            speed = new MembershipFunctionCollection();
            speed.Add(new MembershipFunction("LOW", 0.0, 0.0, 45.0, 50.0));
            speed.Add(new MembershipFunction("OK", 45.0, 50.0, 50.0, 55.0));
            speed.Add(new MembershipFunction("HIGH", 50.0, 55.0, 100.0, 100.0));
            myspeed = new LinguisticVariable("SPEED", speed);


            angle = new MembershipFunctionCollection();
            angle.Add(new MembershipFunction("DOWN", -10.0, -10.0, -5.0, 0.0));
            angle.Add(new MembershipFunction("LEVEL", -5.0, 1.0, 1.0, 5.0));
            angle.Add(new MembershipFunction("UP", 0.0, 5.0, 10.0, 10.0));
            myangle = new LinguisticVariable("ANGLE", angle);

            throttle = new MembershipFunctionCollection();
            throttle.Add(new MembershipFunction("LOW", 0.0, 0.0, 2.0, 4.0));
            throttle.Add(new MembershipFunction("LM", 2.0, 4.0, 4.0, 6.0));
            throttle.Add(new MembershipFunction("MED", 4.0, 6.0, 6.0, 8.0));
            throttle.Add(new MembershipFunction("HM", 6.0, 8.0, 8.0, 10.0));
            throttle.Add(new MembershipFunction("HIGH", 8.0, 10.0, 10.0, 10.0));
            mythrottle = new LinguisticVariable("THROTTLE", throttle);
            



        }

        public void setRules()
        {
            myrules = new FuzzyRuleCollection();
            myrules.Add(new FuzzyRule("IF (SPEED IS HIGH) AND (ANGLE IS UP) THEN THROTTLE IS LM"));
            myrules.Add(new FuzzyRule("IF (SPEED IS HIGH) AND (ANGLE IS LEVEL) THEN THROTTLE IS LM"));
            myrules.Add(new FuzzyRule("IF (SPEED IS HIGH) AND (ANGLE IS DOWN) THEN THROTTLE IS LOW"));
            myrules.Add(new FuzzyRule("IF (SPEED IS OK) AND (ANGLE IS UP) THEN THROTTLE IS HM"));
            myrules.Add(new FuzzyRule("IF (SPEED IS OK) AND (ANGLE IS LEVEL) THEN THROTTLE IS MED"));
            myrules.Add(new FuzzyRule("IF (SPEED IS OK) AND (ANGLE IS DOWN) THEN THROTTLE IS LM"));
            myrules.Add(new FuzzyRule("IF (SPEED IS LOW) AND (ANGLE IS UP) THEN THROTTLE IS HIGH"));
            myrules.Add(new FuzzyRule("IF (SPEED IS OK) AND (ANGLE IS LEVEL) THEN THROTTLE IS HM"));
            myrules.Add(new FuzzyRule("IF (SPEED IS OK) AND (ANGLE IS DOWN) THEN THROTTLE IS HM"));
        }
        public void setFuzzyEngine()
        {
            fe = new FuzzyEngine();
            fe.LinguisticVariableCollection.Add(myspeed);
            fe.LinguisticVariableCollection.Add(myangle);
            fe.LinguisticVariableCollection.Add(mythrottle);
            fe.FuzzyRuleCollection = myrules;
        }
        public void InitializeCharts()
        {
           
            // Sets properties for temperature chart
            Chart.Series[0].Name = "Chart";
            Chart.Series[0].Color = Color.Red;
            Chart.Series[0].ChartType = SeriesChartType.Line;
            Chart.Series[0].Points.AddY(0);
            Chart.ChartAreas[0].AxisY.Minimum = 0;
            Chart.ChartAreas[0].AxisY.Maximum = 60;
        }
            private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void defuziffyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setMembers();
            setRules();
            //setFuzzyEngine();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            setMembers();
            setRules();
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            canExit = true;
            Chart.Series[0].Points.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            InitializeCharts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            canExit = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    while (!canExit)
                    {
                        if (button1.Enabled) on();
                        Thread.Sleep(500);
                        
                    }
                }).Start();
                
            }
            finally
            {
                canExit = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }
        public void fuziffyvalues()
        {
            myspeed.InputValue = (Convert.ToDouble(textBox1.Text));
            myspeed.Fuzzify("LOW");
            myangle.InputValue = (Convert.ToDouble(textBox2.Text));
            myangle.Fuzzify("DOWN");
            myangle.InputValue = (Convert.ToDouble(textBox3.Text));
            myangle.Fuzzify("DOWN");

        }

        public void updateChart()
        {
            double throttle = Convert.ToDouble(textBox3.Text);
            // Updates chart for temperature
            Chart.Invoke((MethodInvoker)(() =>
            {
                if (Chart.Series[0].Points.Count > 25) Chart.Series[0].Points.RemoveAt(0);

                Chart.Series[0].Points.AddY(throttle);
                Chart.ChartAreas[0].AxisX.Minimum = Chart.Series[0].Points[0].XValue;
                Chart.ChartAreas[0].AxisX.Maximum = 25;
            }));
        }
            public void defuzzy()
        {
            fe.Consequent = "THROTTLE";
            textBox3.Invoke((MethodInvoker)(() =>
            {
                textBox3.Text = "" + fe.Defuzzify();
            }));
            
        }

        public void computenewspeed()
        {

            double oldspeed = Convert.ToDouble(textBox1.Text);
            double oldthrottle = Convert.ToDouble(textBox3.Text);
            double oldangle = Convert.ToDouble(textBox2.Text);
            double newspeed = ((1 - 0.1) * (oldspeed)) + (oldthrottle - (0.1 * oldangle));
            textBox1.Invoke((MethodInvoker)(() =>
            {
                textBox1.Text = "" + newspeed;
            }));
        }

        public void on()
        {
            fuziffyvalues();
            defuzzy();
            computenewspeed();
            updateChart();
        }
    }
}
