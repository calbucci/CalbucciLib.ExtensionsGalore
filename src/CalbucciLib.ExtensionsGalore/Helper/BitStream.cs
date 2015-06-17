using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.ExtensionsGalore.Helper
{
	// https://github.com/renmengye/base62-csharp/
	public class BitStream : Stream
	{
		private byte[] Source { get; set; }

		/// <summary>
		/// Initialize the stream with capacity
		/// </summary>
		/// <param name="capacity">Capacity of the stream</param>
		public BitStream(int capacity)
		{
			this.Source = new byte[capacity];
		}

		/// <summary>
		/// Initialize the stream with a source byte array
		/// </summary>
		/// <param name="source"></param>
		public BitStream(byte[] source)
		{
			this.Source = source;
		}

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void Flush()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Bit length of the stream
		/// </summary>
		public override long Length
		{
			get { return Source.Length * 8; }
		}

		/// <summary>
		/// Bit position of the stream
		/// </summary>
		public override long Position { get; set; }

		/// <summary>
		/// Read the stream to the buffer
		/// </summary>
		/// <param name="buffer">Buffer</param>
		/// <param name="offset">Offset bit start position of the stream</param>
		/// <param name="count">Number of bits to read</param>
		/// <returns>Number of bits read</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			// Temporary position cursor
			long tempPos = this.Position;
			tempPos += offset;

			// Buffer byte position and in-byte position
			int readPosCount = 0, readPosMod = 0;

			// Stream byte position and in-byte position
			long posCount = tempPos >> 3;
			int posMod = (int)(tempPos - ((tempPos >> 3) << 3));

			while (tempPos < this.Position + offset + count && tempPos < this.Length)
			{
				// Copy the bit from the stream to buffer
				if ((((int)this.Source[posCount]) & (0x1 << (7 - posMod))) != 0)
				{
					buffer[readPosCount] = (byte)((int)(buffer[readPosCount]) | (0x1 << (7 - readPosMod)));
				}
				else
				{
					buffer[readPosCount] = (byte)((int)(buffer[readPosCount]) & (0xffffffff - (0x1 << (7 - readPosMod))));
				}

				// Increment position cursors
				tempPos++;
				if (posMod == 7)
				{
					posMod = 0;
					posCount++;
				}
				else
				{
					posMod++;
				}
				if (readPosMod == 7)
				{
					readPosMod = 0;
					readPosCount++;
				}
				else
				{
					readPosMod++;
				}
			}
			int bits = (int)(tempPos - this.Position - offset);
			this.Position = tempPos;
			return bits;
		}

		/// <summary>
		/// Set up the stream position
		/// </summary>
		/// <param name="offset">Position</param>
		/// <param name="origin">Position origin</param>
		/// <returns>Position after setup</returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case (SeekOrigin.Begin):
					{
						this.Position = offset;
						break;
					}
				case (SeekOrigin.Current):
					{
						this.Position += offset;
						break;
					}
				case (SeekOrigin.End):
					{
						this.Position = this.Length + offset;
						break;
					}
			}
			return this.Position;
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write from buffer to the stream
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset">Offset start bit position of buffer</param>
		/// <param name="count">Number of bits</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			// Temporary position cursor
			long tempPos = this.Position;

			// Buffer byte position and in-byte position
			int readPosCount = offset >> 3, readPosMod = offset - ((offset >> 3) << 3);

			// Stream byte position and in-byte position
			long posCount = tempPos >> 3;
			int posMod = (int)(tempPos - ((tempPos >> 3) << 3));

			while (tempPos < this.Position + count && tempPos < this.Length)
			{
				// Copy the bit from buffer to the stream
				if ((((int)buffer[readPosCount]) & (0x1 << (7 - readPosMod))) != 0)
				{
					this.Source[posCount] = (byte)((int)(this.Source[posCount]) | (0x1 << (7 - posMod)));
				}
				else
				{
					this.Source[posCount] = (byte)((int)(this.Source[posCount]) & (0xffffffff - (0x1 << (7 - posMod))));
				}

				// Increment position cursors
				tempPos++;
				if (posMod == 7)
				{
					posMod = 0;
					posCount++;
				}
				else
				{
					posMod++;
				}
				if (readPosMod == 7)
				{
					readPosMod = 0;
					readPosCount++;
				}
				else
				{
					readPosMod++;
				}
			}
			this.Position = tempPos;
		}
	}
}
