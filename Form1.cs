using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Project_2
{
    public partial class Form1 : Form
    {
        // Declare variables used in multiple functions
        private List<aCandlestick> allCandlesticks;
        private string csvFilePath;
        private DateTime startDate;
        private DateTime endDate;
        private bool userSelectedStartDate = false;
        private static bool initialFormLoaded = false; // Tracks if the initial form has already loaded a CSV file

        // Initialize the components of the form
        public Form1()
        {
            InitializeComponent();
            InitializeOpenFileDialog();
            InitializeDateTimePickers();
            InitializeChartClickEvent(); // Allow the user to click candlesticks and create a wave
        }

        /// <summary>
        /// Initializes the open file dialog that is used when the button is clicked so that 
        /// only CSV files are displayed in the file explorer for selection.
        /// </summary>
        private void InitializeOpenFileDialog()
        {
            openFileDialog_load = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Open CSV File",
                Multiselect = true // Enable multiple file selection
            };
            openFileDialog_load.FileOk += new CancelEventHandler(openFileDialog_load_FileOk);
        }

        /// <summary>
        /// Initializes the date time pickers so that the user can specify which dates they want to start
        /// and end their data from. It allows the stock data to display dates of your choosing.
        /// </summary>
        private void InitializeDateTimePickers()
        {
            this.Controls.Add(dateTimePicker_startDate);
            this.Controls.Add(dateTimePicker_endDate);

            dateTimePicker_startDate.ValueChanged += (sender, e) => userSelectedStartDate = true;
        }

        /// <summary>
        /// Loads data from CSV and checks if a new form is needed.
        /// </summary>
        private void openFileDialog_load_FileOk(object sender, CancelEventArgs e)
        {
            csvFilePath = openFileDialog_load.FileName;

            if (!initialFormLoaded) // Uses bool to check if there is already a form loaded in the original window
            {
                Text = openFileDialog_load.FileName; // Load the CSV data in the initial form if OG window is still blank
                LoadCsvData(csvFilePath);   // Loads the first data set
                initialFormLoaded = true;   // Updates the bool to mark the initial form as having loaded data

                for (int i = 1; i < openFileDialog_load.FileNames.Length; i++) // Loads the rest of the data
                {
                    string fileName = openFileDialog_load.FileNames[i]; // Skips the first file since it was just loaded
                    Form1 newForm = new Form1();        // Makes new form
                    newForm.Text = fileName;            // Sets the title of the form to the filepath
                    newForm.LoadCsvData(fileName);      // Loads the data in the new CSV file
                    // Force chart to repaint after all data and annotations are added
                    chart_candlesticks.Invalidate();
                    chart_candlesticks.Update();
                    newForm.Show();
                }
            }
            else // If the original window already has stock data
            {
                foreach (string filePath in openFileDialog_load.FileNames) // If multiple files selected load each one at a time
                {
                    Form1 newForm = new Form1();        // Makes new form
                    newForm.Text = filePath;            // Sets the title of the form to the filepath
                    newForm.LoadCsvData(filePath);      // Loads the data in the new CSV file
                    // Force chart to repaint after all data and annotations are added
                    chart_candlesticks.Invalidate();
                    chart_candlesticks.Update();
                    newForm.Show();
                }
            }
        }

        /// <summary>
        /// Loads the data from the specified CSV file path and displays it.
        /// </summary>
        private void LoadCsvData(string filePath)
        {
            aCandlestickLoader loader = new aCandlestickLoader();
            allCandlesticks = loader.LoadFromCsv(filePath);

            allCandlesticks = allCandlesticks.OrderBy(candlestick => candlestick.Date).ToList();

            if (allCandlesticks.Any() && !userSelectedStartDate)
            {
                startDate = allCandlesticks.First().Date;
                dateTimePicker_startDate.Value = startDate;
            }

            FilterAndBindCandlesticks();
        }

        public double currentPeak = 0;
        public double currentValley = 10000000;
        /// <summary>
        /// Checks the high of each candlestick and saves it as the peak if it is higher than all other candlesticks
        /// </summary>
        /// <param name="index"></param>
        /// the index of the candlestick in a list of datapoints on the chart
        /// <param name="high"></param>
        /// the value of the high of the candlestick to see if it is the peak
        private void AddPeakAnnotation(int index, double high)
        {
            if (high < currentPeak) return; // If the current peak is higher than the new high, don't do anything
            currentPeak = high; // Otherwise, set the new current peak = the new high

            // Add an anchor point for the peak text annotation
            chart_candlesticks.Annotations["annotationPeak"].AnchorDataPoint = chart_candlesticks.Series["Series_OHLC"].Points[index];

            // Adds values for the peak line annotation
            // Makes the line width 1 and sets the axis to the y value of the peak
            chart_candlesticks.Annotations["peakLine"].LineWidth = 1;
            chart_candlesticks.Annotations["peakLine"].Y = high;

            chart_candlesticks.Invalidate();        // Makes sure the chart is updated so the annotations show
            chart_candlesticks.Update();
        }

        /// <summary>
        /// Checks the low of each candlestick and saves it as the valley if it is lower than all other candlesticks
        /// </summary>
        /// <param name="index"></param>
        /// the index of the candlestick in a list of datapoints on the chart
        /// <param name="low"></param>
        /// the value of the low of the candlestick to see if it is the valley
        private void AddValleyAnnotation(int index, double low)
        {
            if (low > currentValley) return; // If current valley is lower than the new low, don't do anything
            currentValley = low; // Otherwise, set the new current valley = the new low

            // Add an anchor point for the valley text annotation
            chart_candlesticks.Annotations["annotationValley"].AnchorDataPoint = chart_candlesticks.Series["Series_OHLC"].Points[index];

            // Adds values for the valley line annotation
            // Makes the line width 1 and sets the axis to the y value of the valley
            chart_candlesticks.Annotations["valleyLine"].LineWidth = 1;
            chart_candlesticks.Annotations["valleyLine"].Y = low;

            chart_candlesticks.Invalidate();        // Makes sure the chart is updated so the annotations show
            chart_candlesticks.Update();
        }

        /// <summary>
        /// Takes the start and end date that the user specifies and filters candlesticks to bind to the chart.
        /// It also finds the peaks and valleys in the data and creates annotations for each of them. Finally,
        /// it normalizes the chart by finding the min and max x and y for the dataset.
        /// </summary>
        private void FilterAndBindCandlesticks()
        {
            currentPeak = 0;
            currentValley = 10000000;

            startDate = dateTimePicker_startDate.Value;
            endDate = dateTimePicker_endDate.Value;

            // Filter candlesticks by date range 
            List<aCandlestick> filteredCandlesticks = filterCandlesticks(allCandlesticks, startDate, endDate);

            // Convert filtered candlesticks to aSmartCandlestick and find pattern type
            List<aSmartCandlestick> smartFilteredCandlesticks = filteredCandlesticks
                .Select(c => new aSmartCandlestick(c.Date, c.Open, c.High, c.Low, c.Close, c.Volume)) // Adds each existing value
                .ToList();

            aCandlestickBindingSource1.DataSource = smartFilteredCandlesticks;
            chart_candlesticks.DataSource = aCandlestickBindingSource1;
            chart_candlesticks.DataBind();

            // Identify peaks and valleys
            // Check the first candlestick separately to add its annotation if it's a peak or valley.
            if (smartFilteredCandlesticks.Count > 1)
            {
                var first = smartFilteredCandlesticks[0];   // First candlestick
                var second = smartFilteredCandlesticks[1];  // Seconc candlestick

                if (first.High > second.High) // First candlestick is a peak
                {
                    AddPeakAnnotation(0, first.High);   // Add it to the chart
                }
                if (first.Low < second.Low) // First candlestick is a valley
                {
                    AddValleyAnnotation(0, first.Low);  // Add it to the chart
                }
            }

            // Iterate through the middle candlesticks
            for (int i = 1; i < smartFilteredCandlesticks.Count - 1; i++)
            {
                var previous = smartFilteredCandlesticks[i - 1];    // Previous candlestick to compare
                var current = smartFilteredCandlesticks[i];         // Current candlestick being compared
                var next = smartFilteredCandlesticks[i + 1];        // Next candlestick to compare

                // Check for peak condition
                if (current.High > previous.High && current.High > next.High)   // If it is the highest of the 3
                {
                    AddPeakAnnotation(i, current.High);     // Add it to the chart
                }
                // Check for valley condition 
                if (current.Low < previous.Low && current.Low < next.Low)       // If it is the lowest of the 3
                {
                    AddValleyAnnotation(i, current.Low);    // Add it to the chart
                }
            }

            // Check the last candlestick for a peak or valley
            if (smartFilteredCandlesticks.Count > 1)
            {
                var last = smartFilteredCandlesticks[smartFilteredCandlesticks.Count - 1];  // Last candlestick in the data
                var secondLast = smartFilteredCandlesticks[smartFilteredCandlesticks.Count - 2];    // Second to last one

                if (last.High > secondLast.High)    // If the last is the highest
                {
                    AddPeakAnnotation(smartFilteredCandlesticks.Count - 1, last.High);  // Add it to the chart
                }
                else if (last.Low < secondLast.Low) // If the last is the lowest
                {
                    AddValleyAnnotation(smartFilteredCandlesticks.Count - 1, last.Low); // Add it to the chart
                }
            }

            if (smartFilteredCandlesticks.Any()) // Normalizes the chart
            {
                double minY = Math.Floor(0.98 * (double)smartFilteredCandlesticks.Min(cs => cs.Low));
                double maxY = Math.Ceiling(1.02 * (double)smartFilteredCandlesticks.Max(cs => cs.High));

                chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = minY;
                chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = maxY + 5; // Give room for the peak annotation when it's really high
            }
        }

        /// <summary>
        /// Filters the candlesticks by the selected date range.
        /// </summary>
        public static List<aCandlestick> filterCandlesticks(List<aCandlestick> candlesticks, DateTime startDate, DateTime endDate)
        {
            var filteredCandlesticks = new List<aCandlestick>();
            foreach (var candlestick in candlesticks)
            {
                if (candlestick.Date >= startDate && candlestick.Date <= endDate)
                {
                    filteredCandlesticks.Add(candlestick);
                }
                if (candlestick.Date >= endDate)
                    break;
            }
            return filteredCandlesticks;
        }

        private void button_load_Click_1(object sender, EventArgs e)
        {
            openFileDialog_load.ShowDialog();
        }

        private void button_update_Click_1(object sender, EventArgs e)
        {

            FilterAndBindCandlesticks();
            // Clear the previous selections
            levels.Clear(); // Clear the old list of fib levels
            selectedTopIndex = -1; // Reset selected indices
            selectedBottomIndex = -1;
            isUpwardWave = false; // Reset upward wave 
            foreach (var annotation in chart_candlesticks.Annotations.Cast<Annotation>()) // Make annotations not visible until wave is reselected
            {
                if (annotation.Name.StartsWith("Fibonacci"))
                {
                    annotation.Visible = false;
                }
            }

            // Delete rectangle
            var rectangleWave = chart_candlesticks.Annotations
                .FirstOrDefault(a => a.Name == "Rectangle_wave") as RectangleAnnotation;

            chart_candlesticks.Annotations.Remove(rectangleWave);

        }

        /// <summary>
        /// This function is for when the user changes their selection of the drop down
        /// menu and selects a new pattern to highlight on the chart. It reads the choice
        /// that the user made and then calls the highlight function to highlight candlesticks
        /// of this pattern.
        /// </summary>
        private void comboBox1_pattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected pattern
            string selectedPattern = comboBox_pattern.SelectedItem.ToString();

            // Filter the candlesticks based on the selected pattern
            HighlightCandlesticksBasedOnPattern(selectedPattern);
        }

        /// <summary>
        /// This function takes the pattern that the user wants to highlight and iterates
        /// through each of the candlesticks in the dataset to decide which ones are of
        /// this pattern type. It then calls the highlight function to change the color of
        /// the candlestick if it does match the pattern.
        /// </summary>
        /// <param name="pattern"></param> 
        /// This parameter is the pattern that the user selected from the drop down menu.
        private void HighlightCandlesticksBasedOnPattern(string pattern)
        {
            // Filter candlesticks by date range 
            List<aCandlestick> filteredCandlesticks = filterCandlesticks(allCandlesticks, startDate, endDate);

            // Convert filtered candlesticks to aSmartCandlestick and find pattern type
            List<aSmartCandlestick> smartFilteredCandlesticks = filteredCandlesticks
                .Select(c => new aSmartCandlestick(c.Date, c.Open, c.High, c.Low, c.Close, c.Volume)) // Adds each existing value
                .ToList();

            // Reset all border colors to white before applying new highlight
            for (int i = 0; i < filteredCandlesticks.Count; i++)
            {
                var dataPoint = chart_candlesticks.Series["Series_OHLC"].Points[i];
                dataPoint.BorderColor = default;
                dataPoint.BorderWidth = 1;
            }

            // Iterates through all of the candlesticks and determines it's pattern
            for (int i = 0; i < filteredCandlesticks.Count; i++)
            {
                aSmartCandlestick candlestick = smartFilteredCandlesticks[i];

                bool matchPattern = false;
                
                // Switch statement to check if a candlesticks is a certain pattern based on the input
                switch (pattern)
                {
                    case "Bullish":
                        matchPattern = candlestick.IsBullish;
                        break;
                    case "Bearish":
                        matchPattern = candlestick.IsBearish;
                        break;
                    case "Neutral":
                        matchPattern = candlestick.IsHammer;
                        break;
                    case "Marubozu":
                        matchPattern = candlestick.IsDoji;
                        break;
                    case "Hammer":
                        matchPattern = candlestick.IsMarubozu;
                        break;
                    case "Doji":
                        matchPattern = candlestick.IsDoji;
                        break;
                    case "Dragonfly doji":
                        matchPattern = candlestick.IsDragonflyDoji;
                        break;
                    case "Gravestone doji":
                        matchPattern = candlestick.IsGravestoneDoji;
                        break;
                    case "None":
                        // Reset everything
                        for (int j = 0; j < filteredCandlesticks.Count; j++)
                        {
                            var dataPoint = chart_candlesticks.Series["Series_OHLC"].Points[j];
                            dataPoint.BorderColor = default;
                            dataPoint.BorderWidth = 1;
                        }
                        matchPattern = false;
                        break;
                }

                // If a match is found, highlight the candlestick
                if (matchPattern)
                {
                    HighlightCandlestick(i, candlestick);
                }
            }

            // Rebind and refresh the chart to apply changes
            chart_candlesticks.Invalidate();
            chart_candlesticks.Update();
        }

        /// <summary>
        /// This function takes the index of the candlestick that matches the chosen pattern
        /// type and highlights it blue on the chart so that the user can see that it matches
        /// the chosen pattern.
        /// </summary>
        /// <param name="index"></param>
        /// This parameter is the index of the candlestick that matches the pattern.
        /// <param name="candlestick"></param>
        /// This parameter is the specific candlestick at that index that will be highlighted.
        private void HighlightCandlestick(int index, aSmartCandlestick candlestick)
        {
            // Example: Change the color of the candlestick based on its pattern
            var dataPoint = chart_candlesticks.Series["Series_OHLC"].Points[index];

            // Set a larger border size so it is easier to see the highlight
            dataPoint.BorderWidth = 3;

            // Highlight the candlesticks by making them blue
            if (candlestick.IsBullish)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsBearish)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsNeutral)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsMarubozu)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsHammer)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsDoji)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsDragonflyDoji)
            {
                dataPoint.BorderColor = Color.Blue;
            }
            else if (candlestick.IsGravestoneDoji)
            {
                dataPoint.BorderColor = Color.Blue;
            }
        }
 
        // Declare global variables to store the selected candlestick indices
        private int selectedTopIndex = -1;
        private int selectedBottomIndex = -1;

        /// <summary>
        /// 
        /// </summary>
        private void InitializeChartClickEvent()
        {
            chart_candlesticks.MouseClick += Chart_candlesticks_MouseClick;
        }

        /// <summary>
        /// This function is called whenever the user clicks the candlestick chart, allowing them to
        /// click two candlesticks and select a wave, and then verifies this wave and uses the indices
        /// to call the function to calculate the fibonacci levels. It also highlights the selected 
        /// candlesticks.
        /// </summary>
        private void Chart_candlesticks_MouseClick(object sender, MouseEventArgs e)
        {
            // Get the clicked point on the chart
            HitTestResult result = chart_candlesticks.HitTest(e.X, e.Y);

            // Check if a data point (candlestick) was clicked
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                int clickedIndex = result.PointIndex;

                // If no peak/valley has been selected yet
                if (selectedTopIndex == -1)
                {
                    // Check if the clicked candlestick is a peak or valley
                    if (IsPeakOrValley(clickedIndex))
                    {
                        selectedTopIndex = clickedIndex;
                        HighlightCandlestickWave(clickedIndex); // Highlight the peak/valley
                    }
                    else
                    {
                        MessageBox.Show("Invalid. Please click on a peak or valley.");
                    }
                }
                // If the peak/valley is already selected, select the bottom of the wave
                else if (selectedBottomIndex == -1)
                {
                    // Ensure the second candlestick is after the first one
                    if (clickedIndex > selectedTopIndex)
                    {
                        selectedBottomIndex = clickedIndex;
                        HighlightCandlestickWave(clickedIndex); // Highlight as the bottom of the wave
                        ComputeFibonacciLevels(); // Compute levels after valid selection
                    }
                    else
                    {
                        MessageBox.Show("Invalid. Please select a candlestick after the peak/valley.");
                    }
                }
            }
        }

        /// <summary>
        /// Creates a rectangle around the selected wave
        /// </summary>
        /// <summary>
        /// Creates a rectangle annotation around the selected wave using the indices of the selected top and bottom points.
        /// </summary>
        private void DrawWaveRectangle()
        {
            // Ensure valid indices are selected
            if (selectedTopIndex == -1 || selectedBottomIndex == -1)
            {
                MessageBox.Show("Please select a valid wave before drawing the rectangle.");
                return;
            }

            // Get the DataPoints for the selected top and bottom indices
            var topPoint = chart_candlesticks.Series["Series_OHLC"].Points[selectedTopIndex];
            var bottomPoint = chart_candlesticks.Series["Series_OHLC"].Points[selectedBottomIndex];

            // Calculate the X and Y bounds for the rectangle
            double xMin = topPoint.XValue;
            double xMax = bottomPoint.XValue;
            double yMax = Math.Max(topPoint.YValues[1], bottomPoint.YValues[1]); // High values
            double yMin = Math.Min(topPoint.YValues[2], bottomPoint.YValues[2]); // Low values

            // Check if an annotation with the name "Rectangle_wave" already exists and remove it
            var existingRectangle = chart_candlesticks.Annotations
                .FirstOrDefault(a => a.Name == "Rectangle_wave") as RectangleAnnotation;
            if (existingRectangle != null)
            {
                chart_candlesticks.Annotations.Remove(existingRectangle);
            }

            // Create a new RectangleAnnotation
            var rectangleWave = new RectangleAnnotation
            {
                Name = "Rectangle_wave",
                AxisX = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisX,
                AxisY = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY,
                X = xMin,
                Y = yMax,
                Width = xMax - xMin,
                Height = yMax - yMin,
                LineColor = Color.Red,
                LineWidth = 2,
                BackColor = Color.FromArgb(50, Color.Yellow), // Semi-transparent fill color
                ClipToChartArea = "ChartArea_OHLC" // Ensure the rectangle stays within the chart area
            };

            // Add the annotation to the chart
            chart_candlesticks.Annotations.Add(rectangleWave);

            // Refresh the chart to display the rectangle
            chart_candlesticks.Invalidate();
            chart_candlesticks.Update();
        }

        /// <summary>
        /// This function determines if the selected candlestick is a peak or valley, as the first candlestick
        /// of the wave is required to be a peak or a valley candlestick.
        /// </summary>
        /// <param name="index"></param>
        /// The index parameter is the index of the selected candlesticks that the user clicks.
        /// <returns></returns>
        private bool IsPeakOrValley(int index)
        {
            // Check if the clicked candlestick has an annotation (peak or valley)
            bool isPeak = chart_candlesticks.Annotations.Cast<Annotation>()
                .Any(annotation => annotation.AnchorDataPoint == chart_candlesticks.Series["Series_OHLC"].Points[index] && annotation.Name == "annotationPeak");

            bool isValley = chart_candlesticks.Annotations.Cast<Annotation>()
                .Any(annotation => annotation.AnchorDataPoint == chart_candlesticks.Series["Series_OHLC"].Points[index] && annotation.Name == "annotationValley");

            return isPeak || isValley;
        }

        // Function to highlight the candlestick
        private void HighlightCandlestickWave(int index)
        {
            var dataPoint = chart_candlesticks.Series["Series_OHLC"].Points[index];
            dataPoint.BorderColor = Color.Purple;
            dataPoint.BorderWidth = 3;
        }

        // Function to remove the highlight
        private void RemoveHighlight()
        {
            foreach (var point in chart_candlesticks.Series["Series_OHLC"].Points)
            {
                point.BorderColor = Color.Empty; // Remove border color
                point.BorderWidth = 1; // Reset border width
            }
        }

        // Fibonacci ratios to compute
        private readonly double[] fibonacciRatios = { 0.236, 0.382, 0.5, 0.618, 0.786 };
        // Stores new fib levels
        List<double> levels = new List<double>();

        /// <summary>
        /// This function calculated the fibonacci levels using the given ratios and the high and low
        /// values of the wave. It uses the formula high - ((high-low) * ratio).
        /// </summary>
        /// <param name="high"></param>
        /// This is the value of the first candlestick of the wave
        /// <param name="low"></param>
        /// This is the value of the second candlestick of the wave
        /// <returns></returns>
        private List<double> GetFibonacciLevels(double high, double low)
        {
            // Clears old fib levels so it can calculate new ones each time the function is called
            levels.Clear();
            double range = high - low;

            // Makes sure to calculate all 5 fib levels
            foreach (var ratio in fibonacciRatios)
            {
                levels.Add(high - (range * ratio));
            }
            return levels;
        }

        private bool isUpwardWave = false;   // Tracks if it is an upward or downward wave

        /// <summary>
        /// This function sets the correct high and low values based on the indices that the user picks
        /// for the wave and then calls the fibonacci function and uses the results to set the lines of
        /// the fibonacci levels on the chart and updates the graph. It then calls function to calculate
        /// the beauty values of each candlestick / the whole wave. Finally, it calls more funcitons to show
        /// all of these results in the charts that are needed.
        /// </summary>
        private void ComputeFibonacciLevels()
        {
            // Ensure valid wave selection
            if (selectedTopIndex == -1 || selectedBottomIndex == -1)
            {
                MessageBox.Show("Please select a valid wave before computing Fibonacci levels.");
                return;
            }

            // Get the high and low values of the wave
            double high = chart_candlesticks.Series["Series_OHLC"].Points[(int)selectedTopIndex].YValues[1]; // High value
            double low = chart_candlesticks.Series["Series_OHLC"].Points[(int)selectedBottomIndex].YValues[2]; // Low value

            // Ensure that the high and low are in correct order
            // Determine wave direction (upward or downward)
            isUpwardWave = low > high;

            // Ensure high and low are ordered correctly for Fibonacci calculation
            if (isUpwardWave)
            {
                double temp = high;
                high = low;
                low = temp;
            }

            // Calculate the fibLevels
            List<double> fibLevels = GetFibonacciLevels(high, low);

            // Draw Rectangle around wave
            DrawWaveRectangle();

            // Go through each fibonacci annotation and calculate the levels
            int i = 0;
            foreach (var annotation in chart_candlesticks.Annotations.Cast<Annotation>())
            {
                if (annotation.Name.StartsWith("Fibonacci"))
                {
                    double fibLevel = fibLevels[i];
                    // Update properties of the Fibonacci annotation
                    annotation.Y = fibLevel;
                    annotation.AnchorY = fibLevel;
                    annotation.Visible = true;
                    levels.Add(fibLevel);   // Adds to the list of fib levels to be used to calc Beauty
                    // Set the position of the annotation
                    i++;
                }
            }

            chart_candlesticks.Invalidate(); // Redraw chart to show annotations
            chart_candlesticks.Update();

            // Extract candlesticks in the wave
            List<DataPoint> waveCandlesticks = chart_candlesticks.Series["Series_OHLC"]
                .Points.Skip((int)selectedTopIndex)
                .Take((int)selectedBottomIndex - (int)selectedTopIndex + 1)
                .ToList();

            // Remove previous confirmation points
            confirmationPoints.Clear();

            // Compute total wave beauty
            int waveBeauty = ComputeWaveBeauty(waveCandlesticks, levels);
            MessageBox.Show($"Wave with high {high} and low {low} has a Beauty of: {waveBeauty}");

            // Add annotations to show each confirmation on the chart
            CreateAnnotationsFromConfirmationPoints();

            // Compute Beauty as a function of price
            var beautyByPrice = ComputeBeautyByPrice(waveCandlesticks, high, low, levels);

            // Plot the results
            PlotBeauty(beautyByPrice);

            // Compute Beauty as the wave extends downwards by 25% of its original height
            double extensionPercentage = 0.25; // Extend by 25%
            int steps = 50; // Number of steps for smoother plot
            Dictionary<double, int> extendedBeautyByPrice = ComputeExtendedWaveBeauty(waveCandlesticks, high, low, extensionPercentage, steps);

            // Plot the results
            PlotBeautyVsPrice(extendedBeautyByPrice);
        }

        // Keeps track of all the confirmations the candlesticks have with OHLC values
        private List<Tuple<double, double>> confirmationPoints = new List<Tuple<double, double>>();

        /// <summary>
        /// This function calculates the beauty of each candlestick using the given leeway value
        /// by comparing each candlesticks OHLC values to the fibonacci levels and seeing if there
        /// is any values close enough to hit the lines.
        /// </summary>
        /// <param name="candlestick"></param>
        /// Takes in the data point that the candlestick is located on.
        /// <param name="fibonacciLevels"></param>
        /// Takes all 5 fib levels to compare to the OHLC of each candlestick.
        /// <returns></returns>
        private int CalculateCandlestickBeauty(DataPoint candlestick, List<double> fibonacciLevels)
        {
            double tolerance;
            // Check if the user updated the leeway value or not
            if (percent_leeway.Text == "")
            {
                // If not, leeway defaults to 0.01
                tolerance = 0.01;
            }
            else
            {
                // If they did, set the leeway to whatever they picked
                tolerance = leeway_value;
            }

            int beauty = 0;
            double[] ohlc = candlestick.YValues; // 0: Open, 1: High, 2: Low, 3: Close

            // Loop through the Fibonacci levels to find matches with the OHLC values
            foreach (var level in fibonacciLevels)
            {
                // If there is a match between any OHLC value and the Fibonacci level (within tolerance)
                if (ohlc.Any(value => Math.Abs(value - level) <= level * tolerance))
                {
                    // Add the confirmation point (candlestick XValue and Fibonacci level)
                    confirmationPoints.Add(new Tuple<double, double>(candlestick.XValue, level));
                    beauty++; // Increment beauty score when a match is found
                }
            }

            return beauty;
        }

        /// <summary>
        /// This function takes all of the confirmations found when computing the beauty and uses
        /// them to create data points that display dots on the chart.
        /// </summary>
        private void CreateAnnotationsFromConfirmationPoints()
        {
            // Get the existing candlestick series (e.g., "Series_OHLC")
            var candlestickSeries = chart_candlesticks.Series["Series_OHLC"];

            // Loop through the confirmation points
            foreach (var confirmation in confirmationPoints)
            {
                double xValue = confirmation.Item1; // X value (as OADate)
                double yValue = confirmation.Item2; // Y value (Fibonacci level)

                // Find the corresponding data point in the candlestick series
                var dataPoint = candlestickSeries.Points
                    .FirstOrDefault(p => Math.Abs(p.XValue - xValue) < 1e-6); // Match by XValue with precision

                if (dataPoint != null)
                {
                    // Add the confirmation level visually by styling the data point
                    dataPoint.MarkerStyle = MarkerStyle.Circle;
                    dataPoint.MarkerColor = Color.Red; // Highlight confirmation points with red
                    dataPoint.MarkerSize = 3;
                }
            }

            // Refresh the chart to reflect changes
            chart_candlesticks.Invalidate();
            chart_candlesticks.Update();

            // Debug: Display total number of confirmations added
            int confirmationCount = candlestickSeries.Points.Count(p => p.MarkerStyle == MarkerStyle.Circle);
        }


        /// <summary>
        /// This function computes the total beauty of the whole wave, by adding up each candlestick's beauty.
        /// </summary>
        /// <param name="candlesticks"></param>
        /// Takes the list of candlesticks of the whole wave.
        /// <param name="fibonacciLevels"></param>
        /// Takes the list of all 5 fib levels.
        /// <returns></returns>
        private int ComputeWaveBeauty(List<DataPoint> candlesticks, List<double> fibonacciLevels)
        {
            int totalBeauty = 0;
            foreach (var candlestick in candlesticks)
            {
                totalBeauty += CalculateCandlestickBeauty(candlestick, fibonacciLevels);
            }
            return totalBeauty;
        }
        /// <summary>
        /// This function uses the wave candlesticks to add the price values for the new chart. It takes
        /// incremental prices throughout the total range of prices of the wave to use for the "beauty as
        /// a function of price" chart.
        /// </summary>
        /// <param name="high"></param>
        /// The value of the high price of the top candlestick of the wave.
        /// <param name="low"></param>
        /// The value of the low price of the bottom candlestick of the wave.
        /// <param name="resolution"></param>
        /// Used to increment the price a certain number of steps so lots of values of price are included.
        /// <returns></returns>
        private List<double> GeneratePriceLevels(double high, double low, int resolution = 100)
        {
            List<double> prices = new List<double>();
            // Creates incremental steps using a resolution of 100
            double step = (high - low) / resolution;

            for (double price = low; price <= high; price += step)
            {
                // Adds each price from low to high using each incremental step
                prices.Add(price);
            }

            prices.AddRange(new[] { high, low }); // Ensure Fibonacci levels are included
            prices = prices.Distinct().OrderBy(p => p).ToList(); // Orders the prices in order
            return prices;
        }

        /// <summary>
        /// This function plots the beauty by price chart using the incremental prices and calculates the 
        /// percentage of beauty at each price point depending on the fib levels. 
        /// </summary>
        /// <param name="waveCandlesticks"></param>
        /// The list of all candlesticks in the selected wave.
        /// <param name="high"></param>
        /// The value of the price at the top candlestick of the wave.
        /// <param name="low"></param>
        /// The value of the price at the bottom candlestick of the wave.
        /// <param name="fibonacciLevels"></param>
        /// The list of all 5 fib levels.
        /// <returns></returns>
        private Dictionary<double, int> ComputeBeautyByPrice(List<DataPoint> waveCandlesticks, double high, double low, List<double> fibonacciLevels)
        {
            Dictionary<double, int> beautyByPrice = new Dictionary<double, int>();

            // Calculates the price levels 
            List<double> priceLevels = GeneratePriceLevels(high, low);

            foreach (var price in priceLevels)
            {
                // Only consider Fibonacci levels up to the current price
                var relevantLevels = fibonacciLevels.Where(level => level <= price).ToList();

                // Compute Beauty for current price level
                int beauty = 0;
                foreach (var candlestick in waveCandlesticks)
                {
                    // Determines the total beauty of the wave at each price
                    beauty += CalculateCandlestickBeauty(candlestick, relevantLevels);
                }

                beautyByPrice[price] = beauty;
            }

            return beautyByPrice;
        }

        /// <summary>
        /// This function takes the price and beauty values calculated above and plots them on a new window
        /// called beauty vs price. It adds each calculated point and shows the percentages at each price step. 
        /// </summary>
        /// <param name="beautyByPrice"></param>
        /// The list of percentages and prices for the chart.
        private void PlotBeauty(Dictionary<double, int> beautyByPrice)
        {
            // Create a new series for Beauty vs. Price
            Series beautySeries = new Series("Beauty vs. Price")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Green,
                BorderWidth = 2
            };

            // Add data points to the series
            foreach (var entry in beautyByPrice)
            {
                beautySeries.Points.AddXY(entry.Key, entry.Value);
            }

            // Configure chart area
            Chart chart = new Chart();
            chart.ChartAreas.Add(new ChartArea("BeautyChart"));
            chart.Series.Add(beautySeries);

            // Display the chart
            Form chartForm = new Form
            {
                Text = "Beauty as a Function of Price",
                Width = 800,
                Height = 600
            };
            chart.Dock = DockStyle.Fill;
            chartForm.Controls.Add(chart);
            chartForm.Show();
        }

        /// <summary>
        /// This calculates the extended wave beauty by taking the low or high of the wave (depending on if it
        /// is an up wave or a down wave) and determines 25% below or above the selected wave to predict the
        /// beauty values of these prices.
        /// </summary>
        /// <param name="waveCandlesticks"></param>
        /// The list of candlesticks in the wave
        /// <param name="high"></param>
        /// The price value of the top candlestick in the wave
        /// <param name="originalLow"></param>
        /// The price value of the bottom value of the wave
        /// <param name="extensionPercentage"></param>
        /// The percentage to extend the wave by and use for predictions (usually 25%)
        /// <param name="steps"></param>
        /// Number of steps used in the total extension of the wave
        /// <returns></returns>
        private Dictionary<double, int> ComputeExtendedWaveBeauty(List<DataPoint> waveCandlesticks, double high, double originalLow, double extensionPercentage, int steps)
        {
            Dictionary<double, int> extendedBeautyByPrice = new Dictionary<double, int>();

            // Calculate the extension range
            double extensionAmount = Math.Abs(originalLow - high) * extensionPercentage;

            // Set start and end prices based on the wave direction
            double startPrice;
            double endPrice;

            if (isUpwardWave)
            {
                // For an upward wave, start at the high and extend upward
                startPrice = high; 
                endPrice = high + extensionAmount;
            }
            else
            {
                // For a downward wave, start at the low and extend downward
                startPrice = originalLow;
                endPrice = originalLow - extensionAmount;
            }

            // Ensure positive step size
            double stepSize = extensionAmount / steps;

            double newPrice = startPrice;
            List<double> fibonacciLevels = new List<double>();

            while (isUpwardWave ? newPrice <= endPrice : newPrice >= endPrice)
            {
                fibonacciLevels.Clear();

                // Generate Fibonacci levels
                fibonacciLevels = GetFibonacciLevels(high, newPrice);

                // Compute Beauty
                int beauty = ComputeWaveBeauty(waveCandlesticks, fibonacciLevels);

                extendedBeautyByPrice[newPrice] = beauty;

                // Adjust price
                if (isUpwardWave)
                {
                    newPrice += stepSize;
                }
                else
                {
                    newPrice -= stepSize;
                }
            }

            return extendedBeautyByPrice;
        }

        /// <summary>
        /// This takes the calculations from above and plots the graph in a new beauty chart area and series.
        /// It determines whether it is an upward or downward wave to know how to plot the x axis and whether
        /// or not the extended wave is decreasing or increasing. It then plots the y values based on the 
        /// predicted beauty values at those prices.
        /// </summary>
        /// <param name="beautyByPrice"></param>
        /// The list of price and predicted beauty values calculated above.
        private void PlotBeautyVsPrice(Dictionary<double, int> extendedBeautyByPrice)
        {
            // Rename the axes for the Beauty chart
            chart_candlesticks.ChartAreas["ChartArea_Beauty"].AxisX.Title = "Price";
            chart_candlesticks.ChartAreas["ChartArea_Beauty"].AxisY.Title = "Beauty";

            // Clear previous data to avoid overlap
            chart_candlesticks.Series["Series_Beauty"].Points.Clear();

            // Sort the dictionary by ascending price (X-axis)
            IEnumerable<KeyValuePair<double, int>> sortedBeautyByPrice;

            if (isUpwardWave)
            {
                // For upward waves, plot in normal order (ascending order of prices)
                sortedBeautyByPrice = extendedBeautyByPrice.OrderBy(entry => entry.Key);
            }
            else
            {
                // For downward waves, reverse the order so higher prices (the original low) appear first
                sortedBeautyByPrice = extendedBeautyByPrice.OrderByDescending(entry => entry.Key);
            }

            // Add sorted data points for Beauty vs. Price
            foreach (var entry in sortedBeautyByPrice)
            {
                chart_candlesticks.Series["Series_Beauty"].Points.AddXY(entry.Key, entry.Value);
            }

            // Adjust chart settings to ensure proper scaling
            chart_candlesticks.ChartAreas["ChartArea_Beauty"].RecalculateAxesScale();

            // Set the chart type and update visual settings
            chart_candlesticks.Series["Series_Beauty"].ChartType = SeriesChartType.Column; // Or SeriesChartType.Line
            chart_candlesticks.Series["Series_Beauty"].Color = Color.Blue;

            // Refresh the chart
            chart_candlesticks.Invalidate();
            chart_candlesticks.Update();
        }


        public double leeway_value;
        /// <summary>
        /// Allows the user to specify the percent leeway for the confirmations of beauty.
        /// </summary>
        private void percent_leeway_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Check if the Enter key is pressed
            {
                string input = percent_leeway.Text;
                // Parse the value as a double
                if (double.TryParse(input, out double value))
                {
                    leeway_value = value;
                    MessageBox.Show("Leeway changed, please select the wave to compute with this value. (if needed, hit update and select new wave)");
                }
                else
                {
                    leeway_value = 0.01;
                    MessageBox.Show("Leeway set to default value of 0.01.");
                }
            }
        }

        /// <summary>
        /// When the text box for the new bottom x-value is used and the user clicks enter, this is called.
        /// It is used to compute the beauty at a new custom low value.
        /// </summary>
        private void textBox_newX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Check if the Enter key is pressed
            {
                string input = textBox_newX.Text;
                if (double.TryParse(input, out double newLow))
                {
                    ComputeBeautyAtCustomPrice(newLow);
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric value for the new X value.");
                }

                // Prevent further processing of the Enter key
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Allow the user to set a custom price after the wave for beauty vs X
        /// </summary>
        /// <param name="customLow"></param>
        private void ComputeBeautyAtCustomPrice(double customLow)
        {
            if (selectedTopIndex == -1 || selectedBottomIndex == -1)
            {
                MessageBox.Show("Please select a valid wave.");
                return;
            }

            // Get high price from the wave
            double high = chart_candlesticks.Series["Series_OHLC"].Points[(int)selectedTopIndex].YValues[1];

            // Extract candlesticks in the wave
            List<DataPoint> waveCandlesticks = chart_candlesticks.Series["Series_OHLC"]
                .Points.Skip((int)selectedTopIndex)
                .Take((int)selectedBottomIndex - (int)selectedTopIndex + 1)
                .ToList();

            if (waveCandlesticks.Count == 0)
            {
                MessageBox.Show("No candlesticks found in the selected wave.");
                return;
            }
            // Clear old levels
            levels.Clear();

            // Generate Fibonacci levels for custom price
            List<double> fibonacciLevels = GetFibonacciLevels(high, customLow);

            // Compute Beauty at the custom price
            int newBeauty = ComputeWaveBeauty(waveCandlesticks, fibonacciLevels);
            MessageBox.Show($"Beauty at Price ${customLow}: {newBeauty}");
        }

    }
}
