using GameTools.Framework.Concepts;

namespace GameTools.BlazorClient.Services
{
	public class LimitedResourceModel
	{
        public LimitedResourceModel(MeteredResourceKind resourceKind)
        {
			ResourceTitle = resourceKind;
        }

        public MeteredResourceKind ResourceTitle { get; private set; }

		public int QuotaId { get; private set; }

		public int UnitBudget { get; private set; }

		public int ConsumedUnits { get; private set; }

		public int AvailableQuota => UnitBudget - ConsumedUnits;

		public void SetValues(int quotaId, int unitBudget, int consumedUnits)
		{
			QuotaId = quotaId;
			UnitBudget = unitBudget;
			ConsumedUnits = consumedUnits;
		}
	}
}
