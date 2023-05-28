namespace DTL23_API.DataClasses {
	/// <summary>
	/// Медицинская услуга из загруженных пользователем данных
	/// </summary>
	public class DataSetServiceComparison {
		/// <summary>
		/// ID услуги
		/// </summary>
		public ushort Id { get; set; }

		/// <summary>
		/// Наименование услуги
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Дата добавления в БД
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// Список сопоставленных услуг из пакетных назначений
		/// </summary>
		public List<string> BatchAppointmentsServices { get; set; } = new List<string>();

		/// <summary>
		/// Список сопоставленных услуг из приказов минздрава
		/// </summary>
		public List<string> GeneralStandardsServices { get; set; } = new List<string>();
	}
}
