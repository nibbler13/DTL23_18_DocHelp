namespace DTL23_API.DataClasses {
	/// <summary>
	/// Детализированные данные по приемам\назначениям
	/// </summary>
	public class DetailedInfo {
		internal enum ReferralsStatusType { 
			TreatmentHasNoStandards, 
			TreatmentHasNoReferrals,
			AllInStandards, 
			PartlyInStandards, 
			NoneInStandards 
		}

		/// <summary>
		/// Идентификатор записи
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Пол пациента
		/// </summary>
		public string Sex { get; set; }

		/// <summary>
		/// Дата рождения пациента
		/// </summary>
		public DateTime BirthdayDate { get; set; }

		/// <summary>
		/// ID пациента
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// Код МКБ-10
		/// </summary>
		public string Mkb { get; set; }

		/// <summary>
		/// Диагноз
		/// </summary>
		public string Diagnosis { get; set; }

		/// <summary>
		/// Дата оказания услуги
		/// </summary>
		public DateTime TreatDate { get; set; }

		/// <summary>
		/// Должность
		/// </summary>
		public string DoctorPosition { get; set; }

		/// <summary>
		/// Назначения
		/// </summary>
		public List<DataSetReferral> Referrals { get; set; } = new();

		/// <summary>
		/// Признак, что прием имеет стандарт оказания помощи
		/// </summary>
		public bool TreatmentHasStandards { get; set; }

		internal ReferralsStatusType StatusType { get; set; }

		/// <summary>
		/// Статус назначений в лечении
		/// </summary>
		public string ReferralStatus {
			get {
				switch (StatusType) {
					case ReferralsStatusType.TreatmentHasNoStandards:
						return "Для данного диагноза нет стандартов";

					case ReferralsStatusType.TreatmentHasNoReferrals:
						return "В приеме нет назначений";

					case ReferralsStatusType.AllInStandards:
						return "Все назначения соответствуют стандартам";

					case ReferralsStatusType.PartlyInStandards:
						return "Назначения частично соответствуют стандартам";

					case ReferralsStatusType.NoneInStandards:
						return "Все назначения не соответствуют стандартам";

					default:
						return "Статус неизвестен";
				}
			}
		}

		/// <summary>
		/// Дата и время загрузки данных в БД
		/// </summary>
		public DateTime CreateDateTime { get; set; }
	}
}
