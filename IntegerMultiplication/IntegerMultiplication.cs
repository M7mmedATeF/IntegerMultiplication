using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (power of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>
        /// num1 => B*D
        /// num2 => A*C
        /// Z    => (A+B) * (C+D) - (num1 + num2)
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            byte[] returnVal;
            if (N == 1)
            {
                return multiplyOneElementArray(X, Y);
            }

            // SubArrays
            byte[] A = new byte[N / 2];
            byte[] B = new byte[N / 2];
            byte[] C = new byte[N / 2];
            byte[] D = new byte[N / 2];

            // split Array
            for (int i = 0; i < N / 2; i++)
            {
                int scPart = N / 2 + i;
                A[i] = X[scPart];
                B[i] = X[i];
                C[i] = Y[scPart];
                D[i] = Y[i];
            }

            // Calc num1 => BD
            byte[] num1 = IntegerMultiply(B, D, N / 2);

            // Calc num2 => AC
            byte[] num2 = IntegerMultiply(A, C, N / 2);

            // Calc Z
            byte[] AplusB = sum(A, B);
            byte[] CplusD = sum(C, D);
            byte[] Z = IntegerMultiply(AplusB, CplusD, AplusB.Length);

            // Release From Memory
            A = B = C = D = AplusB = CplusD = null;

            byte[] ACplusBD = sum(num2, num1);
            Z = sub(Z, ACplusBD, Z.Length);

            byte[] arr1 = multiplyByPowerdTen(num1, num1.Length, N);
            byte[] arr2 = multiplyByPowerdTen(Z, Z.Length, N/2);
            returnVal = (arr1.Length > arr2.Length) ? sum(arr1, arr2) : sum(arr2, arr1);
            returnVal = (returnVal.Length > num2.Length) ? sum(returnVal, num2) : sum(num2, returnVal);

            return returnVal;
        }
        #endregion

        static private byte[] sum(byte[] X, byte[] Y)
        {
            int N = X.Length;
            byte[] temp = new byte[N+2];
            byte reminder = 0;
            for(int i=N-1; i >= 0 ; i--)
            {
                int lessINDX = i - N + Y.Length;
                if(lessINDX >= 0)
                {
                    int sumTemp = X[i] + Y[lessINDX] + reminder;
                    if (sumTemp > 9)
                    {
                        reminder = (byte)(sumTemp / 10);
                    }
                    temp[i] = (byte)(sumTemp % 10);
                }
                else
                {
                    temp[i] = (byte)(X[i] + reminder);
                    reminder -= reminder;
                }
            }
            return temp;
        }

        static private byte[] sub(byte[] X, byte[] Y, int N)
        {
            byte[] temp = new byte[N];
            bool isNegative = false;
            if (Y[0] < 0)
                isNegative = true;
            for (int i = N - 1; i >= 0; i--)
            {
                int lessINDX = Y.Length - i;
                if(lessINDX >= 0)
                {
                    int subTemp;
                    if (isNegative)
                        subTemp = X[i] + Y[lessINDX];
                    else
                        subTemp = X[i] - Y[lessINDX];

                    if (subTemp < 0 && i != 0)
                    {
                        subTemp += 10;
                        X[i - 1] -= 1;
                    }
                    temp[i] = (byte)(subTemp);
                }
                else
                {
                    temp[i] = X[i];
                }
            }
            return temp;
        }

        static private byte[] multiplyByPowerdTen(byte[] X, int N, int power)
        {
            byte[] temp = new byte[N+power];
            for(int i = 0; i < N; i++)
            {
                temp[i] = X[i];
            }

            return temp;
        }

        static private byte[] multiplyOneElementArray(byte[] X, byte[] y)
        {
            byte[] temp;
            int mul = X[0] * y[0];
            if (mul < 10)
            {
                temp = new byte[1];
                temp[0] = (byte)mul;
            }
            else { 
                temp = new byte[2];
                short index = 1;
                while(mul != 0)
                {
                    temp[index] = (byte)(mul % 10);
                    index--;
                    mul = (mul - mul%10)/10;
                }
            }
            return temp;
        }
    }
}
