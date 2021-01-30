using System;
using AsmodeeNet.Utils;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200062F RID: 1583
	public abstract class RequiredOnlineStatus
	{
		// Token: 0x06003A45 RID: 14917
		public abstract void MeetRequirements(Action onSuccess, Action onFailure);

		// Token: 0x06003A46 RID: 14918 RVA: 0x001218BB File Offset: 0x0011FABB
		public virtual void SetCallbacks(Action onSuccess, Action onFailure)
		{
			this._onSuccess = onSuccess;
			this._onFailure = onFailure;
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x001218CB File Offset: 0x0011FACB
		protected void CallOnSuccess()
		{
			AsmoLogger.Debug(base.GetType().Name, "Success", null);
			if (this._onSuccess != null)
			{
				this._onSuccess();
			}
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x001218F6 File Offset: 0x0011FAF6
		protected void CallOnFailure()
		{
			AsmoLogger.Warning(base.GetType().Name, "Failure", null);
			if (this._onFailure != null)
			{
				this._onFailure();
			}
		}

		// Token: 0x040025AA RID: 9642
		private Action _onSuccess;

		// Token: 0x040025AB RID: 9643
		private Action _onFailure;
	}
}
