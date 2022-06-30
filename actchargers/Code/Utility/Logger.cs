using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCLStorage;

namespace actchargers
{
	public class Logger
	{
		private static string mcb_filePath;
		private static bool inititlaized;
		private static Mutex myMutex = new Mutex(false);
		const string encKey = "TTHhIikKEErR55sHHREaaH==";
		private static IFile File;
		static System.IO.StreamWriter fw;
		public async static Task<bool> init()
		{
			inititlaized = false;

			IFolder rootFolder = FileSystem.Current.LocalStorage;
			string fileName = "MCB.txt";
			string tempFileName = "THIK_NCK_AHHI.tmp";
			IFile tempFile = null;
			try
			{
				File = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
				tempFile = await rootFolder.CreateFileAsync(tempFileName, CreationCollisionOption.ReplaceExisting);

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Logger " + ex.Message);

			}


    			var file = await File.OpenAsync(FileAccess.Read);
    			var writefile = await tempFile.OpenAsync(FileAccess.ReadAndWrite);

			try
			{
				DateTime d = DateTime.UtcNow;
				string originalLine = null;
				string[] sep = { ">><<" };
				using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
				{
					using (System.IO.StreamWriter writer = new System.IO.StreamWriter(writefile))
					{
						while ((originalLine = reader.ReadLine()) != null)
						{
							string lineRead = originalLine;
							if (lineRead != "" && lineRead[0] == 'd')
							{
								//TODO Implement the Decrypt Part and uncomment the next line
								//lineRead = Decrypt(lineRead.Substring(1));

							}
							string t = lineRead.Split(sep, StringSplitOptions.None)[0];
							if (DateTime.TryParse(t, out d))
							{
								if (d.AddDays(30) < DateTime.UtcNow)
									continue;
								writer.WriteLine(originalLine);
							}
						}
					}
				}
				file.Dispose();
				writefile.Dispose();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Logger " + ex.Message);
				if (file != null)
					file.Dispose();
				if (writefile != null)
					file.Dispose();
				return false;
			}
			try
			{

				string PreviosLog = await tempFile.ReadAllTextAsync();
				fw = new System.IO.StreamWriter(await File.OpenAsync(FileAccess.ReadAndWrite));
				await fw.WriteLineAsync(await tempFile.ReadAllTextAsync());

				await tempFile.DeleteAsync();
				//await tempFile.DeleteAsync();
				//System.IO.File.Replace(tmpFilePath, mcb_filePath, tmpFilePath + ".back");
				//System.IO.File.Delete(tmpFilePath + ".back");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Logger " + ex.Message);
				return false;
			}

			inititlaized = true;
			AddLog(false, "Program start");
			return true;
		}


		public static void AddLog(bool isError, string line, bool forceEncrypt = false)
		{
			if (ACConstants.IS_LOGGING_REQUIRED)
			{
				Task.Run(async () =>
				{

					myMutex.WaitOne();
					try
					{
						if (!inititlaized)
						{
							if (!await init())
							{
								myMutex.ReleaseMutex();
								return;
							}
						}
					}
					catch
					{
						myMutex.ReleaseMutex();
					}


					try
					{
						if (File != null)
						{
							if (fw == null)
							{
								fw = new System.IO.StreamWriter(await File.OpenAsync(FileAccess.ReadAndWrite));
							}
						}
						line = line.Replace(Environment.NewLine, "<<>>");
						string logLine = DateTime.UtcNow.ToString("MM/dd/yy HH:mm:ss") + ">><<" + (isError ? "ERROR" : "Info") + ">><<" + line;
						if ((isError || forceEncrypt) && !ControlObject.isDebugMaster)
						{
							//TODO Implement Encrypting and uncomment the below Code
							//logLine = "d" + Encrypt(logLine);
							logLine = "d";
						}
						if (fw != null)
						{
							await fw.WriteLineAsync(logLine);
							//fw.WriteLine(logLine);
							fw.Flush();
							//fw.Dispose();	
						}

					}
					catch
					{
						if (fw != null) fw.Dispose();
						inititlaized = false;
					}
					try
					{
						myMutex.ReleaseMutex();
						Debug.WriteLine("Added Line to Log File");
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				});
			}
		}
	}
}
