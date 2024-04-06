using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenBullet
{
	// Token: 0x0200003E RID: 62
	public class HugeFileSort
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00005FA5 File Offset: 0x000041A5
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00005FAD File Offset: 0x000041AD
		public StringComparer Comparer { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005FB6 File Offset: 0x000041B6
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00005FBE File Offset: 0x000041BE
		public Encoding Encoding { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00005FC7 File Offset: 0x000041C7
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00005FCF File Offset: 0x000041CF
		public long MaxFileSize
		{
			get
			{
				return this.maxFileSize;
			}
			set
			{
				this.maxFileSize = value;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005FD8 File Offset: 0x000041D8
		public HugeFileSort()
		{
			this.Comparer = StringComparer.CurrentCulture;
			this.Encoding = Encoding.UTF8;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006004 File Offset: 0x00004204
		public void Sort(string inputFileName, string outputFileName)
		{
			this.chunks = new SortedDictionary<string, HugeFileSort.ChunkInfo>(this.Comparer);
			if (new FileInfo(inputFileName).Length < this.maxFileSize)
			{
				this.SortFile(inputFileName, outputFileName);
				return;
			}
			DirectoryInfo dir = new DirectoryInfo("tmp");
			if (dir.Exists)
			{
				dir.Delete(true);
			}
			dir.Create();
			this.SplitFile(inputFileName, 1);
			this.Merge(outputFileName);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006070 File Offset: 0x00004270
		private void Merge(string outputFileName)
		{
			using (FileStream output = File.Create(outputFileName))
			{
				foreach (KeyValuePair<string, HugeFileSort.ChunkInfo> name in this.chunks)
				{
					name.Value.Close();
					if (name.Value.NoSortFileName != null)
					{
						this.CopyFile(name.Value.NoSortFileName, output);
					}
					if (name.Value.FileName != null)
					{
						this.CopyFile(name.Value.FileName, output);
					}
				}
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006128 File Offset: 0x00004328
		private void CopyFile(string fileName, FileStream output)
		{
			using (FileStream file = File.OpenRead(fileName))
			{
				file.CopyTo(output);
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006160 File Offset: 0x00004360
		private void SplitFile(string inputFileName, int chars)
		{
			Dictionary<string, HugeFileSort.FileChunk> files = new Dictionary<string, HugeFileSort.FileChunk>(this.Comparer);
			using (StreamReader sr = new StreamReader(inputFileName, this.Encoding))
			{
				while (sr.Peek() >= 0)
				{
					string entry = sr.ReadLine();
					if (entry.Length < chars)
					{
						HugeFileSort.ChunkInfo nameInfo;
						if (!this.chunks.TryGetValue(entry, out nameInfo))
						{
							this.chunks.Add(entry, nameInfo = new HugeFileSort.ChunkInfo());
						}
						nameInfo.AddSmallString(entry, this.Encoding);
					}
					else
					{
						string start = entry.Substring(0, chars);
						HugeFileSort.FileChunk sfi;
						if (!files.TryGetValue(start, out sfi))
						{
							sfi = new HugeFileSort.FileChunk(this.Encoding);
							files.Add(start, sfi);
						}
						sfi.Append(entry, this.Encoding);
					}
				}
			}
			foreach (KeyValuePair<string, HugeFileSort.FileChunk> file in files)
			{
				file.Value.Close();
				if (file.Value.Size > this.maxFileSize)
				{
					this.SplitFile(file.Value.FileName, chars + 1);
					File.Delete(file.Value.FileName);
				}
				else
				{
					this.SortFile(file.Value.FileName, file.Value.FileName);
					HugeFileSort.ChunkInfo nameInfo2;
					if (!this.chunks.TryGetValue(file.Key, out nameInfo2))
					{
						this.chunks.Add(file.Key, nameInfo2 = new HugeFileSort.ChunkInfo());
					}
					nameInfo2.FileName = file.Value.FileName;
				}
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006314 File Offset: 0x00004514
		private void SortFile(string inputFileName, string outputFileName)
		{
			List<string> entries = new List<string>((int)(new FileInfo(inputFileName).Length / 4L));
			using (StreamReader sr = new StreamReader(inputFileName, this.Encoding))
			{
				while (sr.Peek() >= 0)
				{
					entries.Add(sr.ReadLine());
				}
			}
			entries.Sort(this.Comparer);
			using (StreamWriter sw = new StreamWriter(outputFileName, false, this.Encoding))
			{
				foreach (string entry in entries)
				{
					sw.WriteLine(entry);
				}
			}
		}

		// Token: 0x04000138 RID: 312
		private long maxFileSize = 104857600L;

		// Token: 0x04000139 RID: 313
		private SortedDictionary<string, HugeFileSort.ChunkInfo> chunks;

		// Token: 0x0400013A RID: 314
		private static int fileCounter;

		// Token: 0x0200003F RID: 63
		private class ChunkInfo
		{
			// Token: 0x0600015E RID: 350 RVA: 0x000063E8 File Offset: 0x000045E8
			public void AddSmallString(string str, Encoding encoding)
			{
				if (this.noSortWriter == null)
				{
					this.noSortFileName = this.GenerateFileName();
					this.noSortWriter = new StreamWriter(this.noSortFileName, false, encoding);
				}
				this.noSortWriter.WriteLine(str);
			}

			// Token: 0x0600015F RID: 351 RVA: 0x0000641D File Offset: 0x0000461D
			public void Close()
			{
				if (this.noSortWriter != null)
				{
					this.noSortWriter.Close();
				}
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x06000160 RID: 352 RVA: 0x00006432 File Offset: 0x00004632
			public string NoSortFileName
			{
				get
				{
					return this.noSortFileName;
				}
			}

			// Token: 0x06000161 RID: 353 RVA: 0x0000643C File Offset: 0x0000463C
			private string GenerateFileName()
			{
				return "tmp\\n" + HugeFileSort.fileCounter++.ToString() + ".txt";
			}

			// Token: 0x0400013D RID: 317
			private StreamWriter noSortWriter;

			// Token: 0x0400013E RID: 318
			private string noSortFileName;

			// Token: 0x0400013F RID: 319
			public string FileName;
		}

		// Token: 0x02000040 RID: 64
		private class FileChunk
		{
			// Token: 0x06000163 RID: 355 RVA: 0x0000646D File Offset: 0x0000466D
			public FileChunk(Encoding encoding)
			{
				this.fileName = this.GenerateFileName();
				this.writer = new StreamWriter(this.fileName, false, encoding);
			}

			// Token: 0x06000164 RID: 356 RVA: 0x00006494 File Offset: 0x00004694
			private string GenerateFileName()
			{
				return "tmp\\s" + HugeFileSort.fileCounter++.ToString() + ".txt";
			}

			// Token: 0x06000165 RID: 357 RVA: 0x000064C5 File Offset: 0x000046C5
			public void Append(string entry, Encoding encoding)
			{
				this.writer.WriteLine(entry);
				this.size += (long)(encoding.GetByteCount(entry) + encoding.GetByteCount(Environment.NewLine));
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000166 RID: 358 RVA: 0x000064F4 File Offset: 0x000046F4
			public long Size
			{
				get
				{
					return this.size;
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000167 RID: 359 RVA: 0x000064FC File Offset: 0x000046FC
			public string FileName
			{
				get
				{
					return this.fileName;
				}
			}

			// Token: 0x06000168 RID: 360 RVA: 0x00006504 File Offset: 0x00004704
			public void Close()
			{
				this.writer.Close();
			}

			// Token: 0x04000140 RID: 320
			private StreamWriter writer;

			// Token: 0x04000141 RID: 321
			private long size;

			// Token: 0x04000142 RID: 322
			private string fileName;
		}
	}
}
