using System.Windows.Forms;

namespace Project_2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.TextAnnotation textAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.TextAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation2 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.TextAnnotation textAnnotation2 = new System.Windows.Forms.DataVisualization.Charting.TextAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation3 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation4 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation5 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation6 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation horizontalLineAnnotation7 = new System.Windows.Forms.DataVisualization.Charting.HorizontalLineAnnotation();
            System.Windows.Forms.DataVisualization.Charting.RectangleAnnotation rectangleAnnotation1 = new System.Windows.Forms.DataVisualization.Charting.RectangleAnnotation();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button_load = new System.Windows.Forms.Button();
            this.dateTimePicker_startDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_endDate = new System.Windows.Forms.DateTimePicker();
            this.aCandlestickBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.aCandlestick_datasource1 = new Project1_retry.datasource._aCandlestick_datasource();
            this.openFileDialog_load = new System.Windows.Forms.OpenFileDialog();
            this.chart_candlesticks = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_update = new System.Windows.Forms.Button();
            this.comboBox_pattern = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.percent_leeway = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_newX = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestick_datasource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlesticks)).BeginInit();
            this.SuspendLayout();
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(493, 635);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(126, 34);
            this.button_load.TabIndex = 0;
            this.button_load.Text = "Load Stock";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click_1);
            // 
            // dateTimePicker_startDate
            // 
            this.dateTimePicker_startDate.Location = new System.Drawing.Point(12, 635);
            this.dateTimePicker_startDate.Name = "dateTimePicker_startDate";
            this.dateTimePicker_startDate.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker_startDate.TabIndex = 1;
            this.dateTimePicker_startDate.Value = new System.DateTime(2024, 10, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_endDate
            // 
            this.dateTimePicker_endDate.Location = new System.Drawing.Point(1067, 638);
            this.dateTimePicker_endDate.Name = "dateTimePicker_endDate";
            this.dateTimePicker_endDate.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker_endDate.TabIndex = 2;
            // 
            // aCandlestickBindingSource1
            // 
            this.aCandlestickBindingSource1.DataMember = "aCandlestick";
            this.aCandlestickBindingSource1.DataSource = this.aCandlestick_datasource1;
            // 
            // aCandlestick_datasource1
            // 
            this.aCandlestick_datasource1.DataSetName = "aCandlestick.datasource";
            this.aCandlestick_datasource1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // openFileDialog_load
            // 
            this.openFileDialog_load.FileName = "openFileDialog_load";
            this.openFileDialog_load.Multiselect = true;
            // 
            // chart_candlesticks
            // 
            horizontalLineAnnotation1.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation1.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation1.IsInfinitive = true;
            horizontalLineAnnotation1.LineColor = System.Drawing.Color.ForestGreen;
            horizontalLineAnnotation1.LineWidth = 5;
            horizontalLineAnnotation1.Name = "peakLine";
            horizontalLineAnnotation1.YAxisName = "ChartArea_OHLC\\rY";
            textAnnotation1.AxisXName = "ChartArea_OHLC\\rX";
            textAnnotation1.ClipToChartArea = "ChartArea_OHLC";
            textAnnotation1.ForeColor = System.Drawing.Color.Green;
            textAnnotation1.Name = "annotationPeak";
            textAnnotation1.Text = "peak";
            textAnnotation1.YAxisName = "ChartArea_OHLC\\rY";
            horizontalLineAnnotation2.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation2.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation2.IsInfinitive = true;
            horizontalLineAnnotation2.LineColor = System.Drawing.Color.Red;
            horizontalLineAnnotation2.Name = "valleyLine";
            horizontalLineAnnotation2.YAxisName = "ChartArea_OHLC\\rY";
            textAnnotation2.AxisXName = "ChartArea_OHLC\\rX";
            textAnnotation2.ClipToChartArea = "ChartArea_OHLC";
            textAnnotation2.ForeColor = System.Drawing.Color.Red;
            textAnnotation2.Name = "annotationValley";
            textAnnotation2.Text = "valley";
            textAnnotation2.YAxisName = "ChartArea_OHLC\\rY";
            horizontalLineAnnotation3.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation3.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation3.IsInfinitive = true;
            horizontalLineAnnotation3.Name = "Fibonacci1";
            horizontalLineAnnotation3.YAxisName = "ChartArea_OHLC\\rY";
            horizontalLineAnnotation4.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation4.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation4.IsInfinitive = true;
            horizontalLineAnnotation4.Name = "Fibonacci2";
            horizontalLineAnnotation4.YAxisName = "ChartArea_OHLC\\rY";
            horizontalLineAnnotation5.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation5.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation5.IsInfinitive = true;
            horizontalLineAnnotation5.Name = "Fibonacci3";
            horizontalLineAnnotation5.YAxisName = "ChartArea_OHLC\\rY";
            horizontalLineAnnotation6.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation6.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation6.IsInfinitive = true;
            horizontalLineAnnotation6.Name = "Fibonacci4";
            horizontalLineAnnotation6.YAxisName = "ChartArea_OHLC\\rY";
            horizontalLineAnnotation7.AxisXName = "ChartArea_OHLC\\rX";
            horizontalLineAnnotation7.ClipToChartArea = "ChartArea_OHLC";
            horizontalLineAnnotation7.IsInfinitive = true;
            horizontalLineAnnotation7.Name = "Fibonacci5";
            horizontalLineAnnotation7.YAxisName = "ChartArea_OHLC\\rY";
            rectangleAnnotation1.AxisXName = "ChartArea_OHLC\\rX";
            rectangleAnnotation1.ClipToChartArea = "ChartArea_OHLC";
            rectangleAnnotation1.Name = "Rectangle_wave";
            rectangleAnnotation1.Text = "test";
            rectangleAnnotation1.YAxisName = "ChartArea_OHLC\\rY";
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation1);
            this.chart_candlesticks.Annotations.Add(textAnnotation1);
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation2);
            this.chart_candlesticks.Annotations.Add(textAnnotation2);
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation3);
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation4);
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation5);
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation6);
            this.chart_candlesticks.Annotations.Add(horizontalLineAnnotation7);
            this.chart_candlesticks.Annotations.Add(rectangleAnnotation1);
            chartArea1.AxisX.Title = "Date";
            chartArea1.AxisY.Title = "Price";
            chartArea1.Name = "ChartArea_OHLC";
            chartArea2.Name = "ChartArea_Beauty";
            this.chart_candlesticks.ChartAreas.Add(chartArea1);
            this.chart_candlesticks.ChartAreas.Add(chartArea2);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart_candlesticks.Legends.Add(legend1);
            this.chart_candlesticks.Location = new System.Drawing.Point(0, 3);
            this.chart_candlesticks.Name = "chart_candlesticks";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=Green";
            series1.IsXValueIndexed = true;
            series1.LabelBorderColor = System.Drawing.Color.Black;
            series1.Legend = "Legend1";
            series1.Name = "Series_OHLC";
            series1.XValueMember = "Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueMembers = "High, Low, Open, Close";
            series1.YValuesPerPoint = 4;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea_Beauty";
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.Name = "Series_Beauty";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart_candlesticks.Series.Add(series1);
            this.chart_candlesticks.Series.Add(series2);
            this.chart_candlesticks.Size = new System.Drawing.Size(1280, 626);
            this.chart_candlesticks.TabIndex = 4;
            this.chart_candlesticks.Text = "chart_candlesticks";
            // 
            // button_update
            // 
            this.button_update.Location = new System.Drawing.Point(625, 637);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(125, 32);
            this.button_update.TabIndex = 5;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click_1);
            // 
            // comboBox_pattern
            // 
            this.comboBox_pattern.AllowDrop = true;
            this.comboBox_pattern.FormattingEnabled = true;
            this.comboBox_pattern.Items.AddRange(new object[] {
            "Bullish",
            "Bearish",
            "Neutral",
            "Marubozu",
            "Hammer",
            "Doji",
            "Dragonfly doji",
            "Gravestone doji",
            "None"});
            this.comboBox_pattern.Location = new System.Drawing.Point(275, 640);
            this.comboBox_pattern.Name = "comboBox_pattern";
            this.comboBox_pattern.Size = new System.Drawing.Size(149, 28);
            this.comboBox_pattern.TabIndex = 6;
            this.comboBox_pattern.Text = "Display Pattern";
            this.comboBox_pattern.SelectedIndexChanged += new System.EventHandler(this.comboBox1_pattern_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Click 2 candlesticks to select wave ";
            // 
            // percent_leeway
            // 
            this.percent_leeway.Location = new System.Drawing.Point(999, 3);
            this.percent_leeway.Name = "percent_leeway";
            this.percent_leeway.Size = new System.Drawing.Size(160, 26);
            this.percent_leeway.TabIndex = 8;
            this.percent_leeway.Tag = "";
            this.percent_leeway.KeyDown += new System.Windows.Forms.KeyEventHandler(this.percent_leeway_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(793, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Enter leeway (and hit enter):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(393, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Pick a new low: (and hit enter)";
            // 
            // textBox_newX
            // 
            this.textBox_newX.Location = new System.Drawing.Point(617, 6);
            this.textBox_newX.Name = "textBox_newX";
            this.textBox_newX.Size = new System.Drawing.Size(100, 26);
            this.textBox_newX.TabIndex = 11;
            this.textBox_newX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_newX_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 680);
            this.Controls.Add(this.textBox_newX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.percent_leeway);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_pattern);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.chart_candlesticks);
            this.Controls.Add(this.dateTimePicker_endDate);
            this.Controls.Add(this.dateTimePicker_startDate);
            this.Controls.Add(this.button_load);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestick_datasource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlesticks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.DateTimePicker dateTimePicker_startDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_endDate;
        private System.Windows.Forms.OpenFileDialog openFileDialog_load;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_candlesticks;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.BindingSource aCandlestickBindingSource1;
        private Project1_retry.datasource._aCandlestick_datasource aCandlestick_datasource1;
        private System.Windows.Forms.ComboBox comboBox_pattern;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox percent_leeway;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_newX;
    }
}

