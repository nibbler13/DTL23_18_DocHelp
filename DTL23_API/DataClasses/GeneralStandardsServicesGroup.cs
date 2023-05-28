namespace DTL23_API.DataClasses {
	/// <summary>
	/// Группа услуг
	/// </summary>
	public class GeneralStandardsServicesGroup {
		/// <summary>
		/// Наименование группы
		/// </summary>
		public string GroupName { get; set; }

		/// <summary>
		/// Словарь с медицинскими услугами
		/// </summary>
		public SortedDictionary<string, GeneralStandardService> Services { get; set; } =
			new SortedDictionary<string, GeneralStandardService>();
	}
}
