using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculatorWPF
{
    public partial class MainWindow : Window
    {
        private readonly CultureInfo _cultureInfo = CultureInfo.GetCultureInfo("en-En");
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string operatorSymbol = string.Empty;
        private bool isNewNumber = true;  

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                    SimulateButtonClick("0");
                    break;
                case Key.D1:
                case Key.NumPad1:
                    SimulateButtonClick("1");
                    break;
                case Key.D2:
                case Key.NumPad2:
                    SimulateButtonClick("2");
                    break;
                case Key.D3:
                case Key.NumPad3:
                    SimulateButtonClick("3");
                    break;
                case Key.D4:
                case Key.NumPad4:
                    SimulateButtonClick("4");
                    break;
                case Key.D5:
                case Key.NumPad5:
                    SimulateButtonClick("5");
                    break;
                case Key.D6:
                case Key.NumPad6:
                    SimulateButtonClick("6");
                    break;
                case Key.D7:
                case Key.NumPad7:
                    SimulateButtonClick("7");
                    break;
                case Key.D8:
                case Key.NumPad8:
                    SimulateButtonClick("8");
                    break;
                case Key.D9:
                case Key.NumPad9:
                    SimulateButtonClick("9");
                    break;
                case Key.Add:
                    SimulateButtonClick("+");
                    break;
                case Key.Subtract:
                    SimulateButtonClick("-");
                    break;
                case Key.Multiply:
                    SimulateButtonClick("*");
                    break;
                case Key.Divide:
                    SimulateButtonClick("/");
                    break;
                case Key.Enter:
                case Key.Return:
                    EqualsButton_Click(null, null);
                    break;
                case Key.Escape:
                    ClearButton_Click(null, null);
                    break;
                case Key.Back:
                    BackspaceButton_Click(null, null);
                    break;
                case Key.Decimal:
                case Key.OemPeriod:
                    DotButton_Click(null, null);
                    break;
            }
        }

        private void SimulateButtonClick(string content)
        {
            if (content == "=")
                EqualsButton_Click(null, null);
            else if (content == "+" || content == "-" || content == "*" || content == "/")
            {
                var button = new Button { Content = content };
                OperatorButton_Click(button, null);
            }
            else
            {
                var button = new Button { Content = content };
                NumberButton_Click(button, null);
            }
        }

        private void FormatResult(double number)
        {
            if (number == Math.Floor(number))
                ResultTextBox.Content = number.ToString("0", _cultureInfo);
            else
                ResultTextBox.Content = number.ToString("0.##########", _cultureInfo);
        }

        private void PlusMinusButton_Click(object sender, RoutedEventArgs e)
        {
            string currentResult = (string)ResultTextBox.Content;
            if (currentResult != "0" && currentResult != "Error")
            {
                if (currentResult.StartsWith("-"))
                    ResultTextBox.Content = currentResult.Substring(1);
                else
                    ResultTextBox.Content = "-" + currentResult;
            }
        }

        private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Content = "0";
            isNewNumber = true;
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            string currentResult = (string)ResultTextBox.Content;
            if (currentResult != "0")
            {
                if (currentResult.Length > 1)
                    ResultTextBox.Content = currentResult.Substring(0, currentResult.Length - 1);
                else
                    ResultTextBox.Content = "0";
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string number)
            {
                string currentResult = (string)ResultTextBox.Content;

                if (currentResult == "0" || isNewNumber)
                {
                    ResultTextBox.Content = number;
                    isNewNumber = false;
                }
                else
                {
                    ResultTextBox.Content += number;
                }
            }
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string newOperator)
            {
                string currentResult = (string)ResultTextBox.Content;

                if (currentResult != "0")
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(operatorSymbol))
                        {
                            secondNumber = double.Parse(currentResult, _cultureInfo);
                            double result = PerformOperation(firstNumber, secondNumber, operatorSymbol);
                            FormatResult(result);
                            firstNumber = result;
                        }
                        else
                        {
                            firstNumber = double.Parse(currentResult, _cultureInfo);
                        }

                        operatorSymbol = newOperator;
                        isNewNumber = true;
                    }
                    catch (FormatException)
                    {
                        ResultTextBox.Content = "Error";
                        isNewNumber = true;
                    }
                }
            }
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentResult = (string)ResultTextBox.Content;
                secondNumber = double.Parse(currentResult, _cultureInfo);

                double result = PerformOperation(firstNumber, secondNumber, operatorSymbol);
                FormatResult(result);
                
                firstNumber = result;
                operatorSymbol = string.Empty;
                isNewNumber = true;
            }
            catch (FormatException)
            {
                ResultTextBox.Content = "Error";
                isNewNumber = true;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            firstNumber = 0;
            secondNumber = 0;
            operatorSymbol = string.Empty;
            isNewNumber = true;
            ResultTextBox.Content = "0";
        }

        private double PerformOperation(double firstNumber, double secondNumber, string operatorSymbol)
        {
            try
            {
                return operatorSymbol switch
                {
                    "+" => firstNumber + secondNumber,
                    "-" => firstNumber - secondNumber,
                    "*" => firstNumber * secondNumber,
                    "/" => secondNumber != 0 ? firstNumber / secondNumber : throw new DivideByZeroException(),
                    _ => 0
                };
            }
            catch (DivideByZeroException)
            {
                ResultTextBox.Content = "Cannot divide by zero";
                isNewNumber = true;
                return 0;
            }
        }

        private void DotButton_Click(object sender, RoutedEventArgs e)
        {
            if (((string)(ResultTextBox.Content)).Contains("."))
            {
                return;
            }
            ResultTextBox.Content += ".";
        }

        private void SquareRootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentResult = (string)ResultTextBox.Content;
                double number = double.Parse(currentResult, _cultureInfo);
                
                if (number >= 0)
                {
                    double result = Math.Sqrt(number);
                    FormatResult(result);
                    firstNumber = result;
                    isNewNumber = true;
                }
                else
                {
                    ResultTextBox.Content = "Cannot calculate square root of negative number";
                    isNewNumber = true;
                }
            }
            catch (FormatException)
            {
                ResultTextBox.Content = "Error";
                isNewNumber = true;
            }
        }

        private void PercentageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentResult = (string)ResultTextBox.Content;
                double number = double.Parse(currentResult, _cultureInfo);
                
                if (!string.IsNullOrEmpty(operatorSymbol))
                {
                    double result = (firstNumber * number) / 100;
                    FormatResult(result);
                    secondNumber = result;
                }
                else
                {
                    double result = number / 100;
                    FormatResult(result);
                    firstNumber = result;
                    isNewNumber = true;
                }
            }
            catch (FormatException)
            {
                ResultTextBox.Content = "Error";
                isNewNumber = true;
            }
        }
    }
}