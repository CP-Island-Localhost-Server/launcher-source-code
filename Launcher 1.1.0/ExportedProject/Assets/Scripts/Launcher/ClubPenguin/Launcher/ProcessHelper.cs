using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Disney.LaunchPadFramework;

namespace ClubPenguin.Launcher
{
	public static class ProcessHelper
	{
		public static Process StartProcess(ProcessStartInfo psi, DataReceivedEventHandler stdOutHandler = null, DataReceivedEventHandler stdErrorHandler = null, bool redirectStandardInput = false, bool waitForExit = false, int timeOutSeconds = 180)
		{
			Process process = null;
			TimeSpan timeout = TimeSpan.FromSeconds(timeOutSeconds);
			AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
			try
			{
				AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
				try
				{
					process = new Process();
					psi.UseShellExecute = false;
					psi.RedirectStandardOutput = stdOutHandler != null;
					psi.RedirectStandardError = stdErrorHandler != null;
					psi.RedirectStandardInput = redirectStandardInput;
					psi.CreateNoWindow = true;
					process.StartInfo = psi;
					process.EnableRaisingEvents = !waitForExit;
					bool flag = false;
					try
					{
						if (stdOutHandler != null)
						{
							process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
							{
								if (e.Data == null)
								{
									outputWaitHandle.Set();
								}
								else
								{
									stdOutHandler(sender, e);
								}
							};
						}
						if (stdErrorHandler != null)
						{
							process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
							{
								if (e.Data == null)
								{
									errorWaitHandle.Set();
								}
								else
								{
									stdErrorHandler(sender, e);
								}
							};
						}
						process.Start();
						flag = true;
						if (stdOutHandler != null)
						{
							process.BeginOutputReadLine();
						}
						if (stdErrorHandler != null)
						{
							process.BeginErrorReadLine();
						}
						if (waitForExit && !process.WaitForExit((int)timeout.TotalMilliseconds))
						{
							Log.LogErrorFormatted(typeof(ProcessHelper), "Process timed out: {0} {1}", psi.FileName, psi.Arguments);
						}
					}
					catch (Exception ex)
					{
						Log.LogErrorFormatted(typeof(ProcessHelper), "There was an unhandled exception while executing a process. See below for details about process '{0} {1}'", psi.FileName, psi.Arguments);
						Log.LogException(typeof(ProcessHelper), ex);
						Win32Exception ex2 = ex as Win32Exception;
						if (ex2 != null)
						{
							Log.LogErrorFormatted(typeof(ProcessHelper), "Win32 Exception Details: NativeErrorCode={0} ErrorCode={1} Message={2} Source={3}", ex2.NativeErrorCode, ex2.ErrorCode, ex2.Message, ex2.Source);
						}
						throw;
					}
					finally
					{
						if (waitForExit && flag)
						{
							if (stdOutHandler != null)
							{
								outputWaitHandle.WaitOne(timeout);
							}
							if (stdErrorHandler != null)
							{
								errorWaitHandle.WaitOne(timeout);
							}
							if (process.HasExited && process.ExitCode != 0)
							{
								throw new Exception(string.Format("External process exited with error code: {0} {1}", psi.FileName, process.ExitCode));
							}
						}
						if (process.HasExited)
						{
							process.Dispose();
						}
					}
				}
				finally
				{
					if (errorWaitHandle != null)
					{
						((IDisposable)errorWaitHandle).Dispose();
					}
				}
			}
			finally
			{
				if (outputWaitHandle != null)
				{
					((IDisposable)outputWaitHandle).Dispose();
				}
			}
			return process;
		}

		public static Process ExecuteShellCommand(bool redirectStandardInput, bool waitForExit, string fileName, string argumentFormat, params string[] args)
		{
			return ExecuteShellCommand(null, null, redirectStandardInput, waitForExit, fileName, argumentFormat, args);
		}

		public static Process ExecuteShellCommand(DataReceivedEventHandler stdOutHandler, DataReceivedEventHandler stdErrorHandler, bool redirectStandardInput, bool waitForExit, string fileName, string argumentFormat, params string[] args)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = fileName;
			processStartInfo.Arguments = string.Format(argumentFormat, args);
			return StartProcess(processStartInfo, stdOutHandler, stdErrorHandler, redirectStandardInput, waitForExit);
		}
	}
}
