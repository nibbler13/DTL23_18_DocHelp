namespace DTL23_API.DataClasses {
	/// <summary>
	/// Пакетное назначение
	/// </summary>
	public class BatchAppointment {
		/// <summary>
		/// Раздел
		/// </summary>
		public string Section { get; set; }

		/// <summary>
		/// Группа заболеваний
		/// </summary>
		public string Group { get; set; }

		/// <summary>
		/// Список МКБ-10 кодов
		/// </summary>
		public string Mkb { get; set; }

		/// <summary>
		/// Словарь с услугами
		/// </summary>
		public SortedDictionary<short, BatchAppointmentService> Services { get; set; } =
			new SortedDictionary<short, BatchAppointmentService>();
	}
}
