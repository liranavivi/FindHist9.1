using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace FindHist
{
    public partial class Form1 : Form
    {
        public class SymbolInfo
        {
            private DateTime m_date;
            private double m_close;
            private double m_open;
            private double m_high;
            private double m_low;
            private double m_change;
            public DateTime date
            {
                get { return m_date; }
                set { m_date = value; }
            }
            public double close
            {
                get { return m_close; }
                set { m_close = value; }
            }
            public double open
            {
                get { return m_open; }
                set { m_open = value; }
            }
            public double high
            {
                get { return m_high; }
                set { m_high = value; }
            }
            public double low
            {
                get { return m_low; }
                set { m_low = value; }
            }
            public double change
            {
                get { return m_change; }
                set { m_change = value; }
            }

            public override string ToString()
            {
                return m_date.ToString();
            }
        }
        PointPairList minGapDownC;
        PointPairList maxGapDownC;
        PointPairList minGapUpC;
        PointPairList maxGapUpC;
        PointPairList minNoGapC;
        PointPairList maxNoGapC;

        PointPairList minGapDownH;
        PointPairList maxGapDownH;
        PointPairList minGapUpH;
        PointPairList maxGapUpH;
        PointPairList minNoGapH;
        PointPairList maxNoGapH;

        PointPairList minGapDownL;
        PointPairList maxGapDownL;
        PointPairList minGapUpL;
        PointPairList maxGapUpL;
        PointPairList minNoGapL;
        PointPairList maxNoGapL;

        List<SymbolInfo> symbols = new List<SymbolInfo> { };

        int index = 1;

        public Form1()
        {
            InitializeComponent();

            zedGraphControl1.IsSynchronizeXAxes = true;
            zedGraphControl1.IsSynchronizeYAxes = false;

            zedGraphControl1.IsShowHScrollBar = true;
            zedGraphControl1.IsAutoScrollRange = true;

            MasterPane master = zedGraphControl1.MasterPane;
            master.PaneList.Clear();

            master.Title.IsVisible = false;
            master.Margin.All = 10;

            GraphPane pane1 = new GraphPane();
            GraphPane pane2 = new GraphPane();
            GraphPane pane3 = new GraphPane();

            GraphPane pane4 = new GraphPane();
            GraphPane pane5 = new GraphPane();
            GraphPane pane6 = new GraphPane();

            GraphPane pane7 = new GraphPane();
            GraphPane pane8 = new GraphPane();
            GraphPane pane9 = new GraphPane();

            master.Add(pane2);
            master.Add(pane5);
            master.Add(pane8);

            master.Add(pane3);
            master.Add(pane6);
            master.Add(pane9);

            master.Add(pane1);
            master.Add(pane4);
            master.Add(pane7);

            pane1.Title.Text = "GAP DOWN C";
            pane2.Title.Text = "GAP UP C";
            pane3.Title.Text = "NO GAP C";

            pane4.Title.Text = "GAP DOWN H";
            pane5.Title.Text = "GAP UP H";
            pane6.Title.Text = "NO GAP H";

            pane7.Title.Text = "GAP DOWN L";
            pane8.Title.Text = "GAP UP L";
            pane9.Title.Text = "NO GAP L";

            minGapDownC = new PointPairList();
            maxGapDownC = new PointPairList();
            minGapUpC = new PointPairList();
            maxGapUpC = new PointPairList();
            minNoGapC = new PointPairList();
            maxNoGapC = new PointPairList();

            minGapDownH = new PointPairList();
            maxGapDownH = new PointPairList();
            minGapUpH = new PointPairList();
            maxGapUpH = new PointPairList();
            minNoGapH = new PointPairList();
            maxNoGapH = new PointPairList();

            minGapDownL = new PointPairList();
            maxGapDownL = new PointPairList();
            minGapUpL = new PointPairList();
            maxGapUpL = new PointPairList();
            minNoGapL = new PointPairList();
            maxNoGapL = new PointPairList();

            pane1.AddCurve("min", minGapDownC, Color.Blue, SymbolType.None);
            pane1.AddCurve("max", maxGapDownC, Color.Red, SymbolType.None);
            pane2.AddCurve("min", minGapUpC, Color.Blue, SymbolType.None);
            pane2.AddCurve("max", maxGapUpC, Color.Red, SymbolType.None);
            pane3.AddCurve("min", minNoGapC, Color.Blue, SymbolType.None);
            pane3.AddCurve("max", maxNoGapC, Color.Red, SymbolType.None);

            pane4.AddCurve("min", minGapDownH, Color.Blue, SymbolType.None);
            pane4.AddCurve("max", maxGapDownH, Color.Red, SymbolType.None);
            pane5.AddCurve("min", minGapUpH, Color.Blue, SymbolType.None);
            pane5.AddCurve("max", maxGapUpH, Color.Red, SymbolType.None);
            pane6.AddCurve("min", minNoGapH, Color.Blue, SymbolType.None);
            pane6.AddCurve("max", maxNoGapH, Color.Red, SymbolType.None);

            pane7.AddCurve("min", minGapDownL, Color.Blue, SymbolType.None);
            pane7.AddCurve("max", maxGapDownL, Color.Red, SymbolType.None);
            pane8.AddCurve("min", minGapUpL, Color.Blue, SymbolType.None);
            pane8.AddCurve("max", maxGapUpL, Color.Red, SymbolType.None);
            pane9.AddCurve("min", minNoGapL, Color.Blue, SymbolType.None);
            pane9.AddCurve("max", maxNoGapL, Color.Red, SymbolType.None);

            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.AxisChange();
            using (Graphics g = this.CreateGraphics())
            {
                master.SetLayout(g, 3, 3);
            }


            index = (int)numericUpDown1.Value;
        }

        private async Task WaitAsynchronouslyAsyncUnderlying(string symbol)
        {
            symbol = symbol.ToUpper();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader stReader = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            List<string> lines = new List<string> { };

            MatchCollection matchCollection = null;
            string url;
            string line;
            try
            {
                DateTime startDateTime = DateTime.Now.AddMonths(-12*2);

                string URL = "https://api.tiingo.com/tiingo/daily/{0}/prices?startDate=" + startDateTime.ToString("yyyy-MM-dd") + "&token=1ccc4bc7eb148bea7480315aa025057f59b75375&resampleFreq=";

                Regex regex = new Regex("date\":\"(?<date>[0-9 /-]+)T00:00:00.000Z\"~\"close\":(?<close>[0-9 /.]*)~\"high\":(?<high>[0-9 /.]*)~\"low\":(?<low>[0-9 /.]*)~\"open\":(?<open>[0-9 /.]*)");

                List<Match> matches;

                if (weekradioButton.Checked)
                    url = string.Format(URL + "weekly", symbol);
                else
                    url = string.Format(URL + "daily", symbol);

                request = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                request.Timeout = 300000;
                response = (HttpWebResponse)(await request.GetResponseAsync());

                stReader = new StreamReader(response.GetResponseStream(), true);

                line = stReader.ReadLine();

                if (line != null)
                {
                    matchCollection = regex.Matches(line.Replace(",", "~"));
                    matches = new List<Match> { };
                    symbols.Clear();
                    SymbolInfo si;
                    foreach (Match match in matchCollection)
                    {
                        matches.Add(match);
                        si = new SymbolInfo();
                        si.close = double.Parse(match.Groups["close"].Value);
                        si.open = double.Parse(match.Groups["open"].Value);
                        si.date = DateTime.Parse(match.Groups["date"].Value);
                        si.high = double.Parse(match.Groups["high"].Value);
                        si.low = double.Parse(match.Groups["low"].Value);

                        symbols.Add(si);
                    }
                    CalculateHist();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public class d
        {
            public double diff;
            public double change;
            public double min;
            public double max;
        }

        private void CalculateHist()
        {
            double _max = Math.Max(symbols[symbols.Count - index].open, symbols[symbols.Count - index].close);
            double _min = Math.Min(symbols[symbols.Count - index].open, symbols[symbols.Count - index].close);

            if (index > 1)
            {
                string gap = "NO GAP";
                if (symbols[symbols.Count - index + 1].open > _max)
                    gap = "GAP UP";
                else if (symbols[symbols.Count - index + 1].open < _min)
                    gap = "GAP DOWN";

                label1.Text = string.Format("Date: {0} Open: {1} Close: {2} High: {3} Low: {4} ({5} High: {6} Low: {7} Close: {8})", symbols[symbols.Count - index].date.ToString("dd/MM/yyyy"), symbols[symbols.Count - index].open, symbols[symbols.Count - index].close, symbols[symbols.Count - index].high, symbols[symbols.Count - index].low, gap, symbols[symbols.Count - index + 1].high, symbols[symbols.Count - index + 1].low, symbols[symbols.Count - index + 1].close);
            }
            else
            {
                label1.Text = string.Format("Date: {0} Open: {1} Close: {2} High: {3} Low: {4}", symbols[symbols.Count - index].date.ToString("dd/MM/yyyy"), symbols[symbols.Count - index].open, symbols[symbols.Count - index].close, symbols[symbols.Count - index].high, symbols[symbols.Count - index].low);
            }
            minGapDownC.Clear();
            maxGapDownC.Clear();
            minGapUpC.Clear();
            maxGapUpC.Clear();
            minNoGapC.Clear();
            maxNoGapC.Clear();

            minGapDownH.Clear();
            maxGapDownH.Clear();
            minGapUpH.Clear();
            maxGapUpH.Clear();
            minNoGapH.Clear();
            maxNoGapH.Clear();

            minGapDownL.Clear();
            maxGapDownL.Clear();
            minGapUpL.Clear();
            maxGapUpL.Clear();
            minNoGapL.Clear();
            maxNoGapL.Clear();

            List<double> minGapDownDiffsC = new List<double> { };
            List<double> maxGapDownDiffsC = new List<double> { };

            List<double> minGapUpDiffsC = new List<double> { };
            List<double> maxGapUpDiffsC = new List<double> { };

            List<double> minNoGapDiffsC = new List<double> { };
            List<double> maxNoGapDiffsC = new List<double> { };

            List<double> minGapDownDiffsH = new List<double> { };
            List<double> maxGapDownDiffsH = new List<double> { };

            List<double> minGapUpDiffsH = new List<double> { };
            List<double> maxGapUpDiffsH = new List<double> { };

            List<double> minNoGapDiffsH = new List<double> { };
            List<double> maxNoGapDiffsH = new List<double> { };

            List<double> minGapDownDiffsL = new List<double> { };
            List<double> maxGapDownDiffsL = new List<double> { };

            List<double> minGapUpDiffsL = new List<double> { };
            List<double> maxGapUpDiffsL = new List<double> { };

            List<double> minNoGapDiffsL = new List<double> { };
            List<double> maxNoGapDiffsL = new List<double> { };

            for (int i = 1; i < symbols.Count - index + 2 - 1; i++)
            {
                double max = Math.Max(symbols[i - 1].open, symbols[i - 1].close);
                double min = Math.Min(symbols[i - 1].open, symbols[i - 1].close);

                #region GAP DOWN

                if (symbols[i].open < min)
                {
                    minGapDownDiffsC.Add(100 * (symbols[i].close - min) / min);
                    maxGapDownDiffsC.Add(100 * (symbols[i].close - max) / max);

                    minGapDownDiffsH.Add(100 * (symbols[i].high - min) / min);
                    maxGapDownDiffsH.Add(100 * (symbols[i].high - max) / max);

                    minGapDownDiffsL.Add(100 * (symbols[i].low - min) / min);
                    maxGapDownDiffsL.Add(100 * (symbols[i].low - max) / max);
                }

                #endregion

                #region GAP UP

                if (symbols[i].open > max)
                {
                    minGapUpDiffsC.Add(100 * (symbols[i].close - min) / min);
                    maxGapUpDiffsC.Add(100 * (symbols[i].close - max) / max);

                    minGapUpDiffsH.Add(100 * (symbols[i].high - min) / min);
                    maxGapUpDiffsH.Add(100 * (symbols[i].high - max) / max);

                    minGapUpDiffsL.Add(100 * (symbols[i].low - min) / min);
                    maxGapUpDiffsL.Add(100 * (symbols[i].low - max) / max);
                }

                #endregion

                #region NO GAP

                if (symbols[i].open >= min && symbols[i].open <= max)
                {
                    minNoGapDiffsC.Add(100 * (symbols[i].close - min) / min);
                    maxNoGapDiffsC.Add(100 * (symbols[i].close - max) / max);

                    minNoGapDiffsH.Add(100 * (symbols[i].high - min) / min);
                    maxNoGapDiffsH.Add(100 * (symbols[i].high - max) / max);

                    minNoGapDiffsL.Add(100 * (symbols[i].low - min) / min);
                    maxNoGapDiffsL.Add(100 * (symbols[i].low - max) / max);
                }

                #endregion
            }

            int successDiff;
            int failureDiff;
            string text;
            for (double p = -20; p < 20; p += 0.01)
            {
                #region GAP DOWN

                successDiff = minGapDownDiffsC.Count(d => d < p);
                failureDiff = minGapDownDiffsC.Count(d => d >= p);
                minGapDownC.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minGapDownC[minGapDownC.Count - 1].Tag = p;

                text = string.Empty;
                foreach (double item in minGapDownDiffsC)
                {
                    text += (item < p ? "1" : "0");
                }
                minGapDownC[minGapDownC.Count - 1].Tag = text;

                successDiff = maxGapDownDiffsC.Count(d => d < p);
                failureDiff = maxGapDownDiffsC.Count(d => d >= p);
                maxGapDownC.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxGapDownC[maxGapDownC.Count - 1].Tag = p;

                text = string.Empty;
                foreach (double item in maxGapDownDiffsC)
                {
                    text += (item < p ? "1" : "0");
                }
                maxGapDownC[maxGapDownC.Count - 1].Tag = text;

                successDiff = minGapDownDiffsH.Count(d => d < p);
                failureDiff = minGapDownDiffsH.Count(d => d >= p);
                minGapDownH.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minGapDownH[minGapDownH.Count - 1].Tag = p;

                successDiff = maxGapDownDiffsH.Count(d => d < p);
                failureDiff = maxGapDownDiffsH.Count(d => d >= p);
                maxGapDownH.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxGapDownH[maxGapDownH.Count - 1].Tag = p;

                successDiff = minGapDownDiffsL.Count(d => d < p);
                failureDiff = minGapDownDiffsL.Count(d => d >= p);
                minGapDownL.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minGapDownL[minGapDownL.Count - 1].Tag = p;

                successDiff = maxGapDownDiffsL.Count(d => d < p);
                failureDiff = maxGapDownDiffsL.Count(d => d >= p);
                maxGapDownL.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxGapDownL[maxGapDownL.Count - 1].Tag = p;

                #endregion

                #region GAP UP

                successDiff = minGapUpDiffsC.Count(d => d < p);
                failureDiff = minGapUpDiffsC.Count(d => d >= p);
                minGapUpC.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minGapUpC[minGapUpC.Count - 1].Tag = p;

                text = string.Empty;
                foreach (double item in minGapUpDiffsC)
                {
                    text += (item < p ? "1" : "0");
                }
                minGapUpC[minGapUpC.Count - 1].Tag = text;


                successDiff = maxGapUpDiffsC.Count(d => d < p);
                failureDiff = maxGapUpDiffsC.Count(d => d >= p);
                maxGapUpC.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxGapUpC[maxGapUpC.Count - 1].Tag = p;

                text = string.Empty;
                foreach (double item in maxGapUpDiffsC)
                {
                    text += (item < p ? "1" : "0");
                }
                maxGapUpC[maxGapUpC.Count - 1].Tag = text;

                successDiff = minGapUpDiffsH.Count(d => d < p);
                failureDiff = minGapUpDiffsH.Count(d => d >= p);
                minGapUpH.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minGapUpH[minGapUpH.Count - 1].Tag = p;

                successDiff = maxGapUpDiffsH.Count(d => d < p);
                failureDiff = maxGapUpDiffsH.Count(d => d >= p);
                maxGapUpH.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxGapUpH[maxGapUpH.Count - 1].Tag = p;

                successDiff = minGapUpDiffsL.Count(d => d < p);
                failureDiff = minGapUpDiffsL.Count(d => d >= p);
                minGapUpL.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minGapUpL[minGapUpL.Count - 1].Tag = p;

                successDiff = maxGapUpDiffsL.Count(d => d < p);
                failureDiff = maxGapUpDiffsL.Count(d => d >= p);
                maxGapUpL.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxGapUpL[maxGapUpL.Count - 1].Tag = p;

                #endregion

                #region NO GAP

                successDiff = minNoGapDiffsC.Count(d => d < p);
                failureDiff = minNoGapDiffsC.Count(d => d >= p);
                minNoGapC.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minNoGapC[minNoGapC.Count - 1].Tag = p;

                text = string.Empty;
                foreach (double item in minNoGapDiffsC)
                {
                    text += (item < p ? "1" : "0");
                }
                minNoGapC[minNoGapC.Count - 1].Tag = text;


                successDiff = maxNoGapDiffsC.Count(d => d < p);
                failureDiff = maxNoGapDiffsC.Count(d => d >= p);
                maxNoGapC.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxNoGapC[maxNoGapC.Count - 1].Tag = p;

                text = string.Empty;
                foreach (double item in maxNoGapDiffsC)
                {
                    text += (item < p ? "1" : "0");
                }
                maxNoGapC[maxNoGapC.Count - 1].Tag = text;

                successDiff = minNoGapDiffsH.Count(d => d < p);
                failureDiff = minNoGapDiffsH.Count(d => d >= p);
                minNoGapH.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minNoGapH[minNoGapH.Count - 1].Tag = p;

                successDiff = maxNoGapDiffsH.Count(d => d < p);
                failureDiff = maxNoGapDiffsH.Count(d => d >= p);
                maxNoGapH.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxNoGapH[maxNoGapH.Count - 1].Tag = p;

                successDiff = minNoGapDiffsL.Count(d => d < p);
                failureDiff = minNoGapDiffsL.Count(d => d >= p);
                minNoGapL.Add(_min * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                minNoGapL[minNoGapL.Count - 1].Tag = p;

                successDiff = maxNoGapDiffsL.Count(d => d < p);
                failureDiff = maxNoGapDiffsL.Count(d => d >= p);
                maxNoGapL.Add(_max * (1 + p / 100), 100 * successDiff / (failureDiff + successDiff));
                maxNoGapL[maxNoGapL.Count - 1].Tag = p;

                #endregion
            }

            zedGraphControl1.RestoreScale(zedGraphControl1.GraphPane);
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            //await WaitAsynchronouslyAsyncUnderlying("IWM");
            if (textBox1.Text.Trim() != string.Empty)
                await WaitAsynchronouslyAsyncUnderlying(textBox1.Text.Trim());
            //await WaitAsynchronouslyAsyncUnderlying("DIA");
            //await WaitAsynchronouslyAsyncUnderlying("TSLA");
            //await WaitAsynchronouslyAsyncUnderlying("AAPL");
        }

        private string zedGraphControl1_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair p = curve[iPt];

            //return string.Format("{0:F2}% chance it will be smaller than {1:F2}% of change from {2} price", p.Y, p.X, curve.Label.Text);
            return string.Format("{0:F2}% chance that it will be lower than {1:F2} ({2:F2}% from {3} price)", p.Y, p.X, p.Tag, curve.Label.Text);
            //return string.Format("{0} {1}", p.Y, p.X);
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            index = (int)numericUpDown1.Value;
            CalculateHist();
        }

        private async void weekradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != string.Empty)
                await WaitAsynchronouslyAsyncUnderlying(textBox1.Text.Trim());
        }

        private async void dayradioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != string.Empty)
                await WaitAsynchronouslyAsyncUnderlying(textBox1.Text.Trim());
        }


    }
}
