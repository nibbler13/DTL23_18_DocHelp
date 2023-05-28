using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;
using DTL23_API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение данных для дашборда
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetDashboardDataController : ControllerBase {
		/// <summary>
		/// Получение данных для дашборда из БД
		/// </summary>
		/// <param name="dateBegin">Дата начала периода выборки данных. Формат мм.дд.гггг или гггг-мм-дд</param>
		/// <param name="dateEnd">Дата окончания периода выборки данных. Формат мм.дд.гггг или гггг-мм-дд</param>
		/// <param name="mkbCode">Код МКБ-10 (опционально)</param>
		/// <param name="doctorPostion">Должность (опционально)</param>
		/// <returns>Объект содержащий информацию для дашборда</returns>
		/// <exception cref="ArgumentException">mkbCode должен соответствовать одному 
		/// из кодов справочника МКБ-10</exception>
		[HttpGet]
		public async Task<ActionResult<DashboardData>> Index(DateTime dateBegin,
													   DateTime dateEnd,
													   string? doctorPostion,
													   string? mkbCode) {
			if (!string.IsNullOrEmpty(mkbCode) && !MKB10.IsMkb10StringCorrect(mkbCode))
				throw new ArgumentException("МКБ-10 код не соответствует формату");

			DashboardData dashboardData = null;
			await Task.Run(() => { 
				dashboardData = GetDashboardData.GetData(
					dateBegin, dateEnd, doctorPostion, mkbCode); 
			});
			return dashboardData;
		}
	}
}
