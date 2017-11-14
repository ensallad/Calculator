using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;

namespace Calculator
{
    [Activity(Label = "Calculator", MainLauncher = true, Icon = "@drawable/calc2")]
    public class MainActivity : Activity
    {
        private TextView calculatorText;
        private string[] numbers = new string[2];
        private string @operator;

        private bool negativeStartNumber = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            calculatorText = FindViewById<TextView>(Resource.Id.calculator_text_view);
            //updateCalculatorText();
        }
        [Java.Interop.Export("buttonClick")]
        public void buttonClick(View v)
        {
            Button button = (Button)v;
            if ("0123456789,".Contains(button.Text))
                addNumberOrDecimalPoint(button.Text);
            else if ("÷x+-".Contains(button.Text))
            {       if ("-".Contains(button.Text))
                    startWithNegativeNumber();

            addOperator(button.Text);
        }
            else if ("x²√".Contains(button.Text))
                addOperatorX(button.Text);

            else if ("=" == button.Text)
                calculate();

            else if ("⌫" == button.Text)
                erase();

            else clear();
           
        }
        //test
        private void clear()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            updateCalculatorText();
        }

        private void calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);
            
            switch(@operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "x":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
                
            }
            if(result!= null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                updateCalculatorText();
            }
        }

        private void addOperator(string value)
        {
            if (numbers[1] != null)
            {
                calculate(value);
            }
            else if (numbers[0] != null && numbers[0] != "")
            {
                if (numbers[0] != "-")
                {
                    @operator = value;
                    updateCalculatorText();
                }
                }
            }

        private void addNumberOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;
            string numbersIn = numbers[index];

            if(value == "," && numbersIn == null )
            {
                string newNumber = "0";
                if (numbers[0] == null)
                {
                    if (negativeStartNumber == true)
                    {
                        newNumber = "-0" + value;
                        numbers[index] = newNumber;
                        updateCalculatorText();
                        negativeStartNumber = false;
                    }
                }
                else
                {
                    newNumber = "0" + value;
                    numbers[index] = newNumber;
                    updateCalculatorText();
                }
                //string newNumber = "0" + value;
                //numbers[index] = newNumber;
                //updateCalculatorText();
            }
            if (value == "," && numbersIn == "")
            {
                string newNumber = "0";
                if (numbers[0] == null)
                {
                    if (negativeStartNumber == true)
                    {
                        newNumber = "-0" + value;
                        numbers[index] = newNumber;
                        updateCalculatorText();
                        negativeStartNumber = false;
                    }
                }
                else
                {
                    newNumber = "0" + value;
                    numbers[index] = newNumber;
                    updateCalculatorText();
                }


                //string newNumber = "0" + value;
                //numbers[index] = newNumber;
                //updateCalculatorText();
            }
            if (value == "," && numbers[index].Contains(","))
            {
                return;
            }

            if (negativeStartNumber == true)
            {
                if (numbers[0] == null)
                {
                    string newNumber = "0";
                    newNumber = "-" + value;
                    numbers[index] += newNumber;
                    updateCalculatorText();

                    negativeStartNumber = false;
                }
            }
            else
            {
                numbers[index] += value;
                updateCalculatorText();
            }
            //numbers[index] += value;
            //updateCalculatorText();
        }

        private void updateCalculatorText()
        {
            calculatorText.Text = $"{numbers[0]} {@operator} {numbers[1]}";       
        }

        private void updateCalculatorTextError()
        {
            calculatorText.Text = "ERROR";
        }
        //Methods for X² and √ calculations
        private void addOperatorX(string value)
        {
            if (numbers[1] != null)
            {
                calculateX(value);
            }
            else if (numbers[0] != null && numbers[0] != "")
            {
                @operator = value;
                calculateX(value);
            }
            }

        private void calculateX(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch (@operator)
            {                
                case "x²":
                    if (first == null)
                    {
                        @operator = null;
                        break;
                    }
                    result = first * first;
                    break;
                case "√":
                    if (first == null)
                    {
                        @operator = null;
                        break;
                    }
                        double firstInDouble = (double)first;
                        result = Math.Pow(firstInDouble, 0.5);   
                        break;
            }
            if (result != null)
            {
                if (result >= 0)
                {
                    numbers[0] = result.ToString();
                    @operator = null;
                    numbers[1] = null;
                    updateCalculatorText();
                }
                else
                {
                    updateCalculatorTextError();
                }
            }
        }
        public void erase()
        {
            string numberInString = "";
            string newNumberInString = "";
            int numberOfNumbers;
            int newNumber;
            string operatorTest = $"{@operator}";

            string first = $"{numbers[0]}";
            string second = $"{numbers[1]}";

          
            if (second != "")
            {             
                numberInString = second.ToString();
                numberOfNumbers = numberInString.Length;
                newNumber = numberOfNumbers - 1;
                newNumberInString = numberInString.Remove(newNumber, 1);

                numbers[1] = newNumberInString;

                updateCalculatorText();

                if(newNumberInString == "")
                {
                    numbers[1] = null;
                }

            }
            else if (operatorTest != "")
            {
                @operator = null;
                updateCalculatorText();
            }
            else if(first != "")
            {
                numberInString = first.ToString();
                numberOfNumbers = numberInString.Length;
                newNumber = numberOfNumbers - 1;
                newNumberInString = numberInString.Remove(newNumber, 1);           
                numbers[0] = newNumberInString;
                updateCalculatorText();

                if (numbers[0] == "")
                {
                    numbers[0] = null;
                    negativeStartNumber = false;
                }
            }
          

        }
        //test for method that enables to start with a negative numbers ex -40 + 20
        private void startWithNegativeNumber()
        {
            if (numbers[0] == null)
            {
                negativeStartNumber = true;
            }
        }

    }
}

