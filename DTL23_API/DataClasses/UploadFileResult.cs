namespace DTL23_API.DataClasses {
	/// <summary>
	/// Результат загрузки файла
	/// </summary>
	public class UploadFileResult {
		/// <summary>
		/// Имя загружаемого файла
		/// </summary>
		public string FileName { get; set; } = string.Empty;

		/// <summary>
		/// Файл успешно загружен?
		/// </summary>
		public bool IsLoaded { get; set; } = false;

		/// <summary>
		/// Сообщение для отображения пользователю
		/// </summary>
		public string MessageToDisplay { get; set; } = string.Empty;

		/// <summary>
		/// Список строк, где не удалось распознать дату рождения
		/// </summary>
		public List<int> RowsWithBrokenBirthdayDate { get; set; } = new List<int>();

		/// <summary>
		/// Список строк, где не удалось распознать дату оказания услуги
		/// </summary>
		public List<int> RowsWithBrokenTreatDate { get; set; } = new List<int>();

		/// <summary>
		/// Кол-во считанных из файла строк
		/// </summary>
		public long RowsReaded { get; set; } = 0;

		/// <summary>
		/// Кол-во загруженных в БД строк
		/// </summary>
		public long RowsUploaded { get; set; } = 0;
	}
}
