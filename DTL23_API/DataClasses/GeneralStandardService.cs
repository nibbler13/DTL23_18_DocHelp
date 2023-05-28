namespace DTL23_API.DataClasses {
	/// <summary>
	/// Медицинская услуга в стандарте минздрава
	/// </summary>
	public class GeneralStandardService {
		/// <summary>
		/// ID строки в БД
		/// </summary>
		public string DbRowId { get; set; }

		/// <summary>
		/// Код медицинской услуги
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Наименование медицинской услуги
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Усредненный показатель частоты предоставления
		/// </summary>
		public float ApplicationFrequencyIndex { get; set; }

		/// <summary>
		/// Усредненный показатель кратности применения
		/// </summary>
		public float RateOfSubmitting { get; set; }
	}
}
