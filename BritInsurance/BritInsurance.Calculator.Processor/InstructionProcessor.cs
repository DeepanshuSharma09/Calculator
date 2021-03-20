using BritInsurance.Calculator.Processor.Interfaces;
using System;

namespace BritInsurance.Calculator.Processor
{
    public class InstructionProcessor : IInstructionProcessor
    {
        private int resultantNumber;

        public int Process(string instructions)
        {
            try
            {
                if (!string.IsNullOrEmpty(instructions))
                {
                    this.DetermineInstructions(instructions);
                }

                return this.resultantNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DetermineInstructions(string instructions)
        {
            var arr = instructions.Split(Environment.NewLine);
            var lastInstruction = arr[arr.Length - 1].Split(' ');

            if (!lastInstruction[0].ToLower().Equals("apply"))
                throw new MissingFieldException("Apply Instruction missing.");

            this.resultantNumber = Convert.ToInt32(lastInstruction[1]);

            for (int i = 0; i < arr.Length - 1; i++)
            {
                var item = arr[i].Split(' ');
                var instruction = item[0].ToLower();

                this.ApplyInstructions(instruction, item[1]);
            }
        }

        private void ApplyInstructions(string instruction, string operand)
        {
            switch (instruction)
            {
                case "add":
                    Add(Convert.ToInt32(operand));
                    break;
                case "subtract":
                    Subtract(Convert.ToInt32(operand));
                    break;
                case "multiply":
                    Multiply(Convert.ToInt32(operand));
                    break;
                case "divide":
                    Divide(Convert.ToInt32(operand));
                    break;
            }
        }

        private void Add(int num1)
        {
            this.resultantNumber += num1;
        }

        private void Subtract(int num1)
        {
            this.resultantNumber -= num1;
        }

        private void Divide(int num1)
        {
            try
            {
                this.resultantNumber /= num1;
            }
            catch (DivideByZeroException ex)
            {
                throw ex;
            }
        }

        private void Multiply(int num1)
        {
            this.resultantNumber *= num1;
        }
    }
}
