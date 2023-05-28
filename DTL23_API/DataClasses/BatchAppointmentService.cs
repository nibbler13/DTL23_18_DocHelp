namespace DTL23_API.DataClasses {
	/// <summary>
	/// Медицинская услуга в пакетном назначении
	/// </summary>
	public class BatchAppointmentService {
		/// <summary>
		/// ID строки в БД
		/// </summary>
		public string DbRowId { get; set; }

		/// <summary>
		/// № п/п
		/// </summary>
		public short Order { get; set; }

		/// <summary>
		/// Тип назначений
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Назначения
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Обязательность
		/// </summary>
		public string Necessity { get; set; }

		/// <summary>
		/// Критерии исследований/консультаций
		/// </summary>
		public string Comment { get; set; }
	}
}
