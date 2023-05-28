namespace DTL23_API.DataClasses {
	/// <summary>
	/// Секция услуг
	/// </summary>
	public class GeneralStandardsSection {
		/// <summary>
		/// Наименование секции
		/// </summary>
		public string SectionName { get; set; }

		/// <summary>
		/// Словарь с группами услуг
		/// </summary>
		public SortedDictionary<string, GeneralStandardsServicesGroup> ServicesGroups { get; set; } =
			new SortedDictionary<string, GeneralStandardsServicesGroup>();
	}
}
