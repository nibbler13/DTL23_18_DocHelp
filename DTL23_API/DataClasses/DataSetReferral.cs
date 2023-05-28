namespace DTL23_API.DataClasses {
	/// <summary>
	/// Назначение из загруженных пользователем данных
	/// </summary>
	public class DataSetReferral {
		internal enum StandardType {
			BatchAppointment,
			GeneralStandard
		}

		/// <summary>
		/// Наименование назначения
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Признак присутствия в стандартах
		/// </summary>
		public bool IsPresentsInStandards { get; set; }

		internal StandardType? AssociatedStandardType { get; set; }

		/// <summary>
		/// Соответствующий услуге тип стандарта
		/// </summary>
		public string? AssociatedStandard {
			get {
				if (!AssociatedStandardType.HasValue)
					return null;
				else
					switch (AssociatedStandardType.Value) {
						case StandardType.BatchAppointment:
							return "Пакетное назначение";
						case StandardType.GeneralStandard:
							return "Приказ минздрава";
						default:
							return "Неизвестный тип стандрата";
					}
			}
		}

		/// <summary>
		/// Наименование стандарта, соответствующего услуге
		/// </summary>
		public string? AssociatedStandardsName { get; set; }

		/// <summary>
		/// Идентификатор услуги из стандарта, соответствующей текущей услуге
		/// </summary>
		public string? AssociatedServiceId { get; set; }

		/// <summary>
		/// Наименование услуги из стандарта, соответствующей текущей услуге
		/// </summary>
		public string? AssociatedServiceName { get; set; }

		/// <summary>
		/// Услуга явлется обязательной
		/// </summary>
		public bool? IsServiceNecessary { get; set; }
	}
}
