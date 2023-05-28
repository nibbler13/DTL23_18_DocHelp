namespace DTL23_API.DataClasses {
	/// <summary>
	/// Стандарт минздрава
	/// </summary>
	public class GeneralStandard {
		/// <summary>
		/// Наименование приказа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Применимость приказа
		/// </summary>
		public string Purpose { get; set; }

		/// <summary>
		/// Показания к использованию
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Код по МКБ Х, Нозологические единицы
		/// </summary>
		public string MkbGroup { get; set; }

		/// <summary>
		/// Словарь секций услуг
		/// </summary>
		public SortedDictionary<string, GeneralStandardsSection> Sections { get; set; } =
			new SortedDictionary<string, GeneralStandardsSection>();
	}
}
