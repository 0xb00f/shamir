using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Linq;

namespace shamir
{
	public class Share
	{
		public BigInteger x;
		public BigInteger y;

		public Share(BigInteger a, BigInteger b)
		{
			this.x = a;
			this.y = b;
		}
	}

	public class ShamirSecretSharing
	{
		private BigInteger[] polynomial;
		private BigInteger modulus;
		private RNGCryptoServiceProvider rng;

		public ShamirSecretSharing(int k, BigInteger prime, BigInteger secret)
		{
			this.modulus = prime;
			this.rng = new RNGCryptoServiceProvider();
			this.polynomial = new BigInteger[k];
				
			int i = 1;
			while(i < k)
			{
				BigInteger val = NewRandomValue();
				if(val == 0) continue;
				this.polynomial[i++] = val;
			}
			this.polynomial[0] = secret;
		}

		public Share[] GenerateShares(int n)
		{
			BigInteger[] xs = new BigInteger[n];
			Share[] shares = new Share[n];
			int i = 0;
			while(i < n)
			{
				BigInteger x = NewRandomValue();
				if(x == 0 || xs.Contains(x)) continue;
				xs[i++] = x;
			}
			for(i=0; i < n; i++)
			{	
				shares[i] = new Share(xs[i],EvaluatePolynomial(xs[i]));
			}
			return shares;
		}
		
		public BigInteger Interpolate(Share[] shares)
		{
			int j;
			BigInteger[] lagrange_values = new BigInteger[shares.Length];
			for(j=0; j < shares.Length; j++)
			{
				BigInteger tmp = new BigInteger(1);
				int m;
				for(m=0; m < shares.Length; m++)
				{
					if(shares[j].x == shares[m].x) continue;
					BigInteger diff = shares[m].x - shares[j].x;
					diff = diff < 0 ? diff + this.modulus : diff;
					BigInteger inversemod = BigInteger.ModPow(diff,this.modulus-2,this.modulus);
					tmp = tmp * shares[m].x * inversemod;
				}
				lagrange_values[j] = shares[j].y * tmp;
			}
			return lagrange_values.Aggregate(BigInteger.Add) % this.modulus;
		}

		private BigInteger EvaluatePolynomial(BigInteger x)
		{
			BigInteger y = new BigInteger(0);
			int i;
			for(i=0; i < this.polynomial.Length; i++)
			{
				y = y + BigInteger.ModPow(x,i,this.modulus) * polynomial[i];
			}
			return y % this.modulus;
		}

		private BigInteger NewRandomValue() 
		{
			int numbytes = this.modulus.GetByteCount() > 1 ? this.modulus.GetByteCount() : 2;
			byte[] arr = new byte[numbytes];
			this.rng.GetBytes(arr);
			arr[arr.Length-1] = 0;

			return new BigInteger(arr);
		}
	}
}
