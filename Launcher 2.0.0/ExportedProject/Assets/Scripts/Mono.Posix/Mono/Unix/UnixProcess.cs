using System;
using Mono.Unix.Native;

namespace Mono.Unix
{
	public sealed class UnixProcess
	{
		private int pid;

		public int Id
		{
			get
			{
				return pid;
			}
		}

		public bool HasExited
		{
			get
			{
				int processStatus = GetProcessStatus();
				return Syscall.WIFEXITED(processStatus);
			}
		}

		public int ExitCode
		{
			get
			{
				if (!HasExited)
				{
					throw new InvalidOperationException(global::Locale.GetText("Process hasn't exited"));
				}
				int processStatus = GetProcessStatus();
				return Syscall.WEXITSTATUS(processStatus);
			}
		}

		public bool HasSignaled
		{
			get
			{
				int processStatus = GetProcessStatus();
				return Syscall.WIFSIGNALED(processStatus);
			}
		}

		public Signum TerminationSignal
		{
			get
			{
				if (!HasSignaled)
				{
					throw new InvalidOperationException(global::Locale.GetText("Process wasn't terminated by a signal"));
				}
				int processStatus = GetProcessStatus();
				return Syscall.WTERMSIG(processStatus);
			}
		}

		public bool HasStopped
		{
			get
			{
				int processStatus = GetProcessStatus();
				return Syscall.WIFSTOPPED(processStatus);
			}
		}

		public Signum StopSignal
		{
			get
			{
				if (!HasStopped)
				{
					throw new InvalidOperationException(global::Locale.GetText("Process isn't stopped"));
				}
				int processStatus = GetProcessStatus();
				return Syscall.WSTOPSIG(processStatus);
			}
		}

		public int ProcessGroupId
		{
			get
			{
				return Syscall.getpgid(pid);
			}
			set
			{
				int retval = Syscall.setpgid(pid, value);
				UnixMarshal.ThrowExceptionForLastErrorIf(retval);
			}
		}

		public int SessionId
		{
			get
			{
				int num = Syscall.getsid(pid);
				UnixMarshal.ThrowExceptionForLastErrorIf(num);
				return num;
			}
		}

		internal UnixProcess(int pid)
		{
			this.pid = pid;
		}

		private int GetProcessStatus()
		{
			int status;
			int num = Syscall.waitpid(pid, out status, WaitOptions.WNOHANG | WaitOptions.WUNTRACED);
			UnixMarshal.ThrowExceptionForLastErrorIf(num);
			return num;
		}

		public static UnixProcess GetCurrentProcess()
		{
			return new UnixProcess(GetCurrentProcessId());
		}

		public static int GetCurrentProcessId()
		{
			return Syscall.getpid();
		}

		public void Kill()
		{
			Signal(Signum.SIGKILL);
		}

		[CLSCompliant(false)]
		public void Signal(Signum signal)
		{
			int retval = Syscall.kill(pid, signal);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
		}

		public void WaitForExit()
		{
			int num;
			do
			{
				int status;
				num = Syscall.waitpid(pid, out status, (WaitOptions)0);
			}
			while (UnixMarshal.ShouldRetrySyscall(num));
			UnixMarshal.ThrowExceptionForLastErrorIf(num);
		}
	}
}
