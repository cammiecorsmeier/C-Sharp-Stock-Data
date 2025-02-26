# Stock Price Prediction and Beauty Analysis

## Project Overview
This Windows Forms Application is designed to predict future lows and highs of stock prices by analyzing waves in stock charts. The application allows users to select valid waves and predict potential price movements based on Fibonacci levels and a computed Beauty function. The Beauty function measures the correlation between stock prices and Fibonacci levels.

## Features
- **Stock Data Loading**: Users can load stock data from CSV files using an OpenFileDialog.
- **Candlestick Chart Display**: Displays OHLC data as candlesticks and volume as a column chart.
- **Wave Selection**: Users can select two candlesticks (first being a Peak/Valley) to form a wave.
- **Wave Validation**: Ensures selected waves are valid before proceeding with analysis.
- **Fibonacci Level Calculation**: Computes Fibonacci retracement levels for the selected wave.
- **Beauty Function Calculation**: Computes confirmations of candlesticks with Fibonacci levels.
- **Beauty vs. Price Chart**: Generates a chart showing Beauty as a function of price.
- **Real-time Chart Updates**: Allows updating displayed charts without reloading stock data.
- **Interactive Selection Methods**: Users can select waves using mouse clicks, dragging, or scrollbars.
- **Annotations & Indicators**: Displays Fibonacci levels inside the wave and annotations for selected candlesticks.

## Installation & Usage
1. Ensure you have Visual Studio installed with .NET Framework support.
2. Download and extract the project files.
3. Open the solution in Visual Studio.
4. Build and run the application.
5. Load stock data from a CSV file located in the `Stock Data` folder.
6. Select a wave using mouse interaction.
7. View the computed Fibonacci levels and Beauty chart.

## File Naming Conventions
- Stock data files must be named as follows:
  - `XXX-Day.csv` for daily data
  - `XXX-Week.csv` for weekly data
  - `XXX-Month.csv` for monthly data

## Developer Notes
- **Data Binding**: Used to dynamically update chart data.
- **Form Controls Naming Convention**:
  - Buttons: `button_actionName`
  - Textboxes: `textbox_inputName`
  - Charts: `chart_dataType`
- **Code Documentation**:
  - Every function and method is documented using `///` comments.
  - Inline comments explain key lines of code.
- **Chart Enhancements**:
  - No gaps in daily data for weekends and holidays.
  - Removed legend to maximize space.

## Dependencies
- .NET Framework (Windows Forms)
- System.Windows.Forms.DataVisualization
- CSV file parsing libraries

## Submission Instructions
1. Clean the solution in Visual Studio (`Build` > `Clean Solution`).
2. Zip the solution folder.
3. Submit the zipped file as per the assignment requirements.

## Author
Developed as part of Project 3 in the course. This application builds upon Project 2, incorporating wave selection, Fibonacci levels, and the Beauty function for stock price prediction.

