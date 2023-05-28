namespace DTL23_API.DataClasses {
	/// <summary>
	/// Данные для дашборда
	/// </summary>
	public class DashboardData {
		/// <summary>
		/// Приемы, кол-во, итого
		/// </summary>
		public uint TotalTreatmentsCount { get; set; }

		/// <summary>
		/// Приемы, кол-во, без стандарта
		/// </summary>
		public uint TreatmentsWithStandardsCount { get; set; }

		/// <summary>
		/// Приемы, кол-во, со стандартом
		/// </summary>
		public uint TreatmentsWithoutStandardsCount { get; set; }

		/// <summary>
		/// Приемы, кол-во, без назначений
		/// </summary>
		public uint TreatmentsStatusHasNoReferrals { get; set; }

		/// <summary>
		/// Приемы, кол-во, все назначения соответствуют стандарту
		/// </summary>
		public uint TreatmentsStatusAllInStandards { get; set; }

		/// <summary>
		/// Приемы, кол-во, назначения частично соответствуют стандарту
		/// </summary>
		public uint TreatmentsStatusPartlyInStandards { get; set; }

		/// <summary>
		/// Приемы, кол-во, назначения не соответствуют стандарту
		/// </summary>
		public uint TreatmentsStatusNoneInStandards { get; set; }

		/// <summary>
		/// Назначения в приемах, кол-во, итого
		/// </summary>
		public uint TotalReferralsCount { get; set; }

		/// <summary>
		/// Назначения в приемах, кол-во, со стандартами
		/// </summary>
		public uint ReferralsCountWithStandards { get; set; }

		/// <summary>
		/// Назначения в приемах, кол-во, без стандарта
		/// </summary>
		public uint ReferralsCountWithoutStandards { get; set; }

		/// <summary>
		/// Назначения в приемах сопоставленные со стандартами, кол-во, обязательные
		/// </summary>
		public uint ReferralsHasComparedNecessary { get; set; }

		/// <summary>
		/// Назначения в приемах сопоставленные со стандартами, кол-во, по назначению
		/// </summary>
		public uint ReferralsHasComparedOptional { get; set; }

		/// <summary>
		/// Назначения в приемах сопоставленных со стандартами, кол-во, не входит в стандарт
		/// </summary>
		public uint ReferralsOutsideStandards { get; set; }
	}
}
