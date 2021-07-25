using System;
using System.Numerics;

namespace shamir
{
    class Program
    {
        static void Main(string[] args)
        {
		BigInteger secret = new BigInteger(666);
		int degree = 4;
		BigInteger prime = new BigInteger(15887);
		ShamirSecretSharing sss = new ShamirSecretSharing(degree,prime,secret);	
		
		int threshold = 5;
		Share[] shares = sss.GenerateShares(threshold);
		
		BigInteger recovered = sss.Interpolate(shares);
		Console.WriteLine("secret is {0}",recovered);
	}
    }
}
