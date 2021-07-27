using System;
using System.Numerics;

namespace shamir
{
    class Program
    {
        static void Main(string[] args)
        {
		BigInteger secret = new BigInteger(666);
		Console.WriteLine("secret is {0}",secret);
		
		int threshold = 4;
		BigInteger prime = BigInteger.Parse("130922996888451848749185915881459813412881");
		ShamirSecretSharing sss = new ShamirSecretSharing(threshold,prime,secret);	
		
		int n_shares = 5;
		Share[] shares = sss.GenerateShares(n_shares);
		
		BigInteger recovered = sss.Interpolate(shares);
		Console.WriteLine("recovered secret is {0}",recovered);
	}
    }
}
