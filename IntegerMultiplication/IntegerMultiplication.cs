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
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            if (Y.Length > X.Length)
            {
                if (Y.Length % 2 == 0)
                    N = Y.Length;
                else
                    N = Y.Length + 1;
            }
            else
            {
                if (X.Length % 2 == 0)
                    N = X.Length;
                else
                    N = X.Length + 1;
            }
            X = paddingLeftByZero(X, N);
            Y = paddingLeftByZero(Y, N);

            byte[] returnVal;
            if (N < 10)
            {
                returnVal = multiplyOneElementArray(parseULONG(X), parseULONG(Y));
                return returnVal;
            }

            // SubArrays
            int Mx = (X.Length % 2 == 1) ? X.Length / 2 + 1: X.Length / 2;
            int My = (Y.Length % 2 == 1) ? Y.Length / 2 + 1: Y.Length / 2;
            byte[] A = splitArray(X, 0, Mx);
            byte[] B = splitArray(X, Mx, N);
            byte[] C = splitArray(Y, 0, My);
            byte[] D = splitArray(Y, My, N);

            byte[] AC = IntegerMultiply(A, C, N / 2);
            byte[] BD = IntegerMultiply(B, D, N / 2);

            byte[] ApB = sum(A, B);
            byte[] CpD = sum(C, D);

            byte[] Z = IntegerMultiply(ApB, CpD, ApB.Length);

            byte[] ACpBD = sum(AC, BD);

            Z = sub(Z, ACpBD);

            byte[] M1 = paddingRightByZero(BD, N);
            byte[] M2 = paddingRightByZero(Z, N/2);

            returnVal = sum(M1, M2);

            returnVal = sum(returnVal, AC);

            returnVal = removeUnwantedZeros(returnVal);
            return returnVal;
        }
        #endregion

        static private byte[] removeUnwantedZeros(byte[] array)
        {
            int zeros = 0;
            for(int i= array.Length-1; i > 0; i--)
            {
                if (array[i] == 0)
                    zeros++;
                else
                    break;
            }
            byte[] temp = new byte[array.Length - zeros];

            for(int i = 0; i< temp.Length; i++)
            {
                temp[i] = array[i];
            }

            return temp;
        }

        static private byte[] newArray(byte[] A)
        {
            byte[] temp = new byte[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                temp[i] = A[i];
            }
            return temp;
        }

        static private byte[] sub(byte[] A, byte[] B)
        {
            A = newArray(A);
            A = paddingLeftByZero(A, B.Length);
            B = paddingLeftByZero(B, A.Length);

            byte[] temp = new byte[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                int sub = A[i] - B[i];
                if (sub < 0)
                {
                    int j = i+1;
                    while (j < A.Length)
                    {
                        if (A[j] == 0)
                        {
                            A[j] += 9;
                        }
                        else
                        {
                            A[j] -= 1;
                            break;
                        }
                        j++;
                    }
                    sub += 10;
                }
                temp[i] = (byte)(sub);
            }

            temp = removeUnwantedZeros(temp);
            //Console.Write("Return sub" + level + " : ");
            //Problem.PrintNum(temp);
            return temp;
        }

        static private byte[] paddingRightByZero(byte[] array, int padding)
        {
            byte[] temp = new byte[array.Length + padding];
            int index = temp.Length - 1;
            for (int i = array.Length - 1; i >= 0; i--)
            {
                temp[index] = array[i];
                index--;
            }
            return temp;
        }

        static private byte[] paddingLeftByZero(byte[] array, int arrLength)
        {
            int padding = arrLength - array.Length;
            if (padding <= 0)
                return array;

            byte[] temp = new byte[array.Length + padding];
            for (int i = 0; i < array.Length; i++)
            {
                temp[i] = array[i];
            }
            return temp;
        }

        static private byte[] sum(byte[] A, byte[] B)
        {
            A = paddingLeftByZero(A, B.Length);
            B = paddingLeftByZero(B, A.Length);

            byte[] temp = new byte[A.Length + 2];
            int carry = 0;
            int i;
            for (i = 0; i < A.Length + 1; i++)
            {
                int sum;
                if (i < A.Length)
                    sum = A[i] + B[i] + carry;
                else
                    sum = carry;
                carry = (sum > 9) ? sum / 10 : 0;
                temp[i] = (byte)(sum % 10);
            }

            temp = removeUnwantedZeros(temp);
            return temp;
        }

        static private byte[] multiplyOneElementArray(ulong x, ulong y)
        {
            int size = 0;
            ulong mul = x * y;
            ulong muTemp = mul;

            // get mul Length
            while (muTemp != 0)
            {
                muTemp = muTemp / 10;
                size++;
            }

            size = (size > 0) ? size : 1;
            byte[] temp = new byte[size];
            int i = 0;
            while (mul != 0 && i < size)
            {
                temp[i] = (byte)(mul % 10);
                mul = mul / 10;
                i++;
            }
            return temp;
        }

        static private byte[] splitArray(byte[] array, int start, int end)
        {
            int size = end - start;
            byte[] splited = new byte[size];
            for (int i = 0; i < size; i++)
            {
                if(start < array.Length)
                splited[i] = array[start];
                start++;
            }
            return splited;
        }

        static private ulong parseULONG(byte[] array)
        {
            ulong num = 0;
            for(int i = array.Length-1; i >= 0; i--)
            {
                num = (num*10)+ array[i];
            }
            return num;
        }
    }
}
